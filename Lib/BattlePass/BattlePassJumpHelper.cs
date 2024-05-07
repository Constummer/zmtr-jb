using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static int GetJumpSecondDiff => (int)(DateTime.UtcNow.Subtract(DateTime.UtcNow.Date.AddDays(-1))).TotalSeconds;

    private static Dictionary<ulong, Dictionary<int, int>> BattlePassJumps = new();
    private static Dictionary<ulong, Dictionary<int, int>> BattlePassPremiumJumps = new();

    private int GetAddJumpAmount(Dictionary<int, int> jumpPerSecondDatas)
    {
        var total = 0;
        foreach (var second in jumpPerSecondDatas)
        {
            Server.PrintToChatAll($"{second.Key}_{second.Value}");
            if (second.Value > 3)
            {
                total += 3;
            }
            else
            {
                total += second.Value;
            }
        }
        return total;
    }

    private static void SaveBattlePasssesJumpDataToCache(EventPlayerJump @event)
    {
        var second = GetJumpSecondDiff;
        try
        {
            if (BattlePassPremiumJumps.TryGetValue(@event.Userid.SteamID, out var bppdata))
            {
                if (bppdata != null)
                {
                    if (bppdata.TryGetValue(second, out var jumpCount))
                    {
                        bppdata[second] = jumpCount + 1;
                    }
                    else
                    {
                        bppdata.Add(second, 1);
                    }
                }
                else
                {
                    bppdata = new() { { second, 1 } };
                }
                BattlePassPremiumJumps[@event.Userid.SteamID] = bppdata;
            }
            else
            {
                BattlePassPremiumJumps.Add(@event.Userid.SteamID, new() { { second, 1 } });
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            if (BattlePassJumps.TryGetValue(@event.Userid.SteamID, out var bpdata))
            {
                if (bpdata != null)
                {
                    if (bpdata.TryGetValue(second, out var jumpCount))
                    {
                        bpdata[second] = jumpCount + 1;
                    }
                    else
                    {
                        bpdata.Add(second, 1);
                    }
                }
                else
                {
                    bpdata = new() { { second, 1 } };
                }
                BattlePassJumps[@event.Userid.SteamID] = bpdata;
            }
            else
            {
                BattlePassJumps.Add(@event.Userid.SteamID, new() { { second, 1 } });
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
    }
}