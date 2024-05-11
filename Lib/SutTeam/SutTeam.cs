using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static List<ulong> SutTeamPlayers { get; set; } = new();

    private static void CheckPlayerSutTeamTag(ulong tempSteamId)
    {
        if (SutTeamPlayers.Any(x => x == tempSteamId))
        {
            var player = GetPlayers().Where(x => x.SteamID == tempSteamId).FirstOrDefault();
            if (ValidateCallerPlayer(player, false) == false) return;
            SetSutTeamClanTag(player);
        }
    }

    private static void SetSutTeamClanTag(CCSPlayerController? player)
    {
        if (ValidateCallerPlayer(player, false) == false) return;
        player.Clan = "[Süt Team]";
        SetStatusClanTag(player);
    }

    private void GetAllPlayerSutTeamData(MySqlConnection con)
    {
        if (con == null)
        {
            return;
        }
        MySqlCommand? cmd = new MySqlCommand(@$"SELECT `SteamId` FROM `PlayerSutTeam`", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);

                if (steamId > 0)
                {
                    SutTeamPlayers.Add((ulong)steamId);
                }
            }
        }
    }

    private void AddPlayerSutteamData(ulong steamID)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"INSERT INTO `PlayerSutTeam`
                                          (SteamId)
                                          VALUES (@SteamId);", con);

                cmd.Parameters.AddWithValue("@SteamId", steamID);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void RemovePlayerSutteamData(ulong steamID)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"Delete From `PlayerSutTeam` WHERE `SteamId` = @SteamId;", con);

                cmd.Parameters.AddWithValue("@SteamId", steamID);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }
}