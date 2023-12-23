using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Hook

    [ConsoleCommand("hook", "af")]
    public void Hook(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false && LatestWCommandUser != player.SteamID)
        {
            if (HookPlayers.TryGetValue(player.SteamID, out bool canUse) == false)
            {
                return;
            }
        }
        AllowLaserForWarden(player);
    }

    [ConsoleCommand("hookver")]
    [CommandHelper(1, "<oyuncu ismi>")]
    public void HookVer(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        GetPlayers()
              .Where(x => GetTargetAction(x, target, null))
              .ToList()
              .ForEach(x =>
              {
                  x.PrintToChat($" {ChatColors.LightRed}[ZMTR] Konsolunuza `bind x hook` hazarak hook kullanmaya baslayabilirsiniz!");

                  HookPlayers[x.SteamID] = true;
              });
    }

    [ConsoleCommand("hookal")]
    [CommandHelper(1, "<oyuncu ismi>")]
    public void HookAl(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        GetPlayers()
              .Where(x => GetTargetAction(x, target, null))
              .ToList()
              .ForEach(x =>
              {
                  x.PrintToChat($" {ChatColors.LightRed}[ZMTR] Hookunuz alindi!");
                  HookPlayers.Remove(x.SteamID, out _);
              });
    }

    #endregion Hook
}