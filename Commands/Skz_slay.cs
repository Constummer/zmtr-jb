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
        if (SkzTimeDatas.Where(x => x.Done == false).Any() == false)
        {
            player.PrintToChat($"{Prefix} {CC.W}Yapamayan kimse bulunamadi! {CC.B}!skz {CC.W}ile skz başlattığına emin ol!");
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers(CsTeam.Terrorist)
            .Where(x => x.PawnIsAlive)
            .ToList()
            .ForEach(x =>
            {
                var skzData = SkzTimeDatas.FirstOrDefault(y => y.SteamId == x.SteamID);
                if (skzData != null)
                {
                    if (skzData.Done == false)
                    {
                        if (ValidateCallerPlayer(x, false) == false) return;
                        var playerPawn = x.PlayerPawn.Value;
                        playerPawn.CommitSuicide(false, true);
                    }
                }
                else
                {
                    if (ValidateCallerPlayer(x, false) == false) return;
                    var playerPawn = x.PlayerPawn.Value;
                    playerPawn.CommitSuicide(false, true);
                }
            });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.R}SKZ'yi {CC.W}yapamayan oyunculari {CC.LR}öldürdü{CC.W}.");
    }

    #endregion SKZ
}