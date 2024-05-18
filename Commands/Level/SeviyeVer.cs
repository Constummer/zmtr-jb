using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SeviyeVer

    [ConsoleCommand("seviyever")]
    [CommandHelper(2, "<oyuncu ismi,@t,@ct,@all,@me> <miktar>")]
    public void SeviyeVer(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (info.ArgCount != 3) return;
        var target = info.ArgString.GetArg(0);
        if (!int.TryParse(info.ArgString.GetArg(1), out var miktar))
        {
            player!.PrintToChat($"{Prefix}{CC.G} Miktar duzgun deil!");
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        GetPlayers()
               .Where(x => GetTargetAction(x, target, player))
               .ToList()
               .ForEach(x =>
               {
                   if (ValidateCallerPlayer(x, false) && x?.SteamID != null && x!.SteamID != 0)
                   {
                       if (PlayerLevels.TryGetValue(x.SteamID, out var item))
                       {
                           item.Xp += miktar;
                       }
                       else
                       {
                           item = new(x.SteamID);
                           item.Xp = miktar;
                       }
                       PlayerLevels[x.SteamID] = item;
                       Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.LB}{miktar} {CC.W}TP verdi!");
                   }
               });
    }

    #endregion SeviyeVer
}