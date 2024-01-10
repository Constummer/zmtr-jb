using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Entities;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Cvar

    [ConsoleCommand("css_cvar")]
    [CommandHelper(2, "<cvar> <value>")]
    [RequiresPermissions("@css/cvar")]
    public void OnCvarCommand(CCSPlayerController? caller, CommandInfo command)
    {
        var cvar = ConVar.Find(command.GetArg(1));
        string playerName = caller == null ? "Console" : caller.PlayerName;

        if (cvar == null)
        {
            command.ReplyToCommand($"{Prefix} {CC.W}\"{command.GetArg(1)}\" ayarı bulunamadı.");
            return;
        }

        if (cvar.Name.Equals("sv_cheats") && !AdminManager.PlayerHasPermissions(caller, "@css/cheats"))
        {
            command.ReplyToCommand($"\"{command.GetArg(1)}\" Komutunu degistirme yetkiniz yok");
            return;
        }

        var value = command.GetArg(2);

        Server.ExecuteCommand($"{cvar.Name} {value}");

        Server.PrintToChatAll($"{AdliAdmin(playerName)} {cvar.Name} ayarını {value} olarak değiştirdi.");
        Logger.LogInformation($" {playerName} cvar ayarini {cvar.Name} dan {value} a degistirdi.");
    }

    #endregion Cvar
}