using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ParachuteOnTick(CCSPlayerController player)
    {
        if (Config.ParachuteEnabled
                    && player.IsValid
                    && !player.IsBot
                    && player.PawnIsAlive)
        {
            if (ValidateCallerPlayer(player, false) == false)
            {
                return;
            }
            var buttons = player.Buttons;
            if ((buttons & PlayerButtons.Use) != 0 && !player.PlayerPawn.Value!.OnGroundLastTick)
            {
                bUsingPara[player] = true;
                StartParachute(player);
            }
            else if (bUsingPara[player])
            {
                bUsingPara[player] = false;
                StopParachute(player);
            }
        }
    }

    private void StopParachute(CCSPlayerController player)
    {
        player.GravityScale = 1.0f;
    }

    private void StartParachute(CCSPlayerController player)
    {
        var fallspeed = Config.ParachuteFallSpeed * (-1.0f);
        var isFallSpeed = false;
        var velocity = player.PlayerPawn.Value.AbsVelocity;
        if (velocity.Z >= fallspeed)
        {
            isFallSpeed = true;
        }

        if (velocity.Z < 0.0f)
        {
            if (isFallSpeed && Config.ParachuteLinear || Config.ParachuteDecreaseVec == 0.0)
            {
                velocity.Z = fallspeed;
            }
            else
            {
                velocity.Z = velocity.Z + Config.ParachuteDecreaseVec;
            }

            var position = player.PlayerPawn.Value.AbsOrigin!;
            var angle = player.PlayerPawn.Value.AbsRotation!;
            player.Teleport(position, angle, velocity);
            player.GravityScale = 0.1f;
        }
    }
}