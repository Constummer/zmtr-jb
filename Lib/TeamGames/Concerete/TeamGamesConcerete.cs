﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class TeamGamesGameBase
    {
        public TeamGamesGameBase(TeamGamesMultiChoices multiChoice)
        {
            MultiChoice = multiChoice;
        }

        public TeamGamesGameBase(TeamGamesSoloChoices soloChoice)
        {
            SoloChoice = soloChoice;
        }

        public bool FfActive { get; set; } = true;
        public bool HasAdditionalChoices { get; set; } = false;
        public TeamGamesMultiChoices MultiChoice { get; set; } = TeamGamesMultiChoices.None;
        public TeamGamesSoloChoices SoloChoice { get; set; } = TeamGamesSoloChoices.None;
        public string GameName { get; set; } = "";

        internal void SoloCheckGameFinished(TeamGamesGameBase caller, ulong steamid, List<ulong> playerCount, string attackerPname, Action? action = null)
        {
            playerCount.RemoveAll(x => x == steamid);

            if (action != null)
            {
                action();
            }

            if (playerCount.Count <= 1)
            {
                if (attackerPname == null)
                {
                    attackerPname = GetPlayers()
                                        .Where(x => playerCount.Contains(x.SteamID))
                                        .Select(x => x.PlayerName)
                                        .FirstOrDefault()!;
                }

                if (attackerPname != null)
                {
                    Server.PrintToChatAll($"{Prefix} {CC.Or}{attackerPname}{CC.W} adlı {T_AllLower} kazandı.");
                    PrintToCenterHtmlAll($"{Prefix} {attackerPname} adlı {T_AllLower} kazandı.");
                }

                caller.Clear(true);
            }
        }

        internal void MultiCheckGameFinished(TeamGamesGameBase caller, ulong steamid, Dictionary<int, int> playerCount, Action action = null)
        {
            var team = FindTeam(steamid);
            if (team.Index == -1) return;
            if (playerCount.ContainsKey(team.Index))
            {
                playerCount[team.Index]--;
                var otherTeamIndex = (team.Index + 1) % 2;
                if (action != null)
                {
                    action();
                }
                if (playerCount[team.Index] <= 0)
                {
                    var otherTeam = GetTeamColorAndTextByIndex(otherTeamIndex);
                    if (otherTeam.Msg == null) return;

                    Server.PrintToChatAll($"{Prefix} {otherTeam.Msg} {CC.W}takım kazandı.");
                    PrintToCenterHtmlAll($"{Prefix} {otherTeam.Msg} {CC.W}takım kazandı.");
                    caller.Clear(true);
                }
            }
        }

        internal virtual void StartGame(Action callback)
        {
            callback();
        }

        internal virtual void Clear(bool printMsg)
        {
            if (string.IsNullOrWhiteSpace(GameName))
            {
                return;
            }
            TakimBozAction();
            if (printMsg)
            {
                if (MultiChoice != TeamGamesMultiChoices.None)
                {
                    Server.PrintToChatAll($"{Prefix} {CC.B}{GameName} {CC.Ol}takım {CC.W}oyunu {CC.R}kapatıldı.");
                }
                else
                {
                    Server.PrintToChatAll($"{Prefix}  {CC.B}{GameName} {CC.Ol}tekli {CC.W}oyunu {CC.R}kapatıldı.");
                }
            }
            TgActive = false;
            MarketEnvDisable = false;
            TgTimer?.Kill();
            TgTimer = null;
            if (ActiveTeamGamesGameBase != null)
            {
                Server.ExecuteCommand("mp_teammates_are_enemies 0");
            }
            ActiveTeamGamesGameBase = null;
        }

        internal virtual void AdditionalChoiceMenu(CCSPlayerController player, Action value)
        {
            value();
        }

        internal virtual void EventBombDropped(EventBombDropped @event)
        {
        }

        internal virtual void EventEntityKilled(EventEntityKilled @event)
        {
        }

        internal virtual void EventBombPickup(EventBombPickup @event)
        {
        }

        internal virtual void EventPlayerDeath(EventPlayerDeath @event)
        {
        }

        internal virtual void EventItemPickup(EventItemPickup @event)
        {
        }

        internal virtual void EventWeaponZoom(EventWeaponZoom @event)
        {
        }

        internal virtual void OnTakeDamageHook(CEntityInstance ent, CEntityInstance activator)
        {
        }

        internal virtual void EventWeaponFire(EventWeaponFire @event)
        {
        }

        internal virtual void EventPlayerDisconnect(ulong? tempSteamId)
        {
        }

        internal virtual void EventPlayerHurt(EventPlayerHurt @event)
        {
        }

        internal virtual HookResult OnWeaponCanAcquire(CCSPlayerController client, string weaponName) => HookResult.Continue;
    }
}