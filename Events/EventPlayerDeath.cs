using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

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
            LRPlayerDeath(@event.Attacker, @event.Userid);

            return HookResult.Continue;
        }, HookMode.Pre);
    }
}