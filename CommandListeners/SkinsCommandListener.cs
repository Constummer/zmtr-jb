using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void SkinsCommandListener()
    {
        AddCommandListener("skins", (player, info) =>
        {
            player.PrintToChat($"{Prefix} {CC.W} LÜTFEN BU MENÜYÜ KULLANMAK YERÝNE {CC.R}skin.zmtr.org {CC.W} SÝTESÝNDEN SKÝN SEÇÝNÝZ.");
            player.PrintToChat($"{Prefix} {CC.W} LÜTFEN BU MENÜYÜ KULLANMAK YERÝNE {CC.R}skin.zmtr.org {CC.W} SÝTESÝNDEN SKÝN SEÇÝNÝZ.");
            player.PrintToChat($"{Prefix} {CC.W} LÜTFEN BU MENÜYÜ KULLANMAK YERÝNE {CC.R}skin.zmtr.org {CC.W} SÝTESÝNDEN SKÝN SEÇÝNÝZ.");
            return HookResult.Stop;
        });
    }
}