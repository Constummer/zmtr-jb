using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiMachines500HPTG : TeamGamesGameBase
    {
        public Dictionary<int, int> PlayerCount { get; set; } = new();

        public MultiMachines500HPTG() : base(TeamGamesMultiChoices.Machines500HP)
        {
        }

        internal override void StartGame(Action callback)
        {
            RemoveAllWeapons(giveKnife: false, custom: "weapon_m249", setHp: 500);
            PlayerCount = GetTeamPlayerCounts();
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
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