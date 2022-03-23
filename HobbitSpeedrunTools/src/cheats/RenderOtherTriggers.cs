using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public class RenderOtherTriggers : ToggleCheat
    {
        public override void OnTick(Mem mem)
        {
            mem.WriteMemory(MemoryAddresses.otherTriggers, "int", enabled ? "1" : "0");
        }
    }
}
