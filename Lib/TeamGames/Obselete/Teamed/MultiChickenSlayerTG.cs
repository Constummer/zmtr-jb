using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiChickenSlayerTG : TeamGamesGameBase
    {
        public Dictionary<int, ChickenKiller> ChickenKillCount { get; set; } = new();
        public CounterStrikeSharp.API.Modules.Timers.Timer? PrintTimer { get; set; } = null;
        public CounterStrikeSharp.API.Modules.Timers.Timer? GameTimer { get; set; } = null;

        public MultiChickenSlayerTG() : base(TeamGamesSoloChoices.ChickenSlayer)
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
                SpawnChicken(x?.PlayerPawn?.Value?.AbsOrigin ?? x?.Pawn?.Value?.AbsOrigin ?? x?.AbsOrigin ?? VEC_ZERO);
            }
            PrintTimer = Global?.AddTimer(0.1f, () =>
            {
                PrintToCenterHtmlAll(GetFormattedPrintData());
                PrintToCenterHtmlAll(GetFormattedPrintData());
                PrintToCenterHtmlAll(GetFormattedPrintData());
                PrintToCenterHtmlAll(GetFormattedPrintData());
            }, Full);
            GameTimer = Global?.AddTimer(30f, () =>
            {
                var kazanan = ChickenKillCount
                        .ToList()
                        .OrderByDescending(x => x.Value.Count)
                        .FirstOrDefault();

                var kazananTeam = GetTeamColorAndTextByIndex(kazanan.Key);

                if (kazananTeam.Msg == null) return;

                Server.PrintToChatAll($"{Prefix} {kazananTeam.Msg} {CC.W}takım kazandı.");
                PrintToCenterHtmlAll($"{Prefix} {kazananTeam.Msg} {CC.W}takım kazandı.");

                var otherTeamIndex = (kazanan.Key + 1) % 2;

                var otherTeam = GetTeamColorAndTextByIndex(otherTeamIndex);
                if (otherTeam.Msg != null)
                {
                    if (TeamSteamIds.TryGetValue(otherTeamIndex, out var teamIds))
                    {
                        GetPlayers(CsTeam.Terrorist)
                        .Where(x => x.PawnIsAlive && teamIds.Contains(x.SteamID))
                        .ToList()
                        .ForEach(x => x.CommitSuicide(false, true));
                    }
                }

                Clear(true);
            });

            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            ChickenKillCount?.Clear();
            PrintTimer?.Kill();
            PrintTimer = null;
            GameTimer?.Kill();
            GameTimer = null;
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
                    var team = FindTeam(x.SteamID);
                    if (team.Index == -1) return;

                    if (ChickenKillCount.TryGetValue(team.Index, out var val))
                    {
                        ++val.Count;
                        ChickenKillCount[team.Index] = val;
                        SpawnChicken(x?.PlayerPawn?.Value?.AbsOrigin ?? x?.Pawn?.Value?.AbsOrigin ?? x?.AbsOrigin ?? VEC_ZERO);
                        PrintToCenterHtmlAll(GetFormattedPrintData());
                    }
                    else
                    {
                        val = new ChickenKiller(team.Msg, 1);
                        ChickenKillCount[team.Index] = val;
                        SpawnChicken(x?.PlayerPawn?.Value?.AbsOrigin ?? x?.Pawn?.Value?.AbsOrigin ?? x?.AbsOrigin ?? VEC_ZERO);
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
                        .Select(x => $"{x.Value.Pname}{CC.W} - {x.Value.Count}"));
            return str;
        }
    }
}