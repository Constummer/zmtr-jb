using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Kurallar

    [ConsoleCommand("kurallar")]
    public void Kurallar(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        player.PrintToConsole(@"[ZMTR]             --------------------------------------------                                              ");
        player.PrintToConsole(@"[ZMTR]                                                                                                       ");
        player.PrintToConsole(@"[ZMTR]               ________  __       __  ________  _______                                                ");
        player.PrintToConsole(@"[ZMTR]              /        |/  \     /  |/        |/       \                                               ");
        player.PrintToConsole(@"[ZMTR]              $$$$$$$$/ $$  \   /$$ |$$$$$$$$/ $$$$$$$  |                                              ");
        player.PrintToConsole(@"[ZMTR]                  /$$/  $$$  \ /$$$ |   $$ |   $$ |__$$ |                                              ");
        player.PrintToConsole(@"[ZMTR]                 /$$/   $$$$  /$$$$ |   $$ |   $$    $$<                                               ");
        player.PrintToConsole(@"[ZMTR]                /$$/    $$ $$ $$/$$ |   $$ |   $$$$$$$  |                                              ");
        player.PrintToConsole(@"[ZMTR]               /$$/____ $$ |$$$/ $$ |   $$ |   $$ |  $$ |                                              ");
        player.PrintToConsole(@"[ZMTR]              /$$      |$$ | $/  $$ |   $$ |   $$ |  $$ |                                              ");
        player.PrintToConsole(@"[ZMTR]              $$$$$$$$/ $$/      $$/    $$/    $$/   $$/                                               ");
        player.PrintToConsole(@"[ZMTR]                                                                                                       ");
        player.PrintToConsole(@"[ZMTR]                                                                                                       ");
        player.PrintToConsole(@"[ZMTR]      ---------------------------------------------------------                                        ");
        player.PrintToConsole(@"[ZMTR]      KURALLAR                                                                                         ");
        player.PrintToConsole(@"[ZMTR]      1 -  CT KORUMA ALMAK ICIN 8 - 16 - 24 - 32 - 40 - 48 de bir alabilirler                          ");
        player.PrintToConsole(@"[ZMTR]      2 - 1ci kural hakkinda konusmak yok                                                              ");
        player.PrintToConsole(@"[ZMTR]      3 - xd                                                                                           ");
        player.PrintToConsole(@"[ZMTR]                                                                                                       ");
        player.PrintToConsole(@"[ZMTR]                                                                                                       ");
        player.PrintToConsole(@"[ZMTR]                                                                                                       ");
        player.PrintToConsole(@"[ZMTR]                                                                                                       ");
        player.PrintToConsole(@"[ZMTR]                                                                                                       ");
        player.PrintToConsole(@"[ZMTR]                                                                                                       ");
        player.PrintToConsole(@"[ZMTR]                                                                                                       ");
        player.PrintToConsole(@"[ZMTR]                                                                                                       ");
        player.PrintToConsole(@"[ZMTR]                                                                                                       ");
        player.PrintToConsole(@"[ZMTR]                                                                                                       ");
        player.PrintToConsole(@"[ZMTR]      4 - xd                                                                                           ");

        player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Konsolunuzu kontrol ediniz.");
    }

    #endregion Kurallar
}