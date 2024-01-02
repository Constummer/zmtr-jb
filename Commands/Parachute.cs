using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
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
        _Config.Additional.ParachuteEnabled = false;
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Paraşüt kapandı.");
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
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Paraşüt açıldı.");
    }

    #endregion Parachute
}