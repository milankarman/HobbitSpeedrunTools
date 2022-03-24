using Memory;

namespace HobbitSpeedrunTools
{
    public abstract class ActionCheat
    {
        public Mem? mem;

        public abstract string Name { get; set; }
        public abstract string ShortcutName { get; set; }
        public abstract int Index { get; set; }

        public abstract void Start();
    }
}
