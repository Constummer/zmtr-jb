using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static int CreditModifier { get; set; } = 1;

    [ConsoleCommand("2xkrediac")]
    public void Command2xKrediAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        CreditModifier = 2;
        Server.PrintToChatAll($"{Prefix}{CC.B} 2x{CC.W} kredi kazanımı başlamıştır.");
    }

    [ConsoleCommand("3xkrediac")]
    public void Command3xKrediAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        CreditModifier = 3;
        Server.PrintToChatAll($"{Prefix}{CC.B} 3x{CC.W} kredi kazanımı başlamıştır.");
    }

    [ConsoleCommand("4xkrediac")]
    public void Command4xKrediAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        CreditModifier = 4;
        Server.PrintToChatAll($"{Prefix}{CC.B} 4x{CC.W} kredi kazanımı başlamıştır.");
    }

    [ConsoleCommand("xkredikapa")]
    [ConsoleCommand("2xkredikapa")]
    [ConsoleCommand("3xkredikapa")]
    [ConsoleCommand("4xkredikapa")]
    [ConsoleCommand("xkredikapat")]
    [ConsoleCommand("2xkredikapat")]
    [ConsoleCommand("3xkredikapat")]
    [ConsoleCommand("4xkredikapat")]
    public void CommandxKrediAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        CreditModifier = 1;
        Server.PrintToChatAll($"{Prefix}{CC.W} kredi kazanımları normale dönmüştür.");
    }
}