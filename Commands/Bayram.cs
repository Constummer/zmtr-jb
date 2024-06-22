using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    /* @"CREATE TABLE IF NOT EXISTS `BayramCredit` (
                           `Id` bigint(20) PRIMARY KEY AUTO_INCREMENT,
                           `SteamId` bigint(20) DEFAULT NULL,
                           `RecieveTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                         ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;"
    */

    [ConsoleCommand("Bayram")]
    public void Bayram(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;

        LogManagerCommand(player.SteamID, info.GetCommandString);
        CheckAndGiveBayramCredit(player);
    }

    private void CheckAndGiveBayramCredit(CCSPlayerController? player)
    {
        var now = DateTime.UtcNow.AddHours(3).Date;
        if (now < Config.Additional.BayramCreditStart
            || now > Config.Additional.BayramCreditEnd)
        {
            player.PrintToChat($"{Prefix} {CC.W} Bayramda olmadığımız için malesef kredi alamazsın.");
            return;
        }
        else if (CanGetBayramCredit(player.SteamID))
        {
            if (PlayerMarketModels.TryGetValue(player.SteamID, out var item))
            {
                item.Credit += Config.Additional.BayramCredit;
            }
            else
            {
                item = new(player.SteamID);
                item.Credit = Config.Additional.BayramCredit;
            }
            PlayerMarketModels[player.SteamID] = item;
            Server.PrintToChatAll($"{Prefix} {CC.Ol} {player.PlayerName}{CC.W} adlı oyuncu {CC.B}{Config.Additional.BayramCredit}{CC.W} bayram şekeri aldı.");
        }
        else
        {
            player.PrintToChat($"{Prefix} {CC.W} Daha önce bayram kredini aldığın için tekrar alamazsın");
        }
    }

    private bool CanGetBayramCredit(ulong steamId)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return false;
                }

                var cmd = new MySqlCommand(@$"SELECT `RecieveTime` FROM `BayramCredit` WHERE `SteamId` = @SteamId;", con);
                cmd.Parameters.AddWithValue("@SteamId", steamId);
                var exist = new List<DateTime>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var data = reader.IsDBNull(0) ? DateTime.MinValue : reader.GetDateTime(0);
                        if (data != DateTime.MinValue)
                        {
                            exist.Add(data);
                        }
                    }
                }

                if (exist.Count > 0)
                {
                    if (exist.Any(date => date >= Config.Additional.BayramCreditStart && date <= Config.Additional.BayramCreditEnd))
                    {
                        cmd = new MySqlCommand(@$"INSERT INTO `BayramCredit`
                                                  (SteamId)
                                                  VALUES (@SteamId);", con);

                        cmd.Parameters.AddWithValue("@SteamId", steamId);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    cmd = new MySqlCommand(@$"INSERT INTO `BayramCredit`
                                                  (SteamId)
                                                  VALUES (@SteamId);", con);

                    cmd.Parameters.AddWithValue("@SteamId", steamId);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
            return false;
        }
    }
}