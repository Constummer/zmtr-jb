using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region OnTeamCommand

    [ConsoleCommand("team")]
    [ConsoleCommand("takim")]
    [CommandHelper(0, "<nick-#userid-@me> <ct-t-spec-1-2-3>")]
    public void OnTeamCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye6") == false)
        {
            return;
        }
        if (info.ArgCount != 3) return;
        var targetPlayer = info.ArgString.GetArg(0);
        var targetArgument = GetTargetArgument(targetPlayer);
        var arg = info.ArgString.GetArg(1);
        var targetTeam = arg switch
        {
            "ct" => CsTeam.CounterTerrorist,
            "CT" => CsTeam.CounterTerrorist,
            "cT" => CsTeam.CounterTerrorist,
            "Ct" => CsTeam.CounterTerrorist,
            "3" => CsTeam.CounterTerrorist,
            "t" => CsTeam.Terrorist,
            "T" => CsTeam.Terrorist,
            "2" => CsTeam.Terrorist,
            "spec" => CsTeam.Spectator,
            "SPEC" => CsTeam.Spectator,
            "1" => CsTeam.Spectator,
            _ => CsTeam.None
        };

        if (targetTeam == CsTeam.None)
        {
            player.PrintToChat($"{Prefix} {CC.W}HEDEF yanlış!");
            return;
        }
        if (targetTeam == CsTeam.Spectator)
        {
            if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
            {
                player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor, Spec yasakli.");
                return;
            }
        }
        var players = GetPlayers()
               .Where(x =>
               (targetArgument == TargetForArgument.UserIdIndex
               ? GetUserIdIndex(targetPlayer) == x.UserId : targetArgument == TargetForArgument.Me ? x.SteamID == player.SteamID : false)
                            || (x.PlayerName?.ToLower()?.Contains(targetPlayer?.ToLower()) ?? false))
               .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
            return;
        }
        if (players.Count != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
            return;
        }
        var x = players.FirstOrDefault();
        Unmuteds = Unmuteds.Where(X => X != x.SteamID).ToList();
        x.VoiceFlags |= VoiceFlags.Muted;
        LogManagerCommand(player.SteamID, info.GetCommandString);

        if (x?.SteamID != null && x!.SteamID != 0)
        {
            if (ValidateCallerPlayer(x, false) == false) return;
            switch (targetTeam)
            {
                case CsTeam.Spectator:
                    x.CommitSuicide(false, true);
                    AddTimer(0.3f, () =>
                    {
                        if (ValidateCallerPlayer(x, false) == false) return;
                        x.SwitchTeam(targetTeam);
                    }, SOM);
                    break;

                case CsTeam.CounterTerrorist:
                    if (CTBanCheck(x) == false)
                    {
                        Server.PrintToChatAll($"{Prefix} {CC.W}{x.PlayerName} CT banı olduğu için CT atılamadı!");
                        return;
                    }
                    if (ValidateCallerPlayer(x, false) == false) return;
                    x.ChangeTeam(targetTeam);
                    break;

                case CsTeam.Terrorist:
                    if (x.SteamID == LatestWCommandUser)
                    {
                        RemoveWardenAction(x);
                    }
                    if (ValidateCallerPlayer(x, false) == false) return;
                    x.ChangeTeam(targetTeam); break;

                default: break;
            }
            if (ActiveGodMode.ContainsKey(x.SteamID))
            {
                ActiveGodMode[x.SteamID] = false;
            }
            else
            {
                ActiveGodMode.TryAdd(x.SteamID, false);
            }
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}hedefi {CC.B}{targetTeam.ToString()} {CC.W}takimina gönderdi.");
        }
    }

    #endregion OnTeamCommand
}