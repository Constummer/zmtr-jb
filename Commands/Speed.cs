using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private List<ulong> SpeedActive = new List<ulong>();

    #region Speed

    [ConsoleCommand("speed")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me> <0-1 kapatmak için, 2-9 hız ayarlamak için>")]
    public void OnSpeedCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        if (info.ArgCount != 3) return;
        var target = info.GetArg(1);
        if (!int.TryParse(info.GetArg(2), out var speed) || speed < 0 || speed > 10)
        {
            player.PrintToChat($"{Prefix}{CC.W} 0-1 kapatmak için, 2-9 hız ayarlamak için.");
            return;
        }
        var targetArgument = GetTargetArgument(target);
        GetPlayers()
               .Where(x => x.PawnIsAlive
                        && x.Pawn.Value != null
                        && GetTargetAction(x, target, player!.PlayerName))
               .ToList()
               .ForEach(x =>
               {
                   switch (speed)
                   {
                       case 0:
                       case 1:
                           if (targetArgument == TargetForArgument.None)
                           {
                               Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin hızını sıfırladı.");
                           }
                           x.PlayerPawn.Value.VelocityModifier = 1.0f;
                           break;

                       default:
                           if (targetArgument == TargetForArgument.None)
                           {
                               Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncunun hızını {CC.B}{speed} {CC.W}olarak ayarladı.");
                           }
                           x.PlayerPawn.Value.VelocityModifier = speed;
                           break;
                   }
                   RefreshPawn(x);
               });
        switch (speed)
        {
            case 0:
            case 1:

                if (targetArgument != TargetForArgument.None)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin hızını sıfırladı.");
                }
                break;

            default:
                if (targetArgument != TargetForArgument.None)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin hızını {CC.B}{speed}{CC.W} olarak ayarladı.");
                }
                break;
        }
    }

    #endregion Speed
}