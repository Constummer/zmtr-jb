using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloTaserManiaTG : TeamGamesGameBase
    {
        public int PlayerCount { get; set; } = 0;

        public SoloTaserManiaTG() : base(TeamGamesSoloChoices.TaserMania)
        {
        }

        internal override void StartGame(Action callback)
        {
            PlayerCount = RemoveAllWeapons(giveKnife: false);
            Global?.SinirsizXAction(null, "@t", "taser");
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            PlayerCount = 0;
            Global?.SinirsizXKapaAction("@t", "");
            RemoveAllWeapons(giveKnife: true);
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