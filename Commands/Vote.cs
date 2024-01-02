﻿using CounterStrikeSharp.API;
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
        if (command.GetArg(1) == null || command.GetArg(1).Length < 0 || command.ArgCount < 2)
            return;

        if (command.ArgCount > 7)
        {
            player!.PrintToChat($" {CC.LR}[ZMTR]{CC.G} Maximum 5 adet şık belirleyebilirsin");
            return;
        }
        Answers.Clear();

        string question = command.GetArg(1);
        int answersCount = command.ArgCount;

        LatestVoteMenu = new(question);

        for (int i = 2; i <= answersCount - 1; i++)
        {
            Answers.Add(command.GetArg(i), 0);
            LatestVoteMenu.AddMenuOption(command.GetArg(i), HandleVotes);
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

    internal static void HandleVotes(CCSPlayerController player, ChatMenuOption option)
    {
        if (VoteInProgress)
        {
            Answers[option.Text]++;
            if (ValidateCallerPlayer(player, false) == true)
            {
                player.PrintToChat($" {CC.LR}[ZMTR]{CC.B} {option.Text} {CC.W} Seçeneğine oy verdin.");
            }
        }
    }

    #endregion Vote
}