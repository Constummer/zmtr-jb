using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloCoctailPartyTG : TeamGamesGameBase
    {
        public int PlayerCount { get; set; } = 0;

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
            Global?.SinirsizXKapaAction("@t", "");
            RemoveAllWeapons(giveKnife: true);
            PlayerCount = 0;
            base.Clear(printMsg);
        }

        internal override void EventPlayerDeath(EventPlayerDeath @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Attacker, false) == false) return;

            PlayerCount--;

            if (PlayerCount <= 1)
            {
                Server.PrintToChatAll($"{Prefix} {CC.Or} {@event.Attacker.PlayerName}{CC.W} adlı mahkûm kazandı.");
                PrintToCenterHtmlAll($"{Prefix} {@event.Attacker.PlayerName} adlı mahkûm kazandı.");

                Clear(true);
            }
            base.EventPlayerDeath(@event);
        }
    }
}