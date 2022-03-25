using Memory;

namespace HobbitSpeedrunTools
{
    public class RenderOtherTriggers : ToggleCheat
    {
        public override string Name { get; set; } = "Render Other Triggers";
        public override string ShortName { get; set; } = "OTRIG";
        public override string ShortcutName { get; set; } = "render_other_triggers";

        public RenderOtherTriggers(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            mem?.WriteMemory(MemoryAddresses.otherTriggers, "int", Enabled ? "1" : "0");
        }
    }
}
