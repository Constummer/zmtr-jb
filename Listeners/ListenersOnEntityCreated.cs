using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnEntityCreated()
    {
        RegisterListener<Listeners.OnEntityCreated>(entity =>
        {
            if (entity == null || entity.Entity == null || !entity.IsValid) return;

            UnlimitedAmmoV2(entity);
        });
    }
}