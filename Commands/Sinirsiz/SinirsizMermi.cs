using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool UnlimitedReserverAmmoActive = false;

    #region SinirsizMermi

    [ConsoleCommand("sinirsizmermiac")]
    [ConsoleCommand("smac")]
    public void SinirsizMermiAc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        UnlimitedReserverAmmoActive = true;
    }

    #endregion SinirsizMermi
}