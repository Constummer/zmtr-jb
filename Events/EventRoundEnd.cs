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
            //oyuncularýn rengi defaulta dönmeli
            Server.ExecuteCommand("mp_respawn_on_death_t 1");
            Server.ExecuteCommand("mp_respawn_on_death_ct 1");
            Server.ExecuteCommand("sv_enablebunnyhopping 1");
            Server.ExecuteCommand("sv_autobunnyhopping 1");
            Server.ExecuteCommand("sv_maxspeed 320");
            Server.ExecuteCommand("mp_teammates_are_enemies 0");

            GetPlayers()
            .Where(x => ValidateCallerPlayer(x, false)
                        && LatestWCommandUser != x.SteamID)
            .ToList()
            .ForEach(x =>
            {
                SetColour(x, Color.FromArgb(255, 255, 255, 255));
                RefreshPawn(x);
            });
            return HookResult.Continue;
        });
    }
}