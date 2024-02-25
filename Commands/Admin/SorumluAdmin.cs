using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static Dictionary<ulong, SorumluAdminTypes> SorumluAdmins { get; set; } = new();

    public enum SorumluAdminTypes
    {
        None = 0,
        Sabah,
        Aksam
    }

    [ConsoleCommand("sorumluadmin")]
    [ConsoleCommand("sorumluadminekle")]
    [ConsoleCommand("sorumluadminekle")]
    [CommandHelper(minArgs: 2, "<sorumlu admin olacak kişi> <1: Sabah, 2: Akşam>")]
    public void SorumluAdmin(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/yonetim"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var targetPlayer = info.ArgString.GetArg(0);
        var type = info.ArgString.GetArg(1);
        if (string.IsNullOrWhiteSpace(targetPlayer) || string.IsNullOrWhiteSpace(type))
        {
            return;
        }
        if (Enum.TryParse<SorumluAdminTypes>(type, out var typeRes) == false || typeRes == SorumluAdminTypes.None)
        {
            return;
        }
        var targetArgument = GetTargetArgument(targetPlayer);

        var players = GetPlayers()
               .Where(x =>
               (targetArgument == TargetForArgument.UserIdIndex
               ? GetUserIdIndex(targetPlayer) == x.UserId : targetArgument == TargetForArgument.Me
               ? x.SteamID == player.SteamID : false)
               || (x.PlayerName?.ToLower()?.Contains(targetPlayer?.ToLower()) ?? false)
               || x.SteamID.ToString() == targetPlayer)
               .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
            return;
        }
        if (players.Count != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
            return;
        }
        var y = players.FirstOrDefault();
        if (ValidateCallerPlayer(y, false) == false) return;

        var sorumluAdminType = typeRes switch
        {
            SorumluAdminTypes.Sabah => "[Sabah Sorumlu]",
            SorumluAdminTypes.Aksam => "[Akşam Sorumlu]",
        };
        if (SorumluAdmins.Any(x => x.Key == y.SteamID))
        {
            SorumluAdmins[player.SteamID] = typeRes;
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.B} {y.PlayerName}{CC.W} adlı oyuncuyu {CC.R}{sorumluAdminType}{CC.W} olarak güncelledi");
        }
        else
        {
            SorumluAdmins.Add(y.SteamID, typeRes);
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.B} {y.PlayerName}{CC.W} adlı oyuncuyu {CC.R}{sorumluAdminType}{CC.W} olarak aldı");
        }
        AddOrUpdateSorumluAdminData(y.SteamID, (int)typeRes);
    }

    private void AddOrUpdateSorumluAdminData(ulong steamId, int type)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }

                var cmd = new MySqlCommand(@$"SELECT 1 FROM `SorumluAdmin` WHERE `SteamId` = @SteamId;", con);
                cmd.Parameters.AddWithValue("@SteamId", steamId);
                bool exist = false;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        exist = true;
                    }
                }
                if (exist)
                {
                    cmd = new MySqlCommand(@$"UPDATE `SorumluAdmin`
                                          SET
                                              `TypeId` = @TypeId
                                          WHERE `SteamId` = @SteamId;
                ", con);

                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    cmd.Parameters.AddWithValue("@TypeId", type);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd = new MySqlCommand(@$"INSERT INTO `SorumluAdmin`
                                      (SteamId,TypeId)
                                      VALUES (@SteamId,@TypeId);", con);

                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    cmd.Parameters.AddWithValue("@TypeId", type);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    public static bool SorumluAdminSay(CCSPlayerController? player, CommandInfo info)
    {
        if (SorumluAdmins.TryGetValue(player.SteamID, out var result) == false)
        {
            return false;
        }
        var teamColor = GetTeam(player) switch
        {
            CsTeam.CounterTerrorist => CC.BG,
            CsTeam.Terrorist => CC.Y,
            CsTeam.Spectator => CC.LP,
            CsTeam.None => CC.Or,
        };
        var chatColor = GetTeam(player) switch
        {
            CsTeam.CounterTerrorist => CC.BG,
            CsTeam.Terrorist => CC.Y,
            CsTeam.Spectator => CC.P,
            CsTeam.None => CC.Or,
        };

        var sorumluAdminType = result switch
        {
            SorumluAdminTypes.Sabah => "[Sabah Sorumlu]",
            SorumluAdminTypes.Aksam => "[Akşam Sorumlu]",
        };

        Server.PrintToChatAll($" {CC.M}{sorumluAdminType} {teamColor}{player.PlayerName} {CC.W}: {chatColor}{info.GetArg(1)}");
        return true;
    }

    private void GetAllSorumluAdminData(MySqlConnection con)
    {
        if (con == null)
        {
            return;
        }

        MySqlCommand? cmd = new MySqlCommand(@$"SELECT `SteamId`, `TypeId` FROM `SorumluAdmin`;", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                var typeId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                if (steamId == 0)
                {
                    continue;
                }
                if (typeId == 0)
                {
                    continue;
                }

                if (SorumluAdmins.ContainsKey((ulong)steamId) == false)
                {
                    SorumluAdmins.Add((ulong)steamId, (SorumluAdminTypes)typeId);
                }
                else
                {
                    SorumluAdmins[(ulong)steamId] = (SorumluAdminTypes)typeId;
                }
            }
        }

        return;
    }
}