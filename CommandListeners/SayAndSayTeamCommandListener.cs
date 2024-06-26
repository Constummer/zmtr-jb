using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void SayAndSayTeamCommandListener()
    {
        AddCommandListener("say", OnSay);
        AddCommandListener("say_team", OnSayTeam);
    }

    private HookResult OnSayTeam(CCSPlayerController? player, CommandInfo commandInfo) => OnSayOrSayTeam(player, commandInfo, true);

    private HookResult OnSay(CCSPlayerController? player, CommandInfo commandInfo) => OnSayOrSayTeam(player, commandInfo, false);

    private void LogPlayerChat(ulong steamId, string msg)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"INSERT INTO `PlayerChat`
                                     (SteamId,Msg)
                                     VALUES (@SteamId,@Msg);", con);

                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.Parameters.AddWithValue("@Msg", msg);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
        }
    }

    private static readonly List<string> NumberChoices = new List<string>()
    {
        "!1",
        "!2",
        "!3",
        "!4",
        "!5",
        "!6",
        "!7",
        "!8",
        "!9",
        "!0",
        "/1",
        "/2",
        "/3",
        "/4",
        "/5",
        "/6",
        "/7",
        "/8",
        "/9",
        "/0",
        "css_1",
        "css_2",
        "css_3",
        "css_4",
        "css_5",
        "css_6",
        "css_7",
        "css_8",
        "css_9",
        "css_0",
    };

    private HookResult OnSayOrSayTeam(CCSPlayerController? player, CommandInfo info, bool isSayTeam)
    {
        if (player == null) return HookResult.Continue;
        var arg = info.GetArg(1);
        Server.PrintToConsole($"{player.SteamID}|{player.PlayerName} : {arg}");

        if (string.IsNullOrWhiteSpace(arg) || arg.Replace(" ", string.Empty) == string.Empty)
            return HookResult.Handled;

        if (ValidateCallerPlayer(player, false) == false)
        {
            return HookResult.Continue;
        }
        LogPlayerChat(player.SteamID, arg);

        if (arg.StartsWith("!") || arg.StartsWith("/") || arg.StartsWith("css_"))
        {
            //if (!VoteInProgress && !KomAlVoteInProgress)
            //{
            //    if (NumberChoices.Contains(arg.Trim()))
            //    {
            //        var number = arg.Last();
            //        player.ExecuteClientCommandFromServer($"css_{number}");
            //        return HookResult.Handled;
            //    }
            //}
            if (arg.Contains("!skins"))
            {
                player.PrintToChat($"{Prefix} {CC.W} L�TFEN BU MEN�Y� KULLANMAK YER�NE {CC.R}skin.zmtr.org {CC.W} S�TES�NDEN SK�N SE��N�Z.");
                player.PrintToChat($"{Prefix} {CC.W} L�TFEN BU MEN�Y� KULLANMAK YER�NE {CC.R}skin.zmtr.org {CC.W} S�TES�NDEN SK�N SE��N�Z.");
                player.PrintToChat($"{Prefix} {CC.W} L�TFEN BU MEN�Y� KULLANMAK YER�NE {CC.R}skin.zmtr.org {CC.W} S�TES�NDEN SK�N SE��N�Z.");
                return HookResult.Handled;
            }
            if (csaytestActive && AdminManager.PlayerHasPermissions(player, Perm_Root))
            {
                Server.PrintToChatAll("arg=>" + arg);
                var key = string.IsNullOrWhiteSpace(arg) ? "" : arg.Split(" ")[0];
                if (key.StartsWith("!") || key.StartsWith("/") || key.StartsWith("css_"))
                {
                    key = key.Substring(1);
                }
                Server.PrintToChatAll("key=>" + key);

                ExecuteCommand(key, player);
                return HookResult.Handled;
            }

            if (KomKalanIntercepter(player, arg))
            {
                return HookResult.Handled;
            }
            if (VoteInProgressIntercepter(player, arg) == true)
            {
                return HookResult.Handled;
            }
            if (GagChecker(player, arg, true, isSayTeam))
            {
                return HookResult.Handled;
            }
            if (OnSteamGroupPlayerChat(player, arg))
            {
                return HookResult.Handled;
            }
            return HookResult.Continue;
        }
        if (KomdkIntercepter(player, isSayTeam, arg))
        {
            return HookResult.Handled;
        }
        if (GagChecker(player, arg))
        {
            return HookResult.Handled;
        }
        if (PatronuKoruActive)
        {
            if (PatronuKoruSay(player, info, isSayTeam))
            {
                return HookResult.Handled;
            }
        }
        if (KomutcuAdminSay(player, info, isSayTeam))
        {
            return HookResult.Handled;
        }
        if (LatestWCommandUser == player.SteamID)
        {
            WardenSay(player, info, isSayTeam);
            return HookResult.Handled;
        }
        if (CustomTagSay(player, info, isSayTeam))
        {
            return HookResult.Handled;
        }
        if (LevelSystemPlayer(player, info, isSayTeam))
        {
            return HookResult.Handled;
        }
        if (WLevelPlayer(player, info, isSayTeam))
        {
            return HookResult.Handled;
        }
        return HookResult.Continue;
    }
}