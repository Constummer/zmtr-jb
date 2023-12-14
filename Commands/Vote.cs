using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static Dictionary<string, int> Answers = new Dictionary<string, int>();
    public static bool VoteInProgress = false;

    #region Vote

    [ConsoleCommand("vote")]
    [ConsoleCommand("oylama")]
    [CommandHelper(minArgs: 2, usage: "<soru> [... şıklar ...]", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    public void OnVoteCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (command.GetArg(1) == null || command.GetArg(1).Length < 0 || command.ArgCount < 2)
            return;

        if (command.ArgCount > 7)
        {
            player.PrintToChat("Maximum 7 adet şık belirleyebilirsin");
            return;
        }
        Answers.Clear();

        string question = command.GetArg(1);
        int answersCount = command.ArgCount;

        ChatMenu voteMenu = new(question);

        for (int i = 2; i <= answersCount - 1; i++)
        {
            Answers.Add(command.GetArg(i), 0);
            voteMenu.AddMenuOption(command.GetArg(i), HandleVotes);
        }
        Server.PrintToChatAll($"[ZMTR] {(player == null ? "Console" : $"{player.PlayerName} - {question} Oylamasini baslatti")}");

        GetPlayers().ToList().ForEach(x =>
        {
            x.PrintToCenter(player == null ? "Console" : $"{player.PlayerName} - {question} Oylamasi 20 saniye surecek, oy vermeyi unutmayin");
        });

        VoteInProgress = true;

        foreach (var p in GetPlayers())
        {
            ValidateCallerPlayer(p, false);
            if (p == null) continue;
            ChatMenus.OpenMenu(p, voteMenu);
        }
        Server.PrintToChatAll($"[ZMTR] {(player == null ? "Console" : $"{player.PlayerName} - {question} Oylamasi 20 saniye surecek, oy vermeyi unutmayin")}");

        AddTimer(20, () =>
        {
            Server.PrintToChatAll(question + " SORUSUNUN CEVAPLARI");

            foreach (KeyValuePair<string, int> kvp in Answers)
            {
                Server.PrintToChatAll(kvp.Key + " - " + kvp.Value);
            }
            Answers.Clear();
            VoteInProgress = false;
        }, CounterStrikeSharp.API.Modules.Timers.TimerFlags.STOP_ON_MAPCHANGE);

        return;
    }

    internal static void HandleVotes(CCSPlayerController player, ChatMenuOption option)
    {
        if (VoteInProgress)
            Answers[option.Text]++;
    }

    #endregion Vote
}