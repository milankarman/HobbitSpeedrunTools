using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public class DevMode : ToggleCheat
    {
        public override void OnTick(Mem mem)
        {
            mem.WriteMemory(MemoryAddresses.devMode, "int", enabled ? "1" : "0");
        }
    }
}
