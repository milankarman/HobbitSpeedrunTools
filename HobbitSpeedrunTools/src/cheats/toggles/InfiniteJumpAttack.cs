using Memory;

namespace HobbitSpeedrunTools
{
    public class InfiniteJumpAttack : ToggleCheat
    {
        public override string Name { get; set; } = "Infinite Jump Attack";
        public override string ShortName { get; set; } = "IJA";
        public override string ShortcutName { get; set; } = "infinite_jump_attack";
        public override string ToolTip { get; set; } = "Keeps stamina at maximum to always allow Bilbo to jump attack.";


        public InfiniteJumpAttack(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            if (Enabled)
            {
                mem?.WriteMemory(MemoryAddresses.stamina, "float", "10");
            }
        }
    }
}
