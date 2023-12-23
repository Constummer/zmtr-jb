using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

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
        _Config.ParachuteEnabled = false;
    }

    [ConsoleCommand("pa")]
    [ConsoleCommand("parasutac")]
    public void ParachuteAc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        _Config.ParachuteEnabled = true;
    }

    #endregion Parachute
}