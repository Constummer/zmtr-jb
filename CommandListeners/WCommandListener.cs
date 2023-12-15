using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static ulong LatestWCommandUser = 0;

    private void WCommandListener()
    {
        AddCommandListener("w", (player, info) =>
        {
            if (ValidateCallerPlayer(player, false) == false)
            {
                return HookResult.Continue;
            }
            SetColour(player, Color.FromArgb(255, 0, 0, 255));
            LatestWCommandUser = player!.SteamID;
            return HookResult.Continue;
        });
    }
}