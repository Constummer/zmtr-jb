using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnClientDisconnect()
    {
        RegisterListener<Listeners.OnClientDisconnect>(playerSlot =>
        {
            uint finalSlot = (uint)playerSlot + 1;
            CCSPlayerController player = new CCSPlayerController(NativeAPI.GetEntityFromIndex((int)finalSlot));
            if (player == null || player.UserId < 0)
                return;

            if (player?.SteamID != null && player!.SteamID != 0)
            {
                Task.Run(async () =>
                {
                    await UpdatePlayerMarketData(player!.SteamID);
                });
            }
        });
    }
}