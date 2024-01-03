using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static Dictionary<string, int> Answers = new Dictionary<string, int>();
    public static bool VoteInProgress = false;
    public CounterStrikeSharp.API.Modules.Timers.Timer VoteTimer = null;
    public ChatMenu LatestVoteMenu = null;

    #region Vote

    [ConsoleCommand("vote")]
    [ConsoleCommand("oyla")]
    [ConsoleCommand("oylama")]
    [CommandHelper(minArgs: 2, usage: "<soru> [... şıklar ...]", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    public void OnVoteCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye26"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (LatestVoteMenu != null)
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Mevcut oylama bitmeden yeni oylama açamazsın.");
            return;
        }

        if (string.IsNullOrWhiteSpace(command.ArgString))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} En az 2 şık ve 1 soru belirlemelisin, şıklar arası boşluk bırakmalısın.");
            return;
        }
        var argCount = command.ArgString.Split(" ");
        argCount = argCount.Select(x => x.Trim()).Where(x => string.IsNullOrWhiteSpace(x) == false).ToArray();

        if (argCount.Length == 0)
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} En az 2 şık ve 1 soru belirlemelisin, şıklar arası boşluk bırakmalısın.");
            return;
        }
        if (argCount?.Length < 3)
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} En az 2 şık ve 1 soru belirlemelisin, şıklar arası boşluk bırakmalısın.");
            return;
        }
        if (argCount?.Length > 7)
        {
            player!.PrintToChat($" {CC.LR}[ZMTR]{CC.R} Maximum 5 adet şık belirleyebilirsin");
            return;
        }
        Answers.Clear();

        string question = argCount.FirstOrDefault();
        var answers = argCount.Skip(1).Distinct();
        LatestVoteMenu = new(question);

        foreach (var item in answers)
        {
            Answers.Add(item, 0);
            LatestVoteMenu.AddMenuOption(item, (x, option) =>
            {
                if (VoteInProgress)
                {
                    Answers[option.Text]++;
                    if (ValidateCallerPlayer(x, false) == true)
                    {
                        x.PrintToChat($" {CC.LR}[ZMTR]{CC.B} {option.Text} {CC.W} Seçeneğine oy verdin.");
                    }
                }
            });
        }
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {(player == null ? "Console" : $"{CC.B}{player.PlayerName} - {CC.G}{question} Oylamasını başlattı!")}");

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
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {(player == null ? "Console" : $"{CC.B}{player.PlayerName} - {CC.G}{question} Oylaması 20 saniye sürecek, oy vermeyi unutmayın!")}");

        VoteTimer = AddTimer(20, () =>
          {
              Server.PrintToChatAll(question + $" {CC.R}SORUSUNUN YANITLARI");

              foreach (KeyValuePair<string, int> kvp in Answers)
              {
                  Server.PrintToChatAll($" {CC.LR}[ZMTR]{CC.B} {kvp.Key} {CC.W} - {CC.Y}{kvp.Value}");
              }
              Answers.Clear();
              VoteInProgress = false;
              LatestVoteMenu = null;
          }, TimerFlags.STOP_ON_MAPCHANGE);

        return;
    }

    [ConsoleCommand("cancelvote")]
    public void CancelVote(CCSPlayerController? player, CommandInfo command)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye26"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Answers?.Clear();

        VoteInProgress = false;

        VoteTimer?.Kill();
        VoteTimer = null;
        Server.PrintToChatAll($" {CC.LR}[ZMTR]{CC.W} OYLAMA IPTAL EDILDI.");

        return;
    }

    private bool VoteInProgressIntercepter(CCSPlayerController player, string arg)
    {
        if (VoteInProgress)
        {
            Console.WriteLine("a====" + arg);
            var voteNoStr = arg.Substring(1);
            Console.WriteLine("b====" + voteNoStr);

            if (int.TryParse(voteNoStr, out var voteNo))
            {
                if (voteNo < 1 || voteNo > 5)
                {
                    return false;
                }
                Console.WriteLine("c====" + voteNo);

                if (Answers.Count > voteNo)
                {
                    Console.WriteLine("d====" + Answers.Count);

                    return false;
                }
                Console.WriteLine("e====" + true);

                var answers = Answers.ToList();

                var data = answers[voteNo - 1];
                Console.WriteLine("f====" + data.Key);
                Answers[data.Key]++;

                if (ValidateCallerPlayer(player, false) == true)
                {
                    player.PrintToChat($" {CC.LR}[ZMTR]{CC.B} {data.Key} {CC.W} Seçeneğine oy verdin.");
                }
                return true;
            }
        }
        return false;
    }

    #endregion Vote
}