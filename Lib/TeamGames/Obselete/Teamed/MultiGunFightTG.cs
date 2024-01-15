using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiGunFightTG : TeamGamesGameBase
    {
        private string SelectedWeaponName = null;

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
                    SelectedWeaponName = item.Value?.Split("weapon_")?[1] ?? "";
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
            GetPlayers(CsTeam.Terrorist)
                .Where(x => x.PawnIsAlive)
                .ToList()
                .ForEach(x =>
                {
                    if (ValidateCallerPlayer(x, false) == false) return;
                    RemoveWeapons(x, false);
                });
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
    }
}