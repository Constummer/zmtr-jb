using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Af

    [ConsoleCommand("af", "af")]
    public void Af(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/admin1"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (IsEliGivenCheck == false)
        {
            if (CurrentCtRespawnFirst == false)
            {
                CurrentCtRespawns = 0;
                CurrentCtRespawnFirst = true;
            }
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers()
         .ToList()
         .ForEach(x =>
         {
             if (x.PawnIsAlive != false)
             {
                 x.Pawn.Value!.Health = 100;

                 RefreshPawn(x);
             }
             else
             {
                 CustomRespawn(x);
             }
         });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} herkesi yeniden canlandırdı.");
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} herkesin canını {CC.G}100{CC.W} olarak ayarladı.");
    }

    #endregion Af
}