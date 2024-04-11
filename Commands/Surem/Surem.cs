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

        var item = (PlayerTime)null;
        if (player?.SteamID != null && player!.SteamID != 0)
        {
            if (PlayerTimeTracking.TryGetValue(player.SteamID, out item) == false)
            {
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
        player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}{(item.Total < 120 ? item.Total : item.Total / 60)} {CC.W}{(item.Total < 120 ? "dakikadir" : "saattir")} sunucudasın!");
        player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}{(item.CTTime < 120 ? item.CTTime : item.CTTime / 60)} {CC.W}{(item.CTTime < 120 ? "dakikadir" : "saattir")} {CT_LowerPersonal}!");
        player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}{(item.TTime < 120 ? item.TTime : item.TTime / 60)} {CC.W}{(item.TTime < 120 ? "dakikadir" : "saattir")} {T_LowerPersonal}!");
        player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}{(item.WTime < 120 ? item.WTime : item.WTime / 60)} {CC.W}{(item.WTime < 120 ? "dakikadir" : "saattir")} komutçusun!");
        player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
        player.PrintToChat($"{Prefix} {CC.W}Bu hafta {CC.G}{(item.WeeklyTotalTime < 120 ? item.WeeklyTotalTime : item.WeeklyTotalTime / 60)} {CC.W}{(item.WeeklyTotalTime < 120 ? "dakikadir" : "saattir")} sunucudasın!");
        player.PrintToChat($"{Prefix} {CC.W}Bu hafta {CC.G}{(item.WeeklyWTime < 120 ? item.WeeklyWTime : item.WeeklyWTime / 60)} {CC.W}{(item.WeeklyWTime < 120 ? "dakikadir" : "saattir")} komutçusun!");
        player.PrintToChat($"{Prefix} {CC.W}Bu hafta {CC.G}{(item.WeeklyTTime < 120 ? item.WeeklyTTime : item.WeeklyTTime / 60)} {CC.W}{(item.WeeklyTTime < 120 ? "dakikadir" : "saattir")} {T_AllLower}!");
        player.PrintToChat($"{Prefix} {CC.W}Bu hafta {CC.G}{(item.WeeklyCTTime < 120 ? item.WeeklyCTTime : item.WeeklyCTTime / 60)} {CC.W}{(item.WeeklyCTTime < 120 ? "dakikadir" : "saattir")} {CT_LowerPersonal}!");
        player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
    }

    [ConsoleCommand("suresine")]
    public void suresine(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/yonetim"))
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