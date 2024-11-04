using EleCho.GoCqHttpSdk.Message;

namespace PluginSdk
{
    /// <summary>
    /// 虚拟终端类
    /// </summary>
    public interface IFakeConsole
    {
        /// <summary>
        /// 清空虚拟终端
        /// </summary>
        public void Clear();

        /// <summary>
        /// 关闭虚拟终端
        /// </summary>
        public void Close();

        /// <summary>
        /// 读取一行消息（异步）
        /// </summary>
        /// <returns>读取的消息</returns>
        public Task<string> ReadLine();

        /// <summary>
        /// 获取当前行数
        /// </summary>
        public int Lines { get; }

        public void Write(bool sth);

        public void Write(char sth);

        public void Write(char[] sth);

        public void Write(char[] a, int b, int c);

        public void Write(decimal sth);

        public void Write(double sth);

        public void Write(int sth);

        public void Write(long sth);

        public void Write(object sth);

        public void Write(float sth);

        public void Write(string sth);

        public void Write(uint sth);

        public void Write(ulong sth);

        public void WriteLine();

        public void WriteLine(string sth);

        public void WriteMessage(CqMessage message);
    }
}
