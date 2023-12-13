using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Hediye
    private static Dictionary<ulong, DateTime> LatestHediyeCall = new Dictionary<ulong, DateTime>();

    [ConsoleCommand("hediye")]
    [CommandHelper(2, "<oyuncu ismi> <miktar>")]
    public void Hediye(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (LatestHediyeCall.TryGetValue(player.SteamID, out var call))
        {
            if (DateTime.UtcNow < call.AddSeconds(10))
            {
                player.PrintToChat($" {ChatColors.LightRed}[ZMTR] 10 saniyede bir hediye gönderebilirsin!");
                return;
            }
        }

        if (info.ArgCount != 3) return;
        var target = info.GetArg(1);
        if (!int.TryParse(info.GetArg(2), out var miktar))
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] Miktar duzgun deil!");
            return;
        }
        var data = GetPlayerMarketModel(player.SteamID);
        if (data.Model == null || data.Model.Credit < miktar || data.Model.Credit - miktar < 0)
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] Yetersiz Bakiye!");
            return;
        }
        var players = GetPlayers()
               .Where(x => x.PlayerName.ToLower().Contains(target.ToLower()))
               .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] Eşleşen oyuncu bulunamadı!");
            return;
        }
        if (players.Count != 1)
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] Birden fazla oyuncu bulundu. Devam edilemiyor. İsmi daha iyi belirtin!");
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
            LatestHediyeCall[player.SteamID] = DateTime.UtcNow;
            Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.LightBlue}{player.PlayerName}, {x.PlayerName} oyuncusuna {miktar} kredi yolladı!");
        }
    }

    #endregion Hediye
}