using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public class Invincibility : ToggleCheat
    {
        public new readonly string shortName = "INV";
        public new readonly string shortcutName = "invincibility";

        public Invincibility(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            mem?.WriteMemory(MemoryAddresses.invincibility, "int", enabled ? "1" : "0");
        }
    }
}
