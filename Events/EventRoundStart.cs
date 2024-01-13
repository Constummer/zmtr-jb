using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventRoundStart()
    {
        RegisterEventHandler<EventRoundStart>((@event, _) =>
        {
            Server.ExecuteCommand("mp_force_pick_time 3000");
            Server.ExecuteCommand("mp_autoteambalance 0");
            Server.ExecuteCommand("mp_equipment_reset_rounds 1");
            Server.ExecuteCommand("mp_t_default_secondary \"\" ");
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
                var tempSteamId = x.SteamID;
                var tempName = x.PlayerName;
                var tempUserId = x.UserId;
                if (ValidateCallerPlayer(x, false) == false) continue;
                AddTimer(20f, () =>
                {
                    if (tempSteamId != 0)
                    {
                        GivePlayerRewards(tempSteamId, tempName);
                    }
                });
                AddTimer(0.5f, () =>
                {
                    if (tempUserId != -1)
                    {
                        CreateParachute(tempUserId);
                    }
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