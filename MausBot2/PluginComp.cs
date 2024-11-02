using PluginSdk;

namespace MausBot2
{
    internal class PluginComp : IComparer<IPlugin>
    {
        public int Compare(IPlugin? x, IPlugin? y)
        {
            if (x == null)
                if (y == null)
                    return 0;
                else
                    return -1;
            else if (y == null)
                return 1;
            else
                return x.Priority.CompareTo(y.Priority);
        }
    }
}
