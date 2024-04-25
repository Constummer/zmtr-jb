using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("ckapi")]
    public void ckapi(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        var tarEnt = info.ArgString;

        var target = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("func_door");
        var index = uint.Parse(tarEnt);

        foreach (var ent in target)
        {
            if (!ent.IsValid)
            {
                continue;
            }
            if (ent.Index != index)
            {
                continue;
            }
            Server.PrintToChatAll("----------------------------------------");
            Server.PrintToChatAll($"DamageFilterName = {ent.DamageFilterName}");
            Server.PrintToChatAll($"DesignerName = {ent.DesignerName}");
            Server.PrintToChatAll($"Globalname = {ent.Globalname}");
            Server.PrintToChatAll($"UniqueHammerID = {ent.UniqueHammerID}");
            Server.PrintToChatAll($"Index = {ent.Index}");
            if (ent.Blocker.IsValid)
            {
                var bl = ent.Blocker.Value;
                Server.PrintToChatAll($"bl DamageFilterName = {bl.DamageFilterName}");
                Server.PrintToChatAll($"bl DesignerName = {bl.DesignerName}");
                Server.PrintToChatAll($"bl Globalname = {bl.Globalname}");
            }
            if (ent.OwnerEntity.IsValid)
            {
                var bl = ent.OwnerEntity.Value;
                Server.PrintToChatAll($"bl DamageFilterName = {bl.DamageFilterName}");
                Server.PrintToChatAll($"bl DesignerName = {bl.DesignerName}");
                Server.PrintToChatAll($"bl Globalname = {bl.Globalname}");
            }
            Server.PrintToChatAll($"Entity.Name = {ent.Entity.Name}");
            Server.PrintToChatAll($"Entity.DesignerName = {ent.Entity.DesignerName}");

            Server.PrintToChatAll($"GetHashCode = {ent.GetHashCode()}");
            Server.PrintToChatAll("----------------------------------------");
            ent.AcceptInput("Open");
            AddTimer(1, () => ent.AcceptInput("Close"), SOM);
        }
    }

    [ConsoleCommand("cpi")]
    public void cpi(CCSPlayerController? player, CommandInfo inof)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        a?.Kill();
        a = null;
    }

    private CounterStrikeSharp.API.Modules.Timers.Timer a;

    [ConsoleCommand("cp")]
    public void cinput(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        cinputaction("func_door");
    }

    [ConsoleCommand("cpa")]
    public void cinputa(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        cinputaction("func_movelinear");
    }

    [ConsoleCommand("cpb")]
    public void cinputb(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        cinputaction("func_door_rotating");
    }

    [ConsoleCommand("cpc")]
    public void cinputc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        cinputaction("prop_door_rotating");
    }

    [ConsoleCommand("cpd")]
    public void cinputd(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        cinputaction("func_breakable");
    }

    private void cinputaction(string name)
    {
        var target = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>(name);

        var queue = new Queue<CBaseEntity?>();
        foreach (var item in target)
        {
            queue.Enqueue(item);
        }
        a = AddTimer(1, () =>
        {
            if (queue.TryDequeue(out var item))
            {
                if (!item.IsValid)
                {
                    return;
                }
                //
                //
                //rr
                Server.PrintToChatAll(item.Index.ToString());
                item.AcceptInput("Open");
                AddTimer(1, () => item.AcceptInput("Close"), SOM);
            }
        }, Full);
    }

    [ConsoleCommand("cp2")]
    public void cinput2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        var target = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("func_breakable");

        var queue = new Queue<CBaseEntity?>();
        foreach (var item in target)
        {
            queue.Enqueue(item);
        }
        a = AddTimer(10, () =>
        {
            if (queue.TryDequeue(out var ent))
            {
                if (!ent.IsValid)
                {
                    return;
                }
                Server.PrintToChatAll("----------------------------------------");
                Server.PrintToChatAll($"DamageFilterName = {ent.DamageFilterName}");
                Server.PrintToChatAll($"DesignerName = {ent.DesignerName}");
                Server.PrintToChatAll($"Globalname = {ent.Globalname}");
                Server.PrintToChatAll($"UniqueHammerID = {ent.UniqueHammerID}");
                Server.PrintToChatAll($"Index = {ent.Index}");
                if (ent.Blocker.IsValid)
                {
                    var bl = ent.Blocker.Value;
                    Server.PrintToChatAll($"bl DamageFilterName = {bl.DamageFilterName}");
                    Server.PrintToChatAll($"bl DesignerName = {bl.DesignerName}");
                    Server.PrintToChatAll($"bl Globalname = {bl.Globalname}");
                }
                if (ent.OwnerEntity.IsValid)
                {
                    var bl = ent.OwnerEntity.Value;
                    Server.PrintToChatAll($"bl DamageFilterName = {bl.DamageFilterName}");
                    Server.PrintToChatAll($"bl DesignerName = {bl.DesignerName}");
                    Server.PrintToChatAll($"bl Globalname = {bl.Globalname}");
                }
                Server.PrintToChatAll($"Entity.Name = {ent.Entity.Name}");
                Server.PrintToChatAll($"Entity.DesignerName = {ent.Entity.DesignerName}");

                Server.PrintToChatAll($"GetHashCode = {ent.GetHashCode()}");
                Server.PrintToChatAll("----------------------------------------");
                Server.PrintToChatAll(ent.Index.ToString());
                AddTimer(2, () =>
                {
                    Server.PrintToChatAll(ent.Index.ToString());
                    ent.AcceptInput("Break");
                }, SOM);
            }
        }, Full);
    }
}