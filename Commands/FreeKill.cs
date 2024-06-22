using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region FreeKill

    [ConsoleCommand("fk")]
    [ConsoleCommand("freekill")]
    [CommandHelper(0, "<isim>")]
    public void FreeKill(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (GetTeam(player) != CsTeam.CounterTerrorist)
        {
            return;
        }
        var target = info.ArgString.GetArgSkip(0);
        if (FindSinglePlayer(player, target, out var z) == false)
        {
            if (KilledPlayers.TryGetValue(player.SteamID, out var killedPlayers))
            {
                if (killedPlayers != null && killedPlayers.Count > 0)
                {
                    var killMenu = new ChatMenu("Kill Menu");
                    foreach (var item in killedPlayers.Reverse())
                    {
                        killMenu.AddMenuOption(item.Value, (c, i) =>
                        {
                            if (KilledPlayers.TryGetValue(player.SteamID, out var list))
                            {
                                if (list != null && list.Count > 0)
                                {
                                    var data = list.ToList().Where(x => x.Value == i.Text).ToList() ?? new();
                                    foreach (var item in data)
                                    {
                                        var fkPlayer = GetPlayers().Where(x => x.SteamID == item.Key).FirstOrDefault();
                                        if (fkPlayer != null)
                                        {
                                            if (ValidateCallerPlayer(player, false) == false) return;
                                            if (ValidateCallerPlayer(fkPlayer, false) == false) return;
                                            RespawnPlayer(fkPlayer);
                                            list.Remove(item.Key);
                                            KilledPlayers[player.SteamID] = list;
                                            player.PrintToChat($"{Prefix} {CC.G}{fkPlayer.PlayerName} {CC.W}adlı oyuncuyu canlandırdın.");
                                            Server.PrintToChatAll($"{Prefix} {CC.B}{player.PlayerName} {CC.W}adlı {CT_AllLower}, {CC.Or}{fkPlayer.PlayerName} {CC.W}adlı oyuncuyu canlandırdı.");
                                        }
                                    }
                                }
                            }
                        });
                    }

                    MenuManager.OpenChatMenu(player, killMenu);
                }
            }
            else
            {
                player.PrintToChat($"{Prefix} {CC.W}Zaten kimseyi öldürmedin.");
            }
        }
        else if (KilledPlayers.TryGetValue(player.SteamID, out var killedPlayers))
        {
            if (killedPlayers != null && killedPlayers.Count > 0)
            {
                var fkPlayer = GetPlayers().Where(x => x.SteamID == z.SteamID).FirstOrDefault();
                if (fkPlayer != null)
                {
                    if (ValidateCallerPlayer(player, false) == false) return;
                    if (ValidateCallerPlayer(fkPlayer, false) == false) return;
                    RespawnPlayer(fkPlayer);
                    killedPlayers.Remove(z.SteamID);
                    KilledPlayers[player.SteamID] = killedPlayers;
                    player.PrintToChat($"{Prefix} {CC.G}{fkPlayer.PlayerName} {CC.W}adlı oyuncuyu canlandırdın.");
                    Server.PrintToChatAll($"{Prefix} {CC.B}{player.PlayerName} {CC.W}adlı {CT_AllLower}, {CC.Or}{fkPlayer.PlayerName} {CC.W}adlı oyuncuyu canlandırdı.");
                }
            }
            else
            {
                player.PrintToChat($"{Prefix} {CC.W}Zaten kimseyi öldürmedin.");
            }
        }
        else
        {
            player.PrintToChat($"{Prefix} {CC.W}Zaten kimseyi öldürmedin.");
        }
    }

    #endregion FreeKill
}