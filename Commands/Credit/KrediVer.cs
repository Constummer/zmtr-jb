using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region KrediVer

    [ConsoleCommand("krediver")]
    [CommandHelper(2, "<oyuncu ismi,@t,@ct,@all,@me> <miktar>")]
    public void KrediVer(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount != 3) return;
        var target = info.ArgString.GetArg(0);
        if (!int.TryParse(info.ArgString.GetArg(1), out var miktar))
        {
            player!.PrintToChat($"{Prefix}{CC.G} Miktar duzgun deil!");
            return;
        }
        GetPlayers()
               .Where(x => GetTargetAction(x, target, player!.PlayerName))
               .ToList()
               .ForEach(x =>
               {
                   if (ValidateCallerPlayer(x, false) && x?.SteamID != null && x!.SteamID != 0)
                   {
                       if (PlayerMarketModels.TryGetValue(x.SteamID, out var item))
                       {
                           item.Credit += miktar;
                       }
                       else
                       {
                           item = new(x.SteamID);
                           item.Credit = miktar;
                       }
                       PlayerMarketModels[x.SteamID] = item;
                       Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.LB}{miktar} {CC.W}kredi verdi!");
                   }
               });
    }

    [ConsoleCommand("gizlikrediver")]
    [CommandHelper(2, "<oyuncu ismi,@t,@ct,@all,@me> <miktar>")]
    public void GizliKrediVer(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount != 3) return;
        var target = info.ArgString.GetArg(0);
        if (!int.TryParse(info.ArgString.GetArg(1), out var miktar))
        {
            player!.PrintToChat($"{Prefix}{CC.G} Miktar duzgun deil!");
            return;
        }
        GetPlayers()
               .Where(x => GetTargetAction(x, target, player!.PlayerName))
               .ToList()
               .ForEach(x =>
               {
                   if (ValidateCallerPlayer(x, false) && x?.SteamID != null && x!.SteamID != 0)
                   {
                       if (PlayerMarketModels.TryGetValue(x.SteamID, out var item))
                       {
                           item.Credit += miktar;
                       }
                       else
                       {
                           item = new(x.SteamID);
                           item.Credit = miktar;
                       }
                       PlayerMarketModels[x.SteamID] = item;
                   }
               });
    }

    [ConsoleCommand("panelkrediver")]
    [CommandHelper(2, "<oyuncu ismi,@t,@ct,@all,@me> <miktar>", whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void PanelKrediVer(CCSPlayerController? player, CommandInfo info)
    {
        if (info.ArgCount != 3)
        {
            Server.PrintToConsole($"{Prefix}{CC.G} <oyuncu ismi,@t,@ct,@all,@me> <miktar>");
            return;
        };
        var target = info.ArgString.GetArg(0);
        if (!int.TryParse(info.ArgString.GetArg(1), out var miktar))
        {
            Server.PrintToConsole($"{Prefix}{CC.G} Miktar duzgun deil!");
            return;
        }
        GetPlayers()
               .Where(x => GetTargetAction(x, target, ""))
               .ToList()
               .ForEach(x =>
               {
                   if (ValidateCallerPlayer(x, false) && x?.SteamID != null && x!.SteamID != 0)
                   {
                       if (PlayerMarketModels.TryGetValue(x.SteamID, out var item))
                       {
                           item.Credit += miktar;
                       }
                       else
                       {
                           item = new(x.SteamID);
                           item.Credit = miktar;
                       }
                       PlayerMarketModels[x.SteamID] = item;
                   }
               });
    }

    #endregion KrediVer
}