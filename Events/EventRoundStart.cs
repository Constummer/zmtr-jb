using CounterStrikeSharp.API;
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
            AddTimer(1.0f, () =>
            {
                CoinGo = true;
            });
            foreach (var x in GetPlayers())
            {
                if (x.SteamID == LatestWCommandUser)
                {
                    x.VoiceFlags &= ~VoiceFlags.Muted;
                }
                else
                {
                    if (Unmuteds.Contains(x.SteamID) == false)
                    {
                        x.VoiceFlags |= VoiceFlags.Muted;
                    }
                }
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