using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SKZ

    [ConsoleCommand("skzslay")]
    public void SkzSlay(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (SkzV2FailedSteamIds?.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Yapamayan kimse bulunamadi! {CC.B}!skz {CC.W}ile skz başlattığına emin ol!");
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers(CsTeam.Terrorist)
            .Where(x => x.PawnIsAlive && SkzV2FailedSteamIds.Contains(x.SteamID))
            .ToList()
            .ForEach(x =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                var playerPawn = x.PlayerPawn.Value;
                playerPawn.CommitSuicide(false, true);
            });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.R}SKZ'yi {CC.W}yapamayan oyunculari {CC.LR}öldürdü{CC.W}.");
    }

    #endregion SKZ
}