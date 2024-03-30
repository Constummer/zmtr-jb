using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("gizlikrediver")]
    [CommandHelper(2, "<oyuncu ismi,@t,@ct,@all,@me> <miktar>")]
    public void GizliKrediVer(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
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
}