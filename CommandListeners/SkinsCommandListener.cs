using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void SkinsCommandListener()
    {
        AddCommandListener("skins", (player, info) =>
        {
            player.PrintToChat($"{Prefix} {CC.W} L�TFEN BU MEN�Y� KULLANMAK YER�NE {CC.R}skin.zmtr.org {CC.W} S�TES�NDEN SK�N SE��N�Z.");
            player.PrintToChat($"{Prefix} {CC.W} L�TFEN BU MEN�Y� KULLANMAK YER�NE {CC.R}skin.zmtr.org {CC.W} S�TES�NDEN SK�N SE��N�Z.");
            player.PrintToChat($"{Prefix} {CC.W} L�TFEN BU MEN�Y� KULLANMAK YER�NE {CC.R}skin.zmtr.org {CC.W} S�TES�NDEN SK�N SE��N�Z.");
            return HookResult.Stop;
        });
    }
}