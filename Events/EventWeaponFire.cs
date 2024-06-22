using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventWeaponFire()
    {
        RegisterEventHandler<EventWeaponFire>((@event, _) =>
        {
            try
            {
                LrWeaponFire(@event);
                ActiveTeamGamesGameBase?.EventWeaponFire(@event);

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