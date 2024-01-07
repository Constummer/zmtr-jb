using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private CounterStrikeSharp.API.Modules.Timers.Timer SinirsizXTimer = null;

    #region SinirsizX

    [ConsoleCommand("sinirsizx")]
    [ConsoleCommand("smx")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead> [weapon]")]
    public void SinirsizX(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgCount > 1 ? info.GetArg(1) : null;
        if (info.ArgCount > 2)
        {
            var weapon = info.ArgCount > 2 ? info.GetArg(2) : null;
            if (string.IsNullOrWhiteSpace(weapon))
            {
                player.PrintToChat($"{Prefix} {CC.G} Silah ismini vermeniz gerekmektedir..");
                player.PrintToChat($"{Prefix} {CC.G} Örnek = !sinirsizx @all ssg08.");
                player.PrintToChat($"{Prefix} {CC.G} Örnek = !smx @all ssg08.");
                return;
            }
            else
            {
                SinirsizXTimer?.Kill();
                SinirsizXTimer = null;
                SinirsizXTimer = GiveSinirsizCustomNade(1, SinirsizXTimer, $"weapon_{weapon}", target, player.PlayerName);
            }
        }
        else
        {
            player.PrintToChat($"{Prefix} {CC.G} Silah ismini vermeniz gerekmektedir..");
            player.PrintToChat($"{Prefix} {CC.G} Örnek = !sinirsizx @all ssg08.");
            player.PrintToChat($"{Prefix} {CC.G} Örnek = !smx @all ssg08.");
            return;
        }
    }

    #endregion SinirsizX
}