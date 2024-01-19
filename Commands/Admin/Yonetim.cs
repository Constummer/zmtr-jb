using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool LastRSoundDisable = false;

    [ConsoleCommand("consjoy")]
    public void consjoy(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/yonetim"))
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
        ForceOpenDoor();
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
}