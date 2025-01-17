﻿using EleCho.GoCqHttpSdk;
using EleCho.GoCqHttpSdk.Post;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PluginSdk;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MausBot2;

#pragma warning disable CA2254

public sealed partial class MausBot2Service : IHostedService, IHostedLifecycleService
{
    private readonly ILogger _logger;

    private ILoggerFactory _loggerFactory;

    private CqWsSession _session;

    private Config _config;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    private readonly List<IPlugin> _plugins;

    private readonly List<FakeConsole> _fakeConsoles = [];

    public MausBot2Service(
        ILogger<MausBot2Service> logger,
        IHostApplicationLifetime appLifetime)
    {
        _logger = logger;

        appLifetime.ApplicationStarted.Register(OnStarted);
        appLifetime.ApplicationStopping.Register(OnStopping);
        appLifetime.ApplicationStopped.Register(OnStopped);

        _plugins = [new AdminPlugin.AdminPlugin()];
    }

    Task IHostedLifecycleService.StartingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("1. StartingAsync has been called.");

        return Task.CompletedTask;
    }

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("2. StartAsync has been called.");
        _loggerFactory =
            LoggerFactory.Create(builder =>
            builder.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
                options.SingleLine = true;
                options.TimestampFormat = "HH:mm:ss ";
            }));
        _logger.LogInformation("日志已启动。");
        _logger.LogInformation("开始加载数据");
        _config = (Config)JsonSerializer.Deserialize(await File.ReadAllTextAsync("config.json", cancellationToken), typeof(Config));
        _config.sharedStorage["AdminList"] = Array.ConvertAll(((JsonElement)_config.sharedStorage["AdminList"]).EnumerateArray().ToArray(), n => n.GetInt64());
        await LinkToServer();
        LoadPlugin();
        _ = _session.PostPipeline.Use(async (context, next) =>
        {
            if (context is CqHeartbeatPostContext)
            {
                await next();
                return;
            }
            _ = Task.Run(async () =>
            {
                _fakeConsoles.RemoveAll((fakeConsole) => fakeConsole.Closed);
                _logger.LogInformation($"收到消息：{context}");
                if (context is CqGroupMessagePostContext cqGroupMessagePostContext)
                {
                    foreach (var fakeConsole in _fakeConsoles)
                        switch (fakeConsole.Permission)
                        {
                            case Permission.Admin:
                            case Permission.SameUser:
                                if (fakeConsole.Gid == cqGroupMessagePostContext.GroupId && fakeConsole.Uid == cqGroupMessagePostContext.UserId && fakeConsole.StillHandle(cqGroupMessagePostContext.Message.Text))
                                {
                                    _logger.LogInformation($"发送消息{cqGroupMessagePostContext}到{fakeConsole.Name}");
                                    fakeConsole.SendMessageToStream(cqGroupMessagePostContext.Message.Text);
                                    goto end;
                                }
                                break;
                            case Permission.SameGroup:
                                if (fakeConsole.Gid == cqGroupMessagePostContext.GroupId && fakeConsole.StillHandle(cqGroupMessagePostContext.Message.Text))
                                {
                                    _logger.LogInformation($"发送消息{cqGroupMessagePostContext}到{fakeConsole.Name}");
                                    fakeConsole.SendMessageToStream(cqGroupMessagePostContext.Message.Text);
                                    goto end;
                                }
                                break;
                        }
                    foreach (var plugin in _plugins)
                        if (plugin.CheckStartHandle.IsMatch(cqGroupMessagePostContext.Message.Text))
                        {
                            _logger.LogInformation($"启动{plugin.Name}");
                            _logger.LogInformation($"发送启动消息{cqGroupMessagePostContext}到{plugin.Name}");
                            var fakeConsole = new FakeConsole((message) => { _logger.LogInformation($"发送消息{message}到{cqGroupMessagePostContext.GroupId}"); _session.SendGroupMessage(cqGroupMessagePostContext.GroupId, message); }, plugin.Permission, cqGroupMessagePostContext.GroupId, cqGroupMessagePostContext.UserId, plugin.CheckStillHandle.IsMatch, plugin.Name);
                            _fakeConsoles.Add(fakeConsole);
                            await plugin.Handler(fakeConsole, cqGroupMessagePostContext);
                            fakeConsole.Close();
                            goto end;
                        }
                }
                else if (context is CqPrivateMessagePostContext cqPrivateMessagePostContext)
                {
                    foreach (var fakeConsole in _fakeConsoles)
                        switch (fakeConsole.Permission)
                        {
                            case Permission.Admin:
                            case Permission.SameUser:
                                if (fakeConsole.Gid == cqPrivateMessagePostContext.UserId && fakeConsole.Uid == cqPrivateMessagePostContext.UserId && fakeConsole.StillHandle(cqPrivateMessagePostContext.Message.Text))
                                {
                                    _logger.LogInformation($"发送消息{cqPrivateMessagePostContext}到{fakeConsole.Name}");
                                    fakeConsole.SendMessageToStream(cqPrivateMessagePostContext.Message.Text);
                                    goto end;
                                }
                                break;
                            case Permission.SameGroup:
                                if (fakeConsole.Gid == cqPrivateMessagePostContext.UserId && fakeConsole.StillHandle(cqPrivateMessagePostContext.Message.Text))
                                {
                                    _logger.LogInformation($"发送消息{cqPrivateMessagePostContext}到{fakeConsole.Name}");
                                    fakeConsole.SendMessageToStream(cqPrivateMessagePostContext.Message.Text);
                                    goto end;
                                }
                                break;
                        }
                    foreach (var plugin in _plugins)
                        if (plugin.CheckStartHandle.IsMatch(cqPrivateMessagePostContext.Message.Text))
                        {
                            _logger.LogInformation($"启动{plugin.Name}");
                            _logger.LogInformation($"发送启动消息{cqPrivateMessagePostContext}到{plugin.Name}");
                            var fakeConsole = new FakeConsole((message) => { _logger.LogInformation($"发送消息{message}到{cqPrivateMessagePostContext.UserId}"); _session.SendGroupMessage(cqPrivateMessagePostContext.UserId, message); }, plugin.Permission, cqPrivateMessagePostContext.UserId, cqPrivateMessagePostContext.UserId, plugin.CheckStillHandle.IsMatch, plugin.Name);
                            _fakeConsoles.Add(fakeConsole);
                            await plugin.Handler(fakeConsole, cqPrivateMessagePostContext);
                            fakeConsole.Close();
                            goto end;
                        }
                }
            end:;
            });
            await next();
        });
        return;
    }

    Task IHostedLifecycleService.StartedAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("3. StartedAsync has been called.");

        return Task.CompletedTask;
    }

    private void OnStarted()
    {
        _logger.LogInformation("4. OnStarted has been called.");
    }

    private void OnStopping()
    {
        _logger.LogInformation("5. OnStopping has been called.");
    }

    async Task IHostedLifecycleService.StoppingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("6. StoppingAsync has been called.");
        _logger.LogInformation("开始保存数据......");
        await File.WriteAllTextAsync("config.json", JsonSerializer.Serialize(_config, _jsonSerializerOptions), cancellationToken);
        _logger.LogInformation("数据保存完成");
        return;
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("7. StopAsync has been called.");

        return Task.CompletedTask;
    }

    Task IHostedLifecycleService.StoppedAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("8. StoppedAsync has been called.");

        return Task.CompletedTask;
    }

    private void OnStopped()
    {
        _logger.LogInformation("9. OnStopped has been called.");
    }

    private void LoadPlugin()
    {
        _logger.LogInformation("开始加载指令");
        var folder = new DirectoryInfo($"{Environment.CurrentDirectory}/plugins/");
        foreach (var i in folder.GetDirectories())
        {
            foreach (var j in i.GetFiles())
            {
                if (DllRegex().Match(j.Name).Success)
                {
                    Assembly asm;
                    try
                    {
                        asm = Assembly.LoadFile(j.FullName);
                    }
                    catch (Exception e)
                    {
                        _logger.LogWarning(e.ToString());
                        continue;
                    }
                    foreach (var type in asm.GetTypes())
                        if (type.IsPublic && type.GetInterface("IPlugin") != null)
                            _plugins.Add((IPlugin)Activator.CreateInstance(type));
                }
            }
        }
        _plugins.Sort(new PluginComp());
        _plugins.Reverse();
        foreach (var plugin in _plugins)
        {
            _logger.LogInformation($"指令{plugin.Name}加载完成");
            plugin.ConfigLogger(_loggerFactory);
            plugin.ConfigSession(_session);
            if (!_config.localStorage.ContainsKey(plugin.Name))
                _config.localStorage.TryAdd(plugin.Name, new());
            plugin.ConfigStorage(_config.sharedStorage, _config.localStorage[plugin.Name]);
            plugin.Config();
            _logger.LogInformation($"指令{plugin.Name}初始化完成");
        }
    }

    private async Task LinkToServer()
    {
        _logger.LogInformation("初始化连接......");
        _session = new CqWsSession(new CqWsSessionOptions()
        {
            BaseUri = _config.Uri,  // WebSocket 地址
        });
        await _session.StartAsync();
        _logger.LogInformation("连接成功");
    }

    [GeneratedRegex("\\.dll$")]
    private static partial Regex DllRegex();
}