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
        if (ValidateCallerPlayer(player, true, false) == false && LatestWCommandUser != player.SteamID)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
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
               x!.ChangeTeam(CsTeam.Terrorist);
           });
        Server.PrintToChatAll($"{Prefix} {CC.W}{CT_PluralCamel}, {T_PluralCamel} takımına atıldı.");
    }

    #endregion CT Kov
}