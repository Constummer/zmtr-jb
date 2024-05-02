namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static BattlePassBase? GetBattlePassLevelConfig(int level)
    {
        return level switch
        {
            1 => new BattlePass_Level01(),
            2 => new BattlePass_Level02(),
            3 => new BattlePass_Level03(),
            4 => new BattlePass_Level04(),
            5 => new BattlePass_Level05(),
            6 => new BattlePass_Level06(),
            7 => new BattlePass_Level07(),
            8 => new BattlePass_Level08(),
            9 => new BattlePass_Level09(),
            10 => new BattlePass_Level10(),
            11 => new BattlePass_Level11(),
            12 => new BattlePass_Level12(),
            13 => new BattlePass_Level13(),
            14 => new BattlePass_Level14(),
            15 => new BattlePass_Level15(),
            16 => new BattlePass_Level16(),
            17 => new BattlePass_Level17(),
            18 => new BattlePass_Level18(),
            19 => new BattlePass_Level19(),
            20 => new BattlePass_Level20(),
            21 => new BattlePass_Level21(),
            22 => new BattlePass_Level22(),
            23 => new BattlePass_Level23(),
            24 => new BattlePass_Level24(),
            25 => new BattlePass_Level25(),
            26 => new BattlePass_Level26(),
            27 => new BattlePass_Level27(),
            28 => new BattlePass_Level28(),
            29 => new BattlePass_Level29(),
            30 => new BattlePass_Level30(),
            31 => new BattlePass_Level31(),
            _ => null
        };
    }
}