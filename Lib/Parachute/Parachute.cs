using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<int?, CBaseEntity?> gParaModel = new();

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
                CreateParachute(player.UserId);
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

    private void CreateParachute(int? userid)
    {
        var entity = Utilities.CreateEntityByName<CBaseProp>("prop_dynamic_override");
        if (entity != null && entity.IsValid)
        {
            entity.SetModel("models/props_survival/parachute/chute.vmdl");
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
}