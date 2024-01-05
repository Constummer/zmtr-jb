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
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount != 3) return;
        var target = info.GetArg(1);
        if (!int.TryParse(info.GetArg(2), out var miktar))
        {
            player!.PrintToChat($" {CC.LR}[ZMTR]{CC.G} Miktar duzgun deil!");
            return;
        }
        GetPlayers()
               .Where(x => GetTargetAction(x, target, player!.PlayerName))
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
                       Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.R}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.LB}{miktar} {CC.W}TP verdi!");
                   }
               });
    }

    #endregion SeviyeVer
}