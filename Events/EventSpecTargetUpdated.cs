using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventSpecTargetUpdated()
    {
        RegisterEventHandler<EventSpecTargetUpdated>((@event, _) =>
        {
            Server.PrintToChatAll($"{@event.EventName}");

            return HookResult.Continue;
        });
        RegisterEventHandler<EventCsPrevNextSpectator>((@event, _) =>
        {
            Server.PrintToChatAll($"{@event.EventName}");

            return HookResult.Continue;
        });
        RegisterEventHandler<EventSpecModeUpdated>((@event, _) =>
        {
            Server.PrintToChatAll($"{@event.EventName}");

            return HookResult.Continue;
        });
        RegisterEventHandler<EventSpecTargetUpdated>((@event, _) =>
           {
               if (@event == null) return HookResult.Continue;
               Server.PrintToChatAll($"{@event.Target}");

               if (ValidateCallerPlayer(@event.Userid, false) == false)
               {
                   return HookResult.Continue;
               }
               Server.PrintToChatAll($"{@event.Target}");
               if (!AdminManager.PlayerHasPermissions(@event.Userid, Perm_Root))
               {
                   return HookResult.Continue;
               }
               else
               {
                   var a = GetPlayers().Where(x => x.Handle == @event.Handle).FirstOrDefault();
                   if (ValidateCallerPlayer(a, false))
                   {
                       Server.PrintToChatAll($"{a.PlayerName}");
                   }
               }

               return HookResult.Continue;
           });
    }
}