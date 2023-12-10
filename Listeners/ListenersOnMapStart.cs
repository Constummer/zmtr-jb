using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnMapStart()
    {
        RegisterListener<Listeners.OnMapStart>(name =>
        {
            ClearAll();
        });
    }
}