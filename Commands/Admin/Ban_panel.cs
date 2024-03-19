using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region RR

    /*
        @"CREATE TABLE IF NOT EXISTS `PlayerBan` (
                  `SteamId` bigint(20) DEFAULT NULL,
                  `BannedBySteamId` bigint(20) DEFAULT NULL,
                  `Time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
    */

    [ConsoleCommand("panelunban")]
    [ConsoleCommand("unbanpanel")]
    [ConsoleCommand("banunpanel")]
    [ConsoleCommand("panelbankaldir")]
    [CommandHelper(1, "<playerismi | steamid | #userid>", CommandUsage.SERVER_ONLY)]
    public void cUnBan(CCSPlayerController? player, CommandInfo info)
    {
        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : null;
        if (target == null)
        {
            return;
        }

        Bans.ToList()
             .Where(x => x.Key.ToString() == target)
             .ToList()
             .ForEach(x =>
             {
                 Bans.Remove(x.Key, out var _);
                 RemoveBanData(x.Key);
                 Server.PrintToConsole($"{Prefix} {CC.R}{x.Key} {CC.W} steam id li user bani kaldirildi!");
             });
    }

    [ConsoleCommand("panelban")]
    [CommandHelper(2, "<playerismi | steamid | #userid> <dakika/0 süresiz>", CommandUsage.SERVER_ONLY)]
    public void PanelBan(CCSPlayerController? player, CommandInfo info)
    {
        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : null;
        if (target == null)
        {
            return;
        }

        var godOneTwoStr = info.ArgCount > 2 ? info.ArgString.GetArg(1) : "0";
        if (int.TryParse(godOneTwoStr, out var value) == false)
        {
            return;
        }
        var steamidbck = ulong.MinValue;
        try
        {
            _ = ulong.TryParse(target, out steamidbck);
        }
        catch
        {
        }
        var players = GetPlayers()
            .Where(x =>
            (x.PlayerName?.ToLower()?.Contains(target?.ToLower()) ?? false)
            || GetUserIdIndex(target) == x.UserId
            || x.SteamID == steamidbck).ToList();

        if (players.Count > 1)
        {
            Server.PrintToConsole($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
            return;
        }

        var x = players.FirstOrDefault();
        if (x == null)
        {
            if (value <= 0)
            {
                if (Bans.TryGetValue(steamidbck, out var dateTime))
                {
                    Bans[steamidbck] = DateTime.UtcNow.AddYears(1);
                }
                else
                {
                    Bans.Add(steamidbck, DateTime.UtcNow.AddYears(1));
                }
                AddBanData(steamidbck, 0, DateTime.UtcNow.AddYears(1));
                Server.PrintToConsole($"{Prefix} {CC.G}{steamidbck} {CC.B}Sınırsız{CC.W} banladı.");
            }
            else
            {
                if (Bans.TryGetValue(steamidbck, out var dateTime))
                {
                    Bans[steamidbck] = DateTime.UtcNow.AddMinutes(value);
                }
                else
                {
                    Bans.Add(steamidbck, DateTime.UtcNow.AddMinutes(value));
                }
                AddBanData(steamidbck, 0, DateTime.UtcNow.AddMinutes(value));
                Server.PrintToConsole($"{Prefix} {CC.G}{steamidbck} {CC.B}{value}{CC.W} dakika boyunca banladı.");
            }
        }
        else
        {
            if (value <= 0)
            {
                if (Bans.TryGetValue(x.SteamID, out var dateTime))
                {
                    Bans[x.SteamID] = DateTime.UtcNow.AddYears(1);
                }
                else
                {
                    Bans.Add(x.SteamID, DateTime.UtcNow.AddYears(1));
                }
                AddBanData(x.SteamID, 0, DateTime.UtcNow.AddYears(1));
                Server.PrintToConsole($"{Prefix} {CC.G}KONSOL {CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.B}Sınırsız{CC.W} banladı.");
            }
            else
            {
                if (Bans.TryGetValue(x.SteamID, out var dateTime))
                {
                    Bans[x.SteamID] = DateTime.UtcNow.AddMinutes(value);
                }
                else
                {
                    Bans.Add(x.SteamID, DateTime.UtcNow.AddMinutes(value));
                }
                AddBanData(x.SteamID, 0, DateTime.UtcNow.AddMinutes(value));
                Server.PrintToConsole($"{Prefix} {CC.G}KONSOL {CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.B}{value}{CC.W} dakika boyunca banladı.");
            }
            Server.ExecuteCommand($"kickid {x.UserId}");
        }
    }

    #endregion RR
}