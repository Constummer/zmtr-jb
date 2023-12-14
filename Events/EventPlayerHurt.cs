using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal void EventPlayerHurt()
    {
        RegisterEventHandler<EventPlayerHurt>((@event, info) =>
        {
            if (@event.Userid.IsBot == true)
            {
                return HookResult.Continue;
            }
            CCSPlayerController? player = @event.Userid;

            if (ActiveGodMode.TryGetValue(@event.Userid.SteamID, out var value))
            {
                if (value)
                {
                    player.Health = 100;
                    player.PlayerPawn.Value!.Health = 100;
                }
            }

            return HookResult.Continue;
        }, HookMode.Post);
    }
}