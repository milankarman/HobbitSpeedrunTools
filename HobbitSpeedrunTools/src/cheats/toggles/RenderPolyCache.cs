using Memory;

namespace HobbitSpeedrunTools
{
    public class RenderPolyCache : ToggleCheat
    {
        public override string Name { get; set; } = "Render Polycache";
        public override string ShortName { get; set; } = "POLY";
        public override string ShortcutName { get; set; } = "render_poly_cache";
        public override string ToolTip { get; set; } = "Gives polygons near Bilbo flat colors to be able to observe the geometry.";

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
