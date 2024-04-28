using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloNoZoomTG : TeamGamesGameBase
    {
        private string SelectedWeaponName = null;
        public List<ulong> PlayerCount { get; set; } = new();

        public SoloNoZoomTG() : base(TeamGamesSoloChoices.NoZoom)
        {
            HasAdditionalChoices = true;
        }

        internal override void AdditionalChoiceMenu(CCSPlayerController player, Action value)
        {
            if (ValidateCallerPlayer(player, false) == false) return;
            var soloTGMenu = new ChatMenu("No Scope Silah Menü | Herkes Tek");

            foreach (var item in NoScopeGuns)
            {
                soloTGMenu.AddMenuOption(item.Key, (p, i) =>
                {
                    SelectedWeaponName = item.Value;
                    base.AdditionalChoiceMenu(player, value);
                });
            }
            MenuManager.OpenChatMenu(player, soloTGMenu);
        }

        internal override void StartGame(Action callback)
        {
            PlayerCount = RemoveAllWeapons(giveKnife: false, custom: $"weapon_{SelectedWeaponName}");
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            RemoveAllWeapons(giveKnife: true);
            PlayerCount = new();
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