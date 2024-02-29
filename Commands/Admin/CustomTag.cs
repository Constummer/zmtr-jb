using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class CustomTagData
    {
        public string TagName { get; set; }
        public string TagColor { get; set; }
        public string SayColor { get; set; }
        public string TColor { get; set; }
        public string CTColor { get; set; }
    }

    public static Dictionary<ulong, CustomTagData> CustomPlayerTags { get; set; } = new();

    [ConsoleCommand("customtag")]
    [ConsoleCommand("customtagekle")]
    [CommandHelper(minArgs: 5, "<custom tag verilcek kişi> <tagColor> <tColor> <ctColor> <sayColor> <tag (boşluklu olabilir * OPSIYONEL>")]
    public void CustomTag(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        var targetPlayer = info.ArgString.GetArg(0);
        var tagColor = info.ArgString.GetArg(1);
        var tColor = info.ArgString.GetArg(2);
        var ctColor = info.ArgString.GetArg(3);
        var sayColor = info.ArgString.GetArg(4);
        var tag = info.ArgString.GetArgSkip(5);
        if (string.IsNullOrWhiteSpace(targetPlayer)
            || string.IsNullOrWhiteSpace(tagColor)
            || string.IsNullOrWhiteSpace(tColor)
            || string.IsNullOrWhiteSpace(ctColor)
            || string.IsNullOrWhiteSpace(sayColor))
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(tag))
        {
            player.PrintToChat($"{Prefix}{CC.W} tag hatali");
            return;
        }
        var data = new CustomTagData()
        {
            TagName = tag,
            TagColor = tagColor,
            TColor = tColor,
            CTColor = ctColor,
            SayColor = sayColor,
        };
        var targetArgument = GetTargetArgument(targetPlayer);

        var players = GetPlayers()
               .Where(x =>
               (targetArgument == TargetForArgument.UserIdIndex
               ? GetUserIdIndex(targetPlayer) == x.UserId : targetArgument == TargetForArgument.Me
               ? x.SteamID == player.SteamID : false)
               || (x.PlayerName?.ToLower()?.Contains(targetPlayer?.ToLower()) ?? false)
               || x.SteamID.ToString() == targetPlayer)
               .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
            return;
        }
        if (players.Count != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
            return;
        }
        var y = players.FirstOrDefault();
        if (ValidateCallerPlayer(y, false) == false) return;

        if (CustomPlayerTags.ContainsKey(y.SteamID))
        {
            CustomPlayerTags[y.SteamID] = data;
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.B} {y.PlayerName}{CC.W} adlı oyuncuya {CC.R}[{tag}]{CC.W} tagı verdi");
            return;
        }
        else
        {
            CustomPlayerTags.Add(y.SteamID, data);
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.B} {y.PlayerName}{CC.W} adlı oyuncuya {CC.R}[{tag}]{CC.W} tagı verdi");
        }
        AddOrUpdateCustomTagData(y.SteamID, data);
    }

    [ConsoleCommand("customtagsil")]
    [ConsoleCommand("customtagkaldir")]
    [ConsoleCommand("customtagiptal")]
    [CommandHelper(minArgs: 1, "<custom tagi alinacak kişi>")]
    public void CustomTagSil(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        var targetPlayer = info.ArgString.GetArg(0);
        if (string.IsNullOrWhiteSpace(targetPlayer))
        {
            return;
        }

        var targetArgument = GetTargetArgument(targetPlayer);

        var players = GetPlayers()
               .Where(x =>
               (targetArgument == TargetForArgument.UserIdIndex
               ? GetUserIdIndex(targetPlayer) == x.UserId : targetArgument == TargetForArgument.Me
               ? x.SteamID == player.SteamID : false)
               || (x.PlayerName?.ToLower()?.Contains(targetPlayer?.ToLower()) ?? false)
               || x.SteamID.ToString() == targetPlayer)
               .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
            return;
        }
        if (players.Count != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
            return;
        }
        var y = players.FirstOrDefault();
        if (ValidateCallerPlayer(y, false) == false) return;

        if (CustomPlayerTags.ContainsKey(y.SteamID))
        {
            CustomPlayerTags.Remove(y.SteamID, out var tag);
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.B} {y.PlayerName}{CC.W} adlı oyuncuya {CC.R}[{tag.TagName}]{CC.W} tagını sildi");
            RemoveCustomTagData(y.SteamID);
            return;
        }
    }

    private void RemoveCustomTagData(ulong steamId)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }

                var cmd = new MySqlCommand(@$"Delete From `PlayerCustomTag` WHERE `SteamId` = @SteamId;", con);
                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void AddOrUpdateCustomTagData(ulong steamId, CustomTagData data)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }

                var cmd = new MySqlCommand(@$"SELECT 1 FROM `PlayerCustomTag` WHERE `SteamId` = @SteamId;", con);
                cmd.Parameters.AddWithValue("@SteamId", steamId);
                bool exist = false;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        exist = true;
                    }
                }
                if (exist)
                {
                    cmd = new MySqlCommand(@$"UPDATE `PlayerCustomTag`
                                          SET
                                              `Tag` = @Tag,
                                              `TagColor` = @TagColor,
                                              `SayColor` = @SayColor,
                                              `TColor` = @TColor,
                                              `CTColor` = @CTColor
                                          WHERE `SteamId` = @SteamId;", con);

                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    cmd.Parameters.AddWithValue("@Tag", data.TagName);
                    cmd.Parameters.AddWithValue("@TagColor", data.TagColor);
                    cmd.Parameters.AddWithValue("@SayColor", data.SayColor);
                    cmd.Parameters.AddWithValue("@TColor", data.TColor);
                    cmd.Parameters.AddWithValue("@CTColor", data.CTColor);
                }
                else
                {
                    cmd = new MySqlCommand(@$"INSERT INTO `PlayerCustomTag`
                                      (SteamId,Tag,TagColor,SayColor,TColor,CTColor)
                                      VALUES (@SteamId,@Tag,@TagColor,@SayColor,@TColor,@CTColor);", con);

                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    cmd.Parameters.AddWithValue("@Tag", data.TagName);
                    cmd.Parameters.AddWithValue("@TagColor", data.TagColor);
                    cmd.Parameters.AddWithValue("@SayColor", data.SayColor);
                    cmd.Parameters.AddWithValue("@TColor", data.TColor);
                    cmd.Parameters.AddWithValue("@CTColor", data.CTColor);
                }
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    public static bool CustomTagSay(CCSPlayerController? player, CommandInfo info, bool isSayTeam)
    {
        if (CustomPlayerTags.TryGetValue(player.SteamID, out var data) == false)
        {
            return false;
        }
        var team = GetTeam(player);
        var teamStr = team switch
        {
            CsTeam.CounterTerrorist => $"[GARDİYAN]",
            CsTeam.Terrorist => $"[MAHKÛM]",
            CsTeam.Spectator => $"[SPEC]",
            _ => ""
        };
        var ccRes = GetChatColors(data);
        var c = team switch
        {
            CsTeam.CounterTerrorist => ccRes.CTC,
            CsTeam.Terrorist => ccRes.TC2,
            CsTeam.Spectator => CC.W,
            _ => CC.W
        };
        var deadStr = player.PawnIsAlive == false ? $"{CC.R}*ÖLÜ*" : "";
        var str = $" {deadStr}"
                + $" {ccRes.TC}[{data.TagName}]"
                + $" {c}{(isSayTeam ? $"{teamStr}" : "")}{player.PlayerName}"
                + $" {CC.W}:"
                + $" {ccRes.SC}{info.GetArg(1)}";
        if (isSayTeam)
        {
            GetPlayers(team).ToList()
                .ForEach(x => x.PrintToChat(str));
        }
        else
        {
            Server.PrintToChatAll(str);
        }
        return true;
    }

    private static (char TC, char TC2, char CTC, char SC) GetChatColors(CustomTagData data)
    {
        var tagColor = GetChatColor(data.TagColor);
        var tColor = GetChatColor(data.TColor);
        var ctColor = GetChatColor(data.CTColor);
        var sayColor = GetChatColor(data.SayColor);

        return (tagColor, tColor, ctColor, sayColor);
    }

    private void GetAllCustomTagData(MySqlConnection con)
    {
        if (con == null)
        {
            return;
        }

        MySqlCommand? cmd = new MySqlCommand(@$"SELECT `SteamId`,`Tag`,`TagColor`,`SayColor`,`TColor`,`CTColor` FROM `PlayerCustomTag`;", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                var tag = reader.IsDBNull(1) ? "" : reader.GetString(1);
                var tagColor = reader.IsDBNull(2) ? "w" : reader.GetString(2);
                var sayColor = reader.IsDBNull(3) ? "w" : reader.GetString(3);
                var tColor = reader.IsDBNull(4) ? "w" : reader.GetString(4);
                var ctColor = reader.IsDBNull(5) ? "w" : reader.GetString(5);
                if (steamId == 0)
                {
                    continue;
                }
                if (string.IsNullOrWhiteSpace(tag))
                {
                    continue;
                }
                var data = new CustomTagData()
                {
                    TagName = tag,
                    TagColor = tagColor,
                    SayColor = sayColor,
                    TColor = tColor,
                    CTColor = ctColor
                };

                if (CustomPlayerTags.ContainsKey((ulong)steamId) == false)
                {
                    CustomPlayerTags.Add((ulong)steamId, data);
                }
                else
                {
                    CustomPlayerTags[(ulong)steamId] = data;
                }
            }
        }

        return;
    }
}