using Memory;
using System.Threading;

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
            if (!CheatManager.isHooked || mem == null) return;

            mem.WriteMemory(MemoryAddresses.bilboStateTimer, "float", "10");
            mem.WriteMemory(MemoryAddresses.bilboCoordsY, "float", "100000");
            mem.WriteMemory(MemoryAddresses.bilboNewCoordsY, "float", "100000");
        }
    }
}
