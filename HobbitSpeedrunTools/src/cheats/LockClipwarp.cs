using Memory;

namespace HobbitSpeedrunTools.cheats
{

    public class LockClipwarp : ToggleCheat
    {
        private float savedWarpPosX;
        private float savedWarpPosY;
        private float savedWarpPosZ;

        public override void OnEnable(Mem mem)
        {
            savedWarpPosX = mem.ReadFloat(MemoryAddresses.warpCoordsX);
            savedWarpPosY = mem.ReadFloat(MemoryAddresses.warpCoordsY);
            savedWarpPosZ = mem.ReadFloat(MemoryAddresses.warpCoordsZ);

            base.OnEnable(mem);
        }

        public override void OnTick(Mem mem)
        {
            if (enabled)
            {
                mem.WriteMemory(MemoryAddresses.loadTriggers, "int", enabled ? "1" : "0");
                mem.WriteMemory(MemoryAddresses.warpCoordsX, "float", savedWarpPosX.ToString());
                mem.WriteMemory(MemoryAddresses.warpCoordsY, "float", savedWarpPosY.ToString());
                mem.WriteMemory(MemoryAddresses.warpCoordsZ, "float", savedWarpPosZ.ToString());
            }
        }
    }
}
