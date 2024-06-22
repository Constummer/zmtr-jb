using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level23 : BattlePassBase
    {
        [JsonIgnore]
        public const int Jump = 100_000;

        public int CurrentJump { get; set; } = 0;

        public BattlePass_Level23() : base(23, 10, 3000, 0)
        {
        }

        internal override void OnEventPlayerJump(int jumpAmount)
        {
            CurrentJump += jumpAmount;
            base.OnEventPlayerJump(jumpAmount);
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentJump >= Jump &&
                CurrentTime >= Time)
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
            menu.AddMenuOption($"{CurrentJump}/{Jump} Zıplama", null, true);
        }
    }
}