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

        /// <see cref="System.Text.StringBuilder.Append(bool)"/>
        public void Write(bool sth);

        /// <see cref="System.Text.StringBuilder.Append(char)"/>
        public void Write(char sth);

        /// <see cref="System.Text.StringBuilder.Append(char[])"/>
        public void Write(char[] sth);

        /// <see cref="System.Text.StringBuilder.Append(char[], int, int)"/>
        public void Write(char[] a, int b, int c);

        /// <see cref="System.Text.StringBuilder.Append(decimal)"/>
        public void Write(decimal sth);

        /// <see cref="System.Text.StringBuilder.Append(double)"/>
        public void Write(double sth);

        /// <see cref="System.Text.StringBuilder.Append(int)"/>
        public void Write(int sth);

        /// <see cref="System.Text.StringBuilder.Append(long)"/>
        public void Write(long sth);

        /// <see cref="System.Text.StringBuilder.Append(object)"/>
        public void Write(object sth);

        /// <see cref="System.Text.StringBuilder.Append(float)"/>
        public void Write(float sth);

        /// <see cref="System.Text.StringBuilder.Append(string)"/>
        public void Write(string sth);

        /// <see cref="System.Text.StringBuilder.Append(uint)"/>
        public void Write(uint sth);

        /// <see cref="System.Text.StringBuilder.Append(ulong)"/>
        public void Write(ulong sth);

        /// <see cref="System.Text.StringBuilder.AppendLine()"/>
        public void WriteLine();

        /// <see cref="System.Text.StringBuilder.AppendLine(string)"/>
        public void WriteLine(string sth);

        /// <summary>
        /// 发送原始消息
        /// </summary>
        /// <param name="message">要发送的消息</param>
        public void WriteMessage(CqMessage message);
    }
}
