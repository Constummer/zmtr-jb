using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Ayak Gizle

    [ConsoleCommand("ayakgizle", "Ayaklarini gizler")]
    public void AyakGizle(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        player.PlayerPawn.Value.Render = Color.FromArgb(254, 254, 254, 254);
    }

    [ConsoleCommand("ayakgoster", "Ayaklarini goster")]
    public void AyakGoster(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        player.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 255, 255);
    }

    #endregion Ayak Gizle
}