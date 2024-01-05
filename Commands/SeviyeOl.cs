using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SeviyeOl

    [ConsoleCommand("slotol")]
    [ConsoleCommand("seviyeol")]
    public void SeviyeOl(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        AddPlayerLevelData(player.SteamID);
    }

    #endregion SeviyeOl
}