using Memory;

namespace HobbitSpeedrunTools
{
    public class RenderLoadTriggers : ToggleCheat
    {
        public override TOGGLE_CHEAT_ID ID { get; set; } = TOGGLE_CHEAT_ID.RENDER_LOAD_TRIGGERS;
        public override string Name { get; set; } = "Render Load Triggers";
        public override string ShortName { get; set; } = "LTRIG";
        public override string ShortcutName { get; set; } = "render_load_triggers";

        public RenderLoadTriggers(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            mem?.WriteMemory(MemoryAddresses.loadTriggers, "int", Enabled ? "1" : "0");
        }
    }
}
