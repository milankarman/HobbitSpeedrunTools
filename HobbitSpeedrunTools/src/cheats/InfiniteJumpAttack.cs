using Memory;

namespace HobbitSpeedrunTools
{
    public class InfiniteJumpAttack : ToggleCheat
    {
        public override CHEAT_ID ID { get; set; } = CHEAT_ID.INFINITE_JUMP_ATTACKS;
        public override string ShortName { get; set; } = "IJA";
        public override string ShortcutName { get; set; } = "infinite_jump_attack";

        public InfiniteJumpAttack(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            if (Enabled)
            {
                mem?.WriteMemory(MemoryAddresses.stamina, "int", "10");
            }
        }
    }
}
