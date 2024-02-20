using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnMapEnd()
    {
        RegisterListener<Listeners.OnMapEnd>(() =>
        {
            //KillTimers();
            ClearAll();
            CreditModifier = 1;
            TPModifier = 1;
            HideFoots?.Clear();
            HookPlayers?.Clear();
            bUsingPara?.Clear();
            Unmuteds?.Clear();
            FovActivePlayers?.Clear();
            UpdateAllModels();
        });
    }
}