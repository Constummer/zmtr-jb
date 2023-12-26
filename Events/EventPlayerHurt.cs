using CounterStrikeSharp.API.Core;

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