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
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        var targetArgument = GetTargetArgument(target);

        GetPlayers()
                   .Where(x => x.PawnIsAlive == false && GetTargetAction(x, target, player.PlayerName))
                   .ToList()
                   .ForEach(x =>
                   {
                       if (targetArgument == TargetForArgument.None)
                       {
                           Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu{CC.B} roketledi{CC.W}.");
                       }
                       Rocket(x);
                       _ = AddTimer(2f, () => { x.CommitSuicide(true, true); });
                   });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefini {CC.B}roketledi");
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
        var end = new Vector((float)x, (float)y, (float)z + 1000f);

        Vector playerPosition = player.PlayerPawn?.Value.CBodyComponent?.SceneNode?.AbsOrigin;
        QAngle viewAngles = player.PlayerPawn.Value.EyeAngles;

        if (IsPlayerCloseToTarget(player, end, player.PlayerPawn.Value!.AbsOrigin, 40))
        {
            return;
        }
        PullPlayer(player, end, playerPosition, viewAngles);

        return;
    }

    #endregion Hook
}