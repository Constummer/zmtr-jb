﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloPistolZoomBattleTG : TeamGamesGameBase
    {
        private string SelectedWeaponName = null;
        private Dictionary<ulong, int> PlayerFovs = new Dictionary<ulong, int>();
        public int PlayerCount { get; set; } = 0;

        public SoloPistolZoomBattleTG() : base(TeamGamesSoloChoices.PistolZoomBattle)
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
            PlayerCount = RemoveAllWeapons(giveKnife: false, custom: SelectedWeaponName);
            GetPlayers(CsTeam.Terrorist)
                .Where(x => x.PawnIsAlive)
                .ToList()
                .ForEach(x => PlayerFovs.Add(x.SteamID, 90));

            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            Server.PrintToConsole("1");
            Global?.FovReopenAction(true);
            PlayerFovs?.Clear();
            PlayerCount = 0;
            RemoveAllWeapons(giveKnife: true);
            base.Clear(printMsg);
        }

        internal override void EventWeaponFire(EventWeaponFire @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Userid, false) == false) return;
            if (PlayerFovs.TryGetValue(@event.Userid.SteamID, out var fov))
            {
                if (fov <= 10)
                {
                    fov = 10;
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
            if (ValidateCallerPlayer(@event.Attacker, false) == false) return;

            PlayerCount--;
            PlayerFovs[@event.Attacker.SteamID] = 90;
            Global?.FovAction(@event.Attacker, 90.ToString(), setForce: true, addDic: false);
            if (PlayerCount <= 1)
            {
                Server.PrintToChatAll($"{Prefix} {CC.Or} {@event.Attacker.PlayerName}{CC.W} adlı mahkûm kazandı.");
                PrintToCenterHtmlAll($"{Prefix} {@event.Attacker.PlayerName} adlı mahkûm kazandı.");

                Clear(true);
            }
            base.EventPlayerDeath(@event);
        }
    }
}