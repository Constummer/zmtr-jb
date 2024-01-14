using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiNoZoomTG : TeamGamesGameBase
    {
        private string SelectedWeaponName = null;

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
                });
            }
            ChatMenus.OpenMenu(player, soloTGMenu);
            base.AdditionalChoiceMenu(player, value);
        }

        internal override void StartGame(Action callback)
        {
            if (Global != null)
            {
                Global.UnlimitedReserverAmmoActive = true;
            }
            GiveAction("", "@t", SelectedWeaponName, TargetForArgument.None, false);

            base.StartGame(callback);
        }

        internal override void Clear()
        {
            if (Global != null)
            {
                Global.UnlimitedReserverAmmoActive = false;
            }
            base.Clear();
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
    }
}