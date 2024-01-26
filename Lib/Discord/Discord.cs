using CounterStrikeSharp.API;
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
            var discordPath = Path.Combine(ContentRootPath, "Discord.json");
            Server.PrintToConsole(discordPath);
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
                MaxPlayerCount = Server.MaxPlayers,
                PlayerCount = GetPlayers().Count(),
                WardenName = wardenName
            };
            var serialized = JsonSerializer.Serialize(data,
                            new JsonSerializerOptions
                            {
                                WriteIndented = true,
                                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                            });
            File.WriteAllText(discordPath, serialized);
        }
        catch (Exception e)
        {
            Server.PrintToConsole(e.Message);
        }
    }
}