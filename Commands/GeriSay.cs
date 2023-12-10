using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

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
        BasicCountdown.CommandStartTextCountDown(this, info.ArgString);
    }

    #endregion GeriSay
}