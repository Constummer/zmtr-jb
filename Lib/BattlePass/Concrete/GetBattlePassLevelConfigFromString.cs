using System.Text.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static BattlePassBase? GetBattlePassLevelConfigFromString(int level, string config)
    {
        return level switch
        {
            1 => JsonSerializer.Deserialize<BattlePass_Level01>(config),
            2 => JsonSerializer.Deserialize<BattlePass_Level02>(config),
            3 => JsonSerializer.Deserialize<BattlePass_Level03>(config),
            4 => JsonSerializer.Deserialize<BattlePass_Level04>(config),
            5 => JsonSerializer.Deserialize<BattlePass_Level05>(config),
            6 => JsonSerializer.Deserialize<BattlePass_Level06>(config),
            7 => JsonSerializer.Deserialize<BattlePass_Level07>(config),
            8 => JsonSerializer.Deserialize<BattlePass_Level08>(config),
            9 => JsonSerializer.Deserialize<BattlePass_Level09>(config),
            10 => JsonSerializer.Deserialize<BattlePass_Level10>(config),
            11 => JsonSerializer.Deserialize<BattlePass_Level11>(config),
            12 => JsonSerializer.Deserialize<BattlePass_Level12>(config),
            13 => JsonSerializer.Deserialize<BattlePass_Level13>(config),
            14 => JsonSerializer.Deserialize<BattlePass_Level14>(config),
            15 => JsonSerializer.Deserialize<BattlePass_Level15>(config),
            16 => JsonSerializer.Deserialize<BattlePass_Level16>(config),
            17 => JsonSerializer.Deserialize<BattlePass_Level17>(config),
            18 => JsonSerializer.Deserialize<BattlePass_Level18>(config),
            19 => JsonSerializer.Deserialize<BattlePass_Level19>(config),
            20 => JsonSerializer.Deserialize<BattlePass_Level20>(config),
            21 => JsonSerializer.Deserialize<BattlePass_Level21>(config),
            22 => JsonSerializer.Deserialize<BattlePass_Level22>(config),
            23 => JsonSerializer.Deserialize<BattlePass_Level23>(config),
            24 => JsonSerializer.Deserialize<BattlePass_Level24>(config),
            25 => JsonSerializer.Deserialize<BattlePass_Level25>(config),
            26 => JsonSerializer.Deserialize<BattlePass_Level26>(config),
            27 => JsonSerializer.Deserialize<BattlePass_Level27>(config),
            28 => JsonSerializer.Deserialize<BattlePass_Level28>(config),
            29 => JsonSerializer.Deserialize<BattlePass_Level29>(config),
            30 => JsonSerializer.Deserialize<BattlePass_Level30>(config),
            _ => null
        };
    }
}