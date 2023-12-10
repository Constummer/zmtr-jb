using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

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
                   if (x.PawnIsAlive == false)
                   {
                       x.Respawn();
                   }
                   else
                   {
                       x.Pawn.Value.Health = 100;

                       RefreshPawn(x);
                   }
               });
        Server.PrintToChatAll("aflandiniz");
    }

    #endregion Af
}