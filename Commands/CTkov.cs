using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region CT Kov

    [ConsoleCommand("ctkov")]
    public void CTkov(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false && LatestWCommandUser != player.SteamID)
        {
            return;
        }
        GetPlayers(CsTeam.CounterTerrorist)
           .Where(x => x.SteamID != LatestWCommandUser)
           .ToList()
           .ForEach(x =>
           {
               x.PlayerPawn.Value!.CommitSuicide(false, true);
               x!.ChangeTeam(CsTeam.Terrorist);
           });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Yaşayan Mahkûmlar, Gardiyan takımına atıldı.");
    }

    #endregion CT Kov
}