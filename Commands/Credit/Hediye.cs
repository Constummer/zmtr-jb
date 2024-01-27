using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Hediye

    private static Dictionary<ulong, DateTime> LatestHediyeCommandCalls = new Dictionary<ulong, DateTime>();

    [ConsoleCommand("hediye")]
    [CommandHelper(2, "<oyuncu ismi-#userid> <miktar>")]
    public void Hediye(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (LatestHediyeCommandCalls.TryGetValue(player.SteamID, out var call))
        {
            if (DateTime.UtcNow < call.AddSeconds(10))
            {
                player.PrintToChat($"{Prefix} {CC.W}Tekrar kredi hediye edebilmek için {CC.DR}10 {CC.W}saniye beklemelisin!");
                return;
            }
        }

        if (info.ArgCount != 3) return;
        var target = info.ArgString.GetArg(0);
        if (!int.TryParse(info.ArgString.GetArg(1), out var miktar) || miktar <= 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Miktar yanlış!");
            return;
        }
        var data = GetPlayerMarketModel(player.SteamID);
        if (data.Model == null || data.Model.Credit < miktar || data.Model.Credit - miktar < 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Yetersiz Bakiye!");
            return;
        }
        var players = GetPlayers()
               .Where(x => GetTargetAction(x, target, player))
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

        if (x?.SteamID != null && x!.SteamID != 0)
        {
            if (PlayerMarketModels.TryGetValue(x.SteamID, out var item))
            {
                item.Credit += miktar;
            }
            else
            {
                item = new(x.SteamID);
                item.Credit = miktar;
            }

            PlayerMarketModels[x.SteamID] = item;
            data.Model!.Credit -= miktar;
            PlayerMarketModels[player.SteamID] = data.Model;
            LatestHediyeCommandCalls[player.SteamID] = DateTime.UtcNow;
            Server.PrintToChatAll($"{Prefix} {CC.Ol}{player.PlayerName}{CC.W},{CC.B} {x.PlayerName} {CC.W}adlı oyuncuya {CC.G}{miktar} {CC.W}kredi yolladı!");
        }
    }

    #endregion Hediye
}