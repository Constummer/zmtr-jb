using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerConnectFull()
    {
        var checkqueue = new Queue<Tuple<ulong, string, int?>>();
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
            if (player != null)
            {
                checkqueue.Enqueue(new Tuple<ulong, string, int?>(player.SteamID, player.PlayerName, player.UserId));
            }
            return HookResult.Continue;
        });
        AddTimer(1f, () =>
        {
            if (checkqueue.TryDequeue(out var player))
            {
                if (BanCheck(player.Item1) == false)
                {
                    Server.ExecuteCommand($"kickid {player.Item3}");
                }

                AddOrUpdatePlayerToPlayerNameTable(player.Item1, player.Item2);
                GetPlayerMarketData(player.Item1).Wait();
                InsertAndGetTimeTrackingData(player.Item1);
                GetPGagData(player.Item1);
                InsertAndGetPlayerLevelData(player.Item1, true, player.Item2);
                CheckPlayerGroups(player.Item1);
            }
        }, TimerFlags.REPEAT);
    }
}