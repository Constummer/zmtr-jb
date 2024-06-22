using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloCoctailPartyTG : TeamGamesGameBase
    {
        public List<ulong> PlayerCount { get; set; } = new();

        public SoloCoctailPartyTG() : base(TeamGamesSoloChoices.CoctailParty)
        {
        }

        internal override void StartGame(Action callback)
        {
            PlayerCount = RemoveAllWeapons(giveKnife: false, giveFists: true);
            Global?.SinirsizXAction(null, "@t", "molotov");
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            Global?.SinirsizXKapaAction("@t", null);
            RemoveAllWeapons(giveKnife: true);
            PlayerCount = new();
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