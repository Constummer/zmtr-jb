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
        if (!AdminManager.PlayerHasPermissions(player, Perm_Premium))
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
        if (numberOfPoints == 0)
        {
            numberOfPoints = 1;
        }
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
            double dotProduct = middleX * x + middleY * y;

            double length1 = Math.Sqrt(middleX * middleX + middleY * middleY);
            double length2 = Math.Sqrt(x * x + y * y);

            double cosTheta = dotProduct / (length1 * length2);
            double angleInRadians = Math.Acos(cosTheta);

            double angleInDegrees = angleInRadians * 180 / Math.PI;
            cit.Teleport(new Vector(x, y, z), new QAngle(0, (float)angleInDegrees, 0), VEC_ZERO);
            cit.DispatchSpawn();

            cit.SetModel(path);
            var deq = Cits.Enqueue(cit);
            if (deq != null)
            {
                if (deq.IsValid)
                {
                    deq.Remove();
                }
            }
        }
    }
}