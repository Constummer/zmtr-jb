using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnClientConnected()
    {
        RegisterListener<Listeners.OnClientConnected>(playerSlot =>
        {
            CCSPlayerController player = Utilities.GetPlayerFromSlot(playerSlot);
            if (player == null) return;
            if (player.IsValid == false) return;
            if (player.UserId < 0) return;
            if (player.SteamID < 0) return;
            var tempSteamId = player.SteamID;
            var teampUserId = player.UserId;
            if (BanCheck(tempSteamId) == false)
            {
                Server.ExecuteCommand($"kickid {teampUserId}");
            }

            //if (player?.SteamID != null && player!.SteamID != 0)
            //{
            //    AddTimer(0.1f, () =>
            //    {
            //        AddOrUpdatePlayerToPlayerNameTable(player!.SteamID, player.PlayerName);
            //        _ = Task.Run(async () =>
            //        {
            //            await GetPlayerMarketData(player!.SteamID);
            //        });
            //        InsertAndGetTimeTrackingData(player.SteamID);
            //        GetPGagData(player.SteamID);
            //        InsertAndGetPlayerLevelData(player.SteamID, true);
            //        _ = Task.Run(() => CheckPlayerGroups(player));
            //    });
            //}
        });
    }
}