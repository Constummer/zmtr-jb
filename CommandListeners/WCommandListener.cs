using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void WCommandListener()
    {
        AddCommandListener("w", (player, info) =>
        {
            Logger.LogInformation("w yakalandi");
            if (ValidateCallerPlayer(player, false) == false)
            {
                return HookResult.Continue;
            }
            SetColour(player, Color.FromArgb(255, 0, 0, 255));

            return HookResult.Continue;
        });
    }
}