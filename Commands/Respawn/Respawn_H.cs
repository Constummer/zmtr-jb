using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, Vector> DeathLocations = new();

    [ConsoleCommand("hrespawn", "öldüğü yerde canlanır")]
    [ConsoleCommand("1up", "öldüğü yerde canlanır")]
    [CommandHelper(1, "<playerismi>")]
    public void HRespawn(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye8, Perm_Seviye8) == false)
        {
            return;
        }
        var target = info.ArgString.GetArgSkip(0);
        if (FindSinglePlayer(player, target, out var x, (y => y.PawnIsAlive == false)) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        RespawnPlayer(x);
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu öldüğü yerde {CC.B}canlandırdı{CC.W}.");
    }
}