using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void UpdateWeeklyKomCredit(ulong steamId)
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