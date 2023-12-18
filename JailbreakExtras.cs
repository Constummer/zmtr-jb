using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras : BasePlugin, IPluginConfig<JailConfig>
{
    private static readonly JailbreakExtras instance = new();

    internal static JailbreakExtras Instance
    { get { return instance; } }

    public override string ModuleName => "JailbreakExtras";
    public override string ModuleAuthor => "Constummer";
    public override string ModuleDescription => "Extra jailbreak plugins";
    public override string ModuleVersion => "V.1.0.1";

    private static readonly Dictionary<ulong, bool> ActiveGodMode = new();
    private static readonly Dictionary<ulong, Vector> DeathLocations = new();
    private static readonly Dictionary<ulong, Dictionary<ulong, string>> KilledPlayers = new();

    private static readonly Random _random = new Random();
    public JailConfig Config { get; set; } = new JailConfig();
    public static JailConfig _config { get; set; } = new JailConfig();

    static JailbreakExtras()
    {
    }

    private static readonly string[] BaseRequiresPermissions = new[]
    {
        "@css/root",
        "@jail/debug",
        "@css/ban",
        "@css/chat",
        "@css/slay",
        "@css/cheats",
        "@css/generic",
        "@css/vip"
    };

    public void OnConfigParsed(JailConfig config)
    {
        _config = Config = config;
        lr.lr_stats.config = config;
        lr.config = config;
        warden.config = config;
        warden.mute.config = config;

        lr.lr_config_reload();
    }

    public override void Load(bool hotReload)
    {
        Console.WriteLine("Sucessfully started JB");
        //!!!!DO NOT CHANGE ORDER OF CALLS IN THIS METHOD !!!!!

        #region System Releated

        CreateDataFolder();
        DatabaseInit();
        LoadPlayerModels();

        #endregion System Releated

        #region OtherPlugins

        BlockRadioCommandsLoad();
        CS2JailbreakLoad();

        #endregion OtherPlugins

        #region CSS releated

        CallEvents();
        CallListeners();
        CallCommandListeners();
        AddTimers();

        #endregion CSS releated

        //HookEntityOutput("*", "*", (output, name, activator, caller, value, delay) =>
        //{
        //    Logger.LogInformation("All EntityOutput ({name}, {activator}, {caller}, {delay})", output.Description.Name, activator.DesignerName, caller.DesignerName, delay);

        //    return HookResult.Continue;
        //});

        base.Load(hotReload);
    }

    private void CS2JailbreakLoad()
    {
        register_commands();
        register_hook();
        register_listener();
        // workaround to query global state!
    }

    public override void Unload(bool hotReload)
    {
        VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Unhook(OnTakeDamage, HookMode.Pre);

        base.Unload(hotReload);
    }

    private void Addtimer(float delay, Action value, TimerFlags? timerFlags)
    {
        base.AddTimer(delay, value, timerFlags);
    }
}