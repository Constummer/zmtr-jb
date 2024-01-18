using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloNoZoomTG : TeamGamesGameBase
    {
        private string SelectedWeaponName = null;
        public int PlayerCount { get; set; } = 0;

        public SoloNoZoomTG() : base(TeamGamesSoloChoices.NoZoom)
        {
            HasAdditionalChoices = true;
        }

        internal override void AdditionalChoiceMenu(CCSPlayerController player, Action value)
        {
            if (ValidateCallerPlayer(player, false) == false) return;
            var soloTGMenu = new ChatMenu("No Scope Silah Menü | Herkes Tek");

            foreach (var item in NoScopeGunMenu)
            {
                soloTGMenu.AddMenuOption(item.Key, (p, i) =>
                {
                    SelectedWeaponName = item.Value;
                    base.AdditionalChoiceMenu(player, value);
                });
            }
            ChatMenus.OpenMenu(player, soloTGMenu);
        }

        internal override void StartGame(Action callback)
        {
            PlayerCount = RemoveAllWeapons(giveKnife: false, custom: $"weapon_{SelectedWeaponName}");
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            RemoveAllWeapons(giveKnife: true);
            PlayerCount = 0;
            base.Clear(printMsg);
        }

        internal override void EventWeaponZoom(EventWeaponZoom @event)
        {
            if (@event == null) return;

            if (ValidateCallerPlayer(@event.Userid, false) == false) return;

            @event.Userid.RemoveWeapons();
            @event.Userid.GiveNamedItem($"weapon_{SelectedWeaponName}");

            base.EventWeaponZoom(@event);
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