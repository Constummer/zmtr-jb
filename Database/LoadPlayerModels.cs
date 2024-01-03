using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Reflection.PortableExecutable;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void LoadPlayerModels()
    {
        var con = Connection();

        if (con == null)
        {
            return;
        }
        _ = Task.Run(async () =>
        {
            var cmd = new MySqlCommand(@$"
                SELECT Id, `Text`, PathToModel, TeamNo, Cost FROM PlayerModel;
                WHERE `Enable` = 1", con);

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    var model = new PlayerModel(reader.GetInt32(0))
                    {
                        Text = reader.IsDBNull(1) ? "" : reader.GetString(1),
                        PathToModel = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        TeamNo = reader.IsDBNull(3) ? 0 : (CsTeam)reader.GetInt32(3),
                        Cost = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    };
                    Logger.LogInformation("------------------------------------------------model.Text = " + model.Text);
                    Logger.LogInformation("------------------------------------------------model.PathToModel = " + model.PathToModel);

                    PlayerModels.Add(model.Id, model);
                }
            }
        });
    }
}