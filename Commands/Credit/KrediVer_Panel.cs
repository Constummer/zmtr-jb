using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("panelkrediver")]
    [CommandHelper(2, "<oyuncu ismi,@t,@ct,@all,@me,steamId> <miktar>", whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void PanelKrediVer(CCSPlayerController? player, CommandInfo info)
    {
        if (info.ArgCount != 3)
        {
            Server.PrintToConsole($"{Prefix}{CC.G} <oyuncu ismi,@t,@ct,@all,@me,steamId> <miktar>");
            return;
        };
        var target = info.ArgString.GetArg(0);
        if (!int.TryParse(info.ArgString.GetArg(1), out var miktar))
        {
            Server.PrintToConsole($"{Prefix}{CC.G} Miktar duzgun deil!");
            return;
        }
        LogManagerCommand(1, info.GetCommandString);

        var players = GetPlayers()
               .Where(x => GetTargetAction(x, target, null))
               .ToList();
        if (players.Count > 0)
        {
            players.ForEach(x =>
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
                     Server.PrintToConsole($"{x.PlayerName} isimli oyuncuya {miktar} kredi verdin MESAJ 1");
                     Server.PrintToConsole($"{x.PlayerName} isimli oyuncuya {miktar} kredi verdin MESAJ 2");
                 }
             });
        }
        else
        {
            if (ulong.TryParse(target, out var steamId) == false)
            {
                Server.PrintToConsole($"{target} hatali steamId 1");
                Server.PrintToConsole($"{target} hatali steamId 2");
                return;
            }
            Server.PrintToConsole("aaa");
            AddPlayerMarketCredit(steamId, miktar);
            Server.PrintToConsole($"{target} steamIdli oyuncuya {miktar} kredi verdin MESAJ 1");
            Server.PrintToConsole($"{target} steamIdli oyuncuya {miktar} kredi verdin MESAJ 2");
        }
    }
}