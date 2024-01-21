﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiPistolZoomBattleTG : TeamGamesGameBase
    {
        private string SelectedWeaponName = null;
        private Dictionary<ulong, int> PlayerFovs = new Dictionary<ulong, int>();
        public Dictionary<int, int> PlayerCount { get; set; } = new();

        public MultiPistolZoomBattleTG() : base(TeamGamesMultiChoices.PistolZoomBattle)
        {
            HasAdditionalChoices = true;
        }

        internal override void AdditionalChoiceMenu(CCSPlayerController player, Action value)
        {
            if (ValidateCallerPlayer(player, false) == false) return;

            var soloTGMenu = new ChatMenu("Silah Menü | Takımlı");
            foreach (var item in WeaponMenuHelper.GetPistols())
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
            Global?.FovKapaAction(null, true);
            PlayerFovs?.Clear();
            RemoveAllWeapons(giveKnife: false, custom: SelectedWeaponName);

            GetPlayers(CsTeam.Terrorist)
                .Where(x => x.PawnIsAlive)
                .ToList()
                .ForEach(x => PlayerFovs.Add(x.SteamID, 90));
            PlayerCount = GetTeamPlayerCounts();

            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            Server.PrintToConsole("1");
            Global?.FovReopenAction(true);
            PlayerFovs?.Clear();
            PlayerCount?.Clear();
            RemoveAllWeapons(giveKnife: true);
            base.Clear(printMsg);
        }

        internal override void EventWeaponFire(EventWeaponFire @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Userid, false) == false) return;
            if (PlayerFovs.TryGetValue(@event.Userid.SteamID, out var fov))
            {
                if (fov <= 20)
                {
                    fov = 20;
                }
                else
                {
                    fov -= 10;
                }
                PlayerFovs[@event.Userid.SteamID] = fov;
                Global?.FovAction(@event.Userid, fov.ToString(), setForce: true, addDic: false);
            }
            base.EventWeaponFire(@event);
        }

        internal override void EventPlayerDeath(EventPlayerDeath @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Userid, false) == false) return;

            MultiCheckGameFinished(this, @event.Userid.SteamID, PlayerCount, () =>
            {
                PlayerFovs[@event.Attacker.SteamID] = 90;
                Global?.FovAction(@event.Attacker, 90.ToString(), setForce: true, addDic: false);
            });

            base.EventPlayerDeath(@event);
        }

        internal override void EventPlayerDisconnect(ulong? tempSteamId)
        {
            if (tempSteamId == null) return;
            MultiCheckGameFinished(this, tempSteamId.Value, PlayerCount);
            base.EventPlayerDisconnect(tempSteamId);
        }
    }
}