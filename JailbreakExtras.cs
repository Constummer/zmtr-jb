using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using System.Drawing;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

[MinimumApiVersion(128)]
public partial class JailbreakExtras : BasePlugin, IPluginConfig<JailbreakExtrasConfig>
{
    private static HttpClient _httpClient;

    static JailbreakExtras()
    {
        _httpClient = new HttpClient();
    }

    public override string ModuleName => "JailbreakExtras";
    public override string ModuleAuthor => "Constummer";
    public override string ModuleDescription => "Extra jailbreak plugins";
    public override string ModuleVersion => "V.1.0.1";

    private int ModuleConfigVersion => 8;

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

    private static Color DefaultPlayerColor = Color.FromArgb(255, 255, 255, 255);
    private const ulong FButtonIndex = 34359738368;

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
                force_open();
            }
            return HookResult.Continue;
        });

        base.Load(hotReload);
    }

    private void force_ent_input(String name, String input)
    {
        var target = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>(name);

        foreach (var ent in target)
        {
            if (!ent.IsValid)
            {
                continue;
            }
            ent.AcceptInput(input);
        }
    }

    public void force_open()
    {
        force_ent_input("func_door", "Open");
        force_ent_input("func_movelinear", "Open");
        force_ent_input("func_door_rotating", "Open");
        force_ent_input("prop_door_rotating", "Open");
        force_ent_input("func_breakable", "Break");
    }

    public override void Unload(bool hotReload)
    {
        base.Unload(hotReload);
    }

    public void OnConfigParsed(JailbreakExtrasConfig config)
    {
        if (config.Version < ModuleConfigVersion)
        {
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
        }
        _Config = Config = config;
        SetLevelPermissionDictionary();
        config.Burry.BuryColor = Color.FromArgb(config.Burry.BurryColorR, config.Burry.BurryColorG, config.Burry.BurryColorB);
        //Re-assign after adjustments
        _Config = Config = config;
    }
}