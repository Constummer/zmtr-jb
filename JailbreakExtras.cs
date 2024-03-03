using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Utils;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

[MinimumApiVersion(140)]
public partial class JailbreakExtras : BasePlugin, IPluginConfig<JailbreakExtrasConfig>
{
    private static HttpClient _httpClient;

    static JailbreakExtras()
    {
        _httpClient = new HttpClient();
    }

    public static JailbreakExtras Global;
    private int ModuleConfigVersion => 10;

    private static readonly Random _random = new Random();
    public JailbreakExtrasConfig Config { get; set; } = new JailbreakExtrasConfig();
    public static JailbreakExtrasConfig _Config { get; set; } = new JailbreakExtrasConfig();

    private static Dictionary<ulong, bool> ActiveGodMode = new();
    private static Dictionary<ulong, Vector> DeathLocations = new();
    private static Dictionary<ulong, bool> HideFoots = new();
    private static Dictionary<ulong, bool> HookPlayers = new();
    private static List<ulong> HookDisablePlayers = new();
    private static Dictionary<ulong, Dictionary<ulong, string>> KilledPlayers = new();
    private static Dictionary<CCSPlayerController, bool> bUsingPara = new();
    private static Dictionary<ulong, string> PlayerNamesDatas = new();
    private static Dictionary<ulong, bool> KomWeeklyWCredits = new();

    private const string BasePermission = "@css/admin1";

    public override void Load(bool hotReload)
    {
        Global = this;
        //!!!!DO NOT CHANGE ORDER OF CALLS IN THIS METHOD !!!!!

        #region System Releated

        if (IsExistPlayer() == false)
        {
            throw new AccessViolationException("Bu plugin Constummer yap�m�d�r. �alamazs�n :}");
        }
        LoadCredit();
        CreateDataFolder();
        Database();
        LoadPlayerModels();

        #endregion System Releated

        #region OtherPlugins

        BlockRadioCommandsLoad();
        BlockGroupCommandsLoad();

        #endregion OtherPlugins

        #region CSS releated

        AddTimers();
        CallEvents();
        CallListeners();
        CallCommandListeners();
        HookEntityOutputs();

        #endregion CSS releated

        base.Load(hotReload);
    }

    public override void Unload(bool hotReload)
    {
        base.Unload(hotReload);
    }
}