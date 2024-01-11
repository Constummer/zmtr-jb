using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void HandlerLrMenus(CCSPlayerController? player)
    {
        var lrMenu = new ChatMenu("LR Menü | LR seçimi");
        if (LrDatas.Count == 0)
        {
            player.PrintToChat($"{Prefix}{CC.W} LR devre dışı.");
            return;
        }
        foreach (var item in LrDatas)
        {
            lrMenu.AddMenuOption(item.Text, (_, _) =>
            {
                var gardSelectMenu = new ChatMenu("LR Menü | Gardiyan Seçimi");
                var gards = GetPlayers(CsTeam.CounterTerrorist).ToList();
                if (gards.Any() == false)
                {
                    player.PrintToChat($"{Prefix}{CC.W} Hiç gardiyan bulunmadığı için LR atamazsın!.");

                    return;
                }
                gards.ForEach(gard =>
                {
                    var gardText = string.Empty;
                    if (LatestWCommandUser == gard.SteamID)
                    {
                        gardText = $"{gard.PlayerName} | Komutçu";
                    }
                    else
                    {
                        gardText = $"{gard.PlayerName} | Koruma";
                    }
                    gardSelectMenu.AddMenuOption(gardText, (_, _) =>
                    {
                        if (gard.PawnIsAlive == false)
                        {
                            CustomRespawn(gard);
                        }

                        InitLr(item, player, gard);
                    });
                });
                ChatMenus.OpenMenu(player, gardSelectMenu);
            });
        }

        ChatMenus.OpenMenu(player, lrMenu);
    }

    private void InitLr(LrData item, CCSPlayerController? mahkum, CCSPlayerController gard)
    {
        LrActive = true;
        ActivatedLr = new(mahkum.SteamID, gard.SteamID, item);
        RemoveWeapons(mahkum, true);
        RemoveWeapons(gard, true);
        SetHp(mahkum);
        SetHp(gard);
        ActiveGodMode.Remove(gard.SteamID);
        ActiveGodMode.Remove(mahkum.SteamID);
        Server.PrintToChatAll($"{Prefix}{CC.G}{mahkum.PlayerName}{CC.W}adlı mahkûm,{CC.B}{gard.PlayerName}{CC.W} adlı gardiyana {CC.LY}{item.Text} {CC.W}LR'si başlattı.");
        LrStartSound();
        mahkum.GiveNamedItem("item_assaultsuit");
        gard.GiveNamedItem("item_assaultsuit");

        mahkum.GiveNamedItem(item.WeaponName);
        gard.GiveNamedItem(item.WeaponName);

        CBasePlayerWeapon? mahkumWeapon = GetWeapon(mahkum, item.WeaponName);
        CBasePlayerWeapon? gardWeapon = GetWeapon(gard, item.WeaponName);
        if (LRWeaponIsValid(mahkumWeapon) == false || LRWeaponIsValid(gardWeapon) == false)
        {
            return;
        }
        SetAmmo(mahkumWeapon, 1, 0);
        SetAmmo(gardWeapon, 0, 0);
    }

    private static bool LRWeaponIsValid(CBasePlayerWeapon? weapon) => weapon != null && weapon.IsValid != false;
}