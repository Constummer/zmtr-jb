using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerConnectFull()
    {
        //disabled, was causing crash
        RegisterEventHandler<EventPlayerConnectFull>((@event, _) =>
        {
            if (@event == null)
                return HookResult.Continue;
            if (ValidateCallerPlayer(@event.Userid, false) == false)
            {
                return HookResult.Continue;
            }
            var player = @event.Userid;

            AddOrUpdatePlayerToPlayerNameTable(player!.SteamID, player.PlayerName);
            GetPlayerMarketData(player!.SteamID).Wait();
            InsertAndGetTimeTrackingData(player.SteamID);
            GetPGagData(player.SteamID);
            InsertAndGetPlayerLevelData(player.SteamID, true, player.PlayerName);
            CheckPlayerGroups(player);
            return HookResult.Continue;
        });
    }
}