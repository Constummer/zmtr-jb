using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region MarkerTemizle

    [ConsoleCommand("markertemizle")]
    public void MarkerTemizle(CCSPlayerController? player, CommandInfo info)
    {
        if (player!.SteamID != LatestWCommandUser ||
            ValidateCallerPlayer(player) == false)
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.G} sen bunu temizleyemezsin :}}");
            return;
        }
        ClearLasers();
    }

    #endregion MarkerTemizle
}