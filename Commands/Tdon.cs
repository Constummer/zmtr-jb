using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("tdon", "Freeze a t.")]
    public void OnTDonCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        FreezeTarget("@t", "");
    }

    [ConsoleCommand("tdonboz", "Unfreeze t.")]
    public void TDonbozCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        bool randomFreeze = false;
        GetPlayers(CsTeam.Terrorist)
           .Where(x => x.PawnIsAlive)
           .ToList()
           .ForEach(x =>
           {
               randomFreeze = UnfreezeX(player, x, "@t", randomFreeze);
           });
    }
}