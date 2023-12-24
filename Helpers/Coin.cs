using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static CFuncPlat Coin = null;
    private static QAngle coinAngle = new QAngle(0.0f, 0.0f, 0.0f);

    private static void CoinAfterNewCommander()
    {
        var css = GetPlayers().Where(x => x.SteamID == LatestWCommandUser).FirstOrDefault();
        if (css != null)
        {
            CoinStart(css);
        }
    }

    private static void CoinStart(CCSPlayerController x)
    {
        if (x.SteamID == LatestWCommandUser)
        {
            if (ValidateCallerPlayer(x, false) == true)
            {
                var playerAbs = x.PlayerPawn.Value.AbsOrigin;

                CoinRemove();

                Coin = Utilities.CreateEntityByName<CFuncPlat>("func_plat");
                if (Coin == null)
                {
                    return;
                }
                var vector = new Vector(playerAbs.X, playerAbs.Y, playerAbs.Z + 100);
                Coin.Teleport(vector, coinAngle, VEC_ZERO);
                coinAngle = new QAngle(coinAngle.X, coinAngle.Y, coinAngle.Z);
                Coin.DispatchSpawn();
                Coin.SetModel("models/coop/challenge_coin.vmdl");
            }
        }
    }

    private static void CoinMoveOnTick(CCSPlayerController player)
    {
        if (player.SteamID == LatestWCommandUser)
        {
            if (ValidateCallerPlayer(player, false) == false)
            {
                CoinRemove();
                return;
            }
            if (Coin != null)
            {
                if (Coin.IsValid)
                {
                    var playerAbs = player.PlayerPawn.Value.AbsOrigin;
                    var vector = new Vector(
                            playerAbs.X,
                            playerAbs.Y,
                            playerAbs.Z + 100);
                    coinAngle = new QAngle(coinAngle.X, (coinAngle.Y + 5) % 360.0f, coinAngle.Z);
                    Coin.Teleport(vector, coinAngle, VEC_ZERO);
                }
            }
        }
    }

    private static void CoinRemove()
    {
        if (Coin != null)
        {
            if (Coin.IsValid)
            {
                Coin.Remove();
            }
            Coin = null;
        }
    }
}