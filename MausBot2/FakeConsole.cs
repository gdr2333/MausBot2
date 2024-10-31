using EleCho.GoCqHttpSdk.Message;
using PluginSdk;
using System.Text;
using System.Threading.Channels;

namespace MausBot2
{
    internal class FakeConsole(Action<CqMessage> SendMessage, Permission permission, long Gid, long Uid, Func<string, bool> stillHandle, string name) : IFakeConsole
    {
        private readonly Channel<string> _input = Channel.CreateUnbounded<string>();

        private readonly StringBuilder _outPut = new();

        private bool _HasCleaner = false;

        public Permission Permission { get; private set; } = permission;

        public long Gid { get; private set; } = Gid;

        public long Uid { get; private set; } = Uid;

        public bool Closed { get; private set; }

        public bool StillHandle(string message) => stillHandle(message);

        public string Name { get; private set; } = name;

        private async void Cleaner()
        {
            _HasCleaner = true;
            while (_outPut.Length != 0)
            {
                Clear();
                await Task.Delay(1000);
            }
            _HasCleaner = false;
        }

        private void CheckCleaner()
        {
            if (!_HasCleaner)
                Cleaner();
        }

        public async void SendMessageToStream(string str)
        {
            foreach (var i in str.Split('\n'))
                await _input.Writer.WriteAsync(i);
        }

        public void Clear()
        {
            if (_outPut.Length != 0)
                lock (_outPut)
                {
                    SendMessage(new(_outPut.ToString()));
                    _outPut.Clear();
                }
        }

        public void Close()
        {
            Closed = true;
        }

        public async Task<string> ReadLine() => await _input.Reader.ReadAsync();

        public void Write(bool sth)
        {
            lock (_outPut)
                _outPut.Append(sth);
            CheckCleaner();
        }

        public void Write(char sth)
        {
            lock (_outPut)
                _outPut.Append(sth);
            CheckCleaner();
        }

        public void Write(char[] sth)
        {
            lock (_outPut)
                _outPut.Append(sth);
            CheckCleaner();
        }

        public void Write(char[] a, int b, int c)
        {
            lock (_outPut)
                _outPut.Append(a, b, c);
            CheckCleaner();
        }

        public void Write(decimal sth)
        {
            lock (_outPut)
                _outPut.Append(sth);
            CheckCleaner();
        }

        public void Write(double sth)
        {
            lock (_outPut)
                _outPut.Append(sth);
            CheckCleaner();
        }

        public void Write(int sth)
        {
            lock (_outPut)
                _outPut.Append(sth);
            CheckCleaner();
        }

        public void Write(long sth)
        {
            lock (_outPut)
                _outPut.Append(sth);
            CheckCleaner();
        }

        public void Write(object sth)
        {
            lock (_outPut)
                _outPut.Append(sth);
            CheckCleaner();
        }
        public void Write(float sth)
        {
            lock (_outPut)
                _outPut.Append(sth);
            CheckCleaner();
        }

        public void Write(string sth)
        {
            lock (_outPut)
                _outPut.Append(sth);
            CheckCleaner();
        }

        public void Write(uint sth)
        {
            lock (_outPut)
                _outPut.Append(sth);
            CheckCleaner();
        }

        public void Write(ulong sth)
        {
            lock (_outPut)
                _outPut.Append(sth);
            CheckCleaner();
        }

        public void WriteLine()
        {
            lock (_outPut)
                _outPut.AppendLine();
            CheckCleaner();
        }

        public void WriteLine(string sth)
        {
            lock (_outPut)
                _outPut.AppendLine(sth);
            CheckCleaner();
        }

        public void WriteMessage(CqMessage message) => SendMessage(message);
    }
}
