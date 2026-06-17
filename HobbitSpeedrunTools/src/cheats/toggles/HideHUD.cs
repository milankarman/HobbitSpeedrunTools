using Memory;

namespace HobbitSpeedrunTools
{
    public class HideHUD : ToggleCheat
    {
        public override string Name { get; set; } = "Hide HUD";
        public override string ShortName { get; set; } = "HUD";
        public override string ShortcutName { get; set; } = "hide_hud";
        public override string ToolTip { get; set; } = "Stops HUD from rendering";

        public HideHUD(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            mem?.WriteMemory(MemoryAddresses.renderHUD, "int", Enabled ? "0" : "1");
        }
    }
}
