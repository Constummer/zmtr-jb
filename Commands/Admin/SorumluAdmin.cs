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
    public static List<ulong> SorumluAdmins { get; set; } = new();

    [ConsoleCommand("sorumluadmin")]
    [ConsoleCommand("sorumluadminekle")]
    [ConsoleCommand("sorumluadminekle")]
    [CommandHelper(minArgs: 1, "<sorumlu admin olacak kişi>")]
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
        if (string.IsNullOrWhiteSpace(targetPlayer))
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

        if (SorumluAdmins.Any(x => x == y.SteamID))
        {
            player.PrintToChat($"{Prefix}{CC.B} {y.PlayerName}{CC.W} isimli oyuncu halihazirda [Sorumlu].");
            return;
        }
        else
        {
            SorumluAdmins.Add(y.SteamID);
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.B} {y.PlayerName}{CC.W} adlı oyuncuyu {CC.R}[Sorumlu]{CC.W} olarak ekledi");
            AddSorumluAdminData(y.SteamID);
        }
    }

    private void AddSorumluAdminData(ulong steamId)
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
                if (!exist)
                {
                    cmd = new MySqlCommand(@$"INSERT INTO `SorumluAdmin`
                                      (SteamId)
                                      VALUES (@SteamId);", con);

                    cmd.Parameters.AddWithValue("@SteamId", steamId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    public static bool SorumluAdminSay(CCSPlayerController? player, CommandInfo info, bool isSayTeam)
    {
        if (SorumluAdmins.Contains(player.SteamID) == false)
        {
            return false;
        }
        var team = GetTeam(player);
        var teamColor = team switch
        {
            CsTeam.CounterTerrorist => CC.BG,
            CsTeam.Terrorist => CC.Y,
            CsTeam.Spectator => CC.LP,
            CsTeam.None => CC.Or,
        };
        var chatColor = team switch
        {
            CsTeam.CounterTerrorist => CC.BG,
            CsTeam.Terrorist => CC.Y,
            CsTeam.Spectator => CC.P,
            CsTeam.None => CC.Or,
        };
        var teamStr = team switch
        {
            CsTeam.CounterTerrorist => $"{CC.B}[GARDİYAN]",
            CsTeam.Terrorist => $"{CC.R}[MAHKÛM]",
            CsTeam.Spectator => $"{CC.P}[SPEC]",
            _ => ""
        };
        if (isSayTeam)
        {
            GetPlayers(team).ToList()
                .ForEach(x => x.PrintToChat($" {CC.M}[Sorumlu]{teamStr} {teamColor}{player.PlayerName} {CC.W}: {chatColor}{info.GetArg(1)}"));
        }
        else
        {
            Server.PrintToChatAll($" {CC.M}[Sorumlu] {teamColor}{player.PlayerName} {CC.W}: {chatColor}{info.GetArg(1)}");
        }
        return true;
    }

    private void GetAllSorumluAdminData(MySqlConnection con)
    {
        if (con == null)
        {
            return;
        }

        MySqlCommand? cmd = new MySqlCommand(@$"SELECT `SteamId` FROM `SorumluAdmin`;", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                if (steamId == 0)
                {
                    continue;
                }

                if (SorumluAdmins.Contains((ulong)steamId) == false)
                {
                    SorumluAdmins.Add((ulong)steamId);
                }
            }
        }

        return;
    }
}