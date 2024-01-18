namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private CounterStrikeSharp.API.Modules.Timers.Timer QueueProcessTimer()
    {
        return AddTimer(0.1f, () =>
        {
            QueueConsumer.StartConsumeOnConnect();
        }, Full);
    }
}