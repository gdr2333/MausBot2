﻿using EleCho.GoCqHttpSdk;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MausBot2;

public sealed class MausBot2Service : IHostedService, IHostedLifecycleService
{
    private readonly ILogger _logger;

    private ILoggerFactory _loggerFactory;

    private CqWsSession _session;

    private Config _config;

    public MausBot2Service(
        ILogger<MausBot2Service> logger,
        IHostApplicationLifetime appLifetime)
    {
        _logger = logger;

        appLifetime.ApplicationStarted.Register(OnStarted);
        appLifetime.ApplicationStopping.Register(OnStopping);
        appLifetime.ApplicationStopped.Register(OnStopped);
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
        _config = (Config)JsonSerializer.Deserialize(await File.ReadAllTextAsync("config.json"), typeof(Config));
        _logger.LogInformation("初始化连接......");
        _session = new CqWsSession(new CqWsSessionOptions()
        {
            BaseUri = _config.Uri,  // WebSocket 地址
        });
        _session.Start();
        _logger.LogInformation("连接成功");
        _logger.LogInformation("开始加载指令");

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
        await File.WriteAllTextAsync("config.json", JsonSerializer.Serialize(_config, new JsonSerializerOptions { WriteIndented = true }));
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
}