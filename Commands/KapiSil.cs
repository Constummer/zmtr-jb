using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region KapiAcKapat

    [ConsoleCommand("kapisil")]
    public void kapiSil(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false) return;
        if (!AdminManager.PlayerHasPermissions(player, "@css/sorumlu"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        Server.PrintToChatAll($"{Prefix} {CC.W}Tüm kapılar silindi!");
        LogManagerCommand(player.SteamID, info.GetCommandString);
        ForceRemoveEntity("func_door");
        ForceRemoveEntity("func_movelinear");
        ForceRemoveEntity("func_door_rotating");
        ForceRemoveEntity("prop_door_rotating");
        ForceRemoveEntity("func_breakable");
    }

    private void ForceRemoveEntity(string name)
    {
        var target = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>(name);

        foreach (var ent in target)
        {
            if (!ent.IsValid)
            {
                continue;
            }

            ent.Remove();
        }
    }

    #endregion KapiAcKapat
}