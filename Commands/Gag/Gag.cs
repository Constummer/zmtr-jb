using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, DateTime> Gags = new Dictionary<ulong, DateTime>();

    #region Gag

    [ConsoleCommand("gag")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead>")]
    public void OnGagCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var target = info.ArgCount > 1 ? info.GetArg(1) : null;
        if (target == null)
        {
            return;
        }
        var godOneTwoStr = "0";
        if (int.TryParse(godOneTwoStr, out var value) == false)
        {
            return;
        }

        var targetArgument = GetTargetArgument(target);
        GetPlayers()
            .Where(x => targetArgument switch
            {
                TargetForArgument.All => true,
                TargetForArgument.T => GetTeam(x) == CsTeam.Terrorist,
                TargetForArgument.Ct => GetTeam(x) == CsTeam.CounterTerrorist,
                TargetForArgument.Me => player.PlayerName == x.PlayerName,
                TargetForArgument.Alive => x.PawnIsAlive,
                TargetForArgument.Dead => x.PawnIsAlive == false,
                TargetForArgument.None => x.PlayerName?.ToLower()?.Contains(target) ?? false,
                TargetForArgument.UserIdIndex => GetUserIdIndex(target) == x.UserId,
                _ => false
            }
            && ValidateCallerPlayer(x, false))
            .ToList()
            .ForEach(gagPlayer =>
            {
                if (value <= 0)
                {
                    if (Gags.TryGetValue(gagPlayer.SteamID, out var dateTime))
                    {
                        Gags[gagPlayer.SteamID] = DateTime.UtcNow.AddYears(1);
                    }
                    else
                    {
                        Gags.Add(gagPlayer.SteamID, DateTime.UtcNow.AddYears(1));
                    }
                    if (targetArgument == TargetForArgument.None)
                    {
                        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{gagPlayer.PlayerName} {CC.W}gagladı.");
                    }
                }
            });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini gagladı.");
        }
    }

    private bool GagChecker(CCSPlayerController player, string arg, bool isArg = false, bool isSayTeam = false)
    {
        if (Gags.TryGetValue(player.SteamID, out var call))
        {
            if (call > DateTime.UtcNow)
            {
                player.PrintToChat($"{Prefix} {CC.W}GAGLISIN!");
                return true;
                if (isArg)
                {
                    if (CheckAllowedCommand(player, arg, true, isSayTeam) == false)
                    {
                        player.PrintToChat($"{Prefix} {CC.W}GAGLISIN!");
                        return true;
                    }
                    else
                    {
                        player.PrintToChat($"{Prefix} {CC.W}GAGLISIN!");
                        return true;
                    }
                }
                else
                {
                    player.PrintToChat($"{Prefix} {CC.W}GAGLISIN!");
                    return true;
                }
            }
            else
            {
                Gags.Remove(player.SteamID);
            }
        }
        if (PGags.Contains(player.SteamID))
        {
            player.PrintToChat($"{Prefix} {CC.W}SINIRSIZ GAGLISIN!");
            return true;
        }
        return false;
    }

    private bool CheckAllowedCommand(CCSPlayerController player, string arg, bool isArg = false, bool isSayTeam = false)
    {
        var key = string.IsNullOrWhiteSpace(arg) ? "" : arg.Split(" ")[0];
        if (key.StartsWith("!") || key.StartsWith("/") || key.StartsWith("css_"))
        {
            key = key.Substring(1);
        }

        if (isArg == true && Config.DontBlockOnGagged.DontBlockOnGaggedCommands.Contains(key))
        {
            if (PlayerLevels.TryGetValue(player.SteamID, out var item))
            {
                var config = GetPlayerLevelConfig(item.Xp);
                if (config != null)
                {
                    ExecuteCommand(key, player);
                    PrintMsgCustom(player, $"!{key}", isSayTeam, config);
                    return false;
                }
            }
            ExecuteCommand(key, player);
            PrintMsgCustom(player, $"!{key}", isSayTeam, null);
            return false;
        }
        return true;
    }

    private void ExecuteCommand(string key, CCSPlayerController player)
    {
        foreach (var item in base.CommandHandlers.Keys)
        {
            if (item.Method.Name?.ToLower() == key?.ToLower())
            {
                Logger.LogInformation($"{item.Method.Name} event invoked.");

                if (item is Action<CCSPlayerController?, CommandInfo> customEventHandler)
                {
                    customEventHandler.Invoke(player, null);
                }
                else
                {
                    item.DynamicInvoke(player, null);
                }
                break;
            }
            //base.CommandHandlers.Keys.FirstOrDefault().Method.Name
        }
    }

    #endregion Gag
}