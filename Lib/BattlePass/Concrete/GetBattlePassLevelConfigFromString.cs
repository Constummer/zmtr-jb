using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static BattlePassBase? GetBattlePassLevelConfigFromString(int level, string config)
    {
        return level switch
        {
            1 => JsonConvert.DeserializeObject<BattlePass_Level01>(config),
            2 => JsonConvert.DeserializeObject<BattlePass_Level02>(config),
            3 => JsonConvert.DeserializeObject<BattlePass_Level03>(config),
            4 => JsonConvert.DeserializeObject<BattlePass_Level04>(config),
            5 => JsonConvert.DeserializeObject<BattlePass_Level05>(config),
            6 => JsonConvert.DeserializeObject<BattlePass_Level06>(config),
            7 => JsonConvert.DeserializeObject<BattlePass_Level07>(config),
            8 => JsonConvert.DeserializeObject<BattlePass_Level08>(config),
            9 => JsonConvert.DeserializeObject<BattlePass_Level09>(config),
            10 => JsonConvert.DeserializeObject<BattlePass_Level10>(config),
            11 => JsonConvert.DeserializeObject<BattlePass_Level11>(config),
            12 => JsonConvert.DeserializeObject<BattlePass_Level12>(config),
            13 => JsonConvert.DeserializeObject<BattlePass_Level13>(config),
            14 => JsonConvert.DeserializeObject<BattlePass_Level14>(config),
            15 => JsonConvert.DeserializeObject<BattlePass_Level15>(config),
            16 => JsonConvert.DeserializeObject<BattlePass_Level16>(config),
            17 => JsonConvert.DeserializeObject<BattlePass_Level17>(config),
            18 => JsonConvert.DeserializeObject<BattlePass_Level18>(config),
            19 => JsonConvert.DeserializeObject<BattlePass_Level19>(config),
            20 => JsonConvert.DeserializeObject<BattlePass_Level20>(config),
            21 => JsonConvert.DeserializeObject<BattlePass_Level21>(config),
            22 => JsonConvert.DeserializeObject<BattlePass_Level22>(config),
            23 => JsonConvert.DeserializeObject<BattlePass_Level23>(config),
            24 => JsonConvert.DeserializeObject<BattlePass_Level24>(config),
            25 => JsonConvert.DeserializeObject<BattlePass_Level25>(config),
            26 => JsonConvert.DeserializeObject<BattlePass_Level26>(config),
            27 => JsonConvert.DeserializeObject<BattlePass_Level27>(config),
            28 => JsonConvert.DeserializeObject<BattlePass_Level28>(config),
            29 => JsonConvert.DeserializeObject<BattlePass_Level29>(config),
            30 => JsonConvert.DeserializeObject<BattlePass_Level30>(config),
            31 => JsonConvert.DeserializeObject<BattlePass_Level31>(config),
            _ => null
        };
    }
}