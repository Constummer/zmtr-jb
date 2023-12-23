using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Af

    [ConsoleCommand("af", "af")]
    public void Af(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetPlayers()
         .ToList()
         .ForEach(x =>
         {
             Logger.LogInformation(x.PlayerName);
             Server.ExecuteCommand($"mp_respawn_on_death_ct 1;mp_respawn_on_death_t 1");
             if (x.PawnIsAlive != false)
             {
                 x.Pawn.Value!.Health = 100;

                 RefreshPawn(x);
                 Server.ExecuteCommand($"mp_respawn_on_death_ct 0;mp_respawn_on_death_t 0");
             }
         });
    }

    #endregion Af
}