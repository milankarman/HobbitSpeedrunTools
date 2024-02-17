using Memory;

namespace HobbitSpeedrunTools
{
    public class InfiniteRocks : ToggleCheat
    {
        public override string Name { get; set; } = "Infinite Rocks";
        public override string ShortName { get; set; } = "IR";
        public override string ShortcutName { get; set; } = "infinite_rocks";
        public override string ToolTip { get; set; } = "Ensures you will always have the maximum amount of throwing rocks.";

        public InfiniteRocks(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            if (Enabled)
            {
                mem?.WriteMemory(MemoryAddresses.rocks, "float", "10");
            }
        }
    }
}
