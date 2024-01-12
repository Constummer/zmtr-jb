using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private readonly Dictionary<int?, CBaseEntity?> gParaModel = new();
    private readonly Vector PARA_Vector = new Vector(4497, 4261, -1880);

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
                CreateParachute(player);
            }
            var buttons = player.Buttons;
            if ((buttons & PlayerButtons.Use) != 0 && !player.PlayerPawn.Value!.OnGroundLastTick)
            {
                if (bUsingPara.TryGetValue(player, out bool _))
                {
                    bUsingPara[player] = true;
                }
                else
                {
                    bUsingPara.TryAdd(player, true);
                }
                StartParachute(player);
            }
            else if (bUsingPara.TryGetValue(player, out bool data) && data)
            {
                if (bUsingPara.TryGetValue(player, out bool _))
                {
                    bUsingPara[player] = false;
                }
                else
                {
                    bUsingPara.TryAdd(player, false);
                }
                StopParachute(player);
            }
        }
    }

    private void CreateParachute(CCSPlayerController player)
    {
        var entity = Utilities.CreateEntityByName<CBaseProp>("prop_dynamic_override");
        if (entity != null && entity.IsValid)
        {
            entity.SetModel("models/props_survival/parachute/chute.vmdl");
            entity.MoveType = MoveType_t.MOVETYPE_NOCLIP;
            entity.Collision.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_NONE;
            entity.Collision.CollisionAttribute.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_NONE;
            entity.Teleport(PARA_Vector, ANGLE_ZERO, VEC_ZERO);
            entity.DispatchSpawn();

            gParaModel[player.UserId] = entity;
        }
    }

    private void StopParachute(CCSPlayerController player)
    {
        if (gParaModel.ContainsKey(player.UserId))
        {
            if (gParaModel[player.UserId] != null && gParaModel[player.UserId].IsValid)
            {
                gParaModel[player.UserId]
                .Teleport(PARA_Vector, ANGLE_ZERO, VEC_ZERO);
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

    private void RemoveGivenParachute(int userId)
    {
        if (gParaModel.ContainsKey(userId) && gParaModel[userId] != null && gParaModel[userId].IsValid)
        {
            _ = AddTimer(0.1f, () =>
               {
                   if (gParaModel.ContainsKey(userId))
                   {
                       if (gParaModel[userId] != null && gParaModel[userId].IsValid == true)
                       {
                           gParaModel[userId].Teleport(PARA_Vector, ANGLE_ZERO, VEC_ZERO);
                           gParaModel[userId].Remove();
                           gParaModel[userId] = null;
                           gParaModel.Remove(userId);
                       }
                   }
               });
        };
    }
}