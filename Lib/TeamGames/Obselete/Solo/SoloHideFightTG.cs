using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloHideFightTG : TeamGamesGameBase
    {
        private string SelectedWeaponName = null;
        public int PlayerCount { get; set; } = 0;

        public SoloHideFightTG() : base(TeamGamesSoloChoices.HideFight)
        {
            HasAdditionalChoices = true;
        }

        internal override void AdditionalChoiceMenu(CCSPlayerController player, Action value)
        {
            if (ValidateCallerPlayer(player, false) == false) return;

            var soloTGMenu = new ChatMenu("Silah Menü | Herkes Tek");
            foreach (var item in WeaponMenuHelper.GetGuns())
            {
                soloTGMenu.AddMenuOption(item.Key, (_, _) =>
                {
                    SelectedWeaponName = item.Value;
                    base.AdditionalChoiceMenu(player, value);
                });
            }
            ChatMenus.OpenMenu(player, soloTGMenu);
        }

        internal override void StartGame(Action callback)
        {
            PlayerCount = RemoveAllWeapons(giveKnife: false, custom: SelectedWeaponName);

            GetPlayers(CsTeam.Terrorist)
            .ToList()
            .ForEach(x =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                SetColour(x, Color.FromArgb(0, 0, 0, 0));
                HideWeapons(x);
            });
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            RemoveAllWeapons(giveKnife: true);
            GetPlayers(CsTeam.Terrorist)
                .ToList()
                .ForEach(x =>
                {
                    if (ValidateCallerPlayer(x, false) == false) return;
                    SetColour(x, DefaultColor);
                    ShowWeapons(x);
                });

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