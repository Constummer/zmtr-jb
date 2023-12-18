using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventRoundEnd()
    {
        RegisterEventHandler<EventRoundEnd>((@event, _) =>
        {
            PrepareRoundDefaults();

            return HookResult.Continue;
        });
    }

    private static void PrepareRoundDefaults()
    {
        Server.ExecuteCommand("mp_respawn_on_death_t 1");
        Server.ExecuteCommand("mp_respawn_on_death_ct 1");
        Server.ExecuteCommand("sv_enablebunnyhopping 1");
        Server.ExecuteCommand("sv_autobunnyhopping 1");
        Server.ExecuteCommand("sv_maxspeed 320");
        Server.ExecuteCommand("mp_teammates_are_enemies 0");
        Server.ExecuteCommand("player_ping_token_cooldown 1");

        GetPlayers()
         .Where(x => ValidateCallerPlayer(x, false))
         .ToList()
         .ForEach(x =>
         {
             SetColour(x, Color.FromArgb(255, 255, 255, 255));
             RefreshPawn(x);
         });
    }
}