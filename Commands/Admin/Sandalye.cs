using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("sandalye")]
    public void Sandalye(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        ClearCits();

        SandalyeAction(player, info, true);
    }

    private void SandalyeAction(CCSPlayerController? player, CommandInfo info, bool @new)
    {
        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : "100";
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}oyuncu sayisi - 1 {CC.W} kadar sandalye oluşturdu.");
        if (int.TryParse(target, out var godOneTwo))
        {
            GetTSandalyePoints(player, godOneTwo, @new);
        }
        else
        {
            GetTSandalyePoints(player, 100, @new);
        }
    }

    private void GetTSandalyePoints(CCSPlayerController? player, int maxRad, bool @new)
    {
        float middleX = player.PlayerPawn.Value.AbsOrigin.X;
        float middleY = player.PlayerPawn.Value.AbsOrigin.Y;
        var players = GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive);
        float z = player.PlayerPawn.Value.AbsOrigin.Z;
        int numberOfPoints = players.Count() - 1;

        // Calculate maximum radius based on the number of points
        int maxRadius = maxRad;

        // Calculate angle between each point
        double angleIncrement = 2 * Math.PI / numberOfPoints;

        // Calculate coordinates for each point with adjusted radius
        FreezeOrUnfreezeSound();
        var path = "models/props/de_inferno/hr_i/inferno_chair/inferno_chair.vmdl";
        for (int i = 0; i < numberOfPoints; i++)
        {
            CFuncWall? cit = Utilities.CreateEntityByName<CFuncWall>("func_wall");
            if (cit == null)
            {
                return;
            }
            double angle = i * angleIncrement;
            int x = (int)(middleX + maxRadius * Math.Cos(angle));
            int y = (int)(middleY + maxRadius * Math.Sin(angle));
            // Calculate the direction vector from the node towards the middle point
            var direction = new Vector(middleX - x, middleY - y, 0);

            // Rotate the direction vector by 90 degrees (to make it 'look away' from the middle point)
            var awayFromMiddlePoint = new QAngle(-direction.Y, direction.X, 0);
            cit.Teleport(new Vector(x, y, z), ANGLE_ZERO, VEC_ZERO);
            cit.DispatchSpawn();

            cit.SetModel(path);
            Cits.Add(cit);
        }
    }
}