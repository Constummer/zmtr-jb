using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePassPremium_Level19 : BattlePassPremiumBase
    {
        [JsonIgnore]
        public const int TWin = 200;

        [JsonIgnore]
        public const int M4A4Kill = 250;

        public int CurrentTWin { get; set; } = 0;
        public int CurrentM4A4Kill { get; set; } = 0;

        public BattlePassPremium_Level19() : base(19, 20, 3000, 1000)
        {
        }

        internal override void EventM4A4Kill()
        {
            CurrentM4A4Kill++;
            base.EventM4A4Kill();
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
            if (CurrentM4A4Kill >= M4A4Kill &&
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
            menu.AddMenuOption($"{CurrentM4A4Kill}/{M4A4Kill} M4A4 Kill", null, true);
        }
    }
}