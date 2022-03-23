using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public class InfiniteJumpAttack : ToggleCheat
    {
        public new readonly string shortName = "IJA";
        public new readonly string shortcutName = "infinite_jump_attack";

        public InfiniteJumpAttack(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            if (enabled)
            {
                mem?.WriteMemory(MemoryAddresses.stamina, "int", "10");
            }
        }
    }
}
