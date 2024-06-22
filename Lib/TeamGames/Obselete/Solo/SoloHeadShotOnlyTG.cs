using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloHeadShotOnlyTG : TeamGamesGameBase
    {
        private string SelectedWeaponName = null;
        public List<ulong> PlayerCount { get; set; } = new();

        public SoloHeadShotOnlyTG() : base(TeamGamesSoloChoices.HeadShotOnly)
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
            MenuManager.OpenChatMenu(player, soloTGMenu);
        }

        internal override void StartGame(Action callback)
        {
            Server.ExecuteCommand($"mp_damage_headshot_only 1");
            PlayerCount = RemoveAllWeapons(giveKnife: false, custom: SelectedWeaponName);

            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            Server.ExecuteCommand($"mp_damage_headshot_only 0");
            PlayerCount = new();
            RemoveAllWeapons(true);
            base.Clear(printMsg);
        }

        internal override void EventPlayerDeath(EventPlayerDeath @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Attacker, false) == false) return;

            if (ValidateCallerPlayer(@event.Userid, false) == false) return;
            SoloCheckGameFinished(this, @event.Userid.SteamID, PlayerCount, @event.Attacker.PlayerName);

            base.EventPlayerDeath(@event);
        }

        internal override void EventPlayerDisconnect(ulong? tempSteamId)
        {
            if (tempSteamId == null) return;
            SoloCheckGameFinished(this, tempSteamId.Value, PlayerCount, null);

            base.EventPlayerDisconnect(tempSteamId);
        }
    }
}