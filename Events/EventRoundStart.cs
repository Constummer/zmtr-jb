using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static DateTime RoundStartTime = DateTime.UtcNow;

    private void EventRoundStart()
    {
        RegisterEventHandler<EventRoundStart>((@event, _) =>
        {
            RoundStartTime = DateTime.UtcNow;
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
            }, SOM);
            HookDisabled = true;

            AddTimer(5.0f, () =>
            {
                HookDisabled = false;
            }, SOM);
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
                }, SOM);
                AddTimer(0.5f, () =>
                {
                    if (tempUserId != -1)
                    {
                        CreateParachute(tempUserId);
                    }
                }, SOM);
            }
            return HookResult.Continue;
        });
    }
}