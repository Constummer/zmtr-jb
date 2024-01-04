using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnClientConnected()
    {
        RegisterListener<Listeners.OnClientConnected>(playerSlot =>
        {
            uint finalSlot = (uint)playerSlot + 1;
            CCSPlayerController player = new CCSPlayerController(NativeAPI.GetEntityFromIndex((int)finalSlot));
            if (player == null || player.UserId < 0)
                return;

            if (player?.SteamID != null && player!.SteamID != 0)
            {
                Server.NextFrame(async () =>
                {
                    await GetPlayerMarketData(player!.SteamID);
                    await AddOrUpdatePlayerToPlayerNameTable(player!.SteamID, player.PlayerName);
                });
            }
        });
    }
}