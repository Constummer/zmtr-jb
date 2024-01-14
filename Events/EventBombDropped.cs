using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventBombDropped()
    {
        RegisterEventHandler<EventPlayerBlind>((@event, _) =>
        {
            Server.PrintToChatAll($"Player {@event.Entityid} - {@event.Attacker.PlayerName} flashed {@event.Userid.PlayerName} for {@event.BlindDuration.ToString("0.00")}s");
            return HookResult.Continue;
        });
        //RegisterEventHandler<EventFlashbangDetonate>((@event, _) =>
        //{
        //    Server.PrintToChatAll($"Player {@event.Userid.PlayerName} flashed {new Vector(@event.X, @event.Y,
        //        @event.Z).ToString()}");
        //    return HookResult.Continue;
        //});
        //RegisterEventHandler<EventBombDropped>((@event, _) =>
        //{
        //    ActiveTeamGamesGameBase?.EventBombDropped(@event);
        //    return HookResult.Continue;
        //});
    }
}