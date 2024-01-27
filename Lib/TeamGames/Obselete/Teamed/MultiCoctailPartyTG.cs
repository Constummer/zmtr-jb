using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiCoctailPartyTG : TeamGamesGameBase
    {
        public Dictionary<int, int> PlayerCount { get; set; } = new();

        public MultiCoctailPartyTG() : base(TeamGamesMultiChoices.CoctailParty)
        {
        }

        internal override void StartGame(Action callback)
        {
            RemoveAllWeapons(giveKnife: false, giveFists: true);
            Global?.SinirsizXAction(null, "@t", "molotov");
            PlayerCount = GetTeamPlayerCounts();
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            Global?.SinirsizXKapaAction("@t", null);
            RemoveAllWeapons(giveKnife: true);
            PlayerCount?.Clear();

            base.Clear(printMsg);
        }

        internal override void EventPlayerDeath(EventPlayerDeath @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Userid, false) == false) return;

            MultiCheckGameFinished(this, @event.Userid.SteamID, PlayerCount);

            base.EventPlayerDeath(@event);
        }

        internal override void EventPlayerDisconnect(ulong? tempSteamId)
        {
            if (tempSteamId == null) return;
            MultiCheckGameFinished(this, tempSteamId.Value, PlayerCount);
            base.EventPlayerDisconnect(tempSteamId);
        }
    }
}