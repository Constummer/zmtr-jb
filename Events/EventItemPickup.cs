using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventItemPickup()
    {
        RegisterEventHandler<EventItemPickup>((@event, info) =>
        {
            try
            {
                if (ActiveTeamGamesGameBase != null)
                {
                    ActiveTeamGamesGameBase?.EventItemPickup(@event);
                }
                else
                {
                    if (@event == null) return HookResult.Continue;
                    if (ValidateCallerPlayer(@event.Userid, false) == false) return HookResult.Continue;
                    if (SutolCommandCalls.Contains(@event?.Userid?.SteamID ?? 0))
                    {
                        @event.Userid.RemoveWeapons();
                    }
                }
                return HookResult.Continue;
            }
            catch (Exception e)
            {
                ConsMsg(e.Message);
                return HookResult.Continue;
            }
        }, HookMode.Post);
    }
}