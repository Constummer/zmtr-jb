using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePassPremium_Level16 : BattlePassPremiumBase
    {
        public BattlePassPremium_Level16() : base(16, 999999, 0, 0, "Battle Passi Tamamladın.")
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