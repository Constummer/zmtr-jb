using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiHeadShotOnlyTG : TeamGamesGameBase
    {
        private string SelectedWeaponName = null;
        public Dictionary<int, int> PlayerCount { get; set; } = new();

        public MultiHeadShotOnlyTG() : base(TeamGamesMultiChoices.HeadShotOnly)
        {
            HasAdditionalChoices = true;
        }

        internal override void AdditionalChoiceMenu(CCSPlayerController player, Action value)
        {
            if (ValidateCallerPlayer(player, false) == false) return;

            var soloTGMenu = new ChatMenu("Silah Menü | Takımlı");
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
            RemoveAllWeapons(giveKnife: false, custom: SelectedWeaponName);

            PlayerCount = GetTeamPlayerCounts();
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            Server.ExecuteCommand($"mp_damage_headshot_only 0");
            RemoveAllWeapons(true);
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