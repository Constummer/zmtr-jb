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
        return Utilities.GetPlayers()
             .Where(x => x != null
                         && x.Connected == PlayerConnectedState.PlayerConnected
                         && x.IsValid
                         && !x.IsBot
                         && x.Index != 32767
                         && !x.IsHLTV
                         && x.Pawn?.Value != null
                         && ValidateCallerPlayer(x, false)
                         && (team.HasValue ? team.Value == GetTeam(x) : true));
    }

    private static CCSPlayerController? GetWarden()
    {
        return GetPlayers().Where(x => ValidateCallerPlayer(x, false) && x.SteamID == LatestWCommandUser).FirstOrDefault();
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

    private static bool ValidateCallerPlayer(CCSPlayerController? player, bool checkPermission = true)
    {
        if (player == null) return false;
        if (checkPermission)
        {
            if (CheckPermission(player) == false)
            {
                player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.White} Bu komutu kullanamazsın!");
                return false;
            }
        }
        if (player == null
            || !player.IsValid
            || player.PlayerPawn == null
            || !player.PlayerPawn.IsValid
            || player.PlayerPawn.Value == null
            || !player.PlayerPawn.Value.IsValid
            ) return false;
        if (player.IsBot) return false;
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
            if (ValidateCallerPlayer(player, false) == false)
                return;
            var weaponServices = player.PlayerPawn.Value!.WeaponServices;
            if (weaponServices == null) return;

            player.GiveNamedItem("weapon_healthshot");
            if (weaponServices.MyWeapons != null)
            {
                foreach (var weapon in weaponServices.MyWeapons)
                {
                    if (weapon != null && weapon.IsValid && weapon.Value != null && weapon.Value!.DesignerName == "weapon_healthshot")
                    {
                        weapon.Value.Remove();
                        break;
                    }
                }
            }
        }
    }

    private static void RefreshPawnTP(CCSPlayerController x)
    {
        if (x != null)
        {
            if (ValidateCallerPlayer(x, false) == false)
                return;
            Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
            Vector currentSpeed = new Vector(0, 0, 0);
            QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
            x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
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
            TargetForArgument.T => GetTeam(x) == CsTeam.Terrorist,
            TargetForArgument.Ct => GetTeam(x) == CsTeam.CounterTerrorist,
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
            TargetForArgument.T => GetTeam(x) == CsTeam.Terrorist,
            TargetForArgument.Ct => GetTeam(x) == CsTeam.CounterTerrorist,
            TargetForArgument.Random => randomFreeze,
            TargetForArgument.RandomT => randomFreeze && GetTeam(x) == CsTeam.Terrorist,
            TargetForArgument.RandomCt => randomFreeze && GetTeam(x) == CsTeam.CounterTerrorist,
            TargetForArgument.All => true,
            TargetForArgument.Alive => true,
            TargetForArgument.None => x.PlayerName?.ToLower()?.Contains(target) ?? false,
            TargetForArgument.Me => x.PlayerName == self,
            _ => false,
        };
    }

    private static CsTeam GetTeam(CCSPlayerController x) => x.PendingTeamNum != x.TeamNum ? (CsTeam)x.PendingTeamNum : (CsTeam)x.TeamNum;

    private static TargetForArgument GetTargetArgument(string target) => target switch
    {
        "@all" => TargetForArgument.All,
        "@t" => TargetForArgument.T,
        "@terrorist" => TargetForArgument.T,
        "@ct" => TargetForArgument.Ct,
        "@counterstrike" => TargetForArgument.Ct,
        "@alive" => TargetForArgument.Alive,
        "@dead" => TargetForArgument.Dead,
        "@random" => TargetForArgument.Random,
        "@randomt" => TargetForArgument.RandomT,
        "@randomct" => TargetForArgument.RandomCt,
        "@me" => TargetForArgument.Me,
        _ => TargetForArgument.None,
    };

    private static List<List<T>> ChunkBy<T>(List<T> source, int chunkSize)
    {
        //if (numLists * elementsPerList != list.Count)
        //{
        //    throw new ArgumentException("The product of numLists and elementsPerList must equal the count of the input list.");
        //}
        int elementsPerList = (int)Math.Ceiling((double)source.Count / chunkSize);
        return Enumerable.Range(0, chunkSize)
                        .Select(i => source.Skip(i * elementsPerList).Take(elementsPerList).ToList())
                        .ToList();
        //return Enumerable.Range(0, (int)Math.Ceiling(source.Count / (double)chunkSize))
        //               .Select(i => source.Skip(i * chunkSize).Take(chunkSize).ToList())
        //               .ToList();

        //return source
        //    .Select((x, i) => new { Index = i, Value = x })
        //    .GroupBy(x => x.Index / chunkSize)
        //    .Select(x => x.Select(v => v.Value).ToList())
        //    .ToList();
    }

    private void LoadCredit()
    {
        Console.WriteLine(@"

         _______  _______  _        _______ _________          _______  _______  _______  _______
        (  ____ \(  ___  )( (    /|(  ____ \\__   __/|\     /|(       )(       )(  ____ \(  ____ )
        | (    \/| (   ) ||  \  ( || (    \/   ) (   | )   ( || () () || () () || (    \/| (    )|
        | |      | |   | ||   \ | || (_____    | |   | |   | || || || || || || || (__    | (____)|
        | |      | |   | || (\ \) |(_____  )   | |   | |   | || |(_)| || |(_)| ||  __)   |     __)
        | |      | |   | || | \   |      ) |   | |   | |   | || |   | || |   | || (      | (\ (
        | (____/\| (___) || )  \  |/\____) |   | |   | (___) || )   ( || )   ( || (____/\| ) \ \__
        (_______/(_______)|/    )_)\_______)   )_(   (_______)|/     \||/     \|(_______/|/   \__/

        Jailbreak Extras Plugin is loading, almost ready to go :}
        ");
    }
}