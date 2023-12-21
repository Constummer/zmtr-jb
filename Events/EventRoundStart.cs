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
            foreach (var item in GetPlayers())
            {
                if (HideFoots.TryGetValue(item.SteamID, out bool hideFoot))
                {
                    if (hideFoot)
                    {
                        item.PlayerPawn.Value.Render = Color.FromArgb(254, 254, 254, 254);
                        RefreshPawn(item);
                    }
                    else
                    {
                        item.PlayerPawn.Value.Render = DefaultPlayerColor;
                        RefreshPawn(item);
                    }
                }
            }
            return HookResult.Continue;
        });
    }
}