//using CounterStrikeSharp.API;
//using CounterStrikeSharp.API.Core;
//using CounterStrikeSharp.API.Core.Attributes.Registration;
//using CounterStrikeSharp.API.Modules.Admin;
//using CounterStrikeSharp.API.Modules.Commands;
//using CounterStrikeSharp.API.Modules.Commands.Targeting;
//using CounterStrikeSharp.API.Modules.Utils;
//using System.Text;

//namespace JailbreakExtras;

//public partial class JailbreakExtras
//{
//    #region Gag

//    [ConsoleCommand("gag")]
//    [RequiresPermissions("@css/chat")]
//    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead>  [dakika/0 sınırsız]")]
//    public void OnGagCommand(CCSPlayerController? player, CommandInfo info)
//    {
//        int time = 0;
//        if (info.ArgCount < 2) return;
//        var target = info.GetArg(1);
//        var targetArgument = GetTargetArgument(target);
//        _ = int.TryParse(info.GetArg(2), out time);

//        GetPlayers()
//        .Where(x => targetArgument switch
//        {
//            TargetForArgument.All => true,
//            TargetForArgument.T => GetTeam(x) == CsTeam.Terrorist,
//            TargetForArgument.Ct => GetTeam(x) == CsTeam.CounterTerrorist,
//            TargetForArgument.Me => player.PlayerName == x.PlayerName,
//            TargetForArgument.Alive => x.PawnIsAlive,
//            TargetForArgument.Dead => x.PawnIsAlive == false,
//            TargetForArgument.None => x.PlayerName?.ToLower()?.Contains(target) ?? false,
//            _ => false
//        }
//            && ValidateCallerPlayer(x, false))
//        .ToList()
//        .ForEach(x =>
//        {
//            Task.Run(async () =>
//            {
//                await _muteManager.MutePlayer(playerInfo, adminInfo, reason, time);
//            });

//            if (time > 0 && time <= 30)
//            {
//                AddTimer(time * 60, () =>
//                {
//                    if (x == null || !x.IsValid || x.AuthorizedSteamID == null) return;
//                x.

//                    //MuteManager _muteManager = new(dbConnectionString);
//                    //_ = _muteManager.UnmutePlayer(x.AuthorizedSteamID.SteamId64.ToString(), 0);
//                }, CounterStrikeSharp.API.Modules.Timers.TimerFlags.STOP_ON_MAPCHANGE);
//            }

//            if (time == 0)
//            {
//                x!.PrintToCenter(_localizer!["sa_x_gag_message_perm", reason, x == null ? "Console" : x.PlayerName]);
//                StringBuilder sb = new(_localizer!["sa_prefix"]);
//                sb.Append(_localizer["sa_admin_gag_message_perm", x == null ? "Console" : x.PlayerName, x.PlayerName, reason]);
//                Server.PrintToChatAll(sb.ToString());
//            }
//            else
//            {
//                x!.PrintToCenter(_localizer!["sa_x_gag_message_time", reason, time, x == null ? "Console" : x.PlayerName]);
//                StringBuilder sb = new(_localizer!["sa_prefix"]);
//                sb.Append(_localizer["sa_admin_gag_message_time", x == null ? "Console" : x.PlayerName, x.PlayerName, reason, time]);
//                Server.PrintToChatAll(sb.ToString());
//            }
//        });
//    }

//    [ConsoleCommand("addgag")]
//    [RequiresPermissions("@css/chat")]
//    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead>  [dakika/0 sınırsız]")]
//    public void OnAddGagCommand(CCSPlayerController? player, CommandInfo info)
//    {
//        if (info.ArgCount < 2)
//            return;
//        if (string.IsNullOrEmpty(info.GetArg(1))) return;

//        string steamid = info.GetArg(1);

//        if (!Helper.IsValidSteamID64(steamid))
//        {
//            info.ReplyToCommand($"Invalid SteamID64.");
//            return;
//        }

//        int time = 0;
//        string reason = "Unknown";

//        MuteManager _muteManager = new(dbConnectionString);

//        int.TryParse(info.GetArg(2), out time);

//        if (info.ArgCount >= 3 && info.GetArg(3).Length > 0)
//            reason = info.GetArg(3);

//        PlayerInfo adminInfo = new PlayerInfo
//        {
//            SteamId = player?.AuthorizedSteamID?.SteamId64.ToString(),
//            Name = player?.PlayerName,
//            IpAddress = player?.IpAddress?.Split(":")[0]
//        };

//        List<CCSPlayerController> matches = Helper.GetPlayerFromSteamid64(steamid);
//        if (matches.Count == 1)
//        {
//            CCSPlayerController? player = matches.FirstOrDefault();
//            if (player != null && player.IsValid)
//            {
//                if (!player!.CanTarget(player))
//                {
//                    info.ReplyToCommand($"{player.PlayerName} is more powerful than you!");
//                    return;
//                }

