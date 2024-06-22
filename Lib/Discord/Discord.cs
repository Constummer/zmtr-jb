using CounterStrikeSharp.API;
using JailbreakExtras.Lib.Database;
using MySqlConnector;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class DiscordNotifier
    {
        public string MapName { get; set; }
        public string WardenName { get; set; }
        public int PlayerCount { get; set; }
        public int MaxPlayerCount { get; set; }
    }

    private static void DiscordPost(string uri, string message)
    {
        try
        {
            var body = JsonSerializer.Serialize(new { content = message });
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage res = _httpClient.PostAsync($"{uri}", content).GetAwaiter().GetResult().EnsureSuccessStatusCode();
        }
        catch
        {
        }
    }

    private static void SendDcNotifyOnWardenChange()
    {
        try
        {
            var wardenName = "Komutçu Yok";
            var warden = GetWarden();
            if (warden != null)
            {
                if (ValidateCallerPlayer(warden, false))
                {
                    wardenName = warden.PlayerName;
                }
            }
            var data = new DiscordNotifier()
            {
                MapName = Server.MapName,
                PlayerCount = GetPlayers().Count(),
                WardenName = wardenName
            };
            try
            {
                using (var con = Connection())
                {
                    if (con == null)
                    {
                        return;
                    }

                    var cmd = new MySqlCommand(@$"Delete FROM `DcNotifyData`;

                                      INSERT INTO `DcNotifyData`
                                      (MapName,WardenName,PlayerCount)
                                      VALUES (@MapName,@WardenName,@PlayerCount);", con);

                    cmd.Parameters.AddWithValue("@MapName", data.MapName.GetDbValue());
                    cmd.Parameters.AddWithValue("@WardenName", data.WardenName.GetDbValue());
                    cmd.Parameters.AddWithValue("@PlayerCount", data.PlayerCount.GetDbValue());
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Server.PrintToConsole(e.Message);
            }
        }
        catch (Exception e)
        {
            Server.PrintToConsole(e.Message);
        }
    }
}