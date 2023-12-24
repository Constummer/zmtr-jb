using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal void EventPlayerHurt()
    {
        RegisterEventHandler((GameEventHandler<EventPlayerHurt>)((@event, info) =>
        {
            if (@event.Userid.IsBot == true)
            {
                return HookResult.Continue;
            }
            CCSPlayerController? player = @event.Userid;
            if (LrActive == false)
            {
                GodHurtCover(@event, player);
                TeamYapActive(@event.Attacker, player, @event.DmgHealth, @event.DmgArmor);
            }

            return HookResult.Continue;
        }), HookMode.Post);
    }
}