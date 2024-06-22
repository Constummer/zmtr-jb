namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static string Prefix => $" {CC.LR}[ZMTR]";

    private string WardenDcWebHook => _Config.Additional.WardenDcWebHook;
    private string Total_T_CTDcWebHook => _Config.Additional.Total_T_CTDcWebHook;
    private string Total_IsTop_DcWebHook => _Config.Additional.Total_IsTop_DcWebHook;

    #region Terrorist

    private const string T_AllCap = "MAHKÛM";
    private const string T_AllLower = "mahkûm";
    private const string T_CamelCase = "Mahkûm";
    private const string T_PluralCamel = "Mahkûmlar";
    private const string T_PluralCamelPossesive = "Mahkûmların";
    private const string T_PluralLowerObjective = "mahkûmları";
    private const string T_LowerPrePosition = "mahkûmla";
    private const string T_LowerPositioning = "mahkûmda";
    private const string T_LowerPersonal = "mahkûmsun";

    #endregion Terrorist

    #region CounterTerrorist

    private const string CT_AllCap = "GARDİYAN";
    private const string CT_AllLower = "gardiyan";
    private const string CT_CamelCase = "Gardiyan";
    private const string CT_PluralCamel = "Gardiyanlar";
    private const string CT_PluralCamelPossesive = "Gardiyanların";
    private const string CT_PluralCamelPrePosition = "Gardiyanlara";
    private const string CT_PluralLowerObjective = "gardiyanları";
    private const string CT_LowerPrePosition = "gardiyanla";
    private const string CT_LowerPositioning = "gardiyanda";
    private const string CT_LowerPersonal = "gardiyansın";

    #endregion CounterTerrorist

    private const string T_RoundEndParticle = "particles/zmtr/mahkum.vpcf";
    private const string CT_RoundEndParticle = "particles/zmtr/gardiyan.vpcf";

    private static List<ulong> GrabAllowedSteamIds => new List<ulong>()
    {
        76561198248447996,
        76561198797775438,//add extra and different steamids like this
    };

    #region Perms

    private const string Perm_Root = "@css/root";
    private const string Perm_Yonetim = "@css/yonetim";
    private const string Perm_Sorumlu = "@css/sorumlu";
    private const string Perm_Premium = "@css/premium";
    private const string Perm_Lider = "@css/lider";
    private const string Perm_LiderKredi = "@css/liderkredi";

    private const string Perm_Admin1 = "@css/admin1";
    private const string Perm_AdminKredi = "@css/adminkredi";

    private const string Perm_Komutcu = "@css/komutcu";

    internal const string Perm_Seviye1 = "@css/seviye1";
    internal const string Perm_Seviye2 = "@css/seviye2";
    internal const string Perm_Seviye3 = "@css/seviye3";
    internal const string Perm_Seviye4 = "@css/seviye4";
    internal const string Perm_Seviye5 = "@css/seviye5";
    internal const string Perm_Seviye6 = "@css/seviye6";
    internal const string Perm_Seviye7 = "@css/seviye7";
    internal const string Perm_Seviye8 = "@css/seviye8";
    internal const string Perm_Seviye9 = "@css/seviye9";
    internal const string Perm_Seviye10 = "@css/seviye10";
    internal const string Perm_Seviye11 = "@css/seviye11";
    internal const string Perm_Seviye12 = "@css/seviye12";
    internal const string Perm_Seviye13 = "@css/seviye13";
    internal const string Perm_Seviye14 = "@css/seviye14";
    internal const string Perm_Seviye15 = "@css/seviye15";
    internal const string Perm_Seviye16 = "@css/seviye16";
    internal const string Perm_Seviye17 = "@css/seviye17";
    internal const string Perm_Seviye18 = "@css/seviye18";
    internal const string Perm_Seviye19 = "@css/seviye19";
    internal const string Perm_Seviye20 = "@css/seviye20";
    internal const string Perm_Seviye21 = "@css/seviye21";
    internal const string Perm_Seviye22 = "@css/seviye22";
    internal const string Perm_Seviye23 = "@css/seviye23";
    internal const string Perm_Seviye24 = "@css/seviye24";
    internal const string Perm_Seviye25 = "@css/seviye25";
    internal const string Perm_Seviye26 = "@css/seviye26";
    internal const string Perm_Seviye27 = "@css/seviye27";
    internal const string Perm_Seviye28 = "@css/seviye28";
    internal const string Perm_Seviye29 = "@css/seviye29";
    internal const string Perm_Seviye30 = "@css/seviye30";

    #endregion Perms
}