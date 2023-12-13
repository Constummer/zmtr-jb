using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Cvars;
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
             if (x.PawnIsAlive == false)
             {
                 x.PlayerPawn.Value.Respawn();
                 x.Respawn();
                 x.Teleport(x.PlayerPawn.Value.AbsOrigin, x.PlayerPawn.Value.AbsRotation, x.PlayerPawn.Value.AbsVelocity);
             }
             else
             {
                 x.Pawn.Value.Health = 100;

                 //RefreshPawn(x);
             }
         });
        Server.PrintToChatAll("aflandiniz");
    }

    #endregion Af
}