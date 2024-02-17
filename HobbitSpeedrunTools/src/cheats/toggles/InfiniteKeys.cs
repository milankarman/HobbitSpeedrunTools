using Memory;

namespace HobbitSpeedrunTools
{
    public class InfiniteKeys : ToggleCheat
    {
        public override string Name { get; set; } = "Infinite Keys";
        public override string ShortName { get; set; } = "IK";
        public override string ShortcutName { get; set; } = "infinite_keys";
        public override string ToolTip { get; set; } = "Ensures you always have the maximum amount of skeleton keys.";

        public InfiniteKeys(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            if (Enabled)
            {
                mem?.WriteMemory(MemoryAddresses.keys, "float", "99");
            }
        }
    }
}
