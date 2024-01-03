using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region ToggleDuck

    [ConsoleCommand("toggleDuck")]
    public void ToggleDuck(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false))
        {
            player.ExecuteClientCommand("alias test \"bind k test2; +duck\";alias test2 \"bind k test; -duck\";bind k test");
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.G}K {CC.W} tuşu ile eğilip kalkabilirsin.");
        }
    }

    #endregion ToggleDuck
}