using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("cekilis")]
    [ConsoleCommand("cek")]
    public void Cekilis(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var players = GetPlayers();

        LogManagerCommand(player.SteamID, info.GetCommandString);
        CekilisAction(players);
    }

    [ConsoleCommand("cekilist")]
    [ConsoleCommand("tcekilis")]
    [ConsoleCommand("cekt")]
    [ConsoleCommand("tcek")]
    public void CekilisT(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var players = GetPlayers(CsTeam.Terrorist);

        LogManagerCommand(player.SteamID, info.GetCommandString);
        CekilisAction(players);
    }

    [ConsoleCommand("cekiliso")]
    [ConsoleCommand("ocekilis")]
    [ConsoleCommand("ceko")]
    [ConsoleCommand("ocek")]
    public void CekilisO(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var players = GetPlayers().Where(x => x.PawnIsAlive == false);

        LogManagerCommand(player.SteamID, info.GetCommandString);
        CekilisAction(players);
    }

    [ConsoleCommand("cekilisy")]
    [ConsoleCommand("ycekilis")]
    [ConsoleCommand("ceky")]
    [ConsoleCommand("ycek")]
    public void CekilisY(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var players = GetPlayers().Where(x => x.PawnIsAlive);

        LogManagerCommand(player.SteamID, info.GetCommandString);
        CekilisAction(players);
    }

    [ConsoleCommand("cekilisct")]
    [ConsoleCommand("ctcekilis")]
    [ConsoleCommand("cekct")]
    [ConsoleCommand("ctcek")]
    public void CekilisCT(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var players = GetPlayers(CsTeam.CounterTerrorist);

        LogManagerCommand(player.SteamID, info.GetCommandString);
        CekilisAction(players);
    }

    private static void CekilisAction(IEnumerable<CCSPlayerController> players)
    {
        var randomX = players.Skip(_random.Next(players.Count())).FirstOrDefault();
        if (randomX != null)
        {
            Server.PrintToChatAll($"{Prefix} {CC.W}Çekilişi kazanan kişi {CC.B}{randomX.PlayerName}{CC.W}!.");
        }
    }
}