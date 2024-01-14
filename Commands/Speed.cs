using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, int> SpeedActiveDatas = new();
    private bool SpeedActive = false;

    #region Speed

    [ConsoleCommand("speedkapa")]
    [ConsoleCommand("speedkapat")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
    public void OnSpeedKapatCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        var targetArgument = GetTargetArgument(target);
        SpeedActive = false;
        GetPlayers()
               .Where(x => x.PawnIsAlive
                        && x.Pawn.Value != null
                        && GetTargetAction(x, target, player!.PlayerName))
               .ToList()
               .ForEach(x =>
               {
                   if (targetArgument == TargetForArgument.None)
                   {
                       Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin hızını sıfırladı.");
                   }
                   SpeedActiveDatas.Remove(x.SteamID);
                   x.PlayerPawn.Value.VelocityModifier = 1.0f;
                   RefreshPawn(x);
               });

        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin hızını sıfırladı.");
        }
    }

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
                           SpeedActiveDatas.Remove(x.SteamID);
                           x.PlayerPawn.Value.VelocityModifier = 1.0f;
                           break;

                       default:
                           if (targetArgument == TargetForArgument.None)
                           {
                               Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncunun hızını {CC.B}{speed} {CC.W}olarak ayarladı.");
                           }
                           if (SpeedActiveDatas.ContainsKey(x.SteamID))
                           {
                               SpeedActiveDatas[x.SteamID] = speed;
                           }
                           else
                           {
                               SpeedActiveDatas.Add(x.SteamID, speed);
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

                SpeedActive = false;
                if (targetArgument != TargetForArgument.None)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin hızını sıfırladı.");
                }
                break;

            default:
                SpeedActive = true;
                if (targetArgument != TargetForArgument.None)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin hızını {CC.B}{speed}{CC.W} olarak ayarladı.");
                }
                break;
        }
    }

    private void SpeedActiveCheck(CCSPlayerController? x)
    {
        if (ValidateCallerPlayer(x, false) == false) return;
        if (x.PawnIsAlive == false) return;
        if (x.Health == 0) return;

        if (SpeedActiveDatas.TryGetValue(x.SteamID, out var speed))
        {
            x.PlayerPawn.Value.VelocityModifier = speed;
        }
    }

    #endregion Speed
}