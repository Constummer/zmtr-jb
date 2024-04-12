using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region TDaireOlustur

    [ConsoleCommand("daire2")]
    public void TDaireOlustur2(CCSPlayerController? player, CommandInfo info)
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
        LogManagerCommand(player.SteamID, info.GetCommandString);

        DaireAction(player, info, false);
    }

    [ConsoleCommand("daire")]
    public void TDaireOlustur(CCSPlayerController? player, CommandInfo info)
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
        LogManagerCommand(player.SteamID, info.GetCommandString);

        DaireAction(player, info, true);
    }

    private void DaireAction(CCSPlayerController? player, CommandInfo info, bool @new)
    {
        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : "100";
        if (int.TryParse(target, out var godOneTwo))
        {
            GetTDairePoints(player, godOneTwo, @new);
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {CC.G}@t'yi {CC.W} daire biçiminde ışınladı.");
        }
        else
        {
            GetTDairePoints(player, 100, @new);
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {CC.G}@t'yi {CC.W} daire biçiminde ışınladı.");
        }
    }

    private void GetTDairePoints(CCSPlayerController? player, int maxRad, bool @new)
    {
        float middleX = player.PlayerPawn.Value.AbsOrigin.X;
        float middleY = player.PlayerPawn.Value.AbsOrigin.Y;
        var players = GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive);
        float z = player.PlayerPawn.Value.AbsOrigin.Z;
        int numberOfPoints = players.Count();

        // Calculate maximum radius based on the number of points
        int maxRadius = maxRad;

        // Calculate angle between each point
        double angleIncrement = 2 * Math.PI / numberOfPoints;

        // Calculate coordinates for each point with adjusted radius
        int i = 0;
        FreezeOrUnfreezeSound();

        foreach (var p in players)
        {
            if (ValidateCallerPlayer(p, false) == false) continue;
            double angle = i * angleIncrement;
            int x = (int)(middleX + maxRadius * Math.Cos(angle));
            int y = (int)(middleY + maxRadius * Math.Sin(angle));

            if (@new)
            {
                SetMoveType(p, MoveType_t.MOVETYPE_NONE);
            }
            p.PlayerPawn.Value.Teleport(new Vector(x, y, z), p.PlayerPawn.Value.EyeAngles, VEC_ZERO);

            i++;
        }
    }

    #endregion TDaireOlustur
}