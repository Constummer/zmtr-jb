using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private async Task AddOrUpdatePlayerToPlayerNameTable(ulong steamId, string playerName)
    {
        var con = Connection();
        if (con == null)
        {
            return;
        }
        try
        {
            var cmd = new MySqlCommand(@$"SELECT 1 FROM `PlayerName` WHERE `SteamId` = @SteamId;", con);
            cmd.Parameters.AddWithValue("@SteamId", steamId);
            bool exist = false;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (reader.Read())
                {
                    exist = true;
                }
            }
            if (exist)
            {
                cmd = new MySqlCommand(@$"UPDATE `PlayerName`
                                          SET `Name` = @Name
                                          WHERE `SteamId` = @SteamId;", con);

                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.Parameters.AddWithValue("@Name", playerName);

                await cmd.ExecuteNonQueryAsync();
            }
            else
            {
                cmd = new MySqlCommand(@$"INSERT INTO `PlayerName`
                                          (`SteamId`,`Name`)
                                          Values
                                          (@SteamId,@Name);", con);

                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.Parameters.AddWithValue("@Name", playerName);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }
}