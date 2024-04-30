using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level24 : BattlePassBase
    {
        [JsonIgnore]
        public const int CTWin = 45;

        [JsonIgnore]
        public const int CTKill = 100;

        public int CurrentCTWin { get; set; } = 0;
        public int CurrentCTKill { get; set; } = 0;

        public BattlePass_Level24() : base(24, 10, 0, 1000)
        {
        }

        internal override void EventCTKilled()
        {
            CurrentCTKill++;
            base.EventCTKilled();
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
            if (CurrentTime >= Time &&
                CurrentCTWin >= CTWin &&
                CurrentCTKill >= CTKill)
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
            menu.AddMenuOption($"{CurrentCTWin}/{CTWin} {CT_LowerPositioning} kazanma", null, true);
            menu.AddMenuOption($"{CurrentCTKill}/{CTKill} {CT_CamelCase} Kill", null, true);
        }
    }
}