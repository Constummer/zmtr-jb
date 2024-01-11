using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using System.Drawing;
using static JailbreakExtras.JailbreakExtras;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            HookDisabled = true;

            AddTimer(5.0f, () =>
            {
                HookDisabled = false;
            });
            for (int i = 0; i < Server.MaxPlayers; i++)
            {
                var x = Utilities.GetPlayerFromSlot(i);

                if (x == null)
                    continue;
                if (!x.IsValid || x.UserId == -1)
                    continue;
                AddTimer(20f, () => GivePlayerRewards(x.SteamID, x.PlayerName));
                AddTimer(0.5f, () =>
                {
                    CreateParachute(x);
                });
            }
            //foreach (var x in GetPlayers())
            //{
            //if (x.AuthorizedSteamID == null)
            //{
            //    continue;
            //}

            //if (x.SteamID == LatestWCommandUser)
            //{
            //    x.VoiceFlags &= ~VoiceFlags.Muted;
            //}
            //else
            //{
            //    if (Unmuteds.Contains(x.SteamID) == false)
            //    {
            //        x.VoiceFlags |= VoiceFlags.Muted;
            //    }
            //}
            //if (HideFoots.TryGetValue(x.SteamID, out bool hideFoot))
            //{
            //    if (hideFoot)
            //    {
            //        x.PlayerPawn.Value.Render = Color.FromArgb(254, 254, 254, 254);
            //        RefreshPawn(x);
            //    }
            //    else
            //    {
            //        x.PlayerPawn.Value.Render = DefaultPlayerColor;
            //        RefreshPawn(x);
            //    }
            //}
            //AddTimer(0.5f, () =>
            //{
            //    CreateParachute(x);
            //});
            //}
            return HookResult.Continue;
        });
    }
}