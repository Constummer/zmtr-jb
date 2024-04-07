using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region MapDK

    public DateTime? MapStartTime { get; set; } = null;

    [ConsoleCommand("mapdk")]
    [ConsoleCommand("mapkalan")]
    [ConsoleCommand("mapdeis")]
    [ConsoleCommand("mapdegis")]
    public void MapKalanAction(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;

        var substract = (DateTime.UtcNow - MapStartTime.Value).TotalSeconds;
        TimeSpan remainingTime = TimeSpan.FromHours(8) - TimeSpan.FromSeconds(substract);
        if (remainingTime.TotalSeconds > 0)
        {
            var saat = $"{CC.B}{remainingTime.ToString("hh")}{CC.W}";
            var dk = $"{CC.B}{remainingTime.ToString("mm")}{CC.W}";
            var sn = $"{CC.B}{remainingTime.ToString("ss")}{CC.W}";
            player.PrintToChat($"{Prefix} {CC.W} mapdk'ya daha {saat} saat {dk} dakika {sn} saniye var.");
        }
        else
        {
            player.PrintToChat($"{Prefix} {CC.W} mapdk atılabilir.");
        }
    }

    private static ulong? MapDKLastVoterSteamId = null;

    [ConsoleCommand("mapdkyap")]
    [ConsoleCommand("mapdkbaslat")]
    [ConsoleCommand("mapdkbasla")]
    [ConsoleCommand("mapdkac")]
    public void MapDkBaslat(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false) return;
        var substract = (DateTime.UtcNow - MapStartTime.Value).TotalSeconds;
        TimeSpan remainingTime = TimeSpan.FromHours(8) - TimeSpan.FromSeconds(substract);
        if (remainingTime.TotalSeconds > 0)
        {
            var saat = $"{CC.B}{remainingTime.ToString("hh")}{CC.W}";
            var dk = $"{CC.B}{remainingTime.ToString("mm")}{CC.W}";
            var sn = $"{CC.B}{remainingTime.ToString("ss")}{CC.W}";
            player.PrintToChat($"{Prefix} {CC.W} mapdk'ya daha {saat} saat {dk} dakika {sn} saniye var.");
        }
        else
        {
            LogManagerCommand(player.SteamID, info.GetCommandString);

            MapDKLastVoterSteamId = player.SteamID;
            VoteAction(player, "Map Değiş Kal", 20, MapDkFinished);
        }
    }

    private void MapDkFinished()
    {
        var tempAnswers = Answers;
        var degisCount = 0;
        var kalCount = 0;
        foreach (var item in tempAnswers)
        {
            if (item.Key == "Değiş")
            {
                degisCount = item.Value;
            }
            else
            {
                kalCount = item.Value;
            }
        }

        if (degisCount > kalCount)
        {
            var player = GetPlayers().Where(x => x.SteamID == MapDKLastVoterSteamId).FirstOrDefault();
            if (player == null)
            {
                var ps = GetPlayers();
                player = ps.Skip(_random.Next(ps.Count())).FirstOrDefault();
            }
            var mapNames = Config.Map.MapWorkshopIds.Keys.ToList();
            var join = string.Join(" ", mapNames);
            VoteAction(player, $"Map {join}", 60, MapVoteFinished);
        }
        else
        {
            MapStartTime = DateTime.UtcNow;
        }
    }

    private void MapVoteFinished()
    {
        var tempAnswers = Answers;
        var highest = tempAnswers.OrderByDescending(x => x.Value).FirstOrDefault();
        if (GetPlayerCount() * 0.4 > highest.Value)
        {
            Server.PrintToChatAll($"{Prefix} MAP, SERVERIN %40'I AYNI MAPE OY VERMEDİĞİ İÇİN MAP DEĞİŞMİYOR");
            Server.PrintToChatAll($"{Prefix} MAP, SERVERIN %40'I AYNI MAPE OY VERMEDİĞİ İÇİN MAP DEĞİŞMİYOR");
            Server.PrintToChatAll($"{Prefix} MAP, SERVERIN %40'I AYNI MAPE OY VERMEDİĞİ İÇİN MAP DEĞİŞMİYOR");
            MapStartTime = DateTime.UtcNow;
            return;
        }

        Config.Map.MapWorkshopIds.TryGetValue(highest.Key.Trim(), out var mapWorkshopId);
        if (mapWorkshopId != 0)
        {
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.PrintToChatAll($"{Prefix} MAP DEĞİŞİYOR");
            Server.ExecuteCommand($"host_workshop_map {mapWorkshopId}");
        }
    }

    #endregion MapDK
}