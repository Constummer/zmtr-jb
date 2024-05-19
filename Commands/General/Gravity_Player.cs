using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, float> GravityActiveDatas = new();
    private bool GravityActive = false;

    #region Gravity

    [ConsoleCommand("gravitykapa")]
    [ConsoleCommand("gravitykapat")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
    public void OnGravityKapatCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var target = info.ArgString.GetArgSkip(0);
        var targetArgument = GetTargetArgument(target);
        GravityActive = false;
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers()
               .Where(x => x.PawnIsAlive
                        && x.Pawn.Value != null
                        && GetTargetAction(x, target, player))
               .ToList()
               .ForEach(x =>
               {
                   if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                   {
                       Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin gravitysini sıfırladı.");
                   }
                   GravityActiveDatas.Remove(x.SteamID);
                   x.PlayerPawn.Value.GravityScale = 1.0f;
                   RefreshPawn(x);
               });

        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin gravitysini sıfırladı.");
        }
    }

    [ConsoleCommand("gravity")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me> <0-1 kapatmak için, 2-9 gravity ayarlamak için>")]
    public void OnGravityCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var target = info.ArgString.GetArgSkipFromLast(1);
        if (!float.TryParse(info.ArgString.GetArgLast(), out var gravity) || gravity < 0 || gravity > 10)
        {
            player.PrintToChat($"{Prefix}{CC.W} 0-1 kapatmak için, 2-9 gravity ayarlamak için.");
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        var targetArgument = GetTargetArgument(target);
        GetPlayers()
               .Where(x => x.PawnIsAlive
                        && x.Pawn.Value != null
                        && GetTargetAction(x, target, player))
               .ToList()
               .ForEach(x =>
               {
                   switch (gravity)
                   {
                       case 0:
                       case 1:
                           if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                           {
                               Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin gravitysini sıfırladı.");
                           }
                           GravityActiveDatas.Remove(x.SteamID);
                           x.PlayerPawn.Value.GravityScale = 1.0f;
                           break;

                       default:
                           if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                           {
                               Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncunun gravitysini {CC.B}{gravity} {CC.W}olarak ayarladı.");
                           }
                           if (GravityActiveDatas.ContainsKey(x.SteamID))
                           {
                               GravityActiveDatas[x.SteamID] = gravity;
                           }
                           else
                           {
                               GravityActiveDatas.Add(x.SteamID, gravity);
                           }
                           x.PlayerPawn.Value.GravityScale = gravity;
                           break;
                   }
                   RefreshPawn(x);
               });
        switch (gravity)
        {
            case 0:
            case 1:

                GravityActive = false;
                if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin gravitysini sıfırladı.");
                }
                break;

            default:
                GravityActive = true;
                if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin gravitysini {CC.B}{gravity}{CC.W} olarak ayarladı.");
                }
                break;
        }
    }

    private void GravityActiveCheck(CCSPlayerController? x)
    {
        if (ValidateCallerPlayer(x, false) == false) return;
        if (x.PawnIsAlive == false) return;
        if (x.Health == 0) return;

        if (GravityActiveDatas.TryGetValue(x.SteamID, out var speed))
        {
            x.PlayerPawn.Value.GravityScale = speed;
        }
    }

    #endregion Gravity
}