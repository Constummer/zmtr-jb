using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using System.Net.Http.Json;
using System.Text.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static List<ulong> PlayerSteamGroup = new();
    private static Dictionary<ulong, int> SteamGroupCheckTimer = new();

    private bool OnSteamGroupPlayerChat(CCSPlayerController? player, string arg)
    {
        if (player == null || !player.IsValid || player.IsBot) return false;

        if (arg.StartsWith("!") || arg.StartsWith("/"))
        {
            if (Config.SteamGroup.BlockedCommands.Any() && Config.SteamGroup.BlockedCommands.Contains(arg))
            {
                string steamId64 = new SteamID(player.SteamID).SteamId64.ToString();

                if (PlayerSteamGroup.Contains(player.SteamID))
                {
                    return false;
                }
                else
                {
                    player.PrintToChat($"{Prefix} Bu komutu kullanabilmek icin {CC.B}Steam Grubumuza {CC.G}katilmalisin!!!!!!!");
                    player.PrintToChat($"{Prefix} {CC.W}Katildiysan {CC.B}!grup {CC.W}yaz");
                    return true;
                }
            }
        }

        return false;
    }

    private static bool CheckPlayerGroups(ulong steamId)
    {
        if (_Config.SteamGroup.SteamApiKey == "-" || _Config.SteamGroup.SteamGroupId == "-")
            return false;

        if (PlayerSteamGroup.Contains(steamId))
        {
            return true;
        }
        else
        {
            string apiUrl = $"https://api.steampowered.com/ISteamUser/GetUserGroupList/v1/?key={_Config.SteamGroup.SteamApiKey}&steamid={steamId}";

            try
            {
                Task.Run(async () =>
                {
                    JsonElement jsonData = await _httpClient.GetFromJsonAsync<JsonElement>(apiUrl);
                    dynamic? response = jsonData.Deserialize<dynamic>();

                    if (jsonData.TryGetProperty("response", out var responseProperty) &&
                        responseProperty.ValueKind == JsonValueKind.Object)
                    {
                        bool success = responseProperty.GetProperty("success").GetBoolean();
                        if (success)
                        {
                            foreach (var group in responseProperty.GetProperty("groups").EnumerateArray())
                            {
                                string? groupId = group.GetProperty("gid").GetString();

                                if (groupId == _Config.SteamGroup.SteamGroupId)
                                {
                                    if (PlayerSteamGroup.Contains(steamId))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        PlayerSteamGroup.Add(steamId);
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    if (SteamGroupCheckTimer.ContainsKey(steamId) == false)
                    {
                        SteamGroupCheckTimer.Add(steamId, 0);
                    }
                    return false;
                });
            }
            catch (Exception e)
            {
                ConsMsg(e.Message);
                return false;
            }
        }

        return false;
    }

    private void PlayerGroupCheck(CCSPlayerController? player, CommandInfo info)
    {
        Task.Run(() =>
        {
            if (ValidateCallerPlayer(player, false) == false) return Task.CompletedTask;
            Server.NextFrame(() =>
            {
                if (ValidateCallerPlayer(player, false) == false) return;
                if (CheckPlayerGroups(player.SteamID))
                {
                    player.PrintToChat($"{Prefix} {CC.W}Steam grubumuza katıldığın için teşekkurler. Artık komutları kullanabilirsin");
                    player.PrintToChat($"{Prefix} {CC.W}Steam Grup: steamcommunity.com/groups/zombieturkeyclan/");
                }
                else
                {
                    player.PrintToChat($"{Prefix} {CC.W}Steam grubumuza katılmamışsın.");
                    player.PrintToChat($"{Prefix} {CC.W}Steam hesabının gizli {CC.R}OLMAMASI {CC.W}gerekiyor.");
                    player.PrintToChat($"{Prefix} {CC.W}Steam Grup: steamcommunity.com/groups/zombieturkeyclan/");
                    player.PrintToChat($"{Prefix} {CC.W}Katılarak {CC.B}!knife {CC.W},{CC.B}!ws {CC.W},{CC.B}!skinler {CC.W},{CC.B}!yenile");
                    player.PrintToChat($"{Prefix} {CC.W}Komutlarını kullanabilirsin");
                }
            });

            return Task.CompletedTask;
        });
    }

    private void BlockGroupCommandsLoad()
    {
        foreach (var command in Config.BlockedRadio.BlockedRadioCommands)
        {
            AddCommandListener(command, (player, info) =>
            {
                if (ValidateCallerPlayer(player, false) == false)
                {
                    return HookResult.Continue;
                }

                if (player!.SteamID == LatestWCommandUser
                    && GetTeam(player) == CsTeam.CounterTerrorist)
                {
                    if (Config.BlockedRadio.WardenAllowedRadioCommands.Contains(info.GetCommandString))
                    {
                        return HookResult.Continue;
                    }
                }

                return HookResult.Stop;
            });
        }
    }

    private void CheckSteamGroupsData()
    {
        foreach (var item in SteamGroupCheckTimer.ToList())
        {
            if (SteamGroupCheckTimer.TryGetValue(item.Key, out int value))
            {
                if (value > 3)
                {
                    SteamGroupCheckTimer.Remove(item.Key);
                    continue;
                }
                value++;
                SteamGroupCheckTimer[item.Key] = value;
                CheckPlayerGroups(item.Key);
            }
        }
    }
}