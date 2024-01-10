using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Commands.Targeting;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Noclip

    [ConsoleCommand("noclip")]
    [CommandHelper(0, "<oyuncu ismi,@t,@ct,@all,@me> <0/1>")]
    public void Noclip(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        if (info.ArgCount < 2)
        {
            if (player.PlayerPawn.Value.MoveType == MoveType_t.MOVETYPE_NOCLIP)
            {
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}, {CC.W}kendi {CC.B}noclip'ini {CC.W}kaldirdi.");
                player.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_WALK;
            }
            else
            {
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}, {CC.W}kendine {CC.B}noclip {CC.W}verdi.");
                player.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_NOCLIP;
            }
            RefreshPawn(player);
        }
        else
        {
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
                                   Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}noclip'ini {CC.W}kaldirdi.");
                               }
                               x.PlayerPawn.Value!.MoveType = MoveType_t.MOVETYPE_WALK;

                               break;

                           case 1:
                               if (targetArgument == TargetForArgument.None)
                               {
                                   Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}noclip {CC.W}verdi.");
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
                        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}, {CC.G}{target} {CC.W}hedefine {CC.B}noclip'ini {CC.W}kaldirdi.");
                        break;

                    case 1:
                        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}, {CC.G}{target} {CC.W}hedefine {CC.B}noclip {CC.W}verdi.");
                        break;
                }
            }
        }
    }

    #endregion Noclip
}