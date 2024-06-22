using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Respawn

    [ConsoleCommand("reshp")]
    [ConsoleCommand("revhp")]
    [CommandHelper(2, "<gonderilecek kişi> <hp>")]
    public void Reshp(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false) return;

        if (!int.TryParse(info.ArgString.GetArgLast(), out var miktar) || miktar <= 0)
        {
            player!.PrintToChat($"{Prefix}{CC.G} Can değeri yanlış!");
            return;
        }

        var target = info.ArgString.GetArgSkipFromLast(1);
        if (FindSinglePlayer(player, target, out var x) == false)
        {
            return;
        }

        if (x.PawnIsAlive == false)
        {
            player.PrintToChat($"{Prefix} {CC.W}Hayatta olmayan birisinin ismini verdin.");
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        ResGelHpAction(x, miktar, player.PlayerName);
    }

    private void ResGelHpAction(CCSPlayerController? player, int health, string adminName)
    {
        if (ValidateCallerPlayer(player, false) == false) return;

        GetPlayers(CsTeam.Terrorist)
         .ToList()
         .ForEach(x =>
         {
             if (x.PawnIsAlive != false)
             {
                 x.Pawn.Value!.Health = health;

                 RefreshPawn(x);
             }
             else
             {
                 CustomRespawn(x);
             }
             AddTimer(0.5f, () =>
             {
                 if (ValidateCallerPlayer(x, false) == false) return;
                 if (ValidateCallerPlayer(player, false) == false) return;
                 var playerAbs = player.PawnIsAlive == false ? player.Pawn.Value.AbsOrigin : player.PlayerPawn.Value.AbsOrigin;
                 x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
             }, SOM);
         });
        if (ValidateCallerPlayer(player, false) == false) return;

        Server.PrintToChatAll($"{AdliAdmin(adminName)} {CC.R}ÖLÜLERİ {CC.W}yeniden canlandırdı.");
        Server.PrintToChatAll($"{AdliAdmin(adminName)} {CC.B}YAŞAYAN {CC.W}herkesin canını {CC.G}{health}{CC.W} olarak ayarladı.");
        Server.PrintToChatAll($"{AdliAdmin(adminName)} {CC.W}herkesi {CC.Or}{player.PlayerName}{CC.W} adlı oyuncunun yanına ışınladı.");
    }

    #endregion Respawn
}