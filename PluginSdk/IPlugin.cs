using EleCho.GoCqHttpSdk;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace PluginSdk
{
    internal interface IPlugin
    {
        public string Name { get; }

        public string Description { get; }

        public void ConfigLogger(ILoggerFactory loggerFactory);

        public void ConfigStorage(ConcurrentDictionary<string, object> sharedStorage, ConcurrentDictionary<string, object> localStorage);

        public void ConfigSession(ICqActionSession session);

        public void Config();

        public string CheckStartHandle { get; }

        public string CheckStillHandle { get; }

        public void Handler(IFakeConsole fakeConsole);
    }
}
