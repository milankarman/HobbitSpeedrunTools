using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public abstract class ToggleCheat
    {
        public int hotkey;

        public bool enabled;

        public virtual void OnEnable(Mem mem)
        {
            enabled = true;
        }

        public virtual void OnDisable(Mem mem)
        {
            enabled = false;
        }

        public virtual void OnTick(Mem mem) { }
    }
}
