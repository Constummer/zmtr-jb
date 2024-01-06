﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static List<ulong> PGags = new();

    #region Gag

    [ConsoleCommand("pgag")]
    [RequiresPermissions("@css/chat")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead>")]
    public void OnPGagCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var target = info.ArgCount > 1 ? info.GetArg(1) : null;
        if (target == null)
        {
            return;
        }
        var targetArgument = GetTargetArgument(target);
        GetPlayers()
            .Where(x => targetArgument switch
            {
                TargetForArgument.All => true,
                TargetForArgument.T => GetTeam(x) == CsTeam.Terrorist,
                TargetForArgument.Ct => GetTeam(x) == CsTeam.CounterTerrorist,
                TargetForArgument.Me => player.PlayerName == x.PlayerName,
                TargetForArgument.Alive => x.PawnIsAlive,
                TargetForArgument.Dead => x.PawnIsAlive == false,
                TargetForArgument.None => x.PlayerName?.ToLower()?.Contains(target) ?? false,
                TargetForArgument.UserIdIndex => GetUserIdIndex(target) == x.UserId,
                _ => false
            }
            && ValidateCallerPlayer(x, false))
            .ToList()
            .ForEach(gagPlayer =>
            {
                AddPGagData(gagPlayer.SteamID);
                PGags.Add(gagPlayer.SteamID);
                if (targetArgument == TargetForArgument.None)
                {
                    Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{gagPlayer.PlayerName} {CC.W}adlı oyuncuyu {CC.B}sonsuz gagladı{CC.W}.");
                }
            });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefini {CC.B}gagladı{CC.W}.");
        }
    }

    private void RemoveFromPGag(ulong steamID)
    {
        var con = Connection();
        if (con == null)
        {
            return;
        }

        try
        {
            var cmd = new MySqlCommand(@$"Delete From `PlayerGag` WHERE `SteamId` = @SteamId;", con);

            cmd.Parameters.AddWithValue("@SteamId", steamID);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void GetPGagData(ulong steamID)
    {
        var con = Connection();
        if (con == null)
        {
            return;
        }

        try
        {
            var cmd = new MySqlCommand(@$"SELECT 1 FROM `PlayerGag` WHERE `SteamId` = @SteamId;", con);
            cmd.Parameters.AddWithValue("@SteamId", steamID);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    PGags.Add(steamID);
                    return;
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void AddPGagData(ulong steamID)
    {
        var con = Connection();
        if (con == null)
        {
            return;
        }

        try
        {
            var cmd = new MySqlCommand(@$"INSERT INTO `PlayerGag`
                                          (SteamId)
                                          VALUES (@SteamId);", con);

            cmd.Parameters.AddWithValue("@SteamId", steamID);
            cmd.ExecuteNonQuery();

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    PGags.Add(steamID);
                    return;
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    #endregion Gag
}