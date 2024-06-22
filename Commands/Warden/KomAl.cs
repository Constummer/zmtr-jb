using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Komal

    private static bool KomActive = false;
    private static List<ulong> KomAdays = new List<ulong>();
    public static Dictionary<ulong, int> KomAlAnswers = new Dictionary<ulong, int>();
    public static bool KomAlVoteInProgress = false;
    public CounterStrikeSharp.API.Modules.Timers.Timer? KomalPrintTimer = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? KomaEndTimer = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? KomalTimer = null;

    [ConsoleCommand("komaliptal")]
    [ConsoleCommand("komalcancel")]
    [ConsoleCommand("komiptal")]
    [ConsoleCommand("komcancel")]
    public void KomAlIptal(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye10, Perm_Seviye10) == false)
        {
            return;
        }
        KomActive = false;
        KomAdays?.Clear();
        KomAlAnswers?.Clear();
        KomalTimer?.Kill();
        KomalTimer = null;
        KomalPrintTimer?.Kill();
        KomalPrintTimer = null;
        LogManagerCommand(player.SteamID, info.GetCommandString);
        Server.PrintToChatAll($"{Prefix} {CC.DR}{player.PlayerName} {CC.W}Komutçu alımı iptal edildi.");
        Server.PrintToChatAll($"{Prefix} {CC.DR}{player.PlayerName} {CC.W}Komutçu alımı iptal edildi.");
    }

    [ConsoleCommand("komal")]
    public void KomAl(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye10, Perm_Seviye10) == false)
        {
            return;
        }
        KomActive = false;
        KomAdays?.Clear();
        KomAlAnswers?.Clear();
        LogManagerCommand(player.SteamID, info.GetCommandString);
        Server.PrintToChatAll($"{Prefix} {CC.G}Komutçu alımı başladı! Komutçu adayı olmak için !komaday yazın.");
        Server.PrintToChatAll($"{Prefix} {CC.G}Komutçu alımı başladı! Komutçu adayı olmak için !komaday yazın.");
        Server.PrintToChatAll($"{Prefix} {CC.G}Komutçu adaylığından ayrılmak için !komadayiptal yazın.");
        KomActive = true;
        KomaEndTimer?.Kill();
        KomaEndTimer = null;
        KomalPrintTimer?.Kill();
        KomalPrintTimer = null;
        KomalTimer?.Kill();

        var now = DateTime.UtcNow;
        KomalTimer = AddTimer(30f, () =>
        {
            if (KomActive)
            {
                KomActive = false;
                KomAlStartVote();
            }
        }, SOM);
    }

    [ConsoleCommand("komaday")]
    public void KomAday(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (KomActive)
        {
            if (KomAdays.Count > 6)
            {
                KomActive = false;
                player.PrintToChat($"{Prefix} {CC.W}Komaday doldu.");
            }
            else
            {
                if (KomAdays.Any(x => x == player.SteamID))
                {
                    player.PrintToChat($"{Prefix} {CC.W}Zaten katıldın!");
                }
                else
                {
                    KomAdays.Add(player.SteamID);
                    player.VoiceFlags &= ~VoiceFlags.Muted;
                    Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName} {CC.W}komutçu adayı oldu.");
                }
            }
        }
        else
        {
            player.PrintToChat($"{Prefix} {CC.DR}Komutçu alımı aktif değil.");
        }
    }

    [ConsoleCommand("komadayiptal")]
    [ConsoleCommand("komadaysil")]
    [ConsoleCommand("komadaykaldir")]
    public void KomAdayIptal(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, false, null, null) == false)
        {
            return;
        }
        player.VoiceFlags |= VoiceFlags.Muted;
        KomAdays = KomAdays.Where(x => x != player.SteamID).ToList();
        Server.PrintToChatAll($"{Prefix} {CC.DR}{player.PlayerName} {CC.W}Komutçu adaylığından ayrıldı.");
    }

    private void KomAlStartVote()
    {
        if (KomAdays != null & KomAdays.Any() == true)
        {
            if (KomAdays.Count == 1)
            {
                var kvp = KomAdays[0];

                var x = Utilities.GetPlayerFromSteamId(kvp);

                if (ValidateCallerPlayer(x, false))
                {
                    if (GetTeam(x) != CsTeam.CounterTerrorist)
                    {
                        x.SwitchTeam(CsTeam.CounterTerrorist);
                    }
                }
            }
            else
            {
                var voters = GetPlayers().Where(x => KomAdays.Contains(x.SteamID));

                Server.PrintToChatAll($"{Prefix} {CC.G} Komaday süre bitti, oylama başladı.");
                var komalVoteMenu = new ChatMenu("KomAl aday oylamasi");
                KomAlVoteInProgress = true;
                AlreadyVotedPlayers?.Clear();
                foreach (var voter in voters)
                {
                    KomAlAnswers?.Add(voter.SteamID, 0);

                    komalVoteMenu.AddMenuOption(voter.PlayerName, (x, i) =>
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
                var players = GetPlayers();
                foreach (var x in players)
                {
                    if (ValidateCallerPlayer(x, false) == false) continue;
                    MenuManager.OpenChatMenu(x, komalVoteMenu);
                }
                var i = 0;
                KomalPrintTimer?.Kill();

                KomalPrintTimer = AddTimer(0.1f, () =>
                {
                    i++;
                    var hmtl = $"<b>Oylama: <font color='#00FF00'>Komutçu Al Oylaması</font><br>" +
                                $" Kalan Süre : <font color='{((int)((150 - i) / 10) > 3 ? "#00FF00" : "#FF0000")}'> {(int)((150 - i) / 10)}</font><br>" +
                string.Join("<br>", KomAlAnswers?.Select((x, i) => $"!{i + 1} - {voters.Where(y => y.SteamID == x.Key).Select(y => y.PlayerName).FirstOrDefault()} - {x.Value}")) +
                                   $"</b>";
                    if (i > 151)
                    {
                        KomalPrintTimer?.Kill();
                        KomalPrintTimer = null;
                        return;
                    }
                    GetPlayers()
                    .ToList()
                    .ForEach(x => PrintToCenterHtml(x, hmtl));
                }, Full);

                KomaEndTimer = AddTimer(15, () =>
                {
                    if (KomAlVoteInProgress)
                    {
                        Server.PrintToChatAll($" {CC.R} Komaday sonuçları");

                        var list = KomAlAnswers?.OrderByDescending(x => x.Value)?.ToList() ?? new();

                        foreach (var kvp in list)
                        {
                            var votedPlayer = GetPlayers().Where(x => x.SteamID == kvp.Key).FirstOrDefault();
                            if (votedPlayer != null)
                            {
                                if (ValidateCallerPlayer(votedPlayer, false) == false) continue;
                                Server.PrintToChatAll($"{votedPlayer.PlayerName} - {kvp.Value}");
                            }
                        }

                        KomalPrintTimer?.Kill();
                        KomalPrintTimer = null;
                        AlreadyVotedPlayers?.Clear();
                        KomAlAnswers.Clear();
                        KomAlVoteInProgress = false;
                        KomalTimer?.Kill();
                        KomalTimer = null;
                        LatestVoteAnswerCommandCalls?.Clear();
                        KomaEndTimer?.Kill();
                        KomaEndTimer = null;

                        var winner = list.FirstOrDefault();
                        if (winner.Value != 0 && winner.Key != 0)
                        {
                            var x = GetPlayers().Where(x => x.SteamID == winner.Key).FirstOrDefault();
                            if (x == null || ValidateCallerPlayer(x, false) == false)
                            {
                                winner = list.Skip(1).FirstOrDefault();
                                x = GetPlayers().Where(x => x.SteamID == winner.Key).FirstOrDefault();
                                if (x == null || ValidateCallerPlayer(x, false) == false)
                                {
                                    winner = list.Skip(2).FirstOrDefault();
                                    x = GetPlayers().Where(x => x.SteamID == winner.Key).FirstOrDefault();
                                    if (x == null || ValidateCallerPlayer(x, false) == false)
                                    {
                                        return;
                                    }
                                }
                            }
                            x.SwitchTeam(CsTeam.CounterTerrorist);
                        }
                    }
                }, SOM);
            }
        }
        else
        {
            Server.PrintToChatAll($"{Prefix} {CC.G} Oylama yapılamıyor, yeterli aday yok.");
        }
    }

    #endregion Komal
}