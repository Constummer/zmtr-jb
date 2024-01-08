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
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
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
        Server.PrintToChatAll($"{Prefix} {CC.Ol}{player.PlayerName} {CC.W}adlı admin herkesi yeniden canlandırdı.");
        Server.PrintToChatAll($"{Prefix} {CC.Ol}{player.PlayerName} {CC.W}adlı admin herkesin canını {CC.G}100{CC.W} olarak ayarladı.");
    }

    #endregion Af
}