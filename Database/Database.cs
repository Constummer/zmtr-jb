using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private MySqlConnection? Connection()
    {
        try
        {
            MySqlConnection? database = new MySqlConnection(
                $"Server={Config.Database.Server};User ID={Config.Database.Username};Password={Config.Database.Password};Database={Config.Database.Database};Port={Config.Database.Port}");

            database.Open();

            return database;
        }
        catch
        {
            //Console.WriteLine(ex.ToString());
            return null;
        }
    }

    private void Database()
    {
        var con = Connection();

        if (con == null)
        {
            return;
        }

        _ = Task.Run(async () =>
        {
            var cmd = new MySqlCommand("CREATE DATABASE IF NOT EXISTS cs2_extras", con);
            await cmd.ExecuteNonQueryAsync();

            cmd = new MySqlCommand(
                @"CREATE TABLE IF NOT EXISTS `PlayerMarketModel` (
                  `SteamId` bigint(20) DEFAULT NULL,
                  `ModelIdCT` varchar(0) DEFAULT NULL,
                  `ModelIdT` varchar(1) DEFAULT NULL,
                  `DefaultIdCT` varchar(0) DEFAULT NULL,
                  `DefaultIdT` varchar(1) DEFAULT NULL,
                  `Credit` mediumint(9) DEFAULT NULL
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
            await cmd.ExecuteNonQueryAsync();

            //cmd = new MySqlCommand(
            // @"CREATE TABLE IF NOT EXISTS `PlayerModel` (
            //     `Id` INT DEFAULT NULL,
            //     `Text` VARCHAR(14) DEFAULT NULL,
            //     `PathToModel` VARCHAR(86) DEFAULT NULL,
            //     `TeamNo` TINYINT DEFAULT NULL,
            //     `Cost` MEDIUMINT DEFAULT NULL,
            //     `Enable` TINYINT DEFAULT NULL
            //   ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4", con);
            //await cmd.ExecuteNonQueryAsync();
        });
    }
}