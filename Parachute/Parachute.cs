using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ParachuteOnTick(CCSPlayerController player)
    {
        if (_Config.Additional.ParachuteEnabled
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

    private void StopParachute(CCSPlayerController player)
    {
        player.GravityScale = 1.0f;
    }

    private void StartParachute(CCSPlayerController player)
    {
        var fallspeed = 100 * (-1.0f);
        var velocity = player.PlayerPawn.Value.AbsVelocity;

        if (velocity.Z < 0.0f)
        {
            player.PlayerPawn.Value.AbsVelocity.Z = fallspeed;
        }
    }
}