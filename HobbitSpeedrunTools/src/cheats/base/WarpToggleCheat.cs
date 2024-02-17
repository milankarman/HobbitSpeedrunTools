using System.Numerics;

namespace HobbitSpeedrunTools
{
    public abstract class WarpToggleCheat : ToggleCheat
    {
        public Vector3 SavedWarpPos { get; private set; }

        public override void OnEnable()
        {
            if (!CheatManager.IsHooked || mem == null) return;

            float x = mem.ReadFloat(MemoryAddresses.warpCoordsX);
            float y = mem.ReadFloat(MemoryAddresses.warpCoordsY);
            float z = mem.ReadFloat(MemoryAddresses.warpCoordsZ);

            SavedWarpPos = new(x, y, z);

            base.OnEnable();
        }

        public virtual void SetWarpPosition(Vector3 position)
        {
            SavedWarpPos = position;
        }
    }
}
