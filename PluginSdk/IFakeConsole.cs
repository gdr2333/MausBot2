using EleCho.GoCqHttpSdk.Message;

namespace PluginSdk
{
    public interface IFakeConsole
    {
        public void Clear();

        public void Close();

        public Task<string> ReadLine();

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
