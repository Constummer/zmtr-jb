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
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        AyakGizle(player);
    }

    [ConsoleCommand("ayakgoster", "Ayaklarini goster")]
    public void AyakGoster(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        HideFoots[player.SteamID] = false;
        player!.PlayerPawn.Value!.Render = DefaultPlayerColor;
        RefreshPawn(player);
        player!.PrintToChat($"{Prefix} {CC.G}Ayakların artık gözüküyor!");
    }

    private void AyakGizle(CCSPlayerController player, bool refreshTp = false)
    {
        HideFoots[player.SteamID] = true;
        player!.PlayerPawn.Value!.Render = Color.FromArgb(254, 254, 254, 254);
        if (refreshTp == false)
        {
            RefreshPawn(player);
            player!.PrintToChat($" {CC.G}[ZMTR] {CC.DR}Ayaklarını gizledin!");
        }
        else
        {
            RefreshPawnTP(player);
            player!.PrintToChat($" {CC.G}[ZMTR] {CC.DR}Ayakların otomatik olarak gizlendi. !ayakgoster ile tekrar gösterebilirsin");
        }
    }

    #endregion Ayak Gizle
}