using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region TagKaldir

    [ConsoleCommand("isyantakimkaldir")]
    [ConsoleCommand("isyantakimsil")]
    [ConsoleCommand("isyanteamkaldir")]
    [ConsoleCommand("isyanteamsil")]
    [CommandHelper(minArgs: 1, "<isyan takimdan silinecek kişi>")]
    public void IsyanTakimKaldir(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Yonetim))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var target = info.ArgString.GetArgSkip(0);
        if (FindSinglePlayer(player, target, out var y) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        if (IsyanTeamPlayers.Any(x => x == y.SteamID) == false)
        {
            player.PrintToChat($"{Prefix}{CC.B} {y.PlayerName}{CC.W} isimli oyuncu isyan takımında değil.");
            return;
        }
        else
        {
            IsyanTeamPlayers.Remove(y.SteamID);
            RemovePlayerIsteamData(y.SteamID);
            y.Clan = null;
            AddTimer(0.2f, () =>
            {
                if (ValidateCallerPlayer(y, false) == false) return;
                Utilities.SetStateChanged(y, "CCSPlayerController", "m_szClan");
                Utilities.SetStateChanged(y, "CBasePlayerController", "m_iszPlayerName");
            }, SOM);
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.B} {y.PlayerName}{CC.W} adlı oyuncuyu {CC.R}[İsyan Team]{CC.W}'den çıkardı");
        }
    }

    #endregion TagKaldir
}