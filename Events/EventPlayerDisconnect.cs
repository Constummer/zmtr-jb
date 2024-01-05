using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerDisconnect()
    {
        //disabled, was causing crash
        RegisterEventHandler<EventPlayerDisconnect>((@event, _) =>
        {
            if (@event == null)
                return HookResult.Continue;
            if (ValidateCallerPlayer(@event.Userid, false) == false)
            {
                return HookResult.Continue;
            }
            ClearOnDisconnect(@event.Userid.SteamID, @event.Userid.UserId);
            //if (@event?.Userid?.SteamID == LatestWCommandUser)
            //{
            //    CoinRemove();
            //}
            return HookResult.Continue;
        });
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
            InsertAndGetPlayerLevelData(player.SteamID, true);
            CheckPlayerGroups(player);
            return HookResult.Continue;
        });
    }
}