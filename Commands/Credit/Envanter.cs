using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Envanter

    [ConsoleCommand("env")]
    [ConsoleCommand("envanter")]
    [ConsoleCommand("inventory")]
    [ConsoleCommand("inv")]
    public void Envanter(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (player?.SteamID == null || player!.SteamID == 0)
        {
            return;
        }
        if (PatronuKoruActive)
        {
            player.PrintToChat($"{Prefix} {CC.Go}PATRONU KORU ETKINLIGI {CC.W}nde model değiştiremezsin");
            return;
        }
        if (PlayerMarketModels.TryGetValue(player.SteamID, out var item))
        {
            var marketMenu = new ChatMenu($" {CC.LB}Envanter {CC.W}| {CC.G}Kredin = {CC.W}<{CC.G}{item.Credit}{CC.W}>");
            marketMenu.AddMenuOption(CTOyuncuModeli, OpenSelectedModelEnv);
            marketMenu.AddMenuOption(TOyuncuModeli, OpenSelectedModelEnv);
            marketMenu.AddMenuOption("Aura Market", AuraMarketSelected);
            marketMenu.AddMenuOption("Paraşüt Market", ParachuteMarketSelected);

            MenuManager.OpenChatMenu(player, marketMenu);
        }
        else
        {
            item = new(player.SteamID);
            PlayerMarketModels[player.SteamID] = item;
            player.PrintToChat($"{Prefix} {CC.LB}Envanterinde hiç eşya yok");
        }
    }

    private void OpenSelectedModelEnv(CCSPlayerController player, ChatMenuOption option)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (player?.SteamID == null || player!.SteamID == 0)
        {
            return;
        }
        switch (option.Text)
        {
            case CTOyuncuModeli:
                GetPlayerModelsMenu(player, CsTeam.CounterTerrorist, true);
                break;

            case TOyuncuModeli:
                GetPlayerModelsMenu(player, CsTeam.Terrorist, true);
                break;

            default:
                break;
        }
    }

    #endregion Envanter
}