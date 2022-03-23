using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public class RenderLoadTriggers : ToggleCheat
    {
        public new readonly string shortName = "LTRIG";
        public new readonly string shortcutName = "render_load_triggers";

        public RenderLoadTriggers(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            mem?.WriteMemory(MemoryAddresses.loadTriggers, "int", enabled ? "1" : "0");
        }
    }
}
