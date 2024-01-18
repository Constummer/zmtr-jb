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

        public bool FfActive { get; set; } = true;
        public bool HasAdditionalChoices { get; set; } = false;
        public TeamGamesMultiChoices MultiChoice { get; set; } = TeamGamesMultiChoices.None;
        public TeamGamesSoloChoices SoloChoice { get; set; } = TeamGamesSoloChoices.None;
        public string GameName { get; set; } = "";

        internal virtual void StartGame(Action callback)
        {
            callback();
        }

        internal virtual void Clear(bool printMsg)
        {
            Server.ExecuteCommand("mp_teammates_are_enemies 0");

            if (string.IsNullOrWhiteSpace(GameName))
            {
                return;
            }
            TakimBozAction();
            if (printMsg)
            {
                if (MultiChoice != TeamGamesMultiChoices.None)
                {
                    Server.PrintToChatAll($"{CC.W}{GameName} takım oyununu kapatıldı.");
                }
                else
                {
                    Server.PrintToChatAll($"{CC.W}{GameName} tekli oyununu kapatıldı.");
                }
            }
        }

        internal virtual void AdditionalChoiceMenu(CCSPlayerController player, Action value)
        {
            value();
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

        internal virtual void EventItemPickup(EventItemPickup @event)
        {
        }

        internal virtual void EventWeaponZoom(EventWeaponZoom @event)
        {
        }

        internal virtual void OnTakeDamageHook(CEntityInstance ent, CEntityInstance activator)
        {
        }

        internal virtual void EventWeaponFire(EventWeaponFire @event)
        {
        }
    }
}