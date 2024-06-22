using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class TimeReward_Level31 : TimeRewardBase
    {
        public TimeReward_Level31() : base(31, 999999, 0, 0, "Timer Rewards'i Tamamladın.")
        {
        }

        internal override void BuildLevelMenu(CenterHtmlMenu menu)
        {
            menu.AddMenuOption($"Timer Rewards'i Tamamladın", null, true);
        }
    }
}