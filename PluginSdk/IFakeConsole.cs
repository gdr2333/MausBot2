using EleCho.GoCqHttpSdk.Message;

/// <summary>
/// 表示用于输入和输出操作的虚拟控制台。
/// </summary>
public interface IFakeConsole
{
    /// <summary>
    /// 清除虚拟控制台的内容。
    /// </summary>
    public void Clear();

    /// <summary>
    /// 关闭虚拟控制台，释放所有资源。
    /// </summary>
    public void Close();

    /// <summary>
    /// 异步地从虚拟控制台读取一行输入。
    /// </summary>
    /// <returns>表示异步读取操作的任务。结果为输入的一行文本。</returns>
    public Task<string> ReadLine();

    /// <summary>
    /// 获取虚拟控制台当前行数。
    /// </summary>
    public int Lines { get; }

    /// <summary>
    /// 将布尔值写入虚拟控制台。
    /// </summary>
    /// <param name="value">要写入的布尔值。</param>
    public void Write(bool value);

    /// <summary>
    /// 将字符写入虚拟控制台。
    /// </summary>
    /// <param name="value">要写入的字符。</param>
    public void Write(char value);

    /// <summary>
    /// 将字符数组写入虚拟控制台。
    /// </summary>
    /// <param name="value">包含要写入的字符的数组。</param>
    public void Write(char[] value);

    /// <summary>
    /// 将字符数组的一部分写入虚拟控制台。
    /// </summary>
    /// <param name="value">包含要写入的字符的数组。</param>
    /// <param name="startIndex">子数组的起始索引。</param>
    /// <param name="charCount">要写入的字符数。</param>
    public void Write(char[] value, int startIndex, int charCount);

    /// <summary>
    /// 将十进制数值写入虚拟控制台。
    /// </summary>
    /// <param name="value">要写入的十进制数值。</param>
    public void Write(decimal value);

    /// <summary>
    /// 将双精度浮点数写入虚拟控制台。
    /// </summary>
    /// <param name="value">要写入的双精度浮点数。</param>
    public void Write(double value);

    /// <summary>
    /// 将整数写入虚拟控制台。
    /// </summary>
    /// <param name="value">要写入的整数。</param>
    public void Write(int value);

    /// <summary>
    /// 将长整数写入虚拟控制台。
    /// </summary>
    /// <param name="value">要写入的长整数。</param>
    public void Write(long value);

    /// <summary>
    /// 将对象转换为字符串后写入虚拟控制台。
    /// </summary>
    /// <param name="value">要写入的对象。</param>
    public void Write(object value);

    /// <summary>
    /// 将单精度浮点数写入虚拟控制台。
    /// </summary>
    /// <param name="value">要写入的单精度浮点数。</param>
    public void Write(float value);

    /// <summary>
    /// 将字符串写入虚拟控制台。
    /// </summary>
    /// <param name="value">要写入的字符串。</param>
    public void Write(string value);

    /// <summary>
    /// 将无符号整数写入虚拟控制台。
    /// </summary>
    /// <param name="value">要写入的无符号整数。</param>
    public void Write(uint value);

    /// <summary>
    /// 将无符号长整数写入虚拟控制台。
    /// </summary>
    /// <param name="value">要写入的无符号长整数。</param>
    public void Write(ulong value);

    /// <summary>
    /// 将新行符写入虚拟控制台。
    /// </summary>
    public void WriteLine();

    /// <summary>
    /// 将字符串和一个新行符写入虚拟控制台。
    /// </summary>
    /// <param name="value">要写入的字符串。</param>
    public void WriteLine(string value);

    /// <summary>
    /// 发送指定的消息到虚拟控制台输出。
    /// </summary>
    /// <param name="message">要发送的消息。</param>
    public void WriteMessage(CqMessage message);
}
