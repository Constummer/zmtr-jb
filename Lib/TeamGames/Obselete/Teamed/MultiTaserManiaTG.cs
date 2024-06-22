using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiTaserManiaTG : TeamGamesGameBase
    {
        public Dictionary<int, int> PlayerCount { get; set; } = new();

        public MultiTaserManiaTG() : base(TeamGamesMultiChoices.TaserMania)
        {
        }

        internal override void StartGame(Action callback)
        {
            RemoveAllWeapons(giveKnife: false, custom: "weapon_taser");
            Server.ExecuteCommand("mp_taser_recharge_time 1.5");
            PlayerCount = GetTeamPlayerCounts();
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            Server.ExecuteCommand("mp_taser_recharge_time 15");
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