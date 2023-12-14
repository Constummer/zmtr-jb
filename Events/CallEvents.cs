using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void CallEvents()
    {
        EventRoundStart();
        EventPlayerDeath();
        EventPlayerHurt();
        EventPlayerSpawn();
        RegisterEventHandler<EventPlayerPing>(EXTRAOnPlayerPing);
    }
}