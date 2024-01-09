using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static readonly List<CFuncWall> Cits = new();
    private static List<ulong> CitEnabledPlayers = new();

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
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        Server.PrintToChatAll($"{Prefix}{CC.DR} Çit oluşturma {CC.B}{player.PlayerName} {CC.W}'a açıldı.");
        player.PrintToChat($"{Prefix}{CC.G} Ateş ettiğin yere çit oluşacak.");
        player.PrintToChat($"{Prefix}{CC.G} Kapatmak için !citkapat yaz.");
        player.PrintToChat($"{Prefix}{CC.G} Çitleri silmek çin !cittemizle yaz.");
        CitEnabledPlayers.Add(player.SteamID);
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
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        Server.PrintToChatAll($"{Prefix}{CC.DR} Çit oluşturma {CC.B}{player.PlayerName} {CC.W}'a kapandı.");
        CitEnabledPlayers = CitEnabledPlayers.Where(x => x != player.SteamID).ToList();

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
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        ClearCits();
        return;
    }

    private static void ClearCits(bool displayMsg = false)
    {
        if (Cits != null)
        {
            foreach (var item in Cits)
            {
                if (item.IsValid)
                {
                    item.Remove();
                }
            }
            Cits.Clear();
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
        if (CitEnabledPlayers.Contains(@event.Userid.SteamID) == false)
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
        cit.SetModel("models/props/de_nuke/hr_nuke/chainlink_fence_001/chainlink_fence_001_256_capped.vmdl");
        Cits.Add(cit);
    }
}