using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

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
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.Green} sen bunu temizleyemezsin :}}");
            return;
        }
        ClearLasers();
    }

    #endregion MarkerTemizle
}