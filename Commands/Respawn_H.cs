using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("hrespawn", "öldüğü yerde canlanır")]
    [ConsoleCommand("1up", "öldüğü yerde canlanır")]
    [CommandHelper(1, "<playerismi>")]
    public void HRespawn(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye8", "@css/seviye8") == false)
        {
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
                            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu öldüğü yerde {CC.B}canlandırdı{CC.W}.");
                        }
                    });
                }
                break;

            case TargetForArgument.Me:
                RespawnPlayer(player);
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{player.PlayerName} {CC.W}adlı oyuncuyu öldüğü yerde {CC.B}canlandırdı{CC.W}.");

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
                            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu öldüğü yerde {CC.B}canlandırdı{CC.W}.");
                        }
                    });
                }
                break;

            default:
                break;
        }
    }
}