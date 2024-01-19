using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static List<ulong> IsyanTeamPlayers { get; set; } = new();

    private static void CheckPlayerIsTeamTag(ulong tempSteamId)
    {
        if (IsyanTeamPlayers.Any(x => x == tempSteamId))
        {
            var player = GetPlayers().Where(x => x.SteamID == tempSteamId).FirstOrDefault();
            if (ValidateCallerPlayer(player, false) == false) return;
            SetIsyanTeamClanTag(player);
        }
    }

    private static void SetIsyanTeamClanTag(CCSPlayerController? player)
    {
        if (ValidateCallerPlayer(player, false) == false) return;
        player.Clan = "[İsyan Team]";
        Global?.AddTimer(0.2f, () =>
         {
             if (ValidateCallerPlayer(player, false) == false) return;
             Utilities.SetStateChanged(player, "CCSPlayerController", "m_szClan");
             if (ValidateCallerPlayer(player, false) == false) return;
             Utilities.SetStateChanged(player, "CBasePlayerController", "m_iszPlayerName");
         }, SOM);
    }

    private void GetAllPlayerIsyanTeamData(MySqlConnection con)
    {
        if (con == null)
        {
            return;
        }
        MySqlCommand? cmd = new MySqlCommand(@$"SELECT `SteamId` FROM `PlayerIsyanTeam`", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);

                if (steamId > 0)
                {
                    IsyanTeamPlayers.Add((ulong)steamId);
                }
            }
        }
    }

    private void AddPlayerIsteamData(ulong steamID)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"INSERT INTO `PlayerIsyanTeam`
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

    private void RemovePlayerIsteamData(ulong steamID)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;", con);

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