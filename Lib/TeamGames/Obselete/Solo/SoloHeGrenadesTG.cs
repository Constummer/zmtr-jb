using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloHeGrenadesTG : TeamGamesGameBase
    {
        public int PlayerCount { get; set; } = 0;

        public SoloHeGrenadesTG() : base(TeamGamesSoloChoices.HeGrenades)
        {
        }

        internal override void StartGame(Action callback)
        {
            PlayerCount = RemoveAllWeapons(giveKnife: false, giveFists: true);
            Global?.SinirsizXAction(null, "@t", "hegrenade");
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            Global?.SinirsizXKapaAction("@t", "");
            PlayerCount = 0;
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