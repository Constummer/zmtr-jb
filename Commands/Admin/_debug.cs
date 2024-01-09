using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("parasutsifirla")]
    public void ParachuteSifirla(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
    }

    private bool KapiAcIptal = false;

    [ConsoleCommand("kapiaciptal")]
    public void kapiaciptal(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        KapiAcIptal = !KapiAcIptal;
    }

    [ConsoleCommand("csteamid")]
    public void csteamid(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Logger.LogInformation(player.SteamID.ToString());
        Logger.LogInformation(player.AuthorizedSteamID?.SteamId64.ToString());
    }

    [ConsoleCommand("cdeath")]
    public void cdeath(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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

    [ConsoleCommand("ckapi")]
    public void ckapi(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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
            AddTimer(1, () => ent.AcceptInput("Close"));
        }
    }

    [ConsoleCommand("cpi")]
    public void cpi(CCSPlayerController? player, CommandInfo inof)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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
                AddTimer(1, () => item.AcceptInput("Close"));
            }
        }, CounterStrikeSharp.API.Modules.Timers.TimerFlags.REPEAT);
    }

    [ConsoleCommand("cyet1")]
    public void cyet1(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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

    [ConsoleCommand("cAllPlayerTimeTracking")]
    public void cAllPlayerTimeTracking(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        foreach (var item in AllPlayerTimeTracking)
        {
            player.PrintToConsole($"{item.Key}|{item.Value.Total}|{item.Value.CTTime}|{item.Value.TTime}|{item.Value.WTime}|{item.Value.CTTime}");
        }
    }

    [ConsoleCommand("cPlayerTimeTracking")]
    public void cPlayerTimeTracking(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        foreach (var item in PlayerTimeTracking)
        {
            player.PrintToConsole($"{item.Key}|{item.Value.Total}|{item.Value.CTTime}|{item.Value.TTime}|{item.Value.WTime}|{item.Value.CTTime}");
        }
    }

    [ConsoleCommand("ctparty")]
    public void ctparty(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        foreach (var item in PlayerNamesDatas)
        {
            player.PrintToConsole($"{item.Key}|{item.Value}");
        }
    }

    private static bool LastRSoundDisable = false;

    [ConsoleCommand("consjoy")]
    public void consjoy(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LastRSoundDisable = true;
        RespawnAc(player, info);
        FfAc(player, info);
        Noclip(player, info);
    }

    [ConsoleCommand("css_slot1")]
    public void css_slot1(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.PrintToChatAll("css_slot1");
    }

    [ConsoleCommand("slot1")]
    public void slot1(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.PrintToChatAll("slot1");
    }

    [ConsoleCommand("csayte")]
    public void csayte(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.PrintToChatAll(info.ArgString);
    }

    [ConsoleCommand("takim")]
    public void takim(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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

    [ConsoleCommand("c")]
    public void c(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        foreach (var item in GetPlayers().ToList())
        {
            SharpTimerPrintHtml(item, info.ArgString);
        }
    }

    [ConsoleCommand("hsay3")]
    public void hsay3(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        foreach (var item in GetPlayers().ToList())
        {
            SharpTimerPrintHtml(item, info.ArgString);
            item.PrintToCenter(info.ArgString);
        }
    }

    [ConsoleCommand("hsay4")]
    public void hsay4(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        foreach (var item in GetPlayers().ToList())
        {
            SharpTimerPrintHtml(item, info.ArgString);
            item.PrintToCenter(info.ArgString);
            item.PrintToCenter(info.ArgString);
        }
    }

    [ConsoleCommand("cons1")]
    public void cons1(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.GetArg(1);
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
            _ => null
        };
        if (snd == null)
        {
            return;
        }
        player.ExecuteClientCommand($"play {snd}");
    }
}