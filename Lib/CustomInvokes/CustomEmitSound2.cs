//using CounterStrikeSharp.API;
//using CounterStrikeSharp.API.Core;
//using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
//using Microsoft.Extensions.Logging;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices;
//using System.Text;

//namespace JailbreakExtras;

//public static class EmitSoundExtension
//{
//    // TODO: these are for libserver.so, haven't found these on windows yet
//    private static MemoryFunctionVoid<CBaseEntity, string, int, float, float> CBaseEntity_EmitSoundParamsFunc = new("\\x48\\xB8\\x2A\\x2A\\x2A\\x2A\\x2A\\x2A\\x2A\\x2A\\x55\\x48\\x89\\xE5\\x41\\x55\\x41\\x54\\x49\\x89\\xFC\\x53\\x48\\x89\\xF3");

//    private static MemoryFunctionWithReturn<nint, nint, uint, uint, short, ulong, ulong> CSoundOpGameSystem_StartSoundEventFunc = new("\\x48\\xb8\\x00\\x00\\x00\\x00\\x08\\x00\\x00\\xc0\\x55\\x48\\x89\\xe5\\x41\\x57\\x45\\x89\\xc7\\x41\\x56\\x41\\x55\\x4c\\x8d\\x6d\\xc0\\x41\\x54\\x41\\x89\\xcc\\x53\\x48\\x89\\xfb\\x48\\x8d\\x3d");
//    private static MemoryFunctionVoid<nint, nint, ulong, nint, nint, short, byte> CSoundOpGameSystem_SetSoundEventParamFunc = new("\\x55\\x48\\x89\\xe5\\x41\\x57\\x41\\x56\\x49\\x89\\xd6\\x48\\x89\\xca\\x41\\x55\\x49\\x89\\xf5\\x41\\x54\\x49\\x89\\xfc\\x53\\x48\\x89\\xcb\\x48\\x83\\xec\\x18\\x48\\x8d\\x05");

//    internal static void Init()
//    {
//        CSoundOpGameSystem_StartSoundEventFunc.Hook(CSoundOpGameSystem_StartSoundEventFunc_PostHook, HookMode.Post);
//    }

//    internal static void CleanUp()
//    {
//        CSoundOpGameSystem_StartSoundEventFunc.Unhook(CSoundOpGameSystem_StartSoundEventFunc_PostHook, HookMode.Post);
//    }

//    [ThreadStatic]
//    private static IReadOnlyDictionary<string, float>? CurrentParameters;

//    /// <summary>
//    /// Emit a sound event by name (e.g., "Weapon_AK47.Single").
//    /// TODO: parameters passed in here only seem to work for sound events shipped with the game, not workshop ones.
//    /// </summary>
//    public static void EmitSound(this CBaseEntity entity, string soundName, IReadOnlyDictionary<string, float>? parameters = null)
//    {
//        if (!entity.IsValid)
//        {
//            throw new ArgumentException("Entity is not valid.");
//        }

//        try
//        {
//            // We call CBaseEntity::EmitSoundParams,
//            // which calls a method that returns an ID that we can use
//            // to modify the playing sound.

//            CurrentParameters = parameters;

//            // Pitch, volume etc aren't actually used here
//            CBaseEntity_EmitSoundParamsFunc.Invoke(entity, soundName, 100, 1f, 0f);
//        }
//        finally
//        {
//            CurrentParameters = null;
//        }
//    }

//    private static HookResult CSoundOpGameSystem_StartSoundEventFunc_PostHook(DynamicHook hook)
//    {
//        //if (CurrentParameters is not { Count: > 0 })
//        //{
//        //    return HookResult.Continue;
//        //}

//        var pSoundOpGameSystem = hook.GetParam<nint>(0);
//        var pFilter = hook.GetParam<nint>(1);
//        var soundEventId = hook.GetReturn<ulong>(2);
//        //return HookResult.Continue;

//        Server.PrintToChatAll($"{pSoundOpGameSystem},{pFilter},{soundEventId}");
//        EmitSoundExtension.EmitSound(new CBaseEntity(pSoundOpGameSystem), "Weapon_Knife.Hit", null);

//        foreach (var parameter in CurrentParameters)
//        {
//            Server.PrintToChatAll($"{parameter.Key}...{parameter.Value}");

//            CSoundOpGameSystem_SetSoundEventParam(pSoundOpGameSystem, pFilter,
//                soundEventId, parameter.Key, parameter.Value);
//        }

//        return HookResult.Continue;
//    }

//    [StructLayout(LayoutKind.Sequential)]
//    private readonly struct FloatParamData
//    {
//        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
//        private readonly uint _type1;

//        private readonly uint _type2;

//        private readonly uint _size1;
//        private readonly uint _size2;

//        private readonly float _value;
//        private readonly uint _padding;
//        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable

//        public FloatParamData(float value)
//        {
//            _type1 = 1;
//            _type2 = 8;

//            _size1 = 4;
//            _size2 = 4;

//            _value = value;
//            _padding = 0;
//        }
//    }

//    private static unsafe void CSoundOpGameSystem_SetSoundEventParam(nint pSoundOpGameSystem, nint pFilter,
//        ulong soundEventId, string paramName, float value)
//    {
//        var data = new FloatParamData(value);
//        var nameByteCount = Encoding.UTF8.GetByteCount(paramName);

//        var pData = Unsafe.AsPointer(ref data);
//        var pName = stackalloc byte[nameByteCount + 1];

//        Encoding.UTF8.GetBytes(paramName, new Span<byte>(pName, nameByteCount));

//        CSoundOpGameSystem_SetSoundEventParamFunc.Invoke(pSoundOpGameSystem, pFilter, soundEventId, (nint)pName, (nint)pData, 0, 0);
//    }
//}

