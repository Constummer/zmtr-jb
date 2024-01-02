using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region RR

    [ConsoleCommand("rr")]
    [CommandHelper(0, "<saniye>")]
    public void RR(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        var target = info.ArgCount > 1 ? info.GetArg(1) : "1";
        if (int.TryParse(target, out var value))
        {
            if (value > 120)
            {
                player.PrintToChat("Max 120 sn girebilirsin");
                return;
            }
            else
            {
                Server.ExecuteCommand($"mp_restartgame {target}");
            }
        }
        else
        {
            Server.ExecuteCommand($"mp_restartgame 1");
        }
    }

    #endregion RR
}