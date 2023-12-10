using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region MarkerTemizle

    [ConsoleCommand("markertemizle", "Eli yeniden baslatir")]
    public void LaserTemizle(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        foreach (var item in Lasers)
        {
            item.Remove();
        }
        Lasers.Clear();
    }

    #endregion MarkerTemizle
}