using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;
using static JailbreakExtras.JailbreakExtras;

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

        public BattlePass_Level18() : base(18, 10, 2500, 1000)
        {
        }

        internal override void EventAK47Kill()
        {
            CurrentAK47Kill++;
            base.EventAK47Kill();
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
            if (CurrentAK47Kill >= AK47Kill &&
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
            menu.AddMenuOption($"{CurrentCTWin}/{CTWin} {CT_LowerPositioning} kazanma", null, true);
            menu.AddMenuOption($"{CurrentAK47Kill}/{AK47Kill} AK47 Kill", null, true);
        }
    }
}