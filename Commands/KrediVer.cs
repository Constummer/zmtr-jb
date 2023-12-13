using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region KrediVer

    [ConsoleCommand("krediver")]
    [CommandHelper(2, "<oyuncu ismi,@t,@ct,@all,@me> <miktar>")]
    public void KrediVer(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 3) return;
        var target = info.GetArg(1);
        if (!int.TryParse(info.GetArg(2), out var miktar))
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] Miktar duzgun deil!");
            return;
        }
        GetPlayers()
               .Where(x => x.Pawn.Value != null
                        && GetTargetAction(x, target, player.PlayerName))
               .ToList()
               .ForEach(x =>
               {
                   if (ValidateCallerPlayer(x) && x?.SteamID != null && x!.SteamID != 0)
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
                       Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.LightBlue}{player.PlayerName}, {x.PlayerName} oyuncusuna {miktar} kredi yolladı!");
                   }
               });
    }

    #endregion KrediVer
}