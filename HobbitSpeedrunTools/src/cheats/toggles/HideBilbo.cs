using Memory;

namespace HobbitSpeedrunTools
{
    public class HideBilbo : ToggleCheat
    {
        public override string Name { get; set; } = "Hide Bilbo";
        public override string ShortName { get; set; } = "HIDE";
        public override string ShortcutName { get; set; } = "hide_bilbo";
        public override string ToolTip { get; set; } = "Stops Bilbo from rendering";

        public HideBilbo(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            mem?.WriteMemory(MemoryAddresses.renderBilbo, "int", Enabled ? "0" : "1");
        }
    }
}
