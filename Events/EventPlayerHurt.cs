using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerHurt()
    {
        RegisterEventHandler<EventPlayerHurt>((@event, info) =>
        {
            if (@event.Userid.IsBot == true)
            {
                return HookResult.Continue;
            }

            if (ActiveGodMode.TryGetValue(@event.Userid.SteamID, out var value))
            {
                Logger.LogInformation(value.ToString());
                if (value)
                {
                    return HookResult.Stop;
                }
                return HookResult.Continue;
            }

            return HookResult.Continue;
        }, HookMode.Post);
    }
}