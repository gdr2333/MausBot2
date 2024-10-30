using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MausBot2;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
Console.WriteLine("MausBot by df1050 - Version 2");
builder.Services.AddHostedService<MausBot2Service>();
using IHost host = builder.Build();
Console.WriteLine("Start Host......");
await host.RunAsync();