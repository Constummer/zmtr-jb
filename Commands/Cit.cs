using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static SelfAdjustingQueue<CFuncWall> Cits { get; set; } = new(64);
    private static Dictionary<ulong, string> CitEnabledPlayers = new();

    [ConsoleCommand("citac")]
    [ConsoleCommand("citolustur")]
    [ConsoleCommand("cityap")]
    [ConsoleCommand("cit")]
    public void CitAc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (AdminManager.PlayerHasPermissions(player, "@css/lider") == false && LatestWCommandUser != player.SteamID)
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        LogManagerCommand(player.SteamID, info.GetCommandString);
        Server.PrintToChatAll($"{Prefix}{CC.DR}Varsayılan Çit oluşturma {CC.B}{player.PlayerName} {CC.W}'a açıldı.");
        player.PrintToChat($"{Prefix}{CC.G} Ateş ettiğin yere çit oluşacak.");
        player.PrintToChat($"{Prefix}{CC.G} Kapatmak için !citkapat yaz.");
        player.PrintToChat($"{Prefix}{CC.G} Çitleri silmek çin !cittemizle yaz.");
        var path = "models/props/de_nuke/hr_nuke/chainlink_fence_001/chainlink_fence_001_256_capped.vmdl";
        if (CitEnabledPlayers.ContainsKey(player.SteamID))
        {
            CitEnabledPlayers[player.SteamID] = path;
        }
        else
        {
            CitEnabledPlayers.Add(player.SteamID, path);
        }
        return;
    }

    [ConsoleCommand("citkapa")]
    [ConsoleCommand("citkapat")]
    public void CitKapa(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (AdminManager.PlayerHasPermissions(player, "@css/lider") == false && LatestWCommandUser != player.SteamID)
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        LogManagerCommand(player.SteamID, info.GetCommandString);
        Server.PrintToChatAll($"{Prefix}{CC.DR} Çit oluşturma {CC.B}{player.PlayerName} {CC.W}'a kapandı.");
        CitEnabledPlayers.Remove(player.SteamID);

        return;
    }

    [ConsoleCommand("citkapaall")]
    [ConsoleCommand("citkapatall")]
    [ConsoleCommand("citkapaherkes")]
    [ConsoleCommand("citkapatherkes")]
    public void CitKapaAll(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (AdminManager.PlayerHasPermissions(player, "@css/lider") == false && LatestWCommandUser != player.SteamID)
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        LogManagerCommand(player.SteamID, info.GetCommandString);
        Server.PrintToChatAll($"{Prefix}{CC.DR} Çit oluşturma {CC.B}Herkese {CC.W} kapandı.");
        CitEnabledPlayers?.Clear();

        return;
    }

    [ConsoleCommand("cittemizle")]
    [ConsoleCommand("citsil")]
    public void CitTemizle(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (AdminManager.PlayerHasPermissions(player, "@css/lider") == false && LatestWCommandUser != player.SteamID)
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        LogManagerCommand(player.SteamID, info.GetCommandString);
        ClearCits();
        return;
    }

    private static void ClearCits(bool displayMsg = false)
    {
        if (Cits != null)
        {
            foreach (var item in Cits.ToList())
            {
                if (item.IsValid)
                {
                    item.Remove();
                }
            }
            Cits = new(64);
            if (displayMsg)
            {
                Server.PrintToChatAll($"{Prefix}{CC.DR} Tüm çitler silindi.");
            }
        }
    }

    private void CitEkle(EventBulletImpact @event)
    {
        if (ValidateCallerPlayer(@event.Userid) == false && LatestWCommandUser != @event.Userid.SteamID)
            return;
        if (CitEnabledPlayers.ContainsKey(@event.Userid.SteamID) == false)
            return;
        if (CitEnabledPlayers.TryGetValue(@event.Userid.SteamID, out var citPath) == false)
            return;
        CFuncWall? cit = Utilities.CreateEntityByName<CFuncWall>("func_wall");
        if (cit == null)
        {
            return;
        }
        var vector = new Vector(@event.X, @event.Y, @event.Z);

        cit.Teleport(vector, @event.Userid.PlayerPawn.Value.AbsRotation, VEC_ZERO);
        //cit.Health = 1;
        //cit.TakeDamageFlags = TakeDamageFlags_t.DFLAG_FORCE_DEATH;
        //cit.TakesDamage = true;
        cit.DispatchSpawn();

        cit.SetModel(citPath);
        var deq = Cits.Enqueue(cit);
        if (deq != null)
        {
            if (deq.IsValid)
            {
                deq.Remove();
            }
        }
    }
}