using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region OnTeamCommand

    [ConsoleCommand("swap")]
    [CommandHelper(2, "<nick-#userid-@me> <nick-#userid-@me>")]
    public void OnSwapCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye6) == false)
        {
            return;
        }
        if (FindSinglePlayer(player, info.ArgString.GetArg(0), out var x) == false)
        {
            return;
        }
        if (FindSinglePlayer(player, info.ArgString.GetArg(1), out var y) == false)
        {
            return;
        }
        if (x.SteamID == y.SteamID)
        {
            player.PrintToChat($"{Prefix} {CC.W}Aynı oyuncuyu swaplayamazsın.");
            return;
        }

        var swap1res = SwapTargetOne(x, player);
        var swap2res = SwapTargetOne(y, player);
        if (swap1res.Res && swap2res.Res)
        {
            LogManagerCommand(player.SteamID, info.GetCommandString);
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{swap1res.PName} {CC.W} ile {CC.B}{swap2res.PName} {CC.W} swapladı.");
        }
    }

    private (bool Res, string PName) SwapTargetOne(CCSPlayerController x, CCSPlayerController? player)
    {
        Unmuteds = Unmuteds.Where(X => X != x.SteamID).ToList();
        x.VoiceFlags |= VoiceFlags.Muted;

        if (x?.SteamID != null && x!.SteamID != 0)
        {
            if (ValidateCallerPlayer(x, false) == false) return (false, null);
            switch (GetTeam(x))
            {
                case CsTeam.CounterTerrorist:
                    if (ValidateCallerPlayer(x, false) == false) return (false, null);
                    if (x.SteamID == LatestWCommandUser)
                    {
                        RemoveWardenAction(x);
                    }
                    x.ChangeTeam(CsTeam.Terrorist);
                    break;

                case CsTeam.Terrorist:
                    if (CTBanCheck(x) == false)
                    {
                        Server.PrintToChatAll($"{Prefix} {CC.W}{x.PlayerName} CT banı olduğu için CT atılamadı!");
                        return (false, null);
                    }
                    if (ValidateCallerPlayer(x, false) == false) return (false, null);
                    x.ChangeTeam(CsTeam.CounterTerrorist); break;

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
            return (true, x.PlayerName);
        }
        return (false, null);
    }

    #endregion OnTeamCommand
}