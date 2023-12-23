using Dapper;
using Microsoft.Data.Sqlite;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private SqliteConnection DbConnection = null!;

    private void DatabaseInit()
    {
        DbConnection = new SqliteConnection($"Data Source={Path.Combine(ContentRootPath, "database.db")}");

        DbConnection.Open();

        _ = Task.Run(async () =>
        {
            var query = $@"
                CREATE TABLE IF NOT EXISTS `{nameof(PlayerMarketModel)}` (
	                `{nameof(PlayerMarketModel.SteamId)}` UNSIGNED BIG INT NOT NULL,
	                `{nameof(PlayerMarketModel.ModelIdCT)}` TEXT NULL,
                    `{nameof(PlayerMarketModel.ModelIdT)}` TEXT NULL,
                    `{nameof(PlayerMarketModel.DefaultIdCT)}` INTEGER NULL,
                    `{nameof(PlayerMarketModel.DefaultIdT)}` INTEGER NULL,
	                `{nameof(PlayerMarketModel.Credit)}` INTEGER DEFAULT 0,
	                PRIMARY KEY (`{nameof(PlayerMarketModel.SteamId)}`)
                );

                CREATE TABLE IF NOT EXISTS `{nameof(PlayerModel)}` (
	                `{nameof(PlayerModel.Id)}` INTEGER NOT NULL,
	                `{nameof(PlayerModel.Text)}` NVARCHAR(255) NULL,
                    `{nameof(PlayerModel.PathToModel)}` NVARCHAR(255) NULL,
                    `{nameof(PlayerModel.TeamNo)}` INTEGER DEFAULT 0,
                    `{nameof(PlayerModel.Cost)}` INTEGER DEFAULT 0,
                    `{nameof(PlayerModel.Enable)}` INT2 DEFAULT 0,
	                PRIMARY KEY (`{nameof(PlayerModel.Id)}`)
                );";
            await DbConnection.ExecuteAsync(query);
        });
    }
}