using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    /// CREATE TABLE IF NOT EXISTS `ManagerCommands` (
    ///                  `SteamId` bigint(20) DEFAULT NULL,
    ///                  `Command` TEXT DEFAULT NULL,
    ///                  `Time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
    ///                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

    private void LogManagerCommand(ulong steamId, string command)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"INSERT INTO `ManagerCommands`
                                     (SteamId,Command)
                                     VALUES (@SteamId,@Command);", con);

                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.Parameters.AddWithValue("@Command", command);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
        }
    }
}