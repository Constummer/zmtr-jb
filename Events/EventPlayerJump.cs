using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerJump()
    {
        RegisterEventHandler<EventPlayerJump>((@event, _) =>
        {
            if (@event != null)
            {
                if (ValidateCallerPlayer(@event.Userid, false))
                {
                    if (BattlePassDatas.TryGetValue(@event.Userid.SteamID, out var data))
                    {
                        data.OnEventPlayerJump();
                    }
                    if (BattlePassPremiumDatas.TryGetValue(@event.Userid.SteamID, out var battlePassPremiumData))
                    {
                        battlePassPremiumData?.OnEventPlayerJump();
                    }
                }
            }
            return HookResult.Continue;
        });
    }
}