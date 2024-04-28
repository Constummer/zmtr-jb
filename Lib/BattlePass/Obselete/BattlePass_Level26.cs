using CounterStrikeSharp.API.Modules.Menu;
using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level26 : BattlePassBase
    {
        [JsonIgnore]
        public const int CTWin = 60;

        [JsonIgnore]
        public const int CtKill = 130;

        public int CurrentCTWin { get; set; } = 0;
        public int CurrentCTKill { get; set; } = 0;

        public BattlePass_Level26() : base(26, 10, 3000, 500)
        {
        }

        internal override void OnRoundCTWinCommand()
        {
            CurrentCTWin++;
            base.OnRoundCTWinCommand();
            CheckIfLevelUp(false);
        }

        internal override void EventCTKilled()
        {
            CurrentCTKill++;
            base.EventCTKilled();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentTime >= Time &&
                CurrentCTWin >= CTWin &&
                CurrentCTKill >= CtKill)
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
            menu.AddMenuOption($"{CurrentCTKill}/{CtKill} {CT_CamelCase} Kill", null, true);
        }
    }
}