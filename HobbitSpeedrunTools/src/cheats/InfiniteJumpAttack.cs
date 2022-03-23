using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public class InfiniteJumpAttack : ToggleCheat
    {
        public override void OnTick(Mem mem)
        {
            if (enabled)
            {
                mem.WriteMemory(MemoryAddresses.stamina, "int", "10");
            }
        }
    }
}
