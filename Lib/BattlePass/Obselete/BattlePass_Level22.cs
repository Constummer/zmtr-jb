﻿using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level22 : BattlePassBase
    {
        [JsonIgnore]
        public const int Sut = 40;

        public int CurrentSut { get; set; } = 0;

        public BattlePass_Level22() : base(22, 10, 3000, 0)
        {
        }

        internal override void OnSutCommand()
        {
            CurrentSut++;
            base.OnSutCommand();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentSut >= Sut &&
                CurrentTime >= Time)
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
            menu.AddMenuOption($"{CurrentSut}/{Sut} Süt Olma", null, true);
        }
    }
}