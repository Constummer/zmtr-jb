﻿using CounterStrikeSharp.API;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloHeadShotOnlyTG : TeamGamesGameBase
    {
        public SoloHeadShotOnlyTG() : base(TeamGamesSoloChoices.HeadShotOnly)
        {
        }

        internal override void StartGame(Action callback)
        {
            Server.ExecuteCommand($"mp_damage_headshot_only 1");

            base.StartGame(callback);
        }

        internal override void Clear()
        {
            Server.ExecuteCommand($"mp_damage_headshot_only 0");

            base.Clear();
        }
    }
}