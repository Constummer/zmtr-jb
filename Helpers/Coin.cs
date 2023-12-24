using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static CFuncWall Coin = null;
    private static QAngle CoinAngle = new QAngle(0.0f, 0.0f, 0.0f);
    private static bool CoinSpawned { get; set; } = false;

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

                Coin = Utilities.CreateEntityByName<CFuncWall>("func_wall");

                if (Coin == null)
                {
                    return;
                }
                var vector = new Vector(playerAbs.X, playerAbs.Y, playerAbs.Z + 100);
                Coin.Teleport(vector, CoinAngle, VEC_ZERO);
                CoinAngle = new QAngle(CoinAngle.X, CoinAngle.Y, CoinAngle.Z);
                Coin.DispatchSpawn();
                CoinSpawned = true;
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
                return;
            }
            if (CoinSpawned)
            {
                if (Coin != null)
                {
                    if (Coin.IsValid)
                    {
                        var playerAbs = player.PlayerPawn.Value.AbsOrigin;
                        var vector = new Vector(
                                playerAbs.X,
                                playerAbs.Y,
                                playerAbs.Z + 100);
                        CoinAngle = new QAngle(CoinAngle.X, (CoinAngle.Y + 5) % 360.0f, CoinAngle.Z);
                        Coin.Teleport(vector, CoinAngle, VEC_ZERO);
                    }
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
            CoinSpawned = false;
        }
    }
}