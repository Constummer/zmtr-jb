using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level31 : BattlePassBase
    {
        public BattlePass_Level31() : base(31, 999999, 0, 0, "Battle Passi Tamamladın.")
        {
        }

        internal override void CheckIfLevelUp(bool completed)
        {
        }

        internal override void BuildLevelMenu(CenterHtmlMenu menu)
        {
            menu.AddMenuOption($"Battle Passi Tamamladın", null, true);
        }
    }
}