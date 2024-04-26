﻿using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level18 : BattlePassBase
    {
        [JsonIgnore]
        public const int CTWin = 40;

        [JsonIgnore]
        public const int AK47Kill = 300;

        public int CurrentCTWin { get; set; } = 0;
        public int CurrentAK47Kill { get; set; } = 0;

        public BattlePass_Level18() : base(18, 200, 2500, 1000)
        {
        }

        internal override void OnRoundCTWinCommand()
        {
            CurrentCTWin++;
            base.OnRoundCTWinCommand();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentTime >= Time && CurrentCTWin >= CTWin)
            {
                base.CheckIfLevelUp(true);
            }
            else
            {
                base.CheckIfLevelUp(false);
            }
        }
    }
}