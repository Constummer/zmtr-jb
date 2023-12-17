using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras : BasePlugin, IPluginConfig<JailConfig>
{
    public override string ModuleName => "JailbreakExtras";
    public override string ModuleAuthor => "Constummer";
    public override string ModuleDescription => "Extra jailbreak plugins";
    public override string ModuleVersion => "V. 1.0.1";

    private static readonly Dictionary<ulong, bool> ActiveGodMode = new();
    private static readonly Dictionary<ulong, Vector> DeathLocations = new();
    private static readonly Dictionary<ulong, Dictionary<ulong, string>> KilledPlayers = new();

    private static readonly Random _random = new Random();
    public JailConfig Config { get; set; } = new JailConfig();
    private JailPlugin? jailPlugin;

    static JailbreakExtras()
    {
        JailPlugin jailPlugin = new JailPlugin();
        JailPlugin.global_ctx = jailPlugin;
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
        JailPlugin.Config = this.Config = config;
        JailPlugin.lr.lr_stats.config = config;
        JailPlugin.lr.config = config;
        JailPlugin.warden.config = config;
        JailPlugin.warden.mute.config = config;

        JailPlugin.lr.lr_config_reload();
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
        if (jailPlugin != null)
        {
            JailPlugin.global_extras = this;
            jailPlugin.register_commands();
            jailPlugin.register_hook();
            jailPlugin.register_listener();
            // workaround to query global state!
        }
    }

    public override void Unload(bool hotReload)
    {
        if (jailPlugin != null)
        {
            VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Unhook(jailPlugin.OnTakeDamage, HookMode.Pre);
        }

        base.Unload(hotReload);
    }
}