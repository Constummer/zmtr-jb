using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void Hook_chicken_OnTakeDamage()
    {
        HookEntityOutput("chicken", "OnTakeDamage", (output, name, activator, ent, value, delay) =>
        {
            ActiveTeamGamesGameBase?.OnTakeDamageHook(ent, activator);

            return HookResult.Continue;
        });
    }
}