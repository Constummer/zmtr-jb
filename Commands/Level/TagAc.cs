using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region TagAc

    [ConsoleCommand("tagac")]
    public void TagAc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (AdminManager.PlayerHasPermissions(player, Perm_Admin1))
        {
            player.PrintToChat($"{Prefix}{CC.B} !tagac{CC.W} komutunu kullanamazsın.");
            return;
        }
        UpdatePlayerLevelTagDisableData(player.SteamID, false);
        LevelTagDisabledPlayers = LevelTagDisabledPlayers.Where(x => x != player.SteamID).ToList();
        player.PrintToChat($"{Prefix}{CC.B} !tagkaldir{CC.W} ile tagini tekrardan kaldirabilirsin.");
    }

    #endregion TagAc
}