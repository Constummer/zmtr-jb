using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Surem

    [ConsoleCommand("surem")]
    public void Surem(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var item = (PlayerTime?)null;
        if (player?.SteamID != null && player!.SteamID != 0)
        {
            if (PlayerTimeTracking.TryGetValue(player.SteamID, out item) == false)
            {
                player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
                player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}1 {CC.W}saattir sunucudasın!");
                player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
                return;
            }
        }

        if (item == null)
        {
            player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
            player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}1 {CC.W}saattir sunucudasın!");
            player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
            return;
        }
        player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
        player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}{GetLT120(item.Total)} {CC.W}{GetLT120Str(item.Total)} sunucudasın!");
        player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}{GetLT120(item.CTTime)} {CC.W}{GetLT120Str(item.CTTime)} {CT_LowerPersonal}!");
        player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}{GetLT120(item.TTime)} {CC.W}{GetLT120Str(item.TTime)} {T_LowerPersonal}!");
        player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}{GetLT120(item.WTime)} {CC.W}{GetLT120Str(item.WTime)} komutçusun!");
        player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}{GetLT120(item.KaTime)} {CC.W}{GetLT120Str(item.KaTime)} komutçu adminsin!");
        player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
        player.PrintToChat($"{Prefix} {CC.W}Bu hafta {CC.G}{GetLT120(item.WeeklyTotalTime)} {CC.W}{GetLT120Str(item.WeeklyTotalTime)} sunucudasın!");
        player.PrintToChat($"{Prefix} {CC.W}Bu hafta {CC.G}{GetLT120(item.WeeklyCTTime)} {CC.W}{GetLT120Str(item.WeeklyCTTime)} {CT_LowerPersonal}!");
        player.PrintToChat($"{Prefix} {CC.W}Bu hafta {CC.G}{GetLT120(item.WeeklyTTime)} {CC.W}{GetLT120Str(item.WeeklyTTime)} {T_LowerPersonal}!");
        player.PrintToChat($"{Prefix} {CC.W}Bu hafta {CC.G}{GetLT120(item.WeeklyWTime)} {CC.W}{GetLT120Str(item.WeeklyWTime)} komutçusun!");
        player.PrintToChat($"{Prefix} {CC.W}Bu hafta {CC.G}{GetLT120(item.WeeklyKaTime)} {CC.W}{GetLT120Str(item.WeeklyKaTime)} komutçu adminsin!");
        player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");

        static int GetLT120(int item)
        {
            return item < 120 ? item : item / 60;
        }
        static string GetLT120Str(int item)
        {
            return item < 120 ? "dakikadir" : "saattir";
        }
    }

    [ConsoleCommand("suresine")]
    public void suresine(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Yonetim))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : null;
        if (target == null)
        {
            return;
        }

        GetPlayers()
            .Where(x => x.PlayerName?.ToLower()?.Contains(target?.ToLower()) ?? false
            || GetUserIdIndex(target) == x.UserId
            || x.SteamID.ToString() == target)
            .ToList()
            .ForEach(x =>
            {
                if (AllPlayerTotalTimeTracking.TryGetValue(x.SteamID, out var t))
                {
                    player.PrintToChat($"{x.PlayerName} = {(int)((t) / 60)} saat");
                }
            });
    }

    #endregion Surem
}