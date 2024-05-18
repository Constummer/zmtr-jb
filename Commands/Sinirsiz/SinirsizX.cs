using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private CounterStrikeSharp.API.Modules.Timers.Timer SinirsizXTimer = null;

    #region SinirsizX

    [ConsoleCommand("sinirsizxac")]
    [ConsoleCommand("smxac")]
    [ConsoleCommand("smx")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead> [weapon]")]
    public void SinirsizX(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgString.GetArgSkip(0);
        LogManagerCommand(player.SteamID, info.GetCommandString);
        var weapon = info.ArgString.GetArgLast();
        if (string.IsNullOrWhiteSpace(weapon))
        {
            player.PrintToChat($"{Prefix} {CC.G} Silah ismini vermeniz gerekmektedir..");
            player.PrintToChat($"{Prefix} {CC.G} Örnek = !sinirsizx @all ssg08.");
            player.PrintToChat($"{Prefix} {CC.G} Örnek = !smx @all ssg08.");
            return;
        }
        else
        {
            weapon = GiveHandler(weapon);
            if (ValidWantedWeapon(weapon) == false)
            {
                return;
            }
            SinirsizXAction(player, target, weapon);
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.Ol}{target}{CC.W} hedefine {CC.P}{weapon} {CC.W} silahıyla {CC.DB}SMX{CC.W} başlattı.");
        }
    }

    public void SinirsizXAction(CCSPlayerController? self, string? target, string? weapon)
    {
        SinirsizXTimer?.Kill();
        SinirsizXTimer = null;
        SinirsizXTimer = GiveSinirsizCustomNade(1, SinirsizXTimer, $"weapon_{weapon}", target, self);
    }

    #endregion SinirsizX
}