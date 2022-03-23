using Memory;

namespace HobbitSpeedrunTools.Cheats
{
    public class DevMode : ToggleCheat
    {
        public override CHEAT_ID ID { get; set; } = CHEAT_ID.DEV_MODE;
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
