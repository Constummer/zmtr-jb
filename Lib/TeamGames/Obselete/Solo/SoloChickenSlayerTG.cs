using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloChickenSlayerTG : TeamGamesGameBase
    {
        public Dictionary<ulong, ChickenKiller> ChickenKillCount { get; set; } = new();
        public CounterStrikeSharp.API.Modules.Timers.Timer? PrintTimer { get; set; } = null;
        public CounterStrikeSharp.API.Modules.Timers.Timer? GameTimer { get; set; } = null;

        public SoloChickenSlayerTG() : base(TeamGamesSoloChoices.ChickenSlayer)
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

                var kazananPlayer = GetPlayers(CsTeam.Terrorist).Where(x => x.SteamID == kazanan.Key).FirstOrDefault();
                if (kazananPlayer != null)
                {
                    Server.PrintToChatAll($"{Prefix} {kazananPlayer.PlayerName} {CC.W} tekli oyunu kazandı.");
                    PrintToCenterHtmlAll($"{Prefix} {kazananPlayer.PlayerName} {CC.W}tekli oyunu kazandı.");

                    GetPlayers(CsTeam.Terrorist)
                    .Where(x => x.PawnIsAlive && x.SteamID != kazanan.Key)
                    .ToList()
                    .ForEach(x => x.CommitSuicide(false, true));
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
                    if (ChickenKillCount.TryGetValue(x.SteamID, out var val))
                    {
                        ++val.Count;
                        ChickenKillCount[x.SteamID] = val;
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
                        .Select(x => $"{x.Value.Pname} - {x.Value.Count}"));
            return str;
        }
    }
}