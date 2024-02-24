using CounterStrikeSharp.API.Modules.Admin;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public CounterStrikeSharp.API.Modules.Timers.Timer SaveAllParticleDataTimer()
    {
        return AddTimer(360f, () =>
        {
            SaveAllParticleData();
        }, Full);
    }
}