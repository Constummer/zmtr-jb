using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class TeamGamesGameBase
    {
        public TeamGamesGameBase(TeamGamesMultiChoices multiChoice)
        {
            MultiChoice = multiChoice;
        }

        public TeamGamesGameBase(TeamGamesSoloChoices soloChoice)
        {
            SoloChoice = soloChoice;
        }

        public TeamGamesMultiChoices MultiChoice { get; set; } = TeamGamesMultiChoices.None;
        public TeamGamesSoloChoices SoloChoice { get; set; } = TeamGamesSoloChoices.None;
        public string GameName { get; set; } = "";

        internal virtual void StartGame(Action callback)
        {
            callback();
        }

        internal virtual void Clear()
        {
            if (string.IsNullOrWhiteSpace(GameName))
            {
                return;
            }
            if (MultiChoice != TeamGamesMultiChoices.None)
            {
                Server.PrintToChatAll($"{CC.W}{GameName} takım oyununu kapatıldı.");
            }
            else
            {
                Server.PrintToChatAll($"{CC.W}{GameName} tekli oyununu kapatıldı.");
            }
        }

        internal virtual void EventBombDropped(EventBombDropped @event)
        {
        }

        internal virtual void EventEntityKilled(EventEntityKilled @event)
        {
        }

        internal virtual void EventBombPickup(EventBombPickup @event)
        {
        }

        internal virtual void EventPlayerDeath(EventPlayerDeath @event)
        {
        }
    }
}