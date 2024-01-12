using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerConnectFull()
    {
        //disabled, was causing crash
        RegisterEventHandler<EventPlayerConnectFull>((@event, _) =>
        {
            if (@event == null) return HookResult.Continue;
            if (@event.Userid == null) return HookResult.Continue;
            if (@event.Userid.IsValid == false) return HookResult.Continue;
            if (@event.Userid.SteamID == 0) return HookResult.Continue;
            if (ValidateCallerPlayer(@event.Userid, false) == false) return HookResult.Continue;
            var tempSteamId = @event.Userid.SteamID;
            var tempPlayerName = @event.Userid.PlayerName;

            AddOrUpdatePlayerToPlayerNameTable(tempSteamId, tempPlayerName);
            GetPlayerMarketData(tempSteamId).Wait();
            InsertAndGetTimeTrackingData(tempSteamId);
            GetPGagData(tempSteamId);
            InsertAndGetPlayerLevelData(tempSteamId, true, tempPlayerName);
            CheckPlayerGroups(tempSteamId);
            return HookResult.Continue;
        });
    }
}