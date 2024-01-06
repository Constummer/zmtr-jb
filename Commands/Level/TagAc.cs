using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
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
        LevelTagDisabledPlayers = LevelTagDisabledPlayers.Where(x => x != player.SteamID).ToList();
        player.PrintToChat($" {CC.LR}[ZMTR]{CC.B} !tagkaldir{CC.W} ile tagini tekrardan kaldirabilirsin.");
    }

    #endregion TagAc
}