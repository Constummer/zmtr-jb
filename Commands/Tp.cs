using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Tp

    [ConsoleCommand("tp")]
    [ConsoleCommand("thirdperson")]
    [ConsoleCommand("thirdp")]
    public void Tp(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
    }

    #endregion Tp
}