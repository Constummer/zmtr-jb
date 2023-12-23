using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void LoadPlayerModels()
    {
        Logger.LogInformation("Loading player models");

        _ = Task.Run(async () =>
        {
            var command = DbConnection.CreateCommand();
            command.CommandText = @$"
                SELECT `{nameof(PlayerModel.Id)}`,
                       `{nameof(PlayerModel.Text)}`,
                       `{nameof(PlayerModel.TeamNo)}`,
                       `{nameof(PlayerModel.Cost)}`,
                       `{nameof(PlayerModel.PathToModel)}`
                FROM `{nameof(PlayerModel)}`
                WHERE `{nameof(PlayerModel.Enable)}` = 1";

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    var model = new PlayerModel(reader.GetInt32(0))
                    {
                        Text = reader.IsDBNull(1) ? null : reader.GetString(1),
                        TeamNo = reader.IsDBNull(2) ? 0 : (CsTeam)reader.GetInt32(2),
                        Cost = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        PathToModel = reader.IsDBNull(4) ? null : reader.GetString(4),
                    };
                    PlayerModels.Add(model.Id, model);
                }
            }
        });
    }
}