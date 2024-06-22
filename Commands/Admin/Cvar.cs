using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Cvars;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Cvar

    [ConsoleCommand("css_cvar")]
    [CommandHelper(2, "<cvar> <value>")]
    [RequiresPermissions("@css/cvar")]
    public void OnCvarCommand(CCSPlayerController? player, CommandInfo info)
    {
        var cvar = ConVar.Find(info.ArgString.GetArg(0));
        string playerName = player == null ? "Console" : player.PlayerName;

        if (cvar == null)
        {
            info.ReplyToCommand($"{Prefix} {CC.W}\"{info.ArgString.GetArg(0)}\" ayarı bulunamadı.");
            return;
        }

        if (cvar.Name.Equals("sv_cheats") && !AdminManager.PlayerHasPermissions(player, "@css/cheats"))
        {
            info.ReplyToCommand($"\"{info.ArgString.GetArg(0)}\" Komutunu degistirme yetkiniz yok");
            return;
        }

        var value = info.ArgString.GetArg(1);
        LogManagerCommand(player.SteamID, info.GetCommandString);

        Server.ExecuteCommand($"{cvar.Name} {value}");

        Server.PrintToChatAll($"{AdliAdmin(playerName)} {cvar.Name} ayarını {value} olarak değiştirdi.");
        Logger.LogInformation($" {playerName} cvar ayarini {cvar.Name} dan {value} a degistirdi.");
    }

    #endregion Cvar
}