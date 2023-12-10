using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras : BasePlugin
{
    public override string ModuleName => "JailbreakExtras";
    public override string ModuleAuthor => "Constummer";
    public override string ModuleDescription => "Extra jailbreak plugins";
    public override string ModuleVersion => "V. 1.0.1";

    private static readonly Dictionary<ulong, bool> ActiveGodMode = new();
    private static readonly Dictionary<ulong, Vector> DeathLocations = new();
    private static readonly Dictionary<ulong, Dictionary<ulong, string>> KilledPlayers = new();
    private static readonly List<CEnvBeam> Lasers = new();

    private static LaserConfigData LaserConfig = new LaserConfigData()
    {
        Radius = 75,//marker R
        Width = 2,//marker genisligi
        EdgeCount = 100,// marker kenarindaki line sayisi
        Color = CYAN,// marker color
    };

    private static readonly Random _random = new Random();

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

    public override void Load(bool hotReload)
    {
        HookEntityOutput("*", "*", (output, name, activator, caller, value, delay) =>
        {
            //Logger.LogInformation("All EntityOutput ({name}, {activator}, {caller}, {delay})", output.Description.Name, activator.DesignerName, caller.DesignerName, delay);

            return HookResult.Continue;
        });
        CallEvents();
        CallListeners();

        base.Load(hotReload);
    }
}