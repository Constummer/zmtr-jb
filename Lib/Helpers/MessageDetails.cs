using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static string Prefix { get => $" {CC.LR}[ZMTR]"; }
    public static string NotEnoughPermission { get => $"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor."; }

    private static string AdliAdmin(string adminName) => $"{Prefix} {CC.Ol}{adminName}{CC.W} adlý admin,";

    public static void PrintToCenterHtml(CCSPlayerController player, string msg)
    {
        var @event = new EventShowSurvivalRespawnStatus(false)
        {
            LocToken = msg,
            Duration = 5,
            Userid = player
        };
        @event.FireEvent(false);

        @event = null;
    }

    public static void PrintToCenterHtmlAll(string msg)
    {
        GetPlayers()
            .ToList()
            .ForEach(x =>
            {
                PrintToCenterHtml(x, msg);
                PrintToCenterHtml(x, msg);
                PrintToCenterHtml(x, msg);
                PrintToCenterHtml(x, msg);
            });
    }

    public static void PrintToCenterAll(string msg)
    {
        GetPlayers()
            .ToList()
            .ForEach(x => x.PrintToCenter(msg));
    }
}