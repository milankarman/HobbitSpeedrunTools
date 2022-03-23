using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public class Invincibility : ToggleCheat
    {
        public override void OnTick(Mem mem)
        {
            mem.WriteMemory(MemoryAddresses.invincibility, "int", enabled ? "1" : "0");
        }
    }
}
