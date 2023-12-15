using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerPing()
    {
        RegisterEventHandler<EventPlayerPing>(EXTRAOnPlayerPing);
    }
}