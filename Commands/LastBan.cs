using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Admin;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static SelfAdjustingQueue<LastBanData> LastBanDatas { get; set; } = new SelfAdjustingQueue<LastBanData>(maxSize: 7);

    public static Dictionary<string, int> LastBanMinutes { get; set; } = new()
  {
      {"10",10 },
      {"60",60 },
      {"300",300 },
      {"600",600 },
      {"Sinirsiz",-1 }
  };

    [ConsoleCommand("lastban")]
    public void LastBan(CCSPlayerController? player, CommandInfo info)
    {
        if (AdminManager.PlayerHasPermissions(player, "@css/yonetim") == false)
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        if (ValidateCallerPlayer(player) == false) return;
        var datas = LastBanDatas.ToList();
        datas.Reverse();
        var LastBanMenu = new ChatMenu("LastBan Menu");
        foreach (var item in datas)
        {
            LastBanMenu.AddMenuOption(item.PlayerName, (p, i) =>
            {
                var lastBanInternalMenu = new ChatMenu("LastBan Menu");
                foreach (var ban in LastBanMinutes.ToList())
                {
                    lastBanInternalMenu.AddMenuOption(ban.Key, (p, i) =>
                    {
                        if (ban.Value <= 0)
                        {
                            if (Bans.TryGetValue(item.SteamId, out var dateTime))
                            {
                                Bans[item.SteamId] = DateTime.UtcNow.AddYears(1);
                            }
                            else
                            {
                                Bans.Add(item.SteamId, DateTime.UtcNow.AddYears(1));
                            }
                            LogManagerCommand(player.SteamID, info.GetCommandString);
                            AddBanData(item.SteamId, 0, DateTime.UtcNow.AddYears(1));
                            Server.PrintToConsole($"{Prefix} {CC.G}KONSOL {CC.W} adlı admin, {CC.G}{item.SteamId} {CC.B}Sınırsız{CC.W} banladı.");
                        }
                        else
                        {
                            if (Bans.TryGetValue(item.SteamId, out var dateTime))
                            {
                                Bans[item.SteamId] = DateTime.UtcNow.AddMinutes(ban.Value);
                            }
                            else
                            {
                                Bans.Add(item.SteamId, DateTime.UtcNow.AddMinutes(ban.Value));
                            }
                            LogManagerCommand(player.SteamID, info.GetCommandString);
                            AddBanData(item.SteamId, 0, DateTime.UtcNow.AddMinutes(ban.Value));
                            Server.PrintToConsole($"{Prefix} {CC.G}KONSOL {CC.W} adlı admin, {CC.G}{item.SteamId} {CC.B}{ban.Value}{CC.W} dakika boyunca banladı.");
                        }
                        Server.PrintToConsole($"{item.PlayerName}|{item.SteamId}|{ban.Key}|{ban.Value}");
                    });
                }
                if (ValidateCallerPlayer(player) == false) return;
                ChatMenus.OpenMenu(player, lastBanInternalMenu);
            });
        }
        if (ValidateCallerPlayer(player) == false) return;
        ChatMenus.OpenMenu(player, LastBanMenu);
    }

    public class LastBanData
    {
        public LastBanData()
        {
        }

        public LastBanData(ulong steamId, string playerName)
        {
            SteamId = steamId;
            PlayerName = playerName;
        }

        public ulong SteamId { get; set; }
        public string? PlayerName { get; set; }
    }
}