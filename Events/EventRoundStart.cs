using CounterStrikeSharp.API.Core;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventRoundStart()
    {
        RegisterEventHandler<EventRoundStart>((@event, _) =>
        {
            PrepareRoundDefaults();
            ClearAll();
            CoinAfterNewCommander();
            foreach (var x in GetPlayers())
            {
                if (HideFoots.TryGetValue(x.SteamID, out bool hideFoot))
                {
                    if (hideFoot)
                    {
                        x.PlayerPawn.Value.Render = Color.FromArgb(254, 254, 254, 254);
                        RefreshPawn(x);
                    }
                    else
                    {
                        x.PlayerPawn.Value.Render = DefaultPlayerColor;
                        RefreshPawn(x);
                    }
                }
            }
            return HookResult.Continue;
        });
    }
}