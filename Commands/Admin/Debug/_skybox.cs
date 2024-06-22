using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("cskybox1")]
    public void cskybox1(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        var target = Utilities.FindAllEntitiesByDesignerName<CSkyboxReference>("skybox_reference");

        foreach (var ent in target)
        {
            if (!ent.IsValid)
            {
                continue;
            }

            Logger.LogInformation("----------------------------------------");
            Logger.LogInformation($"DamageFilterName = {ent.DamageFilterName}");
            Logger.LogInformation($"DesignerName = {ent.DesignerName}");
            Logger.LogInformation($"Globalname = {ent.Globalname}");
            Logger.LogInformation($"UniqueHammerID = {ent.UniqueHammerID}");
            Logger.LogInformation($"Index = {ent.Index}");
            if (ent.Blocker.IsValid)
            {
                var bl = ent.Blocker.Value;
                Logger.LogInformation($"bl DamageFilterName = {bl.DamageFilterName}");
                Logger.LogInformation($"bl DesignerName = {bl.DesignerName}");
                Logger.LogInformation($"bl Globalname = {bl.Globalname}");
            }
            if (ent.OwnerEntity.IsValid)
            {
                var bl = ent.OwnerEntity.Value;
                Logger.LogInformation($"bl DamageFilterName = {bl.DamageFilterName}");
                Logger.LogInformation($"bl DesignerName = {bl.DesignerName}");
                Logger.LogInformation($"bl Globalname = {bl.Globalname}");
            }
            Logger.LogInformation($"Entity.Name = {ent.Entity.Name}");
            Logger.LogInformation($"Entity.DesignerName = {ent.Entity.DesignerName}");

            Logger.LogInformation($"GetHashCode = {ent.GetHashCode()}");
            Logger.LogInformation("----------------------------------------");
            ent.AcceptInput("Open");
            AddTimer(1, () => ent.AcceptInput("Close"), SOM);
        }
    }

    [ConsoleCommand("cskybox2")]
    public void cskybox2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        var target = Utilities.FindAllEntitiesByDesignerName<CEnvSky>("env_sky");

        foreach (var ent in target)
        {
            if (!ent.IsValid)
            {
                continue;
            }

            Logger.LogInformation("----------------------------------------");
            Logger.LogInformation($"DamageFilterName = {ent.DamageFilterName}");
            Logger.LogInformation($"DesignerName = {ent.DesignerName}");
            Logger.LogInformation($"Globalname = {ent.Globalname}");
            Logger.LogInformation($"UniqueHammerID = {ent.UniqueHammerID}");
            Logger.LogInformation($"Index = {ent.Index}");

            Logger.LogInformation($"SkyMaterial= {ent.SkyMaterial}");
            Logger.LogInformation($"SkyMaterialLightingOnly= {ent.SkyMaterialLightingOnly}");
            Logger.LogInformation($"StartDisabled= {ent.StartDisabled}");
            Logger.LogInformation($"TintColor= {ent.TintColor}");
            Logger.LogInformation($"TintColorLightingOnly= {ent.TintColorLightingOnly}");
            Logger.LogInformation($"BrightnessScale= {ent.BrightnessScale}");
            Logger.LogInformation($"FogType= {ent.FogType}");
            Logger.LogInformation($"FogMinStart= {ent.FogMinStart}");
            Logger.LogInformation($"FogMinEnd= {ent.FogMinEnd}");
            Logger.LogInformation($"FogMaxStart= {ent.FogMaxStart}");
            Logger.LogInformation($"FogMaxEnd= {ent.FogMaxEnd}");
            Logger.LogInformation($"Enabled= {ent.Enabled}");
            if (ent.Blocker.IsValid)
            {
                var bl = ent.Blocker.Value;
                Logger.LogInformation($"bl DamageFilterName = {bl.DamageFilterName}");
                Logger.LogInformation($"bl DesignerName = {bl.DesignerName}");
                Logger.LogInformation($"bl Globalname = {bl.Globalname}");
            }
            if (ent.OwnerEntity.IsValid)
            {
                var bl = ent.OwnerEntity.Value;
                Logger.LogInformation($"bl DamageFilterName = {bl.DamageFilterName}");
                Logger.LogInformation($"bl DesignerName = {bl.DesignerName}");
                Logger.LogInformation($"bl Globalname = {bl.Globalname}");
            }
            Logger.LogInformation($"Entity.Name = {ent.Entity.Name}");
            Logger.LogInformation($"Entity.DesignerName = {ent.Entity.DesignerName}");

            Logger.LogInformation($"GetHashCode = {ent.GetHashCode()}");
            Logger.LogInformation("----------------------------------------");
            ent.AcceptInput("Open");
            AddTimer(1, () => ent.AcceptInput("Close"), SOM);
        }
    }

    [ConsoleCommand("cskybox3")]
    public void cskybox3(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        var skybox = Utilities.CreateEntityByName<CEnvSky>("env_sky");

        if (skybox != null && skybox.IsValid)
        {
            //   skybox.EffectName = "particles/testsystems/test_cross_product.vpcf";
            //skybox.EffectName = "particles/ui/status_levels/ui_status_level_8_energycirc.vpcf";
            //skybox.EffectName = "particles/testsystems/test_cross_product.vpcf";
            //skybox.SetModel("models/coop/challenge_coin.vmdl";
            skybox.SetModel("s2_de_inferno_sky01");
            skybox.Teleport(player.PlayerPawn.Value.AbsOrigin, ANGLE_ZERO, VEC_ZERO);
            skybox.DispatchSpawn();
            skybox.AcceptInput("Start");
            CustomSetParent(skybox, player.PlayerPawn.Value);
        }
    }
}