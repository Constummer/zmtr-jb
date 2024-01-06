using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region TagKaldir

    [ConsoleCommand("tagkaldir")]
    public void TagKaldir(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (LevelTagDisabledPlayers.Any(x => x == player.SteamID) == false)
        {
            LevelTagDisabledPlayers.Add(player.SteamID);

            player.Clan = null;
            AddTimer(0.2f, () =>
            {
                Utilities.SetStateChanged(player, "CCSPlayerController", "m_szClan");
                Utilities.SetStateChanged(player, "CBasePlayerController", "m_iszPlayerName");
            });
            player.PrintToChat($"{Prefix}{CC.B} !tagac{CC.W} ile tagini tekrardan acabilirsin.");
        }
    }

    #endregion TagKaldir
}