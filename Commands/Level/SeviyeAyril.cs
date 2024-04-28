using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SeviyeIptal

    [ConsoleCommand("slotiptal")]
    [ConsoleCommand("slotolma")]
    [ConsoleCommand("slotayril")]
    [ConsoleCommand("slotsilt")]
    [ConsoleCommand("seviyeiptal")]
    [ConsoleCommand("seviyeolma")]
    [ConsoleCommand("seviyesil")]
    [ConsoleCommand("seviyeayril")]
    public void SeviyeIptal(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        var marketMenu = new ChatMenu($"Seviye sisteminden ayrılmak istediğine emin misin?");
        marketMenu.AddMenuOption("GERİ DÖNÜŞÜ OLMAYAN BİR İŞLEM", null, true);
        marketMenu.AddMenuOption("GERİ DÖNÜŞÜ OLMAYAN BİR İŞLEM", null, true);
        for (int i = 0; i < 2; i++)
        {
            var testz = "Evet";
            if (i == 1)
            {
                testz = "Hayır";
            }
            marketMenu.AddMenuOption(testz, (p, opt) =>
            {
                if (opt.Text == "Evet")
                {
                    if (ValidateCallerPlayer(player, false) == false) return;
                    if (ValidateCallerPlayer(p, false) == false) return;
                    var marketMenu = new ChatMenu($"Seviye sisteminden ayrılmak istediğine emin misin? Son kontrol");
                    marketMenu.AddMenuOption("GERİ DÖNÜŞÜ OLMAYAN BİR İŞLEM", null, true);
                    marketMenu.AddMenuOption("Eğer tekrar girmek istersen TP bilgin sıfırlanacak!", null, true);
                    for (int j = 0; j < 2; j++)
                    {
                        var testz = "Evet";
                        if (j == 1)
                        {
                            testz = "Hayır";
                        }
                        marketMenu.AddMenuOption(testz, (p, opt2) =>
                        {
                            if (opt2.Text == "Evet")
                            {
                                if (ValidateCallerPlayer(player, false) == false) return;
                                if (ValidateCallerPlayer(p, false) == false) return;
                                RemoveFromLevelSystem(player);
                                player.PrintToChat($"{Prefix}{CC.G} Seviye sisteminden ayrıldın.");
                            }
                            else
                            {
                                player.PrintToChat($"{Prefix}{CC.G} Vazgeçtin, bir şey değişmedi.");
                                return;
                            }
                        });
                    }
                    MenuManager.OpenChatMenu(player, marketMenu);
                }
                else
                {
                    player.PrintToChat($"{Prefix}{CC.G} Vazgeçtin, bir şey değişmedi.");
                    return;
                }
            });
        }
        MenuManager.OpenChatMenu(player, marketMenu);
    }

    #endregion SeviyeIptal
}