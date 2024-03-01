using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventBombDropped()
    {
        //RegisterEventHandler<EventPlayerBlind>((@event, _) =>
        //{
        //    Server.PrintToChatAll($"Player {@event.Entityid} - {@event.Attacker.PlayerName} flashed {@event.Userid.PlayerName} for {@event.BlindDuration.ToString("0.00")}s");
        //    return HookResult.Continue;
        //});
        //RegisterEventHandler<EventPlayerSound>((@event, _) =>
        //{
        //    Server.PrintToConsole($"Player {@event.Userid.PlayerName};EventName {@event.EventName};Duration {@event.Duration};Step {@event.Step};Radius {@event.Radius};Handle {@event.Handle}");

        //    return HookResult.Continue;
        //});
        RegisterEventHandler<EventBombDropped>((@event, _) =>
        {
            ActiveTeamGamesGameBase?.EventBombDropped(@event);
            return HookResult.Continue;
        });
    }
}