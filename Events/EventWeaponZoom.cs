using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventWeaponZoom()
    {
        RegisterEventHandler<EventWeaponZoom>((@event, _) =>
        {
            try
            {
                LrWeaponZoom(@event);
                ActiveTeamGamesGameBase?.EventWeaponZoom(@event);

                return HookResult.Continue;
            }
            catch (Exception e)
            {
                ConsMsg(e.Message);
                return HookResult.Continue;
            }
        });
    }
}