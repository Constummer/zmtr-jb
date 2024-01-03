using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void KapilariAcCommandListener()
    {
        AddCommandListener("kapilariac", (player, info) =>
        {
            IsEliWardenNotify();
            return HookResult.Continue;
        });
        AddCommandListener("kapiac", (player, info) =>
        {
            IsEliWardenNotify();
            return HookResult.Continue;
        });
    }
}