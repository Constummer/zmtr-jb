using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static List<ulong> PGags = new();

    #region Gag

    [ConsoleCommand("pgag")]
    [RequiresPermissions("@css/chat")]
    [CommandHelper(1, "<playerismi>")]
    public void OnPGagCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var playerStr = string.Empty;
        if (info.ArgCount <= 1)
        {
            playerStr = info.GetArg(1);
        }

        var players = GetPlayers()
               .Where(x => x.PlayerName.ToLower().Contains(playerStr.ToLower()))
               .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Eşleşen oyuncu bulunamadı!");
            return;
        }
        if (players.Count != 1)
        {
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Birden fazla oyuncu bulundu.");
            return;
        }
        var gagPlayer = players.FirstOrDefault();
        if (gagPlayer == null)
        {
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.W} Oyuncu bulunamadı!");
            return;
        }
        AddPGagData(gagPlayer.SteamID);
        PGags.Add(gagPlayer.SteamID);
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