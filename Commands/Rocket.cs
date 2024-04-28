using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Hook

    [ConsoleCommand("rocket")]
    [ConsoleCommand("roket")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
    public void OnRocketCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.ArgString.GetArg(0);
        LogManagerCommand(player.SteamID, info.GetCommandString);
        var targetArgument = GetTargetArgument(target);
        var snd = "particles/kolka/5_particle.vpcf";//rocket
        var rocketParticles = new List<CParticleSystem>();
        GetPlayers()
                    .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player))
                    .ToList()
                    .ForEach(x =>
                    {
                        if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                        {
                            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu{CC.B} roketledi{CC.W}.");
                        }
                        var particle = Utilities.CreateEntityByName<CParticleSystem>("info_particle_system");
                        if (particle != null && particle.IsValid)
                        {
                            particle.EffectName = snd;
                            particle.TintCP = 1;
                            particle.Teleport(player.PlayerPawn.Value.AbsOrigin, ANGLE_ZERO, VEC_ZERO);
                            particle.DispatchSpawn();
                            particle.AcceptInput("Start");
                            RoundEndParticles.Add(particle);
                            rocketParticles.Add(particle);
                            CustomSetParent(particle, player.PlayerPawn.Value);
                        }
                        Rocket(x);
                    });

        _ = AddTimer(1f, () =>
        {
            GetPlayers()
                   .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player))
                   .ToList()
                   .ForEach(x =>
                   {
                       x.CommitSuicide(false, true);
                   });
            foreach (var item in rocketParticles)
            {
                if (item != null && item.IsValid)
                {
                    item.Remove();
                }
            }
        }, SOM);
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B}roketledi");
        }
    }

    private void Rocket(CCSPlayerController player)
    {
        if (ValidateCallerPlayer(player, false) == false
            || player.PlayerPawn.Value!.AbsOrigin == null)
        {
            return;
        }
        float x, y, z;
        x = player.PlayerPawn.Value!.AbsOrigin!.X;
        y = player.PlayerPawn.Value!.AbsOrigin!.Y;
        z = player.PlayerPawn.Value!.AbsOrigin!.Z;
        var start = new Vector((float)x, (float)y, (float)z);
        var end = new Vector((float)x, (float)y, (float)z + 650f);

        Vector? playerPosition = player.PlayerPawn?.Value?.CBodyComponent?.SceneNode?.AbsOrigin ?? VEC_ZERO;
        QAngle? viewAngles = player?.PlayerPawn?.Value?.EyeAngles ?? ANGLE_ZERO;

        PullPlayerToUp(player, end, playerPosition, viewAngles);

        return;
    }

    private void PullPlayerToUp(CCSPlayerController player, Vector grappleTarget, Vector playerPosition, QAngle viewAngles)
    {
        if (player == null || player.PlayerPawn == null || player.PlayerPawn.Value.CBodyComponent == null || playerPosition == null || !player.IsValid || !player.PawnIsAlive)
        {
            Console.WriteLine("Player is null.");
            return;
        }

        if (player.PlayerPawn.Value.CBodyComponent.SceneNode == null)
        {
            Console.WriteLine("SceneNode is null. Skipping pull.");
            return;
        }

        if (grappleTarget == null)
        {
            Console.WriteLine("Grapple target is null.");
            return;
        }

        var direction = grappleTarget - playerPosition;
        float grappleSpeed = Config.Additional.GrappleSpeed;

        var newVelocity = new Vector(
            direction.X * grappleSpeed,
            direction.Y * grappleSpeed,
            direction.Z * grappleSpeed
        );

        if (player.PlayerPawn.Value.AbsVelocity != null)
        {
            player.PlayerPawn.Value.AbsVelocity.X = newVelocity.X;
            player.PlayerPawn.Value.AbsVelocity.Y = newVelocity.Y;
            player.PlayerPawn.Value.AbsVelocity.Z = newVelocity.Z;
        }
        if (player.Pawn.Value.AbsVelocity != null)
        {
            player.Pawn.Value.AbsVelocity.X = newVelocity.X;
            player.Pawn.Value.AbsVelocity.Y = newVelocity.Y;
            player.Pawn.Value.AbsVelocity.Z = newVelocity.Z;
        }

        if (player.AbsVelocity != null)
        {
            player.AbsVelocity.X = newVelocity.X;
            player.AbsVelocity.Y = newVelocity.Y;
            player.AbsVelocity.Z = newVelocity.Z;
        }

        //if (playerGrapples[player.Slot].GrappleWire != null)
        //{
        //    playerGrapples[player.Slot].GrappleWire.Teleport(playerPosition, new QAngle(0, 0, 0), new Vector(0, 0, 0));
        //}
        //else
        //{
        //    Console.WriteLine("GrappleWire is null.");
        //}
    }

    #endregion Hook
}