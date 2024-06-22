//using CounterStrikeSharp.API;
//using CounterStrikeSharp.API.Core;
//using CounterStrikeSharp.API.Modules.Commands;
//using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
//using CounterStrikeSharp.API.Modules.Utils;
//using System.Drawing;

//namespace AmbrosianSutOl;

//public class AmbrosianSutOl : BasePlugin
//{
//    public override string ModuleName => "Süt Ol";
//    public override string ModuleAuthor => "Ambrosian";
//    public override string ModuleVersion => "1.0";
//    public bool ElSonu { get; set; } = false;

//    private static readonly int?[] SutMu = new int?[65];

//    public int TSayi;

//    public override void Load(bool hotReload)
//    {
//        GetCSWeaponDataFromKeyFunc = new(GameData.GetSignature("GetCSWeaponDataFromKey"));
//        CCSPlayer_CanAcquireFunc = new(GameData.GetSignature("CCSPlayer_CanAcquire"));
//        CCSPlayer_CanAcquireFunc.Hook(OnWeaponCanAcquire, HookMode.Pre);

//        RegisterEventHandler<EventRoundStart>(RoundBasi);
//        RegisterEventHandler<EventRoundEnd>(RoundSonu);
//        RegisterEventHandler<EventPlayerDeath>(OyuncuOldugunde);
//        AddCommand("css_sutol", "Süt Ol", OnSutOlCommand);
//    }

//    public override void Unload(bool hotReload)
//    {
//        CCSPlayer_CanAcquireFunc.Unhook(OnWeaponCanAcquire, HookMode.Pre);

//        base.Unload(hotReload);
//    }

//    public void OnSutOlCommand(CCSPlayerController? x, CommandInfo command)
//    {
//        if (x == null || !x.IsValid || x.IsBot || x.IsHLTV) return;

//        if (!IsAlive(x))
//        {
//            x.PrintToChat(" \x02[Volts]\x10 Bu komutu sadece yaşayan oyuncular kullanabilir.");
//        }

//        if (ElSonu)
//        {
//            x.PrintToChat(" \x02[Volts]\x10 Bu komut el sonu kullanılamaz.");
//        }

//        if ((CsTeam)x.TeamNum != CsTeam.Terrorist)
//        {
//            x.PrintToChat(" \x02[Volts]\x10 Bu komutu sadece isyancılar kullanabilir.");
//        }

//        if (SutMu[x.Index] == 1)
//        {
//            x.PrintToChat(" \x02[Volts]\x10 Bu komutu süt olmayan isyancılar kullanabilir.");
//        }

//        var SutOlan = x.PlayerName;
//        Server.PrintToChatAll(" \x02[Volts] \x0C" + SutOlan + " \x10isimli oyuncu süt oldu.");
//        SutMu[x.Index] = 1;
//        x.RemoveWeapons();
//        if (x.PlayerPawn != null && x.PlayerPawn.Value != null)
//        {
//            x.PlayerPawn.Value.Render = Color.FromArgb(255, 0, 255);
//        }
//    }

//    private HookResult RoundSonu(EventRoundEnd @event, GameEventInfo info)
//    {
//        ElSonu = true;
//        return HookResult.Continue;
//    }

//    private HookResult RoundBasi(EventRoundStart @event, GameEventInfo info)
//    {
//        ElSonu = false;
//        Utilities.GetPlayers().Where(IsValid).ToList().ForEach(x =>
//        {
//            if (IsValid(x) == false)
//            {
//                return;
//            }
//            SutMu[x.Index] = 0;
//        });
//        return HookResult.Continue;
//    }

//    private HookResult OyuncuOldugunde(EventPlayerDeath @event, GameEventInfo info)
//    {
//        TSayi = YasayanT(CsTeam.Terrorist);

//        ///TODO: check if the <see cref="TSayi"></see> must be equal to 1 or 2, i believe it must be 2 since this event triggers like in secs.
//        if (TSayi == 2)
//        {
//            Utilities.GetPlayers().Where(IsValid).ToList().ForEach(x =>
//            {
//                if (IsValid(x) == false)
//                {
//                    return;
//                }

//                SutMu[x.Index] = 0;
//            });
//        }
//        return HookResult.Continue;
//    }

//    public static bool IsValid(CCSPlayerController? x)
//    {
//        return x != null && x.IsValid && x.PlayerPawn.IsValid && x.Connected == PlayerConnectedState.PlayerConnected;
//    }

//    public static bool IsAlive(CCSPlayerController x)
//    {
//        return x?.PlayerPawn.Value?.LifeState == (byte)LifeState_t.LIFE_ALIVE;
//    }

//    public static int YasayanT(CsTeam? csTeam = null)
//    {
//        return Utilities.GetPlayers().Count(player => IsValid(player) && IsAlive(player) && (csTeam.HasValue ? csTeam.Value == player.Team : true));
//    }

//    public enum AcquireResult : int
//    {
//        Allowed = 0,
//        InvalidItem,
//        AlreadyOwned,
//        AlreadyPurchased,
//        ReachedGrenadeTypeLimit,
//        ReachedGrenadeTotalLimit,
//        NotAllowedByTeam,
//        NotAllowedByMap,
//        NotAllowedByMode,
//        NotAllowedForPurchase,
//        NotAllowedByProhibition,
//    };

//    // Possible results for CSPlayer::CanAcquire
//    public enum AcquireMethod : int
//    {
//        PickUp = 0,
//        Buy,
//    };

//    public MemoryFunctionWithReturn<CCSPlayer_ItemServices, CEconItemView, AcquireMethod, NativeObject, AcquireResult> CCSPlayer_CanAcquireFunc;

//    public MemoryFunctionWithReturn<int, string, CCSWeaponBaseVData> GetCSWeaponDataFromKeyFunc;

//    public HookResult OnWeaponCanAcquire(DynamicHook hook)
//    {
//        var x = hook.GetParam<CCSPlayer_ItemServices>(0).Pawn.Value!.Controller.Value!.As<CCSPlayerController>();

//        if (x == null || !x.IsValid || !x.PawnIsAlive)
//            return HookResult.Continue;

//        if (hook.GetParam<AcquireMethod>(2) != AcquireMethod.PickUp)
//        {
//            hook.SetReturn(AcquireResult.AlreadyOwned);
//        }
//        else
//        {
//            hook.SetReturn(AcquireResult.InvalidItem);
//        }
//        if (IsValid(x) && IsAlive(x) && SutMu[x.Index] == 1)
//        {
//            return HookResult.Stop;
//        }
//        return HookResult.Continue;
//    }
//}