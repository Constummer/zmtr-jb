using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void IseliCommandListener()
    {
        AddCommandListener("iseli", (player, info) =>
            {
                IsEliWardenNotify();
                return HookResult.Continue;
            });
    }
}