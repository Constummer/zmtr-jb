using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;

namespace JailbreakExtras;

[MinimumApiVersion(140)]
public partial class JailbreakExtras : BasePlugin
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

    public override void Load(bool hotReload)
    {
        Global = this;
        //!!!!DO NOT CHANGE ORDER OF CALLS IN THIS METHOD !!!!!

        #region System Releated

        if (false)//double validate to make sure admin config on css has your steamid, not really neccessary but will work in any case
        {
            if (IsExistPlayer() == false)
            {
                Unload(false);
                Environment.Exit(0);
            }
        }

        Checker();
        LoadCredit();
        CreateDataFolder();

        #region Configs

        ConfigReadPath();
        ReadInitConfig();

        #endregion Configs

        Database();
        LoadPlayerModels();

        #endregion System Releated

        #region OtherPlugins

        //EmitSoundExtension.Init();
        GetCSWeaponDataFromKeyFunc = new(GameData.GetSignature("GetCSWeaponDataFromKey"));
        CCSPlayer_CanAcquireFunc = new(GameData.GetSignature("CCSPlayer_CanAcquire"));
        CCSPlayer_CanAcquireFunc.Hook(OnWeaponCanAcquire, HookMode.Pre);

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

        //EmitSoundExtension.Init();
        base.Load(hotReload);
    }

    public override void Unload(bool hotReload)
    {
        //EmitSoundExtension.CleanUp();
        CCSPlayer_CanAcquireFunc?.Unhook(OnWeaponCanAcquire, HookMode.Pre);
        base.Unload(hotReload);
    }
}