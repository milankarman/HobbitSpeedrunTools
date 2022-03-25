using Memory;

namespace HobbitSpeedrunTools
{
    public class InstantClipwarp : ActionCheat
    {
        public override string Name { get; set; } = "Level Reload";
        public override string ShortcutName { get; set; } = "instant_clipwarp";

        public InstantClipwarp(Mem _mem)
        {
            mem = _mem;
        }

        public override void Start()
        {
            if (mem == null) return;

            float x = mem.ReadFloat(MemoryAddresses.warpCoordsX);
            float y = mem.ReadFloat(MemoryAddresses.warpCoordsY);
            float z = mem.ReadFloat(MemoryAddresses.warpCoordsZ);

            mem.WriteMemory(MemoryAddresses.bilboCoordsX, "float", x.ToString());
            mem.WriteMemory(MemoryAddresses.bilboCoordsY, "float", y.ToString());
            mem.WriteMemory(MemoryAddresses.bilboCoordsZ, "float", z.ToString());
        }
    }
}
