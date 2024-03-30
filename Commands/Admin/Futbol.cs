using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static SelfAdjustingQueue<CPhysicsPropMultiplayer> FootballEntities { get; set; } = new SelfAdjustingQueue<CPhysicsPropMultiplayer>(maxSize: 5);

    [ConsoleCommand("futbol")]
    public void Futbol(CCSPlayerController? player, CommandInfo info)
    {
        if (player.SteamID != LatestWCommandUser)
        {
            if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
            {
                player.PrintToChat(NotEnoughPermission);
                return;
            }
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var entity = Utilities.CreateEntityByName<CPhysicsPropMultiplayer>("prop_physics_multiplayer");
        var removeEntity = FootballEntities.Enqueue(entity);
        try
        {
            if (removeEntity != null)
            {
                if (removeEntity.IsValid)
                {
                    removeEntity.Remove();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        if (entity == null || !entity.IsValid)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        entity.Teleport(
            new Vector(
                player.PlayerPawn.Value!.AbsOrigin!.X,
                player.PlayerPawn.Value!.AbsOrigin!.Y,
                player.PlayerPawn.Value!.AbsOrigin!.Z + 100
            ),
            player.PlayerPawn.Value!.EyeAngles,
            player.PlayerPawn.Value!.AbsVelocity
        );
        entity.MoveCollide = MoveCollide_t.MOVECOLLIDE_FLY_BOUNCE;

        entity.SetModel("models/props/de_dust/hr_dust/dust_soccerball/dust_soccer_ball001.vmdl");
        entity.DispatchSpawn();
    }

    private void ClearFootbalEntities()
    {
        var datas = FootballEntities.ToList();
        foreach (var data in datas)
        {
            try
            {
                if (data != null)
                {
                    if (data.IsValid)
                    {
                        data.Remove();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}