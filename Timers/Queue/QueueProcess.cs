using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void QueueProcess()
    {
        AddTimer(0.1f, () =>
        {
            QueueConsumer.StartConsumeOnConnect();
        }, Full);
    }
}