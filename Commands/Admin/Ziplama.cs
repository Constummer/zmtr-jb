using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public Dictionary<ulong, Tuple<int, string>> JumpCount { get; set; } = new();
    public bool JumpCountActive = false;

    [ConsoleCommand("ziplama")]
    public void Ziplama(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/premium"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        JumpCount = new();
        JumpCountActive = true;
        var t = AddTimer(0.1f, () =>
        {
            PrintToCenterHtmlAll(GetFormattedPrintData("Zıplama Sayıları"));
            PrintToCenterHtmlAll(GetFormattedPrintData("Zıplama Sayıları"));
            PrintToCenterHtmlAll(GetFormattedPrintData("Zıplama Sayıları"));
            PrintToCenterHtmlAll(GetFormattedPrintData("Zıplama Sayıları"));
        }, Full);
        AddTimer(30f, () =>
        {
            t.Kill();
        }, SOM);
    }

    private string GetFormattedPrintData(string firstLine)
    {
        var str = string.Join(" <br> ",
                JumpCount
                    .ToList()
                    .OrderByDescending(x => x.Value.Item1)
                    .Select(x => $"{x.Value.Item2} - {x.Value.Item1}"));
        return $"{firstLine} <br> {str}";
    }
}