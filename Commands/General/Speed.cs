using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, float> SpeedActiveDatas = new();
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

        var target = info.ArgString.GetArgSkip(0);
        var targetArgument = GetTargetArgument(target);
        SpeedActive = false;
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
                       Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin hızını sıfırladı.");
                   }
                   SpeedActiveDatas.Remove(x.SteamID);
                   x.PlayerPawn.Value.VelocityModifier = 1.0f;
                   RefreshPawn(x);
               });

        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin hızını sıfırladı.");
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