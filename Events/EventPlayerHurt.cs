using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal void EventPlayerHurt()
    {
        RegisterEventHandler<EventPlayerHurt>((@event, info) =>
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
            }
            //-------ATTACKER RELEATED THINGS MUST BE UNDER THIS IF AFTER THIS CHECK---------
            //-------ATTACKER RELEATED THINGS MUST BE UNDER THIS IF AFTER THIS CHECK---------
            //-------ATTACKER RELEATED THINGS MUST BE UNDER THIS IF AFTER THIS CHECK---------
            //-------ATTACKER RELEATED THINGS MUST BE UNDER THIS IF AFTER THIS CHECK---------
            //-------ATTACKER RELEATED THINGS MUST BE UNDER THIS IF AFTER THIS CHECK---------
            //-------ATTACKER RELEATED THINGS MUST BE UNDER THIS IF AFTER THIS CHECK---------
            if (@event.Attacker == null
                 || ((CEntityInstance)@event.Attacker).IsValid != true
                 || ((CEntityInstance)@event.Attacker).Index == 32767)
            {
                return HookResult.Continue;
            }

            if (LrActive == false)
            {
                TeamYapActive(@event.Attacker, player, @event.DmgHealth, @event.DmgArmor);
            }

            return HookResult.Continue;
        }, HookMode.Post);
    }
}