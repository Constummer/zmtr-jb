namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void CallListeners()
    {
        ListenersOnMapStart();
        //ListenersOnMapEnd();
        ListenersOnTick();
        ListenersOnClientConnected();
        ListenersOnClientDisconnect();
        ListenersOnClientVoice();
        ListenersOnEntityCreated();
    }
}