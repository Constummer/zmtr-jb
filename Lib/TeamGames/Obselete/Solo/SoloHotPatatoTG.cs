using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static CounterStrikeSharp.API.Modules.Timers.Timer C4HitTimer { get; set; } = null;
    public static CounterStrikeSharp.API.Modules.Timers.Timer C4HitIncreaseTimer { get; set; } = null;
    public static int C4CarierHitDamage { get; set; } = 0;

    private void HotPatatoTimer()
    {
        C4HitTimer = AddTimer(10, () =>
        {
            //PerformSlap(, C4CarierHitDamage);
        }, Full);
        C4HitIncreaseTimer = AddTimer(10, () =>
        {
            if (C4CarierHitDamage == 0)
            {
                C4CarierHitDamage = 3;
            }
            else
            {
                C4CarierHitDamage += 4;
            }
        }, Full);
    }

    internal class SoloHotPatatoTG : TeamGamesGameBase
    {
        public CCSPlayerController BombCarrier { get; set; } = null;
        public CBasePlayerWeapon C4 { get; set; } = null;

        public SoloHotPatatoTG() : base(TeamGamesSoloChoices.HotPatato)
        {
        }

        internal override void StartGame(Action callback)
        {
            var players = GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive);
            BombCarrier = GetRandomBombCarrier(players);
            BombCarrier.GiveNamedItem("weapon_c4");
            C4 = GetWeapon(BombCarrier, "weapon_c4");

            base.StartGame(callback);
        }

        internal override void EventPlayerDeath(EventPlayerDeath @event)
        {
            if (BombCarrier != null)
            {
                var tempCheck = GetPlayers(CsTeam.Terrorist).Where(x => x.SteamID == BombCarrier.SteamID).FirstOrDefault();
                if (ValidateCallerPlayer(tempCheck, false))
                {
                    if (tempCheck.PawnIsAlive == false)
                    {
                        var players = GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive);
                        BombCarrier = GetRandomBombCarrier(players);
                        BombCarrier.GiveNamedItem("weapon_c4");
                        if (C4 != null && (C4?.IsValid ?? false))
                        {
                            C4.Remove();
                        }
                        C4 = GetWeapon(BombCarrier, "weapon_c4");
                    }
                    else
                    {
                        BombCarrier.GiveNamedItem("weapon_c4");
                        if (C4 != null && (C4?.IsValid ?? false))
                        {
                            C4.Remove();
                        }
                        C4 = GetWeapon(BombCarrier, "weapon_c4");
                    }
                }
            }
            base.EventPlayerDeath(@event);
        }

        internal override void EventBombPickup(EventBombPickup @event)
        {
            BombCarrier = @event.Userid;
            base.EventBombPickup(@event);
        }

        internal override void EventBombDropped(EventBombDropped @event)
        {
            if (BombCarrier != null)
            {
                var tempCheck = GetPlayers(CsTeam.Terrorist).Where(x => x.SteamID == BombCarrier.SteamID).FirstOrDefault();
                if (ValidateCallerPlayer(tempCheck, false))
                {
                    if (tempCheck.PawnIsAlive == false)
                    {
                        var players = GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive);
                        BombCarrier = GetRandomBombCarrier(players);
                        BombCarrier.GiveNamedItem("weapon_c4");
                        if (C4 != null && (C4?.IsValid ?? false))
                        {
                            C4.Remove();
                        }
                        C4 = GetWeapon(BombCarrier, "weapon_c4");
                    }
                    else
                    {
                        BombCarrier.GiveNamedItem("weapon_c4");
                        if (C4 != null && (C4?.IsValid ?? false))
                        {
                            C4.Remove();
                        }
                        C4 = GetWeapon(BombCarrier, "weapon_c4");
                    }
                }
            }
            else
            {
                var players = GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive);
                BombCarrier = GetRandomBombCarrier(players);
                BombCarrier.GiveNamedItem("weapon_c4");
                if (C4 != null && (C4?.IsValid ?? false))
                {
                    C4.Remove();
                }
                C4 = GetWeapon(BombCarrier, "weapon_c4");
            }
            base.EventBombDropped(@event);
        }

        internal override void Clear()
        {
            if (C4 != null && (C4?.IsValid ?? false))
            {
                C4.Remove();
            }
            C4CarierHitDamage = 0;
            C4HitTimer?.Kill();
            C4HitIncreaseTimer?.Kill();
            BombCarrier = null;
            base.Clear();
        }

        private CCSPlayerController GetRandomBombCarrier(IEnumerable<CCSPlayerController> players)
        {
            var bombCarrier = players.Skip(_random.Next(players.Count())).FirstOrDefault();

            if (ValidateCallerPlayer(bombCarrier, false) == false)
            {
                return GetRandomBombCarrier(players);
            }

            return bombCarrier;
        }
    }
}