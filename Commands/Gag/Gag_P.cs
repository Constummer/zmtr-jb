using CounterStrikeSharp.API;
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
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead>")]
    public void OnPGagCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : null;
        if (target == null)
        {
            return;
        }
        var targetArgument = GetTargetArgument(target);
        GetPlayers()
            .Where(x => GetTargetAction(x, target, player))
            .ToList()
            .ForEach(gagPlayer =>
            {
                AddPGagData(gagPlayer.SteamID);
                PGags.Add(gagPlayer.SteamID);
                if (targetArgument == TargetForArgument.SingleUser)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{gagPlayer.PlayerName} {CC.W}adlı oyuncuyu {CC.B}sonsuz gagladı{CC.W}.");
                }
            });
        if (targetArgument != TargetForArgument.SingleUser)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B}gagladı{CC.W}.");
        }
    }

    private void RemoveFromPGag(ulong steamID)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"Delete From `PlayerGag` WHERE `SteamId` = @SteamId;", con);

                cmd.Parameters.AddWithValue("@SteamId", steamID);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private static void GetPGagData(ulong steamID)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
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
        }
        catch (Exception e)
        {
        }
    }

    private void AddPGagData(ulong steamID)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
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
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    #endregion Gag
}