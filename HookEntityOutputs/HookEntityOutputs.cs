namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void HookEntityOutputs()
    {
        Hook_chicken_OnTakeDamage();
        Hook_all_all();
    }
}