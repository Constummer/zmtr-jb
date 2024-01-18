using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Donate

    private static Dictionary<ulong, DateTime> LatestDonateCommandCalls = new Dictionary<ulong, DateTime>();

    [ConsoleCommand("donate")]
    [ConsoleCommand("bagis")]
    [CommandHelper(1, "<miktar>")]
    public void Donate(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (LatestDonateCommandCalls.TryGetValue(player.SteamID, out var call))
        {
            if (DateTime.UtcNow < call.AddSeconds(10))
            {
                player.PrintToChat($"{Prefix} {CC.W}Komutçuya tekrar kredi donateleyebilmek için {CC.DR}10 {CC.W}saniye beklemelisin!");
                return;
            }
        }

        if (info.ArgCount != 2) return;
        if (!int.TryParse(info.GetArg(1), out var miktar) || miktar <= 0)
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
               .Where(x => x.SteamID == LatestWCommandUser)
               .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Mevcutta komutçu yok!");
            return;
        }
        if (players.Count != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Mevcutta komutçu yok!");
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
            LatestDonateCommandCalls[player.SteamID] = DateTime.UtcNow;
            Server.PrintToChatAll($"{Prefix} {CC.Ol}{player.PlayerName}{CC.W},{CC.B} {x.PlayerName} {CC.W}adlı komutçuya {CC.G}{miktar} {CC.W}kredi donateledi!");

            PrintToCenterAll($"{player.PlayerName}, {x.PlayerName} adlı komutçuya {miktar} kredi donateledi!");
        }
    }

    #endregion Donate
}