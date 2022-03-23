using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public abstract class ToggleCheat
    {
        public Mem? mem;

        public string? shortName;
        public string? shortcutName;
        public int hotkey;
        public bool enabled;

        public virtual void OnEnable()
        {
            enabled = true;
        }

        public virtual void OnDisable()
        {
            enabled = false;
        }

        public virtual void OnTick() { }
    }
}
