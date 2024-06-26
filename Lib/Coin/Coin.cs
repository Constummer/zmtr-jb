﻿using CounterStrikeSharp.API;
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
        if (LatestWCommandUser.HasValue == false) return;
        var css = GetPlayers().Where(x => x.SteamID == LatestWCommandUser).FirstOrDefault();
        if (css != null)
        {
            if (ValidateCallerPlayer(css, false))
            {
                CoinStart(css);
            }
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
        var data = _Config.Map.MapConfigDatums.TryGetValue(Server.MapName, out var conf);
        var coords = new Vector(0, 0, 0);
        if (data && data != null)
        {
            coords = new Vector(conf.CoinCoords.X, conf.CoinCoords.Y, conf.CoinCoords.Z);
        }

        Coin.Teleport(coords, new QAngle(0.0f, 0.0f, 0.0f), VEC_ZERO);
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
                    //Coin.AbsOrigin.X = vector.X;
                    //Coin.AbsOrigin.Y = vector.Y;
                    //Coin.AbsOrigin.Z = vector.Z;
                    //Coin.AbsRotation.Y = CoinAngleY;
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
        if (warden.PlayerName?.ToLower()?.Contains(pname?.ToLower()) ?? false)
        {
            Server.NextFrame(() =>
            {
                CoinRemove();
            });
        }
        else if (pname == "@me"
            && warden.PlayerName == player.PlayerName)
        {
            Server.NextFrame(() =>
          {
              CoinRemove();
          });
        }
    }
}