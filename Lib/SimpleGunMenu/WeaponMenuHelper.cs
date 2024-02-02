using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static class WeaponMenuHelper
    {
        private static Dictionary<string, Weapon> _weapons;
        private static Dictionary<string, Weapon> _weaponCheckers;

        static WeaponMenuHelper()
        {
            var res = WeaponHelper.LoadWeapons();
            _weapons = res.Weapons;
            _weaponCheckers = res.WeaponCheckers;
        }

        public static void GetGuns(ChatMenu gunMenu, WeaponType? type = null)
        {
            Dictionary<string, Weapon> weapons = _weapons;
            if (type != null)
            {
                weapons = _weapons.Where(x => x.Value.Type == type.Value)
                                  .ToDictionary(x => x.Key, y => y.Value);
            }
            foreach (var item in weapons)
            {
                gunMenu.AddMenuOption(item.Key, GiveSelectedItem);
            }
        }

        public static Dictionary<string, string> GetGuns()
        {
            return _weapons.ToList().ToDictionary(x => x.Key, y => y.Value.GiveName);
        }

        public static Dictionary<string, string> GetPistols()
        {
            return _weapons.ToList().Where(x => x.Value.Type == WeaponType.Secondary).ToDictionary(x => x.Key, y => y.Value.GiveName);
        }

        public static bool ValidWeaponChecker(string designerName)
        {
            return _weaponCheckers.TryGetValue(designerName, out _);
        }

        private static void GiveSelectedItem(CCSPlayerController player, ChatMenuOption option)
        {
            if (player == null
                || player.IsValid == false
                || player.IsBot == true
                || player?.PlayerPawn?.Value?.WeaponServices?.MyWeapons == null)
            {
                return;
            }
            if (FFMenuCheck == true)
            {
                if (_weapons.TryGetValue(option.Text, out var selectedWeapon))
                {
                    RemoveCurrentWeapon(player, selectedWeapon);
                    player.GiveNamedItem(selectedWeapon.GiveName);
                }
            }
            else if (IsEliMenuCheck == true && GetTeam(player) == CsTeam.CounterTerrorist)
            {
                if (_weapons.TryGetValue(option.Text, out var selectedWeapon))
                {
                    RemoveCurrentWeapon(player, selectedWeapon);
                    player.GiveNamedItem(selectedWeapon.GiveName);
                }
            }
        }

        private static void RemoveCurrentWeapon(CCSPlayerController? player, Weapon selectedWeapon)
        {
            foreach (var weapon in player!.PlayerPawn.Value!.WeaponServices!.MyWeapons)
            {
                if (weapon.Value != null &&
                    string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false &&
                    weapon.Value.DesignerName != "[null]" &&
                    _weaponCheckers.TryGetValue(weapon.Value.DesignerName, out var currentWeapon))
                {
                    if (currentWeapon.Type == selectedWeapon.Type)
                    {
                        weapon.Value.Remove();
                    }
                }
            }
        }
    }
}