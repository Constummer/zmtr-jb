using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Grup

    [ConsoleCommand("grup", "steam grup")]
    [ConsoleCommand("grub", "steam grup")]
    [ConsoleCommand("steam", "steam grup")]
    [ConsoleCommand("steamgrup", "steam grup")]
    public void Grup(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        PlayerGroupCheck(player, info);
    }

    #endregion Grup
}