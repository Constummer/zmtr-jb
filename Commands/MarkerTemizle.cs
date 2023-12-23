using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region RR

    [ConsoleCommand("markertemizle", "Eli yeniden baslatir")]
    public void MarkerTemizle(CCSPlayerController? player, CommandInfo info)
    {
        if (player!.SteamID != LatestWCommandUser ||
            ValidateCallerPlayer(player) == false)
        {
            player.PrintToChat($"sen bunu temizleyemezsin :}}");
            return;
        }
        ClearLasers();
    }

    #endregion RR
}