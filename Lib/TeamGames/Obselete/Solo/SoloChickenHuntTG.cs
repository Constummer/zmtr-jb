using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloChickenHuntTG : TeamGamesGameBase
    {
        public Dictionary<ulong, ChickenKiller> ChickenKillCount { get; set; } = new();

        public SoloChickenHuntTG() : base(TeamGamesSoloChoices.ChickenHunt)
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
                ChickenKillCount.Add(x.SteamID, new(x.PlayerName, 0));
                if (ValidateCallerPlayer(x, false) == false) continue;
                SpawnChicken(x?.PlayerPawn?.Value?.AbsOrigin ?? x?.Pawn?.Value?.AbsOrigin ?? x?.AbsOrigin ?? VEC_ZERO);
            }

            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            ChickenKillCount?.Clear();
            base.Clear(printMsg);
        }

        internal override void OnTakeDamageHook(CEntityInstance ent, CEntityInstance activator)
        {
            if (ent != null && ent.IsValid)
            {
                var x = GetPlayers().ToList().Where(x =>
                       activator.Index == x.PlayerPawn.Index ||
                       activator.Index == x.PlayerPawn.Value.Index ||
                       activator.Index == x.Pawn.Index ||
                       activator.Index == x.Pawn.Value.Index).FirstOrDefault();
                if (x != null && x.IsValid)
                {
                    if (ValidateCallerPlayer(x, false) == false) return;
                    if (ChickenKillCount.TryGetValue(x.SteamID, out var val))
                    {
                        val.Count++;
                        ChickenKillCount[x.SteamID] = val;
                        //SpawnChicken(x?.PlayerPawn?.Value?.AbsOrigin ?? x?.Pawn?.Value?.AbsOrigin ?? x?.AbsOrigin ?? VEC_ZERO);
                        PrintToCenterHtmlAll(GetFormattedPrintData());
                    }
                }
            }

            base.OnTakeDamageHook(ent, activator);
        }

        private string GetFormattedPrintData()
        {
            var str = string.Join(" <br> ",
                    ChickenKillCount
                        .ToList()
                        .OrderByDescending(x => x.Value.Count)
                        .Select(x => $"{x.Value.Pname} - {x.Value.Count}"));
            return str;
        }
    }
}