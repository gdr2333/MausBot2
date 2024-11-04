using EleCho.GoCqHttpSdk;
using EleCho.GoCqHttpSdk.Post;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace PluginSdk
{
    /// <summary>
    /// 定义插件操作所需的权限级别。
    /// </summary>
    public enum Permission
    {
        /// <summary>
        /// 仅允许相同的管理员操作。
        /// </summary>
        Admin,

        /// <summary>
        /// 仅允许相同的用户操作。
        /// </summary>
        SameUser,

        /// <summary>
        /// 仅允许相同的群聊成员操作。
        /// </summary>
        SameGroup
    };

    /// <summary>
    /// 插件接口定义了插件的基本属性和行为。
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 获取插件的名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 获取插件的描述信息。
        /// 注：当前描述信息未使用。
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 获取插件所需的权限级别。
        /// </summary>
        public Permission Permission { get; }

        /// <summary>
        /// 配置插件的日志记录器。
        /// </summary>
        /// <param name="loggerFactory">日志记录器工厂实例。</param>
        public void ConfigLogger(ILoggerFactory loggerFactory);

        /// <summary>
        /// 配置插件的存储。
        /// 注意：除了sharedStorage["AdminList"]的数据外，其余数据类型为JsonElement。
        /// </summary>
        /// <param name="sharedStorage">共享存储池。</param>
        /// <param name="localStorage">插件独占存储池。</param>
        public void ConfigStorage(ConcurrentDictionary<string, object> sharedStorage, ConcurrentDictionary<string, object> localStorage);

        /// <summary>
        /// 配置插件当前会话信息。
        /// </summary>
        /// <param name="session">当前会话实例。</param>
        public void ConfigSession(ICqActionSession session);

        /// <summary>
        /// 执行插件的通用配置。
        /// </summary>
        public void Config();

        /// <summary>
        /// 获取用于检测是否开始处理任务的正则表达式。
        /// </summary>
        public Regex CheckStartHandle { get; }

        /// <summary>
        /// 获取用于检测是否继续处理任务的正则表达式。
        /// </summary>
        public Regex CheckStillHandle { get; }

        /// <summary>
        /// 处理任务的方法。
        /// </summary>
        /// <param name="fakeConsole">任务的虚拟终端实例。</param>
        /// <param name="startMessage">任务的启动消息。</param>
        /// <returns>表示任务执行的可等待任务。</returns>
        public Task Handler(IFakeConsole fakeConsole, CqMessagePostContext startMessage);

        /// <summary>
        /// 获取插件的优先级。
        /// </summary>
        public int Priority { get; }
    }
}
