namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static TimeRewardBase? GetTimeRewardLevelConfig(int level)
    {
        return level switch
        {
            1 => new TimeReward_Level01(),
            2 => new TimeReward_Level02(),
            3 => new TimeReward_Level03(),
            4 => new TimeReward_Level04(),
            5 => new TimeReward_Level05(),
            6 => new TimeReward_Level06(),
            7 => new TimeReward_Level07(),
            8 => new TimeReward_Level08(),
            9 => new TimeReward_Level09(),
            10 => new TimeReward_Level10(),
            11 => new TimeReward_Level11(),
            12 => new TimeReward_Level12(),
            13 => new TimeReward_Level13(),
            14 => new TimeReward_Level14(),
            15 => new TimeReward_Level15(),
            16 => new TimeReward_Level16(),
            17 => new TimeReward_Level17(),
            18 => new TimeReward_Level18(),
            19 => new TimeReward_Level19(),
            20 => new TimeReward_Level20(),
            21 => new TimeReward_Level21(),
            22 => new TimeReward_Level22(),
            23 => new TimeReward_Level23(),
            24 => new TimeReward_Level24(),
            25 => new TimeReward_Level25(),
            26 => new TimeReward_Level26(),
            27 => new TimeReward_Level27(),
            28 => new TimeReward_Level28(),
            29 => new TimeReward_Level29(),
            30 => new TimeReward_Level30(),
            31 => new TimeReward_Level31(),
            _ => null
        };
    }
}