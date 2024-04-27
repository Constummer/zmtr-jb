using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public partial class BattlePassBase
    {
        internal virtual void CheckIfLevelUp(bool completed) =>
            UpdateBattlePassData(this, completed);

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
    }
}