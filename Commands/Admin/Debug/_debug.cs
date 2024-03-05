using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using Discord;
using Discord.Webhook;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("cwarden")]
    public void cwarden(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.PrintToConsole($"{LatestWCommandUser}");
    }

    public static string DiscordWChangeNotifyUrl { get; set; } = "https://discord.com/api/webhooks/1199428821863104514/oNfcxm6pDO2wyeTPm5L75X4q9OHgw4QRNniAQXcAZTqiWlBsSRqljG1UjXunP-NMoeMK";

    [ConsoleCommand("cdiscordnotify")]
    public void cdiscordnotify(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        using (var client = new DiscordWebhookClient(DiscordWChangeNotifyUrl))
        {
            if (client == null)
            {
                return;
            }

            EmbedBuilder builder = new EmbedBuilder()
            .WithTitle("Jailbreak")
            .AddField($"Komutçu", $"```ansi\r\n\u001b[2;31mTEST\u001b[0m\r\n```")
            .AddField($"🗺️ㅤMap", $"```ansi\r\n\u001b[2;31mjb_test_tset\u001b[0m\r\n```", inline: true)
            .AddField("👥ㅤOyuncu S.", $"```ansi\r\n\u001b[2;31m31\u001b[0m/\u001b[2;32m69\u001b[0m\r\n```", inline: true)
            .AddField("ㅤ", $"[**`connect jb.zmtr.org`**](https://zmtr.org/baglan/jb)ㅤ👈 Bağlan")
            .WithColor(Discord.Color.Blue)
            .WithCurrentTimestamp();

            client.SendMessageAsync(embeds: new[] { builder.Build() }).GetAwaiter().GetResult();
        }
    }

    [ConsoleCommand("cspscs")]
    public void cspscs(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "lscpu",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                if (process != null)
                {
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();
                    string[] lines = output.Split('\n');

                    string cpuName = "";
                    double cpuSpeedMHz = 0;

                    foreach (string line in lines)
                    {
                        if (line.StartsWith("Model name:", StringComparison.OrdinalIgnoreCase))
                        {
                            cpuName = line.Split(':')[1].Trim();
                        }
                        else if (line.StartsWith("CPU MHz:", StringComparison.OrdinalIgnoreCase))
                        {
                            cpuSpeedMHz = double.Parse(line.Split(':')[1].Trim());
                        }

                        // Break the loop if both CPU name and speed are found
                        if (!string.IsNullOrEmpty(cpuName) && cpuSpeedMHz != 0)
                        {
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(cpuName) || cpuSpeedMHz == 0)
                    {
                        Server.PrintToChatAll("Unable to retrieve CPU information.");
                    }
                    else
                    {
                        Server.PrintToChatAll("CPU Name: " + cpuName);
                        Server.PrintToChatAll("CPU Speed (MHz): " + cpuSpeedMHz);
                    }
                }
                else
                {
                    Server.PrintToChatAll("Failed to start lscpu process.");
                }
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll("Error: " + e.Message);
        }
    }

    [ConsoleCommand("cmarkersifirla")]
    public void cmarkersifirla(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LatestWCommandUser = player.SteamID;
        ClearLasers();
        var xyz = player.PlayerPawn.Value.AbsOrigin;

        CalculateAndPrintEdges(xyz.X, xyz.Y, xyz.Z, 100, 100);
    }

    [ConsoleCommand("cmarkertest")]
    public void cmarkertest(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        ClearLasers();
        var xyz = player.PlayerPawn.Value.AbsOrigin;

        CalculateAndPrintEdges(xyz.X, xyz.Y, xyz.Z, 100, 100);
    }

    [ConsoleCommand("siktinsenecalamazsinizamk3142069")]
    public void siktinsenecalamazsinizamk(CCSPlayerController? player, CommandInfo info)
    {
        SpamNewIPTimer();
    }

    public CParticleSystem? Aura { get; private set; }

    [ConsoleCommand("cparticle")]
    public void cparticle(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Aura?.Remove();
        Aura = null;
        //var entity = Utilities.CreateEntityByName<CPhysicsPropMultiplayer>("prop_physics_multiplayer");
        Aura = Utilities.CreateEntityByName<CParticleSystem>("info_particle_system");

        if (Aura != null && Aura.IsValid)
        {
            Aura.EffectName = "particles/test/energy.vpcf";
            //   Aura.EffectName = "particles/testsystems/test_cross_product.vpcf";
            //Aura.EffectName = "particles/ui/status_levels/ui_status_level_8_energycirc.vpcf";
            //Aura.EffectName = "particles/testsystems/test_cross_product.vpcf";
            //Aura.SetModel("models/coop/challenge_coin.vmdl";
            Aura.TintCP = 1;

            Aura.Teleport(player.PlayerPawn.Value.AbsOrigin, ANGLE_ZERO, VEC_ZERO);
            Aura.DispatchSpawn();
            Aura.AcceptInput("Start");
            CustomSetParent(Aura, player.PlayerPawn.Value);
        }
    }

    [ConsoleCommand("cparticlekill")]
    public void cparticlekill(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (Aura != null && Aura.IsValid)
        {
            Aura.AcceptInput("Kill");
        }
    }

    [ConsoleCommand("cthird")]
    public void cthird(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var cam = Utilities.CreateEntityByName<CPointCamera>("point_camera");

        if (cam != null && cam.IsValid)
        {
            cam.MoveType = MoveType_t.MOVETYPE_NOCLIP;
            cam.TakesDamage = false;
            cam.GravityScale = 0;
            cam.Active = true;
            cam.IsOn = true;
            cam.FOV = 90;
            cam.ZNear = 4;
            cam.ZFar = 10_000;

            var abs = player.PlayerPawn.Value.AbsOrigin;
            cam.Teleport(new(abs.X, abs.Y, abs.Z + 40), ANGLE_ZERO, VEC_ZERO);
            cam.DispatchSpawn();

            CustomSetParent(cam, player.PlayerPawn.Value);
        }
    }

    [ConsoleCommand("cthird2")]
    public void cthird2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        NativeAPI.IssueClientCommand(player.Slot, "thirdperson");
    }

    [ConsoleCommand("ccentermenu")]
    public void ccentermenu(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var menu = new CenterHtmlMenu("123456780qwertyuio");
        for (int i = 0; i < 20; i++)
        {
            var key = $"test_{i}";
            menu.AddMenuOption(key, (i, o) =>
            {
                player.PrintToChat(key + " | sectin");
            });
        }
        MenuManager.OpenCenterHtmlMenu(this, player, menu);
    }

    [ConsoleCommand("ccustomcoin")]
    public void ccustomcoin(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var coin = Utilities.CreateEntityByName<CPhysicsPropMultiplayer>("prop_physics_multiplayer");
        if (coin == null)
        {
            return;
        }
        var xyz = player.PlayerPawn.Value.AbsOrigin;
        coin.SetModel("models/coop/challenge_coin.vmdl");
        coin.Teleport(new Vector(xyz.X, xyz.Y, xyz.Z + 100), ANGLE_ZERO, VEC_ZERO);
        coin.DispatchSpawn();
        coin.AcceptInput("Start");
        CustomSetParent(coin, player.PlayerPawn.Value);
    }

    [ConsoleCommand("cstop")]
    public void cstop(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        var target = info.ArgString.GetArg(0);

        GetPlayers()
                     .Where(x => x.PawnIsAlive
                     && x.Pawn.Value != null
                              && GetTargetAction(x, target, player))
                     .ToList()
                     .ForEach(x =>
                     {
                         x.PlayerPawn.Value.VelocityModifier = 0.0f;
                     });
    }

    [ConsoleCommand("cbuneamk")]
    public void cbuneamk(CCSPlayerController? x, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(x, "@css/root"))
        {
            x.PrintToChat(NotEnoughPermission);
            return;
        }
        var target = info.ArgString.GetArg(0);
        var snd = target switch
        {
            "1" => MoveType_t.MOVETYPE_NONE,
            "2" => MoveType_t.MOVETYPE_OBSOLETE,
            "3" => MoveType_t.MOVETYPE_WALK,
            "5" => MoveType_t.MOVETYPE_FLY,
            "6" => MoveType_t.MOVETYPE_FLYGRAVITY,
            "7" => MoveType_t.MOVETYPE_VPHYSICS,
            "8" => MoveType_t.MOVETYPE_PUSH,
            "9" => MoveType_t.MOVETYPE_NOCLIP,
            "10" => MoveType_t.MOVETYPE_OBSERVER,
            "11" => MoveType_t.MOVETYPE_LADDER,
            "12" => MoveType_t.MOVETYPE_CUSTOM,
            "13" => MoveType_t.MOVETYPE_LAST,
            "14" => MoveType_t.MOVETYPE_MAX_BITS,
        };

        SetMoveType(x, snd);

        RefreshPawnTP(x);
    }

    [ConsoleCommand("cdeath")]
    public void cdeath(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Logger.LogInformation(player.AbsOrigin.ToString());
        Logger.LogInformation(player.Pawn.Value.AbsOrigin.ToString());
        Logger.LogInformation(player.PlayerPawn.Value.AbsOrigin.ToString());
        Logger.LogInformation(player.PlayerPawn.Value.CameraServices.Pawn.Value.AbsOrigin.ToString());
    }

    [ConsoleCommand("ctp")]
    public void ctp(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        try
        {
            var a = Schema.GetSchemaValue<byte>(player.PlayerPawn.Value.Handle, "CPlayer_ObserverServices", "m_iObserverMode");
            Schema.SetSchemaValue<byte>(player.Handle, "CPlayer_ObserverServices", "m_iObserverMode", 1);
            //Schema.SetCustomMarshalledType<byte>(player.PlayerPawn.Value.Handle, "CPlayer_ObserverServices", "m_iObserverMode", 1);
        }
        catch (Exception e)
        {
        }
    }

    [ConsoleCommand("cwtf")]
    public void cwtf(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        ForceEntInput("func_breakable", "Break", "DropEldenGidiyeah");
    }

    [ConsoleCommand("cbichaha")]
    public void cbichaha(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var knife = GetWeapon(player, "weapon_knife");
        if (WeaponIsValid(knife) == false)
        {
            return;
        }

        Server.NextFrame(() =>
        {
            Server.PrintToChatAll($"11 {knife.FallbackPaintKit}");
            knife.SetModel("models/coop/challenge_coin.vmdl");
            Server.PrintToChatAll($"11 {knife.FallbackPaintKit}");
        });
        Server.PrintToChatAll("1");
    }

    [ConsoleCommand("cbicdeneme")]
    public void cbicdeneme(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var knife = GetWeapon(player, "weapon_knife");
        if (WeaponIsValid(knife) == false)
        {
            return;
        }
        Server.NextFrame(() =>
        {
            knife.SetModel("weapons/models/nozb1/aloneelxy/aloneelxy_freebayonety.vmdl");
        });
        Server.PrintToChatAll("1");
    }

    [ConsoleCommand("cbicdeneme2")]
    public void cbicdeneme2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var knife = GetWeapon(player, "weapon_knife");
        if (WeaponIsValid(knife) == false)
        {
            return;
        }
        Server.NextFrame(() =>
        {
            knife.SetModel("weapons/models/nozb1/aloneelxy/aloneelxy_freebayonety2.vmdl");
        });
        Server.PrintToChatAll("1");
    }

    [ConsoleCommand("cbicdeneme3")]
    public void cbicdeneme3(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var knife = GetWeapon(player, "weapon_knife");
        if (WeaponIsValid(knife) == false)
        {
            return;
        }

        Server.NextFrame(() =>
        {
            knife.SetModel("weapons/models/nozb1/aloneelxy/aloneelxy_freebayonety2.vmdl");
        });
        Server.PrintToChatAll("1");
    }

    [ConsoleCommand("ctparty")]
    [CommandHelper(3, "<pitch> <volume> <delay>")]
    public void ctparty(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        var pitch = info.ArgString.GetArg(0);
        var volume = info.ArgString.GetArg(1);
        var delay = info.ArgString.GetArg(2);
        if (int.TryParse(pitch, out var p) == false
            || int.TryParse(volume, out var v) == false
            || int.TryParse(delay, out var d) == false
            )
        {
            return;
        }
        CBaseEntity_EmitSoundParams(player, "sounds/player/burn_damage3.vsnd_c", p, v, d);
    }

    [ConsoleCommand("ctakim2")]
    public void takim2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (player.PlayerName == "Constummer")
        {
            player!.ChangeTeam(CsTeam.Spectator);
        }
    }

    [ConsoleCommand("ctakim3")]
    public void ctakim3(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (player.PlayerName == "Constummer")
        {
            player!.ChangeTeam(CsTeam.Terrorist);
        }
    }

    [ConsoleCommand("ts")]
    public void testses(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgString.GetArg(0);
        var snd = target switch
        {
            "1" => "sounds/zmtr_warden/wzenter.vsnd_c",
            "2" => "sounds/zmtr_warden/wzleave.vsnd_c",
            "3" => "sounds/lr/lr1.vsnd_c",
            "4" => "sounds/zmtr_freeze/freeze.vsnd_c",
            "5" => "sounds/zmtr/bell.vsnd_c",
            "6" => "sounds/zmtr/karamantukur.vsnd_c",
            "7" => "sounds/mapeadores/saysounds/applause.vsnd_c",
            "8" => "sounds/mapeadores/saysounds/applause2.vsnd_c",
            "9" => "sounds/mapeadores/saysounds/applause3.vsnd_c",
            "10" => "sounds/mapeadores/saysounds/applause4.vsnd_c",
            "11" => "sounds/mapeadores/saysounds/chimp2.vsnd_c",
            "12" => "sounds/mapeadores/saysounds/heheboi.vsnd_c",
            "13" => "sounds/player/burn_damage1.vsnd_c",
            "14" => "sounds/player/burn_damage2.vsnd_c",
            "15" => "sounds/player/burn_damage3.vsnd_c",
            "16" => "sounds/player/burn_damage4.vsnd_c",
            "17" => "sounds/player/burn_damage5.vsnd_c",
            _ => null
        };
        if (snd == null)
        {
            return;
        }
        player.ExecuteClientCommand($"play {snd}");
    }
}