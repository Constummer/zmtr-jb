using CounterStrikeSharp.API.Core;

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
            CoinRemoveOnWardenTeamChange(player, info);

            return HookResult.Continue;
        });
    }
}