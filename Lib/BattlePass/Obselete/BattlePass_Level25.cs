using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level25 : BattlePassBase
    {
        [JsonIgnore]
        public const int CTWin = 60;

        [JsonIgnore]
        public const int CTKill = 120;

        public int CurrentCTWin { get; set; } = 0;
        public int CurrentCTKill { get; set; } = 0;

        public BattlePass_Level25() : base(25, 10, 0, 0, "Bir Adet Oyuncu Modeli - Dc'den ticket açmalısın.")
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