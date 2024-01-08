using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Respawn

    [ConsoleCommand("hrespawn", "öldüğü yerde canlanır")]
    [ConsoleCommand("1up", "öldüğü yerde canlanır")]
    [CommandHelper(1, "<playerismi>")]
    public void HRespawn(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye8"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        var targetArgument = GetTargetArgument(target);

        switch (targetArgument)
        {
            case TargetForArgument.None:
                {
                    var ps = GetPlayers()
                            .Where(x => (x.PlayerName?.ToLower()?.Contains(target) ?? false)
                                       && x.PawnIsAlive == false)
                            .ToList();
                    if (ps.Count > 1)
                    {
                        player.PrintToChat($"{Prefix} {CC.W} Birden fazla eşleşme bulundu, eğer isim veremiyorsanız #1 gibi oyuncu id ile deneyebilirsin.");
                    }
                    ps.ForEach(x =>
                    {
                        RespawnPlayer(x);
                        if (targetArgument == TargetForArgument.None)
                        {
                            Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu öldüğü yerde {CC.B}canlandırdı{CC.W}.");
                        }
                    });
                }
                break;

            case TargetForArgument.Me:
                RespawnPlayer(player);
                Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{player.PlayerName} {CC.W}adlı oyuncuyu öldüğü yerde {CC.B}canlandırdı{CC.W}.");

                break;

            case TargetForArgument.UserIdIndex:
                {
                    var ps = GetPlayers()
                            .Where(x => GetUserIdIndex(target) == x.UserId
                                       && x.PawnIsAlive == false)
                            .ToList();
                    if (ps.Count > 1)
                    {
                        player.PrintToChat($"{Prefix} {CC.W} Birden fazla eşleşme bulundu, eğer isim veremiyorsanız #1 gibi oyuncu id ile deneyebilirsin.");
                    }
                    ps.ForEach(x =>
                    {
                        RespawnPlayer(x);
                        if (targetArgument == TargetForArgument.None)
                        {
                            Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu öldüğü yerde {CC.B}canlandırdı{CC.W}.");
                        }
                    });
                }
                break;

            default:
                break;
        }
    }

    #endregion Respawn
}