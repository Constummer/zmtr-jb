using CounterStrikeSharp.API;
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
        if (ValidateCallerPlayer(player) == false && LatestWCommandUser != player.SteamID)
        {
            return;
        }
        GetPlayers(CsTeam.Terrorist)
           .Where(x => x.PawnIsAlive)
           .ToList()
           .ForEach(x =>
           {
               x.PlayerPawn.Value!.CommitSuicide(false, true);
               x!.ChangeTeam(CsTeam.CounterTerrorist);
           });
        //SlayAllAction();

        Server.PrintToChatAll($"{Prefix} {CC.W}Yaşayan Mahkûmlar, Gardiyan takımına atıldı.");
    }

    #endregion CT AL
}