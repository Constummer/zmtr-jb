using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnMapEnd()
    {
        RegisterListener<Listeners.OnMapEnd>(() =>
        {
            ClearAll();
            HideFoots?.Clear();
            HookPlayers?.Clear();
            bUsingPara?.Clear();
            Unmuteds?.Clear();
            UpdateAllModels();
            Unload(true);
        });
    }
}