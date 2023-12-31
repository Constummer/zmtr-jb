using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

[MinimumApiVersion(128)]
public partial class JailbreakExtras : BasePlugin, IPluginConfig<JailbreakExtrasConfig>
{
    public override string ModuleName => "JailbreakExtras";
    public override string ModuleAuthor => "Constummer";
    public override string ModuleDescription => "Extra jailbreak plugins";
    public override string ModuleVersion => "V.1.0.1";

    private static readonly Random _random = new Random();
    public JailbreakExtrasConfig Config { get; set; } = new JailbreakExtrasConfig();
    public static JailbreakExtrasConfig _Config { get; set; } = new JailbreakExtrasConfig();

    private static bool LrActive = false;
    private static readonly Dictionary<ulong, bool> ActiveGodMode = new();
    private static readonly Dictionary<ulong, Vector> DeathLocations = new();
    private static readonly Dictionary<ulong, bool> HideFoots = new();
    private static readonly Dictionary<ulong, bool> HookPlayers = new();
    private static readonly List<ulong> HookDisablePlayers = new();
    private static readonly Dictionary<ulong, Dictionary<ulong, string>> KilledPlayers = new();
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
        DatabaseInit();
        LoadPlayerModels();
        Task.Run(() => { SharedMemoryConsumer.StartListenerData(); });

        #endregion System Releated

        #region OtherPlugins

        BlockRadioCommandsLoad();

        #endregion OtherPlugins

        #region CSS releated

        CallEvents();
        CallListeners();
        CallCommandListeners();
        AddTimers();

        #endregion CSS releated

        //HookEntityOutput("*", "*", (output, name, activator, caller, value, delay) =>
        //{
        //    Console.WriteLine(name);
        //    return HookResult.Continue;
        //});

        base.Load(hotReload);
    }

    public override void Unload(bool hotReload)
    {
        base.Unload(hotReload);
    }

    public void OnConfigParsed(JailbreakExtrasConfig config)
    {
        _Config = Config = config;
        config.BuryColor = Color.FromArgb(config.BurryColorR, config.BurryColorG, config.BurryColorB);
        config.LaserColor = Color.FromArgb(config.LaserColorR, config.LaserColorG, config.LaserColorB);
        //Re-assign after adjustments
        _Config = Config = config;
    }
}