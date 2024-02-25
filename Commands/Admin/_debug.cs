using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Memory;

//using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Diagnostics;
using System.Drawing;
using System.Net.Security;
using System.Text;
using System.Text.Json;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("parasutsifirla")]
    public void ParachuteSifirla(CCSPlayerController? player, CommandInfo info)
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

    public List<VectorTemp> BulletImpactVectors { get; set; } = new List<VectorTemp>();

    [ConsoleCommand("ceventbulletimpactwithsave")]
    public void ceventbulletimpactwithsave(CCSPlayerController? player, CommandInfo info)
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
        BulletImpactActive = false;
        var data = new
        {
            MapName = Server.MapName,
            Data = BulletImpactVectors
        };
        var serialized = JsonSerializer.Serialize(data,
                        new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        });
        var savePath = Path.Combine(ContentRootPath, $"{Server.MapName}.json");

        File.WriteAllText(savePath, serialized);
        BulletImpactVectors?.Clear();
    }

    [ConsoleCommand("ceventbulletimpact")]
    public void ceventbulletimpact(CCSPlayerController? player, CommandInfo info)
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
        BulletImpactActive = !BulletImpactActive;
        BulletImpactVectors?.Clear();
    }

    private bool BulletImpactActive = false;

    private void DebugBulletImpact(EventBulletImpact @event, GameEventInfo info)
    {
        if (BulletImpactActive)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Userid) == false) return;
            if (!AdminManager.PlayerHasPermissions(@event.Userid, "@css/root")) return;
            BulletImpactVectors.Add(new(@event?.X, @event?.Y, @event?.Z));
            Server.PrintToConsole($"{@event?.X},{@event?.Y},{@event?.Z} - {BulletImpactVectors.Count}");
        }
    }

    [ConsoleCommand("csteamid")]
    public void csteamid(CCSPlayerController? player, CommandInfo info)
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
        Logger.LogInformation(player.SteamID.ToString());
        Logger.LogInformation(player.AuthorizedSteamID?.SteamId64.ToString());
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

    public CounterStrikeSharp.API.Modules.Timers.Timer AimPlayerTimer { get; private set; } = null;

    [ConsoleCommand("caimplayer")]
    public void caimplayer(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        AimPlayerTimer = AddTimer(0.1f, () =>
             {
                 GetPlayers()
                              .Where(x => x.PawnIsAlive
                                         && x.Pawn.Value != null
                                       && GetTargetAction(x, "@aim", player))
                              .ToList()
                              .ForEach(x =>
                              {
                                  player.PrintToChat($"{Prefix} {CC.W}{x.PlayerName} in your aim");
                              });
             }, Full);
    }

    [ConsoleCommand("caimplayeroff")]
    public void caimplayeroff(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        AimPlayerTimer?.Kill();
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

    [ConsoleCommand("cwalk")]
    [CommandHelper(1, "<0/1>")]
    public void cwalk(CCSPlayerController? player, CommandInfo info)
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
        if (info.ArgCount != 2)
        {
            return;
        }
        var target = info.ArgString.GetArg(0);
        int.TryParse(target, out var godOneTwo);
        if (godOneTwo < 0 || godOneTwo > 1)
        {
            player.PrintToChat($"{Prefix}{CC.W} 0 = kapatmak icin, 1 = acmak icin.");
            return;
        }
        switch (godOneTwo)
        {
            case 0:
                if (ValidateCallerPlayer(player, false) == false) return;
                SetMoveType(player, MoveType_t.MOVETYPE_OBSOLETE);
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W} Mahkûmlar için kapadı.");

                break;

            case 1:
                if (ValidateCallerPlayer(player, false) == false) return;
                SetMoveType(player, MoveType_t.MOVETYPE_WALK);
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W} Mahkûmlar için kapadı.");

                break;
        }
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

    [ConsoleCommand("cxray")]
    public void cxray(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
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

    public static Dictionary<int, string> weaponDefindex { get; } = new Dictionary<int, string>
    {
        { 1, "weapon_deagle" },
        { 2, "weapon_elite" },
        { 3, "weapon_fiveseven" },
        { 4, "weapon_glock" },
        { 7, "weapon_ak47" },
        { 8, "weapon_aug" },
        { 9, "weapon_awp" },
        { 10, "weapon_famas" },
        { 11, "weapon_g3sg1" },
        { 13, "weapon_galilar" },
        { 14, "weapon_m249" },
        { 16, "weapon_m4a1" },
        { 17, "weapon_mac10" },
        { 19, "weapon_p90" },
        { 23, "weapon_mp5sd" },
        { 24, "weapon_ump45" },
        { 25, "weapon_xm1014" },
        { 26, "weapon_bizon" },
        { 27, "weapon_mag7" },
        { 28, "weapon_negev" },
        { 29, "weapon_sawedoff" },
        { 30, "weapon_tec9" },
        { 32, "weapon_hkp2000" },
        { 33, "weapon_mp7" },
        { 34, "weapon_mp9" },
        { 35, "weapon_nova" },
        { 36, "weapon_p250" },
        { 38, "weapon_scar20" },
        { 39, "weapon_sg556" },
        { 40, "weapon_ssg08" },
        { 60, "weapon_m4a1_silencer" },
        { 61, "weapon_usp_silencer" },
        { 63, "weapon_cz75a" },
        { 64, "weapon_revolver" },
        { 500, "weapon_bayonet" },
        { 503, "weapon_knife_css" },
        { 505, "weapon_knife_flip" },
        { 506, "weapon_knife_gut" },
        { 507, "weapon_knife_karambit" },
        { 508, "weapon_knife_m9_bayonet" },
        { 509, "weapon_knife_tactical" },
        { 512, "weapon_knife_falchion" },
        { 514, "weapon_knife_survival_bowie" },
        { 515, "weapon_knife_butterfly" },
        { 516, "weapon_knife_push" },
        { 517, "weapon_knife_cord" },
        { 518, "weapon_knife_canis" },
        { 519, "weapon_knife_ursus" },
        { 520, "weapon_knife_gypsy_jackknife" },
        { 521, "weapon_knife_outdoor" },
        { 522, "weapon_knife_stiletto" },
        { 523, "weapon_knife_widowmaker" },
        { 525, "weapon_knife_skeleton" }
    };

    public CParticleSystem? Aura { get; private set; }

    [ConsoleCommand("cweaponGet")]
    public void cweaponGet(CCSPlayerController? player, CommandInfo info)
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
        CBasePlayerWeapon newWeapon = new(player.GiveNamedItem(weaponDefindex[525]));

        Server.NextFrame(() =>
        {
            if (newWeapon == null) return;
            try
            {
                newWeapon.Clip1 = 0;
                newWeapon.ReserveAmmo[0] = 0;
            }
            catch (Exception)
            { }
        });
    }

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

    [ConsoleCommand("cyet1")]
    public void cyet1(CCSPlayerController? player, CommandInfo info)
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
        if (HasLevelPermissionToActivate(player.SteamID, "@css/seviye15"))
        {
            Server.PrintToChatAll("yep");
        }
        else
        {
            Server.PrintToChatAll("nop");
        }
    }

    //[ConsoleCommand("ccentermenu")]
    //public void ccentermenu(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
    //    {
    //        player.PrintToChat(NotEnoughPermission);
    //        return;
    //    }
    //    var bune = new CenterHtmlMenu("test");
    //    bune.AddMenuOption("bune", null, true);
    //    bune.AddMenuOption("bune2", null, false);
    //    bune.AddMenuOption("bune3", (c, i) =>
    //    {
    //        Server.PrintToChatAll("aaa");
    //    }, false);
    //    bune.AddMenuOption("bune4", (c, i) =>
    //    {
    //        Server.PrintToChatAll("bbb");
    //    }, true);
    //    var a = new CenterHtmlMenuInstance(Global, player, bune);
    //    a.Display();
    //}

    [ConsoleCommand("cAllPlayerTimeTracking")]
    public void cAllPlayerTimeTracking(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        foreach (var item in AllPlayerTotalTimeTracking)
        {
            player.PrintToConsole($"{item.Key}|{item.Value}");
        }
        foreach (var item in AllPlayerTTimeTracking)
        {
            player.PrintToConsole($"{item.Key}|{item.Value}");
        }
        foreach (var item in AllPlayerCTTimeTracking)
        {
            player.PrintToConsole($"{item.Key}|{item.Value}");
        }
        foreach (var item in AllPlayerWTimeTracking)
        {
            player.PrintToConsole($"{item.Key}|{item.Value}");
        }
        foreach (var item in AllPlayerWeeklyWTimeTracking)
        {
            player.PrintToConsole($"{item.Key}|{item.Value}");
        }
    }

    [ConsoleCommand("cPlayerTimeTracking")]
    public void cPlayerTimeTracking(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        foreach (var item in PlayerTimeTracking)
        {
            player.PrintToConsole($"{item.Key}|{item.Value.Total}|{item.Value.CTTime}|{item.Value.TTime}|{item.Value.WTime}|{item.Value.WeeklyWTime}|{item.Value.CTTime}");
        }
    }

    [ConsoleCommand("ctparty")]
    public void ctparty(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        //player.PlayerPawn.Value.ObserverServices.ObserverMode = 3;
        //player.PlayerPawn.Value.ObserverServices.ForcedObserverMode = true;
        //Schema.SetSchemaValue<int>(player.PlayerPawn.Value!.ObserverServices!.Handle, "CPlayer_ObserverServices", "m_hObserverTarget", 3);
        //Schema.SetSchemaValue<int>(player.PlayerPawn.Value!.CameraServices!.Handle, "CCSPlayerBase_CameraServices", "m_iFOV", 120);
        //Schema.SetSchemaValue<int>(player.PlayerPawn.Value.ObserverServices.Handle, "CPlayer_ObserverServices", "m_iObserverMode", 1);
    }

    [ConsoleCommand("cPlayerNamesDatas")]
    public void cPlayerNamesDatas(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        foreach (var item in PlayerNamesDatas)
        {
            player.PrintToConsole($"{item.Key}|{item.Value}");
        }
    }

    [ConsoleCommand("cRenderMode")]
    public void cRenderMode(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (Enum.TryParse<RenderMode_t>(info.ArgString.GetArg(0), out var res))
        {
            Server.PrintToChatAll(res.ToString());
            if (player?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
            {
                foreach (var weapon in player.PlayerPawn.Value.WeaponServices!.MyWeapons)
                {
                    if (weapon.Value != null
                        && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                        && weapon.Value.DesignerName != "[null]")
                    {
                        weapon.Value.RenderMode = res;
                        //weapon.Value.Render = Color.FromArgb(0, 0, 0, 0);
                        //weapon.Value.Render = DefaultColor;
                    }
                }
            }
        }

        //public enum RenderMode_t : byte
        //{
        //    kRenderNormal = 0x0,
        //    kRenderTransColor = 0x1,
        //    kRenderTransTexture = 0x2,
        //    kRenderGlow = 0x3,
        //    kRenderTransAlpha = 0x4,
        //    kRenderTransAdd = 0x5,
        //    kRenderEnvironmental = 0x6,
        //    kRenderTransAddFrameBlend = 0x7,
        //    kRenderTransAlphaAdd = 0x8,
        //    kRenderWorldGlow = 0x9,
        //    kRenderNone = 0xA,
        //    kRenderDevVisualizer = 0xB,
        //    kRenderModeCount = 0xC,
        //}
    }

    [ConsoleCommand("chide")]
    public void chide(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        HideWeapons(player);
    }

    [ConsoleCommand("cshow")]
    public void cshow(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        ShowWeapons(player);
    }

    [ConsoleCommand("css_slot1")]
    public void css_slot1(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        Server.PrintToChatAll("css_slot1");
    }

    [ConsoleCommand("slot1")]
    public void slot1(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        Server.PrintToChatAll("slot1");
    }

    [ConsoleCommand("csayte")]
    public void csayte(CCSPlayerController? player, CommandInfo info)
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
        Server.PrintToChatAll(info.ArgString);
    }

    [ConsoleCommand("ctakim")]
    public void takim(CCSPlayerController? player, CommandInfo info)
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
            player!.ChangeTeam(CsTeam.CounterTerrorist);
        }
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
    public void takim3(CCSPlayerController? player, CommandInfo info)
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
            CustomRespawn(player);
        }
    }

    [ConsoleCommand("c")]
    public void c(CCSPlayerController? player, CommandInfo info)
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
        CoinAfterNewCommander();
    }

    [ConsoleCommand("cbozukbu")]
    public void cbozukbu(CCSPlayerController? player, CommandInfo info)
    {
        player.ExecuteClientCommand("bind 1 \"slot1; track_slot1\"");
    }

    [ConsoleCommand("c2")]
    public void c2(CCSPlayerController? player, CommandInfo info)
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
        GetPlayers().ToList().ForEach(x =>
        {
            Logger.LogInformation("{0},{1}", x.Index, x.UserId);
        });
    }

    [ConsoleCommand("csay")]
    public void csay(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/yonetim"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.PrintToChatAll($"{player.PlayerName}: {info.ArgString}");
    }

    [ConsoleCommand("hsay")]
    public void hsay(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/yonetim"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        foreach (var item in GetPlayers().ToList())
        {
            item.PrintToCenter(info.ArgString);
        }
    }

    [ConsoleCommand("hsay2")]
    public void hsay2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/yonetim"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        foreach (var item in GetPlayers().ToList())
        {
            PrintToCenterHtml(item, info.ArgString);
        }
    }

    [ConsoleCommand("hsay3")]
    public void hsay3(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/yonetim"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        foreach (var item in GetPlayers().ToList())
        {
            PrintToCenterHtml(item, info.ArgString);
            item.PrintToCenter(info.ArgString);
        }
    }

    [ConsoleCommand("hsay4")]
    public void hsay4(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/yonetim"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        foreach (var item in GetPlayers().ToList())
        {
            PrintToCenterHtml(item, info.ArgString);
            item.PrintToCenter(info.ArgString);
            item.PrintToCenter(info.ArgString);
        }
    }

    [ConsoleCommand("cons1")]
    public void cons1(CCSPlayerController? player, CommandInfo info)
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
        Console.WriteLine("wardenSet");
        Server.NextFrame(() =>
        {
            var temp = LatestWCommandUser;
            LatestWCommandUser = player.SteamID;
            WardenRefreshPawn();
            ClearLasers();
            CoinAfterNewCommander();
            WardenUnmute();

            CoinGoWanted = true;
            if (temp != LatestWCommandUser)
            {
                WardenEnterSound();
            }
        });
    }

    [ConsoleCommand("cons2")]
    public void cons2(CCSPlayerController? player, CommandInfo info)
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
        CleanTagOnKomutcuAdmin();
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

    [ConsoleCommand("cDoubleSqlHaha")]
    public void cDoubleSqlHaha(CCSPlayerController? player, CommandInfo info)
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

        try
        {
            var watch = Stopwatch.StartNew();
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;", con);

                cmd.Parameters.AddWithValue("@SteamId", player.SteamID);
                cmd.ExecuteNonQuery();
            }
            watch.Stop();
            Server.PrintToChatAll($"{watch.Elapsed.TotalMilliseconds}");
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    [ConsoleCommand("cDoubleSqlHaha2")]
    public void cDoubleSqlHaha2(CCSPlayerController? player, CommandInfo info)
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

        try
        {
            var watch = Stopwatch.StartNew();
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;Delete From `PlayerIsyanTeam` WHERE `SteamId` = @SteamId;", con);

                cmd.Parameters.AddWithValue("@SteamId", player.SteamID);
                cmd.ExecuteNonQuery();
            }
            watch.Stop();
            Server.PrintToChatAll($"{watch.Elapsed.TotalMilliseconds}");
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }
}