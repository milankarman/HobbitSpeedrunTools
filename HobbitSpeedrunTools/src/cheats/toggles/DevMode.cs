using Memory;

namespace HobbitSpeedrunTools
{
    public class DevMode : ToggleCheat
    {
        public override TOGGLE_CHEAT_ID ID { get; set; } = TOGGLE_CHEAT_ID.DEV_MODE;
        public override string Name { get; set; } = "Developer Mode";
        public override string ShortName { get; set; } = "DEV";
        public override string ShortcutName { get; set; } = "dev_mode";

        public DevMode(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            mem?.WriteMemory(MemoryAddresses.devMode, "int", Enabled ? "1" : "0");
        }
    }
}
