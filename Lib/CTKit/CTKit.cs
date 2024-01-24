using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Menu;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    /*
      CREATE TABLE IF NOT EXISTS `PlayerCTKit` (
                          `SteamId` bigint(20) DEFAULT NULL,
                          `KitId` mediumint(9) DEFAULT 0
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
    */
    public Dictionary<ulong, int> CTKitDatas { get; set; } = new();

    public class CTKitConfig
    {
        [JsonPropertyName("CTKits")]
        public List<CTKit> CTKits { get; set; } = new()
        {
            new(0, "M4A4 - Deagle", "m4a1", "deagle"),
            new(1, "M4A4 - Dual Baretta", "m4a1", "elite"),
            new(2, "AK47 - Deagle", "ak47", "deagle"),
            new(3, "AK47 - Dual Baretta", "ak47", "elite"),
            new(4, "AUG - Deagle", "aug", "deagle"),
            new(5, "AUG - Dual Baretta", "aug", "elite"),
            new(6, "P90 - Deagle", "p90", "deagle"),
            new(7, "P90 - Dual Baretta", "p90", "elite"),
            new(8, "AWP - Deagle", "awp", "deagle"),
            new(9, "AWP - Dual Baretta", "awp", "elite"),
            new(10, "Auto Sniper - Deagle", "scar20", "deagle"),
            new(11, "Auto Sniper - Dual Baretta", "scar20", "elite"),
        };
    }

    public class CTKit
    {
        public CTKit(int id, string text, string givePrimary, string giveSecondary)
        {
            Id = id;
            Text = text;
            GivePrimary = givePrimary;
            GiveSecondary = giveSecondary;
        }

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Text")]
        public string Text { get; set; } = "M4A4 - Deagle";

        [JsonPropertyName("GivePrimary")]
        public string GivePrimary { get; set; } = "m4a1";

        [JsonPropertyName("GiveSecondary")]
        public string GiveSecondary { get; set; } = "deagle";
    }

    private void CTKitOpenMenu(CCSPlayerController? player)
    {
        if (ValidateCallerPlayer(player, false) == false) return;

        var ctKitMenu = new ChatMenu("CT Kit Menu");
        ctKitMenu.AddMenuOption("Seçimine göre CT olarak doðduðunda", null, true);
        ctKitMenu.AddMenuOption("Bu silahlar ile baþlayacaksýn.", null, true);
        foreach (var item in Config.CTKit.CTKits)
        {
            ctKitMenu.AddMenuOption(item.Text, (p, i) =>
            {
                if (ValidateCallerPlayer(p, false) == false) return;
                if (ValidateCallerPlayer(player, false) == false) return;

                if (item.Id == 0)
                {
                    CTKitDatas.Remove(p.SteamID, out _);
                    RemoveCTKitData(p.SteamID);
                }
                else
                {
                    if (CTKitDatas.TryGetValue(p.SteamID, out var data))
                    {
                        CTKitDatas[p.SteamID] = item.Id;
                    }
                    else
                    {
                        CTKitDatas.Add(p.SteamID, item.Id);
                    }
                    AddCTKitData(p.SteamID, item.Id);
                }
            });
        }
        if (ValidateCallerPlayer(player, false) == false) return;

        ChatMenus.OpenMenu(player!, ctKitMenu);
    }

    private void GetAllCTKitData(MySqlConnection con)
    {
        if (con == null)
        {
            return;
        }
        MySqlCommand? cmd = new MySqlCommand(@$"SELECT `SteamId`, `KitId` FROM `PlayerCTKit`", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                var kitId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                if (kitId == 0)
                {
                    continue;
                }
                if (CTKitDatas.ContainsKey((ulong)steamId) == false)
                {
                    CTKitDatas.Add((ulong)steamId, kitId);
                }
                else
                {
                    CTKitDatas[(ulong)steamId] = kitId;
                }
            }
        }

        return;
    }

    private void AddCTKitData(ulong steamId, int kitId)
    {
        if (kitId == 0)
            return;
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }

                var cmd = new MySqlCommand(@$"SELECT 1 FROM `PlayerCTKit` WHERE `SteamId` = @SteamId;", con);
                cmd.Parameters.AddWithValue("@SteamId", steamId);
                bool exist = false;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        exist = true;
                    }
                }
                if (exist)
                {
                    cmd = new MySqlCommand(@$"UPDATE `PlayerCTKit`
                                          SET
                                              `KitId` = @KitId
                                          WHERE `SteamId` = @SteamId;", con);

                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    cmd.Parameters.AddWithValue("@KitId", kitId);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd = new MySqlCommand(@$"INSERT INTO `PlayerCTKit`
                                      (SteamId,KitId)
                                      VALUES (@SteamId,@KitId);", con);

                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    cmd.Parameters.AddWithValue("@KitId", kitId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void RemoveCTKitData(ulong steamId)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"Delete From `PlayerCTKit` WHERE `SteamId` = @SteamId;", con);

                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private HookResult CTKitOnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        CCSPlayerController? player = @event.Userid;

        if (player != null && IsValid(player))
        {
            int? slot = Slot(player);

            AddTimer(0.5f, () =>
            {
                if (slot != null)
                {
                    Spawn(Utilities.GetPlayerFromSlot(slot.Value));
                }
            });
        }

        return HookResult.Continue;
    }

    public void Spawn(CCSPlayerController? player)
    {
        if (player == null || !IsValidAlive(player))
        {
            return;
        }

        SetupPlayerGuns(player);

        // mute.spawn(player);
    }

    internal static readonly List<string> weaponList = new()
    {
         "weapon_knife",
         "weapon_knife_m9_bayonet",
         "weapon_knife_karambit",
         "weapon_bayonet",
         "weapon_knife_survival_bowie",
         "weapon_knife_butterfly",
         "weapon_knife_falchion",
         "weapon_knife_flip",
         "weapon_knife_gut",
         "weapon_knife_tactical",
         "weapon_knife_push",
         "weapon_knife_gypsy_jackknife",
         "weapon_knife_stiletto",
         "weapon_knife_widowmaker",
         "weapon_knife_ursus",
         "weapon_knife_css",
         "weapon_knife_cord",
         "weapon_knife_canis",
         "weapon_knife_outdoor",
         "weapon_knife_skeleton",
    };

    public void SetupPlayerGuns(CCSPlayerController? player)
    {
        if (player == null || !IsValidAlive(player))
        {
            return;
        }

        // cvars take care of this for us now
        // player.strip_weapons();
        if (player == null || !IsValidAlive(player))
        {
            return;
        }

        //var randomX = weaponList.Skip(_random.Next(weaponList.Count)).FirstOrDefault();
        //player.GiveNamedItem(randomX);

        if (IsCt(player))
        {
            // if(config.ct_guns)
            if (CTKitDatas.TryGetValue(player.SteamID, out var data) && data > 0)
            {
                var kit = Config.CTKit.CTKits.FirstOrDefault(x => x.Id == data);
                if (kit != null && kit.Id > 0)
                {
                    if (player == null || !IsValidAlive(player))
                    {
                        return;
                    }
                    player.GiveNamedItem($"weapon_{kit.GiveSecondary}");
                    if (player == null || !IsValidAlive(player))
                    {
                        return;
                    }
                    player.GiveNamedItem($"weapon_{kit.GivePrimary}");
                }
                else
                {
                    if (player == null || !IsValidAlive(player))
                    {
                        return;
                    }
                    player.GiveNamedItem("weapon_deagle");
                    if (player == null || !IsValidAlive(player))
                    {
                        return;
                    }
                    player.GiveNamedItem("weapon_m4a1");
                }
            }
            else
            {
                if (player == null || !IsValidAlive(player))
                {
                    return;
                }
                player.GiveNamedItem("weapon_deagle");
                if (player == null || !IsValidAlive(player))
                {
                    return;
                }
                player.GiveNamedItem("weapon_m4a1");
            }

            if (player == null || !IsValidAlive(player))
            {
                return;
            }
            player.GiveNamedItem("item_assaultsuit");
        }
    }

    public static int? ToSlot(int? user_id)
    {
        if (user_id == null)
        {
            return null;
        }

        return user_id & 0xff;
    }

    public static int? Slot(CCSPlayerController? player)
    {
        if (player == null)
        {
            return null;
        }

        return ToSlot(player.UserId);
    }

    public const int TEAM_CT = 3;

    public static bool IsCt(CCSPlayerController? player) => player != null && IsValid(player) && player.TeamNum == TEAM_CT;

    public static bool IsValidAlive(CCSPlayerController? player)
    {
        return player != null && IsValid(player) && player.PawnIsAlive && GetHealth(player) > 0;
    }

    public static int GetHealth(CCSPlayerController? player)
    {
        CCSPlayerPawn? pawn = Pawn2(player);

        if (pawn == null)
        {
            return 100;
        }

        return pawn.Health;
    }

    public static CCSPlayerPawn? Pawn2(CCSPlayerController? player)
    {
        if (player == null || !IsValid(player))
        {
            return null;
        }

        CCSPlayerPawn? pawn = player.PlayerPawn.Value;

        return pawn;
    }
}