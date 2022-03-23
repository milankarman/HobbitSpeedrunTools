using System;
using Memory;

namespace HobbitSpeedrunTools
{
    public abstract class ToggleCheat
    {
        public Mem? mem;

        public abstract CHEAT_ID ID { get; set; }
        public abstract string ShortName { get; set; }
        public abstract string ShortcutName { get; set; }

        public Action? onEnable;
        public Action? onDisable;

        public bool Enabled { get; set; }

        public virtual void Enable()
        {
            Enabled = true;
            onEnable?.Invoke();
        }

        public virtual void Disable()
        {
            Enabled = true;
            onEnable?.Invoke();
        }

        public virtual void OnEnable() { }

        public virtual void OnDisable() { }

        public virtual void OnTick() { }
    }
}
