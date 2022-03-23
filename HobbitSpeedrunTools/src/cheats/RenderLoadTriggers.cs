using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public class RenderLoadTriggers : ToggleCheat
    {
        public override void OnTick(Mem mem)
        {
            mem.WriteMemory(MemoryAddresses.loadTriggers, "int", enabled ? "1" : "0");
        }
    }
}
