using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void AddTimers()
    {
        // AddTimer(0.3f, () =>
        //{
        //    if (freezeWanted)
        //    {
        //        if (freezeTimer == 0)
        //        {
        //            GetPlayers()
        //            .Where(x => x != null
        //                 && x.PlayerPawn.IsValid
        //                 && x.PawnIsAlive
        //                 && x.IsValid
        //                 && x?.PlayerPawn?.Value != null
        //                 && (CsTeam)x.TeamNum == CsTeam.Terrorist
        //                 && x.PlayerPawn.Value.MoveType == MoveType_t.MOVETYPE_NONE)
        //            .ToList()
        //            .ForEach(x =>
        //            {
        //                SetColour(x, Color.FromArgb(255, 0, 0, 255));
        //                RefreshPawn(x);
        //            });
        //        }
        //    }
        //}, TimerFlags.REPEAT);
    }
}