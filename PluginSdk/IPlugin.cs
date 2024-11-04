using EleCho.GoCqHttpSdk;
using EleCho.GoCqHttpSdk.Post;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace PluginSdk
{

    /// <summary>
    /// 插件权限
    /// </summary>
    public enum Permission 
    { 
        /// <summary>
        /// 仅限相同管理员
        /// </summary>
        Admin, 
        /// <summary>
        /// 仅限相同用户
        /// </summary>
        SameUser, 
        /// <summary>
        /// 仅限相同群聊
        /// </summary>
        SameGroup 
    };

    /// <summary>
    /// 插件接口
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 插件描述（当前没用）
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 插件权限
        /// </summary>
        public Permission Permission { get; }

        /// <summary>
        /// 配置日志记录器
        /// </summary>
        /// <param name="loggerFactory">记录器生成程序</param>
        public void ConfigLogger(ILoggerFactory loggerFactory);

        /// <summary>
        /// 配置存储（注意：除了sharedStroage["AdminList"]的数据之外都是JsonElement
        /// </summary>
        /// <param name="sharedStorage">共享存储池</param>
        /// <param name="localStorage">插件独占存储池</param>
        public void ConfigStorage(ConcurrentDictionary<string, object> sharedStorage, ConcurrentDictionary<string, object> localStorage);

        /// <summary>
        /// 配置当前会话
        /// </summary>
        /// <param name="session">当前会话</param>
        public void ConfigSession(ICqActionSession session);

        /// <summary>
        /// 通用配置程序
        /// </summary>
        public void Config();

        /// <summary>
        /// 开始匹配的正则表达式
        /// </summary>
        public Regex CheckStartHandle { get; }

        /// <summary>
        /// 继续匹配的正则表达式
        /// </summary>
        public Regex CheckStillHandle { get; }

        /// <summary>
        /// 任务处理器
        /// </summary>
        /// <param name="fakeConsole">当前任务的虚拟终端</param>
        /// <param name="startMessage">当前任务的启动消息</param>
        /// <returns>可等待任务</returns>
        public Task Handler(IFakeConsole fakeConsole, CqMessagePostContext startMessage);

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; }
    }
}
