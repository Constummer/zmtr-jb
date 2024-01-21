using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloReloadBattleTG : TeamGamesGameBase
    {
        private string SelectedWeaponName = null;
        public List<ulong> PlayerCount { get; set; } = new();
        public CounterStrikeSharp.API.Modules.Timers.Timer? ReloadAmmoTimer { get; set; } = null;

        public SoloReloadBattleTG() : base(TeamGamesSoloChoices.ReloadBattle)
        {
            HasAdditionalChoices = true;
        }

        internal override void AdditionalChoiceMenu(CCSPlayerController player, Action value)
        {
            if (ValidateCallerPlayer(player, false) == false) return;

            var soloTGMenu = new ChatMenu("Silah Menü | Herkes Tek");
            foreach (var item in WeaponMenuHelper.GetGuns())
            {
                soloTGMenu.AddMenuOption(item.Key, (_, _) =>
                {
                    SelectedWeaponName = item.Value;
                    base.AdditionalChoiceMenu(player, value);
                });
            }
            ChatMenus.OpenMenu(player, soloTGMenu);
        }

        internal override void StartGame(Action callback)
        {
            PlayerCount = RemoveAllWeapons(giveKnife: false, custom: SelectedWeaponName);
            GetPlayers(CsTeam.Terrorist)
                .Where(x => x.PawnIsAlive)
                .ToList()
                .ForEach(x =>
                {
                    if (ValidateCallerPlayer(x, false) == false) return;
                    CBasePlayerWeapon? weapon = GetWeapon(x, SelectedWeaponName);
                    if (WeaponIsValid(weapon) == false)
                    {
                        return;
                    }
                    SetAmmo(weapon, 0, 0);
                });

            Global?.AddTimer(0.1f, () =>
             {
                 GetPlayers(CsTeam.Terrorist)
                     .Where(x => x.PawnIsAlive)
                     .ToList()
                     .ForEach(x =>
                     {
                         if (ValidateCallerPlayer(x, false) == false) return;
                         CBasePlayerWeapon? weapon = GetWeapon(x, SelectedWeaponName);
                         if (WeaponIsValid(weapon) == false)
                         {
                             return;
                         }
                         SetAmmo(weapon, 0, 0);
                     });
             });
            ReloadAmmoTimer = Global?.AddTimer(2.5f, () =>
            {
                GetPlayers(CsTeam.Terrorist)
                    .Where(x => x.PawnIsAlive)
                    .ToList()
                    .ForEach(x =>
                    {
                        if (ValidateCallerPlayer(x, false) == false) return;
                        CBasePlayerWeapon? weapon = GetWeapon(x, SelectedWeaponName);
                        if (WeaponIsValid(weapon) == false)
                        {
                            return;
                        }
                        SetAmmo(weapon, 1, 0);
                    });
            }, Full);
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            RemoveAllWeapons(giveKnife: true);
            ReloadAmmoTimer?.Kill();
            PlayerCount = new();
            base.Clear(printMsg);
        }

        internal override void EventPlayerDeath(EventPlayerDeath @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Attacker, false) == false) return;

            if (ValidateCallerPlayer(@event.Userid, false) == false) return;
            SoloCheckGameFinished(this, @event.Userid.SteamID, PlayerCount, @event.Attacker.PlayerName);

            base.EventPlayerDeath(@event);
        }

        internal override void EventPlayerDisconnect(ulong? tempSteamId)
        {
            if (tempSteamId == null) return;
            SoloCheckGameFinished(this, tempSteamId.Value, PlayerCount, null);

            base.EventPlayerDisconnect(tempSteamId);
        }
    }
}