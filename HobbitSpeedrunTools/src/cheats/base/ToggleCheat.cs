using Memory;
using System;

namespace HobbitSpeedrunTools
{
    public abstract class ToggleCheat
    {
        public Mem? mem;

        public abstract string Name { get; set; }
        public abstract string ShortName { get; set; }
        public abstract string ShortcutName { get; set; }
        public abstract string ToolTip { get; set; }

        public Action? onEnable;
        public Action? onDisable;

        public bool Enabled { get; set; }

        public virtual void Enable()
        {
            Enabled = true;
            onEnable?.Invoke();
            OnEnable();
        }

        public virtual void Disable()
        {
            Enabled = false;
            onDisable?.Invoke();
            OnDisable();
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

        public virtual void SetShortcut(string shortcut)
        {
            ToolTip += "\n\n" + "Hotkey: " + shortcut;
        }

        public virtual void OnEnable() { }

        public virtual void OnDisable() { }

        public virtual void OnTick() { }
    }
}
