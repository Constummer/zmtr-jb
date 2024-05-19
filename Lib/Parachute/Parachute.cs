using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using JailbreakExtras.Lib.Database;
using JailbreakExtras.Lib.Database.Models;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, PlayerParticleData> PlayerParachuteDatas = new();
    private static Dictionary<int?, CBaseEntity?> gParaModel = new();
    private static Dictionary<ulong, bool> bUsingPara = new();

    private void ParachuteOnTick(CCSPlayerController player, int i)
    {
        if (_Config.Additional.ParachuteEnabled
                    && player.PawnIsAlive)
        {
            if (ValidateCallerPlayer(player, false) == false)
            {
                return;
            }
            if (gParaModel.ContainsKey(player.UserId) == false)
            {
                CreateParachute(player.UserId, player.SteamID);
            }
            var buttons = player.Buttons;
            if ((buttons & PlayerButtons.Use) != 0 && !player.PlayerPawn.Value!.OnGroundLastTick)
            {
                if (bUsingPara.TryGetValue(player.SteamID, out bool _))
                {
                    bUsingPara[player.SteamID] = true;
                }
                else
                {
                    bUsingPara.TryAdd(player.SteamID, true);
                }
                StartParachute(player);
            }
            else if (bUsingPara.TryGetValue(player.SteamID, out bool data) && data)
            {
                if (bUsingPara.TryGetValue(player.SteamID, out bool _))
                {
                    bUsingPara[player.SteamID] = false;
                }
                else
                {
                    bUsingPara.TryAdd(player.SteamID, false);
                }
                StopParachute(player);
            }
        }
    }

    private void CreateParachute(int? userid, ulong steamID)
    {
        var entity = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic_override");
        if (entity != null && entity.IsValid)
        {
            var model = "models/zmtr/special.vmdl";
            if (PlayerParachuteDatas.TryGetValue(steamID, out var parachuteData))
            {
                if (string.IsNullOrWhiteSpace(parachuteData?.SelectedModelId) == false &&
                    int.TryParse(parachuteData?.SelectedModelId, out var modelId) &&
                    modelId != 0)
                {
                    var d = _Config.Parachute.MarketModeller.Where(x => x.Id == modelId);
                    if (d.Any())
                    {
                        var data = d.FirstOrDefault();
                        if (data != null && string.IsNullOrWhiteSpace(data.PathToModel) == false)
                        {
                            model = data.PathToModel;
                        }
                    }
                }
            }
            entity.MoveType = MoveType_t.MOVETYPE_NOCLIP;
            entity.Collision.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_NONE;
            entity.Collision.CollisionAttribute.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_NONE;
            if (Config.Map.MapConfigDatums.TryGetValue(Server.MapName, out var conf) && conf != null && conf.ParamCoords != null)
            {
                var cords = new Vector(conf.ParamCoords.X, conf.ParamCoords.Y, conf.ParamCoords.Z);
                if (cords != null)
                    entity.Teleport(cords, ANGLE_ZERO, VEC_ZERO);
            }
            else
            {
                entity.Teleport(VEC_ZERO, ANGLE_ZERO, VEC_ZERO);
            }
            entity.DispatchSpawn();

            entity.SetModel(model);

            gParaModel[userid] = entity;
        }
    }

    private void StopParachute(CCSPlayerController player)
    {
        if (gParaModel.ContainsKey(player.UserId))
        {
            if (gParaModel[player.UserId] != null && gParaModel[player.UserId].IsValid)
            {
                if (Config.Map.MapConfigDatums.TryGetValue(Server.MapName, out var conf) && conf != null && conf.ParamCoords != null)
                {
                    var cords = new Vector(conf.ParamCoords.X, conf.ParamCoords.Y, conf.ParamCoords.Z);
                    if (cords != null)
                        gParaModel[player.UserId].Teleport(cords, ANGLE_ZERO, VEC_ZERO);
                }
                else
                {
                    gParaModel[player.UserId].Teleport(VEC_ZERO, ANGLE_ZERO, VEC_ZERO);
                }
            }
        }
        player.GravityScale = 1.0f;
    }

    private void StartParachute(CCSPlayerController player)
    {
        var fallspeed = 100 * (-1.0f);
        var velocity = player.PlayerPawn.Value.AbsVelocity;

        var position = player.PlayerPawn.Value.AbsOrigin!;
        var angle = player.PlayerPawn.Value.AbsRotation!;
        if (velocity.Z < 0.0f)
        {
            player.PlayerPawn.Value.AbsVelocity.Z = fallspeed;
            if (Config.Additional.ParachuteModelEnabled)
            {
                if (gParaModel[player.UserId] != null && gParaModel[player.UserId].IsValid)
                {
                    gParaModel[player.UserId].Teleport(position, angle, velocity);
                }
            }
        }
    }

    private void ClearParachutes()
    {
        foreach (var userId in gParaModel.Keys.ToList())
        {
            if (gParaModel[userId] != null && gParaModel[userId].IsValid)
            {
                gParaModel[userId].Remove();
                gParaModel[userId] = null;
                gParaModel.Remove(userId);
            }
        }
    }

    private static void RemoveGivenParachute(int userId)
    {
        if (gParaModel.ContainsKey(userId) && gParaModel[userId] != null && gParaModel[userId].IsValid)
        {
            _ = Global?.AddTimer(0.1f, () =>
            {
                if (gParaModel.ContainsKey(userId))
                {
                    if (gParaModel[userId] != null && gParaModel[userId].IsValid == true)
                    {
                        if (_Config.Map.MapConfigDatums.TryGetValue(Server.MapName, out var conf) && conf != null && conf.ParamCoords != null)
                        {
                            var cords = new Vector(conf.ParamCoords.X, conf.ParamCoords.Y, conf.ParamCoords.Z);
                            if (cords != null)
                                gParaModel[userId].Teleport(cords, ANGLE_ZERO, VEC_ZERO);
                        }
                        else
                        {
                            gParaModel[userId].Teleport(VEC_ZERO, ANGLE_ZERO, VEC_ZERO);
                        }
                        gParaModel[userId].Remove();
                        gParaModel[userId] = null;
                        gParaModel.Remove(userId);
                    }
                }
            }, SOM);
        };
    }

    private void ParachuteMarketSelected(CCSPlayerController player, ChatMenuOption option)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (player?.SteamID == null || player!.SteamID == 0)
        {
            return;
        }
        var data = GetPlayerMarketModel(player.SteamID);
        if (data.Model == null) return;
        PlayerParachuteDatas.TryGetValue(player.SteamID, out var parachuteData);
        var boughtModelIds = parachuteData?.GetBoughtModelIds();

        var marketMenu = new ChatMenu($" {CC.LB}Paraşüt Market {CC.W}|{CC.G} Kredin {CC.W}<{CC.G}{data.Model.Credit}{CC.W}>");
        if (player.PawnIsAlive == false)
        {
            marketMenu.AddMenuOption($"{CC.R}SEÇİM YAPABİLMEK İÇİN {CC.DR}HAYATTA {CC.R}OLMALISINIZ", null, true);
        }
        foreach (var item in Config.Parachute.MarketModeller)
        {
            var bought = boughtModelIds != null && boughtModelIds.Contains(item.Id);
            marketMenu.AddMenuOption($"{item.Text} | {item.Cost}{(bought ? " | Satın Alınmış" : "")}", (p, info) =>
            {
                if (ValidateCallerPlayer(player, false) == false) return;
                if (player?.SteamID == null || player!.SteamID == 0) return;
                if (bought == false)
                {
                    if (data.Model == null
                        || data.Model.Credit < item.Cost
                        || data.Model.Credit - item.Cost < 0)
                    {
                        player.PrintToChat($"{Prefix} {CC.W}Yetersiz Bakiye!");
                        return;
                    }
                    if (ValidateCallerPlayer(player, false) == false) return;
                    ConfirmMenu(player, data.Model.Credit, item.Text, () =>
                    {
                        if (ValidateCallerPlayer(player, false) == false) return;
                        data.Model.Credit -= item.Cost;
                        PlayerMarketModels[player.SteamID] = data.Model;

                        player.PrintToChat($"{Prefix} {CC.B}{item.Cost} {CC.W}Kredi Karşılığında {CC.B}{item.Text} {CC.W}Paraşüt Aldın!");
                        player.PrintToChat($"{Prefix} {CC.W}Mevcut Kredin = {CC.B}{data.Model.Credit}{CC.R}");
                        if (parachuteData == null)
                        {
                            parachuteData = new(player.SteamID)
                            {
                                BoughtModelIds = item.Id.ToString(),
                                SelectedModelId = item.Id.ToString()
                            };
                            PlayerParachuteDatas.Add(player.SteamID, parachuteData);
                        }
                        else
                        {
                            parachuteData.SelectedModelId = item.Id.ToString();
                            parachuteData.BoughtModelIds = AddModelIdToGivenData(parachuteData.BoughtModelIds, item.Id);
                            PlayerParachuteDatas[player.SteamID] = parachuteData;
                        }
                        //UseGivenParticle(player, item.Id);
                    });
                }
                else
                {
                    if (ValidateCallerPlayer(player, false) == false) return;
                    parachuteData.SelectedModelId = item.Id.ToString();
                    PlayerParachuteDatas[player.SteamID] = parachuteData;
                    //UseGivenParticle(player, item.Id);
                }
            }, !player.PawnIsAlive);
        }
        MenuManager.OpenChatMenu(player, marketMenu);
    }

    private static void GetPlayerParachuteData(ulong steamId)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                MySqlCommand? cmd = new MySqlCommand(@$"SELECT `BoughtModelIds`,`SelectedModelId` FROM `PlayerParachuteData` WHERE `SteamId` = @SteamId;", con);
                cmd.Parameters.AddWithValue("@SteamId", steamId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var data = new PlayerParticleData(steamId)
                        {
                            BoughtModelIds = reader.IsDBNull(0) ? "" : reader.GetString(0),
                            SelectedModelId = reader.IsDBNull(1) ? "" : reader.GetString(1),
                        };
                        if (PlayerParachuteDatas.TryGetValue(steamId, out _))
                        {
                            PlayerParachuteDatas[steamId] = data;
                        }
                        else
                        {
                            PlayerParachuteDatas.Add(steamId, data);
                        }
                        return;
                    }
                }
            }
        }
        catch (Exception e)
        {
           ConsMsg(e.Message);
        }
    }

    private static void UpdatePlayerParachuteDataOnDisconnect(ulong steamId)
    {
        try
        {
            if (PlayerParachuteDatas.TryGetValue(steamId, out var data))
            {
                using (var con = Connection())
                {
                    if (con == null)
                    {
                        return;
                    }

                    var cmd = new MySqlCommand(@$"SELECT 1 FROM `PlayerParachuteData` WHERE `SteamId` = @SteamId;", con);
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
                        cmd = new MySqlCommand(@$"UPDATE `PlayerParachuteData`
                                          SET
                                              `BoughtModelIds` = @BoughtModelIds,
                                              `SelectedModelId` = @SelectedModelId
                                          WHERE `SteamId` = @SteamId;", con);

                        cmd.Parameters.AddWithValue("@SteamId", steamId);
                        cmd.Parameters.AddWithValue("@BoughtModelIds", data.BoughtModelIds.GetDbValue());
                        cmd.Parameters.AddWithValue("@SelectedModelId", data.SelectedModelId);

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd = new MySqlCommand(@$"INSERT INTO `PlayerParachuteData`
                                      (SteamId,BoughtModelIds,SelectedModelId)
                                      VALUES (@SteamId,@BoughtModelIds,@SelectedModelId);", con);

                        cmd.Parameters.AddWithValue("@SteamId", steamId);
                        cmd.Parameters.AddWithValue("@BoughtModelIds", data.BoughtModelIds.GetDbValue());
                        cmd.Parameters.AddWithValue("@SelectedModelId", data.SelectedModelId.GetDbValue());

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception e)
        {
           ConsMsg(e.Message);
        }
    }
}