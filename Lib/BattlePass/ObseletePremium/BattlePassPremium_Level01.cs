﻿using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePassPremium_Level01 : BattlePassPremiumBase
    {
        [JsonIgnore]
        public const int CtKill = 5;

        [JsonIgnore]
        public const int WKill = 1;

        public int CurrentCtKill { get; set; } = 0;
        public int CurrentWKill { get; set; } = 0;

        public BattlePassPremium_Level01() : base(1, 10, 250, 0)
        {
        }

        internal override void EventCTKilled()
        {
            CurrentCtKill++;
            base.EventCTKilled();
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
            if (CurrentCtKill >= CtKill &&
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
            menu.AddMenuOption($"{CurrentCtKill}/{CtKill} {CT_CamelCase} Kill", null, true);
            menu.AddMenuOption($"{CurrentWKill}/{WKill} Komutçu Kill", null, true);
        }
    }
}