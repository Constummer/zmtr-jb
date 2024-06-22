using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

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
    private static List<ulong> LatestVoteAnswerCommandCalls = new();

    #region Vote

    [ConsoleCommand("vote")]
    [ConsoleCommand("oyla")]
    [ConsoleCommand("oylama")]
    [CommandHelper(minArgs: 2, usage: "<soru> [... şıklar ...]", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    public void OnVoteCommand(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(player.SteamID, info.GetCommandString);
        if (OnCommandValidater(player, true, Perm_Seviye26, Perm_Seviye26) == false)
        {
            return;
        }
        VoteAction(player, info.ArgString);
    }

    private void VoteAction(CCSPlayerController? player, string argstr, short voteTime = 20, Action<Dictionary<string, int>> voteFinished = null)
    {
        if (LatestVoteMenu != null)
        {
            player.PrintToChat($"{Prefix}{CC.W} Mevcut oylama bitmeden yeni oylama açamazsın.");
            return;
        }

        if (string.IsNullOrWhiteSpace(argstr))
        {
            player.PrintToChat($"{Prefix}{CC.W} En az 2 şık ve 1 soru belirlemelisin, şıklar arası boşluk bırakmalısın.");
            return;
        }
        var argCount = argstr.Split(" ");
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
        string question = LatestVoteName = argCount.FirstOrDefault();
        if (question == "kom" || question == "komdk" || question == "komutcu")
        {
            player.PrintToChat($"{Prefix}{CC.W} EĞER KOM DK YAPMAK İSTİYORSAN !komdkyap KOMUTUNU KULLAN.");
            player.PrintToChat($"{Prefix}{CC.W} EĞER KOM DK YAPMAK İSTİYORSAN !komdkyap KOMUTUNU KULLAN.");
            player.PrintToChat($"{Prefix}{CC.W} EĞER KOM DK YAPMAK İSTİYORSAN !komdkyap KOMUTUNU KULLAN.");
            return;
        }
        Answers.Clear();
        AlreadyVotedPlayers.Clear();
        var answers = argCount.Skip(1).Distinct();
        if (answers.Count() < 2)
        {
            player.PrintToChat($"{Prefix}{CC.W} En az 2 şık ve 1 soru belirlemelisin, şıklar arası boşluk bırakmalısın.");
            return;
        }
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
            x.PrintToCenter(player == null ? "Console" : $"{CC.B}{player.PlayerName} - {CC.G}{question} Oylaması {voteTime} saniye sürecek, oy vermeyi unutmayın!");
        });

        VoteInProgress = true;

        foreach (var p in GetPlayers())
        {
            ValidateCallerPlayer(p, false);
            if (p == null) continue;
            MenuManager.OpenChatMenu(p, LatestVoteMenu);
        }
        Server.PrintToChatAll($"{Prefix} {(player == null ? "Console" : $"{CC.B}{player.PlayerName} - {CC.G}{question} Oylaması {voteTime} saniye sürecek, oy vermeyi unutmayın!")}");

        var i = 0;
        VotePrintTimer = AddTimer(0.1f, () =>
        {
            i++;
            var hmtl = $"<b>Oylama: <font color='#00FF00'>{LatestVoteName}</font><br>" +
                        $" <font color='{((int)(((voteTime * 10) - i) / 10) > 3 ? "#FA2F2F" : "#FF0000")}'>Kalan Süre : {(int)(((voteTime * 10) - i) / 10)}</font><br>" +
        string.Join("<br>",
        Answers.Select((x, i) => $" <font color='#FF7B00'>!{i + 1}</font> || {x.Key} - {x.Value}")) +
                           $"</b>";

            GetPlayers()
            .ToList()
            .ForEach(x =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                PrintToCenterHtml(x, hmtl);
            });
        }, Full);

        VoteTimer = AddTimer(voteTime, () =>
        {
            Server.PrintToChatAll(question + $" {CC.R}SORUSUNUN YANITLARI");

            foreach (KeyValuePair<string, int> kvp in Answers)
            {
                Server.PrintToChatAll($"{Prefix}{CC.Or} {kvp.Key} {CC.W} - {CC.Y}{kvp.Value}");
            }
            var answersTemp = new Dictionary<string, int>(Answers);
            Answers.Clear();
            VoteInProgress = false;
            LatestVoteMenu = null;
            VotePrintTimer?.Kill();
            VotePrintTimer = null;
            LatestVoteAnswerCommandCalls?.Clear();
            AlreadyVotedPlayers.Clear();
            if (voteFinished != null)
            {
                voteFinished?.Invoke(answersTemp);
            }
        }, SOM);

        return;
    }

    [ConsoleCommand("cancelvote")]
    [ConsoleCommand("oylamaiptal")]
    public void CancelVote(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye26, Perm_Seviye26) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        Answers?.Clear();

        VoteInProgress = false;
        LatestVoteMenu = null;
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
                if (voteNo < 1 || voteNo > 7)
                {
                    return false;
                }

                if (Answers.Count < voteNo)
                {
                    return false;
                }
                if (AlreadyVotedPlayers.TryGetValue(player.SteamID, out string votedKey))
                {
                    if (HasLevelPermissionToActivate(player.SteamID, Perm_Seviye2) == false)
                    {
                        if (ValidateCallerPlayer(player) == false)
                        {
                            player.PrintToChat($"{Prefix}{CC.W} Yalnızca bir seçeneğe oy verebilirsin.");
                            return true;
                        }
                    }
                    else
                    {
                        if (ValidateCallerPlayer(player, false) == false)
                        {
                            return true;
                        }
                    }

                    if (LatestVoteAnswerCommandCalls.Contains(player.SteamID))
                    {
                        player.PrintToChat($"{Prefix} {CC.W}Tekrar oy değiştiremezsin!");
                        return true;
                    }
                    var answers = Answers.ToList();

                    var data = answers[voteNo - 1];
                    Answers[data.Key]++;
                    Answers[votedKey]--;

                    if (ValidateCallerPlayer(player, false) == true)
                    {
                        player.PrintToChat($"{Prefix}{CC.W}Oyunu{CC.Ol} {data.Key} {CC.W} olarak değiştirdin.");
                        GetPlayers()
                            .Where(x => x.SteamID != player.SteamID)
                            .ToList()
                            .ForEach(x =>
                            {
                                x.PrintToChat($"{Prefix}{CC.B} {player.PlayerName} {CC.W} Oyunu değiştirdi.");
                            });
                    }
                    LatestVoteAnswerCommandCalls.Add(player.SteamID);

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

                        player.PrintToChat($"{Prefix}{CC.Ol} {data.Key} {CC.G} Seçeneğine oy verdin.");
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
                if (voteNo < 1 || voteNo > 9)
                {
                    return false;
                }

                if (KomAlAnswers.Count < voteNo)
                {
                    return false;
                }
                if (AlreadyVotedPlayers.TryGetValue(player.SteamID, out string votedKey))
                {
                    if (HasLevelPermissionToActivate(player.SteamID, Perm_Seviye2) == false)
                    {
                        if (ValidateCallerPlayer(player) == false)
                        {
                            player.PrintToChat($"{Prefix}{CC.W} Yalnızca bir seçeneğe oy verebilirsin.");
                            return true;
                        }
                    }

                    if (LatestVoteAnswerCommandCalls.Contains(player.SteamID))
                    {
                        player.PrintToChat($"{Prefix} {CC.W}Tekrar oy değiştiremezsin!");
                        return true;
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
                    LatestVoteAnswerCommandCalls.Add(player.SteamID);
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

                        player.PrintToChat($"{Prefix}{CC.Ol} {voted} {CC.G} Seçeneğine oy verdin.");
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