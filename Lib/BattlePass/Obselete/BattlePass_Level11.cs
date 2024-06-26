﻿using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level11 : BattlePassBase
    {
        [JsonIgnore]
        public const int WKill = 25;

        [JsonIgnore]
        public const int NoScopeKill = 20;

        public int CurrentWKill { get; set; } = 0;
        public int CurrentNoScopeKill { get; set; } = 0;

        public BattlePass_Level11() : base(11, 10, 2000, 0)
        {
        }

        internal override void EventNoScopeKill()
        {
            CurrentNoScopeKill++;
            base.EventNoScopeKill();
            CheckIfLevelUp(false);
        }

        internal override void EventWKilled()
        {
            CurrentWKill++;
            base.EventWKilled();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentNoScopeKill >= NoScopeKill &&
                CurrentTime >= Time &&
                CurrentWKill >= WKill)
            {
                base.CheckIfLevelUp(true);
            }
            else
            {
                base.CheckIfLevelUp(false);
            }
        }

        internal override void BuildLevelMenu(CenterHtmlMenu menu)
        {
            base.BuildLevelMenu(menu);
            menu.AddMenuOption($"{CurrentNoScopeKill}/{NoScopeKill} No Scope Kill", null, true);
            menu.AddMenuOption($"{CurrentWKill}/{WKill} Komutçu Kill", null, true);
        }
    }
}