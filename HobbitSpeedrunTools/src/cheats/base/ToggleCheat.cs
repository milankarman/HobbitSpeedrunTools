using System;
using Memory;

namespace HobbitSpeedrunTools
{
    public abstract class ToggleCheat
    {
        public Mem? mem;

        public abstract TOGGLE_CHEAT_ID ID { get; set; }
        public abstract string Name { get; set; }
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
            Enabled = false;
            onDisable?.Invoke();
        }

        public virtual void SetActive(bool active)
        {
            if (active)
            {
                Enable();
            }
            if (!active)
            {
                Disable();
            }
        }

        public virtual void Toggle()
        {
            if (!Enabled) Enable();
            else Disable();
        }

        public virtual void OnEnable() { }

        public virtual void OnDisable() { }

        public virtual void OnTick() { }
    }
}
