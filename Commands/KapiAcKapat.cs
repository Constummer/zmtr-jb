﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region KapiAcKapat

    [RequiresPermissions("@css/lider")]
    [ConsoleCommand("kapiac")]
    [ConsoleCommand("kapilariac")]
    [ConsoleCommand("hucrekapiac")]
    public void KapiAc(CCSPlayerController? invoke, CommandInfo command)
    {
        ForceOpenDoor();
    }

    [RequiresPermissions("@css/lider")]
    [ConsoleCommand("kapikapat")]
    [ConsoleCommand("kapikapa")]
    [ConsoleCommand("kapilarikapat")]
    public void KapiKapat(CCSPlayerController? invoke, CommandInfo command)
    {
        ForceCloseDoor();
    }

    private void ForceEntInput(String name, String input, string entityName = null)
    {
        var target = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>(name);

        foreach (var ent in target)
        {
            Server.PrintToChatAll("0 ");
            if (!ent.IsValid)
            {
                continue;
            }
            Server.PrintToChatAll("1 " + ent.Entity.Name);
            if (string.IsNullOrEmpty(entityName) == false)
            {
                if (ent.Entity != null
                && ent.Entity.Name != entityName)
                {
                    continue;
                }
            }
            Server.PrintToChatAll("2 " + ent.Entity.Name);

            ent.AcceptInput(input);
        }
    }

    public void ForceClose(bool sendMsg = false)
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

    public void ForceOpen(bool sendMsg = false)
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

    public void ForceOpenDoor()
    {
        ForceEntInput("func_door", "Open", "kapi2");
        ForceEntInput("func_door", "Open", "kacak");
    }

    public void ForceCloseDoor()
    {
        ForceEntInput("func_door", "Close", "kapi2");
        ForceEntInput("func_door", "Close", "kacak");
    }

    #endregion KapiAcKapat
}