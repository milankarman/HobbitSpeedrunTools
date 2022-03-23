using Memory;

namespace HobbitSpeedrunTools.Cheats
{
    public abstract class ToggleCheat
    {
        public Mem? mem;

        public abstract CHEAT_ID ID { get; set; }
        public abstract string ShortName { get; set; }
        public abstract string ShortcutName { get; set; }

        public bool Enabled { get; set; }

        public virtual void OnEnable()
        {
            Enabled = true;
        }

        public virtual void OnDisable()
        {
            Enabled = false;
        }

        public virtual void OnTick() { }
    }
}