/*
training.train_activetestingcomplete_12 : param size = 34, field size = 50
training.train_activetestingcomplete_12b : param size = 34, field size = 50
training.train_activetestingcomplete_13 : param size = 34, field size = 50
training.train_activetestingcomplete_14 : param size = 34, field size = 50
training.train_activetestingcomplete_15 : param size = 34, field size = 50
training.train_activetestingcomplete_16 : param size = 34, field size = 50
training.train_activetestingcomplete_17 : param size = 34, field size = 50
training.train_activetestingcomplete_18 : param size = 34, field size = 50
training.train_activetestingcomplete_19 : param size = 34, field size = 50
training.train_activetestingcomplete_20 : param size = 34, field size = 50
training.train_activetestingcomplete_21 : param size = 34, field size = 50
training.train_activetestingcomplete_22 : param size = 34, field size = 50
training.train_activetestingcomplete_22b : param size = 34, field size = 50
training.train_activetestingcomplete_23 : param size = 34, field size = 50
training.train_activetestingcomplete_24 : param size = 34, field size = 50
training.train_activetestingcomplete_25 : param size = 34, field size = 50
training.train_activetestingcomplete_26 : param size = 34, field size = 50
training.train_activetestingcomplete_27 : param size = 34, field size = 50
training.train_activetestingcomplete_28 : param size = 34, field size = 50
training.train_activetestingintro_01 : param size = 34, field size = 50
training.train_activetestingintro_02 : param size = 34, field size = 50
training.train_activetestingintro_03 : param size = 34, field size = 50
training.train_bodydamagechestshot_01 : param size = 34, field size = 50
training.train_bodydamagechestshot_02 : param size = 34, field size = 50
training.train_bodydamagechestshot_03 : param size = 34, field size = 50
training.train_bodydamagechestshot_04 : param size = 34, field size = 50
training.train_bodydamagechestshot_05 : param size = 34, field size = 50
training.train_bodydamageheadshot_01 : param size = 34, field size = 50
training.train_bodydamageheadshot_01b : param size = 34, field size = 50
training.train_bodydamageheadshot_02 : param size = 34, field size = 50
training.train_bodydamageheadshot_03 : param size = 34, field size = 50
training.train_bodydamageheadshot_04 : param size = 34, field size = 50
training.train_bodydamageheadshot_05 : param size = 34, field size = 50
training.train_bodydamageheadshot_06 : param size = 34, field size = 50
training.train_bodydamageintro_01 : param size = 34, field size = 50
training.train_bodydamagelegshot_01 : param size = 34, field size = 50
training.train_bodydamagelegshot_02 : param size = 34, field size = 50
training.train_bodydamagelegshot_03 : param size = 34, field size = 50
training.train_bodydamagelegshot_04 : param size = 34, field size = 50
training.train_bodydamagelegshot_05 : param size = 34, field size = 50
training.train_bodydamagelegshot_05b : param size = 34, field size = 50
training.train_bodydamageoutofammo_01 : param size = 34, field size = 50
training.train_bodydamageoutofammo_02 : param size = 34, field size = 50
training.train_bodydamageoutofammo_03 : param size = 34, field size = 50
training.train_bodydamagestart_01 : param size = 34, field size = 50
training.train_bodydamagestart_02 : param size = 34, field size = 50
training.train_bodydamagestart_03 : param size = 34, field size = 50
training.train_bodydamagestart_04 : param size = 34, field size = 50
training.train_bodydamagestart_05 : param size = 34, field size = 50
training.train_bodydamagestart_05a : param size = 34, field size = 50
training.train_bodydamagestart_06 : param size = 34, field size = 50
training.train_bodydamagetimercomplete_01 : param size = 34, field size = 50
training.train_bodydamagetimercomplete_02 : param size = 34, field size = 50
training.train_bodydamagetimercomplete_02b : param size = 34, field size = 50
training.train_bodydamagetimerstart_01 : param size = 34, field size = 50
training.train_bodydamagetimerstart_012.wav : param size = 34, field size = 50
training.train_bodydamagetimerstart_013.wav : param size = 34, field size = 50
training.train_bodydamagetimerstart_014.wav : param size = 34, field size = 50
training.train_bodydamagetimerstart_015.wav : param size = 34, field size = 50
training.train_bodydamagetimerstart_016.wav : param size = 34, field size = 50
training.train_bodydamagetimerstart_017.wav : param size = 34, field size = 50
training.train_bodydamagetimerstart_018.wav : param size = 34, field size = 50
training.train_bombplanta_01 : param size = 34, field size = 50
training.train_bombplanta_02 : param size = 34, field size = 50
training.train_bombplantbcomplete_01 : param size = 34, field size = 50
training.train_bombplantbcomplete_02 : param size = 34, field size = 50
training.train_bombplantbcomplete_02b : param size = 34, field size = 50
training.train_bombplantbdefusing_01 : param size = 34, field size = 50
training.train_bombplantbdefusing_01b : param size = 34, field size = 50
training.train_bombplantbdefusing_01c : param size = 34, field size = 50
training.train_bombplantbfail_01 : param size = 34, field size = 50
training.train_bombplantbfail_02 : param size = 34, field size = 50
training.train_bombplantbfail_03 : param size = 34, field size = 50
training.train_bombplantbfail_04 : param size = 34, field size = 50
training.train_bombplantbfail_04b : param size = 34, field size = 50
training.train_bombplantbfail_04c : param size = 34, field size = 50
training.train_bombplantbfail_05 : param size = 34, field size = 50
training.train_bombplantbfail_05b : param size = 34, field size = 50
training.train_bombplantbfail_06 : param size = 34, field size = 50
training.train_bombplantbfail_06b : param size = 34, field size = 50
training.train_bombplantintro_01 : param size = 34, field size = 50
training.train_bombplantintro_02 : param size = 34, field size = 50
training.train_bombplantintro_03 : param size = 34, field size = 50
training.train_burstblockertestcompleted_01 : param size = 34, field size = 50
training.train_burstblockertestcompleted_02 : param size = 34, field size = 50
training.train_burstblockertestfailed_01 : param size = 34, field size = 50
training.train_burstblockertestfailed_02 : param size = 34, field size = 50
training.train_burstblockertestfailed_03 : param size = 34, field size = 50
training.train_burstblockerteststart_01 : param size = 34, field size = 50
training.train_burstblockerteststart_02 : param size = 34, field size = 50
training.train_bursttestfailed_01 : param size = 34, field size = 50
training.train_bursttestfailed_02 : param size = 34, field size = 50
training.train_failure_01 : param size = 34, field size = 50
training.train_failure_02 : param size = 34, field size = 50
training.train_failure_03 : param size = 34, field size = 50
training.train_failure_03b : param size = 34, field size = 50
training.train_failureammo_01 : param size = 34, field size = 50
training.train_failuretime_02 : param size = 34, field size = 50
training.train_flashbangcompleted_01 : param size = 34, field size = 50
training.train_flashbangcompleted_02 : param size = 34, field size = 50
training.train_flashbangcompleted_03 : param size = 34, field size = 50
training.train_flashbangcompleted_04 : param size = 34, field size = 50
training.train_flashbangcompleted_05 : param size = 34, field size = 50
training.train_flashbangfail_01 : param size = 34, field size = 50
training.train_flashbangfail_01b : param size = 34, field size = 50
training.train_flashbangfail_02 : param size = 34, field size = 50
training.train_flashbangfail_03 : param size = 34, field size = 50
training.train_flashbangfail_04 : param size = 34, field size = 50
training.train_flashbangflashed_01 : param size = 34, field size = 50
training.train_flashbangflashed_02 : param size = 34, field size = 50
training.train_flashbangflashed_03 : param size = 34, field size = 50
training.train_flashbangflashed_04 : param size = 34, field size = 50
training.train_flashbangflashed_05 : param size = 34, field size = 50
training.train_flashbangintro_01 : param size = 34, field size = 50
training.train_flashbangintro_01a : param size = 34, field size = 50
training.train_flashbangintro_01b : param size = 34, field size = 50
training.train_flashbangmultifail_01 : param size = 34, field size = 50
training.train_flashbangmultifail_012.wav : param size = 34, field size = 50
training.train_flashbangnotflashed_01 : param size = 34, field size = 50
training.train_flashbangstart_02 : param size = 34, field size = 50
training.train_grenadetestactive_01 : param size = 34, field size = 50
training.train_grenadetestactive_01b : param size = 34, field size = 50
training.train_grenadetestactive_02 : param size = 34, field size = 50
training.train_grenadetestactive_03 : param size = 34, field size = 50
training.train_grenadetestactive_04 : param size = 34, field size = 50
training.train_grenadetestactive_05 : param size = 34, field size = 50
training.train_grenadetestactive_06 : param size = 34, field size = 50
training.train_grenadetestallblownup_01 : param size = 34, field size = 50
training.train_grenadetestcompletebad_01 : param size = 34, field size = 50
training.train_grenadetestcompletebad_01b : param size = 34, field size = 50
training.train_grenadetestcompletegood_01 : param size = 34, field size = 50
training.train_grenadeteststart_01 : param size = 34, field size = 50
training.train_grenadeteststart_02 : param size = 34, field size = 50
training.train_grenadeteststart_03 : param size = 34, field size = 50
training.train_guntestcomplete_01 : param size = 34, field size = 50
training.train_guntestcomplete_02 : param size = 34, field size = 50
training.train_guntestcomplete_03 : param size = 34, field size = 50
training.train_guntestcomplete_04 : param size = 34, field size = 50
training.train_guntestcomplete_05 : param size = 34, field size = 50
training.train_guntestcomplete_052.wav : param size = 34, field size = 50
training.train_guntestgoing_01 : param size = 34, field size = 50
training.train_guntestnotshooting_01 : param size = 34, field size = 50
training.train_gunteststart_01 : param size = 34, field size = 50
training.train_gunteststart_02 : param size = 34, field size = 50
training.train_guntestwait_01 : param size = 34, field size = 50
training.train_guntestwait_012.wav : param size = 34, field size = 50
training.train_guntestwait_013.wav : param size = 34, field size = 50
training.train_guntestwait_014.wav : param size = 34, field size = 50
training.train_guntestwait_01_b : param size = 34, field size = 50
training.train_guntestwait_02 : param size = 34, field size = 50
training.train_guntestwait_03 : param size = 34, field size = 50
training.train_guntestwait_03_b : param size = 34, field size = 50
training.train_idtestcompleted_01 : param size = 34, field size = 50
training.train_idtestcompleted_012.wav : param size = 34, field size = 50
training.train_idtestcompleted_02 : param size = 34, field size = 50
training.train_idtestfailed_01 : param size = 34, field size = 50
training.train_idtestfailed_012.wav : param size = 34, field size = 50
training.train_idtestfailed_02 : param size = 34, field size = 50
training.train_idtestfailed_03 : param size = 34, field size = 50
training.train_idtestfailed_04 : param size = 34, field size = 50
training.train_idteststart_01 : param size = 34, field size = 50
training.train_idteststart_02 : param size = 34, field size = 50
training.train_idteststart_03 : param size = 34, field size = 50
training.train_idtestwaiting_01 : param size = 34, field size = 50
training.train_idtestwaiting_02 : param size = 34, field size = 50
training.train_mainintro_01 : param size = 34, field size = 50
training.train_mainintro_01a : param size = 34, field size = 50
training.train_mainintro_01a2.wav : param size = 34, field size = 50
training.train_mainintro_01a3.wav : param size = 34, field size = 50
training.train_misc_01 : param size = 34, field size = 50
training.train_misc_01b : param size = 34, field size = 50
training.train_misc_02 : param size = 34, field size = 50
tuscan.TSpawn : param size = 24, field size = 222
tuscan.TSpawn.OceanTone : param size = 14, field size = 266
tuscan.TSpawn.Waves : param size = 26, field size = 290
tuscan.TSpawn.Waves2 : param size = 26, field size = 290
UI.ArmsRace.BecomeMatchLeader : param size = 42, field size = 122
UI.ArmsRace.BecomeTeamLeader : param size = 38, field size = 122
UI.ArmsRace.Demoted : param size = 38, field size = 122
UI.ArmsRace.FinalKill_Knife : param size = 50, field size = 148
UI.ArmsRace.FinalKill_Tone : param size = 46, field size = 170
UI.ArmsRace.Kill1 : param size = 46, field size = 122
UI.ArmsRace.PlayerDeath : param size = 42, field size = 122
UI.ArmsRace.ReachedKnife : param size = 50, field size = 148
UI.ArmsRace.ReachedKnife_A : param size = 46, field size = 172
UI.ArmsRace.ReachedKnife_Music : param size = 50, field size = 314
UI.ArmsRace.Weapon_LevelDown : param size = 42, field size = 122
UI.ArmsRace.Weapon_LevelUp : param size = 46, field size = 124
UI.ArmsRace.Weapon_LevelUp_E : param size = 42, field size = 122
UI.BookClose : param size = 42, field size = 122
UI.BookOpen : param size = 42, field size = 122
UI.BookPageBwd : param size = 42, field size = 152
UI.BookPageFwd : param size = 42, field size = 122
UI.ButtonRolloverLarge : param size = 42, field size = 122
UI.CoinLevelUp : param size = 42, field size = 122
UI.CompetitiveAccept : param size = 42, field size = 122
UI.ContractSeal : param size = 42, field size = 122
UI.ContractType : param size = 42, field size = 140
UI.CounterBeep : param size = 58, field size = 122
UI.CounterDoneBeep : param size = 38, field size = 170
UI.CrateDisplay : param size = 42, field size = 122
UI.CrateItemScroll : param size = 42, field size = 122
UI.CrateOpen : param size = 42, field size = 122
UI.DeathMatch.AchievedBonusGrenade : param size = 38, field size = 122
UI.DeathMatch.Dominating : param size = 38, field size = 122
UI.DeathMatch.EndTimer : param size = 46, field size = 124
UI.DeathMatch.EOM_reveal : param size = 54, field size = 98
UI.DeathMatch.ImpendingLevelUp : param size = 58, field size = 122
UI.DeathMatch.Nemesis : param size = 38, field size = 122
UI.DeathMatch.Revenge : param size = 38, field size = 122
UI.DeathMatchBonusAlertEnd : param size = 38, field size = 122
UI.DeathMatchBonusAlertStart : param size = 38, field size = 122
UI.DeathMatchBonusKill : param size = 62, field size = 124
UI.DeathNotice : param size = 38, field size = 122
UI.Guardian.TooFarWarning : param size = 38, field size = 122
UI.ItemInspect : param size = 42, field size = 122
UI.KillCard.1 : param size = 70, field size = 124
UI.Lobby.Chat : param size = 42, field size = 122
UI.Lobby.Joined : param size = 38, field size = 74
UI.Lobby.Kicked : param size = 38, field size = 74
UI.Lobby.Left : param size = 38, field size = 74
UI.PageScroll : param size = 42, field size = 122
UI.PlayerPing : param size = 46, field size = 122
UI.PlayerPingUrgent : param size = 50, field size = 170
UI.Premier.CounterTimer : param size = 58, field size = 122
UI.Premier.EOM.RankDown : param size = 58, field size = 122
UI.Premier.EOM.RankNeutral : param size = 58, field size = 122
UI.Premier.EOM.RankUp : param size = 58, field size = 122
UI.Premier.EOM.SlideIn : param size = 58, field size = 122
UI.Premier.MapDeselect : param size = 58, field size = 122
UI.Premier.MapSelect : param size = 58, field size = 122
UI.Premier.MapsLocked : param size = 58, field size = 122
UI.Premier.SubmenuTransition : param size = 50, field size = 98
UI.Premier.TeamSelect : param size = 58, field size = 122
UI.QueuedMatchStartLoading : param size = 38, field size = 136
UI.RankDown : param size = 38, field size = 122
UI.ShowcaseCoin : param size = 38, field size = 122
UI.ShowcaseKnife : param size = 38, field size = 122
UI.StickerApply : param size = 42, field size = 122
UI.StickerApplyConfirm : param size = 42, field size = 122
UI.StickerDisplay : param size = 42, field size = 122
UI.StickerItemScroll : param size = 42, field size = 122
UI.StickerScratch : param size = 42, field size = 130
UI.StickerSelect : param size = 42, field size = 122
UI.XP.LevelUp : param size = 38, field size = 122
UI.XP.Milestone_01 : param size = 38, field size = 122
UI.XP.Milestone_02 : param size = 38, field size = 122
UI.XP.Milestone_03 : param size = 38, field size = 122
UI.XP.Milestone_04 : param size = 38, field size = 122
UI.XP.Milestone_05 : param size = 38, field size = 122
UI.XP.Remaining : param size = 38, field size = 122
UIPanorama.BG_ar_baggage : param size = 62, field size = 198
UIPanorama.BG_ar_house : param size = 58, field size = 194
UIPanorama.BG_ar_shoots : param size = 58, field size = 194
UIPanorama.BG_coop_kasbah : param size = 58, field size = 194
UIPanorama.BG_cs_italy : param size = 58, field size = 194
UIPanorama.BG_cs_office : param size = 58, field size = 194
UIPanorama.BG_de_ancient : param size = 58, field size = 194
UIPanorama.BG_de_anubis : param size = 58, field size = 194
UIPanorama.BG_de_cache : param size = 58, field size = 194
UIPanorama.BG_de_cbble : param size = 58, field size = 194
UIPanorama.BG_de_chlorine : param size = 58, field size = 194
UIPanorama.BG_de_dust2 : param size = 58, field size = 194
UIPanorama.BG_de_inferno : param size = 58, field size = 194
UIPanorama.BG_de_mirage : param size = 58, field size = 194
UIPanorama.BG_de_mutiny : param size = 58, field size = 194
UIPanorama.BG_de_nuke : param size = 58, field size = 194
UIPanorama.BG_de_overpass : param size = 58, field size = 194
UIPanorama.BG_de_vertigo : param size = 58, field size = 194
UIPanorama.BG_dz_blacksite : param size = 58, field size = 194
UIPanorama.BG_warehouse : param size = 62, field size = 196
UIPanorama.BG_warehouse_space : param size = 58, field size = 194
UIPanorama.buymenu_failure : param size = 50, field size = 98
UIPanorama.buymenu_mouseover : param size = 46, field size = 50
UIPanorama.buymenu_purchase : param size = 50, field size = 98
UIPanorama.buymenu_select : param size = 46, field size = 50
UIPanorama.charDisplay_swapWeapon : param size = 50, field size = 110
UIPanorama.checkbox_toggle : param size = 46, field size = 50
UIPanorama.coin_startRotate : param size = 50, field size = 110
UIPanorama.container_countdown : param size = 50, field size = 98
UIPanorama.container_graffiti_open : param size = 50, field size = 98
UIPanorama.container_graffiti_reveal : param size = 50, field size = 98
UIPanorama.container_graffiti_ticker : param size = 50, field size = 98
UIPanorama.container_music_open : param size = 50, field size = 98
UIPanorama.container_sticker_open : param size = 50, field size = 98
UIPanorama.container_sticker_reveal : param size = 50, field size = 98
UIPanorama.container_sticker_ticker : param size = 50, field size = 98
UIPanorama.container_weapon_fall : param size = 54, field size = 98
UIPanorama.container_weapon_open : param size = 50, field size = 98
UIPanorama.container_weapon_purchaseKey : param size = 50, field size = 98
UIPanorama.container_weapon_ticker : param size = 50, field size = 98
UIPanorama.container_weapon_unlock : param size = 50, field size = 98
UIPanorama.default_startRotate : param size = 50, field size = 110
UIPanorama.EOM.CardReveal : param size = 50, field size = 172
UIPanorama.EOM.CardRevealBling : param size = 50, field size = 98
UIPanorama.equip_musicKit : param size = 50, field size = 98
UIPanorama.exitPopup_confirm : param size = 50, field size = 98
UIPanorama.exitPopup_scroll : param size = 50, field size = 98
UIPanorama.findingGame_cancel_select : param size = 50, field size = 98
UIPanorama.findingGame_scroll : param size = 50, field size = 98
UIPanorama.findingGame_settings_select : param size = 50, field size = 98
UIPanorama.friendElement_press : param size = 50, field size = 98
UIPanorama.gameover_show : param size = 50, field size = 98
UIPanorama.generic_button_press : param size = 50, field size = 98
UIPanorama.generic_button_press_B : param size = 50, field size = 98
UIPanorama.generic_button_rollover : param size = 50, field size = 98
UIPanorama.generic_select : param size = 46, field size = 50
UIPanorama.gift_claim : param size = 64, field size = 98
UIPanorama.gift_deselect : param size = 64, field size = 98
UIPanorama.gift_select : param size = 64, field size = 98
UIPanorama.IntroLogo : param size = 62, field size = 268
UIPanorama.inventory_inspect_character : param size = 50, field size = 98
UIPanorama.inventory_inspect_close : param size = 50, field size = 98
UIPanorama.inventory_inspect_coin : param size = 50, field size = 98
UIPanorama.inventory_inspect_container : param size = 50, field size = 98
UIPanorama.inventory_inspect_gloves : param size = 50, field size = 98
UIPanorama.inventory_inspect_graffiti : param size = 50, field size = 98
UIPanorama.inventory_inspect_knife : param size = 50, field size = 98
UIPanorama.inventory_inspect_musicKit : param size = 50, field size = 98
UIPanorama.inventory_inspect_sellOnMarket : param size = 50, field size = 98
UIPanorama.inventory_inspect_showInfo : param size = 50, field size = 98
UIPanorama.inventory_inspect_sticker : param size = 50, field size = 98
UIPanorama.inventory_inspect_weapon : param size = 50, field size = 98
UIPanorama.inventory_inspect_zoomin : param size = 50, field size = 98
UIPanorama.inventory_inspect_zoomout : param size = 50, field size = 98
UIPanorama.inventory_item_equip : param size = 58, field size = 98
UIPanorama.inventory_item_notequipped : param size = 54, field size = 98
UIPanorama.inventory_item_pickup : param size = 54, field size = 98
UIPanorama.inventory_item_popupSelect : param size = 50, field size = 98
UIPanorama.inventory_item_putdown : param size = 58, field size = 100
UIPanorama.inventory_item_rollover : param size = 54, field size = 98
UIPanorama.inventory_item_select : param size = 50, field size = 98
UIPanorama.inventory_new_item : param size = 50, field size = 98
UIPanorama.inventory_new_item_accept : param size = 50, field size = 98
UIPanorama.inventory_new_item_scroll : param size = 50, field size = 98
UIPanorama.ItemDropAncient : param size = 50, field size = 98
UIPanorama.ItemDropCommon : param size = 50, field size = 98
UIPanorama.ItemDropLegendary : param size = 50, field size = 98
UIPanorama.ItemDropMythical : param size = 50, field size = 98
UIPanorama.ItemDropRare : param size = 50, field size = 98
UIPanorama.ItemDropUncommon : param size = 50, field size = 98
UIPanorama.ItemRevealRarityAncient : param size = 50, field size = 98
UIPanorama.ItemRevealRarityCommon : param size = 50, field size = 98
UIPanorama.ItemRevealRarityLegendary : param size = 50, field size = 98
UIPanorama.ItemRevealRarityMythical : param size = 50, field size = 98
UIPanorama.ItemRevealRarityRare : param size = 50, field size = 98
UIPanorama.ItemRevealRarityUncommon : param size = 50, field size = 98
UIPanorama.ItemRevealSingle : param size = 50, field size = 98
UIPanorama.ItemRevealSingleLocalPlayer : param size = 50, field size = 98
UIPanorama.knife_startRotate : param size = 50, field size = 110
UIPanorama.loadout_sector_scroll : param size = 46, field size = 50
UIPanorama.loadout_sector_select : param size = 46, field size = 50
UIPanorama.LookingToPlay_toggleOff : param size = 50, field size = 98
UIPanorama.LookingToPlay_toggleOn : param size = 42, field size = 122
UIPanorama.mainmenu_press_GO : param size = 50, field size = 98
UIPanorama.mainmenu_press_home : param size = 50, field size = 98
UIPanorama.mainmenu_press_quit : param size = 50, field size = 98
UIPanorama.mainmenu_rollover : param size = 54, field size = 50
UIPanorama.null : param size = 46, field size = 50
UIPanorama.popup_accept_match_beep : param size = 54, field size = 194
UIPanorama.popup_accept_match_confirmed : param size = 50, field size = 98
UIPanorama.popup_accept_match_confirmed_casual : param size = 58, field size = 98
UIPanorama.popup_accept_match_found : param size = 50, field size = 98
UIPanorama.popup_accept_match_person : param size = 50, field size = 98
UIPanorama.popup_accept_match_waitquiet : param size = 50, field size = 98
UIPanorama.popup_newweapon : param size = 50, field size = 98
UIPanorama.rename_purchaseSuccess : param size = 50, field size = 98
UIPanorama.rename_select : param size = 50, field size = 98
UIPanorama.rename_teletype : param size = 50, field size = 116
UIPanorama.resetSettings : param size = 50, field size = 98
UIPanorama.resetSettings_confirm : param size = 50, field size = 98
UIPanorama.round_report_line_down : param size = 50, field size = 98
UIPanorama.round_report_line_up : param size = 50, field size = 98
UIPanorama.round_report_odds_dn : param size = 50, field size = 98
UIPanorama.round_report_odds_none : param size = 50, field size = 98
UIPanorama.round_report_odds_up : param size = 50, field size = 98
UIPanorama.round_report_round_lost : param size = 50, field size = 98
UIPanorama.round_report_round_won : param size = 50, field size = 98
UIPanorama.settings.sliderchange_sfx : param size = 50, field size = 98
UIPanorama.sidemenu_rollover : param size = 58, field size = 98
UIPanorama.sidemenu_select : param size = 46, field size = 50
UIPanorama.sidemenu_slidein : param size = 54, field size = 98
UIPanorama.sidemenu_slideout : param size = 54, field size = 98
UIPanorama.stats_reveal : param size = 50, field size = 98
UIPanorama.sticker_applyConfirm : param size = 50, field size = 98
UIPanorama.sticker_applySticker : param size = 50, field size = 98
UIPanorama.sticker_nextPosition : param size = 50, field size = 98
UIPanorama.store_item_activated : param size = 50, field size = 98
UIPanorama.store_item_purchased : param size = 50, field size = 98
UIPanorama.store_item_rolloff : param size = 46, field size = 50
UIPanorama.store_item_rollover : param size = 46, field size = 50
UIPanorama.submenu_contents_rollover : param size = 46, field size = 50
UIPanorama.submenu_dropdown_option_rollover : param size = 46, field size = 50
UIPanorama.submenu_dropdown_option_select : param size = 50, field size = 98
UIPanorama.submenu_dropdown_select : param size = 50, field size = 98
UIPanorama.submenu_leveloptions_rollover : param size = 46, field size = 50
UIPanorama.submenu_leveloptions_select : param size = 50, field size = 98
UIPanorama.submenu_leveloptions_slidein : param size = 50, field size = 98
UIPanorama.submenu_rollover : param size = 50, field size = 50
UIPanorama.submenu_select : param size = 58, field size = 148
UIPanorama.submenu_slidein : param size = 50, field size = 98
UIPanorama.tab_mainmenu_csgo360 : param size = 50, field size = 98
UIPanorama.tab_mainmenu_inventory : param size = 50, field size = 98
UIPanorama.tab_mainmenu_loadout : param size = 50, field size = 98
UIPanorama.tab_mainmenu_news : param size = 50, field size = 98
UIPanorama.tab_mainmenu_overwatch : param size = 50, field size = 98
UIPanorama.tab_mainmenu_play : param size = 64, field size = 98
UIPanorama.tab_mainmenu_shop : param size = 64, field size = 98
UIPanorama.tab_mainmenu_watch : param size = 50, field size = 98
UIPanorama.tab_settings_settings : param size = 54, field size = 100
UIPanorama.TeamIntro : param size = 54, field size = 146
UIPanorama.TeamIntro_CTSuits : param size = 54, field size = 146
UIPanorama.TeamIntro_TSuits : param size = 54, field size = 146
UIPanorama.ui_custom_lobby_dialog_slide : param size = 50, field size = 98
UIPanorama.weapon_select_char_ctm_sas : param size = 50, field size = 104
UIPanorama.weapon_select_char_tm_leet_variantb : param size = 50, field size = 102
UIPanorama.weapon_select_char_tm_leet_variantc : param size = 50, field size = 102
UIPanorama.weapon_select_char_tm_leet_variantd : param size = 50, field size = 102
UIPanorama.weapon_select_char_tm_phoenix : param size = 50, field size = 102
UIPanorama.weapon_selectDashboard : param size = 50, field size = 98
UIPanorama.weapon_selectReplace : param size = 50, field size = 98
UIPanorama.weapon_showOnChar : param size = 50, field size = 98
UIPanorama.weapon_showSolo : param size = 50, field size = 98
UIPanorama.weapon_startRotate : param size = 50, field size = 110
UIPanorama.XP.BarFull : param size = 46, field size = 50
UIPanorama.XP.Milestone_01 : param size = 50, field size = 98
UIPanorama.XP.Milestone_02 : param size = 50, field size = 98
UIPanorama.XP.Milestone_03 : param size = 50, field size = 98
UIPanorama.XP.Milestone_04 : param size = 50, field size = 98
UIPanorama.XP.NewRank : param size = 50, field size = 98
UIPanorama.XP.NewSkillGroup : param size = 50, field size = 98
UIPanorama.XP.RankDown : param size = 50, field size = 98
UIPanorama.XP.Ticker : param size = 50, field size = 98
UIPanorama.XrayStart : param size = 50, field size = 98
Underwater.BulletImpact : param size = 58, field size = 126
vertigo.BombsiteA : param size = 64, field size = 228
vertigo.BombsiteA.Side : param size = 64, field size = 228
vertigo.BombsiteB : param size = 64, field size = 230
vertigo.CeilingTile : param size = 64, field size = 222
vertigo.Corridor.CT : param size = 64, field size = 224
vertigo.CTHole : param size = 64, field size = 222
vertigo.CTSpawn : param size = 64, field size = 230
vertigo.Edges : param size = 68, field size = 236
vertigo.Edges.Ambulance : param size = 66, field size = 218
vertigo.Edges.CarHonks : param size = 66, field size = 234
vertigo.Edges.Cars : param size = 66, field size = 220
vertigo.Edges.Construction : param size = 42, field size = 98
vertigo.Edges.Elevators : param size = 70, field size = 270
vertigo.Edges.MetalScrapes : param size = 70, field size = 280
vertigo.Edges.Planes : param size = 70, field size = 270
vertigo.Edges.TruckSqueaks : param size = 70, field size = 270
vertigo.Edges.Wind3 : param size = 30, field size = 242
vertigo.Edges.WindGusts : param size = 66, field size = 306
vertigo.Elevator.LowerT : param size = 64, field size = 224
vertigo.Elevator.LowerT.Clangs : param size = 78, field size = 230
vertigo.Elevator.LowerT.Tone : param size = 26, field size = 290
vertigo.Elevator.Shaft : param size = 64, field size = 224
vertigo.Elevator.UpperCT : param size = 64, field size = 222
vertigo.Elevator.UpperT : param size = 64, field size = 222
vertigo.Inside : param size = 60, field size = 228
vertigo.Inside.IndustrialTone : param size = 26, field size = 290
vertigo.Inside.RoomTone : param size = 26, field size = 290
vertigo.Inside.ShaftWind : param size = 38, field size = 242
vertigo.Inside.Wind : param size = 26, field size = 290
vertigo.Insulated : param size = 60, field size = 222
vertigo.Insulated.RoomTone : param size = 30, field size = 292
vertigo.Insulated.Wind : param size = 26, field size = 290
vertigo.LadderRoom : param size = 16, field size = 100
vertigo.LadderRoom.Tone : param size = 18, field size = 194
vertigo.LadderRoom.Wind : param size = 22, field size = 146
vertigo.Light : param size = 26, field size = 314
vertigo.Middle : param size = 64, field size = 222
vertigo.New.BombsiteA.Crane : param size = 66, field size = 226
vertigo.New.BombsiteA.Light : param size = 38, field size = 314
vertigo.New.BombsiteA.Light2 : param size = 38, field size = 314
vertigo.New.BombsiteB.Fan : param size = 38, field size = 314
vertigo.New.BombsiteB.Fan2 : param size = 46, field size = 290
vertigo.New.CorridorCT.VentOutlet : param size = 38, field size = 290
vertigo.New.CTSpawn.Generator : param size = 38, field size = 314
vertigo.New.CTSpawn.Light : param size = 38, field size = 314
vertigo.New.CTSpawn.Light2 : param size = 38, field size = 314
vertigo.New.Elevator.LowerT.Forklift : param size = 38, field size = 266
vertigo.New.Elevator.Shaft.Cables : param size = 54, field size = 246
vertigo.New.Elevator.Shaft.Industrial : param size = 38, field size = 290
vertigo.New.Elevator.Shaft.Wind : param size = 38, field size = 290
vertigo.New.Elevator.UpperCT.VentOutlet : param size = 38, field size = 338
vertigo.New.Elevator.UpperT.Flies : param size = 74, field size = 298
vertigo.New.THalls.Fan : param size = 38, field size = 290
vertigo.New.THalls.FanLoud : param size = 38, field size = 362
vertigo.New.THalls.FanLoud2 : param size = 42, field size = 338
vertigo.New.THalls.Generator : param size = 38, field size = 290
vertigo.New.THalls.Light : param size = 38, field size = 314
vertigo.Outside.Wind : param size = 22, field size = 266
vertigo.Ramp : param size = 64, field size = 220
vertigo.Roof : param size = 60, field size = 224
vertigo.Roof.Amb : param size = 22, field size = 266
vertigo.Roof.Amb_Quiet : param size = 22, field size = 266
vertigo.Roof.Ambulance : param size = 54, field size = 218
vertigo.Roof.CarHonks : param size = 54, field size = 222
vertigo.Roof.Cars : param size = 54, field size = 220
vertigo.Roof.Elevators : param size = 58, field size = 270
vertigo.Roof.Helicopters : param size = 54, field size = 218
vertigo.Roof.Industrial : param size = 26, field size = 266
vertigo.Roof.MetalScrapes : param size = 58, field size = 280
vertigo.Roof.Planes : param size = 58, field size = 270
vertigo.Roof.Train : param size = 58, field size = 266
vertigo.Roof.TruckSqueaks : param size = 58, field size = 270
vertigo.Roof.Vehicles : param size = 54, field size = 226
vertigo.Roof.Wind : param size = 26, field size = 266
vertigo.Roof.Wind2 : param size = 26, field size = 266
vertigo.Roof.Wind3 : param size = 26, field size = 266
vertigo.Roof.WindGusts : param size = 62, field size = 282
vertigo.Scaffolding : param size = 64, field size = 224
vertigo.Stairs : param size = 64, field size = 220
vertigo.StairsBase : param size = 64, field size = 220
vertigo.THalls : param size = 64, field size = 232
vertigo.THalls2 : param size = 64, field size = 230
vertigo.THole : param size = 64, field size = 222
vertigo.Transformer : param size = 26, field size = 314
vertigo.TSpawn : param size = 64, field size = 224
vertigo.TSpawn2 : param size = 68, field size = 276
vertigo.TSpawnGenerator : param size = 76, field size = 220
vertigo.TSpawnInside : param size = 64, field size = 224
vertigo.whistle : param size = 6, field size = 98
Vote.Cast.No : param size = 38, field size = 122
Vote.Cast.Yes : param size = 38, field size = 122
Vote.Created : param size = 38, field size = 122
Vote.Failed : param size = 38, field size = 122
Vote.Passed : param size = 38, field size = 122
Water.BulletImpact : param size = 58, field size = 126
Water.HEGrenade.Splash : param size = 58, field size = 126
Water.PlayerEnter : param size = 70, field size = 202
Water.PlayerExit : param size = 70, field size = 204
water_flood_finished_loop : param size = 34, field size = 122
water_flood_in : param size = 34, field size = 122
Watermelon.BulletImpact : param size = 58, field size = 128
Watermelon.Impact : param size = 54, field size = 200
Weapon.AutoSemiAutoSwitch : param size = 70, field size = 218
Weapon.AutoSemiAutoSwitch_Auto : param size = 66, field size = 220
weapon.bass.impact : param size = 58, field size = 224
weapon.BulletImpact : param size = 58, field size = 126
weapon.C4.Impact : param size = 86, field size = 300
weapon.C4Beep.Impact : param size = 54, field size = 122
weapon.Flashbang.Impact : param size = 74, field size = 248
weapon.Heavy.Impact : param size = 82, field size = 226
weapon.HEGrenade.Impact : param size = 74, field size = 246
weapon.Incendiary.Impact : param size = 74, field size = 248
weapon.Knife.Impact : param size = 78, field size = 250
weapon.Knife.ImpactRattles : param size = 74, field size = 224
weapon.Molotov.Impact : param size = 66, field size = 200
weapon.Pistol.Impact : param size = 70, field size = 202
weapon.Rifle.Impact : param size = 82, field size = 204
weapon.Rifle.Impact.Rattles : param size = 70, field size = 226
weapon.Shotgun.Impact : param size = 82, field size = 204
weapon.Shotgun.Impact.Rattles : param size = 74, field size = 230
weapon.SMG.Impact : param size = 74, field size = 206
weapon.Sniper.Impact : param size = 78, field size = 230
weapon.Sniper.Impact.Metal : param size = 74, field size = 226
Weapon.WeaponMove1 : param size = 78, field size = 170
Weapon.WeaponMove2 : param size = 78, field size = 170
Weapon.WeaponMove3 : param size = 78, field size = 170
Weapon_AK47.BoltPull : param size = 74, field size = 194
Weapon_AK47.BoltPull_Q : param size = 74, field size = 194
Weapon_AK47.Clipin : param size = 74, field size = 194
Weapon_AK47.Clipout : param size = 74, field size = 194
Weapon_AK47.Draw : param size = 70, field size = 170
Weapon_AK47.Single : param size = 98, field size = 272
Weapon_AK47.SingleDistant : param size = 86, field size = 194
Weapon_AK47.WeaponMove1 : param size = 74, field size = 122
Weapon_AK47.WeaponMove2 : param size = 74, field size = 122
Weapon_AK47.WeaponMove3 : param size = 74, field size = 122
Weapon_AUG.Boltpull : param size = 66, field size = 146
Weapon_AUG.Boltpull_Q : param size = 66, field size = 146
Weapon_AUG.Boltrelease : param size = 66, field size = 146
Weapon_AUG.Boltrelease_Q : param size = 66, field size = 146
Weapon_AUG.Cliphit : param size = 66, field size = 146
Weapon_AUG.Clipin : param size = 66, field size = 146
Weapon_AUG.Clipout : param size = 66, field size = 146
Weapon_AUG.Draw : param size = 66, field size = 122
Weapon_AUG.Single : param size = 94, field size = 274
Weapon_AUG.SingleDistant : param size = 86, field size = 170
Weapon_AUG.WeaponMove1 : param size = 74, field size = 122
Weapon_AUG.WeaponMove2 : param size = 74, field size = 122
Weapon_AUG.WeaponMove3 : param size = 74, field size = 122
Weapon_AUG.ZoomIn : param size = 74, field size = 194
Weapon_AUG.ZoomOut : param size = 74, field size = 146
Weapon_AWP.BoltBack : param size = 66, field size = 146
Weapon_AWP.BoltBack_Q : param size = 66, field size = 146
Weapon_AWP.BoltForward : param size = 66, field size = 146
Weapon_AWP.BoltForward_Q : param size = 66, field size = 146
Weapon_AWP.Cliphit : param size = 66, field size = 146
Weapon_AWP.Clipin : param size = 66, field size = 146
Weapon_AWP.Clipout : param size = 66, field size = 146
Weapon_AWP.Draw : param size = 66, field size = 122
Weapon_AWP.Single : param size = 102, field size = 294
Weapon_AWP.SingleDistant : param size = 90, field size = 266
Weapon_AWP.WeaponMove1 : param size = 74, field size = 122
Weapon_AWP.WeaponMove2 : param size = 74, field size = 122
Weapon_AWP.WeaponMove3 : param size = 74, field size = 122
Weapon_AWP.Zoom : param size = 66, field size = 146
Weapon_AWP.ZoomOut : param size = 66, field size = 146
Weapon_bizon.BoltBack : param size = 66, field size = 146
Weapon_bizon.BoltBack_Q : param size = 66, field size = 146
Weapon_bizon.BoltForward : param size = 66, field size = 146
Weapon_bizon.BoltForward_Q : param size = 66, field size = 146
Weapon_bizon.Clipin : param size = 66, field size = 146
Weapon_bizon.Clipout : param size = 66, field size = 146
Weapon_bizon.Draw : param size = 66, field size = 122
Weapon_bizon.Single : param size = 98, field size = 270
Weapon_bizon.SingleDistant : param size = 86, field size = 194
Weapon_bizon.WeaponMove1 : param size = 74, field size = 122
Weapon_bizon.WeaponMove2 : param size = 74, field size = 122
Weapon_bizon.WeaponMove3 : param size = 74, field size = 122
Weapon_CZ.Clipin : param size = 66, field size = 146
Weapon_CZ.Clipin_Q : param size = 66, field size = 146
Weapon_CZ.Clipout : param size = 66, field size = 146
Weapon_CZ.Clipout2 : param size = 66, field size = 146
Weapon_CZ.Draw : param size = 66, field size = 122
Weapon_CZ.Slideback : param size = 66, field size = 146
Weapon_CZ.Slideback_Q : param size = 66, field size = 146
Weapon_CZ.Sliderelease : param size = 66, field size = 146
Weapon_CZ.Sliderelease_Q : param size = 66, field size = 146
Weapon_CZ75A.Single : param size = 98, field size = 272
Weapon_CZ75A.SingleDistant : param size = 90, field size = 194
Weapon_DEagle.Clipin : param size = 66, field size = 146
Weapon_DEagle.Clipout : param size = 66, field size = 146
Weapon_DEagle.Draw : param size = 66, field size = 122
Weapon_DEagle.LookAt009 : param size = 66, field size = 122
Weapon_DEagle.LookAt036 : param size = 66, field size = 122
Weapon_DEagle.LookAt057 : param size = 66, field size = 122
Weapon_DEagle.LookAt081 : param size = 66, field size = 122
Weapon_DEagle.LookAt111 : param size = 66, field size = 122
Weapon_DEagle.LookAt133 : param size = 66, field size = 122
Weapon_DEagle.LookAt166 : param size = 66, field size = 122
Weapon_DEagle.LookAt193 : param size = 66, field size = 122
Weapon_DEagle.LookAt228 : param size = 66, field size = 122
Weapon_DEagle.Single : param size = 98, field size = 270
Weapon_DEagle.SingleDistant : param size = 90, field size = 194
Weapon_DEagle.Slideback : param size = 66, field size = 146
Weapon_DEagle.Slideforward : param size = 66, field size = 146
Weapon_DEagle.WeaponMove1 : param size = 74, field size = 122
Weapon_DEagle.WeaponMove2 : param size = 74, field size = 122
Weapon_DEagle.WeaponMove3 : param size = 74, field size = 122
Weapon_DEagle.WeaponMove4 : param size = 74, field size = 122
Weapon_ELITE.Clipout : param size = 66, field size = 146
Weapon_ELITE.Draw : param size = 66, field size = 122
Weapon_ELITE.Lclipin : param size = 66, field size = 146
Weapon_ELITE.Rclipin : param size = 66, field size = 218
Weapon_ELITE.Reloadstart : param size = 58, field size = 194
Weapon_ELITE.Single : param size = 98, field size = 274
Weapon_ELITE.SingleDistant : param size = 86, field size = 194
Weapon_ELITE.Sliderelease : param size = 66, field size = 146
Weapon_ELITE.Sliderelease_Q : param size = 74, field size = 146
Weapon_ELITE.TauntLook1 : param size = 78, field size = 126
Weapon_ELITE.TauntLook2 : param size = 78, field size = 126
Weapon_ELITE.TauntStartTap : param size = 66, field size = 126
Weapon_ELITE.TauntTap1 : param size = 66, field size = 122
Weapon_ELITE.TauntTap2 : param size = 66, field size = 122
Weapon_ELITE.TauntTwirl : param size = 66, field size = 122
Weapon_FAMAS.BoltBack : param size = 66, field size = 146
Weapon_FAMAS.BoltBack_Q : param size = 66, field size = 146
Weapon_FAMAS.BoltForward : param size = 70, field size = 146
Weapon_FAMAS.BoltForward_Q : param size = 66, field size = 146
Weapon_FAMAS.ClipHit : param size = 66, field size = 194
Weapon_FAMAS.Clipin : param size = 66, field size = 146
Weapon_FAMAS.Clipout : param size = 66, field size = 146
Weapon_FAMAS.Draw : param size = 66, field size = 122
Weapon_FAMAS.Mech : param size = 62, field size = 146
Weapon_FAMAS.Single : param size = 98, field size = 274
Weapon_FAMAS.SingleDistant : param size = 86, field size = 170
Weapon_FAMAS.WeaponMove1 : param size = 74, field size = 122
Weapon_FAMAS.WeaponMove2 : param size = 74, field size = 122
Weapon_FAMAS.WeaponMove3 : param size = 74, field size = 122
Weapon_FiveSeven.Clipin : param size = 66, field size = 146
Weapon_FiveSeven.Clipout : param size = 66, field size = 146
Weapon_FiveSeven.Draw : param size = 66, field size = 122
Weapon_FiveSeven.Single : param size = 98, field size = 268
Weapon_FiveSeven.SingleDistant : param size = 86, field size = 194
Weapon_FiveSeven.Slideback : param size = 66, field size = 146
Weapon_FiveSeven.Slideback_Q : param size = 66, field size = 146
Weapon_FiveSeven.Sliderelease : param size = 66, field size = 146
Weapon_FiveSeven.Sliderelease_Q : param size = 66, field size = 146
Weapon_G3SG1.Clipin : param size = 66, field size = 146
Weapon_G3SG1.Clipout : param size = 66, field size = 146
Weapon_G3SG1.Draw : param size = 66, field size = 122
Weapon_G3SG1.Single : param size = 98, field size = 272
Weapon_G3SG1.SingleDistant : param size = 86, field size = 198
Weapon_G3SG1.SlideBack : param size = 66, field size = 146
Weapon_G3SG1.SlideBack_Q : param size = 66, field size = 146
Weapon_G3SG1.SlideForward : param size = 66, field size = 146
Weapon_G3SG1.SlideForward_Q : param size = 66, field size = 146
Weapon_G3SG1.WeaponMove1 : param size = 74, field size = 122
Weapon_G3SG1.WeaponMove2 : param size = 74, field size = 122
Weapon_G3SG1.WeaponMove3 : param size = 74, field size = 122
Weapon_G3SG1.Zoom : param size = 66, field size = 146
Weapon_G3SG1.ZoomOut : param size = 66, field size = 146
Weapon_GalilAR.BoltBack : param size = 66, field size = 146
Weapon_GalilAR.BoltBack_Q : param size = 66, field size = 146
Weapon_GalilAR.BoltForward : param size = 70, field size = 146
Weapon_GalilAR.BoltForward_Q : param size = 66, field size = 146
Weapon_GalilAR.Clipin : param size = 66, field size = 146
Weapon_GalilAR.Clipout : param size = 66, field size = 146
Weapon_GalilAR.Draw : param size = 66, field size = 122
Weapon_GalilAR.Single : param size = 98, field size = 268
Weapon_GalilAR.SingleDistant : param size = 86, field size = 194
Weapon_GalilAR.WeaponMove1 : param size = 74, field size = 122
Weapon_GalilAR.WeaponMove2 : param size = 74, field size = 122
Weapon_GalilAR.WeaponMove3 : param size = 74, field size = 122
Weapon_Glock.Clipin : param size = 66, field size = 146
Weapon_Glock.Clipout : param size = 66, field size = 146
Weapon_Glock.Draw : param size = 66, field size = 122
Weapon_Glock.Element_Click : param size = 74, field size = 170
Weapon_Glock.Element_CRB : param size = 78, field size = 172
Weapon_Glock.Single : param size = 98, field size = 272
Weapon_Glock.SingleDistant : param size = 86, field size = 194
Weapon_Glock.Slideback : param size = 66, field size = 146
Weapon_Glock.Slideback_Q : param size = 66, field size = 146
Weapon_Glock.Sliderelease : param size = 66, field size = 146
Weapon_Glock.Sliderelease_Q : param size = 66, field size = 146
Weapon_hkp2000.Clipin : param size = 66, field size = 146
Weapon_hkp2000.Clipout : param size = 66, field size = 146
Weapon_hkp2000.Draw : param size = 66, field size = 122
Weapon_hkp2000.Single : param size = 102, field size = 272
Weapon_hkp2000.SingleDistant : param size = 86, field size = 194
Weapon_hkp2000.Slideback : param size = 66, field size = 146
Weapon_hkp2000.Slideback_Q : param size = 66, field size = 146
Weapon_hkp2000.Sliderelease : param size = 66, field size = 146
Weapon_hkp2000.Sliderelease_Q : param size = 66, field size = 146
Weapon_Knife.Deploy : param size = 66, field size = 122
Weapon_Knife.Hit : param size = 66, field size = 152
Weapon_Knife.HitWall : param size = 66, field size = 154
Weapon_Knife.Slash : param size = 74, field size = 220
Weapon_Knife.Stab : param size = 54, field size = 218
Weapon_M249.Boxin : param size = 66, field size = 122
Weapon_M249.Boxout : param size = 66, field size = 122
Weapon_M249.Chain : param size = 66, field size = 122
Weapon_M249.Coverdown : param size = 66, field size = 122
Weapon_M249.Coverup : param size = 66, field size = 122
Weapon_M249.Draw : param size = 66, field size = 122
Weapon_M249.Jangle : param size = 26, field size = 104
Weapon_M249.Pump : param size = 66, field size = 122
Weapon_M249.Pump_Q : param size = 66, field size = 122
Weapon_M249.Single : param size = 98, field size = 250
Weapon_M249.SingleDistant : param size = 86, field size = 194
Weapon_M4A1.BoltBack : param size = 66, field size = 146
Weapon_M4A1.BoltBack_Q : param size = 66, field size = 146
Weapon_M4A1.BoltForward : param size = 66, field size = 146
Weapon_M4A1.BoltForward_Q : param size = 66, field size = 146
Weapon_M4A1.ClipHit : param size = 66, field size = 146
Weapon_M4A1.Clipin : param size = 66, field size = 146
Weapon_M4A1.Clipout : param size = 66, field size = 146
Weapon_M4A1.Draw : param size = 66, field size = 122
Weapon_M4A1.Silenced : param size = 74, field size = 242
Weapon_M4A1.Silencer_Off : param size = 66, field size = 122
Weapon_M4A1.Silencer_On : param size = 66, field size = 122
Weapon_M4A1.SilencerScrew1 : param size = 66, field size = 122
Weapon_M4A1.SilencerScrew2 : param size = 66, field size = 122
Weapon_M4A1.SilencerScrew3 : param size = 66, field size = 122
Weapon_M4A1.SilencerScrew4 : param size = 66, field size = 122
Weapon_M4A1.SilencerScrew5 : param size = 66, field size = 122
Weapon_M4A1.SilencerScrewOffEnd : param size = 66, field size = 122
Weapon_M4A1.SilencerScrewOnStart : param size = 66, field size = 122
Weapon_M4A1.SilencerWeaponMove1 : param size = 66, field size = 122
Weapon_M4A1.SilencerWeaponMove2 : param size = 66, field size = 122
Weapon_M4A1.SilencerWeaponMove3 : param size = 66, field size = 122
Weapon_M4A1.Single : param size = 82, field size = 226
Weapon_M4A1.SingleDistant : param size = 78, field size = 150
Weapon_M4A1.WeaponMove1 : param size = 74, field size = 122
Weapon_M4A1.WeaponMove2 : param size = 74, field size = 122
Weapon_M4A1.WeaponMove3 : param size = 74, field size = 122
Weapon_M4A1S.BoltBack : param size = 66, field size = 146
Weapon_M4A1S.BoltBack_Q : param size = 66, field size = 146
Weapon_M4A1S.BoltForward : param size = 66, field size = 146
Weapon_M4A1S.BoltForward_Q : param size = 66, field size = 146
Weapon_M4A4.Single : param size = 98, field size = 270
Weapon_M4A4.SingleDistant : param size = 86, field size = 194
Weapon_MAC10.BoltBack : param size = 66, field size = 146
Weapon_MAC10.BoltBack_Q : param size = 66, field size = 146
Weapon_MAC10.BoltForward : param size = 66, field size = 146
Weapon_MAC10.BoltForward_Q : param size = 66, field size = 146
Weapon_MAC10.Clipin : param size = 66, field size = 146
Weapon_MAC10.Clipout : param size = 66, field size = 146
Weapon_MAC10.Draw : param size = 66, field size = 122
Weapon_MAC10.Single : param size = 98, field size = 270
Weapon_MAC10.SingleDistant : param size = 86, field size = 194
Weapon_MAC10.WeaponMove1 : param size = 74, field size = 122
Weapon_MAC10.WeaponMove2 : param size = 74, field size = 122
Weapon_MAC10.WeaponMove3 : param size = 74, field size = 122
Weapon_Mag7.Clipin : param size = 66, field size = 146
Weapon_Mag7.Clipout : param size = 66, field size = 146
Weapon_Mag7.Draw : param size = 66, field size = 122
Weapon_Mag7.Insertshell : param size = 66, field size = 122
Weapon_Mag7.PumpBack : param size = 70, field size = 122
Weapon_Mag7.PumpBack_Q : param size = 66, field size = 122
Weapon_Mag7.PumpForward : param size = 70, field size = 122
Weapon_Mag7.PumpForward_Q : param size = 66, field size = 122
Weapon_Mag7.Single : param size = 90, field size = 270
Weapon_Mag7.SingleDistant : param size = 86, field size = 196
Weapon_MP5.Clipin : param size = 66, field size = 146
Weapon_MP5.Clipout : param size = 66, field size = 146
Weapon_MP5.Draw : param size = 66, field size = 122
Weapon_MP5.Grab : param size = 66, field size = 146
Weapon_MP5.Single : param size = 98, field size = 292
Weapon_MP5.Slideback : param size = 66, field size = 146
Weapon_MP5.SlideForward : param size = 66, field size = 146
Weapon_MP5.SlideForward_Q : param size = 66, field size = 146
Weapon_MP5.WeaponMove1 : param size = 74, field size = 122
Weapon_MP5.WeaponMove2 : param size = 74, field size = 122
Weapon_MP5.WeaponMove3 : param size = 74, field size = 122
Weapon_MP7.Clipin : param size = 66, field size = 146
Weapon_MP7.Clipout : param size = 66, field size = 146
Weapon_MP7.Draw : param size = 66, field size = 122
Weapon_MP7.Single : param size = 98, field size = 272
Weapon_MP7.SingleDistant : param size = 86, field size = 194
Weapon_MP7.Slideback : param size = 66, field size = 146
Weapon_MP7.Slideback_Q : param size = 66, field size = 146
Weapon_MP7.SlideForward : param size = 66, field size = 146
Weapon_MP7.SlideForward_Q : param size = 66, field size = 146
Weapon_MP7.WeaponMove1 : param size = 74, field size = 122
Weapon_MP7.WeaponMove2 : param size = 74, field size = 122
Weapon_MP7.WeaponMove3 : param size = 74, field size = 122
Weapon_MP9.BoltBack : param size = 66, field size = 146
Weapon_MP9.BoltBack_Q : param size = 66, field size = 146
Weapon_MP9.BoltForward : param size = 66, field size = 146
Weapon_MP9.BoltForward_Q : param size = 66, field size = 146
Weapon_MP9.Clipin : param size = 66, field size = 146
Weapon_MP9.Clipout : param size = 66, field size = 146
Weapon_MP9.Draw : param size = 66, field size = 122
Weapon_MP9.Single : param size = 98, field size = 274
Weapon_MP9.SingleDistant : param size = 86, field size = 194
Weapon_MP9.WeaponMove1 : param size = 74, field size = 122
Weapon_MP9.WeaponMove2 : param size = 74, field size = 122
Weapon_MP9.WeaponMove3 : param size = 74, field size = 122
Weapon_Negev.Boxin : param size = 66, field size = 122
Weapon_Negev.Boxout : param size = 66, field size = 122
Weapon_Negev.Chain : param size = 66, field size = 122
Weapon_Negev.Coverdown : param size = 66, field size = 122
Weapon_Negev.Coverup : param size = 66, field size = 122
Weapon_Negev.Draw : param size = 66, field size = 122
Weapon_Negev.Pump : param size = 66, field size = 122
Weapon_Negev.Pump_Q : param size = 66, field size = 122
Weapon_Negev.Single : param size = 98, field size = 270
Weapon_Negev.SingleDistant : param size = 86, field size = 194
Weapon_Negev.SingleFocused : param size = 98, field size = 270
Weapon_Nova.Draw : param size = 66, field size = 122
Weapon_Nova.Insertshell : param size = 66, field size = 126
Weapon_Nova.Pump : param size = 66, field size = 122
Weapon_Nova.Pump_Q : param size = 66, field size = 122
Weapon_Nova.Single : param size = 90, field size = 268
Weapon_Nova.SingleDistant : param size = 90, field size = 194
Weapon_Nova.WeaponMove1 : param size = 74, field size = 122
Weapon_Nova.WeaponMove2 : param size = 74, field size = 122
Weapon_Nova.WeaponMove3 : param size = 74, field size = 122
Weapon_P250.Clipin : param size = 66, field size = 146
Weapon_P250.Clipout : param size = 66, field size = 146
Weapon_P250.Draw : param size = 66, field size = 122
Weapon_P250.Single : param size = 98, field size = 268
Weapon_P250.SingleDistant : param size = 90, field size = 194
Weapon_P250.Slideback : param size = 66, field size = 146
Weapon_P250.Slideback_Q : param size = 66, field size = 146
Weapon_P250.Sliderelease : param size = 66, field size = 146
Weapon_P250.Sliderelease_Q : param size = 66, field size = 146
Weapon_P90.BoltBack : param size = 66, field size = 146
Weapon_P90.BoltBack_Q : param size = 66, field size = 146
Weapon_P90.BoltForward : param size = 66, field size = 146
Weapon_P90.BoltForward_Q : param size = 66, field size = 146
Weapon_P90.ClipHit : param size = 66, field size = 146
Weapon_P90.Clipin : param size = 66, field size = 146
Weapon_P90.Clipout : param size = 66, field size = 146
Weapon_P90.Cliprelease : param size = 66, field size = 146
Weapon_P90.Draw : param size = 66, field size = 122
Weapon_P90.Single : param size = 98, field size = 270
Weapon_P90.SingleDistant : param size = 86, field size = 218
Weapon_P90.WeaponMove1 : param size = 74, field size = 122
Weapon_P90.WeaponMove2 : param size = 74, field size = 122
Weapon_P90.WeaponMove3 : param size = 74, field size = 122
Weapon_PartyHorn.Single : param size = 86, field size = 266
Weapon_PartyHorn.Taser : param size = 54, field size = 122
Weapon_Revolver.BarrelRoll : param size = 58, field size = 122
Weapon_Revolver.BarrelRoll_Q : param size = 58, field size = 122
Weapon_Revolver.Clipin : param size = 66, field size = 146
Weapon_Revolver.Clipin_Q : param size = 66, field size = 146
Weapon_Revolver.Clipout : param size = 66, field size = 146
Weapon_Revolver.Draw : param size = 66, field size = 122
Weapon_Revolver.Hammer : param size = 58, field size = 122
Weapon_Revolver.Prepare : param size = 58, field size = 146
Weapon_Revolver.Sideback : param size = 66, field size = 122
Weapon_Revolver.Sideback_Q : param size = 66, field size = 122
Weapon_Revolver.Siderelease : param size = 66, field size = 122
Weapon_Revolver.Siderelease_Q : param size = 66, field size = 122
Weapon_Revolver.Single : param size = 90, field size = 268
Weapon_Revolver.SingleDistant : param size = 86, field size = 194
Weapon_Sawedoff.Draw : param size = 66, field size = 122
Weapon_Sawedoff.Insertshell : param size = 66, field size = 128
Weapon_Sawedoff.Pump : param size = 66, field size = 122
Weapon_Sawedoff.Pump_Q : param size = 66, field size = 122
Weapon_Sawedoff.Single : param size = 98, field size = 268
Weapon_Sawedoff.SingleDistant : param size = 86, field size = 194
Weapon_Sawedoff.WeaponMove1 : param size = 74, field size = 122
Weapon_Sawedoff.WeaponMove2 : param size = 74, field size = 122
Weapon_Sawedoff.WeaponMove3 : param size = 74, field size = 122
Weapon_SCAR20.BoltBack : param size = 66, field size = 146
Weapon_SCAR20.BoltBack_Q : param size = 66, field size = 146
Weapon_SCAR20.BoltForward : param size = 66, field size = 146
Weapon_SCAR20.BoltForward_Q : param size = 66, field size = 146
Weapon_SCAR20.Clipin : param size = 66, field size = 146
Weapon_SCAR20.Clipout : param size = 66, field size = 146
Weapon_SCAR20.ClipTouch : param size = 66, field size = 146
Weapon_SCAR20.Draw : param size = 66, field size = 122
Weapon_SCAR20.Single : param size = 98, field size = 272
Weapon_SCAR20.SingleDistant : param size = 86, field size = 194
Weapon_SCAR20.WeaponMove1 : param size = 74, field size = 122
Weapon_SCAR20.WeaponMove2 : param size = 74, field size = 122
Weapon_SCAR20.WeaponMove3 : param size = 74, field size = 122
Weapon_SCAR20.Zoom : param size = 66, field size = 146
Weapon_SCAR20.ZoomOut : param size = 66, field size = 146
Weapon_sg556.BoltBack : param size = 66, field size = 146
Weapon_sg556.BoltBack_Q : param size = 66, field size = 146
Weapon_sg556.BoltForward : param size = 66, field size = 146
Weapon_sg556.BoltForward_Q : param size = 66, field size = 146
Weapon_sg556.ClipHit : param size = 66, field size = 146
Weapon_sg556.Clipin : param size = 66, field size = 146
Weapon_sg556.Clipout : param size = 66, field size = 146
Weapon_SG556.Draw : param size = 66, field size = 122
Weapon_sg556.Single : param size = 98, field size = 274
Weapon_sg556.SingleDistant : param size = 86, field size = 194
Weapon_sg556.WeaponMove1 : param size = 74, field size = 122
Weapon_sg556.WeaponMove2 : param size = 74, field size = 122
Weapon_sg556.WeaponMove3 : param size = 74, field size = 122
Weapon_sg556.ZoomIn : param size = 74, field size = 146
Weapon_sg556.ZoomOut : param size = 78, field size = 146
Weapon_SSG08.BoltBack : param size = 74, field size = 146
Weapon_SSG08.BoltBack_Q : param size = 74, field size = 146
Weapon_SSG08.BoltForward : param size = 74, field size = 146
Weapon_SSG08.BoltForward_Q : param size = 74, field size = 146
Weapon_SSG08.ClipHit : param size = 66, field size = 146
Weapon_SSG08.Clipin : param size = 66, field size = 146
Weapon_SSG08.Clipout : param size = 66, field size = 146
Weapon_SSG08.Draw : param size = 70, field size = 170
Weapon_SSG08.Single : param size = 98, field size = 268
Weapon_SSG08.SingleDistant : param size = 86, field size = 194
Weapon_SSG08.WeaponMove1 : param size = 74, field size = 122
Weapon_SSG08.WeaponMove2 : param size = 74, field size = 122
Weapon_SSG08.WeaponMove3 : param size = 74, field size = 122
Weapon_SSG08.Zoom : param size = 66, field size = 146
Weapon_SSG08.ZoomOut : param size = 66, field size = 146
Weapon_Taser.ChargeNotReady : param size = 78, field size = 152
Weapon_Taser.ChargeNotReady_Empty : param size = 74, field size = 146
Weapon_Taser.ChargeReady : param size = 74, field size = 148
Weapon_Taser.ChargeReady_Zap : param size = 70, field size = 150
Weapon_Taser.Charging : param size = 98, field size = 218
Weapon_Taser.Draw : param size = 66, field size = 122
Weapon_Taser.Hit : param size = 66, field size = 146
Weapon_Taser.Single : param size = 90, field size = 196
Weapon_tec9.Boltpull : param size = 66, field size = 146
Weapon_tec9.Boltpull_Q : param size = 66, field size = 146
Weapon_tec9.Boltrelease : param size = 66, field size = 146
Weapon_tec9.Boltrelease_Q : param size = 66, field size = 146
Weapon_tec9.Clipin : param size = 66, field size = 146
Weapon_tec9.Clipout : param size = 66, field size = 146
Weapon_tec9.Draw : param size = 66, field size = 122
Weapon_tec9.Single : param size = 98, field size = 268
Weapon_tec9.SingleDistant : param size = 86, field size = 194
Weapon_UMP45.BoltBack : param size = 66, field size = 146
Weapon_UMP45.BoltBack_Q : param size = 66, field size = 146
Weapon_UMP45.BoltForward : param size = 66, field size = 146
Weapon_UMP45.BoltForward_Q : param size = 66, field size = 146
Weapon_UMP45.Clipin : param size = 66, field size = 146
Weapon_UMP45.Clipout : param size = 66, field size = 146
Weapon_UMP45.Draw : param size = 66, field size = 122
Weapon_UMP45.Single : param size = 98, field size = 268
Weapon_UMP45.SingleDistant : param size = 86, field size = 194
Weapon_UMP45.WeaponMove1 : param size = 74, field size = 122
Weapon_UMP45.WeaponMove2 : param size = 74, field size = 122
Weapon_UMP45.WeaponMove3 : param size = 74, field size = 122
Weapon_USP.AttachSilencer : param size = 66, field size = 122
Weapon_USP.Clipin : param size = 66, field size = 146
Weapon_USP.Clipout : param size = 66, field size = 146
Weapon_USP.DetachSilencer : param size = 66, field size = 122
Weapon_USP.Draw : param size = 66, field size = 122
Weapon_USP.SilencedShot : param size = 74, field size = 294
Weapon_USP.SilencerScrew1 : param size = 66, field size = 122
Weapon_USP.SilencerScrew2 : param size = 66, field size = 122
Weapon_USP.SilencerScrew3 : param size = 66, field size = 122
Weapon_USP.SilencerScrew4 : param size = 66, field size = 122
Weapon_USP.SilencerScrew5 : param size = 66, field size = 122
Weapon_USP.SilencerScrewOffEnd : param size = 66, field size = 122
Weapon_USP.SilencerScrewOnStart : param size = 66, field size = 122
Weapon_USP.SilencerWeaponMove1 : param size = 66, field size = 122
Weapon_USP.SilencerWeaponMove2 : param size = 66, field size = 122
Weapon_USP.SilencerWeaponMove3 : param size = 66, field size = 122
Weapon_USP.Single : param size = 86, field size = 272
Weapon_USP.Slideback : param size = 66, field size = 146
Weapon_USP.Slideback2 : param size = 66, field size = 146
Weapon_USP.Slideback_Q : param size = 66, field size = 146
Weapon_USP.Sliderelease : param size = 66, field size = 146
Weapon_USP.Sliderelease_Q : param size = 66, field size = 146
Weapon_XM1014.Draw : param size = 66, field size = 122
Weapon_XM1014.InsertShell : param size = 66, field size = 126
Weapon_XM1014.Single : param size = 98, field size = 268
Weapon_XM1014.SingleDistant : param size = 86, field size = 194
Weapon_XM1014.WeaponMove1 : param size = 74, field size = 122
Weapon_XM1014.WeaponMove2 : param size = 74, field size = 122
Weapon_XM1014.WeaponMove3 : param size = 74, field size = 122
WeaponMagazine.BulletImpact : param size = 58, field size = 126
WeaponMagazine.ImpactHard : param size = 58, field size = 198
WeaponMagazine.ImpactSoft : param size = 58, field size = 198
Weather.thunder_1 : param size = 22, field size = 2
Weather.thunder_2 : param size = 18, field size = 2
Weather.thunder_3 : param size = 18, field size = 2
Weather.thunder_close_1 : param size = 18, field size = 50
Weather.thunder_close_2 : param size = 18, field size = 50
Weather.thunder_close_3 : param size = 18, field size = 50
Weather.thunder_close_4 : param size = 18, field size = 50
Weather.thunder_close_all_4 : param size = 18, field size = 54
Weather.thunder_far_away_1 : param size = 18, field size = 2
Weather.thunder_far_away_2 : param size = 18, field size = 2
Weather.thunder_faraway_all_1 : param size = 18, field size = 52
WetTile.Splash : param size = 98, field size = 134
windchimes.snd01 : param size = 18, field size = 50
windchimes.snd02 : param size = 18, field size = 50
windchimes.snd03 : param size = 18, field size = 50
windchimes.snd04 : param size = 18, field size = 50
Wood.Break : param size = 50, field size = 132
Wood.BulletImpact : param size = 58, field size = 130
Wood.ImpactHard : param size = 54, field size = 198
Wood.ImpactSoft : param size = 54, field size = 174
Wood.ImpactSoftGloves : param size = 54, field size = 200
Wood_Box.Break : param size = 50, field size = 122
Wood_Box.BulletImpact : param size = 58, field size = 128
Wood_Box.ImpactHard : param size = 54, field size = 198
Wood_Box.ImpactSoft : param size = 54, field size = 174
Wood_Crate.Break : param size = 50, field size = 126
Wood_Crate.ImpactHard : param size = 54, field size = 198
Wood_Crate.ImpactSoft : param size = 54, field size = 174
Wood_Furniture.Break : param size = 50, field size = 124
Wood_Furniture.ImpactSoft : param size = 54, field size = 174
Wood_Panel.Break : param size = 50, field size = 122
Wood_Panel.BulletImpact : param size = 58, field size = 128
Wood_Panel.ImpactHard : param size = 54, field size = 198
Wood_Panel.ImpactSoft : param size = 54, field size = 174
*/