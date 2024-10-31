using EleCho.GoCqHttpSdk;
using EleCho.GoCqHttpSdk.Post;
using Microsoft.Extensions.Logging;
using PluginSdk;
using System.Collections.Concurrent;

namespace TestPlugin
{
    public class Echo : IPlugin
    {
        public string Name => "Echo";

        public string Description => "Echo";

        public Permission Permission => Permission.SameUser;

        public string CheckStartHandle => "^#echo$";

        public string CheckStillHandle => "";

        public void Config()
        {
        }

        public void ConfigLogger(ILoggerFactory loggerFactory)
        {
        }

        public void ConfigSession(ICqActionSession session)
        {
        }

        public void ConfigStorage(ConcurrentDictionary<string, object> sharedStorage, ConcurrentDictionary<string, object> localStorage)
        {
        }

        public async Task Handler(IFakeConsole fakeConsole, CqMessagePostContext startMessage)
        {
            while(true)
            {
                var i = await fakeConsole.ReadLine();
                if (i == "#exit")
                    return;
                fakeConsole.WriteLine(i);
            }
        }
    }
}
