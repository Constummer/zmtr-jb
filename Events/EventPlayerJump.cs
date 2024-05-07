using CounterStrikeSharp.API;
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
                    if ((@event.Userid.Buttons & PlayerButtons.Jump) != 0)
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
                    if (JumpCountActive)
                    {
                        if (JumpCount.TryGetValue(@event.Userid.SteamID, out var val))
                        {
                            JumpCount[@event.Userid.SteamID] = new(val.Item1 + 1, val.Item2);
                        }
                        else
                        {
                            JumpCount.Add(@event.Userid.SteamID, new(1, @event.Userid.PlayerName));
                        }
                    }
                }
            }
            return HookResult.Continue;
        });
    }
}