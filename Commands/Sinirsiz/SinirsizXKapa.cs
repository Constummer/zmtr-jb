using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SinirsizX

    [ConsoleCommand("sinirsizxKapa")]
    [ConsoleCommand("smx")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead>")]
    public void SinirsizXKapa(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgCount > 1 ? info.GetArg(1) : null;

        SinirsizXTimer = GiveSinirsizCustomNade(0, SinirsizXTimer, null, target, player.PlayerName);
    }

    #endregion SinirsizX
}