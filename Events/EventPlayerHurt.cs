using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

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
            if (LrActive == false)
            {
                if (ActiveGodMode.TryGetValue(@event.Userid.SteamID, out var value))
                {
                    if (value)
                    {
                        player.Health = 100;
                        player.PlayerPawn.Value!.Health = 100;
                        if (player.PawnArmor != 0)
                        {
                            player.PawnArmor = 100;
                        }
                        if (player.PlayerPawn.Value!.ArmorValue != 0)
                        {
                            player.PlayerPawn.Value!.ArmorValue = 100;
                        }
                    }
                }
            }

            return HookResult.Continue;
        }, HookMode.Post);
    }
}