using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static string Prefix { get => $" {CC.LR}[ZMTR]"; }

    public static void SharpTimerPrintHtml(CCSPlayerController player, string hudContent)
    {
        var @event = new EventShowSurvivalRespawnStatus(false)
        {
            LocToken = hudContent,
            Duration = 5,
            Userid = player
        };
        @event.FireEvent(false);

        @event = null;
    }
}