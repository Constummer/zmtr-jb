using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void TeamCommandListener()
    {
        AddCommandListener("team", (player, info) =>
        {
            CoinRemoveOnWardenTeamChange(player, info);
            return HookResult.Continue;
        });
    }
}