using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
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

            if (GetTeam(@event.Userid) == CsTeam.Terrorist)
            {
                LastAliveTSound();
            }

            if (ValidateCallerPlayer(@event?.Userid, false))
            {
                Vector? currentPosition = @event?.Userid.PlayerPawn.Value!.AbsOrigin;
                if (currentPosition != null)
                {
                    DeathLocations.TryAdd(@event!.Userid.SteamID, currentPosition);
                }
            }
            if (@event?.Userid?.SteamID == LatestWCommandUser)
            {
                CoinRemove();
            }
            if (@event.Userid.UserId != null && @event.Userid.UserId != -1)
            {
                RemoveGivenParachute(@event.Userid.UserId.Value);
            }
            //-------ATTACKER RELEATED THINGS MUST BE UNDER THIS IF AFTER THIS CHECK---------
            //-------ATTACKER RELEATED THINGS MUST BE UNDER THIS IF AFTER THIS CHECK---------
            //-------ATTACKER RELEATED THINGS MUST BE UNDER THIS IF AFTER THIS CHECK---------
            //-------ATTACKER RELEATED THINGS MUST BE UNDER THIS IF AFTER THIS CHECK---------
            //-------ATTACKER RELEATED THINGS MUST BE UNDER THIS IF AFTER THIS CHECK---------
            //-------ATTACKER RELEATED THINGS MUST BE UNDER THIS IF AFTER THIS CHECK---------
            if (@event.Attacker == null
                    || ((CEntityInstance)@event.Attacker).IsValid != true
                    || ((CEntityInstance)@event.Attacker).Index == 32767)
            {
                return HookResult.Continue;
            }

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

            if (@event?.Attacker.UserId != @event?.Userid.UserId && GetTeam(@event?.Attacker!) != GetTeam(@event!.Userid))
            {
                if (GetPlayerCount() > 8)
                {
                    AddCreditToAttacker(@event?.Attacker, GetTeam(@event!.Userid));
                }
            }

            return HookResult.Continue;
        }, HookMode.Pre);
    }

    private void AddCreditToAttacker(CCSPlayerController? attacker, CsTeam victimTeamNo)
    {
        if (ValidateCallerPlayer(attacker, false) == false)
        {
            return;
        }
        if (attacker?.SteamID != null && attacker.SteamID != 0)
        {
            var amount = victimTeamNo switch
            {
                CsTeam.Terrorist => Config.Credit.RetrieveCreditEveryTKill,
                CsTeam.CounterTerrorist => Config.Credit.RetrieveCreditEveryCTKill,
                _ => 0
            };
            if (amount <= 0)
            {
                return;
            }
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

            var teamShortName = victimTeamNo switch
            {
                CsTeam.Terrorist => "T",
                CsTeam.CounterTerrorist => "CT",
                _ => ""
            };
            attacker!.PrintToChat($"{Prefix} {CC.LB}{teamShortName} Oldurdugun için, {amount} kredi kazandýn!");
        }
    }
}