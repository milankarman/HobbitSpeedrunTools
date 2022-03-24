using Memory;

namespace HobbitSpeedrunTools
{
    public class RenderPolyCache : ToggleCheat
    {
        public override CHEAT_ID ID { get; set; } = CHEAT_ID.RENDER_POLY_CACHE;
        public override string Name { get; set; } = "Render Polycache";
        public override string ShortName { get; set; } = "POLY";
        public override string ShortcutName { get; set; } = "render_poly_cache";

        public RenderPolyCache(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            mem?.WriteMemory(MemoryAddresses.polyCache, "int", Enabled ? "1" : "0");
        }
    }
}
