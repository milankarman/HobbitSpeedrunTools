using Memory;

namespace HobbitSpeedrunTools
{
    public class RenderLoadTriggers : ToggleCheat
    {
        public override string Name { get; set; } = "Render Load Triggers";
        public override string ShortName { get; set; } = "LTRIG";
        public override string ShortcutName { get; set; } = "render_load_triggers";
        public override string ToolTip { get; set; } = "Renders bounding boxes for triggers that will load and unload parts of a level.";

        public RenderLoadTriggers(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            mem?.WriteMemory(MemoryAddresses.loadTriggers, "int", Enabled ? "1" : "0");
        }
    }
}
