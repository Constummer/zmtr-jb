using CounterStrikeSharp.API;
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
    public void KapiAc(CCSPlayerController? player, CommandInfo info)
    {
        Server.PrintToChatAll($"{Prefix} {CC.W}Tüm kapılar açıldı!");
        LogManagerCommand(player.SteamID, info.GetCommandString);
        ForceOpenDoor();
    }

    [RequiresPermissions("@css/lider")]
    [ConsoleCommand("kapikapat")]
    [ConsoleCommand("kapikapa")]
    [ConsoleCommand("kapilarikapat")]
    public void KapiKapat(CCSPlayerController? player, CommandInfo info)
    {
        Server.PrintToChatAll($"{Prefix} {CC.W}Tüm kapılar kapandı!");
        LogManagerCommand(player.SteamID, info.GetCommandString);
        ForceCloseDoor();
    }

    private void ForceEntInput(String name, String input, string entityName = null)
    {
        var target = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>(name);

        foreach (var ent in target)
        {
            if (!ent.IsValid)
            {
                continue;
            }
            if (string.IsNullOrEmpty(entityName) == false)
            {
                if (ent.Entity != null
                && ent.Entity.Name != entityName)
                {
                    continue;
                }
            }
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
        if (Config.Map.MapConfigDatums.TryGetValue(Server.MapName, out var list)
            && list != null && list.KapiAcKapaList != null && list.KapiAcKapaList.Count > 0)
        {
            foreach (var item in list.KapiAcKapaList)
            {
                var act = item.Value switch
                {
                    "func_breakable" => "Break",
                    _ => "Open"
                };
                ForceEntInput(item.Value, act, item.Key);
            }
        }
        else
        {
            ForceEntInput("func_door", "Open");
            ForceEntInput("func_movelinear", "Open");
            ForceEntInput("func_door_rotating", "Open");
            ForceEntInput("prop_door_rotating", "Open");
            ForceEntInput("func_breakable", "Break");
        }
    }

    public void ForceCloseDoor()
    {
        if (Config.Map.MapConfigDatums.TryGetValue(Server.MapName, out var list)
            && list != null && list.KapiAcKapaList != null && list.KapiAcKapaList.Count > 0)
        {
            foreach (var item in list.KapiAcKapaList)
            {
                var act = item.Value switch
                {
                    "func_breakable" => "Break",
                    _ => "Close"
                };
                ForceEntInput(item.Value, act, item.Key);
            }
        }
        else
        {
            ForceEntInput("func_door", "Close");
            ForceEntInput("func_movelinear", "Close");
            ForceEntInput("func_door_rotating", "Close");
            ForceEntInput("prop_door_rotating", "Close");
        }
    }

    #endregion KapiAcKapat
}