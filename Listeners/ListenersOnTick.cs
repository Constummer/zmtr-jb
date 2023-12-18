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

                var player = new CCSPlayerController(ent);
                if (player == null || !player.IsValid)
                    continue;
                AllowLaserForWarden(player);

                if (Countdown_enable)
                {
                    player.PrintToCenterHtml(
                    $"<font color='gray'>►</font> <font class='fontSize-m' color='red'>{Time} saniye kaldı</font> <font color='gray'>◄</font>"
                    );
                }
                changed = BasicCountdown.CountdownEnableTextHandler(changed, player);
            }
        }));
    }

    private void AllowLaserForWarden(CCSPlayerController player)
    {
        if (LatestWCommandUser == player.SteamID)
        {
            foreach (var c in player.Pawn.Value!.MovementServices!.Buttons.ButtonStates)
            {
                if (c == FButtonIndex)
                {
                    if (ValidateCallerPlayer(player, false) == false
                        || player.PlayerPawn.Value!.AbsOrigin == null)
                    {
                        break;
                    }
                    float x, y, z;
                    x = player.PlayerPawn.Value!.AbsOrigin!.X;
                    y = player.PlayerPawn.Value!.AbsOrigin!.Y;
                    z = player.PlayerPawn.Value!.AbsOrigin!.Z;

                    LasersEntry(x, y, z);
                    break;
                }
            }
        }
    }
}