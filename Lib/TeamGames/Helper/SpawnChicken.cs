using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static uint SpawnChicken(Vector pos)
    {
        return 0;
        var entity = Utilities.CreateEntityByName<CChicken>("chicken");
        if (entity != null && entity.IsValid)
        {
            entity.Teleport(pos, ANGLE_ZERO, VEC_ZERO);
            entity.DispatchSpawn();
            if (entity.IsValid)
            {
                return entity.Index;
            }
        }
        return 0;
    }
}