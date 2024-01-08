using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void TeamCommandListener()
    {
        AddCommandListener("team", (player, info) =>
        {
            if (RespawnAcActive)
            {
                AddTimer(1, () =>
                {
                    if (ValidateCallerPlayer(player, false) == true)
                    {
                        CustomRespawn(player);
                    }
                });
            }

            Unmuteds = Unmuteds.Where(X => X != player.SteamID).ToList();
            player.VoiceFlags |= VoiceFlags.Muted;
            CoinRemoveOnWardenTeamChange(player, info);

            return HookResult.Continue;
        });
    }
}