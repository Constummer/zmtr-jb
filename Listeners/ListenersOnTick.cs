using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnTick()
    {
        RegisterListener((Listeners.OnTick)(() =>
        {
            if (CoinAngleYUpdaterActive)
            {
                CoinAngleY = (CoinAngleY + 2) % 360;
            }

            bool changed = false;
            // CoinMoveOnTick(GetWarden());
            for (int i = 1; i < Server.MaxPlayers; i++)
            {
                var ent = NativeAPI.GetEntityFromIndex(i);
                if (ent == 0)
                    continue;

                var player = new CCSPlayerController(ent);
                if (player == null || !player.IsValid)
                    continue;
                ParachuteOnTick(player);

                //   changed = BasicCountdown.CountdownEnableTextHandler(changed, player);
            }
        }));
    }
}