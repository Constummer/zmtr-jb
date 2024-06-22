using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloChickenRouletteTG : TeamGamesGameBase
    {
        public uint LuckyChickenIndex { get; set; } = 0;

        public SoloChickenRouletteTG() : base(TeamGamesSoloChoices.ChickenRoulette)
        {
            FfActive = false;
        }

        internal override void StartGame(Action callback)
        {
            var players = GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive).ToList();
            RemoveAllWeapons(giveKnife: true);

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

        internal override void Clear(bool printMsg)
        {
            LuckyChickenIndex = 0;
            base.Clear(printMsg);
        }

        internal override void OnTakeDamageHook(CEntityInstance ent, CEntityInstance activator)
        {
            if (ent != null && ent.IsValid && ent.Index == LuckyChickenIndex)
            {
                var attacker = GetPlayers().ToList().Where(x =>
                       activator.Index == x.PlayerPawn.Index ||
                       activator.Index == x.PlayerPawn.Value.Index ||
                       activator.Index == x.Pawn.Index ||
                       activator.Index == x.Pawn.Value.Index).FirstOrDefault();
                if (attacker != null && attacker.IsValid)
                {
                    if (ValidateCallerPlayer(attacker, false) == false) return;
                    Server.PrintToChatAll($"{Prefix} {CC.Or} {attacker.PlayerName}{CC.W} adlı {T_AllLower} kazandı.");

                    GetPlayers(CsTeam.Terrorist)
                    .Where(x => x.PawnIsAlive && x.SteamID != attacker.SteamID)
                    .ToList()
                    .ForEach(x => x.CommitSuicide(false, true));
                    Clear(false);
                }
            }

            base.OnTakeDamageHook(ent, activator);
        }
    }
}