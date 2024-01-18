using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiNoZoomTG : TeamGamesGameBase
    {
        private string SelectedWeaponName = null;
        public Dictionary<int, int> PlayerCount { get; set; } = new();

        public MultiNoZoomTG() : base(TeamGamesMultiChoices.NoZoom)
        {
            HasAdditionalChoices = true;
        }

        internal override void AdditionalChoiceMenu(CCSPlayerController player, Action value)
        {
            if (ValidateCallerPlayer(player, false) == false) return;
            var soloTGMenu = new ChatMenu("No Scope Silah Menü | Takımlı");

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
            RemoveAllWeapons(giveKnife: false, custom: $"weapon_{SelectedWeaponName}");
            PlayerCount = GetTeamPlayerCounts();
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            RemoveAllWeapons(giveKnife: true);
            PlayerCount?.Clear();
            base.Clear(printMsg);
        }

        internal override void EventWeaponZoom(EventWeaponZoom @event)
        {
            if (@event == null) return;

            if (ValidateCallerPlayer(@event.Userid, false) == false) return;
            CBasePlayerWeapon? weapon = null;

            RemoveWeapon(@event.Userid, $"weapon_{SelectedWeaponName}");
            @event.Userid.GiveNamedItem($"weapon_{SelectedWeaponName}");
            weapon = GetWeapon(@event.Userid, $"weapon_{SelectedWeaponName}");
            if (WeaponIsValid(weapon) == false)
            {
                return;
            }
            SetAmmo(weapon, 999, 999);

            base.EventWeaponZoom(@event);
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