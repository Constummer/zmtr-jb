namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private Dictionary<string, WeaponDefault> WeaponDefaults = new();

    public class WeaponDefault
    {
        internal int _1Clip1;
        internal int _2ReserveAmmo0;

        public int VData1MaxClip1 { get; internal set; }
        public int VData1DefaultClip1 { get; internal set; }
        public int VData2PrimaryReserveAmmoMax { get; internal set; }
    }

    public string Weapons { get; set; }
     = @"weapon_deagle,-1,999|
            weapon_elite,-1,999|
            weapon_fiveseven,-1,999|
            weapon_glock,-1,999|
            weapon_ak47,-1,999|
            weapon_aug,-1,999|
            weapon_awp,-1,999|
            weapon_famas,-1,999|
            weapon_g3sg1,-1,999|
            weapon_galilar,-1,999|
            weapon_m249,-1,999|
            weapon_m4a1,-1,999|
            weapon_mac10,-1,999|
            weapon_p90,-1,999|
            weapon_mp5sd,-1,999|
            weapon_ump45,-1,999|
            weapon_xm1014,-1,999|
            weapon_bizon,-1,999|
            weapon_mag7,-1,999|
            weapon_negev,-1,999|
            weapon_sawedoff,-1,999|
            weapon_tec9,-1,999|
            weapon_hkp2000,-1,999|
            weapon_mp7,-1,999|
            weapon_mp9,-1,999|
            weapon_nova,-1,999|
            weapon_p250,-1,999|
            weapon_scar20,-1,999|
            weapon_sg556,-1,999|
            weapon_ssg08,-1,999|
            weapon_m4a1_silencer,-1,999|
            weapon_usp_silencer,-1,999|
            weapon_cz75a,-1,999|
            weapon_revolver,-1,999";

    private static readonly Dictionary<int, string> WeaponDefindex = new()
      {
        { 1, "weapon_deagle" },
        { 2, "weapon_elite" },
        { 3, "weapon_fiveseven" },
        { 4, "weapon_glock" },
        { 7, "weapon_ak47" },
        { 8, "weapon_aug" },
        { 9, "weapon_awp" },
        { 10, "weapon_famas" },
        { 11, "weapon_g3sg1" },
        { 13, "weapon_galilar" },
        { 14, "weapon_m249" },
        { 16, "weapon_m4a1" },
        { 17, "weapon_mac10" },
        { 19, "weapon_p90" },
        { 23, "weapon_mp5sd" },
        { 24, "weapon_ump45" },
        { 25, "weapon_xm1014" },
        { 26, "weapon_bizon" },
        { 27, "weapon_mag7" },
        { 28, "weapon_negev" },
        { 29, "weapon_sawedoff" },
        { 30, "weapon_tec9" },
        { 32, "weapon_hkp2000" },
        { 33, "weapon_mp7" },
        { 34, "weapon_mp9" },
        { 35, "weapon_nova" },
        { 36, "weapon_p250" },
        { 38, "weapon_scar20" },
        { 39, "weapon_sg556" },
        { 40, "weapon_ssg08" },
        { 60, "weapon_m4a1_silencer" },
        { 61, "weapon_usp_silencer" },
        { 63, "weapon_cz75a" },
        { 64, "weapon_revolver" },
      };
}