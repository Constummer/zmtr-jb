using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloCoronaTG : TeamGamesGameBase
    {
        public CounterStrikeSharp.API.Modules.Timers.Timer? CoronaHitTimer { get; set; } = null;
        public CounterStrikeSharp.API.Modules.Timers.Timer? CoronaHitIncreaseTimer { get; set; } = null;

        public Dictionary<ulong, int> CoronaPlayers { get; set; } = new();

        public SoloCoronaTG() : base(TeamGamesSoloChoices.Corona)
        {
        }

        internal override void StartGame(Action callback)
        {
            CoronaPlayers?.Clear();
            RemoveAllWeapons(giveKnife: true);

            var players = GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive);
            var coronaPlayer = GetRandomCorona(players);
            CoronaPlayers.Add(coronaPlayer.SteamID, 1);
            CoronaTimerStart();
            base.StartGame(callback);
        }

        internal override void EventPlayerHurt(EventPlayerHurt @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Userid, false) == false) return;
            if (ValidateCallerPlayer(@event.Attacker, false) == false) return;
            //SALDIRAN CORONALIYSA
            if (GetTeam(@event.Userid) == CsTeam.Terrorist && GetTeam(@event.Attacker) == CsTeam.Terrorist)
            {
                //GODLU BIRI DEILSE
                if (ActiveGodMode.TryGetValue(@event.Userid.SteamID, out var value) == false)
                {
                    //SALDIRANIN HASARINI IPTAL EDIP 100E CEK
                    @event.Userid.Health = 100;
                    @event.Userid.PlayerPawn.Value!.Health = 100;
                    if (@event.Userid.PawnArmor != 0)
                    {
                        @event.Userid.PawnArmor = 100;
                    }
                    if (@event.Userid.PlayerPawn.Value!.ArmorValue != 0)
                    {
                        @event.Userid.PlayerPawn.Value!.ArmorValue = 100;
                    }
                    //CORONASI YOKSA
                    if (CoronaPlayers.ContainsKey(@event.Userid.SteamID) == false)
                    {
                        CoronaPlayers.Add(@event.Userid.SteamID, 1);
                        if (ValidateCallerPlayer(@event.Userid, false) == false) return;
                        SetColour(@event.Userid, Color.FromArgb(0, 255, 0));
                    }
                }
            }
            base.EventPlayerHurt(@event);
        }

        internal override void EventPlayerDeath(EventPlayerDeath @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Userid, false) == false) return;

            if (CoronaPlayers.ContainsKey(@event.Userid.SteamID))
            {
                CoronaPlayers.Remove(@event.Userid.SteamID);

                if (CoronaPlayers.Count == 0)
                {
                    Server.PrintToChatAll($"{Prefix} {CC.W}Virüs bulaşmayanlar kazandı.");
                    PrintToCenterHtmlAll($"{Prefix} Virüs bulaşmayanlar kazandı.");

                    Clear(true);
                }
            }
            base.EventPlayerDeath(@event);
        }

        internal override void Clear(bool printMsg)
        {
            CoronaHitTimer?.Kill();
            CoronaHitIncreaseTimer?.Kill();
            CoronaHitTimer = null;
            CoronaHitIncreaseTimer = null;
            base.Clear(printMsg);
        }

        private CCSPlayerController GetRandomCorona(IEnumerable<CCSPlayerController> players)
        {
            var coronaCarrier = players.Skip(_random.Next(players.Count())).FirstOrDefault();

            if (ValidateCallerPlayer(coronaCarrier, false) == false)
            {
                return GetRandomCorona(players);
            }
            SetColour(coronaCarrier, Color.FromArgb(0, 255, 0));
            PrintToAll(coronaCarrier.PlayerName);

            return coronaCarrier;
        }

        private void CoronaTimerStart()
        {
            CoronaHitTimer?.Kill();
            CoronaHitIncreaseTimer?.Kill();

            CoronaHitTimer = Global?.AddTimer(1, () =>
            {
                GetPlayers(CsTeam.Terrorist)
                .Where(x => CoronaPlayers.ContainsKey(x.SteamID))
                .ToList()
                .ForEach(x =>
                {
                    if (x.PawnIsAlive)
                    {
                        if (CoronaPlayers.TryGetValue(x.SteamID, out var hitdamage))
                        {
                            if (ValidateCallerPlayer(x, false) == false) return;
                            PerformBurn(x, hitdamage);
                        }
                    }
                    else
                    {
                        CoronaPlayers.Remove(x.SteamID);
                    }
                });
            }, Full);
            CoronaHitIncreaseTimer = Global?.AddTimer(10, () =>
            {
                foreach (var item in CoronaPlayers.ToList())
                {
                    var hitDamage = item.Value;
                    if (hitDamage == 1)
                    {
                        hitDamage = 3;
                    }
                    else if (hitDamage >= 10)
                    {
                        hitDamage = 10;
                    }
                    else
                    {
                        hitDamage += 3;
                    }
                    CoronaPlayers[item.Key] = hitDamage;
                }
            }, Full);
        }

        private void PrintToAll(string pname)
        {
            Server.PrintToChatAll($"{Prefix}{CC.W} Corona {CC.Or} {pname}{CC.W} adlı mahkûmda. Kaçın...");
            PrintToCenterHtmlAll($"Corona {pname} adlı mahkûmda. Kaçın...");
        }
    }
}