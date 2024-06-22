using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region RR

    [ConsoleCommand("time")]
    [ConsoleCommand("thetime")]
    public void TheTime(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;

        var now = DateTime.UtcNow.AddHours(3);

        player.PrintToChat($"{Prefix} {CC.W}{now.ToString("yyyy-MM-dd HH:mm:ss")}");
    }

    #endregion RR
}