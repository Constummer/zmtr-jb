namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void CallListeners()
    {
        ListenersOnMapStart();
        ListenersOnTick();
        ListenersOnClientConnected();
    }
}