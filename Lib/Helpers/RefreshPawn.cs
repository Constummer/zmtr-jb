using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static void RefreshPawn(CCSPlayerController player)
    {
        if (player != null)
        {
            if (ValidateCallerPlayer(player, false) == false)
                return;
            var weaponServices = player.PlayerPawn.Value!.WeaponServices;
            if (weaponServices == null) return;

            player.GiveNamedItem("weapon_healthshot");
            if (weaponServices.MyWeapons != null)
            {
                foreach (var weapon in weaponServices.MyWeapons)
                {
                    if (weapon != null && weapon.IsValid && weapon.Value != null && weapon.Value!.DesignerName == "weapon_healthshot")
                    {
                        weapon.Value.Remove();
                        break;
                    }
                }
            }
        }
    }

    private static void RefreshPawnnew2(CCSPlayerController player)
    {
        if (player != null)
        {
            if (ValidateCallerPlayer(player, false) == false)
                return;
            Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_MoveType");
        }
    }

    private static void RefreshPawnTPnew2(CCSPlayerController x)
    {
        if (x != null)
        {
            if (ValidateCallerPlayer(x, false) == false)
                return;
            Utilities.SetStateChanged(x.PlayerPawn.Value, "CBaseEntity", "m_MoveType");
        }
    }

    private static void RefreshPawnTP(CCSPlayerController x)
    {
        if (x != null)
        {
            if (ValidateCallerPlayer(x, false) == false)
                return;
            Vector currentPosition = x.PlayerPawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
            Vector currentSpeed = new Vector(0, 0, 0);
            QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
            x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
        }
    }
}