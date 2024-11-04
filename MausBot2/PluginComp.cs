using PluginSdk;

namespace MausBot2
{
    internal class PluginComp : IComparer<IPlugin>
    {
        public int Compare(IPlugin? x, IPlugin? y)
        {
            // 如果两个插件都为null，它们相等
            if (x == null && y == null) return 0;
            // 如果x为null，它小于y
            if (x == null) return -1;
            // 如果y为null，它小于x
            if (y == null) return 1;
            // 比较两个插件的优先级
            return x.Priority.CompareTo(y.Priority);
        }
    }
}
