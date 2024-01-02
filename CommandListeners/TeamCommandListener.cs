using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void TeamCommandListener()
    {
        AddCommandListener("team", (player, info) =>
        {
            Unmuteds = Unmuteds.Where(X => X != player.SteamID).ToList();
            player.VoiceFlags |= VoiceFlags.Muted;
            CoinRemoveOnWardenTeamChange(player, info);
            return HookResult.Continue;
        });
    }
}