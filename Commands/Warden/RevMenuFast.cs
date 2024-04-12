using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region RevMenuFast

    [ConsoleCommand("rmf")]
    public void RevMenuFast(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        if (LatestWCommandUser != player.SteamID)
        {
            player.PrintToChat($"{Prefix} {CC.B}Sadece {CC.W} Komutçu bu menüyü açabilir");
            return;
        }
        if (CurrentCtRespawns >= 3)
        {
            player.PrintToChat($"{Prefix} {CC.B}En Fazla {CC.R}3 kere {CC.W} respawn atabilirsin");
            return;
        }

        var players = GetPlayers(CsTeam.CounterTerrorist)
            .Where(x => x.PawnIsAlive == false)
            .ToList();
        if (players == null || players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W} Revlenecek Hiç Ölü CT yok");
        }
        else
        {
            LogManagerCommand(player.SteamID, info.GetCommandString);
            var fastRev = players.FirstOrDefault();
            if (fastRev != null)
            {
                CustomRespawn(fastRev);
                CurrentCtRespawns++;
                Server.PrintToChatAll($"{Prefix} {CC.B}{fastRev.PlayerName} {CC.W} Rev menüden revlendi | Son {3 - CurrentCtRespawns} rev");
            }
        }
    }

    [ConsoleCommand("rmfsilent")]
    public void RevMenuFastSilent(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        if (LatestWCommandUser != player.SteamID)
        {
            return;
        }
        if (CurrentCtRespawns >= 3)
        {
            return;
        }

        var players = GetPlayers(CsTeam.CounterTerrorist)
            .Where(x => x.PawnIsAlive == false)
            .ToList();
        if (players != null && players.Count != 0)
        {
            LogManagerCommand(player.SteamID, info.GetCommandString);
            var fastRev = players.FirstOrDefault();
            if (fastRev != null)
            {
                CustomRespawn(fastRev);
                CurrentCtRespawns++;
                Server.PrintToChatAll($"{Prefix} {CC.B}{fastRev.PlayerName} {CC.W} Rev menüden revlendi | Son {3 - CurrentCtRespawns} rev");
            }
        }
    }

    #endregion RevMenuFast
}