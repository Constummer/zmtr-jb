using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiChickenRouletteTG : TeamGamesGameBase
    {
        public uint LuckyChickenIndex { get; set; } = 0;

        public MultiChickenRouletteTG() : base(TeamGamesMultiChoices.ChickenRoulette)
        {
        }

        internal override void StartGame(Action callback)
        {
            var players = GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive).ToList();

            List<uint> cindexes = new List<uint>();
            foreach (var x in players)
            {
                if (ValidateCallerPlayer(x, false) == false) continue;
                var chickenIndex = SpawnChicken(x?.PlayerPawn?.Value?.AbsOrigin ?? x?.Pawn?.Value?.AbsOrigin ?? x?.AbsOrigin ?? VEC_ZERO);
                if (chickenIndex > 0)
                {
                    cindexes.Add(chickenIndex);
                }
            }
            LuckyChickenIndex = cindexes.Skip(_random.Next(players.Count())).FirstOrDefault();

            base.StartGame(callback);
        }

        internal override void Clear()
        {
            LuckyChickenIndex = 0;
            base.Clear();
        }

        internal override void EventEntityKilled(EventEntityKilled @event)
        {
            if (@event != null && @event.EntindexKilled == LuckyChickenIndex)
            {
                if (@event.EntindexAttacker <= 0
                    || @event.EntindexAttacker == 32767)
                {
                    return;
                }
                if (@event.EntindexAttacker > int.MaxValue)
                {
                    return;
                }
                var attacker = Utilities.GetPlayerFromIndex((int)@event.EntindexAttacker);
                if (attacker != null)
                {
                    if (ValidateCallerPlayer(attacker, false) == false) return;

                    var team = FindTeam(attacker.SteamID);
                    if (team.Msg == null)
                    {
                        Server.PrintToChatAll($"{Prefix} {CC.W} hiçbir takımda olmayan mahkûm kazandi = {attacker.PlayerName}.");
                    }
                    else
                    {
                        Server.PrintToChatAll($"{Prefix} {team.Msg} {CC.W}takım şanslı tavuğu buldu.");
                    }
                }
            }
            base.EventEntityKilled(@event);
        }
    }
}