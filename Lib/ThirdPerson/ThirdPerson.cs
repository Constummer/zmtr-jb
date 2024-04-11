using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static Dictionary<ulong, CPhysicsPropMultiplayer> SmoothThirdPersonPool = new();
    public static Dictionary<ulong, CPointCamera> SmoothThirdPersonPool2 = new();

    public static void SetTPColor<T>(T? prop, Color colour) where T : CBreakableProp
    {
        if (prop != null && prop.IsValid)
        {
            prop.Render = colour;
            Utilities.SetStateChanged(prop, "CBaseModelEntity", "m_clrRender");
        }
    }

    public static void UpdateCamera(CDynamicProp _cameraProp, CCSPlayerController target)
    {
        _cameraProp.Teleport(CalculatePositionInFront(target, -110, 90), target.PlayerPawn.Value!.V_angle, new Vector());
    }

    public static void UpdateCameraSmooth(CPhysicsPropMultiplayer _cameraProp, CCSPlayerController player)
    {
        if (_cameraProp != null)
        {
            if (_cameraProp.IsValid)
            {
                if (ValidateCallerPlayer(player, false) == false) return;

                Vector velocity = CalculateVelocity(_cameraProp.AbsOrigin!, CalculatePositionInFront(player, -110, 90), 0.01f);
                _cameraProp.Teleport(_cameraProp.AbsOrigin!, player.PlayerPawn.Value!.V_angle, velocity);
            }
        }
    }

    public static void UpdateCameraSmooth2(CPointCamera _cameraProp, CCSPlayerController player)
    {
        if (_cameraProp != null)
        {
            if (_cameraProp.IsValid)
            {
                if (ValidateCallerPlayer(player, false) == false) return;

                Vector velocity = CalculateVelocity(_cameraProp.AbsOrigin!, CalculatePositionInFront(player, -110, 90), 0.01f);
                _cameraProp.Teleport(_cameraProp.AbsOrigin!, player.PlayerPawn.Value!.V_angle, velocity);
            }
        }
    }

    public static Vector CalculateVelocity(Vector positionA, Vector positionB, float timeDuration)
    {
        // Step 1: Determine direction from A to B
        Vector directionVector = positionB - positionA;

        // Step 2: Calculate distance between A and B
        float distance = directionVector.Length();

        // Step 3: Choose a desired time duration for the movement
        // Ensure that timeDuration is not zero to avoid division by zero
        if (timeDuration == 0)
        {
            timeDuration = 1;
        }

        // Step 4: Calculate velocity magnitude based on distance and time
        float velocityMagnitude = distance / timeDuration;

        // Step 5: Normalize direction vector
        if (distance != 0)
        {
            directionVector /= distance;
        }

        // Step 6: Scale direction vector by velocity magnitude to get velocity vector
        Vector velocityVector = directionVector * velocityMagnitude;

        return velocityVector;
    }

    public static Vector CalculatePositionInFront(CCSPlayerController player, float offSetXY, float offSetZ = 0)
    {
        if (ValidateCallerPlayer(player, false))
        {
            var pawn = player.PlayerPawn.Value;
            // Extract yaw angle from player's rotation QAngle
            float yawAngle = pawn!.EyeAngles!.Y;

            // Convert yaw angle from degrees to radians
            float yawAngleRadians = (float)(yawAngle * Math.PI / 180.0);

            // Calculate offsets in x and y directions
            float offsetX = offSetXY * (float)Math.Cos(yawAngleRadians);
            float offsetY = offSetXY * (float)Math.Sin(yawAngleRadians);

            // Calculate position in front of the player
            var positionInFront = new Vector
            {
                X = pawn!.AbsOrigin!.X + offsetX,
                Y = pawn!.AbsOrigin!.Y + offsetY,
                Z = pawn!.AbsOrigin!.Z + offSetZ
            };

            return positionInFront;
        }
        return null;
    }

    public void SmoothThirdPerson2(CCSPlayerController p)
    {
        if (!SmoothThirdPersonPool2.ContainsKey(p.SteamID))
        {
            var cam = Utilities.CreateEntityByName<CPointCamera>("point_camera");

            if (cam != null && cam.IsValid)
            {
                cam.Active = true;
                //cam.MoveType = MoveType_t.MOVETYPE_NOCLIP;
                //cam.TakesDamage = false;
                //cam.GravityScale = 0;
                //cam.SentToClients = 1;
                cam.IsOn = true;
                cam.AspectRatio = 1;
                cam.UseScreenAspectRatio = false;
                cam.FogEnable = false;
                cam.CanHLTVUse = true;
                //player.PlayerPawn.Value.ObserverServices.ObserverTarget. = 0;
                //player.PlayerPawn.Value.ObserverServices.ObserverMode = (int)ObserverMode_t.OBS_MODE_CHASE;
                //player.PlayerPawn.Value.ObserverServices.ObserverMode = 1;
                //Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_hObserverTarget");

                cam.FOV = 90;
                cam.ZNear = 4;
                cam.ZFar = 10_000;
                p.PlayerPawn.Value!.CameraServices!.ViewEntity.Raw = cam.EntityHandle.Raw;
                Utilities.SetStateChanged(p.PlayerPawn.Value, "CBasePlayerPawn", "m_pCameraServices");

                var abs = p.PlayerPawn.Value.AbsOrigin;

                cam.Teleport(new(abs.X, abs.Y, abs.Z + 40), p.PlayerPawn.Value.EyeAngles, p.PlayerPawn.Value.AbsVelocity);
                cam.AcceptInput("SetOnAndTurnOthersOff");
                cam.AcceptInput("Enable");
                cam.DispatchSpawn();

                //CustomSetParent(cam, player.PlayerPawn.Value);
                //var cameraProp = Utilities.CreateEntityByName<CPhysicsPropMultiplayer>("prop_physics_multiplayer");

                //if (cameraProp == null || !cameraProp.IsValid) return;

                //cameraProp.DispatchSpawn();

                //cameraProp.Collision.CollisionGroup = 1;
                //cameraProp.Collision.SolidFlags = 12;
                //cameraProp.Collision.SolidType = SolidType_t.SOLID_VPHYSICS;

                //SetTPColor(cameraProp, Color.FromArgb(0, 255, 255, 255));

                //Changes players view to camera prop- ViewEntity Raw value can be set to uint.MaxValue to change back to normal player cam
                p.PlayerPawn.Value!.CameraServices!.ViewEntity.Raw = cam.EntityHandle.Raw;
                Utilities.SetStateChanged(p.PlayerPawn.Value, "CBasePlayerPawn", "m_pCameraServices");

                cam.Teleport(CalculatePositionInFront(p, -110, 90), p.PlayerPawn.Value.V_angle, new Vector());

                SmoothThirdPersonPool2.Add(p.SteamID, cam);

                p.PrintToChat($"{Prefix} {CC.W} Third person aktifleştirildi");
            }
        }
        else
        {
            p!.PlayerPawn!.Value!.CameraServices!.ViewEntity.Raw = uint.MaxValue;
            AddTimer(0.3f, () => Utilities.SetStateChanged(p.PlayerPawn!.Value!, "CBasePlayerPawn", "m_pCameraServices"));
            if (SmoothThirdPersonPool2[p.SteamID] != null && SmoothThirdPersonPool2[p.SteamID].IsValid)
            {
                SmoothThirdPersonPool2[p.SteamID].Remove();
            }
            p.PrintToChat($"{Prefix} {CC.W} Third person deaktifleştirildi");
            SmoothThirdPersonPool2.Remove(p.SteamID);
        }
    }

    public void SmoothThirdPerson(CCSPlayerController p)
    {
        if (!SmoothThirdPersonPool.ContainsKey(p.SteamID))
        {
            var cameraProp = Utilities.CreateEntityByName<CPhysicsPropMultiplayer>("prop_physics_multiplayer");

            if (cameraProp == null || !cameraProp.IsValid) return;

            cameraProp.DispatchSpawn();

            cameraProp.Collision.CollisionGroup = 1;
            cameraProp.Collision.SolidFlags = 12;
            cameraProp.Collision.SolidType = SolidType_t.SOLID_VPHYSICS;

            SetTPColor(cameraProp, Color.FromArgb(0, 255, 255, 255));

            //Changes players view to camera prop- ViewEntity Raw value can be set to uint.MaxValue to change back to normal player cam
            p.PlayerPawn.Value!.CameraServices!.ViewEntity.Raw = cameraProp.EntityHandle.Raw;
            Utilities.SetStateChanged(p.PlayerPawn.Value, "CBasePlayerPawn", "m_pCameraServices");

            cameraProp.Teleport(CalculatePositionInFront(p, -110, 90), p.PlayerPawn.Value.V_angle, new Vector());

            SmoothThirdPersonPool.Add(p.SteamID, cameraProp);

            p.PrintToChat($"{Prefix} {CC.W} Third person aktifleştirildi");
        }
        else
        {
            p!.PlayerPawn!.Value!.CameraServices!.ViewEntity.Raw = uint.MaxValue;
            AddTimer(0.3f, () => Utilities.SetStateChanged(p.PlayerPawn!.Value!, "CBasePlayerPawn", "m_pCameraServices"));
            if (SmoothThirdPersonPool[p.SteamID] != null && SmoothThirdPersonPool[p.SteamID].IsValid)
            {
                SmoothThirdPersonPool[p.SteamID].Remove();
            }
            p.PrintToChat($"{Prefix} {CC.W} Third person deaktifleştirildi");
            SmoothThirdPersonPool.Remove(p.SteamID);
        }
    }

    //public void OnTPCommand(CCSPlayerController? player)
    //{
    //    if (player == null || !player.PawnIsAlive) return;
    //    if (ValidateCallerPlayer(player, false) == false) return;

    //    if (!ThirdPersonPool.ContainsKey(player.SteamID))
    //    {
    //        CDynamicProp? _cameraProp = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");

    //        if (_cameraProp == null) return;

    //        _cameraProp.DispatchSpawn();
    //        SetTPColor(_cameraProp, Color.FromArgb(0, 255, 255, 255));

    //        _cameraProp.Teleport(CalculatePositionInFront(player, -110, 90), player.PlayerPawn.Value!.V_angle, new Vector());

    //        player.PlayerPawn!.Value!.CameraServices!.ViewEntity.Raw = _cameraProp.EntityHandle.Raw;
    //        Utilities.SetStateChanged(player.PlayerPawn!.Value!, "CBasePlayerPawn", "m_pCameraServices");

    //        player.PrintToChat($"{Prefix} {CC.W} Third person aktifleştirildi");

    //        ThirdPersonPool.Add(player.SteamID, _cameraProp);

    //        AddTimer(0.5f, () =>
    //        {
    //            _cameraProp.Teleport(CalculatePositionInFront(player, -110, 90), player.PlayerPawn.Value.V_angle, new Vector());
    //        }, SOM);
    //    }
    //    else
    //    {
    //        player!.PlayerPawn!.Value!.CameraServices!.ViewEntity.Raw = uint.MaxValue;
    //        AddTimer(0.3f, () => Utilities.SetStateChanged(player.PlayerPawn!.Value!, "CBasePlayerPawn", "m_pCameraServices"), SOM);
    //        if (ThirdPersonPool[player.SteamID] != null
    //             && ThirdPersonPool[player.SteamID].IsValid)
    //        {
    //            ThirdPersonPool[player.SteamID].Remove();
    //        }
    //        player.PrintToChat($"{Prefix} {CC.W} Third person deaktifleştirildi");
    //        ThirdPersonPool.Remove(player.SteamID);
    //    }
    //}
}