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