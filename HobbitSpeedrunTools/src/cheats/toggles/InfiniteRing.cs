using Memory;

namespace HobbitSpeedrunTools
{
    public class InfiniteRing : ToggleCheat
    {
        public override string Name { get; set; } = "Infinite Ring";
        public override string ShortName { get; set; } = "RING";
        public override string ShortcutName { get; set; } = "infinite_ring";

        public InfiniteRing(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            if (Enabled)
            {
                mem?.WriteMemory(MemoryAddresses.ringMeter, "float", "100");
            }
        }
    }
}
