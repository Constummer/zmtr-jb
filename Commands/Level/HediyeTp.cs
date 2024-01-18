using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region HediyeTp

    //private static Dictionary<ulong, DateTime> LatestHediyeTpCommandCalls = new Dictionary<ulong, DateTime>();

    //[ConsoleCommand("hediyetp")]
    //[CommandHelper(2, "<oyuncu ismi> <miktar>")]
    //public void HediyeTp(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player, false) == false)
    //    {
    //        return;
    //    }
    //    if (LatestHediyeTpCommandCalls.TryGetValue(player.SteamID, out var call))
    //    {
    //        if (DateTime.UtcNow < call.AddSeconds(10))
    //        {
    //            player.PrintToChat($"{Prefix} {CC.W}Tekrar TP hediye edebilmek için {CC.DR}10 {CC.W}saniye beklemelisin!");
    //            return;
    //        }
    //    }

    //    if (info.ArgCount != 3) return;
    //    var target = info.ArgString.GetArg(0);
    //    if (!int.TryParse(info.ArgString.GetArg(1), out var miktar) || miktar <= 0)
    //    {
    //        player.PrintToChat($"{Prefix} {CC.W}Miktar yanlış!");
    //        return;
    //    }
    //    if (PlayerLevels.ContainsKey(player.SteamID) == false)
    //    {
    //        player.PrintToChat($"{Prefix} {CC.W}TP hediye edebilmek için seviye sisteminde olman gerekli. Bunun için !seviyeol, !slotol yazabilirsin!");
    //        return;
    //    }
    //    PlayerLevels.TryGetValue(player.SteamID, out var data);
    //    if (data.Xp < miktar || data.Xp - miktar < 0)
    //    {
    //        player.PrintToChat($"{Prefix} {CC.W}Yetersiz Bakiye!");
    //        return;
    //    }
    //    var players = GetPlayers()
    //           .Where(x => x.PlayerName.ToLower().Contains(target.ToLower()))
    //           .ToList();
    //    if (players.Count == 0)
    //    {
    //        player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
    //        return;
    //    }
    //    if (players.Count != 1)
    //    {
    //        player.PrintToChat($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
    //        return;
    //    }
    //    var x = players.FirstOrDefault();

    //    if (x?.SteamID != null && x!.SteamID != 0)
    //    {
    //        if (PlayerLevels.TryGetValue(x.SteamID, out var item))
    //        {
    //            item.Xp += miktar;
    //        }
    //        else
    //        {
    //            item = new(x.SteamID);
    //            item.Xp = miktar;
    //        }

    //        PlayerLevels[x.SteamID] = item;
    //        data.Xp -= miktar;
    //        PlayerLevels[player.SteamID] = data;
    //        LatestHediyeTpCommandCalls[player.SteamID] = DateTime.UtcNow;
    //        Server.PrintToChatAll($"{Prefix} {CC.W}{player.PlayerName}, {x.PlayerName} adlı oyuncuya {CC.G}{miktar} {CC.W}TP yolladı!");
    //    }
    //}

    #endregion HediyeTp
}