using Memory;

namespace HobbitSpeedrunTools
{
    public class Invincibility : ToggleCheat
    {
        public override string Name { get; set; } = "Invincibility";
        public override string ShortName { get; set; } = "INV";
        public override string ShortcutName { get; set; } = "invincibility";
        public override string ToolTip { get; set; } = "Makes Bilbo immune to enemy and fall damage.";

        public Invincibility(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            mem?.WriteMemory(MemoryAddresses.invincibility, "int", Enabled ? "1" : "0");
        }
    }
}
