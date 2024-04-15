using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("modelsat")]
    [ConsoleCommand("satmodel")]
    public void ModelSat(CCSPlayerController? player, CommandInfo info)
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

            ChatMenus.OpenMenu(player, marketMenu);
        }
        else
        {
            item = new(player.SteamID);
            PlayerMarketModels[player.SteamID] = item;
            player.PrintToChat($"{Prefix} {CC.LB}Envanterinde hiç eşya yok");
        }
    }
}