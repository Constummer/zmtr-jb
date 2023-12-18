using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region CT AL

    [ConsoleCommand("ctal", "T de yasayan herkesi ct alir")]
    public void CTal(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetPlayers(CsTeam.Terrorist)
           .Where(x => x.PawnIsAlive)
           .ToList()
           .ForEach(x =>
           {
               x.PlayerPawn.Value!.CommitSuicide(false, true);
               player!.ChangeTeam(CsTeam.CounterTerrorist);
           });
    }

    #endregion CT AL
}