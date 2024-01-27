using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static IEnumerable<CCSPlayerController> GetPlayers(CsTeam? team = null)
    {
        return Utilities.GetPlayers()
             .Where(x => ValidateCallerPlayer(x, false)
                         && (GetTeam(x) == CsTeam.Terrorist || GetTeam(x) == CsTeam.CounterTerrorist)
                         && (team.HasValue ? team.Value == GetTeam(x) : true));
    }

    private static int GetPlayerCount(CsTeam? team = null, bool? alive = null)
    {
        List<CCSPlayerController> players = new();

        for (int i = 0; i < Server.MaxPlayers; i++)
        {
            var controller = Utilities.GetPlayerFromSlot(i);

            if (!controller.IsValid || controller.UserId == -1)
                continue;
            if ((team.HasValue ? team.Value == GetTeam(controller) : true) == false)
            {
                continue;
            }
            if ((alive.HasValue ? alive.Value == controller.PawnIsAlive : true) == false)
            {
                continue;
            }

            if ((GetTeam(controller) == CsTeam.Terrorist || GetTeam(controller) == CsTeam.CounterTerrorist))
            {
                players.Add(controller);
            }
        }

        return players.Count;
    }

    private static CCSPlayerController? GetWarden()
    {
        return GetPlayers().Where(x => ValidateCallerPlayer(x, false) && x.SteamID == LatestWCommandUser).FirstOrDefault();
    }

    private static bool CheckPermission(CCSPlayerController player)
    {
        bool res = false;
        foreach (var item in BaseRequiresPermissions)
        {
            if (AdminManager.PlayerHasPermissions(player, item))
            {
                res = true; break;
            }
        }
        return res;
    }

    public static bool is_valid(CCSPlayerController? player)
    {
        return player != null && player.IsValid && player.PlayerPawn.IsValid;
    }

    public static CCSPlayerPawn? Pawn(CCSPlayerController? player)
    {
        if (ValidateCallerPlayer(player, false) == false) return null;
        if (player == null || !IsValid(player))
        {
            return null;
        }

        CCSPlayerPawn? pawn = player.PlayerPawn.Value;

        return pawn;
    }

    public static int get_health(CCSPlayerController? player)
    {
        CCSPlayerPawn? pawn = Pawn(player);

        if (pawn == null)
        {
            return 100;
        }

        return pawn.Health;
    }

    private static bool ValidateCallerPlayer(CCSPlayerController? x, bool checkPermission = true, bool printMsg = true)
    {
        if (is_valid(x) == false)
        {
            return false;
        }

        if (x == null) return false;
        if (checkPermission)
        {
            if (CheckPermission(x) == false)
            {
                if (printMsg)
                {
                    x.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
                }
                return false;
            }
        }
        //if (player == null
        //    || !player.IsValid
        //    //|| player.PlayerPawn == null
        //    //|| !player.PlayerPawn.IsValid
        //    //|| player.PlayerPawn.Value == null
        //    //|| !player.PlayerPawn.Value.IsValid
        //    ) return false;
        //if (player.AuthorizedSteamID == null
        //    || player.AuthorizedSteamID.SteamId64 != player.SteamID)
        //{
        //    return false;
        //}
        //if (player.AuthorizedSteamID.IsValid() == false) return false;//todo chjeck
        if (x.IsBot) return false;
        if (x.Connected == PlayerConnectedState.PlayerConnected
            && x.Index != 32767
            && !x.IsHLTV
            //&& player.Pawn?.Value != null
            )
            if ((GetTeam(x) == CsTeam.Terrorist || GetTeam(x) == CsTeam.CounterTerrorist))
            {
                return true;
            }
        return false;
    }

    private bool OnCommandValidater(CCSPlayerController? player, bool checkPermission, string seviyeLevel = null, string permLevel = "@css/admin1")
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return false;
        }
        if (checkPermission)
        {
            if (string.IsNullOrWhiteSpace(seviyeLevel) == false)
            {
                if (HasLevelPermissionToActivate(player.SteamID, seviyeLevel) == false)
                {
                    if (AdminManager.PlayerHasPermissions(player, permLevel) == false)
                    {
                        player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
                        return false;
                    }
                }
            }
            else if (string.IsNullOrWhiteSpace(permLevel) == false)
            {
                if (AdminManager.PlayerHasPermissions(player, permLevel) == false)
                {
                    player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
                    return false;
                }
            }
            else
            {
                player.PrintToChat($"{Prefix}{CC.W} Admin ile görüşmen lazım.");
                return false;
            }
        }

        return true;
    }

    private static bool IsValid(CCSPlayerController? player)
    {
        return player != null && player.IsValid && player.PlayerPawn.IsValid;
    }

    private static void SetColour(CCSPlayerController? x, Color colour)
    {
        if (ValidateCallerPlayer(x, false) == false) return;

        CCSPlayerPawn? pawn = x.PlayerPawn.Value;

        if (pawn != null)
        {
            if (ValidateCallerPlayer(x, false) == false) return;

            pawn.RenderMode = RenderMode_t.kRenderTransColor;
            if (ValidateCallerPlayer(x, false) == false) return;
            pawn.Render = colour;
            if (ValidateCallerPlayer(x, false) == false) return;
            Utilities.SetStateChanged(pawn, "CBaseModelEntity", "m_clrRender");
            if (ValidateCallerPlayer(x, false) == false) return;
            Utilities.SetStateChanged(pawn, "CBaseModelEntity", "m_nRenderMode");
        }
    }

    internal static bool GetTargetAction(CCSPlayerController x, string target, CCSPlayerController? self)
    {
        var targetArgument = GetTargetArgument(target);

        return targetArgument switch
        {
            TargetForArgument.All => true,
            TargetForArgument.Alive => x.PawnIsAlive == true,
            TargetForArgument.Dead => x.PawnIsAlive == false,
            TargetForArgument.T => GetTeam(x) == CsTeam.Terrorist,
            TargetForArgument.Ct => GetTeam(x) == CsTeam.CounterTerrorist,
            TargetForArgument.None => x.PlayerName?.ToLower()?.Contains(target?.ToLower() ?? "") ?? false,
            TargetForArgument.Me => x.SteamID == self?.SteamID,
            TargetForArgument.UserIdIndex => GetUserIdIndex(target) == x.UserId,
            TargetForArgument.Aim => GetClosestPlayer(self, x),
            _ => false
        };
    }

    private static bool ExecuteFreezeOrUnfreeze(CCSPlayerController x, string target, string self, out bool randomFreeze)
    {
        randomFreeze = _random.NextDouble() >= 0.5;

        var targetArgument = GetTargetArgument(target);
        return targetArgument switch
        {
            TargetForArgument.T => GetTeam(x) == CsTeam.Terrorist,
            TargetForArgument.Ct => GetTeam(x) == CsTeam.CounterTerrorist,
            TargetForArgument.Random => randomFreeze,
            TargetForArgument.RandomT => randomFreeze && GetTeam(x) == CsTeam.Terrorist,
            TargetForArgument.RandomCt => randomFreeze && GetTeam(x) == CsTeam.CounterTerrorist,
            TargetForArgument.All => true,
            TargetForArgument.Alive => true,
            TargetForArgument.None => x.PlayerName?.ToLower()?.Contains(target?.ToLower()) ?? false,
            TargetForArgument.Me => x.PlayerName == self,
            TargetForArgument.UserIdIndex => GetUserIdIndex(target) == x.UserId,
            _ => false,
        };
    }

    [Obsolete("Bundayken çökme yaşandığı oluyordu, bundan mı emin değilim ama iptal")]
    private static CsTeam GetTeamOld(CCSPlayerController x) => x.PendingTeamNum != x.TeamNum ? (CsTeam)x.PendingTeamNum : (CsTeam)x.TeamNum;

    private static CsTeam GetTeam(CCSPlayerController x) => (CsTeam)x.TeamNum;

    private static TargetForArgument GetTargetArgument(string target) => target?.ToLower() switch
    {
        "@all" => TargetForArgument.All,
        "@t" => TargetForArgument.T,
        "@terrorist" => TargetForArgument.T,
        "@terorist" => TargetForArgument.T,
        "@ct" => TargetForArgument.Ct,
        "@counterstrike" => TargetForArgument.Ct,
        "@alive" => TargetForArgument.Alive,
        "@dead" => TargetForArgument.Dead,
        "@random" => TargetForArgument.Random,
        "@randomt" => TargetForArgument.RandomT,
        "@randomct" => TargetForArgument.RandomCt,
        "@me" => TargetForArgument.Me,
        "@aim" => TargetForArgument.Aim,
        _ when IsUserIdIndexChecker(target, out var userId) && userId != null => TargetForArgument.UserIdIndex,
        _ => TargetForArgument.None,
    };

    private static bool IsUserIdIndexChecker(string target, out int? userId)
    {
        userId = GetUserIdIndex(target);
        return userId.HasValue;
    }

    private static int? GetUserIdIndex(string target)
    {
        if (string.IsNullOrWhiteSpace(target)) return null;
        target = target.Trim();
        if (target.StartsWith("#"))
        {
            var split = target.Split("#");
            if (split.Length == 2)
            {
                if (int.TryParse(split[1], out var userIdout))
                {
                    return userIdout;
                }
            }
        }
        return null;
    }

    private static List<List<T>> ChunkBy<T>(List<T> list, int numberOfLists)
    {
        //if (numLists * elementsPerList != list.Count)
        //{
        //    throw new ArgumentException("The product of numLists and elementsPerList must equal the count of the input list.");
        //}
        //int elementsPerList = (int)Math.Ceiling((double)source.Count / chunkSize);
        //return Enumerable.Range(0, chunkSize)
        //                .Select(i => source.Skip(i * elementsPerList).Take(elementsPerList).ToList())
        //                .ToList();
        //return Enumerable.Range(0, (int)Math.Ceiling(source.Count / (double)chunkSize))
        //               .Select(i => source.Skip(i * chunkSize).Take(chunkSize).ToList())
        //               .ToList();
        //}

        int totalItems = list.Count;
        int itemsPerList = totalItems / numberOfLists;
        int remainder = totalItems % numberOfLists;

        List<List<T>> result = new List<List<T>>();

        int startIndex = 0;

        for (int i = 0; i < numberOfLists; i++)
        {
            int sublistSize = itemsPerList + (i < remainder ? 1 : 0);

            if (sublistSize > 0)
            {
                result.Add(list.GetRange(startIndex, sublistSize));
                startIndex += sublistSize;
            }
        }

        return result;
        //int totalItems = source.Count;
        //int chunks = (int)Math.Ceiling((double)totalItems / chunkSize);

        //List<List<T>> result = new List<List<T>>();

        //for (int i = 0; i < chunks; i++)
        //{
        //    int startIndex = i * chunkSize;
        //    int endIndex = Math.Min((i + 1) * chunkSize, totalItems);

        //    result.Add(source.GetRange(startIndex, endIndex - startIndex));
        //}

        //return result;
        //return source
        //    .Select((x, i) => new { Index = i, Value = x })
        //    .GroupBy(x => x.Index / chunkSize)
        //    .Select(x => x.Select(v => v.Value).ToList())
        //    .ToList();
    }

    private static double CalculateMinutesUntilSundayMidnight()
    {
        DateTime now = DateTime.UtcNow.AddHours(3);

        // Calculate the difference in days between today and the next Friday
        int daysUntilNextFriday = ((int)DayOfWeek.Sunday - (int)now.DayOfWeek + 7) % 7;

        // Calculate the next Friday by adding the difference in days
        DateTime nextFridayMidnight = now.AddDays(daysUntilNextFriday).Date.AddHours(24);

        // Calculate the milliseconds until the next Friday
        double millisecondsUntilNextFriday = (nextFridayMidnight - now).TotalMilliseconds;

        return millisecondsUntilNextFriday / 1000 / 60;
    }
}