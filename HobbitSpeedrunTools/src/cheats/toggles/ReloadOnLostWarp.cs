using Memory;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace HobbitSpeedrunTools
{
    public class ReloadOnLostWarp : WarpToggleCheat
    {
        public override string Name { get; set; } = "Reload on Lost Warp";
        public override string ShortName { get; set; } = "RELO";
        public override string ShortcutName { get; set; } = "reload_lost_warp";
        public override string ToolTip { get; set; } = "Reloads your last save when Bilbo's clipwarp position changes from the spot it is at when this cheat is enabled.\n" +
            "When a succesful warp back is performed this cheat will be disabled until Bilbo dies or a save is loaded.";

        private bool waiting;
        private bool succesfullyWarped;

        private Vector3 lastBilboPos;

        public ReloadOnLostWarp(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            if (mem == null || !Enabled) return;

            Vector3 bilboPos = new(mem.ReadFloat(MemoryAddresses.bilboCoordsX), mem.ReadFloat(MemoryAddresses.bilboCoordsY), mem.ReadFloat(MemoryAddresses.bilboCoordsZ));

            // Seeing Bilbo's position jump significantly shows a warp has likely been performed
            bool hasMovedSignificantly = Vector3.Distance(bilboPos, lastBilboPos) > 500 && !Equals(lastBilboPos, Vector3.Zero);

            // Used to check if Bilbo is then at the right destination
            bool isNearWarpPosition = Vector3.Distance(bilboPos, SavedWarpPos) < 100;

            if (!waiting && !succesfullyWarped && hasMovedSignificantly && isNearWarpPosition)
            {
                succesfullyWarped = true;
            }

            lastBilboPos = bilboPos;

            // If Bilbo has succesfully warped to his destination, disable the cheat until Bilbo dies or the game is loading.
            if (succesfullyWarped)
            {
                if (StateLists.deathStates.Contains(mem.ReadInt(MemoryAddresses.bilboState)) || mem.ReadInt(MemoryAddresses.loading) == 1)
                {
                    succesfullyWarped = false;
                }

                return;
            }

            // If Bilbo has just died or the game has loaded, pause the cheat until Bilbo starts moving again.
            if (StateLists.deathStates.Contains(mem.ReadInt(MemoryAddresses.bilboState)) || mem.ReadInt(MemoryAddresses.loading) == 1)
            {
                waiting = true;
            }

            if (waiting && StateLists.movementStates.Contains(mem.ReadInt(MemoryAddresses.bilboState)))
            {
                waiting = false;
            }

            // Keep clipwarp set to the saved cheat setting while Bilbo is waiting
            if (waiting)
            {
                mem.WriteMemory(MemoryAddresses.warpCoordsX, "float", SavedWarpPos.X.ToString());
                mem.WriteMemory(MemoryAddresses.warpCoordsY, "float", SavedWarpPos.Y.ToString());
                mem.WriteMemory(MemoryAddresses.warpCoordsZ, "float", SavedWarpPos.Z.ToString());

                return;
            }

            Vector3 warpPos = new(mem.ReadFloat(MemoryAddresses.warpCoordsX), mem.ReadFloat(MemoryAddresses.warpCoordsY), mem.ReadFloat(MemoryAddresses.warpCoordsZ));

            if (warpPos.X == 0 && warpPos.Y == 0 && warpPos.Z == 0)
            {
                return;
            }

            // If Bilbo's warp position changes, reload the save by putting Bilbo in a falling death state
            if (SavedWarpPos.X != warpPos.X || SavedWarpPos.Y != warpPos.Y || SavedWarpPos.Z != warpPos.Z)
            {
                mem?.WriteMemory(MemoryAddresses.stamina, "float", "10");
                mem?.WriteMemory(MemoryAddresses.bilboState, "int", "27");

                waiting = true;
            }
        }
    }
}
