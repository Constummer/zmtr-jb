using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region OnTeamCommand

    [ConsoleCommand("team")]
    [CommandHelper(0, "<nick> <ct-t-spec-1-2-3>")]
    public void OnTeamCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 3) return;
        var targetPlayer = info.GetArg(1);
        var targetArgument = GetTargetArgument(targetPlayer);
        var targetTeam = info.GetArg(2) switch
        {
            "ct" => CsTeam.CounterTerrorist,
            "CT" => CsTeam.CounterTerrorist,
            "cT" => CsTeam.CounterTerrorist,
            "Ct" => CsTeam.CounterTerrorist,
            "3" => CsTeam.CounterTerrorist,
            "t" => CsTeam.Terrorist,
            "T" => CsTeam.Terrorist,
            "2" => CsTeam.Terrorist,
            "spec" => CsTeam.Spectator,
            "SPEC" => CsTeam.Spectator,
            "1" => CsTeam.Spectator,
            _ => CsTeam.None
        };

        if (targetTeam == CsTeam.None)
        {
            player.PrintToChat($"{Prefix} {CC.W}HEDEF yanlış!");
            return;
        }
        var players = GetPlayers()
               .Where(x => x.PlayerName.ToLower().Contains(targetPlayer.ToLower()))
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

        if (x?.SteamID != null && x!.SteamID != 0)
        {
            x.ChangeTeam(targetTeam);
        }
        Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{player.PlayerName} {CC.W}hedefi {CC.B}{targetTeam.ToString()} {CC.W}takimina gönderdi.");
    }

    #endregion OnTeamCommand
}