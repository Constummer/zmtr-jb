using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiGunFightTG : TeamGamesGameBase
    {
        private string SelectedWeaponName = null;
        public Dictionary<int, int> PlayerCount { get; set; } = new();

        public MultiGunFightTG() : base(TeamGamesMultiChoices.GunFight)
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
            ChatMenus.OpenMenu(player, soloTGMenu);
        }

        internal override void StartGame(Action callback)
        {
            RemoveAllWeapons(giveKnife: false, custom: SelectedWeaponName);
            PlayerCount = GetTeamPlayerCounts();
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            RemoveAllWeapons(giveKnife: true);
            PlayerCount?.Clear();
            base.Clear(printMsg);
        }

        internal override void EventPlayerDeath(EventPlayerDeath @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Userid, false) == false) return;

            var team = FindTeam(@event.Userid.SteamID);
            if (team.Index == -1) return;
            if (PlayerCount.ContainsKey(team.Index))
            {
                PlayerCount[team.Index]--;
                var otherTeamIndex = (team.Index + 1) % 2;

                if (PlayerCount[team.Index] <= 0)
                {
                    var otherTeam = GetTeamColorAndTextByIndex(otherTeamIndex);
                    if (otherTeam.Msg == null) return;

                    Server.PrintToChatAll($"{Prefix} {otherTeam.Msg} {CC.W}takım kazandı.");
                    PrintToCenterHtmlAll($"{Prefix} {otherTeam.Msg} {CC.W}takım kazandı.");
                    Clear(true);
                }
            }

            base.EventPlayerDeath(@event);
        }
    }
}