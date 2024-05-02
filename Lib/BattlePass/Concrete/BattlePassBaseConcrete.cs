using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public partial class BattlePassBase
    {
        internal virtual void CheckIfLevelUp(bool completed)
        {
            this.Completed = completed;
            UpdateBattlePassData(this, completed);
        }

        internal virtual void EventCTKilled()
        {
        }

        internal virtual void EventTKilled()
        {
        }

        internal virtual void EventWKilled()
        {
        }

        internal virtual void OnSutCommand()
        {
        }

        internal virtual void OnRoundTWinCommand()
        {
        }

        internal virtual void OnRoundCTWinCommand()
        {
        }

        internal virtual void EventNoScopeKill()
        {
        }

        internal virtual void BuildLevelMenu(CenterHtmlMenu menu)
        {
            menu.AddMenuOption($"{(int)(CurrentTime / 60)}/{(int)(Time / 60)} Saat", null, true);
        }

        internal virtual void OnEventPlayerJump()
        {
        }

        internal virtual void EventSSG08Kill()
        {
        }

        internal virtual void EventM4A4Kill()
        {
        }

        internal virtual void EventMAG7Kill()
        {
        }

        internal virtual void EventAWPKill()
        {
        }

        internal virtual void EventAK47Kill()
        {
        }

        internal virtual void EventP90Kill()
        {
        }

        internal virtual void EventKnifeKill()
        {
        }
    }
}