﻿using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePassPremium_Level12 : BattlePassPremiumBase
    {
        [JsonIgnore]
        public const int CTWin = 35;

        [JsonIgnore]
        public const int AWPKill = 35;

        public int CurrentCTWin { get; set; } = 0;
        public int CurrentAWPKill { get; set; } = 0;

        public BattlePassPremium_Level12() : base(12, 10, 0, 500)
        {
        }

        internal override void EventAWPKill()
        {
            CurrentAWPKill++;
            base.EventAWPKill();
            CheckIfLevelUp(false);
        }

        internal override void OnRoundCTWinCommand()
        {
            CurrentCTWin++;
            base.OnRoundCTWinCommand();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentAWPKill >= AWPKill &&
                CurrentTime >= Time &&
                CurrentCTWin >= CTWin)
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
            menu.AddMenuOption($"{CurrentAWPKill}/{AWPKill} AWP Kill", null, true);
            menu.AddMenuOption($"{CurrentCTWin}/{CTWin} {CT_LowerPositioning} kazanma", null, true);
        }
    }
}