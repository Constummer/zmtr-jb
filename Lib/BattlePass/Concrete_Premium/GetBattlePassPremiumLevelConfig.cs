namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static BattlePassPremiumBase? GetBattlePassPremiumLevelConfig(int level)
    {
        return level switch
        {
            1 => new BattlePassPremium_Level01(),
            2 => new BattlePassPremium_Level02(),
            3 => new BattlePassPremium_Level03(),
            4 => new BattlePassPremium_Level04(),
            5 => new BattlePassPremium_Level05(),
            6 => new BattlePassPremium_Level06(),
            7 => new BattlePassPremium_Level07(),
            8 => new BattlePassPremium_Level08(),
            9 => new BattlePassPremium_Level09(),
            10 => new BattlePassPremium_Level10(),
            11 => new BattlePassPremium_Level11(),
            12 => new BattlePassPremium_Level12(),
            13 => new BattlePassPremium_Level13(),
            14 => new BattlePassPremium_Level14(),
            15 => new BattlePassPremium_Level15(),
            16 => new BattlePassPremium_Level16(),
            _ => null
        };
    }
}