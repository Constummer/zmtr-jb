using CounterStrikeSharp.API;
using Discord;
using Discord.Webhook;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static ulong LatestDcNotifyId = 0;

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

    private static async Task SendDcNotifyOnWardenChange()
    {
        return;
        try
        {
            using (var client = new DiscordWebhookClient(_Config.Additional.DiscordWChangeNotifyUrl))
            {
                if (client == null)
                {
                    return;
                }
                var wardenName = "Komutçu Yok";
                var warden = GetWarden();
                if (warden != null)
                {
                    if (ValidateCallerPlayer(warden, false))
                    {
                        wardenName = warden.PlayerName;
                    }
                }
                var pcount = GetPlayerCount();
                Stopwatch w = new Stopwatch();
                w.Start();

                EmbedBuilder builder = new EmbedBuilder()
                .WithTitle("Jailbreak")
                .AddField($"Komutçu", $"```ansi\r\n\u001b[2;31m{wardenName}\u001b[0m\r\n```")
                .AddField($"🗺️ㅤMap", $"```ansi\r\n\u001b[2;31m{Server.MapName}\u001b[0m\r\n```", inline: true)
                .AddField("👥ㅤOyuncu S.", $"```ansi\r\n\u001b[2;31m{pcount}\u001b[0m/\u001b[2;32m{Server.MaxPlayers}\u001b[0m\r\n```", inline: true)
                .AddField("ㅤ", $"[**`connect jb.zmtr.org`**](https://zmtr.org/baglan/jb)ㅤ👈 Bağlan")
                .WithColor(Discord.Color.Blue)
                .WithCurrentTimestamp();

                w.Stop();
                Server.PrintToChatAll("1- " + w.Elapsed.TotalMilliseconds);
                w = new Stopwatch();
                w.Start();

                if (LatestDcNotifyId != 0)
                {
                    client.DeleteMessageAsync(LatestDcNotifyId);
                }
                LatestDcNotifyId = client.SendMessageAsync(embeds: new[] { builder.Build() }).GetAwaiter().GetResult();
                w.Stop();
                Server.PrintToChatAll("2- " + w.Elapsed.TotalMilliseconds);
            }
        }
        catch (Exception e)
        {
            Server.PrintToConsole(e.Message);
        }
    }
}