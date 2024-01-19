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
    public void RespawnHp(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false) return;

        var target = info.ArgString.GetArg(0);
        if (!int.TryParse(info.ArgString.GetArg(1), out var health) || health < 1)
        {
            player!.PrintToChat($"{Prefix}{CC.G} Can değeri yanlış!");
            return;
        }
        var targetArgument = GetTargetArgument(target);
        var players = GetPlayers()
                 .Where(x => GetTargetAction(x, target, player!.PlayerName))
                 .ToList();

        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
            return;
        }
        if (players.Count != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
            return;
        }
        var x = players.FirstOrDefault();

        if (x.PawnIsAlive == false)
        {
            player.PrintToChat($"{Prefix} {CC.W}Hayatta olmayan birisinin ismini verdin.");
            return;
        }

        ResGelHpAction(x, health, player.PlayerName);
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