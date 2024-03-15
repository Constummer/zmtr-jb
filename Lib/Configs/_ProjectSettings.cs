namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static string Prefix => $" {CC.LR}[ZMTR]";

    private const string BasePermission = "@css/admin1";
    private const string WardenDcWebHook = "https://discord.com/api/webhooks/1194758709344215090/-XRiPj35x-KTHRtAyWlB5i1I16lFylHl_17we6SOS5HbYY5JCFPQYiOjYot6trvQiUcR";
    private const string Total_T_CTDcWebHook = "https://discord.com/api/webhooks/1200909469496905888/7sNtxOzC3t8PgDmfuzgzRIIkp3u_Oj6evcGAY3pIcmRZC75eVhf6e2-Q4WbsBdCdVdua";

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