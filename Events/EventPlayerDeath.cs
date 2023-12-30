using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerDeath()
    {
        RegisterEventHandler<EventPlayerDeath>((@event, info) =>
        {
            if (@event == null)
                return HookResult.Continue;
            if (ValidateCallerPlayer(@event.Userid, false) == false)
            {
                return HookResult.Continue;
            }
            if (@event.Userid?.IsBot == true)
            {
                return HookResult.Continue;
            }
            if (@event.Attacker == null
                    || ((CEntityInstance)@event.Attacker).IsValid != true
                    || ((CEntityInstance)@event.Attacker).Index == 32767)
            {
                return HookResult.Continue;
            }

            //if (GetTeam(@event.Attacker) == CsTeam.CounterTerrorist)
            //{
            //    if (KilledPlayers.TryGetValue(@event.Attacker.SteamID, out var kilList))
            //    {
            //        kilList.TryAdd(@event.Userid.SteamID, @event.Userid.PlayerName);
            //    }
            //    else
            //    {
            //        KilledPlayers.TryAdd(@event.Attacker.SteamID, new Dictionary<ulong, string>()
            //        {
            //            { @event.Userid.SteamID, @event.Userid.PlayerName }
            //        });
            //    }
            //}
            if (GetTeam(@event.Userid) == CsTeam.Terrorist)
            {
                LastAliveTSound();
            }

            if (@event?.Attacker.UserId != @event?.Userid.UserId && GetTeam(@event?.Attacker!) != GetTeam(@event!.Userid))
            {
                AddCreditToAttacker(@event?.Attacker, GetTeam(@event!.Userid));
            }

            //if (ValidateCallerPlayer(@event?.Userid, false))
            //{
            //    Vector? currentPosition = @event?.Userid.PlayerPawn.Value!.AbsOrigin;
            //    if (currentPosition != null)
            //    {
            //        DeathLocations.TryAdd(@event!.Userid.SteamID, currentPosition);
            //    }
            //}
            Logger.LogInformation("f");
            if (@event?.Userid?.SteamID == LatestWCommandUser)
            {
                CoinRemove();
            }

            return HookResult.Continue;
        }, HookMode.Pre);
    }

    private void AddCreditToAttacker(CCSPlayerController? attacker, CsTeam teamNum)
    {
        if (ValidateCallerPlayer(attacker, false) == false)
        {
            return;
        }
        if (attacker?.SteamID != null && attacker.SteamID != 0)
        {
            var amount = teamNum switch
            {
                CsTeam.Terrorist => Config.RetrieveCreditEveryTKill,
                CsTeam.CounterTerrorist => Config.RetrieveCreditEveryCTKill,
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