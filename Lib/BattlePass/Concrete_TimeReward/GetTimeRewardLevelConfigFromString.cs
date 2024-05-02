using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static TimeRewardBase? GetTimeRewardLevelConfigFromString(int level, string config)
    {
        return level switch
        {
            1 => JsonConvert.DeserializeObject<TimeReward_Level01>(config),
            2 => JsonConvert.DeserializeObject<TimeReward_Level02>(config),
            3 => JsonConvert.DeserializeObject<TimeReward_Level03>(config),
            4 => JsonConvert.DeserializeObject<TimeReward_Level04>(config),
            5 => JsonConvert.DeserializeObject<TimeReward_Level05>(config),
            6 => JsonConvert.DeserializeObject<TimeReward_Level06>(config),
            7 => JsonConvert.DeserializeObject<TimeReward_Level07>(config),
            8 => JsonConvert.DeserializeObject<TimeReward_Level08>(config),
            9 => JsonConvert.DeserializeObject<TimeReward_Level09>(config),
            10 => JsonConvert.DeserializeObject<TimeReward_Level10>(config),
            11 => JsonConvert.DeserializeObject<TimeReward_Level11>(config),
            12 => JsonConvert.DeserializeObject<TimeReward_Level12>(config),
            13 => JsonConvert.DeserializeObject<TimeReward_Level13>(config),
            14 => JsonConvert.DeserializeObject<TimeReward_Level14>(config),
            15 => JsonConvert.DeserializeObject<TimeReward_Level15>(config),
            16 => JsonConvert.DeserializeObject<TimeReward_Level16>(config),
            17 => JsonConvert.DeserializeObject<TimeReward_Level17>(config),
            18 => JsonConvert.DeserializeObject<TimeReward_Level18>(config),
            19 => JsonConvert.DeserializeObject<TimeReward_Level19>(config),
            20 => JsonConvert.DeserializeObject<TimeReward_Level20>(config),
            21 => JsonConvert.DeserializeObject<TimeReward_Level21>(config),
            22 => JsonConvert.DeserializeObject<TimeReward_Level22>(config),
            23 => JsonConvert.DeserializeObject<TimeReward_Level23>(config),
            24 => JsonConvert.DeserializeObject<TimeReward_Level24>(config),
            25 => JsonConvert.DeserializeObject<TimeReward_Level25>(config),
            26 => JsonConvert.DeserializeObject<TimeReward_Level26>(config),
            27 => JsonConvert.DeserializeObject<TimeReward_Level27>(config),
            28 => JsonConvert.DeserializeObject<TimeReward_Level28>(config),
            29 => JsonConvert.DeserializeObject<TimeReward_Level29>(config),
            30 => JsonConvert.DeserializeObject<TimeReward_Level30>(config),
            31 => JsonConvert.DeserializeObject<TimeReward_Level31>(config),
            _ => null
        };
    }
}