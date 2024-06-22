using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static void AddOrUpdatePlayerToPlayerNameTable(ulong steamId, string playerName)
    {
        if (PlayerNamesDatas.ContainsKey(steamId))
        {
            PlayerNamesDatas[steamId] = playerName;
        }
        else
        {
            PlayerNamesDatas.Add(steamId, playerName);
        }

        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"SELECT 1 FROM `PlayerName` WHERE `SteamId` = @SteamId;", con);
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
                    cmd = new MySqlCommand(@$"UPDATE `PlayerName`
                                          SET `Name` = @Name
                                          WHERE `SteamId` = @SteamId;", con);

                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    cmd.Parameters.AddWithValue("@Name", playerName);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd = new MySqlCommand(@$"INSERT INTO `PlayerName`
                                          (`SteamId`,`Name`)
                                          Values
                                          (@SteamId,@Name);", con);

                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    cmd.Parameters.AddWithValue("@Name", playerName);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
        }
    }
}