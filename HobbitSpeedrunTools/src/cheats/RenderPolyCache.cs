using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public class RenderPolyCache : ToggleCheat
    {
        public new readonly string shortName = "POLY";
        public new readonly string shortcutName = "render_polycache";

        public RenderPolyCache(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            mem?.WriteMemory(MemoryAddresses.polyCache, "int", enabled ? "1" : "0");
        }
    }
}
