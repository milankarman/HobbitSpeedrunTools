using Memory;

namespace HobbitSpeedrunTools.Cheats
{
    public class RenderLoadTriggers : ToggleCheat
    {
        public override CHEAT_ID ID { get; set; } = CHEAT_ID.RENDER_LOAD_TRIGGERS;
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
