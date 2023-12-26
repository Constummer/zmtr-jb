using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static readonly List<CFuncWall> Cits = new();
    private static bool CitEnable { get; set; } = false;

    [ConsoleCommand("citac")]
    [ConsoleCommand("citolustur")]
    [ConsoleCommand("cityap")]
    [ConsoleCommand("cit")]
    public void CitAc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false && LatestWCommandUser != player.SteamID)
        {
            if (!AdminManager.PlayerHasPermissions(player, "@css/adminzmtr"))
            {
                return;
            }
        }
        player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.Green} Ateş ettiğin yere çit oluşacak.");
        player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.Green} Kapatmak için !citkapat yaz.");
        player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.Green} Çitleri silmek çin !cittemizle yaz.");
        CitEnable = true;
        return;
    }

    [ConsoleCommand("citkapa")]
    [ConsoleCommand("citkapat")]
    public void CitKapa(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false && LatestWCommandUser != player.SteamID)
        {
            return;
        }
        player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.Darkred} Çit oluşturma kapandı.");
        CitEnable = false;

        return;
    }

    [ConsoleCommand("cittemizle")]
    [ConsoleCommand("citsil")]
    public void CitTemizle(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false && LatestWCommandUser != player.SteamID)
        {
            return;
        }
        ClearCits();
        return;
    }

    private static void ClearCits()
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
            Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR]{ChatColors.Darkred} Tüm çitler silindi.");
        }
    }

    private void CitEkle(EventBulletImpact @event)
    {
        if (CitEnable == false)
            return;
        if (ValidateCallerPlayer(@event.Userid) == false && LatestWCommandUser != @event.Userid.SteamID)
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