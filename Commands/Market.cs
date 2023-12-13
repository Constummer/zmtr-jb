using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using Dapper;
using Microsoft.Data.Sqlite;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, PlayerMarketModel> PlayerMarketModels = new();
    private static Dictionary<int, PlayerModel> PlayerModels = new();

    private enum ModelMenuType
    {
        CT = 1,
        T = 2
    }

    private const string CTOyuncuModeli = "CT Oyuncu Modelleri";
    private const string TOyuncuModeli = "T Oyuncu Modelleri";
    private const string CTModeli = " CT Modeli";
    private const string TModeli = " T Modeli";

    #region Market

    [ConsoleCommand("market")]
    public void Market(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (player?.SteamID == null || player!.SteamID == 0)
        {
            return;
        }

        var data = GetPlayerMarketModel(player.SteamID);
        if (data.Model == null) return;

        var marketMenu = new ChatMenu($"Market | Krediniz = [{data.Model.Credit}]");
        marketMenu.AddMenuOption(CTOyuncuModeli, OpenSelectedModelMarket);
        marketMenu.AddMenuOption(TOyuncuModeli, OpenSelectedModelMarket);

        ChatMenus.OpenMenu(player, marketMenu);
    }

    private void OpenSelectedModelMarket(CCSPlayerController player, ChatMenuOption option)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (player?.SteamID == null || player!.SteamID == 0)
        {
            return;
        }
        switch (option.Text)
        {
            case CTOyuncuModeli:
                GetPlayerModelsMenu(player, CsTeam.CounterTerrorist);
                break;

            case TOyuncuModeli:
                GetPlayerModelsMenu(player, CsTeam.Terrorist);
                break;

            default:
                break;
        }
    }

    private void GetPlayerModelsMenu(CCSPlayerController player, CsTeam team, bool onlyBoughtOnes = false)
    {
        var data = GetPlayerMarketModel(player.SteamID);
        if (data.Model == null) return;
        if (PlayerModels == null) return;

        var ctmodels = (data.Model.ModelIdCT ?? "").Split(",").ToList();
        var tmodels = (data.Model.ModelIdT ?? "").Split(",").ToList();

        var menuText = team switch
        {
            CsTeam.CounterTerrorist => CTOyuncuModeli,
            CsTeam.Terrorist => TOyuncuModeli,
            _ => "Market"
        };
        var marketMenu = new ChatMenu($"{menuText} Market | Krediniz = [{data.Model.Credit}]");
        var models = PlayerModels.ToList().Where(x => x.Value.TeamNo == team).ToList();
        if (onlyBoughtOnes && models.Count == 0)
        {
            player.PrintToChat("Satın aldığınız hiç eşya yok");
        }
        else
        {
            foreach (var item in models)
            {
                var selected = team switch
                {
                    CsTeam.CounterTerrorist => ctmodels.Contains(item.Key.ToString()),
                    CsTeam.Terrorist => tmodels.Contains(item.Key.ToString()),
                    _ => false
                };

                if (onlyBoughtOnes)
                {
                    if (selected == false)
                    {
                        continue;
                    }
                }
                var teamText = team switch
                {
                    CsTeam.CounterTerrorist => CTModeli,
                    CsTeam.Terrorist => TModeli,
                };

                var text = $"{item.Value.Text} | ({item.Value.Cost}) | {teamText}";
                if (selected)
                {
                    text += " | [SATIN ALINDI]";
                }
                marketMenu.AddMenuOption(text, OpenSelectedModel);
            }
        }
        ChatMenus.OpenMenu(player, marketMenu);
    }

    private void OpenSelectedModel(CCSPlayerController player, ChatMenuOption option)
    {
        if (ValidateCallerPlayer(player) == false) return;
        if (player?.SteamID == null || player!.SteamID == 0) return;
        if (string.IsNullOrWhiteSpace(option.Text)) return;

        var data = GetPlayerMarketModel(player.SteamID);
        if (data.Model == null) return;

        var modelName = option.Text?.Split(" | (")?[0];
        if (string.IsNullOrWhiteSpace(modelName))
        {
            player.PrintToChat("Admin ile irtibata geçin");
            return;
        }
        var models = PlayerModels.Where(x => x.Value.Text.Equals(modelName)).ToList();
        if (models.Count != 1)
        {
            player.PrintToChat("Admin ile irtibata geçin");
            return;
        }
        var model = models.Select(x => x.Value).FirstOrDefault();
        if (model == null)
        {
            player.PrintToChat("Admin ile irtibata geçin");
            return;
        }
        SetModel(player, model!, data.Model, option.Text, option.Text!.EndsWith(" | [SATIN ALINDI]"));
    }

    private void SetModel(CCSPlayerController player, PlayerModel model, PlayerMarketModel playerData, string? text, bool isBoughtAlready)
    {
        if (!isBoughtAlready)
        {
            if (playerData.Credit < model.Cost
                || playerData.Credit - model.Cost < 0)
            {
                player.PrintToChat("Yeterli krediniz bulunmuyor");
                return;
            }
            playerData.Credit -= model.Cost;
            if (text.Contains(CTModeli))
            {
                playerData.ModelIdCT = AddModelIdToGivenData(playerData.ModelIdCT, model.Id);
                playerData.DefaultIdCT = model.Id;
            }
            else
            {
                playerData.ModelIdT = AddModelIdToGivenData(playerData.ModelIdT, model.Id);
                playerData.DefaultIdT = model.Id;
            }
            player.PrintToChat($"[ZMTR] {model.Text} modelini satin aldin");
        }
        else
        {
        }

        PlayerMarketModels[player.SteamID] = playerData;

        switch (GetTeam(player))
        {
            case CsTeam.Terrorist:
                if (text.Contains(TModeli))
                {
                    player.PrintToChat($"[ZMTR] {model.Text} modelini kullaniyorsun");
                    SetModelNextServerFrame(player.PlayerPawn.Value, model.PathToModel);
                }
                break;

            case CsTeam.CounterTerrorist:
                if (text.Contains(CTModeli))
                {
                    player.PrintToChat($"[ZMTR] {model.Text} modelini kullaniyorsun");
                    SetModelNextServerFrame(player.PlayerPawn.Value, model.PathToModel);
                }
                break;
        }
    }

    public static void SetModelNextServerFrame(CCSPlayerPawn playerPawn, string model)
    {
        Server.NextFrame(() =>
        {
            playerPawn.SetModel(model);
        });
    }

    private static string AddModelIdToGivenData(string? modelId, int id)
    {
        if (string.IsNullOrWhiteSpace(modelId))
        {
            modelId = id.ToString();
        }
        else
        {
            var splitted = modelId.Split(',');
            if (splitted.ToList().Contains(id.ToString()) == false)
            {
                modelId += $",{id}";
            }
        }
        return modelId;
    }

    private async Task<PlayerMarketModel> GetPlayerMarketData(ulong steamID)
    {
        if (PlayerMarketModels == null)
        {
            PlayerMarketModels = new();
        }

        if (PlayerMarketModels.ContainsKey(steamID))
        {
            return null;
        }

        var command = DbConnection.CreateCommand();
        command.CommandText = @$"
                SELECT `{nameof(PlayerMarketModel.ModelIdCT)}`,
                       `{nameof(PlayerMarketModel.ModelIdT)}`,
                       `{nameof(PlayerMarketModel.DefaultIdCT)}`,
                       `{nameof(PlayerMarketModel.DefaultIdT)}`,
                       `{nameof(PlayerMarketModel.Credit)}`
                FROM `{nameof(PlayerMarketModel)}`
                WHERE `{nameof(PlayerMarketModel.SteamId)}` = @SteamId";
        command.Parameters.AddWithValue("@SteamId", steamID);

        PlayerMarketModel data = null;

        using (var reader = await command.ExecuteReaderAsync())
        {
            if (reader.Read())
            {
                data = new PlayerMarketModel(steamID);
                data.ModelIdCT = reader.IsDBNull(0) ? null : reader.GetString(0);
                data.ModelIdT = reader.IsDBNull(1) ? null : reader.GetString(1);
                data.DefaultIdCT = reader.IsDBNull(2) ? null : reader.GetInt32(2);
                data.DefaultIdT = reader.IsDBNull(3) ? null : reader.GetInt32(3);
                data.Credit = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
            }
        }

        if (data == null)
        {
            await DbConnection.ExecuteAsync($@"
                INSERT INTO `{nameof(PlayerMarketModel)}`
                    (`{nameof(PlayerMarketModel.SteamId)}`)
                VALUES (@SteamId)",
            new
            {
                SteamId = steamID
            });
            data = new(steamID);
        }

        PlayerMarketModels.TryAdd(steamID, data);
        return data;
    }

    private async Task UpdatePlayerMarketData(ulong steamID)
    {
        var data = GetPlayerMarketModel(steamID);
        if (data.Model == null) return;

        var command = DbConnection.CreateCommand();
        command.CommandText = @$"
                INSERT OR REPLACE INTO `{nameof(PlayerMarketModel)}`
                (`{nameof(PlayerMarketModel.SteamId)}`,
                 `{nameof(PlayerMarketModel.ModelIdCT)}`,
                 `{nameof(PlayerMarketModel.ModelIdT)}`,
                 `{nameof(PlayerMarketModel.DefaultIdCT)}`,
                 `{nameof(PlayerMarketModel.DefaultIdT)}`,
                 `{nameof(PlayerMarketModel.Credit)}`)
                VALUES (@SteamId,@ModelNameCT,@ModelNameT,@defaultCT,@defaultT,@Credit)";

        command.Parameters.Add(new SqliteParameter("@SteamId", data.Model.SteamId));
        command.Parameters.Add(new SqliteParameter("@ModelNameCT", data.Model.ModelIdCT.GetDbValue()));
        command.Parameters.Add(new SqliteParameter("@ModelNameT", data.Model.ModelIdT.GetDbValue()));
        command.Parameters.Add(new SqliteParameter("@defaultCT", data.Model.DefaultIdCT.GetDbValue()));
        command.Parameters.Add(new SqliteParameter("@defaultT", data.Model.DefaultIdT.GetDbValue()));
        command.Parameters.Add(new SqliteParameter("@Credit", data.Model.Credit.GetDbValue()));

        await command.ExecuteNonQueryAsync();
    }

    private (PlayerMarketModel Model, bool ChooseRandom) GetPlayerMarketModel(ulong? steamId)
    {
        bool chooseRandom = false;
        PlayerMarketModel? data = null;

        if (steamId.HasValue == false || steamId == 0)
            return (null, true);

        if (PlayerMarketModels == null)
        {
            data = new PlayerMarketModel(steamId.Value);
            PlayerMarketModels = new()
            {
                {steamId.Value,data }
            };
            chooseRandom = true;
        }

        var res = false;
        if (PlayerMarketModels.TryGetValue(steamId.Value, out data))
        {
            res = true;
        }
        else if (res == false)
        {
            data = GetPlayerMarketData(steamId.Value).GetAwaiter().GetResult();
            if (data != null)
            {
                PlayerMarketModels[steamId.Value] = data;
            }
            else
            {
                data = new PlayerMarketModel(steamId.Value);
                PlayerMarketModels[steamId.Value] = data;
                chooseRandom = true;
            }
        }
        else
        {
            data = new PlayerMarketModel(steamId.Value);
            PlayerMarketModels[steamId.Value] = data;
            chooseRandom = true;
        }

        return (data, chooseRandom);
    }

    #endregion Market
}