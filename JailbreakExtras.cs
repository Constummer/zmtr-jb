using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using System.Drawing;
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

    private int ModuleConfigVersion => 9;

    private static readonly Random _random = new Random();
    public JailbreakExtrasConfig Config { get; set; } = new JailbreakExtrasConfig();
    public static JailbreakExtrasConfig _Config { get; set; } = new JailbreakExtrasConfig();

    private static bool LrActive = false;
    private static Dictionary<ulong, bool> ActiveGodMode = new();
    private static Dictionary<ulong, Vector> DeathLocations = new();
    private static Dictionary<ulong, bool> HideFoots = new();
    private static Dictionary<ulong, bool> HookPlayers = new();
    private static List<ulong> HookDisablePlayers = new();
    private static Dictionary<ulong, Dictionary<ulong, string>> KilledPlayers = new();
    private static readonly Dictionary<CCSPlayerController, bool> bUsingPara = new();
    private static Dictionary<ulong, string> PlayerNamesDatas = new();
    private static ulong? LatestWCommandUser { get; set; }

    private static readonly string[] BaseRequiresPermissions = new[]
    {
        "@css/admin1"
    };

    public override void Load(bool hotReload)
    {
        //!!!!DO NOT CHANGE ORDER OF CALLS IN THIS METHOD !!!!!

        #region System Releated

        LoadCredit();
        CreateDataFolder();
        Database();
        LoadPlayerModels();
        Task.Run(() => { SharedMemoryConsumer.StartListenerData(); });

        #endregion System Releated

        #region OtherPlugins

        BlockRadioCommandsLoad();
        BlockGroupCommandsLoad();

        #endregion OtherPlugins

        #region CSS releated

        CallEvents();
        CallListeners();
        CallCommandListeners();
        AddTimers();

        #endregion CSS releated

        HookEntityOutput("*", "*", (output, name, activator, caller, value, delay) =>
        {
            if (name == "OnUnblockedClosing" || name == "OnBlockedOpening")
            {
                //ForceOpen();
            }
            return HookResult.Continue;
        });

        base.Load(hotReload);
    }

    public override void Unload(bool hotReload)
    {
        base.Unload(hotReload);
    }
}