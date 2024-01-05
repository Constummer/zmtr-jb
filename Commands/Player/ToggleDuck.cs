using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region ToggleDuck

    [ConsoleCommand("toggleduck")]
    public void ToggleDuck(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false))
        {
            player.PrintToConsole($" [ZMTR] alttaki komut ile K duck toggle, eğilip kalkabilirsin");
            player.PrintToConsole($" [ZMTR] alias test \"bind k test2; +duck\";alias test2 \"bind k test; -duck\";bind k test");
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.G}Konsoluna bak.");
        }
    }

    #endregion ToggleDuck
}