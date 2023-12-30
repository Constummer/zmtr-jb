using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal void EventPlayerHurt()
    {
        RegisterEventHandler((GameEventHandler<EventPlayerHurt>)((@event, info) =>
        {
            if (@event == null)
                return HookResult.Continue;
            if (ValidateCallerPlayer(@event.Userid, false) == false)
            {
                return HookResult.Continue;
            }

            if (@event.Userid?.IsBot == true)
            {
                return HookResult.Continue;
            }

            CCSPlayerController? player = @event.Userid;
            if (LrActive == false)
            {
                GodHurtCover(@event, player);
                if (@event.Attacker != null
                && ((CEntityInstance)@event.Attacker).IsValid == true
                && ((CEntityInstance)@event.Attacker).Index != 32767)
                {
                    TeamYapActive(@event.Attacker, player, @event.DmgHealth, @event.DmgArmor);
                }
            }

            return HookResult.Continue;
        }), HookMode.Post);
    }
}