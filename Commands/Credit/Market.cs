﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using JailbreakExtras.Lib.Database;
using JailbreakExtras.Lib.Database.Models;
using MySqlConnector;

//using Microsoft.Data.Sqlite;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, PlayerMarketModel> PlayerMarketModels = new();
    private static Dictionary<int, PlayerModel> PlayerModels = new();
    public static bool MarketEnvDisable { get; set; } = false;

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
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (player?.SteamID == null || player!.SteamID == 0)
        {
            return;
        }
        if (PatronuKoruActive)
        {
            player.PrintToChat($"{Prefix} {CC.Go}PATRONU KORU ETKINLIGI {CC.W}nde model değiştiremezsin");
            return;
        }
        if (TelliSeferActive)
        {
            player.PrintToChat($"{Prefix} {CC.Go}TELLI VS SEFER ETKINLIGI {CC.W}nde model değiştiremezsin");
            return;
        }
        var data = GetPlayerMarketModel(player.SteamID);
        if (data.Model == null) return;

        var marketMenu = new ChatMenu($" {CC.LB}Market {CC.W}|{CC.G} Kredin {CC.W}<{CC.G}{data.Model.Credit}{CC.W}>");
        marketMenu.AddMenuOption(CTOyuncuModeli, OpenSelectedModelMarket);
        marketMenu.AddMenuOption(TOyuncuModeli, OpenSelectedModelMarket);
        marketMenu.AddMenuOption("Aura Market", AuraMarketSelected);
        marketMenu.AddMenuOption("Paraşüt Market", ParachuteMarketSelected);
        marketMenu.AddMenuOption("TP Market", TPMarketSelected);

        MenuManager.OpenChatMenu(player, marketMenu);
    }

    private void OpenSelectedModelMarket(CCSPlayerController player, ChatMenuOption option)
    {
        if (ValidateCallerPlayer(player, false) == false)
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
        var marketMenu = new ChatMenu($" {CC.LB}{menuText} Market {CC.W}|{CC.G} Kredin {CC.W}<{CC.G}{data.Model.Credit}{CC.W}>");
        var models = PlayerModels.ToList().Where(x => x.Value.TeamNo == team).ToList();
        if (onlyBoughtOnes && models.Count == 0)
        {
            player.PrintToChat($"{Prefix}{CC.G} Satın aldığınız hiç eşya yok");
        }
        else
        {
            if (player.PawnIsAlive == false)
            {
                marketMenu.AddMenuOption($"{CC.R}SEÇİM YAPABİLMEK İÇİN {CC.DR}HAYATTA {CC.R}OLMALISINIZ", null, true);
            }
            if (MarketEnvDisable == true)
            {
                marketMenu.AddMenuOption($"{CC.R}SEÇİM YAPABİLMEK İÇİN {CC.DR}TG{CC.R}'NIN BITMESINI BEKLEMELISIN", null, true);
            }
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
                    _ => ""
                };

                var text = $"{item.Value.Text} | ({item.Value.Cost}) | {teamText}";
                if (selected)
                {
                    text += " | [SATIN ALINDI]";
                }
                marketMenu.AddMenuOption(text, (p, i) =>
                {
                    if (ValidateCallerPlayer(player, false) == false) return;
                    if (player?.SteamID == null || player!.SteamID == 0) return;

                    if (data.Model == null) return;

                    SetModel(player, item.Value, data.Model, i.Text, i.Text!.EndsWith(" | [SATIN ALINDI]"));
                }, !player.PawnIsAlive || MarketEnvDisable);
            }
        }
        MenuManager.OpenChatMenu(player, marketMenu);
    }

    private void SetModel(CCSPlayerController player, PlayerModel model, PlayerMarketModel playerData, string? text, bool isBoughtAlready)
    {
        if (text == null)
        {
            return;
        }
        if (!isBoughtAlready)
        {
            if (model.Cost == 0 && model.Id < 0)
            {
                if (text.Contains(CTModeli))
                {
                    playerData.ModelIdCT = AddModelIdToGivenData(playerData.ModelIdCT, model.Id);
                    playerData.DefaultIdCT = model.Id.ToString();
                }
                else
                {
                    playerData.ModelIdT = AddModelIdToGivenData(playerData.ModelIdT, model.Id);
                    playerData.DefaultIdT = model.Id.ToString();
                }

                FinalizeModelSet(player, model, playerData, text);
            }
            else
            {
                if (playerData.Credit < model.Cost
                    || playerData.Credit - model.Cost < 0)
                {
                    player.PrintToChat($"{Prefix}{CC.G} Yeterli krediniz bulunmuyor");
                    return;
                }
                var marketMenu = new ChatMenu($"{model.Text} Modeli satin almak istedine emin misin? | Krediniz = [{playerData.Credit}]");
                for (int i = 0; i < 2; i++)
                {
                    var testz = "Evet";
                    if (i == 1)
                    {
                        testz = "Hayır";
                    }
                    marketMenu.AddMenuOption(testz, (p, i) =>
                    {
                        if (i.Text == "Evet")
                        {
                            playerData.Credit -= model.Cost;
                            if (text.Contains(CTModeli))
                            {
                                playerData.ModelIdCT = AddModelIdToGivenData(playerData.ModelIdCT, model.Id);
                                playerData.DefaultIdCT = model.Id.ToString();
                            }
                            else
                            {
                                playerData.ModelIdT = AddModelIdToGivenData(playerData.ModelIdT, model.Id);
                                playerData.DefaultIdT = model.Id.ToString();
                            }
                            player.PrintToChat($"{Prefix}{CC.G} {model.Text} modelini satin aldin.");
                            FinalizeModelSet(player, model, playerData, text);
                        }
                        else
                        {
                            player.PrintToChat($"{Prefix}{CC.G} {model.Text} modelini satin almaktan vazgeçtin.");
                            return;
                        }
                    });
                }
                MenuManager.OpenChatMenu(player, marketMenu);
            }
        }
        else
        {
            if (text.Contains(CTModeli))
            {
                playerData.ModelIdCT = AddModelIdToGivenData(playerData.ModelIdCT, model.Id);
                playerData.DefaultIdCT = model.Id.ToString();
            }
            else
            {
                playerData.ModelIdT = AddModelIdToGivenData(playerData.ModelIdT, model.Id);
                playerData.DefaultIdT = model.Id.ToString();
            }

            FinalizeModelSet(player, model, playerData, text);
        }

        static void FinalizeModelSet(CCSPlayerController player, PlayerModel model, PlayerMarketModel playerData, string? text)
        {
            PlayerMarketModels[player.SteamID] = playerData;
            switch (GetTeam(player))
            {
                case CsTeam.Terrorist:
                    if (text.Contains(TModeli))
                    {
                        player.PrintToChat($"{Prefix}{CC.G} {model.Text} modelini kullaniyorsun");
                        if (model.Cost == 0 && model.Id < 0)
                        {
                            GiveRandomSkin(player);
                        }
                        else
                        {
                            SetModelNextServerFrame(player, model.PathToModel);
                        }
                    }
                    break;

                case CsTeam.CounterTerrorist:
                    if (text.Contains(CTModeli))
                    {
                        player.PrintToChat($"{Prefix}{CC.G} {model.Text} modelini kullaniyorsun");
                        if (model.Cost == 0 && model.Id < 0)
                        {
                            GiveRandomSkin(player);
                        }
                        else
                        {
                            SetModelNextServerFrame(player, model.PathToModel);
                        }
                    }
                    break;
            }
        }
    }

    private static void SetModelNextServerFrame(CCSPlayerController x, string? model)
    {
        if (_Config.Model.SetModelActive == false) return;
        if (_Config.Model.CustomModelActive)
        {
            model = x?.TeamNum switch
            {
                0 => null,
                1 => null,
                //t
                2 => _Config.Market.MarketModeller.Where(x => x.Id == _Config.Model.SelectedModelIdT).Select(x => x.PathToModel).FirstOrDefault(),
                //ct
                3 => _Config.Market.MarketModeller.Where(x => x.Id == _Config.Model.SelectedModelIdCT).Select(x => x.PathToModel).FirstOrDefault(),
                _ => null
            };
        }
        if (string.IsNullOrWhiteSpace(model))
        {
            return;
        }
        Server.NextFrame(() =>
        {
            if (ValidateCallerPlayer(x, false) == false) return;
            if (x.PlayerPawn is not null && x.PlayerPawn.Value is not null && x.PawnIsAlive)
            {
                x.PlayerPawn.Value.SetModel(model);
            }
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

    private static PlayerMarketModel? GetPlayerMarketData(ulong steamID)
    {
        if (PlayerMarketModels.ContainsKey(steamID))
        {
            return null;
        }

        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return null;
                }
                PlayerMarketModel data = null!;

                var cmd = new MySqlCommand(@$"
                SELECT `ModelIdCT`,
                       `ModelIdT`,
                       `DefaultIdCT`,
                       `DefaultIdT`,
                       `Credit`
                FROM `PlayerMarketModel`
                WHERE `SteamId` = @SteamId", con);
                cmd.Parameters.AddWithValue("@SteamId", steamID);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        data = new PlayerMarketModel(steamID);
                        data.ModelIdCT = reader.IsDBNull(0) ? null : reader.GetString(0);
                        data.ModelIdT = reader.IsDBNull(1) ? null : reader.GetString(1);
                        data.DefaultIdCT = reader.IsDBNull(2) ? null : reader.GetString(2);
                        data.DefaultIdT = reader.IsDBNull(3) ? null : reader.GetString(3);
                        data.Credit = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                    }
                }

                if (data == null)
                {
                    cmd = new MySqlCommand($@"
                INSERT INTO `PlayerMarketModel`
                    (`SteamId`)
                VALUES (@SteamId)", con);
                    cmd.Parameters.AddWithValue("@SteamId", steamID);
                    cmd.ExecuteNonQuery();

                    data = new(steamID);
                }

                data = RuletV2GetNotExpiredGambleData(steamID, data);
                PlayerMarketModels.TryAdd(steamID, data);

                return data;
            }
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
        }
        return null;
    }

    private static void AddPlayerMarketCredit(ulong steamID, int credit)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }

                var cmd = new MySqlCommand(@$"UPDATE `PlayerMarketModel`
                                          SET `Credit` = `Credit` + @Credit
                                          WHERE `SteamId` = @SteamId;", con);

                cmd.Parameters.AddWithValue("@SteamId", steamID);
                cmd.Parameters.AddWithValue("@Credit", credit);

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
        }
    }

    private static void UpdatePlayerMarketData(ulong steamID)
    {
        var data = GetPlayerMarketModel(steamID);
        if (data.Model == null) return;

        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }

                var cmd = new MySqlCommand(@$"SELECT 1 FROM `PlayerMarketModel` WHERE `SteamId` = @SteamId;", con);
                cmd.Parameters.AddWithValue("@SteamId", steamID);
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
                    cmd = new MySqlCommand(@$"UPDATE `PlayerMarketModel`
                                          SET
                                              `ModelIdCT` = @ModelIdCT,
                                              `ModelIdT` = @ModelIdT,
                                              `DefaultIdCT` = @DefaultIdCT,
                                              `DefaultIdT` = @DefaultIdT,
                                              `Credit` = @Credit
                                          WHERE `SteamId` = @SteamId;", con);

                    cmd.Parameters.AddWithValue("@SteamId", data.Model.SteamId);
                    cmd.Parameters.AddWithValue("@ModelIdCT", data.Model.ModelIdCT.GetDbValue());
                    cmd.Parameters.AddWithValue("@ModelIdT", data.Model.ModelIdT.GetDbValue());
                    cmd.Parameters.AddWithValue("@DefaultIdCT", data.Model.DefaultIdCT.GetDbValue());
                    cmd.Parameters.AddWithValue("@DefaultIdT", data.Model.DefaultIdT.GetDbValue());
                    cmd.Parameters.AddWithValue("@Credit", data.Model.Credit.GetDbValue());

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd = new MySqlCommand(@$"INSERT INTO `PlayerMarketModel`
                                          (`SteamId`, `ModelIdCT`, `ModelIdT`, `DefaultIdCT`, `DefaultIdT`, `Credit`)
                                          Values
                                          (@SteamId,@ModelIdCT,@ModelIdT,@DefaultIdCT,@DefaultIdT,@Credit);", con);

                    cmd.Parameters.AddWithValue("@SteamId", data.Model.SteamId);
                    cmd.Parameters.AddWithValue("@ModelIdCT", data.Model.ModelIdCT.GetDbValue());
                    cmd.Parameters.AddWithValue("@ModelIdT", data.Model.ModelIdT.GetDbValue());
                    cmd.Parameters.AddWithValue("@DefaultIdCT", data.Model.DefaultIdCT.GetDbValue());
                    cmd.Parameters.AddWithValue("@DefaultIdT", data.Model.DefaultIdT.GetDbValue());
                    cmd.Parameters.AddWithValue("@Credit", data.Model.Credit.GetDbValue());

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
        }
    }

    private static (PlayerMarketModel Model, bool ChooseRandom) GetPlayerMarketModel(ulong? steamId)
    {
        PlayerMarketModel? data = null;

        if (steamId.HasValue == false || steamId == 0)
            return (null!, true);

        if (PlayerMarketModels.TryGetValue(steamId.Value, out data) == false)
        {
            data = GetPlayerMarketData(steamId.Value);
            if (data != null)
            {
                PlayerMarketModels[steamId.Value] = data;
            }
            else
            {
                data = new PlayerMarketModel(steamId.Value);
                PlayerMarketModels[steamId.Value] = data;
            }
        }

        if (data == null)
        {
            data = new PlayerMarketModel(steamId.Value);
            PlayerMarketModels[steamId.Value] = data;
        }

        var cttrue = false;
        if (string.IsNullOrWhiteSpace(data.DefaultIdCT) == false)
        {
            if (int.TryParse(data.DefaultIdCT, out var parsed))
            {
                if (parsed < 0)
                {
                    cttrue = true;
                }
            }
        }
        var ttrue = false;
        if (string.IsNullOrWhiteSpace(data.DefaultIdT) == false)
        {
            if (int.TryParse(data.DefaultIdT, out var parsed))
            {
                if (parsed < 0)
                {
                    ttrue = true;
                }
            }
        }

        if (cttrue || ttrue)
        {
            return (data, true);
        }
        return (data, false);
    }

    private void UpdateAllModels()
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmdText = "";
                var i = 0;
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                foreach (var player in GetPlayers())
                {
                    if (player?.SteamID != null && player!.SteamID != 0)
                    {
                        var data = GetPlayerMarketModel(player.SteamID);
                        if (data.Model == null) continue;

                        cmdText += $@"INSERT INTO `PlayerMarketModel` (`SteamId`, `ModelIdCT`, `ModelIdT`, `DefaultIdCT`, `DefaultIdT`, `Credit`)
                                      VALUES (@SteamId_{i}, @ModelIdCT_{i}, @ModelIdT_{i}, @DefaultIdCT_{i}, @DefaultIdT_{i}, @Credit_{i})
                                      ON DUPLICATE KEY UPDATE
                                          `ModelIdCT`= IFNULL(VALUES(`ModelIdCT`), `ModelIdCT`),
                                          `ModelIdT`= IFNULL(VALUES(`ModelIdT`), `ModelIdT`),
                                          `DefaultIdCT`= IFNULL(VALUES(`DefaultIdCT`), `DefaultIdCT`),
                                          `DefaultIdT`= IFNULL(VALUES(`DefaultIdT`), `DefaultIdT`),
                                          `Credit`= IFNULL(VALUES(`Credit`), `Credit`);
                                      ";

                        parameters.Add(new MySqlParameter($"@SteamId_{i}", player.SteamID));
                        parameters.Add(new MySqlParameter($"@ModelIdCT_{i}", data.Model?.ModelIdCT?.GetDbValue()));
                        parameters.Add(new MySqlParameter($"@ModelIdT_{i}", data.Model?.ModelIdT?.GetDbValue()));
                        parameters.Add(new MySqlParameter($"@DefaultIdCT_{i}", data.Model?.DefaultIdCT?.GetDbValue()));
                        parameters.Add(new MySqlParameter($"@DefaultIdT_{i}", data.Model?.DefaultIdT?.GetDbValue()));
                        parameters.Add(new MySqlParameter($"@Credit_{i}", data.Model?.Credit.GetDbValue()));

                        i++;
                    }
                }
                if (string.IsNullOrWhiteSpace(cmdText))
                {
                    return;
                }

                var cmd = new MySqlCommand(cmdText, con);
                cmd.Parameters.AddRange(parameters.ToArray());
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
            ConsMsg(e.Message);
        }
    }

    #endregion Market
}