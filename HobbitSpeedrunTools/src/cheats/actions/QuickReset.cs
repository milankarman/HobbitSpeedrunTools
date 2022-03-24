using Memory;

namespace HobbitSpeedrunTools
{
    public class QuickReset : ActionCheat
    {
        public override ACTION_CHEAT_ID ID { get; set; } = ACTION_CHEAT_ID.QUICK_RESET;
        public override string Name { get; set; } = "Quick Reset";
        public override string ShortcutName { get; set; } = "quick_reset";

        public QuickReset(Mem _mem)
        {
            mem = _mem;
        }

        public override void Start()
        {
            mem?.WriteMemory(MemoryAddresses.stamina, "float", "10");
            mem?.WriteMemory(MemoryAddresses.bilboState, "int", "27");
        }
    }
}
