using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void GetAllKomWeeklyCreditData(MySqlConnection con)
    {
        if (con == null)
        {
            return;
        }
        var weekno = GetIso8601WeekOfYear(DateTime.UtcNow.AddHours(3));

        MySqlCommand? cmd = new MySqlCommand(@$"SELECT `SteamId`, `Recieved` FROM `KomWeeklyWCredit` where `WeekNo` = {weekno};", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                var recieved = reader.IsDBNull(1) ? false : reader.GetBoolean(1);
                if (KomWeeklyWCredits.ContainsKey((ulong)steamId) == false)
                {
                    KomWeeklyWCredits.Add((ulong)steamId, recieved);
                }
                else
                {
                    KomWeeklyWCredits[(ulong)steamId] = recieved;
                }
            }
        }

        return;
    }
}