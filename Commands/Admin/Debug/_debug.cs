﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Menu;

//using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("cwarden")]
    public void cwarden(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.PrintToConsole($"{LatestWCommandUser}");
    }

    [ConsoleCommand("cmarkersifirla")]
    public void cmarkersifirla(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LatestWCommandUser = player.SteamID;
        ClearLasers();
        var xyz = player.PlayerPawn.Value.AbsOrigin;

        CalculateAndPrintEdges(xyz.X, xyz.Y, xyz.Z, 100, 100);
    }

    [ConsoleCommand("cmarkertest")]
    public void cmarkertest(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        ClearLasers();
        var xyz = player.PlayerPawn.Value.AbsOrigin;

        CalculateAndPrintEdges(xyz.X, xyz.Y, xyz.Z, 100, 100);
    }

    [ConsoleCommand("cparticle")]
    public void cparticle(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Aura?.Remove();
        Aura = null;
        //var entity = Utilities.CreateEntityByName<CPhysicsPropMultiplayer>("prop_physics_multiplayer");
        Aura = Utilities.CreateEntityByName<CParticleSystem>("info_particle_system");

        if (Aura != null && Aura.IsValid)
        {
            Aura.EffectName = "particles/test/energy.vpcf";
            //   Aura.EffectName = "particles/testsystems/test_cross_product.vpcf";
            //Aura.EffectName = "particles/ui/status_levels/ui_status_level_8_energycirc.vpcf";
            //Aura.EffectName = "particles/testsystems/test_cross_product.vpcf";
            //Aura.SetModel("models/coop/challenge_coin.vmdl";
            Aura.TintCP = 1;

            Aura.Teleport(player.PlayerPawn.Value.AbsOrigin, ANGLE_ZERO, VEC_ZERO);
            Aura.DispatchSpawn();
            Aura.AcceptInput("Start");
            CustomSetParent(Aura, player.PlayerPawn.Value);
        }
    }

    [ConsoleCommand("cparticlekill")]
    public void cparticlekill(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (Aura != null && Aura.IsValid)
        {
            Aura.AcceptInput("Kill");
        }
    }

    [ConsoleCommand("cthird")]
    public void cthird(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        NativeAPI.IssueClientCommandFromServer(player.Slot, "thirdperson");
    }

    [ConsoleCommand("cthird2")]
    public void cthird2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        NativeAPI.IssueClientCommand(player.Slot, "thirdperson");
    }

    [ConsoleCommand("ccentermenu")]
    public void ccentermenu(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var menu = new CenterHtmlMenu("123456780qwertyuio");
        for (int i = 0; i < 20; i++)
        {
            var key = $"test_{i}";
            menu.AddMenuOption(key, (i, o) =>
            {
                player.PrintToChat(key + " | sectin");
            });
        }
        MenuManager.OpenCenterHtmlMenu(this, player, menu);
    }

    [ConsoleCommand("ccustomcoin")]
    public void ccustomcoin(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var coin = Utilities.CreateEntityByName<CPhysicsPropMultiplayer>("prop_physics_multiplayer");
        if (coin == null)
        {
            return;
        }
        var xyz = player.PlayerPawn.Value.AbsOrigin;
        coin.SetModel("models/coop/challenge_coin.vmdl");
        coin.Teleport(new Vector(xyz.X, xyz.Y, xyz.Z + 100), ANGLE_ZERO, VEC_ZERO);
        coin.DispatchSpawn();
        coin.AcceptInput("Start");
        CustomSetParent(coin, player.PlayerPawn.Value);
    }

    [ConsoleCommand("cstop")]
    public void cstop(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        var target = info.ArgString.GetArg(0);

        GetPlayers()
                     .Where(x => x.PawnIsAlive
                     && x.Pawn.Value != null
                              && GetTargetAction(x, target, player))
                     .ToList()
                     .ForEach(x =>
                     {
                         x.PlayerPawn.Value.VelocityModifier = 0.0f;
                     });
    }

    [ConsoleCommand("cbuneamk")]
    public void cbuneamk(CCSPlayerController? x, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(x, "@css/root"))
        {
            x.PrintToChat(NotEnoughPermission);
            return;
        }
        var target = info.ArgString.GetArg(0);
        var snd = target switch
        {
            "1" => MoveType_t.MOVETYPE_NONE,
            "2" => MoveType_t.MOVETYPE_OBSOLETE,
            "3" => MoveType_t.MOVETYPE_WALK,
            "5" => MoveType_t.MOVETYPE_FLY,
            "6" => MoveType_t.MOVETYPE_FLYGRAVITY,
            "7" => MoveType_t.MOVETYPE_VPHYSICS,
            "8" => MoveType_t.MOVETYPE_PUSH,
            "9" => MoveType_t.MOVETYPE_NOCLIP,
            "10" => MoveType_t.MOVETYPE_OBSERVER,
            "11" => MoveType_t.MOVETYPE_LADDER,
            "12" => MoveType_t.MOVETYPE_CUSTOM,
            "13" => MoveType_t.MOVETYPE_LAST,
            "14" => MoveType_t.MOVETYPE_MAX_BITS,
        };

        SetMoveType(x, snd);

        RefreshPawnTP(x);
    }

    [ConsoleCommand("cdeath")]
    public void cdeath(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Logger.LogInformation(player.AbsOrigin.ToString());
        Logger.LogInformation(player.Pawn.Value.AbsOrigin.ToString());
        Logger.LogInformation(player.PlayerPawn.Value.AbsOrigin.ToString());
        Logger.LogInformation(player.PlayerPawn.Value.CameraServices.Pawn.Value.AbsOrigin.ToString());
    }

    [ConsoleCommand("ctp")]
    public void ctp(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        try
        {
            var a = Schema.GetSchemaValue<byte>(player.PlayerPawn.Value.Handle, "CPlayer_ObserverServices", "m_iObserverMode");
            Schema.SetSchemaValue<byte>(player.Handle, "CPlayer_ObserverServices", "m_iObserverMode", 1);
            //Schema.SetCustomMarshalledType<byte>(player.PlayerPawn.Value.Handle, "CPlayer_ObserverServices", "m_iObserverMode", 1);
        }
        catch (Exception e)
        {
        }
    }

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
            Logger.LogInformation("----------------------------------------");
            Logger.LogInformation($"DamageFilterName = {ent.DamageFilterName}");
            Logger.LogInformation($"DesignerName = {ent.DesignerName}");
            Logger.LogInformation($"Globalname = {ent.Globalname}");
            Logger.LogInformation($"UniqueHammerID = {ent.UniqueHammerID}");
            Logger.LogInformation($"Index = {ent.Index}");
            if (ent.Blocker.IsValid)
            {
                var bl = ent.Blocker.Value;
                Logger.LogInformation($"bl DamageFilterName = {bl.DamageFilterName}");
                Logger.LogInformation($"bl DesignerName = {bl.DesignerName}");
                Logger.LogInformation($"bl Globalname = {bl.Globalname}");
            }
            if (ent.OwnerEntity.IsValid)
            {
                var bl = ent.OwnerEntity.Value;
                Logger.LogInformation($"bl DamageFilterName = {bl.DamageFilterName}");
                Logger.LogInformation($"bl DesignerName = {bl.DesignerName}");
                Logger.LogInformation($"bl Globalname = {bl.Globalname}");
            }
            Logger.LogInformation($"Entity.Name = {ent.Entity.Name}");
            Logger.LogInformation($"Entity.DesignerName = {ent.Entity.DesignerName}");

            Logger.LogInformation($"GetHashCode = {ent.GetHashCode()}");
            Logger.LogInformation("----------------------------------------");
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
        var target = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("func_door");

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

    [ConsoleCommand("cwtf")]
    public void cwtf(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        ForceEntInput("func_breakable", "Break", "DropEldenGidiyeah");
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

    public CParticleSystem? Aura { get; private set; }

    [ConsoleCommand("cbichaha")]
    public void cbichaha(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var knife = GetWeapon(player, "weapon_knife");
        if (WeaponIsValid(knife) == false)
        {
            return;
        }

        Server.NextFrame(() =>
        {
            Server.PrintToChatAll($"11 {knife.FallbackPaintKit}");
            knife.SetModel("models/coop/challenge_coin.vmdl");
            Server.PrintToChatAll($"11 {knife.FallbackPaintKit}");
        });
        Server.PrintToChatAll("1");
    }

    [ConsoleCommand("cbicdeneme")]
    public void cbicdeneme(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var knife = GetWeapon(player, "weapon_knife");
        if (WeaponIsValid(knife) == false)
        {
            return;
        }
        Server.NextFrame(() =>
        {
            knife.SetModel("weapons/models/nozb1/aloneelxy/aloneelxy_freebayonety.vmdl");
        });
        Server.PrintToChatAll("1");
    }

    [ConsoleCommand("cbicdeneme2")]
    public void cbicdeneme2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var knife = GetWeapon(player, "weapon_knife");
        if (WeaponIsValid(knife) == false)
        {
            return;
        }
        Server.NextFrame(() =>
        {
            knife.SetModel("weapons/models/nozb1/aloneelxy/aloneelxy_freebayonety2.vmdl");
        });
        Server.PrintToChatAll("1");
    }

    [ConsoleCommand("cbicdeneme3")]
    public void cbicdeneme3(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var knife = GetWeapon(player, "weapon_knife");
        if (WeaponIsValid(knife) == false)
        {
            return;
        }

        Server.NextFrame(() =>
        {
            knife.SetModel("weapons/models/nozb1/aloneelxy/aloneelxy_freebayonety2.vmdl");
        });
        Server.PrintToChatAll("1");
    }

    [ConsoleCommand("ctparty")]
    [CommandHelper(3, "<pitch> <volume> <delay>")]
    public void ctparty(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        var pitch = info.ArgString.GetArg(0);
        var volume = info.ArgString.GetArg(1);
        var delay = info.ArgString.GetArg(2);
        if (int.TryParse(pitch, out var p) == false
            || int.TryParse(volume, out var v) == false
            || int.TryParse(delay, out var d) == false
            )
        {
            return;
        }
        CBaseEntity_EmitSoundParams(player, "sounds/player/burn_damage3.vsnd_c", p, v, d);
    }

    [ConsoleCommand("ctakim2")]
    public void takim2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (player.PlayerName == "Constummer")
        {
            player!.ChangeTeam(CsTeam.Spectator);
        }
    }

    [ConsoleCommand("ctakim3")]
    public void ctakim3(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (player.PlayerName == "Constummer")
        {
            player!.ChangeTeam(CsTeam.Terrorist);
        }
    }

    [ConsoleCommand("ts")]
    public void testses(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgString.GetArg(0);
        var snd = target switch
        {
            "1" => "sounds/zmtr_warden/wzenter.vsnd_c",
            "2" => "sounds/zmtr_warden/wzleave.vsnd_c",
            "3" => "sounds/lr/lr1.vsnd_c",
            "4" => "sounds/zmtr_freeze/freeze.vsnd_c",
            "5" => "sounds/zmtr/bell.vsnd_c",
            "6" => "sounds/zmtr/karamantukur.vsnd_c",
            "7" => "sounds/mapeadores/saysounds/applause.vsnd_c",
            "8" => "sounds/mapeadores/saysounds/applause2.vsnd_c",
            "9" => "sounds/mapeadores/saysounds/applause3.vsnd_c",
            "10" => "sounds/mapeadores/saysounds/applause4.vsnd_c",
            "11" => "sounds/mapeadores/saysounds/chimp2.vsnd_c",
            "12" => "sounds/mapeadores/saysounds/heheboi.vsnd_c",
            "13" => "sounds/player/burn_damage1.vsnd_c",
            "14" => "sounds/player/burn_damage2.vsnd_c",
            "15" => "sounds/player/burn_damage3.vsnd_c",
            "16" => "sounds/player/burn_damage4.vsnd_c",
            "17" => "sounds/player/burn_damage5.vsnd_c",
            _ => null
        };
        if (snd == null)
        {
            return;
        }
        player.ExecuteClientCommand($"play {snd}");
    }
}