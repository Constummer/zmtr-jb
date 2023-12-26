using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region God

    [ConsoleCommand("god", "godmode a player")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
    public void God(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);

        var targetArgument = GetTargetArgument(target);
        GetPlayers()
               .Where(x => x.PawnIsAlive
                        && GetTargetAction(x, target, player.PlayerName))
               .ToList()
               .ForEach(x =>
               {
                   if (targetArgument == TargetForArgument.None)
                   {
                       Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] Admin, {ChatColors.Green}{x.PlayerName} {ChatColors.White}adlı oyuncuya {ChatColors.Green}god {ChatColors.White}verdi.");
                   }
                   if (ActiveGodMode.TryGetValue(x.SteamID, out var god))
                   {
                       ActiveGodMode[x.SteamID] = !god;
                   }
                   else
                   {
                       ActiveGodMode.TryAdd(x.SteamID, true);
                   }
                   RefreshPawn(x);
               });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] Admin, {ChatColors.Green}{target} {ChatColors.White}hedefine {ChatColors.Green}god {ChatColors.White}verdi.");
        }
    }

    [ConsoleCommand("q", "godmode ct player")]
    public void Q(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetPlayers(CsTeam.CounterTerrorist)
               .ToList()
               .ForEach(x =>
               {
                   ActiveGodMode[x.SteamID] = true;
                   RefreshPawn(x);
               });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] Tüm {ChatColors.Blue}gardiyanlara {ChatColors.Green}god {ChatColors.White}verildi.");
    }

    [ConsoleCommand("qq", "remove godmode ct player")]
    public void Qq(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetPlayers(CsTeam.CounterTerrorist)
               .ToList()
               .ForEach(x =>
               {
                   ActiveGodMode.Remove(x.SteamID);
                   RefreshPawn(x);
               });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] Tüm {ChatColors.Blue}gardiyanların {ChatColors.Green}godunu {ChatColors.White}kaldırdı.");
    }

    private static void GodHurtCover(EventPlayerHurt @event, CCSPlayerController player)
    {
        if (ActiveGodMode.TryGetValue(@event.Userid.SteamID, out var value))
        {
            if (value)
            {
                player.Health = 100;
                player.PlayerPawn.Value!.Health = 100;
                if (player.PawnArmor != 0)
                {
                    player.PawnArmor = 100;
                }
                if (player.PlayerPawn.Value!.ArmorValue != 0)
                {
                    player.PlayerPawn.Value!.ArmorValue = 100;
                }
            }
        }
    }

    #endregion God
}