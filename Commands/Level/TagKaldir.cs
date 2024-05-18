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
            SetStatusClanTag(player);

            player.PrintToChat($"{Prefix}{CC.B} !tagac{CC.W} ile tagini tekrardan acabilirsin.");
        }
    }

    [ConsoleCommand("taginikaldir")]
    [ConsoleCommand("taginikapat")]
    [ConsoleCommand("taginisil")]
    [CommandHelper(1, "<Seviye tagi kaldirilacak kişi>")]
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
        if (FindSinglePlayer(player, target, out var x) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        if (LevelTagDisabledPlayers.Any(y => y == x.SteamID) == false)
        {
            LevelTagDisabledPlayers.Add(x.SteamID);
            UpdatePlayerLevelTagDisableData(x.SteamID, true);
            x.Clan = null;
            SetStatusClanTag(x);
            player.PrintToChat($"{Prefix} {CC.B}{x.PlayerName}{CC.W} Adlı oyuncunun tagını kapattın.");
            x.PrintToChat($"{AdliAdmin(player.PlayerName)}{CC.B} SEVIYE {CC.W} tagini kapatti.");
        }
    }

    #endregion TagKaldir
}