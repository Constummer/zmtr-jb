using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using JailbreakExtras.Lib.Database;
using JailbreakExtras.Lib.Database.Models;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    // <summary>
    //   @"CREATE TABLE IF NOT EXISTS `PlayerParticleData` (
    //                       `SteamId` bigint(20) DEFAULT NULL,
    //                       `BoughtModelIds` TEXT DEFAULT NULL,
    //                       `SelectedModelId` TEXT DEFAULT NULL
    //                     ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;"
    // </summary>
    private static Dictionary<ulong, PlayerParticleData> PlayerParticleDatas = new();

    private static Dictionary<ulong, CParticleSystem> PlayerAuras = new();
    private List<CParticleSystem> RoundEndParticles = new();

    public void RoundEndParticle(int winner)
    {
        var effectName = (CsTeam)winner switch
        {
            CsTeam.Terrorist => "particles/test/mahkum.vpcf",
            CsTeam.CounterTerrorist => "particles/test/gardiyan.vpcf",
            _ => null
        };
        if (effectName == null)
        {
            return;
        }
        GetPlayers()
            .ToList()
            .ForEach(p =>
            {
                var particle = Utilities.CreateEntityByName<CParticleSystem>("info_particle_system");

                if (particle != null && particle.IsValid)
                {
                    particle.EffectName = effectName;
                    particle.TintCP = 1;
                    particle.Teleport(p.PlayerPawn.Value.AbsOrigin, ANGLE_ZERO, VEC_ZERO);
                    particle.DispatchSpawn();
                    particle.AcceptInput("Start");
                    CustomSetParent(particle, p.PlayerPawn.Value);
                }
                RoundEndParticles.Add(particle);
            });
    }

    private static void GetPlayerParticleData(ulong steamId)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                MySqlCommand? cmd = new MySqlCommand(@$"SELECT `BoughtModelIds`,`SelectedModelId` FROM `PlayerParticleData` WHERE `SteamId` = @SteamId;", con);
                cmd.Parameters.AddWithValue("@SteamId", steamId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var data = new PlayerParticleData(steamId)
                        {
                            BoughtModelIds = reader.IsDBNull(0) ? null : reader.GetString(0),
                            SelectedModelId = reader.IsDBNull(1) ? null : reader.GetString(1),
                        };
                        if (PlayerParticleDatas.TryGetValue(steamId, out _))
                        {
                            PlayerParticleDatas.Add(steamId, data);
                        }
                        else
                        {
                            PlayerParticleDatas[steamId] = data;
                        }
                        return;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static void UpdatePlayerParticleDataOnDisonnect(ulong steamId)
    {
        try
        {
            if (PlayerParticleDatas.TryGetValue(steamId, out var data))
            {
                using (var con = Connection())
                {
                    if (con == null)
                    {
                        return;
                    }

                    var cmd = new MySqlCommand(@$"SELECT 1 FROM `PlayerParticleData` WHERE `SteamId` = @SteamId;", con);
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
                        cmd = new MySqlCommand(@$"UPDATE `PlayerParticleData`
                                          SET
                                              `BoughtModelIds` = @BoughtModelIds,
                                              `SelectedModelId` = @SelectedModelId
                                          WHERE `SteamId` = @SteamId;", con);

                        cmd.Parameters.AddWithValue("@SteamId", steamId);
                        cmd.Parameters.AddWithValue("@BoughtModelIds", data.BoughtModelIds.GetDbValue());
                        cmd.Parameters.AddWithValue("@SelectedModelId", data.SelectedModelId);

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd = new MySqlCommand(@$"INSERT INTO `PlayerParticleData`
                                      (SteamId,BoughtModelIds,SelectedModelId)
                                      VALUES (@SteamId,@BoughtModelIds,@SelectedModelId);", con);

                        cmd.Parameters.AddWithValue("@SteamId", steamId);
                        cmd.Parameters.AddWithValue("@BoughtModelIds", data.BoughtModelIds.GetDbValue());
                        cmd.Parameters.AddWithValue("@SelectedModelId", data.SelectedModelId.GetDbValue());

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void AuraMarketSelected(CCSPlayerController player, ChatMenuOption option)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (player?.SteamID == null || player!.SteamID == 0)
        {
            return;
        }
        var data = GetPlayerMarketModel(player.SteamID);
        if (data.Model == null) return;
        PlayerParticleDatas.TryGetValue(player.SteamID, out var particleData);
        var boughtModelIds = particleData?.GetBoughtModelIds();

        var marketMenu = new ChatMenu($" {CC.LB}Aura Market {CC.W}|{CC.G} Kredin {CC.W}<{CC.G}{data.Model.Credit}{CC.W}>");
        if (player.PawnIsAlive == false)
        {
            marketMenu.AddMenuOption($"{CC.R}SEÇİM YAPABİLMEK İÇİN {CC.DR}HAYATTA {CC.R}OLMALISINIZ", null, true);
        }
        foreach (var item in Config.Particle.MarketModeller)
        {
            var bought = boughtModelIds != null && boughtModelIds.Contains(item.Id);
            marketMenu.AddMenuOption($"{item.Text} | {item.Cost}{(bought ? " | Satın Alınmış" : "")}", (p, info) =>
            {
                if (ValidateCallerPlayer(player, false) == false) return;
                if (player?.SteamID == null || player!.SteamID == 0) return;
                if (bought == false)
                {
                    if (data.Model == null
                        || data.Model.Credit < item.Cost
                        || data.Model.Credit - item.Cost < 0)
                    {
                        player.PrintToChat($"{Prefix} {CC.W}Yetersiz Bakiye!");
                        return;
                    }
                    if (ValidateCallerPlayer(player, false) == false) return;
                    ConfirmMenu(player, data.Model.Credit, item.Text, () =>
                    {
                        if (ValidateCallerPlayer(player, false) == false) return;
                        data.Model.Credit -= item.Cost;
                        PlayerMarketModels[player.SteamID] = data.Model;

                        player.PrintToChat($"{Prefix} {CC.B}{item.Cost} {CC.W}Kredi Karşılığında {CC.B}{item.Text} {CC.W}Aura Satın Aldın!");
                        player.PrintToChat($"{Prefix} {CC.W}Mevcut Kredin = {CC.B}{data.Model.Credit}{CC.R}");
                        if (particleData == null)
                        {
                            particleData = new(player.SteamID)
                            {
                                BoughtModelIds = item.Id.ToString(),
                                SelectedModelId = item.Id.ToString()
                            };
                            PlayerParticleDatas.Add(player.SteamID, particleData);
                        }
                        else
                        {
                            particleData.SelectedModelId = item.Id.ToString();
                            particleData.BoughtModelIds = AddModelIdToGivenData(particleData.BoughtModelIds, item.Id);
                            PlayerParticleDatas[player.SteamID] = particleData;
                        }
                        UseGivenParticle(player, item.Id);
                    });
                }
                else
                {
                    if (ValidateCallerPlayer(player, false) == false) return;
                    particleData.SelectedModelId = item.Id.ToString();
                    PlayerParticleDatas[player.SteamID] = particleData;
                    UseGivenParticle(player, item.Id);
                }
            }, !player.PawnIsAlive);
        }
        ChatMenus.OpenMenu(player, marketMenu);
    }

    private void CreateAuraParticle(ulong tempUserId)
    {
        var x = GetPlayers().Where(x => x.SteamID == tempUserId).FirstOrDefault();
        if (x == null) return;
        if (ValidateCallerPlayer(x, false) == false) return;
        if (PlayerParticleDatas.TryGetValue(x.SteamID, out var particle))
        {
            if (string.IsNullOrWhiteSpace(particle.SelectedModelId)) return;
            if (int.TryParse(particle.SelectedModelId, out var modelId))
            {
                UseGivenParticle(x, modelId);
            }
        }
    }

    public void UseGivenParticle(CCSPlayerController? player, int id)
    {
        if (ValidateCallerPlayer(player, false) == false || player.PawnIsAlive == false)
        {
            return;
        }
        if (id == 0)
        {
            if (PlayerAuras.TryGetValue(player.SteamID, out var defAura))
            {
                if (defAura != null && defAura.IsValid)
                {
                    defAura.AcceptInput("Kill");
                    defAura.Remove();
                    defAura = null;
                }
                PlayerAuras.Remove(player.SteamID, out _);
            }
            return;
        }
        var selectedParticle = _Config.Particle.MarketModeller.Where(x => x.Id == id).FirstOrDefault();
        if (selectedParticle == null) return;
        if (selectedParticle.Enable == false)
        {
            player.PrintToChat($"{Prefix} {CC.B}{selectedParticle.Text} {CC.W}isimli aura kullanım dışıdır. Yetkili ile görüşmelisin.");
            return;
        }
        if (ValidateCallerPlayer(player, false) == false || player.PawnIsAlive == false)
        {
            return;
        }
        if (PlayerAuras.TryGetValue(player.SteamID, out var aura))
        {
            if (aura != null && aura.IsValid)
            {
                aura.AcceptInput("Kill");
                aura.Remove();
                aura = null;
            }
        }
        if (ValidateCallerPlayer(player, false) == false || player.PawnIsAlive == false)
        {
            return;
        }
        aura = null;
        aura = Utilities.CreateEntityByName<CParticleSystem>("info_particle_system");

        if (aura != null && aura.IsValid)
        {
            aura.EffectName = selectedParticle.PathToModel;
            aura.TintCP = 1;

            if (ValidateCallerPlayer(player, false) == false || player.PawnIsAlive == false)
            {
                return;
            }
            aura.Teleport(player.PlayerPawn.Value.AbsOrigin, ANGLE_ZERO, VEC_ZERO);
            aura.DispatchSpawn();
            aura.AcceptInput("Start");
            if (ValidateCallerPlayer(player, false) == false || player.PawnIsAlive == false)
            {
                return;
            }
            CustomSetParent(aura, player.PlayerPawn.Value);
        }
        if (PlayerAuras.ContainsKey(player.SteamID))
        {
            PlayerAuras[player.SteamID] = aura;
        }
        else
        {
            PlayerAuras.Add(player.SteamID, aura);
        }
    }
}