using CounterStrikeSharp.API.Modules.Menu;
using System.Text.Json.Serialization;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level15 : BattlePassBase
    {
        [JsonIgnore]
        public const int Mag7Kill = 50;

        [JsonIgnore]
        public const int TWin = 150;

        public int CurrentMag7Kill { get; set; } = 0;
        public int CurrentTWin { get; set; } = 0;

        public BattlePass_Level15() : base(15, 10, 2250, 0)
        {
        }

        internal override void EventMAG7Kill()
        {
            CurrentMag7Kill++;
            base.EventMAG7Kill();
            CheckIfLevelUp(false);
        }

        internal override void OnRoundTWinCommand()
        {
            CurrentTWin++;
            base.OnRoundTWinCommand();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentMag7Kill >= Mag7Kill &&
                CurrentTime >= Time &&
                CurrentTWin >= TWin)
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
            menu.AddMenuOption($"{CurrentTWin}/{TWin} {T_LowerPositioning} kazanma", null, true);
            menu.AddMenuOption($"{CurrentMag7Kill}/{Mag7Kill} Mag7 Kill", null, true);
        }
    }
}