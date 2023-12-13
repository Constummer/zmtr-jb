using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal void EventPlayerHurt()
    {
        RegisterEventHandler<EventPlayerHurt>((@event, info) =>
        {
            if (@event.Userid.IsBot == true)
            {
                return HookResult.Continue;
            }

            if (ActiveGodMode.TryGetValue(@event.Userid.SteamID, out var value))
            {
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