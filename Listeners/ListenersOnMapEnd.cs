using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnMapEnd()
    {
        RegisterListener<Listeners.OnMapEnd>(() =>
        {
            ClearAll();
            UpdateAllModels();
            Unload(true);
        });
    }

    private void UpdateAllModels()
    {
        foreach (var item in GetPlayers())
        {
            if (item?.SteamID != null && item!.SteamID != 0)
            {
                Task.Run(async () =>
                {
                    await UpdatePlayerMarketData(item!.SteamID);
                });
            }
        }
    }
}