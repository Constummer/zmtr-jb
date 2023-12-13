namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void CallEvents()
    {
        EventRoundStart();
        EventPlayerDeath();
        EventPlayerHurt();
        EventPlayerSpawn();
    }
}