using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, bool> KomWeeklyWCredits = new();

    private void CheckAndGiveWeeklyKomCredit(CCSPlayerController? player)
    {
        if (KomWeeklyWCredits.TryGetValue(player.SteamID, out var recieved))
        {
            if (recieved)
            {
                player.PrintToChat($"{Prefix}{CC.W} Haftalık kredini daha önce aldığın için tekrar alamazsın.");
                return;
            }
            else
            {
                KomWeeklyWCredits[player.SteamID] = true;
            }
        }
        else
        {
            KomWeeklyWCredits.Add(player.SteamID, true);
        }

        if (PlayerMarketModels.TryGetValue(player.SteamID, out var item))
        {
            item.Credit += Config.Additional.KomWeeklyCredit;
        }
        else
        {
            item = new(player.SteamID);
            item.Credit = Config.Additional.KomWeeklyCredit;
        }
        PlayerMarketModels[player.SteamID] = item;
        InsertWeeklyKomCredit(player.SteamID);
        Server.PrintToChatAll($"{Prefix} {CC.Ol} {player.PlayerName}{CC.W} adlı komutçu haftalık {CC.B}{Config.Additional.KomWeeklyCredit}{CC.W} kredisini aldı.");
    }

    private void InsertWeeklyKomCredit(ulong steamId)
    {
        using (var con = Connection())
        {
            if (con == null)
            {
                return;
            }
            var weekno = GetIso8601WeekOfYear(DateTime.UtcNow.AddHours(3));

            var cmd = new MySqlCommand(@$"INSERT INTO `KomWeeklyWCredit`
                                          (`SteamId`,`Recieved`,`WeekNo`)
                                          VALUES
                                          (@SteamId, @Recieved, @WeekNo);", con);

            cmd.Parameters.AddWithValue("@SteamId", steamId);
            cmd.Parameters.AddWithValue("@WeekNo", weekno);
            cmd.Parameters.AddWithValue("@Recieved", true);

            cmd.ExecuteNonQuery();
        }
    }
}