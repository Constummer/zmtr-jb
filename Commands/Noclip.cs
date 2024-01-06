using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Noclip

    [ConsoleCommand("noclip")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me> <0/1>")]
    public void Noclip(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye30"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (ValidateCallerPlayer(player))
        {
            return;
        }
        if (info.ArgCount < 2) return;
        var target = info.ArgCount > 1 ? info.GetArg(1) : null;
        var godOneTwoStr = info.ArgCount > 2 ? info.GetArg(2) : null;
        int.TryParse(godOneTwoStr, out var godOneTwo);
        if (godOneTwo < 0 || godOneTwo > 1)
        {
            player.PrintToChat($"{Prefix}{CC.W} 0 = kapatmak icin, 1 = acmak icin.");
            return;
        }

        var targetArgument = GetTargetArgument(target);
        GetPlayers()
               .Where(x => x.PawnIsAlive
                        && GetTargetAction(x, target, player.PlayerName))
               .ToList()
               .ForEach(x =>
               {
                   switch (godOneTwo)
                   {
                       case 0:
                           if (targetArgument == TargetForArgument.None)
                           {
                               Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}noclibini {CC.W}kaldirdi.");
                           }
                           x.PlayerPawn.Value!.MoveType = MoveType_t.MOVETYPE_WALK;

                           break;

                       case 1:
                           if (targetArgument == TargetForArgument.None)
                           {
                               Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}noclip {CC.W}verdi.");
                           }
                           x.PlayerPawn.Value!.MoveType = MoveType_t.MOVETYPE_NOCLIP;

                           break;

                       default:

                           x.PlayerPawn.Value!.MoveType = MoveType_t.MOVETYPE_WALK;

                           break;
                   }
                   RefreshPawn(x);
               });
        if (targetArgument != TargetForArgument.None)
        {
            switch (godOneTwo)
            {
                case 0:
                    Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefine {CC.B}noclibini {CC.W}kaldirdi.");
                    break;

                case 1:
                    Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefine {CC.B}noclip {CC.W}verdi.");
                    break;
            }
        }
    }

    #endregion Noclip
}