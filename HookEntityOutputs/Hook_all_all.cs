using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void Hook_all_all()
    {
        HookEntityOutput("*", "*", (output, name, activator, ent, value, delay) =>
        {
            if (name == "OnUnblockedClosing" || name == "OnBlockedOpening")
            {
                if (KapiAcIptal == false)
                {
                    if (ent?.IsValid != true)
                    {
                        return HookResult.Continue;
                    }
                    if (ent.Entity != null)
                    {
                        if (Config.Map.MapConfigDatums
                            .TryGetValue(Server.MapName, out var door) != true
                            || door == null || door.ForceOpenDoor == null)
                        {
                            return HookResult.Continue;
                        }

                        if (ent.Entity.Name != door.ForceOpenDoor)
                        {
                            return HookResult.Continue;
                        }
                    }

                    ent.AcceptInput("Open");
                }
            }
            return HookResult.Continue;
        });
    }
}