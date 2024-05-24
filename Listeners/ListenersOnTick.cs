using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

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
            CoinMoveOnTick(GetWarden());
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                var ent = NativeAPI.GetEntityFromIndex(i);
                if (ent == 0)
                    continue;

                var player = new CCSPlayerController(ent);
                if (player == null || !player.IsValid)
                    continue;
                ParachuteOnTick(player, i);
                SpeedoMeterOnTick(player);
                if (SkzStartTime != null && SkzTimeDatas.Count > 0)
                {
                    var data = SkzTimeDatas.Where(x => x.SteamId == player.SteamID).FirstOrDefault();
                    if (data != null && !data.Done)
                    {
                        UpdatePlayersBasedOnTheirPos(player?.PlayerPawn?.Value?.AbsOrigin ?? VEC_ZERO, player.SteamID);
                    }
                }
                //CustomImageMenuOnTick(player);
                if (GrabOrCizPlayers.TryGetValue(player.SteamID, out var c) && c)
                {
                    CizOnTick(player);
                }
                else
                {
                    GrabOnTick(player);
                }
                changed = BasicCountdown.CountdownEnableTextHandler(changed, player);
                //try
                //{
                //    if (ThirdPersonPool.TryGetValue(player.SteamID, out var cam))
                //    {
                //        UpdateCamera(cam, player);
                //    }
                //}
                //catch (Exception e)
                //{
                //}
                try
                {
                    if (SmoothThirdPersonPool.TryGetValue(player.SteamID, out var cam2))
                    {
                        if (cam2 != null)
                        {
                            if (cam2.IsValid)
                            {
                                UpdateCameraSmooth(cam2, player);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ConsMsg(e.Message);
                }
                try
                {
                    if (SmoothThirdPersonPool2.TryGetValue(player.SteamID, out var cam2))
                    {
                        if (cam2 != null)
                        {
                            if (cam2.IsValid)
                            {
                                UpdateCameraSmooth2(cam2, player);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ConsMsg(e.Message);
                }
            }
        }));
    }
}