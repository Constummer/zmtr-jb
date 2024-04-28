using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static void ConfirmMenu(CCSPlayerController player, int credit, string text, Action confirmed)
    {
        var eminMisinMenu = new ChatMenu($"{text} satýn almak istedine emin misin? | Krediniz = [{credit}]");
        for (int i = 0; i < 2; i++)
        {
            var testz = "Evet";
            if (i == 1)
            {
                testz = "Hayýr";
            }
            eminMisinMenu.AddMenuOption(testz, (p, i) =>
            {
                if (i.Text == "Evet")
                {
                    confirmed();
                }
                else
                {
                    player.PrintToChat($"{Prefix}{CC.G} {text} satýn almaktan vazgeçtin.");
                    return;
                }
            });
        }
        ChatMenus.OpenMenu(player, eminMisinMenu);
    }

    private void ConfirmMenuHtml(CCSPlayerController player, int credit, string text, Action confirmed, List<string> extraDisabledTexts = null)
    {
        var eminMisinMenu = new CenterHtmlMenu($"Satýn almak istedine emin misin?", this);
        eminMisinMenu.AddMenuOption(text, null, true);
        eminMisinMenu.AddMenuOption($"Kredin {credit}", null, true);
        for (int i = 0; i < 2; i++)
        {
            var testz = "Evet";
            if (i == 1)
            {
                testz = "Hayýr";
            }
            eminMisinMenu.AddMenuOption(testz, (p, i) =>
            {
                if (i.Text == "Evet")
                {
                    confirmed();
                }
                else
                {
                    player.PrintToChat($"{Prefix}{CC.G} {text} satýn almaktan vazgeçtin.");
                    return;
                }
            });
        }
        if (extraDisabledTexts != null && extraDisabledTexts.Count > 0)
        {
            foreach (var item in extraDisabledTexts)
            {
                eminMisinMenu.AddMenuOption(item, null, true);
            }
        }
        MenuManager.OpenCenterHtmlMenu(this, player, eminMisinMenu);
    }
}