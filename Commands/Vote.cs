using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Timers;
using System.Diagnostics.Metrics;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<string, int> Answers = new();
    private static bool VoteInProgress = false;
    private CounterStrikeSharp.API.Modules.Timers.Timer VoteTimer = null;
    private CounterStrikeSharp.API.Modules.Timers.Timer VotePrintTimer = null;
    private ChatMenu LatestVoteMenu = null;
    private Dictionary<ulong, string> AlreadyVotedPlayers = new();
    private string LatestVoteName = null;
    private static Dictionary<ulong, DateTime> LatestVoteAnswerCommandCalls = new Dictionary<ulong, DateTime>();

    #region Vote

    [ConsoleCommand("vote")]
    [ConsoleCommand("oyla")]
    [ConsoleCommand("oylama")]
    [CommandHelper(minArgs: 2, usage: "<soru> [... şıklar ...]", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    public void OnVoteCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye26"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (LatestVoteMenu != null)
        {
            player.PrintToChat($"{Prefix}{CC.W} Mevcut oylama bitmeden yeni oylama açamazsın.");
            return;
        }

        if (string.IsNullOrWhiteSpace(command.ArgString))
        {
            player.PrintToChat($"{Prefix}{CC.W} En az 2 şık ve 1 soru belirlemelisin, şıklar arası boşluk bırakmalısın.");
            return;
        }
        var argCount = command.ArgString.Split(" ");
        argCount = argCount.Select(x => x.Trim()).Where(x => string.IsNullOrWhiteSpace(x) == false).ToArray();

        if (argCount.Length == 0)
        {
            player.PrintToChat($"{Prefix}{CC.W} En az 2 şık ve 1 soru belirlemelisin, şıklar arası boşluk bırakmalısın.");
            return;
        }
        if (argCount?.Length < 3)
        {
            player.PrintToChat($"{Prefix}{CC.W} En az 2 şık ve 1 soru belirlemelisin, şıklar arası boşluk bırakmalısın.");
            return;
        }
        if (argCount?.Length > 7)
        {
            player!.PrintToChat($"{Prefix}{CC.R} Maximum 5 adet şık belirleyebilirsin");
            return;
        }
        Answers.Clear();
        AlreadyVotedPlayers.Clear();
        string question = LatestVoteName = argCount.FirstOrDefault();
        var answers = argCount.Skip(1).Distinct();
        LatestVoteMenu = new(question);

        foreach (var item in answers)
        {
            Answers.Add(item, 0);
            LatestVoteMenu.AddMenuOption(item, (x, option) =>
            {
                ///<see cref="VoteInProgressIntercepter(CCSPlayerController, string)"/>
                ///if (VoteInProgress)
                ///{
                ///    Answers[option.Text]++;
                ///    if (ValidateCallerPlayer(x, false) == true)
                ///    {
                ///        x.PrintToChat($"{Prefix}{CC.B} {option.Text} {CC.W} Seçeneğine oy verdin.");
                ///    }
                ///}
            });
        }
        Server.PrintToChatAll($"{Prefix} {(player == null ? "Console" : $"{CC.B}{player.PlayerName} - {CC.G}{question} Oylamasını başlattı!")}");

        GetPlayers().ToList().ForEach(x =>
        {
            x.PrintToCenter(player == null ? "Console" : $"{CC.B}{player.PlayerName} - {CC.G}{question} Oylaması 20 saniye sürecek, oy vermeyi unutmayın!");
        });

        VoteInProgress = true;

        foreach (var p in GetPlayers())
        {
            ValidateCallerPlayer(p, false);
            if (p == null) continue;
            ChatMenus.OpenMenu(p, LatestVoteMenu);
        }
        Server.PrintToChatAll($"{Prefix} {(player == null ? "Console" : $"{CC.B}{player.PlayerName} - {CC.G}{question} Oylaması 20 saniye sürecek, oy vermeyi unutmayın!")}");

        var i = 0;
        VotePrintTimer = AddTimer(0.1f, () =>
        {
            i++;
            var hmtl = $"<pre><b>Oylama: <font color='#00FF00'>{LatestVoteName}</font><br>" +
                        $" Kalan Süre : <font color='{((int)((200 - i) / 10) > 3 ? "#00FF00" : "#FF0000")}'> {(int)((200 - i) / 10)}</font><br>" +
        string.Join("<br>", Answers.Select((x, i) => $"!{i + 1} || {x.Key} - {x.Value}")) +
                           $"</b></pre>";

            GetPlayers()
            .ToList()
            .ForEach(x => x.PrintToCenterHtml(hmtl));
        }, TimerFlags.REPEAT);

        VoteTimer = AddTimer(20, () =>
          {
              Server.PrintToChatAll(question + $" {CC.R}SORUSUNUN YANITLARI");

              foreach (KeyValuePair<string, int> kvp in Answers)
              {
                  Server.PrintToChatAll($"{Prefix}{CC.B} {kvp.Key} {CC.W} - {CC.Y}{kvp.Value}");
              }
              Answers.Clear();
              VoteInProgress = false;
              LatestVoteMenu = null;
              VotePrintTimer?.Kill();
              VotePrintTimer = null;
              AlreadyVotedPlayers.Clear();
          }, TimerFlags.STOP_ON_MAPCHANGE);

        return;
    }

    [ConsoleCommand("cancelvote")]
    [ConsoleCommand("oylamaiptal")]
    public void CancelVote(CCSPlayerController? player, CommandInfo command)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye26"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Answers?.Clear();

        VoteInProgress = false;

        VoteTimer?.Kill();
        VoteTimer = null;
        VotePrintTimer?.Kill();
        VotePrintTimer = null;
        Server.PrintToChatAll($"{Prefix}{CC.W} OYLAMA IPTAL EDILDI.");

        return;
    }

    private bool VoteInProgressIntercepter(CCSPlayerController player, string arg)
    {
        if (VoteInProgress)
        {
            var voteNoStr = arg.Substring(1);

            if (int.TryParse(voteNoStr, out var voteNo))
            {
                if (voteNo < 1 || voteNo > 5)
                {
                    return false;
                }

                if (Answers.Count < voteNo)
                {
                    return false;
                }
                if (AlreadyVotedPlayers.TryGetValue(player.SteamID, out string votedKey))
                {
                    if (HasLevelPermissionToActivate(player.SteamID, "@css/seviye2") == false)
                    {
                        if (ValidateCallerPlayer(player) == false)
                        {
                            player.PrintToChat($"{Prefix}{CC.W} Yalnızca bir seçeneğe oy verebilirsin.");
                            return true;
                        }
                    }

                    if (LatestVoteAnswerCommandCalls.TryGetValue(player.SteamID, out var call))
                    {
                        if (DateTime.UtcNow < call.AddSeconds(3))
                        {
                            player.PrintToChat($"{Prefix} {CC.W}Oy değiştirebilmek için {CC.DR}3 {CC.W}saniye beklemelisin!");
                            return true;
                        }
                    }
                    var answers = Answers.ToList();

                    var data = answers[voteNo - 1];
                    Answers[data.Key]++;
                    Answers[votedKey]--;

                    if (ValidateCallerPlayer(player, false) == true)
                    {
                        player.PrintToChat($"{Prefix}{CC.W}Oyunu{CC.B} {data.Key} {CC.W} olarak değiştirdin verdin.");
                        GetPlayers()
                            .Where(x => x.SteamID != player.SteamID)
                            .ToList()
                            .ForEach(x =>
                            {
                                x.PrintToChat($"{Prefix}{CC.B} {player.PlayerName} {CC.W} Oyunu değiştirdi.");
                            });
                    }
                    LatestVoteAnswerCommandCalls[player.SteamID] = DateTime.UtcNow;

                    return true;
                }
                else
                {
                    var answers = Answers.ToList();

                    var data = answers[voteNo - 1];
                    Answers[data.Key]++;

                    if (ValidateCallerPlayer(player, false) == true)
                    {
                        AlreadyVotedPlayers.Add(player.SteamID, data.Key);

                        player.PrintToChat($"{Prefix}{CC.B} {data.Key} {CC.W} Seçeneğine oy verdin.");
                        GetPlayers()
                            .Where(x => x.SteamID != player.SteamID)
                            .ToList()
                            .ForEach(x =>
                            {
                                x.PrintToChat($"{Prefix}{CC.B} {player.PlayerName} {CC.W} Oyunu kullandı.");
                            });
                    }
                }
                return true;
            }
        }
        else if (KomAlVoteInProgress)
        {
            var voteNoStr = arg.Substring(1);

            if (int.TryParse(voteNoStr, out var voteNo))
            {
                if (voteNo < 1 || voteNo > 5)
                {
                    return false;
                }

                if (KomAlAnswers.Count < voteNo)
                {
                    return false;
                }
                if (AlreadyVotedPlayers.TryGetValue(player.SteamID, out string votedKey))
                {
                    if (HasLevelPermissionToActivate(player.SteamID, "@css/seviye2") == false)
                    {
                        if (ValidateCallerPlayer(player) == false)
                        {
                            player.PrintToChat($"{Prefix}{CC.W} Yalnızca bir seçeneğe oy verebilirsin.");
                            return true;
                        }
                    }

                    if (LatestVoteAnswerCommandCalls.TryGetValue(player.SteamID, out var call))
                    {
                        if (DateTime.UtcNow < call.AddSeconds(3))
                        {
                            player.PrintToChat($"{Prefix} {CC.W}Oy değiştirebilmek için {CC.DR}3 {CC.W}saniye beklemelisin!");
                            return true;
                        }
                    }
                    var answers = KomAlAnswers.ToList();

                    var data = answers[voteNo - 1];
                    KomAlAnswers[data.Key]++;
                    KomAlAnswers[ulong.Parse(votedKey)]--;

                    if (ValidateCallerPlayer(player, false) == true)
                    {
                        var voted = GetPlayers().Where(x => x.SteamID == data.Key).Select(x => x.PlayerName).FirstOrDefault();
                        player.PrintToChat($"{Prefix}{CC.W}Oyunu{CC.B} {voted} {CC.W} olarak değiştirdin verdin.");
                        GetPlayers()
                            .Where(x => x.SteamID != player.SteamID)
                            .ToList()
                            .ForEach(x =>
                            {
                                x.PrintToChat($"{Prefix}{CC.B} {player.PlayerName} {CC.W} Oyunu değiştirdi.");
                            });
                    }
                    LatestVoteAnswerCommandCalls[player.SteamID] = DateTime.UtcNow;
                }
                else
                {
                    var answers = KomAlAnswers.ToList();

                    var data = answers[voteNo - 1];
                    KomAlAnswers[data.Key]++;

                    if (ValidateCallerPlayer(player, false) == true)
                    {
                        AlreadyVotedPlayers.Add(player.SteamID, data.Key.ToString());
                        var voted = GetPlayers().Where(x => x.SteamID == data.Key).Select(x => x.PlayerName).FirstOrDefault();

                        player.PrintToChat($"{Prefix}{CC.B} {voted} {CC.W} Seçeneğine oy verdin.");
                        GetPlayers()
                            .Where(x => x.SteamID != player.SteamID)
                            .ToList()
                            .ForEach(x =>
                            {
                                x.PrintToChat($"{Prefix}{CC.B} {player.PlayerName} {CC.W} Oyunu kullandı.");
                            });
                    }
                }
                return true;
            }
        }
        return false;
    }

    #endregion Vote
}