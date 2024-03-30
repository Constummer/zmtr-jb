using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GeriSay

    [ConsoleCommand("gerisay", "Bicak dahil silme")]
    [CommandHelper(1, "<Mesaj, Örn: !gerisay 10 sn içinde kafese>")]
    public void GeriSay(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount <= 1) return;
        LogManagerCommand(player.SteamID, info.GetCommandString);
        BasicCountdown.CommandStartTextCountDown(this, info.ArgString);
    }

    #endregion GeriSay
}