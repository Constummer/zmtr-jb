using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level20 : BattlePassBase
    {
        [JsonIgnore]
        public const int TWin = 250;

        [JsonIgnore]
        public const int KnifeKill = 500;

        public int CurrentTWin { get; set; } = 0;
        public int CurrentKnifeKill { get; set; } = 0;

        public BattlePass_Level20() : base(20, 80, 0, 1000)
        {
        }

        internal override void EventKnifeKill()
        {
            CurrentKnifeKill++;
            base.EventKnifeKill();
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
            if (CurrentKnifeKill >= KnifeKill &&
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
            menu.AddMenuOption($"{CurrentKnifeKill}/{KnifeKill} Knife Kill", null, true);
        }
    }
}