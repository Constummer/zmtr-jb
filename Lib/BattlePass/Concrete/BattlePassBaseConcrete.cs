using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public partial class BattlePassBase
    {
        internal virtual void CheckIfLevelUp()
        {
        }

        internal virtual void EventEntityKilled(EventEntityKilled @event)
        {
        }

        internal virtual void EventPlayerDeath(EventPlayerDeath @event)
        {
        }

        internal virtual void EventWeaponZoom(EventWeaponZoom @event)
        {
        }

        internal virtual void OnTakeDamageHook(CEntityInstance ent, CEntityInstance activator)
        {
        }

        internal virtual void EventWeaponFire(EventWeaponFire @event)
        {
        }

        internal virtual void EventPlayerDisconnect(ulong? tempSteamId)
        {
        }

        internal virtual void EventPlayerHurt(EventPlayerHurt @event)
        {
        }

        internal virtual void OnSutCommand()
        {
        }
    }
}