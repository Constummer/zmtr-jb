using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region TagKaldir

    [ConsoleCommand("isyantakimkaldir")]
    [ConsoleCommand("isyantakimsil")]
    [CommandHelper(minArgs: 1, "<isyan takimdan silinecek kişi>")]
    public void IsyanTakimKaldir(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        if (info.ArgCount != 2) return;
        var targetPlayer = info.ArgString.GetArg(0);
        var targetArgument = GetTargetArgument(targetPlayer);

        var players = GetPlayers()
               .Where(x =>
               (targetArgument == TargetForArgument.UserIdIndex
               ? GetUserIdIndex(targetPlayer) == x.UserId : targetArgument == TargetForArgument.Me ? x.SteamID == player.SteamID : false) || (x.PlayerName?.ToLower()?.Contains(targetPlayer?.ToLower()) ?? false))
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
        var y = players.FirstOrDefault();
        if (ValidateCallerPlayer(y, false) == false) return;

        if (IsyanTeamPlayers.Any(x => x == y.SteamID) == false)
        {
            player.PrintToChat($"{Prefix}{CC.B} {y.PlayerName}{CC.W} isimli oyuncu isyan takımında değil.");
            return;
        }
        else
        {
            IsyanTeamPlayers.Remove(player.SteamID);
            RemovePlayerIsteamData(player.SteamID);
            player.Clan = null;
            AddTimer(0.2f, () =>
            {
                if (ValidateCallerPlayer(player, false) == false) return;
                Utilities.SetStateChanged(player, "CCSPlayerController", "m_szClan");
                Utilities.SetStateChanged(player, "CBasePlayerController", "m_iszPlayerName");
            }, SOM);
        }
    }

    #endregion TagKaldir
}