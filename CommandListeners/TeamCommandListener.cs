using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

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