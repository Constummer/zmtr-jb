using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void SayAndSayTeamCommandListener()
    {
        AddCommandListener("say", (player, info) => OnSayOrSayTeam(player, info));
        AddCommandListener("say_team", (player, info) => OnSayOrSayTeam(player, info));
    }

    public HookResult OnSayOrSayTeam(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null) return HookResult.Continue;
        if (info.GetArg(1).StartsWith("!") || info.GetArg(1).StartsWith("/"))
        {
            return HookResult.Continue;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return HookResult.Continue;
        }
        var teamColor = GetTeam(player) switch
        {
            CsTeam.CounterTerrorist => CC.BG,
            CsTeam.Terrorist => CC.Y,
            CsTeam.Spectator => CC.LP,
            CsTeam.None => CC.Or,
        };
        var chatColor = GetTeam(player) switch
        {
            CsTeam.CounterTerrorist => CC.BG,
            CsTeam.Terrorist => CC.Y,
            CsTeam.Spectator => CC.P,
            CsTeam.None => CC.Or,
        };
        if (KomutcuAdminId == player.SteamID)
        {
            Server.PrintToChatAll($" {CC.P}[Komutçu Admin] {teamColor}{player.PlayerName} {CC.W}: {chatColor}{info.GetArg(1)}");
            return HookResult.Handled;
        }
        return HookResult.Continue;
    }
}