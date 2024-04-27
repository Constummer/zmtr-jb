using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, Dictionary<ulong, string>> KilledPlayers = new();

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
            if (@event?.Userid?.SteamID != null && @event?.Userid?.SteamID != 0)
            {
                RemoveCurrentParticle(@event.Userid.SteamID);
                SutolCommandCalls?.Remove(@event.Userid.SteamID);
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
            if (LatestWCommandUser.HasValue
                && @event?.Userid?.SteamID == LatestWCommandUser)
            {
                if (GetPlayerCount() > 16)
                {
                    var team = @event?.Attacker?.Team;
                    var steamId = @event?.Attacker?.SteamID;
                    if (team != null && steamId != null)
                    {
                        IsTopPlayerDeath(team, steamId);
                    }
                }
            }
            if (cDeathActive)
            {
                if (AdminManager.PlayerHasPermissions(@event.Attacker, "@css/root"))
                {
                    Server.PrintToChatAll($"EventName=\t\t{@event.EventName}");
                    Server.PrintToChatAll($"Noreplay=\t\t{@event.Noreplay}");
                    Server.PrintToChatAll($"Assistedflash=\t\t{@event.Assistedflash}");
                    Server.PrintToChatAll($"DmgArmor=\t\t{@event.DmgArmor}");
                    Server.PrintToChatAll($"Attackerblind=\t\t{@event.Attackerblind}");
                    Server.PrintToChatAll($"Attackerinair=\t\t{@event.Attackerinair}");
                    Server.PrintToChatAll($"Distance=\t\t{@event.Distance}");
                    Server.PrintToChatAll($"DmgArmor=\t\t{@event.DmgArmor}");
                    Server.PrintToChatAll($"DmgHealth=\t\t{@event.DmgHealth}");
                    Server.PrintToChatAll($"Dominated=\t\t{@event.Dominated}");
                    Server.PrintToChatAll($"Headshot=\t\t{@event.Headshot}");
                    Server.PrintToChatAll($"Hitgroup=\t\t{@event.Hitgroup}");
                    Server.PrintToChatAll($"Noscope=\t\t{@event.Noscope}");
                    Server.PrintToChatAll($"Penetrated=\t\t{@event.Penetrated}");
                    Server.PrintToChatAll($"Revenge=\t\t{@event.Revenge}");
                    Server.PrintToChatAll($"Thrusmoke=\t\t{@event.Thrusmoke}");
                    Server.PrintToChatAll($"Weapon=\t\t{@event.Weapon}");
                    Server.PrintToChatAll($"WeaponFauxitemid=\t\t{@event.WeaponFauxitemid}");
                    Server.PrintToChatAll($"WeaponItemid=\t\t{@event.WeaponItemid}");
                    Server.PrintToChatAll($"WeaponOriginalownerXuid=\t\t{@event.WeaponOriginalownerXuid}");
                    Server.PrintToChatAll($"Wipe=\t\t{@event.Wipe}");
                }
            }
            BattlePassBase.EventPlayerDeath(@event);

            return HookResult.Continue;
        }, HookMode.Pre);

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

            ActiveTeamGamesGameBase?.EventPlayerDeath(@event);

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

            return HookResult.Continue;
        }, HookMode.Post);
    }
}