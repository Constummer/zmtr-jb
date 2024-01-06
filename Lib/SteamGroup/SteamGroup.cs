using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private readonly List<ulong> PlayerSteamGroup = new();

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

    private bool CheckPlayerGroups(CCSPlayerController? player)
    {
        if (player == null || !player.IsValid || player.IsBot) return false;

        if (Config.SteamGroup.SteamApiKey == "-" || Config.SteamGroup.SteamGroupId == "-")
            return false;

        if (PlayerSteamGroup.Contains(player.SteamID))
        {
            return true;
        }
        else
        {
            string apiUrl = $"https://api.steampowered.com/ISteamUser/GetUserGroupList/v1/?key={Config.SteamGroup.SteamApiKey}&steamid={player.SteamID}";

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

                                if (groupId == Config.SteamGroup.SteamGroupId)
                                {
                                    PlayerSteamGroup.Add(player.SteamID);
                                    return true;
                                }
                            }
                        }
                    }

                    return false;
                });
            }
            catch (Exception e)
            {
                Logger.LogError(e, "hata");

                return false;
            }
        }

        return false;
    }

    private void PlayerGroupCheck(CCSPlayerController? player, CommandInfo info)
    {
        Task.Run(() =>
        {
            Server.NextFrame(() =>
            {
                if (CheckPlayerGroups(player))
                {
                    player.PrintToChat($"{Prefix} {CC.W}Steam grubumuza katıldığın için teşekkurler. Artık komutları kullanabilirsin");
                }
                else
                {
                    player.PrintToChat($"{Prefix} {CC.W}Steam grubumuza katılmamışsın.");
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
}