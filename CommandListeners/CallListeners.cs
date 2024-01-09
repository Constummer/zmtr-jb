using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void CallCommandListeners()
    {
        JointeamCommandListener();
        TeamCommandListener();
        KapilariAcCommandListener();
        SayAndSayTeamCommandListener();
        AddCommandListener("slot1", (player, info) =>
        {
            Logger.LogInformation("aaaaaaaaaaaaaaa");
            return HookResult.Continue;
        });
    }
}