namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnClientConnected()
    {
        //RegisterListener<Listeners.OnClientConnected>(playerSlot =>
        //{
        //    CCSPlayerController player = Utilities.GetPlayerFromSlot(playerSlot);
        //    _ClientConnectQueue.Enqueue(new() { SteamId = player?.SteamID ?? 0 });

        //    //if (ValidateCallerPlayer(player, false) == false) return;
        //    //var tempSteamId = player.SteamID;
        //    //var teampUserId = player.UserId;
        //    //if (BanCheck(tempSteamId) == false)
        //    //{
        //    //    if (ValidateCallerPlayer(player, false) == false) return;
        //    //    Server.ExecuteCommand($"kickid {teampUserId}");
        //    //}

        //    //if (player?.SteamID != null && player!.SteamID != 0)
        //    //{
        //    //    AddTimer(0.1f, () =>
        //    //    {
        //    //        AddOrUpdatePlayerToPlayerNameTable(player!.SteamID, player.PlayerName);
        //    //        _ = Task.Run(async () =>
        //    //        {
        //    //            await GetPlayerMarketData(player!.SteamID);
        //    //        });
        //    //        InsertAndGetTimeTrackingData(player.SteamID);
        //    //        GetPGagData(player.SteamID);
        //    //        InsertAndGetPlayerLevelData(player.SteamID, true);
        //    //        _ = Task.Run(() => CheckPlayerGroups(player));
        //    //    });
        //    //}
        //});
    }
}