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
               if (ActiveGodMode.ContainsKey(x.SteamID))
               {
                   ActiveGodMode[x.SteamID] = false;
               }
               else
               {
                   ActiveGodMode.TryAdd(x.SteamID, false);
               }
               x.PlayerPawn.Value!.CommitSuicide(false, true);
               x!.SwitchTeam(CsTeam.Terrorist);
           });
        Server.PrintToChatAll($"{Prefix} {CC.W}Gardiyanlar, Mahkûmlar takımına atıldı.");
    }

    #endregion CT Kov
}