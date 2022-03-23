using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public class RenderPolyCache : ToggleCheat
    {
        public override void OnTick(Mem mem)
        {
            mem.WriteMemory(MemoryAddresses.polyCache, "int", enabled ? "1" : "0");
        }
    }
}
