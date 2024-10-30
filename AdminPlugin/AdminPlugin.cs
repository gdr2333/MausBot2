using EleCho.GoCqHttpSdk;
using EleCho.GoCqHttpSdk.Post;
using Microsoft.Extensions.Logging;
using PluginSdk;
using System.Collections.Concurrent;

namespace AdminPlugin
{
    public class AdminPlugin : IPlugin
    {
        public string Name => "管理员插件";

        public string Description => "添加管理员";

        public string CheckStartHandle => "^#成为管理$";

        public string CheckStillHandle => "^.*$";

        public Permission Permission => Permission.SameUser;

        ILogger logger;

        ConcurrentDictionary<string, object> sharedStorage;

        public void Config()
        {
        }

        public void ConfigLogger(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<AdminPlugin>();
        }

        public void ConfigSession(ICqActionSession session)
        {
        }

        public void ConfigStorage(ConcurrentDictionary<string, object> sharedStorage, ConcurrentDictionary<string, object> localStorage)
        {
            if (!sharedStorage.ContainsKey("AdminList"))
                sharedStorage.TryAdd("AdminList", Array.Empty<long>());
            this.sharedStorage = sharedStorage;
        }

        public async Task Handler(IFakeConsole fakeConsole, CqMessagePostContext startMessage)
        {
            if(((long[])sharedStorage["AdminList"]).Contains(startMessage.UserId))
            {
                fakeConsole.WriteLine("您已经是管理员了！");
                return;
            }
            var capcha = Convert.ToBase64String(BitConverter.GetBytes(Random.Shared.NextInt64()));
            logger.LogInformation($"验证码是：{capcha}");
            fakeConsole.WriteLine("请输入验证码：");
            var capchaInput = await fakeConsole.ReadLine();
            if(capchaInput == capcha)
            {
                var adminList = (long[])sharedStorage["AdminList"];
                var newAdminList = new long[adminList.Length + 1];
                Array.Copy(adminList, newAdminList, adminList.Length);
                newAdminList[adminList.Length] = startMessage.UserId;
                fakeConsole.WriteLine("已添加到管理员列表！");
            }
            else
            {
                fakeConsole.WriteLine("验证码错误！");
            }
        }
    }
}
