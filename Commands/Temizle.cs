using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Temizle

    [ConsoleCommand("temizle")]
    public void Temizle(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        var target = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("weapon_");

        var playerWeaponIndexes = new List<uint>();
        GetPlayers()
            .Where(x => x.PawnIsAlive)
            .ToList()
            .ForEach(x =>
            {
                playerWeaponIndexes.AddRange(PlayerWeaponIndexes(x));
            });

        foreach (var ent in target)
        {
            if (!ent.IsValid)
            {
                continue;
            }

            if (playerWeaponIndexes.Contains(ent.Index) == false)
            {
                ent.Remove();
            }
        }
    }

    #endregion Temizle
}