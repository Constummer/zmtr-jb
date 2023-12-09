using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventRoundStart()
    {
        RegisterEventHandler<EventRoundStart>((@event, _) =>
        {
            Countdown_enable_text = false;
            Countdown_enable = false;
            timer_2?.Kill();
            timer_1?.Kill();
            ActiveGodMode.Clear();
            DeathLocations.Clear();
            KilledPlayers.Clear();
            return HookResult.Continue;
        });
    }

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

    private void EventPlayerHurt()
    {
        RegisterEventHandler<EventPlayerHurt>((@event, info) =>
        {
            if (@event.Userid.IsBot == true)
            {
                return HookResult.Continue;
            }

            Logger.LogInformation(@event.Userid.SteamID.ToString());
            if (ActiveGodMode.ContainsKey(@event.Userid.SteamID))
            {
                Logger.LogInformation("true");
                return HookResult.Handled;
            }

            return HookResult.Continue;
        }, HookMode.Pre);
    }
}