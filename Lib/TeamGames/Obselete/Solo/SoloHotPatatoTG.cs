using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloHotPatatoTG : TeamGamesGameBase
    {
        public CounterStrikeSharp.API.Modules.Timers.Timer? C4HitTimer { get; set; } = null;
        public CounterStrikeSharp.API.Modules.Timers.Timer? C4HitIncreaseTimer { get; set; } = null;
        public int C4CarierHitDamage { get; set; } = 0;

        public CCSPlayerController BombCarrier { get; set; } = null;
        public CBasePlayerWeapon C4 { get; set; } = null;

        public SoloHotPatatoTG() : base(TeamGamesSoloChoices.HotPatato)
        {
            FfActive = false;
        }

        internal override void StartGame(Action callback)
        {
            RemoveAllWeapons(giveKnife: false);

            var players = GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive);
            BombCarrier = GetRandomBombCarrier(players);
            BombCarrier.GiveNamedItem("weapon_c4");
            C4 = GetWeapon(BombCarrier, "weapon_c4");
            HotPatatoTimerStart();
            base.StartGame(callback);
        }

        internal override void EventPlayerDeath(EventPlayerDeath @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Userid, false) == false) return;

            var players2 = GetPlayers(CsTeam.Terrorist)
                .Where(x => x.SteamID != @event.Userid.SteamID && x.PawnIsAlive).ToList();

            if (players2.Count == 1)
            {
                var winner = players2.FirstOrDefault();
                if (ValidateCallerPlayer(winner, false) == false) return;
                Server.PrintToChatAll($"{Prefix} {CC.Or} {winner.PlayerName}{CC.W} adlı mahkûm kazandı.");
                PrintToCenterHtmlAll($"{Prefix} {winner.PlayerName} adlı mahkûm kazandı.");
                Clear(true);
            }
            else
            {
                if (C4 != null && (C4?.IsValid ?? false))
                {
                    C4.Remove();
                }
                var players = GetPlayers(CsTeam.Terrorist).Where(x => x.SteamID != @event.Userid.SteamID && x.PawnIsAlive);
                BombCarrier = GetRandomBombCarrier(players);
                BombCarrier.GiveNamedItem("weapon_c4");

                C4 = GetWeapon(BombCarrier, "weapon_c4");

                HotPatatoTimerStart();
            }
            base.EventPlayerDeath(@event);
        }

        internal override void Clear(bool printMsg)
        {
            if (C4 != null && (C4?.IsValid ?? false))
            {
                C4.Remove();
            }
            C4CarierHitDamage = 0;
            C4HitTimer?.Kill();
            C4HitIncreaseTimer?.Kill();
            C4HitTimer = null;
            C4HitIncreaseTimer = null;
            BombCarrier = null;
            base.Clear(printMsg);
        }

        internal override void EventBombPickup(EventBombPickup @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Userid, false) == false) return;

            if (BombCarrier.SteamID != @event.Userid.SteamID)
            {
                BombCarrier = @event.Userid;
                if (C4 != null && (C4?.IsValid ?? false))
                {
                    C4.Remove();
                }

                BombCarrier.GiveNamedItem("weapon_c4");
                C4 = GetWeapon(BombCarrier, "weapon_c4");

                HotPatatoTimerStart();
            }

            base.EventBombPickup(@event);
        }

        internal override void EventBombDropped(EventBombDropped @event)
        {
            base.EventBombDropped(@event);
        }

        private CCSPlayerController GetRandomBombCarrier(IEnumerable<CCSPlayerController> players)
        {
            var bombCarrier = players.Skip(_random.Next(players.Count())).FirstOrDefault();

            if (ValidateCallerPlayer(bombCarrier, false) == false)
            {
                return GetRandomBombCarrier(players);
            }
            PrintToAll(bombCarrier.PlayerName);

            return bombCarrier;
        }

        private void HotPatatoTimerStart()
        {
            C4HitTimer?.Kill();
            C4HitIncreaseTimer?.Kill();
            C4CarierHitDamage = 1;

            C4HitTimer = Global?.AddTimer(1, () =>
            {
                if (ValidateCallerPlayer(BombCarrier, false) == false) return;

                if (BombCarrier.PawnIsAlive)
                {
                    PerformBurn(BombCarrier, C4CarierHitDamage);
                }
            }, Full);
            C4HitIncreaseTimer = Global?.AddTimer(10, () =>
            {
                if (C4CarierHitDamage == 1)
                {
                    C4CarierHitDamage = 3;
                }
                else if (C4CarierHitDamage >= 10)
                {
                    C4CarierHitDamage = 10;
                }
                else
                {
                    C4CarierHitDamage += 3;
                }
            }, Full);
        }

        private void PrintToAll(string pname)
        {
            Server.PrintToChatAll($"{Prefix}{CC.W} Bomba {CC.Or} {pname}{CC.W} adlı mahkûmda.");
            PrintToCenterHtmlAll($"Bomba {pname} adlı mahkûmda.");
        }
    }
}