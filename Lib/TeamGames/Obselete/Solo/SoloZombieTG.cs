using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloZombieTG : TeamGamesGameBase
    {
        public List<ulong> PlayerCount { get; set; } = new();

        public SoloZombieTG() : base(TeamGamesSoloChoices.Zombie)
        {
        }

        internal override void StartGame(Action callback)
        {
            PlayerCount = RemoveAllWeapons(giveKnife: false, custom: "weapon_ak47", setHp: 100);
            GetPlayers(CsTeam.CounterTerrorist)
                .ToList()
                .ForEach(x =>
                {
                    if (ValidateCallerPlayer(x, false) == false) return;
                    x.RemoveWeapons();
                    if (ValidateCallerPlayer(x, false) == false) return;
                    x.GiveNamedItem("weapon_knife");
                    if (ValidateCallerPlayer(x, false) == false) return;
                    if (x.PawnIsAlive == false)
                    {
                        CustomRespawn(x);
                    }
                    if (ValidateCallerPlayer(x, false) == false) return;
                    SetHp(x, 8_000);
                    if (ValidateCallerPlayer(x, false) == false) return;
                    RefreshPawn(x);
                });
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            PlayerCount = new();
            RemoveAllWeapons(giveKnife: true);
            GetPlayers(CsTeam.CounterTerrorist)
              .Where(x => x.PawnIsAlive)
              .ToList()
              .ForEach(x =>
              {
                  if (ValidateCallerPlayer(x, false) == false) return;
                  SetHp(x, 100);

                  if (ValidateCallerPlayer(x, false) == false) return;
                  RefreshPawn(x);
              });
            base.Clear(printMsg);
        }

        internal override void EventPlayerDeath(EventPlayerDeath @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Attacker, false) == false) return;

            if (ValidateCallerPlayer(@event.Userid, false) == false) return;
            if (GetTeam(@event.Attacker) == CsTeam.CounterTerrorist
                && GetTeam(@event.Userid) == CsTeam.Terrorist)
            {
                var steamid = @event.Userid.SteamID;
                @event.Userid.ChangeTeam(CsTeam.CounterTerrorist);
                Global?.AddTimer(0.2f, () =>
                {
                    var x = GetPlayers().Where(x => x.PawnIsAlive == false && x.SteamID == steamid).FirstOrDefault();
                    if (ValidateCallerPlayer(x, false) == false) return;
                    if (x.PawnIsAlive == false)
                    {
                        if (ValidateCallerPlayer(x, false) == false) return;
                        CustomRespawn(x);
                    }
                    if (ValidateCallerPlayer(x, false) == false) return;
                    SetHp(x, 8_000);
                    RefreshPawn(x);
                });
            }
            SoloCheckGameFinished(this, @event.Userid.SteamID, PlayerCount, @event.Attacker.PlayerName);

            base.EventPlayerDeath(@event);
        }

        internal override void EventPlayerDisconnect(ulong? tempSteamId)
        {
            if (tempSteamId == null) return;
            SoloCheckGameFinished(this, tempSteamId.Value, PlayerCount, null);

            base.EventPlayerDisconnect(tempSteamId);
        }
    }
}