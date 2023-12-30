using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void TeamCommandListener()
    {
        AddCommandListener("team", (player, info) =>
        {
            player.VoiceFlags |= VoiceFlags.Muted;

            CoinRemoveOnWardenTeamChange(player, info);
            return HookResult.Continue;
        });
    }
}