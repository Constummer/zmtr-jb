using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Serilog.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static CFuncPlat Coin = null;
    private static bool CoinSpawned { get; set; } = false;
    private static float CoinAngleY = 0.0f;
    private static bool CoinAngleYUpdaterActive = false;

    private static void CoinAfterNewCommander()
    {
        var css = GetPlayers(CsTeam.CounterTerrorist)
            .Where(x => ValidateCallerPlayer(x, false))
            .Where(x => x.SteamID == LatestWCommandUser)
            .FirstOrDefault();
        if (css != null)
        {
            CoinStart(css);
        }
    }

    private static void CoinStart(CCSPlayerController x)
    {
        if (ValidateCallerPlayer(x, false) == false)
        {
            return;
        }
        if (x.SteamID == LatestWCommandUser)
        {
            CoinRemove();
            if (GetTeam(x) != CsTeam.CounterTerrorist)
            {
                return;
            }

            Coin = Utilities.CreateEntityByName<CFuncPlat>("func_plat");

            if (Coin == null)
            {
                return;
            }
            var playerAbs = x.PlayerPawn.Value.AbsOrigin;
            var vector = new Vector(playerAbs.X, playerAbs.Y, playerAbs.Z + 100);
            Coin.Teleport(vector, new QAngle(0.0f, 0.0f, 0.0f), VEC_ZERO);
            Coin.SetModel("models/coop/challenge_coin.vmdl");
            Coin.DispatchSpawn();

            CoinSpawned = true;
            CoinAngleYUpdaterActive = true;
        }
    }

    private static void CoinMoveOnTick(CCSPlayerController player)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (player.SteamID == LatestWCommandUser)
        {
            if (CoinSpawned)
            {
                if (GetTeam(player) == CsTeam.CounterTerrorist)
                {
                    if (Coin != null && Coin.IsValid)
                    {
                        var playerAbs = player.PlayerPawn.Value.AbsOrigin;
                        var vector = new Vector(
                                playerAbs.X,
                                playerAbs.Y,
                                playerAbs.Z + 100);

                        Coin.Teleport(vector, new QAngle(0.0f, CoinAngleY, 0.0f), VEC_ZERO);
                    }
                }
            }
        }
    }

    private static void CoinRemove()
    {
        return;
        if (Coin != null)
        {
            if (Coin.IsValid)
            {
                Coin.Remove();
            }
            CoinAngleYUpdaterActive = false;
            CoinSpawned = false;
        }
    }
}