using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public partial class TimeRewardBase
    {
        internal void CheckIfLevelUp()
        {
            UpdateTimeRewardData(this, CurrentTime >= Time);
        }

        internal virtual void BuildLevelMenu(CenterHtmlMenu menu)
        {
            menu.AddMenuOption($"{(int)(CurrentTime / 60)}/{(int)(Time / 60)} Saat", null, true);
        }
    }
}