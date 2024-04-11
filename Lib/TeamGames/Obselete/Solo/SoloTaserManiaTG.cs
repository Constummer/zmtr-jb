using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloTaserManiaTG : TeamGamesGameBase
    {
        public List<ulong> PlayerCount { get; set; } = new();

        public SoloTaserManiaTG() : base(TeamGamesSoloChoices.TaserMania)
        {
        }

        internal override void StartGame(Action callback)
        {
            PlayerCount = RemoveAllWeapons(giveKnife: false, custom: "weapon_taser");
            Server.ExecuteCommand("mp_taser_recharge_time 1.5");

            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            Server.ExecuteCommand("mp_taser_recharge_time 15");
            PlayerCount = new();
            RemoveAllWeapons(giveKnife: true);
            base.Clear(printMsg);
        }

        internal override void EventPlayerDeath(EventPlayerDeath @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Attacker, false) == false) return;

            if (ValidateCallerPlayer(@event.Userid, false) == false) return;
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