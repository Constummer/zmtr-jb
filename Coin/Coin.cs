using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static CPhysicsPropMultiplayer Coin { get; set; } = null;
    private static bool CoinSpawned { get; set; } = false;
    private static float CoinAngleY { get; set; } = 0.0f;
    private static bool CoinAngleYUpdaterActive { get; set; } = false;
    private static bool CoinGo { get; set; } = false;
    private static bool CoinGoWanted { get; set; } = false;

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
        CoinSpawn();
    }

    private static void CoinSpawn()
    {
        CoinRemove();
        //if (GetTeam(x) != CsTeam.CounterTerrorist)
        //{
        //    return;
        //}

        Coin = Utilities.CreateEntityByName<CPhysicsPropMultiplayer>("prop_physics_multiplayer");
        if (Coin == null)
        {
            return;
        }

        //var playerAbs = x.PlayerPawn.Value.AbsOrigin;
        //var vector = new Vector(playerAbs.X, playerAbs.Y, playerAbs.Z + 100);
        //var vector = VEC_ZERO;
        var vector = new Vector(-718, -765, 24);

        Coin.Teleport(vector, new QAngle(0.0f, 0.0f, 0.0f), VEC_ZERO);
        Coin.DispatchSpawn();
        Coin.SetModel("models/coop/challenge_coin.vmdl");

        CoinGoWanted = true;
        CoinSpawned = true;
        CoinAngleYUpdaterActive = true;
    }

    private static void CoinMoveOnTick(CCSPlayerController player)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (player?.SteamID == LatestWCommandUser)
        {
            if (CoinSpawned && CoinGo)
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

    private static void CoinRemove()
    {
        if (Coin != null)
        {
            if (Coin.IsValid)
            {
                Coin.Remove();
            }
            CoinGo = false;
            CoinAngleYUpdaterActive = false;
            CoinSpawned = false;
        }
    }

    private void CoinRemoveOnWardenTeamChange(CCSPlayerController? player, CommandInfo info)
    {
        if (string.IsNullOrWhiteSpace(info.ArgString))
        {
            return;
        }
        var split = info.ArgString.Split(' ');
        if (split.Length < 2)
        {
            return;
        }
        var pname = split[0];
        if (string.IsNullOrWhiteSpace(pname))
        {
            return;
        }
        var teamNo = split[1];
        if (Enum.TryParse<CsTeam>(teamNo ?? "", out var team) == false)
        {
            return;
        }
        if (LatestWCommandUser.HasValue == false)
        {
            return;
        }
        var warden = GetWarden();
        if (warden == null)
        {
            return;
        }
        if (warden.PlayerName.ToLower().Contains(pname.ToLower()))
        {
            Server.NextFrame(() =>
            {
                WardenRefreshPawn();
                LatestWCommandUser = null;
                ClearLasers();
                CoinRemove();
                WardenLeaveSound();
            });
        }
        else if (pname == "@me"
            && warden.PlayerName == player.PlayerName)
        {
            Server.NextFrame(() =>
          {
              WardenRefreshPawn();
              LatestWCommandUser = null;
              ClearLasers();
              CoinRemove();
              WardenLeaveSound();
          });
        }
    }
}