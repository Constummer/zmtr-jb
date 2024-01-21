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
                if (player.PawnIsAlive == false)
                {
                    return;
                }
                var gardSelectMenu = new ChatMenu("LR Menü | Gardiyan Seçimi");
                var gards = GetPlayers(CsTeam.CounterTerrorist).ToList();
                if (gards.Any() == false)
                {
                    player.PrintToChat($"{Prefix}{CC.W} Hiç gardiyan bulunmadığı için LR atamazsın!.");

                    return;
                }
                gards.ForEach(gard =>
                {
                    if (player.PawnIsAlive == false)
                    {
                        return;
                    }
                    var gardText = string.Empty;
                    if (LatestWCommandUser == gard.SteamID)
                    {
                        gardText = $"{gard.PlayerName} | {CC.DB}Komutçu{CC.W}";
                    }
                    else
                    {
                        gardText = $"{gard.PlayerName} | {CC.B}Koruma{CC.W}";
                    }
                    gardSelectMenu.AddMenuOption(gardText, (_, _) =>
                    {
                        if (player.PawnIsAlive == false)
                        {
                            return;
                        }
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
        //BasicCountdown.CommandStartTextCountDown(this, $"{item.Text} {CC.W}LR'si  başlamasına 3 saniye.");
        LrActive = true;
        ActivatedLr = new(mahkum.SteamID, gard.SteamID, item);
        //LRTimer = AddTimer(3f, () =>
        //{
        if (ValidateCallerPlayer(mahkum, false) == false) return;
        if (ValidateCallerPlayer(gard, false) == false) return;
        RemoveWeapons(mahkum, true);
        RemoveWeapons(gard, true);
        if (ValidateCallerPlayer(mahkum, false) == false) return;
        if (ValidateCallerPlayer(gard, false) == false) return;
        SetHp(mahkum);
        SetHp(gard);
        if (ValidateCallerPlayer(mahkum, false) == false) return;
        if (ValidateCallerPlayer(gard, false) == false) return;
        ActiveGodMode.Remove(gard.SteamID);
        ActiveGodMode.Remove(mahkum.SteamID);
        Server.PrintToChatAll($"{Prefix} {CC.G}{mahkum.PlayerName} {CC.W}adlı mahkûm, {CC.B}{gard.PlayerName}{CC.W} adlı gardiyana {CC.LY}{item.Text} {CC.W}LR'si başladı.");
        LrStartSound();
        if (ValidateCallerPlayer(mahkum, false) == false) return;
        if (ValidateCallerPlayer(gard, false) == false) return;
        mahkum.GiveNamedItem("item_assaultsuit");
        gard.GiveNamedItem("item_assaultsuit");
        if (ValidateCallerPlayer(mahkum, false) == false) return;
        if (ValidateCallerPlayer(gard, false) == false) return;
        mahkum.GiveNamedItem(item.WeaponName);
        gard.GiveNamedItem(item.WeaponName);
        if (ValidateCallerPlayer(mahkum, false) == false) return;
        if (ValidateCallerPlayer(gard, false) == false) return;
        GiveRandomSkin(gard);
        GiveRandomSkin(mahkum);
        if (ValidateCallerPlayer(mahkum, false) == false) return;
        if (ValidateCallerPlayer(gard, false) == false) return;
        SetColour(mahkum, System.Drawing.Color.Red);
        SetColour(gard, System.Drawing.Color.Blue);
        switch (item.Choice)
        {
            case LrChoices.None:
                break;

            case LrChoices.Deagle:
                //AddTimer(1f, () =>
                //{
                if (ValidateCallerPlayer(mahkum, false) == false) return;
                CBasePlayerWeapon? mahkumWeapon = GetWeapon(mahkum, item.WeaponName);
                if (WeaponIsValid(mahkumWeapon) == false)
                {
                    return;
                }
                SetAmmo(mahkumWeapon, 1, 0);
                if (ValidateCallerPlayer(gard, false) == false) return;
                CBasePlayerWeapon? gardWeapon = GetWeapon(gard, item.WeaponName);
                if (WeaponIsValid(gardWeapon) == false)
                {
                    return;
                }
                SetAmmo(gardWeapon, 0, 0);
                Global?.AddTimer(0.1f, () =>
                {
                    if (ValidateCallerPlayer(mahkum, false) == false) return;
                    CBasePlayerWeapon? mahkumWeapon = GetWeapon(mahkum, item.WeaponName);
                    if (WeaponIsValid(mahkumWeapon) == false)
                    {
                        return;
                    }
                    SetAmmo(mahkumWeapon, 1, 0);
                    if (ValidateCallerPlayer(gard, false) == false) return;
                    CBasePlayerWeapon? gardWeapon = GetWeapon(gard, item.WeaponName);
                    if (WeaponIsValid(gardWeapon) == false)
                    {
                        return;
                    }
                    SetAmmo(gardWeapon, 0, 0);
                });
                //AddTimer(0.1f, () => SetAmmo(gardWeapon, 0, 0));
                //AddTimer(0.1f, () => SetAmmo(gardWeapon, 0, 0));
                //AddTimer(0.1f, () => SetAmmo(mahkumWeapon, 1, 0));
                //});
                break;

            case LrChoices.NoScopeScout:
                break;

            case LrChoices.NoScopeAwp:
                break;

            default:
                break;
        }
        //}, SOM);
    }
}