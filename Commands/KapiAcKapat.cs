using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region KapiAcKapat

    [RequiresPermissions("@css/lider")]
    [ConsoleCommand("kapiac")]
    [ConsoleCommand("kapilariac")]
    public void KapiAc(CCSPlayerController? invoke, CommandInfo command)
    {
        ForceOpen();
    }

    [RequiresPermissions("@css/lider")]
    [ConsoleCommand("kapikapat")]
    [ConsoleCommand("kapilarikapat")]
    public void KapiKapat(CCSPlayerController? invoke, CommandInfo command)
    {
        ForceClose();
    }

    private static void ForceEntInput(String name, String input)
    {
        var target = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>(name);

        foreach (var ent in target)
        {
            if (!ent.IsValid)
            {
                continue;
            }
            ent.AcceptInput(input);
        }
    }

    public static void ForceClose(bool sendMsg = true)
    {
        if (sendMsg)
        {
            Server.PrintToChatAll($"{Prefix} {CC.W}Tüm kapılar kapandı!");
        }

        ForceEntInput("func_door", "Close");
        ForceEntInput("func_movelinear", "Close");
        ForceEntInput("func_door_rotating", "Close");
        ForceEntInput("prop_door_rotating", "Close");
    }

    public static void ForceOpen(bool sendMsg = true)
    {
        if (sendMsg)
        {
            Server.PrintToChatAll($"{Prefix} {CC.W}Tüm kapılar açıldı!");
        }

        ForceEntInput("func_door", "Open");
        ForceEntInput("func_movelinear", "Open");
        ForceEntInput("func_door_rotating", "Open");
        ForceEntInput("prop_door_rotating", "Open");
        ForceEntInput("func_breakable", "Break");
    }

    #endregion KapiAcKapat
}