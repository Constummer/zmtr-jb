using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerDisconnect()
    {
        RegisterEventHandler<EventPlayerDisconnect>((@event, _) =>
        {
            try
            {
                var tempSteamId = @event?.Userid?.SteamID;
                var tempUserId = @event?.Userid?.UserId;
                var tempUserName = @event?.Userid?.PlayerName;
                if (tempSteamId.HasValue)
                {
                    LastBanDatas.Enqueue(new(tempSteamId.Value, tempUserName));
                }
                _ClientQueue.Enqueue(new(tempSteamId ?? 0, tempUserId, "", QueueItemType.OnClientDisconnect));
                return HookResult.Continue;
            }
            catch (Exception e)
            {
                ConsMsg(e.Message);
                return HookResult.Continue;
            }
        }, HookMode.Pre);
    }
}