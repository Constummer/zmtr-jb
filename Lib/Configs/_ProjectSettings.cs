namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static string Prefix => $" {CC.LR}[ZMTR]";

    #region Terrorist

    private static string T_AllCap => $"MAHKÛM";
    private static string T_AllLower => $"mahkûm";
    private static string T_CamelCase => $"Mahkûm";
    private static string T_PluralCamel => $"Mahkûmlar";
    private static string T_PluralCamelPossesive => $"Mahkûmların";
    private static string T_PluralLowerObjective => $"mahkûmları";
    private static string T_LowerPrePosition => $"mahkûmla";
    private static string T_LowerPositioning => $"mahkûmda";
    private static string T_LowerPersonal => $"mahkûmsun";

    #endregion Terrorist

    #region CounterTerrorist

    private static string CT_AllCap => $"GARDİYAN";
    private static string CT_AllLower => $"gardiyan";
    private static string CT_CamelCase => $"Gardiyan";
    private static string CT_PluralCamel => $"Gardiyanlar";
    private static string CT_PluralCamelPossesive => $"Gardiyanların";
    private static string CT_PluralCamelPrePosition => $"Gardiyanlara";
    private static string CT_PluralLowerObjective => $"gardiyanları";
    private static string CT_LowerPrePosition => $"gardiyanla";
    private static string CT_LowerPositioning => $"gardiyanda";
    private static string CT_LowerPersonal => $"gardiyansın";

    #endregion CounterTerrorist

    private static string T_RoundEndParticle => "particles/zmtr/mahkum.vpcf";
    private static string CT_RoundEndParticle => "particles/zmtr/gardiyan.vpcf";

    private static List<ulong> GrabAllowedSteamIds => new List<ulong>()
    {
        76561198248447996,
        76561198248447996,//add extra and different steamids like this
    };
}