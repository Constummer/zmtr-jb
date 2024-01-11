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
            if ((team.HasValue ? team.Value != GetTeam(controller) : true) == false)
            {
                continue;
            }
            if ((alive.HasValue ? alive.Value == controller.PawnIsAlive : true) == false)
            {
                continue;
            }

            players.Add(controller);
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

    private static bool ValidateCallerPlayer(CCSPlayerController? player, bool checkPermission = true, bool printMsg = true)
    {
        if (player == null) return false;
        if (checkPermission)
        {
            if (CheckPermission(player) == false)
            {
                if (printMsg)
                {
                    player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
                }
                return false;
            }
        }
        if (player == null
            || !player.IsValid
            //|| player.PlayerPawn == null
            //|| !player.PlayerPawn.IsValid
            //|| player.PlayerPawn.Value == null
            //|| !player.PlayerPawn.Value.IsValid
            ) return false;
        //if (player.AuthorizedSteamID == null
        //    || player.AuthorizedSteamID.SteamId64 != player.SteamID)
        //{
        //    return false;
        //}
        //if (player.AuthorizedSteamID.IsValid() == false) return false;//todo chjeck
        if (player.IsBot) return false;
        if (player.Connected == PlayerConnectedState.PlayerConnected
            && player.Index != 32767
            && !player.IsHLTV
            //&& player.Pawn?.Value != null
            )
            return true;
        return false;
    }

    private static bool IsValid(CCSPlayerController? player)
    {
        return player != null && player.IsValid && player.PlayerPawn.IsValid;
    }

    private static void SetColour(CCSPlayerController? player, Color colour)
    {
        if (player == null || !IsValid(player))
        {
            return;
        }

        CCSPlayerPawn? pawn = player.PlayerPawn.Value;

        if (pawn != null)
        {
            pawn.RenderMode = RenderMode_t.kRenderTransColor;
            pawn.Render = colour;
        }
    }

    private static void RemoveWeapons(CCSPlayerController x, bool knifeStays)
    {
        if (x?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
        {
            foreach (var weapon in x.PlayerPawn.Value.WeaponServices!.MyWeapons)
            {
                if (weapon.Value != null
                    && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                    && weapon.Value.DesignerName != "[null]")
                {
                    if (knifeStays == true)
                    {
                        if (weapon.Value.DesignerName.Contains("knife") == false)
                        {
                            weapon.Value.Remove();
                        }
                    }
                    else
                    {
                        weapon.Value.Remove();
                    }
                }
            }
        }
    }

    private void HideWeapons(CCSPlayerController x)
    {
        if (x?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
        {
            foreach (var weapon in x.PlayerPawn.Value.WeaponServices!.MyWeapons)
            {
                if (weapon.Value != null
                    && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                    && weapon.Value.DesignerName != "[null]")
                {
                    weapon.Value.Render = Color.FromArgb(0, 0, 0, 0);
                    AddTimer(0.2f, () =>
                    {
                        Utilities.SetStateChanged(weapon.Value, "CBaseModelEntity", "m_clrRender");
                    });
                }
            }
        }
    }

    private void ShowWeapons(CCSPlayerController x)
    {
        if (x?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
        {
            foreach (var weapon in x.PlayerPawn.Value.WeaponServices!.MyWeapons)
            {
                if (weapon.Value != null
                    && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                    && weapon.Value.DesignerName != "[null]")
                {
                    weapon.Value.Render = DefaultColor;
                    AddTimer(0.2f, () =>
                    {
                        Utilities.SetStateChanged(weapon.Value, "CBaseModelEntity", "m_clrRender");
                    });
                }
            }
        }
    }

    private static List<uint> PlayerWeaponIndexes(CCSPlayerController x)
    {
        var res = new List<uint>();
        if (x?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
        {
            foreach (var weapon in x.PlayerPawn.Value.WeaponServices!.MyWeapons)
            {
                if (weapon.Value != null
                    && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                    && weapon.Value.DesignerName != "[null]")
                {
                    res.Add(weapon.Index);
                }
            }
        }
        return res;
    }

    private static bool GetTargetAction(CCSPlayerController x, string target, string self)
    {
        var targetArgument = GetTargetArgument(target);

        return targetArgument switch
        {
            TargetForArgument.All => true,
            TargetForArgument.T => GetTeam(x) == CsTeam.Terrorist,
            TargetForArgument.Ct => GetTeam(x) == CsTeam.CounterTerrorist,
            TargetForArgument.None => x.PlayerName?.ToLower()?.Contains(target) ?? false,
            TargetForArgument.Me => x.PlayerName == self,
            TargetForArgument.UserIdIndex => GetUserIdIndex(target) == x.UserId,
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
            TargetForArgument.None => x.PlayerName?.ToLower()?.Contains(target) ?? false,
            TargetForArgument.Me => x.PlayerName == self,
            TargetForArgument.UserIdIndex => GetUserIdIndex(target) == x.UserId,
            _ => false,
        };
    }

    private static CsTeam GetTeamOld(CCSPlayerController x) => x.PendingTeamNum != x.TeamNum ? (CsTeam)x.PendingTeamNum : (CsTeam)x.TeamNum;

    private static CsTeam GetTeam(CCSPlayerController x) => (CsTeam)x.TeamNum;

    private static TargetForArgument GetTargetArgument(string target) => target switch
    {
        "@all" => TargetForArgument.All,
        "@ALL" => TargetForArgument.All,
        "@t" => TargetForArgument.T,
        "@T" => TargetForArgument.T,
        "@terrorist" => TargetForArgument.T,
        "@terorist" => TargetForArgument.T,
        "@TERRORIST" => TargetForArgument.T,
        "@TERORIST" => TargetForArgument.T,
        "@ct" => TargetForArgument.Ct,
        "@Ct" => TargetForArgument.Ct,
        "@cT" => TargetForArgument.Ct,
        "@CT" => TargetForArgument.Ct,
        "@counterstrike" => TargetForArgument.Ct,
        "@COUNTERSTRIKE" => TargetForArgument.Ct,
        "@COUNTERSTRİKE" => TargetForArgument.Ct,
        "@alive" => TargetForArgument.Alive,
        "@ALIVE" => TargetForArgument.Alive,
        "@dead" => TargetForArgument.Dead,
        "@DEAD" => TargetForArgument.Dead,
        "@random" => TargetForArgument.Random,
        "@RANDOM" => TargetForArgument.Random,
        "@randomt" => TargetForArgument.RandomT,
        "@RANDOMT" => TargetForArgument.RandomT,
        "@randomct" => TargetForArgument.RandomCt,
        "@RANDOMCT" => TargetForArgument.RandomCt,
        "@me" => TargetForArgument.Me,
        "@ME" => TargetForArgument.Me,
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