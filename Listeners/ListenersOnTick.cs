using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnTick()
    {
        RegisterListener((Listeners.OnTick)(() =>
        {
            bool changed = false;
            for (int i = 1; i < Server.MaxPlayers; i++)
            {
                var ent = NativeAPI.GetEntityFromIndex(i);
                if (ent == 0)
                    continue;

                var client = new CCSPlayerController(ent);
                if (client == null || !client.IsValid)
                    continue;
                if (Countdown_enable)
                {
                    client.PrintToCenterHtml(
                    $"<font color='gray'>►</font> <font class='fontSize-m' color='red'>{Time} saniye kaldı</font> <font color='gray'>◄</font>"
                    );
                }
                changed = BasicCountdown.CountdownEnableTextHandler(changed, client);
            }
        }));
    }
}