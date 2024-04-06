using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloZombie2TG : TeamGamesGameBase
    {
        public List<ulong> PlayerCount { get; set; } = new();

        public SoloZombie2TG() : base(TeamGamesSoloChoices.Zombie2)
        {
        }

        internal override void StartGame(Action callback)
        {
            FfActive = false;
            PlayerCount = RemoveAllWeapons(giveKnife: false, custom: "weapon_ak47", setHp: 100);
            RemoveAllWeaponsCT(true, false, null, 3000);
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            PlayerCount = new();
            RemoveAllWeapons(giveKnife: true);
            RemoveAllWeaponsCT(true, false, null, 100);
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
                if (ValidateCallerPlayer(@event.Userid, false) == false) return;
                Global?.AddTimer(2f, () =>
                {
                    var x = GetPlayers().Where(x => x.SteamID == steamid).FirstOrDefault();
                    if (x != null)
                    {
                        if (ValidateCallerPlayer(x, false) == false) return;
                        CustomRespawn(x);
                    }
                }, SOM);
            }

            base.EventPlayerDeath(@event);
        }

        internal override void EventPlayerDisconnect(ulong? tempSteamId)
        {
            if (tempSteamId == null) return;
            //SoloCheckGameFinished(this, tempSteamId.Value, PlayerCount, null);

            base.EventPlayerDisconnect(tempSteamId);
        }
    }
}