using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Parachute

    [ConsoleCommand("pk")]
    [ConsoleCommand("parasutkapa")]
    [ConsoleCommand("parasutkapat")]
    public void ParachuteKapa(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        ClearParachutes();
        _Config.Additional.ParachuteEnabled = false;
        _Config.Additional.ParachuteModelEnabled = false;
        Server.PrintToChatAll($"{Prefix} {CC.W}Paraşüt kapandı.");
    }

    [ConsoleCommand("pa")]
    [ConsoleCommand("parasutac")]
    public void ParachuteAc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        _Config.Additional.ParachuteEnabled = true;
        _Config.Additional.ParachuteModelEnabled = true;

        Server.PrintToChatAll($"{Prefix} {CC.W}Paraşüt açıldı.");
    }

    #endregion Parachute
}