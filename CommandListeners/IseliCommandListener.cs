using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void IseliCommandListener()
    {
        AddCommandListener("iseli", (player, info) =>
            {
                IsEliWardenNotify();
                IsEliTerroristTp(player);
                return HookResult.Continue;
            });
    }

    private void IsEliTerroristTp(CCSPlayerController? player)
    {
        if (ValidateCallerPlayer(player, printMsg: false) == false
           || GetTeam(player) != CsTeam.CounterTerrorist)
        {
            return;
        }

        GetPlayers()
        .Where(x => x.PawnIsAlive
                           && GetTargetAction(x, "@t", null))
               .ToList()
               .ForEach(x =>
               {
                   RemoveWeapons(x, true);

                   x.PlayerPawn.Value.Teleport(Hucre, ANGLE_ZERO, VEC_ZERO);
               });
    }
}