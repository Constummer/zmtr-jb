using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventRoundStart()
    {
        RegisterEventHandler<EventRoundStart>((@event, _) =>
        {
            ClearAll();
            return HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerShoot>((@event, info) =>
        {
            Logger.LogInformation("aaaaaaa");

            if (ValidateCallerPlayer(@event?.Userid, false) == false)
            {
                return HookResult.Continue;
            }
            Logger.LogInformation("{0}   =    {1}", @event.Weapon, @event.Mode);
            return HookResult.Continue;
        });
    }
}