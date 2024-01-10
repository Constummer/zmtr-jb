namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void CallListeners()
    {
        ListenersOnMapStart();
        ListenersOnTick();
        ListenersOnClientConnected();
        ListenersOnClientDisconnect();
        ListenersOnClientVoice();
        ListenersOnEntityCreated();
    }
}