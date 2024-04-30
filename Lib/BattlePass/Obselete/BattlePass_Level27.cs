using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level27 : BattlePassBase
    {
        [JsonIgnore]
        public const int TWin = 200;

        [JsonIgnore]
        public const int SSGKill = 150;

        public int CurrentTWin { get; set; } = 0;
        public int CurrentSSGKill { get; set; } = 0;

        public BattlePass_Level27() : base(27, 10, 3250, 0)
        {
        }

        internal override void EventSSG08Kill()
        {
            CurrentSSGKill++;
            base.EventSSG08Kill();
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
            if (CurrentSSGKill >= SSGKill &&
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
            menu.AddMenuOption($"{CurrentSSGKill}/{SSGKill} SSG-08 Kill", null, true);
        }
    }
}