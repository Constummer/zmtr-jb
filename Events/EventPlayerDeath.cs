using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerDeath()
    {
        RegisterEventHandler<EventPlayerDeath>((@event, info) =>
        {
            if (@event.Userid.IsBot == true)
            {
                return HookResult.Continue;
            }

            if (@event.Attacker != null
                && (CsTeam)@event.Attacker.TeamNum == CsTeam.CounterTerrorist)
            {
                if (KilledPlayers.TryGetValue(@event.Attacker.SteamID, out var kilList))
                {
                    kilList.TryAdd(@event.Userid.SteamID, @event.Userid.PlayerName);
                }
                else
                {
                    KilledPlayers.TryAdd(@event.Attacker.SteamID, new Dictionary<ulong, string>()
                    {
                        { @event.Userid.SteamID, @event.Userid.PlayerName }
                    });
                }
            }
            Vector currentPosition = @event?.Userid?.Pawn?.Value?.CBodyComponent?.SceneNode?.AbsOrigin;
            if (currentPosition != null)
            {
                DeathLocations.TryAdd(@event.Userid.SteamID, currentPosition);
            }

            return HookResult.Continue;
        }, HookMode.Pre);
    }
}