using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace JailbreakExtras;

public partial class JailbreakExtras : BasePlugin
{
    public override string ModuleName => "JailbreakExtras";
    public override string ModuleAuthor => "Constummer";
    public override string ModuleDescription => "Extra jailbreak plugins";
    public override string ModuleVersion => "V.1.0.1";

    private static readonly Random _random = new Random();

    private static readonly Dictionary<ulong, bool> ActiveGodMode = new();
    private static readonly Dictionary<ulong, Vector> DeathLocations = new();
    private static readonly Dictionary<ulong, Dictionary<ulong, string>> KilledPlayers = new();

    private const ulong FButtonIndex = 34359738368;

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
        Console.WriteLine("Sucessfully started JB");
        //!!!!DO NOT CHANGE ORDER OF CALLS IN THIS METHOD !!!!!

        #region System Releated

        CreateDataFolder();
        DatabaseInit();
        LoadPlayerModels();

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
        //    Logger.LogInformation("All EntityOutput ({name}, {activator}, {caller}, {delay})", output.Description.Name, activator.DesignerName, caller.DesignerName, delay);

        //    return HookResult.Continue;
        //});
        Task.Run(() => { SharedMemoryConsumer.ReadData(); });
        //AddTimer(0.1f, () => {  }, TimerFlags.REPEAT);
        base.Load(hotReload);
    }

    public override void Unload(bool hotReload)
    {
        base.Unload(hotReload);
    }

    public class SharedMemoryConsumer
    {
        public static void ReadData()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 12345);
            listener.Start();

            while (true)
            {
                using (TcpClient client = listener.AcceptTcpClient())
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = new byte[10000];
                    int bytesRead = stream.Read(data, 0, data.Length);

                    var str = Encoding.UTF8.GetString(data, 0, bytesRead);
                    Console.WriteLine(str);
                    ulong.TryParse(str ?? "", out LatestWCommandUser);
                }
            }
        }
    }
}