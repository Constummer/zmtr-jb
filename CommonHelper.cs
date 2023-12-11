using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static IEnumerable<CCSPlayerController> GetPlayers(CsTeam? team = null)
    {
        bool changed = false;
        //var players = new List<CCSPlayerController>();
        //for (int i = 1; i < Server.MaxPlayers; i++)
        //{
        //    var ent = NativeAPI.GetEntityFromIndex(i);
        //    if (ent == 0)
        //        continue;

        //    var player = new CCSPlayerController(ent);
        //    if (player == null || !player.IsValid)
        //        continue;
        //    players.Add(player);
        //}
        //return players
        return Utilities.GetPlayers()
             .Where(x => x != null
                         && x.Connected == PlayerConnectedState.PlayerConnected
                         && x.IsValid
                         && !x.IsBot
                         && x.Pawn?.Value != null
                         && (team.HasValue ? team.Value == (CsTeam)x.TeamNum : true));
    }

    private static bool CheckPermission(CCSPlayerController player)
    {
        bool res = false;
        foreach (var item in BaseRequiresPermissions)
        {
            if (AdminManager.PlayerHasPermissions(player, item))
            {
                res = true; break;
            }
        }
        return res;
    }

    private static bool ValidateCallerPlayer(CCSPlayerController? player)
    {
        if (player == null) return false;
        if (CheckPermission(player) == false)
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] Bu komutu kullanamazsın!");
            return false;
        }
        if (player.IsBot) return false;
        if (!player.IsValid || !player.PlayerPawn.IsValid) return false;
        return true;
    }

    private static bool IsValid(CCSPlayerController? player)
    {
        return player != null && player.IsValid && player.PlayerPawn.IsValid;
    }

    private static void RefreshPawn(CCSPlayerController player)
    {
        if (player != null)
        {
            if (player.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
            {
                foreach (var weapon in player.PlayerPawn.Value.WeaponServices!.MyWeapons)
                {
                    if (weapon.Value != null
                        && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                        && weapon.Value.DesignerName != "[null]")
                    {
                        if (weapon.Value.DesignerName.Contains("knife") == true)
                        {
                            weapon.Value.Remove();
                        }
                    }
                }
            }
            player.GiveNamedItem("weapon_knife");
        }
    }

    private static void SetColour(CCSPlayerController? player, Color colour)
    {
        if (player == null || !IsValid(player))
        {
            return;
        }

        CCSPlayerPawn? pawn = player.PlayerPawn.Value;

        if (pawn != null)
        {
            pawn.RenderMode = RenderMode_t.kRenderTransColor;
            pawn.Render = colour;
        }
    }

    private static void RemoveTerroristWeapons(bool knifeStays)
    {
        GetPlayers(CsTeam.Terrorist)
        .ToList()
        .ForEach(x =>
        {
            RemoveWeapons(x, knifeStays);
        });
    }

    private static void RemoveWeapons(CCSPlayerController x, bool knifeStays)
    {
        if (x?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
        {
            foreach (var weapon in x.PlayerPawn.Value.WeaponServices!.MyWeapons)
            {
                if (weapon.Value != null
                    && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                    && weapon.Value.DesignerName != "[null]")
                {
                    if (knifeStays == true)
                    {
                        if (weapon.Value.DesignerName.Contains("knife") == false)
                        {
                            weapon.Value.Remove();
                        }
                    }
                    else
                    {
                        weapon.Value.Remove();
                    }
                }
            }
        }
    }

    private static bool GetTargetAction(CCSPlayerController x, string target, string self)
    {
        var targetArgument = GetTargetArgument(target);

        return targetArgument switch
        {
            TargetForArgument.All => true,
            TargetForArgument.T => (CsTeam)x.TeamNum == CsTeam.Terrorist,
            TargetForArgument.Ct => (CsTeam)x.TeamNum == CsTeam.CounterTerrorist,
            TargetForArgument.None => x.PlayerName?.ToLower()?.Contains(target) ?? false,
            TargetForArgument.Me => x.PlayerName == self,
            _ => false
        };
    }

    private static bool ExecuteFreezeOrUnfreeze(CCSPlayerController x, string target, string self, out bool randomFreeze)
    {
        randomFreeze = _random.NextDouble() >= 0.5;

        var targetArgument = GetTargetArgument(target);
        return targetArgument switch
        {
            TargetForArgument.T => (CsTeam)x.TeamNum == CsTeam.Terrorist,
            TargetForArgument.Ct => (CsTeam)x.TeamNum == CsTeam.CounterTerrorist,
            TargetForArgument.Random => randomFreeze,
            TargetForArgument.RandomT => randomFreeze && (CsTeam)x.TeamNum == CsTeam.Terrorist,
            TargetForArgument.RandomCt => randomFreeze && (CsTeam)x.TeamNum == CsTeam.CounterTerrorist,
            TargetForArgument.All => true,
            TargetForArgument.Alive => true,
            TargetForArgument.None => x.PlayerName?.ToLower()?.Contains(target) ?? false,
            TargetForArgument.Me => x.PlayerName == self,
            _ => false,
        };
    }

    private static TargetForArgument GetTargetArgument(string target) => target switch
    {
        "@all" => TargetForArgument.All,
        "@t" => TargetForArgument.T,
        "@ct" => TargetForArgument.Ct,
        "@alive" => TargetForArgument.Alive,
        "@random" => TargetForArgument.Random,
        "@randomt" => TargetForArgument.RandomT,
        "@randomct" => TargetForArgument.RandomCt,
        "@me" => TargetForArgument.Me,
        _ => TargetForArgument.None,
    };
}