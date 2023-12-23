using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Ayak Gizle

    [ConsoleCommand("ayakgizle", "Ayaklarini gizler")]
    public void AyakGizle(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        AyakGizle(player);
    }

    private void AyakGizle(CCSPlayerController player, bool refreshTp = false)
    {
        HideFoots[player.SteamID] = true;
        player!.PlayerPawn.Value!.Render = Color.FromArgb(254, 254, 254, 254);
        if (refreshTp == false)
        {
            RefreshPawn(player);
            player!.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.Darkred}Ayaklarını gizledin!");
        }
        else
        {
            RefreshPawnTP(player);
            player!.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.Darkred}Ayakların otomatik olarak gizlendi. !ayakgoster ile tekrar gösterebilirsin");
        }
    }

    private void AyakGoster(CCSPlayerController player)
    {
        HideFoots[player.SteamID] = false;
        player!.PlayerPawn.Value!.Render = DefaultPlayerColor;
        RefreshPawn(player);
        player!.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}Ayakların artık gözüküyor!");
    }

    [ConsoleCommand("ayakgoster", "Ayaklarini goster")]
    public void AyakGoster(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        AyakGoster(player);
    }

    #endregion Ayak Gizle
}