//                if (time == 0)
//                {
//                    player!.PrintToCenter(_localizer!["sa_player_gag_message_perm", reason, player == null ? "Console" : player.PlayerName]);
//                    StringBuilder sb = new(_localizer!["sa_prefix"]);
//                    sb.Append(_localizer["sa_admin_gag_message_perm", player == null ? "Console" : player.PlayerName, player.PlayerName, reason]);
//                    Server.PrintToChatAll(sb.ToString());
//                }
//                else
//                {
//                    player!.PrintToCenter(_localizer!["sa_player_gag_message_time", reason, time, player == null ? "Console" : player.PlayerName]);
//                    StringBuilder sb = new(_localizer!["sa_prefix"]);
//                    sb.Append(_localizer["sa_admin_gag_message_time", player == null ? "Console" : player.PlayerName, player.PlayerName, reason, time]);
//                    Server.PrintToChatAll(sb.ToString());
//                }

//                if (TagsDetected)
//                    NativeAPI.IssueServerCommand($"tag_mute {player!.Index.ToString()}");

//                if (time > 0 && time <= 30)
//                {
//                    AddTimer(time * 60, () =>
//                    {
//                        if (player == null || !player.IsValid || player.AuthorizedSteamID == null) return;

//                        if (TagsDetected)
//                            NativeAPI.IssueServerCommand($"tag_unmute {player.Index.ToString()}");

//                        if (gaggedPlayers.Contains((int)player.Index))
//                        {
//                            if (gaggedPlayers.TryTake(out int removedItem) && removedItem != (int)player.Index)
//                            {
//                                gaggedPlayers.Add(removedItem);
//                            }
//                        }

//                        //_ = _muteManager.UnmutePlayer(player.AuthorizedSteamID.SteamId64.ToString(), 0);
//                    }, CounterStrikeSharp.API.Modules.Timers.TimerFlags.STOP_ON_MAPCHANGE);
//                }

//                if (!gaggedPlayers.Contains((int)player.Index))
//                    gaggedPlayers.Add((int)player.Index);
//            }
//        }
//        _ = _muteManager.AddMuteBySteamid(steamid, adminInfo, reason, time, 0);
//        info.ReplyToCommand($"Gagged player with steamid {steamid}.");
//    }

//    [ConsoleCommand("ungag")]
//    [RequiresPermissions("@css/chat")]
//    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead>  [dakika/0 sınırsız]")]
//    public void OnUngagCommand(CCSPlayerController? player, CommandInfo info)
//    {
//        if (info.GetArg(1).Length <= 1)
//        {
//            info.ReplyToCommand($"Too short pattern to search.");
//            return;
//        }

//        bool found = false;

//        string pattern = info.GetArg(1);
//        MuteManager _muteManager = new(dbConnectionString);

//        if (Helper.IsValidSteamID64(pattern))
//        {
//            List<CCSPlayerController> matches = Helper.GetPlayerFromSteamid64(pattern);
//            if (matches.Count == 1)
//            {
//                CCSPlayerController? player = matches.FirstOrDefault();
//                if (player != null && player.IsValid)
//                {
//                    if (gaggedPlayers.Contains((int)player.Index))
//                    {
//                        if (gaggedPlayers.TryTake(out int removedItem) && removedItem != (int)player.Index)
//                        {
//                            gaggedPlayers.Add(removedItem);
//                        }
//                    }

//                    if (TagsDetected)
//                        NativeAPI.IssueServerCommand($"tag_unmute {player!.Index.ToString()}");

//                    found = true;
//                }
//            }
//        }
//        else
//        {
//            List<CCSPlayerController> matches = Helper.GetPlayerFromName(pattern);
//            if (matches.Count == 1)
//            {
//                CCSPlayerController? player = matches.FirstOrDefault();
//                if (player != null && player.IsValid)
//                {
//                    if (gaggedPlayers.Contains((int)player.Index))
//                    {
//                        if (gaggedPlayers.TryTake(out int removedItem) && removedItem != (int)player.Index)
//                        {
//                            gaggedPlayers.Add(removedItem);
//                        }
//                    }

//                    if (TagsDetected)
//                        NativeAPI.IssueServerCommand($"tag_unmute {player!.Index.ToString()}");

//                    pattern = player.AuthorizedSteamID!.SteamId64.ToString();

//                    found = true;
//                }
//            }
//        }
//        if (found)
//        {
//            _ = _muteManager.UnmutePlayer(pattern, 0); // Unmute by type 0 (gag)
//            info.ReplyToCommand($"Ungaged player with pattern {pattern}.");
//            return;
//        }

//        TargetResult? targets = GetTarget(info);
//        if (targets == null) return;
//        List<CCSPlayerController> playersToTarget = targets!.Players.Where(player => player!.CanTarget(player) && player != null && player.IsValid).ToList();

//        if (playersToTarget.Count > 1 && Config.DisableDangerousCommands)
//        {
//            return;
//        }

//        if (playersToTarget.Count > 1)
//        {
//            playersToTarget.ForEach(player =>
//            {
//                if (gaggedPlayers.Contains((int)player.Index))
//                {
//                    if (gaggedPlayers.TryTake(out int removedItem) && removedItem != (int)player.Index)
//                    {
//                        gaggedPlayers.Add(removedItem);
//                    }
//                }

//                if (player.AuthorizedSteamID != null)
//                    _ = _muteManager.UnmutePlayer(player.AuthorizedSteamID.SteamId64.ToString(), 0); // Unmute by type 0 (gag)

//                if (TagsDetected)
//                    NativeAPI.IssueServerCommand($"tag_unmute {player!.Index.ToString()}");
//            });

//            info.ReplyToCommand($"Ungaged player with pattern {pattern}.");
//            return;
//        }
//    }

//    #endregion Gag
//}