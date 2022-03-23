using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public class RenderOtherTriggers : ToggleCheat
    {
        public new readonly string shortName = "OTRIG";
        public new readonly string shortcutName = "render_other_triggers";

        public RenderOtherTriggers(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            mem?.WriteMemory(MemoryAddresses.otherTriggers, "int", enabled ? "1" : "0");
        }
    }
}
