using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SinirsizMermiKapa

    [ConsoleCommand("sinirsizmermikapa")]
    [ConsoleCommand("sinirsizmermikapat")]
    [ConsoleCommand("smkapa")]
    [ConsoleCommand("smkapat")]
    public void SinirsizMermiKapa(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        UnlimitedReserverAmmoActive = false;

        SinirsizMolyTimer = GiveSinirsizCustomNade(0, SinirsizMolyTimer, "weapon_incgrenade");
        SinirsizBombaTimer = GiveSinirsizCustomNade(0, SinirsizBombaTimer, "weapon_hegrenade");
        SinirsizXTimer = GiveSinirsizCustomNade(0, SinirsizXTimer, "weapon_hegrenade");
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W} sınırsız mermiyi kapadı.");
    }

    #endregion SinirsizMermiKapa
}