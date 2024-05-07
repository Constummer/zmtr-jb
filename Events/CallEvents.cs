namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void CallEvents()
    {
        EventRoundStart();
        EventPlayerJump();
        EventRoundEnd();
        EventPlayerDeath();
        EventPlayerHurt();
        EventPlayerSpawn();
        EventWeaponReload();
        EventPlayerPing();
        EventBulletImpact();
        EventPlayerDisconnect();
        EventPlayerConnectFull();
        EventWeaponZoom();
        EventWeaponFire();
        EventEntityKilled();
        EventBombDropped();
        EventItemPickup();
        EventBombPickup();
        EventCsWinPanelRound();
        //EventSpecTargetUpdated();
    }
}