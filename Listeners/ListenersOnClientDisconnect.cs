using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnClientDisconnect()
    {
        RegisterListener<Listeners.OnClientDisconnect>(playerSlot =>
        {
            uint finalSlot = (uint)playerSlot + 1;
            CCSPlayerController player = new CCSPlayerController(NativeAPI.GetEntityFromIndex((int)finalSlot));
            if (player == null || player.UserId < 0)
                return;
            if (player?.SteamID != null && player!.SteamID != 0)
            {
                Logger.LogInformation("a");
                if (player == null) return;
                Logger.LogInformation("b");

                if (player.IsValid == false) return;
                Logger.LogInformation("c");

                if (player.AuthorizedSteamID == null) return;
                Logger.LogInformation("d");

                if (player.AuthorizedSteamID.SteamId64 == null) return;
                Logger.LogInformation("e");

                if (player?.AuthorizedSteamID?.SteamId64 != player?.SteamID) return;
                Logger.LogInformation("f");

                if (player.TeamNum == 0) return;
                Logger.LogInformation("g");

                if (player.UserId.HasValue == false) return;
                Logger.LogInformation("h");

                if (player.UserId == 0) return;
                Logger.LogInformation("i");

                //if (player.EverFullyConnected == false) return;
                //Logger.LogInformation("j");

                //if (player.Connected != PlayerConnectedState.PlayerConnected) return;
                //Logger.LogInformation("k");

                if (ValidateCallerPlayer(player, false) == false)
                {
                    return;
                }
                Logger.LogInformation($"BASARILI GIRMIS, CIKIS ISLEMLERI UYGULANIYOR = {player.PlayerName}");
                Logger.LogInformation($"BASARILI GIRMIS, CIKIS ISLEMLERI UYGULANIYOR = {player.PlayerName}");
                Logger.LogInformation($"BASARILI GIRMIS, CIKIS ISLEMLERI UYGULANIYOR = {player.PlayerName}");

                ClearOnDisconnect(player.SteamID, player.UserId);
                if (player?.SteamID == LatestWCommandUser)
                {
                    CoinRemove();
                }

                return;
            }
        });
    }
}