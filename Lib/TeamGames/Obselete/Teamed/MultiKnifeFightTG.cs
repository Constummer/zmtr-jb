﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiKnifeFightTG : TeamGamesGameBase
    {
        public Dictionary<int, int> PlayerCount { get; set; } = new();

        public MultiKnifeFightTG() : base(TeamGamesMultiChoices.KnifeFight)
        {
        }

        internal override void StartGame(Action callback)
        {
            RemoveAllWeapons(true);
            PlayerCount = GetTeamPlayerCounts();
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            PlayerCount?.Clear();
            base.Clear(printMsg);
        }

        internal override void EventPlayerDeath(EventPlayerDeath @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Userid, false) == false) return;

            var team = FindTeam(@event.Userid.SteamID);
            if (team.Index == -1) return;
            if (PlayerCount.ContainsKey(team.Index))
            {
                PlayerCount[team.Index]--;
                var otherTeamIndex = (team.Index + 1) % 2;

                if (PlayerCount[team.Index] <= 0)
                {
                    var otherTeam = GetTeamColorAndTextByIndex(otherTeamIndex);
                    if (otherTeam.Msg == null) return;

                    Server.PrintToChatAll($"{Prefix} {otherTeam.Msg} {CC.W}takım kazandı.");
                    PrintToCenterHtmlAll($"{Prefix} {otherTeam.Msg} {CC.W}takım kazandı.");
                    Clear(true);
                }
            }

            base.EventPlayerDeath(@event);
        }
    }
}