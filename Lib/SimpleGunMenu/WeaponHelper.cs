﻿namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static readonly Dictionary<string, string> NoScopeGuns = new Dictionary<string, string>()
        {
                {"AWP",                                new("awp")},
                {"SSG 08",                             new("ssg08")},
                {"G3SG1",                              new("g3sg1")},
                {"SCAR-20",                            new("scar20" )},
        };

    public enum WeaponType
    {
        Primary = 0,
        Secondary = 1,
    }

    public class Weapon
    {
        public Weapon(string giveName, WeaponType type = WeaponType.Primary, bool hideIseli = false)
        {
            Type = type;
            GiveName = giveName;
            HideIseli = hideIseli;
        }

        public WeaponType Type { get; set; }
        public string GiveName { get; set; }
        public bool HideIseli { get; set; } = false;
    }

    private static class WeaponHelper
    {
        internal static (Dictionary<string, Weapon> Weapons, Dictionary<string, Weapon> WeaponCheckers) LoadWeapons()
        {
            var weapons = new Dictionary<string, Weapon>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"AK-47",                              new("weapon_ak47")},
                {"AWP",                                new("weapon_awp")},
                {"Desert Eagle",                       new("weapon_deagle",         WeaponType.Secondary)},
                {"P90",                                new("weapon_p90")},
                {"Negev",                              new("weapon_negev", hideIseli: true)},
                {"SSG 08",                             new("weapon_ssg08")},
                {"M4A4",                               new("weapon_m4a1")},
                {"M4A1-S",                             new("weapon_m4a1_silencer")},
                {"XM1014",                             new("weapon_xm1014")},
                {"MAG-7",                              new("weapon_mag7")},
                {"SCAR-20",                            new("weapon_scar20" , hideIseli: true)},
                {"SG 556",                             new("weapon_sg556")},
                {"AUG",                                new("weapon_aug")},
                {"M249",                               new("weapon_m249", hideIseli: true)},
                {"FAMAS",                              new("weapon_famas")},
                {"Galil AR",                           new("weapon_galilar")},
                {"Glock-18",                           new("weapon_glock",          WeaponType.Secondary)},
                {"P2000",                              new("weapon_hkp2000",        WeaponType.Secondary)},
                {"USP-S",                              new("weapon_usp_silencer",   WeaponType.Secondary)},
                {"Tec-9",                              new("weapon_tec9",           WeaponType.Secondary)},
                {"P250",                               new("weapon_p250",           WeaponType.Secondary)},
                {"CZ75-Auto",                          new("weapon_cz75a",          WeaponType.Secondary)},
                {"Dual Berettas",                      new("weapon_elite",          WeaponType.Secondary)},
                {"Five-SeveN",                         new("weapon_fiveseven",      WeaponType.Secondary)},
                {"R8 Revolver",                        new("weapon_revolver",       WeaponType.Secondary)},
                {"MP5-SD",                             new("weapon_mp5sd")},
                {"PP-Bizon",                           new("weapon_bizon")},
                {"UMP-45",                             new("weapon_ump45")},
                {"MP9",                                new("weapon_mp9")},
                {"MP7",                                new("weapon_mp7")},
                {"MAC-10",                             new("weapon_mac10")},
                {"G3SG1",                              new("weapon_g3sg1", hideIseli: true)},
                {"Sawed-Off",                          new("weapon_sawedoff")},
                {"Nova",                               new("weapon_nova")},
            };

            ///Duplicates of weapons dictionarys value objects givename property.
            ///<see cref="WeaponMenuHelper.RemoveCurrentWeapon"/>
            var weaponCheckers = new Dictionary<string, Weapon>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"weapon_ak47",                        new("weapon_ak47")},
                {"weapon_awp",                         new("weapon_awp")},
                {"weapon_deagle",                      new("weapon_deagle")},
                {"weapon_m4a1",                        new("weapon_m4a1")},
                {"weapon_m4a1_silencer",               new("weapon_m4a1_silencer")},
                {"weapon_aug",                         new("weapon_aug")},
                {"weapon_ssg08",                       new("weapon_ssg08")},
                {"weapon_negev",                       new("weapon_negev")},
                {"weapon_bizon",                       new("weapon_bizon")},
                {"weapon_m249",                        new("weapon_m249")},
                {"weapon_famas",                       new("weapon_famas")},
                {"weapon_galilar",                     new("weapon_galilar")},
                {"weapon_glock",                       new("weapon_glock",          WeaponType.Secondary)},
                {"weapon_hkp2000",                     new("weapon_hkp2000",        WeaponType.Secondary)},
                {"weapon_usp_silencer",                new("weapon_usp_silencer",   WeaponType.Secondary)},
                {"weapon_tec9",                        new("weapon_tec9",           WeaponType.Secondary)},
                {"weapon_p250",                        new("weapon_p250",           WeaponType.Secondary)},
                {"weapon_cz75a",                       new("weapon_cz75a",          WeaponType.Secondary)},
                {"weapon_fiveseven",                   new("weapon_fiveseven",      WeaponType.Secondary)},
                {"weapon_elite",                       new("weapon_elite",          WeaponType.Secondary)},
                {"weapon_revolver",                    new("weapon_revolver",       WeaponType.Secondary)},
                {"weapon_mp5sd",                       new("weapon_mp5sd")},
                {"weapon_ump45",                       new("weapon_ump45")},
                {"weapon_mp9",                         new("weapon_mp9")},
                {"weapon_p90",                         new("weapon_p90")},
                {"weapon_mp7",                         new("weapon_mp7")},
                {"weapon_mac10",                       new("weapon_mac10")},
                {"weapon_sg556",                       new("weapon_sg556")},
                {"weapon_g3sg1",                       new("weapon_g3sg1")},
                {"weapon_scar20",                      new("weapon_scar20" )},
                {"weapon_xm1014",                      new("weapon_xm1014")},
                {"weapon_mag7",                        new("weapon_mag7")},
                {"weapon_sawedoff",                    new("weapon_sawedoff")},
                {"weapon_nova",                        new("weapon_nova")},
            };
            return (weapons, weaponCheckers);
        }
    }
}