using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private const int EveryCTKill = 5;//her CT oldurme
    private const int EveryTKill = 1;//her T oldurme

    private void EventPlayerDeath()
    {
        RegisterEventHandler<EventPlayerDeath>((@event, info) =>
        {
            if (@event.Userid.IsBot == true)
            {
                return HookResult.Continue;
            }

            if (@event.Attacker != null
                    && ((CEntityInstance)@event.Attacker).IsValid == true
                    && ((CEntityInstance)@event.Attacker).Index != 32767)
            {
                if (GetTeam(@event.Attacker) == CsTeam.CounterTerrorist)
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
                if (@event?.Attacker.UserId != @event?.Userid.UserId)
                {
                    AddCreditToAttacker(@event?.Attacker, GetTeam(@event.Userid));
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

    private void AddCreditToAttacker(CCSPlayerController? attacker, CsTeam teamNum)
    {
        if (ValidateCallerPlayer(attacker) == false)
        {
            return;
        }
        if (attacker?.SteamID != null && attacker.SteamID != 0)
        {
            var amount = teamNum switch
            {
                CsTeam.Terrorist => EveryTKill,
                CsTeam.CounterTerrorist => EveryCTKill,
                _ => 0
            };

            if (PlayerMarketModels.TryGetValue(attacker!.SteamID, out var item))
            {
                item.Credit += amount;
            }
            else
            {
                item = new(attacker!.SteamID);
                item.Credit = amount;
            }
            PlayerMarketModels[attacker!.SteamID] = item;

            var teamShortName = teamNum switch
            {
                CsTeam.Terrorist => "T",
                CsTeam.CounterTerrorist => "CT",
                _ => ""
            };
            attacker!.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.LightBlue}{teamShortName} Oldurdugun için, {amount} kredi kazandýn!");
        }
    }
}