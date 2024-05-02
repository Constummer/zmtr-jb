using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static BattlePassPremiumBase? GetBattlePassPremiumLevelConfigFromString(int level, string config)
    {
        return level switch
        {
            1 => JsonConvert.DeserializeObject<BattlePassPremium_Level01>(config),
            2 => JsonConvert.DeserializeObject<BattlePassPremium_Level02>(config),
            3 => JsonConvert.DeserializeObject<BattlePassPremium_Level03>(config),
            4 => JsonConvert.DeserializeObject<BattlePassPremium_Level04>(config),
            5 => JsonConvert.DeserializeObject<BattlePassPremium_Level05>(config),
            6 => JsonConvert.DeserializeObject<BattlePassPremium_Level06>(config),
            7 => JsonConvert.DeserializeObject<BattlePassPremium_Level07>(config),
            8 => JsonConvert.DeserializeObject<BattlePassPremium_Level08>(config),
            9 => JsonConvert.DeserializeObject<BattlePassPremium_Level09>(config),
            10 => JsonConvert.DeserializeObject<BattlePassPremium_Level10>(config),
            11 => JsonConvert.DeserializeObject<BattlePassPremium_Level11>(config),
            12 => JsonConvert.DeserializeObject<BattlePassPremium_Level12>(config),
            13 => JsonConvert.DeserializeObject<BattlePassPremium_Level13>(config),
            14 => JsonConvert.DeserializeObject<BattlePassPremium_Level14>(config),
            15 => JsonConvert.DeserializeObject<BattlePassPremium_Level15>(config),
            16 => JsonConvert.DeserializeObject<BattlePassPremium_Level16>(config),
            17 => JsonConvert.DeserializeObject<BattlePassPremium_Level17>(config),
            18 => JsonConvert.DeserializeObject<BattlePassPremium_Level18>(config),
            19 => JsonConvert.DeserializeObject<BattlePassPremium_Level19>(config),
            20 => JsonConvert.DeserializeObject<BattlePassPremium_Level20>(config),
            21 => JsonConvert.DeserializeObject<BattlePassPremium_Level21>(config),
            _ => null
        };
    }
}