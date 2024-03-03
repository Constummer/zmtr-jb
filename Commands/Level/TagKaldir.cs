using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region TagKaldir

    [ConsoleCommand("tagkaldir")]
    [ConsoleCommand("tagkapat")]
    [ConsoleCommand("tagsil")]
    public void TagKaldir(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (AdminManager.PlayerHasPermissions(player, BasePermission))
        {
            player.PrintToChat($"{Prefix}{CC.B} !tagkapat{CC.W} komutunu kullanamazsın.");
            return;
        }
        if (LevelTagDisabledPlayers.Any(x => x == player.SteamID) == false)
        {
            LevelTagDisabledPlayers.Add(player.SteamID);
            UpdatePlayerLevelTagDisableData(player.SteamID, true);
            player.Clan = null;
            AddTimer(0.2f, () =>
            {
                if (ValidateCallerPlayer(player, false) == false) return;
                Utilities.SetStateChanged(player, "CCSPlayerController", "m_szClan");
                Utilities.SetStateChanged(player, "CBasePlayerController", "m_iszPlayerName");
            }, SOM);
            player.PrintToChat($"{Prefix}{CC.B} !tagac{CC.W} ile tagini tekrardan acabilirsin.");
        }
    }

    #endregion TagKaldir
}