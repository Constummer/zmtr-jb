using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Memory;
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

    private static void SetMoveType(CCSPlayerController x, MoveType_t mtype)
    {
        var actualMType = Schema.GetRef<MoveType_t>(x.PlayerPawn.Value.Handle, "CBaseEntity", "m_nActualMoveType");
        x.PlayerPawn.Value!.MoveType = mtype;
        actualMType = mtype;
        Schema.SetSchemaValue<byte>(x.PlayerPawn.Value.Handle, "CBaseEntity", "m_nActualMoveType", (byte)mtype);
        Utilities.SetStateChanged(x.PlayerPawn.Value, "CBaseEntity", "m_nActualMoveType");
        Utilities.SetStateChanged(x.PlayerPawn.Value, "CBaseEntity", "m_MoveType");
    }

    private static void SetStatusClanTag(CCSPlayerController? player)
    {
        //if (ValidateCallerPlayer(player, false) == false) return;
        //if (string.IsNullOrWhiteSpace(player.Clan))
        //{
        //    player.Clan = $"[#{player.UserId}]";
        //}
        //else
        //{
        //    if (!player.Clan.Contains($"[#{player.UserId}]"))
        //    {
        //        player.Clan += $"[#{player.UserId}]";
        //    }
        //}
        Global?.AddTimer(0.2f, () =>
        {
            if (ValidateCallerPlayer(player, false) == false) return;
            Utilities.SetStateChanged(player, "CCSPlayerController", "m_szClan");
            if (ValidateCallerPlayer(player, false) == false) return;
            Utilities.SetStateChanged(player, "CBasePlayerController", "m_iszPlayerName");
        }, SOM);
    }

    private static int GetPlayerCount(CsTeam? team = null, bool? alive = null)
    {
        int players = 0;

        for (int i = 0; i < Server.MaxPlayers; i++)
        {
            var controller = Utilities.GetPlayerFromSlot(i);

            if (controller == null
                || !controller.IsValid
                || controller.UserId == -1
                || controller.PlayerPawn == null
                || controller.PlayerPawn.Value == null
                || !controller.PlayerPawn.IsValid)
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
                players++;
            }
        }

        return players;
    }

    private static CCSPlayerController? GetWarden()
    {
        return GetPlayers().Where(x => ValidateCallerPlayer(x, false) && x.SteamID == LatestWCommandUser).FirstOrDefault();
    }

    private static bool CheckPermission(CCSPlayerController player)
    {
        bool res = false;
        if (AdminManager.PlayerHasPermissions(player, Perm_Admin1))
        {
            res = true;
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
                    x.PrintToChat(NotEnoughPermission);
                }
                return false;
            }
        }
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

    private bool OnCommandValidater(CCSPlayerController? player, bool checkPermission, string? seviyeLevel = null, string permLevel = Perm_Admin1, bool printMsg = true)
    {
        if (ValidateCallerPlayer(player, false, printMsg) == false)
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
                        player.PrintToChat(NotEnoughPermission);
                        return false;
                    }
                }
            }
            else if (string.IsNullOrWhiteSpace(permLevel) == false)
            {
                if (AdminManager.PlayerHasPermissions(player, permLevel) == false)
                {
                    player.PrintToChat(NotEnoughPermission);
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

    internal static bool GetTargetAction(CCSPlayerController x, string target, CCSPlayerController? self, bool forSingle = false)
    {
        if (string.IsNullOrWhiteSpace(target)) return false;
        var targetArgument = GetTargetArgument(target);

        return targetArgument switch
        {
            TargetForArgument.All => forSingle ? GetExtraCheck(x, "@all") : true,
            TargetForArgument.Alive => forSingle ? GetExtraCheck(x, "@alive") : x.PawnIsAlive == true,
            TargetForArgument.Dead => forSingle ? GetExtraCheck(x, "@dead") : x.PawnIsAlive == false,
            TargetForArgument.T => forSingle ? GetExtraCheck(x, "@t") : GetTeam(x) == CsTeam.Terrorist,
            TargetForArgument.Ct => forSingle ? GetExtraCheck(x, "@ct") : GetTeam(x) == CsTeam.CounterTerrorist,
            TargetForArgument.None => (x.PlayerName?.ToLower()?.Contains(target?.ToLower() ?? "") ?? false) || x.SteamID.ToString() == target,
            TargetForArgument.Me => forSingle ? GetExtraCheck(x, "@me") : x.SteamID == self?.SteamID,
            TargetForArgument.UserIdIndex => GetUserIdIndex(target) == x.UserId,
            TargetForArgument.Aim => GetClosestPlayer(self, x),
            TargetForArgument.Sut => forSingle ? GetExtraCheck(x, "@sut") : IsSutPlayer(x),
            _ => false
        };

        static bool GetExtraCheck(CCSPlayerController x, string text) => x.PlayerName?.ToLower() == text;

        static bool IsSutPlayer(CCSPlayerController x) => SutolCommandCalls.Contains(x.SteamID);
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
        "@sut" => TargetForArgument.Sut,
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