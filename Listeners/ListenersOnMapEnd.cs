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

    private void UpdateAllModels()
    {
        foreach (var player in GetPlayers())
        {
            if (player?.SteamID != null && player!.SteamID != 0)
            {
                Task.Run(async () =>
                {
                    await UpdatePlayerMarketData(player!.SteamID);
                });
            }
        }
    }
}