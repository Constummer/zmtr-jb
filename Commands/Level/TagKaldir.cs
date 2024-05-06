using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Commands.Targeting;

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

    [ConsoleCommand("taginikaldir")]
    [ConsoleCommand("taginikapat")]
    [ConsoleCommand("taginisil")]
    public void TaginiKaldir(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        var target = info.ArgString.GetArgSkip(0);

        if (FindSinglePlayer(player, target, out var x) == false || ValidateCallerPlayer(x, false) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        if (LevelTagDisabledPlayers.Any(y => y == x.SteamID) == false)
        {
            LevelTagDisabledPlayers.Add(x.SteamID);
            UpdatePlayerLevelTagDisableData(x.SteamID, true);
            x.Clan = null;
            AddTimer(0.2f, () =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                Utilities.SetStateChanged(x, "CCSPlayerController", "m_szClan");
                Utilities.SetStateChanged(x, "CBasePlayerController", "m_iszPlayerName");
            }, SOM);
            x.PrintToChat($"{AdliAdmin(player.PlayerName)}{CC.B} tagini {CC.W} kapatti.");
        }
    }

    #endregion TagKaldir
}