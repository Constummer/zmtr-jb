using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private Dictionary<ulong, PlayerLevel> PlayerLevels = new Dictionary<ulong, PlayerLevel>();

    //++ biri sunucuya girince dbye bakilcak kaydi var mi diye, varsa playerLevels modeline eklenecek
    //++ yoksa player !seviyeal, !slotol gibi bisi yazana kadar playerLevels'e eklenmicek, yazinca database e kaydi atilcak
    //++ 60 snde bir playerlevels lere 1 xp eklencek ve en son dbye kaydedilcek
    //biri disconnect atinca son datasi kaydedilip playerLevelsten silincek
    //markete YEni secenek eklenecek ve tp alabilecek ratio configten okunacak, ornegin 10_000 kredi = 100 tp
    //60 sndebir tp vermede, hediyetp de,seviyever de configdeki datalara gore playerin xpsine uyan admin permission verecek veya alacak
    //tum komutlara seviyelere gore permission check eklenecek
    private void AddPlayerLevelData(ulong steamId)
    {
        if (PlayerLevels.ContainsKey(steamId) == false)
        {
            InsertAndGetPlayerLevelData(steamId);
        }
    }

    private void InsertAndGetPlayerLevelData(ulong steamId)
    {
        var con = Connection();
        if (con == null)
        {
            return;
        }

        try
        {
            var cmd = new MySqlCommand(@$"SELECT `SteamId`, `Xp` FROM `PlayerLevel` WHERE `SteamId` = @SteamId;", con);
            cmd.Parameters.AddWithValue("@SteamId", steamId);
            bool exist = false;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    exist = true;
                    var data = new PlayerLevel(steamId)
                    {
                        Xp = reader.IsDBNull(1) ? 0 : reader.GetInt32(1)
                    };
                    PlayerLevels.Add(steamId, data);
                }
            }
            if (exist)
            {
                return;
            }
            else
            {
                PlayerLevels.Add(steamId, new(steamId) { Xp = 0 });
                cmd = new MySqlCommand(@$"INSERT INTO `PlayerLevel`
                                          (SteamId, Xp)
                                          VALUES (@SteamId, 0);", con);

                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private async Task UpdatePlayerLevelData(PlayerLevel item)
    {
        var con = Connection();
        if (con == null)
        {
            return;
        }

        try
        {
            var cmd = new MySqlCommand(@$"UPDATE `PlayerLevel`
                                          SET `Xp` = @Xp
                                          WHERE `SteamId` = @SteamId;", con);

            cmd.Parameters.AddWithValue("@SteamId", item.SteamId);
            cmd.Parameters.AddWithValue("@Xp", item.Xp);

            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private async Task UpdatePlayerLevelData(ulong steamId, int xp)
    {
        var con = Connection();
        if (con == null)
        {
            return;
        }

        try
        {
            var cmd = new MySqlCommand(@$"UPDATE `PlayerLevel`
                                          SET `Xp` = @Xp
                                          WHERE `SteamId` = @SteamId;", con);

            cmd.Parameters.AddWithValue("@SteamId", steamId);
            cmd.Parameters.AddWithValue("@Xp", xp);

            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }
}