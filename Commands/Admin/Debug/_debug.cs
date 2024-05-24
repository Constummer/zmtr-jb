using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private CPointWorldText wallText = null;

    [ConsoleCommand("cwalltext")]
    public void cwalltext(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        if (wallText != null && wallText.IsValid)
        {
            wallText.Remove();
        }
        wallText = Utilities.CreateEntityByName<CPointWorldText>("point_worldtext");
        if (wallText != null && wallText.IsValid)
        {
            wallText.MessageText = $"****SKZ SÜRELERİ****";
            wallText.Enabled = true;
            wallText.FontSize = 30;
            wallText.Color = Color.Red;
            wallText.Fullbright = true;
            wallText.WorldUnitsPerPx = 1.0f;
            wallText.DepthOffset = 0.0f;
            wallText.JustifyHorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT;
            wallText.JustifyVertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP;
            wallText.ReorientMode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE;

            wallText.Teleport(new Vector(-3274, -2044, 792),
                new QAngle(0, -180, 90),
                new Vector(0, 0, 0));
            wallText.DispatchSpawn();

            //wallText.AcceptInput("display", activator: player.PlayerPawn.Value, value: "!activator");
            AddTimer(1.0f, () =>
            {
                wallText.MessageText = Random.Shared.Next().ToString() + "\n amskaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaam cok ıyı1" + "\n amskm cok ıyı2" + "\n amskm cok ıyı3" + "\n amskm cok ıyı4" + "\n amskm cok ıyı5" + "\n amskm cok ıyı6" + "\n amskm cok ıyı7" + "\n amskm cok ıyı8" + "\n amskm cok ıyı9";
                Utilities.SetStateChanged(wallText, "CPointWorldText", "m_messageText");
            }, TimerFlags.REPEAT);
            //CustomSetParent(wallText, player.PlayerPawn.Value);

            return;
            wallText.MessageText = $"Heelo\n a";
            wallText.Enabled = true;
            wallText.FontSize = 30;
            wallText.Color = Color.Red;
            wallText.Fullbright = true;
            wallText.WorldUnitsPerPx = 1.0f;
            wallText.DepthOffset = 0.0f;
            wallText.JustifyHorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT;
            wallText.JustifyVertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP;
            wallText.ReorientMode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE;

            wallText.Teleport(player.PlayerPawn.Value!.AbsOrigin!.With(z: player.PlayerPawn.Value.AbsOrigin.Z + 10), player.PlayerPawn.Value.AbsRotation!, new Vector(0, 0, 0));
            wallText.DispatchSpawn();
            AddTimer(1.0f, () =>
            {
                wallText.MessageText = Random.Shared.Next().ToString() + "\n amskaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaam cok ıyı1" + "\n amskm cok ıyı2" + "\n amskm cok ıyı3" + "\n amskm cok ıyı4" + "\n amskm cok ıyı5" + "\n amskm cok ıyı6" + "\n amskm cok ıyı7" + "\n amskm cok ıyı8" + "\n amskm cok ıyı9";
                Utilities.SetStateChanged(wallText, "CPointWorldText", "m_messageText");
            }, TimerFlags.REPEAT);
            //wallText.MessageText = "testtesttesttesttest";
            //wallText.FontName = "TF2";
            //wallText.Enabled = true;
            //wallText.FontSize = 10;
            //wallText.Color = Color.Red;
            //wallText.ReorientMode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_AROUND_UP;
            //wallText.JustifyVertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP;
            //wallText.JustifyHorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_CENTER;
            ////wallText.Teleport(player.PlayerPawn.Value.AbsOrigin, ANGLE_ZERO, VEC_ZERO);
            //wallText.DispatchSpawn();
            ////wallText.AcceptInput("Start");
            //CustomSetParent(wallText, player.PlayerPawn.Value);
        }
    }

    [ConsoleCommand("cchangemodel")]
    public void cchangemodel(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        if (!int.TryParse(info.ArgString.GetArgLast(), out var id))
        {
            player!.PrintToChat($"{Prefix}{CC.G} id yanlış!");
            return;
        }

        var target = info.ArgString.GetArgSkipFromLast(1);
        if (FindSinglePlayer(player, target, out var x) == false)
        {
            return;
        }
        if (id <= 0)
        {
            GiveRandomSkin(x);
        }
        else
        {
            var model = Config.Market.MarketModeller.Where(x => x.Id == id).FirstOrDefault();
            if (model != null)
            {
                SetModelNextServerFrame(x!.PlayerPawn.Value!, model.PathToModel);
            }
        }
    }

    [ConsoleCommand("cchickencontrol")]
    public void cchickencontrol(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var entity = Utilities.CreateEntityByName<CChicken>("chicken");
        if (entity != null && entity.IsValid)
        {
            entity.Teleport(player.PlayerPawn.Value.AbsOrigin, ANGLE_ZERO, VEC_ZERO);
            entity.DispatchSpawn();

            player.PlayerPawn.Value!.CameraServices!.ViewEntity.Raw = entity.EntityHandle.Raw;
            Utilities.SetStateChanged(player.PlayerPawn.Value, "CBasePlayerPawn", "m_pCameraServices");
        }
    }

    [ConsoleCommand("cvoiceunmute")]
    public void cvoiceunmute(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var DATA = info.ArgString.GetArgLast();
        var target = info.ArgString.GetArgSkip(0);
        Server.PrintToChatAll($"{target}");
        if (FindSinglePlayer(player, target, out var x) == false)
        {
            return;
        }
        var snd = DATA switch
        {
            "1" => VoiceFlags.Normal,
            "2" => VoiceFlags.Muted,
            "3" => VoiceFlags.All,
            "4" => VoiceFlags.ListenAll,
            "5" => VoiceFlags.Team,
            "6" => VoiceFlags.ListenTeam,
        };
        x.VoiceFlags |= snd;
    }

    [ConsoleCommand("cvoicemute")]
    public void cvoicemute(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
            "1" => VoiceFlags.Normal,
            "2" => VoiceFlags.Muted,
            "3" => VoiceFlags.All,
            "4" => VoiceFlags.ListenAll,
            "5" => VoiceFlags.Team,
            "6" => VoiceFlags.ListenTeam,
        };
        player.VoiceFlags &= ~snd;
    }

    [ConsoleCommand("cscore")]
    public void cscore(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (PlayerLevels.TryGetValue(player.SteamID, out var item))
        {
            player.CompetitiveWins = 10;
            Utilities.SetStateChanged(player, "CCSPlayerController", "m_iCompetitiveWins");
            player.CompetitiveRankType = (sbyte)(11);
            Utilities.SetStateChanged(player, "CCSPlayerController", "m_iCompetitiveRankType");
            player.CompetitiveRanking = item.Xp;
            Utilities.SetStateChanged(player, "CCSPlayerController", "m_iCompetitiveRanking");
        }
    }

    [ConsoleCommand("cscore2")]
    public void cscore2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (PlayerLevels.TryGetValue(player.SteamID, out var item))
        {
            player.CompetitiveWins = 10;
            Utilities.SetStateChanged(player, "CCSPlayerController", "m_iCompetitiveWins");
            player.CompetitiveRankType = (sbyte)(12);
            Utilities.SetStateChanged(player, "CCSPlayerController", "m_iCompetitiveRankType");
            player.CompetitiveRanking = item.Xp;
            Utilities.SetStateChanged(player, "CCSPlayerController", "m_iCompetitiveRanking");
        }
    }

    [ConsoleCommand("cscore3")]
    public void cscore3(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.PrintToChatAll("---");
        Server.PrintToChatAll($"{player.CompetitiveWins}");
        Server.PrintToChatAll($"{player.CompetitiveRankType}");
        Server.PrintToChatAll($"{player.CompTeammateColor}");
        Server.PrintToChatAll($"{player.CompetitiveRanking}");
        Server.PrintToChatAll($"{player.CompetitiveRankingPredicted_Win}");
        Server.PrintToChatAll($"{player.CompetitiveRankingPredicted_Tie}");
        Server.PrintToChatAll($"{player.CompetitiveRankingPredicted_Loss}");
        Server.PrintToChatAll("---");
    }

    [ConsoleCommand("cwarden")]
    public void cwarden(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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

    private bool csaytestActive = false;

    [ConsoleCommand("csaytest")]
    public void csaytest(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        csaytestActive = !csaytestActive;
    }

    [ConsoleCommand("csaytest2")]
    public void csaytest2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.PrintToChatAll("csaytest2");
        Server.PrintToChatAll($"_ =>{info.ArgString}");
        Server.PrintToChatAll($"_ =>{info.ArgString.GetArg(0)}");
    }

    [ConsoleCommand("cprintall")]
    public void cprintall(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        player.PrintToConsole("---------CommandHandlers------------");
        foreach (var item in base.CommandHandlers.Keys)
        {
            player.PrintToConsole(item.GetType().Name);
            player.PrintToConsole(item.GetType().FullName);
            player.PrintToConsole(item.GetType().ToString());
            player.PrintToConsole(item.Method.Name?.ToLower());
            player.PrintToConsole("-");
            player.PrintToConsole(item.GetType().BaseType.Name);
            player.PrintToConsole(item.GetType().BaseType.FullName);
            player.PrintToConsole(item.GetType().BaseType.ToString());
            player.PrintToConsole("------");
        }
        player.PrintToConsole("--------Handlers-------------");
        foreach (var item in base.Handlers.Keys)
        {
            player.PrintToConsole(item.GetType().Name);
            player.PrintToConsole(item.GetType().FullName);
            player.PrintToConsole(item.GetType().ToString());
            player.PrintToConsole(item.Method.Name?.ToLower());
            player.PrintToConsole("-");
            player.PrintToConsole(item.GetType().BaseType.Name);
            player.PrintToConsole(item.GetType().BaseType.FullName);
            player.PrintToConsole(item.GetType().BaseType.ToString());
            player.PrintToConsole("------");
        }
        player.PrintToConsole("--------CommandListeners-------------");
        foreach (var item in base.CommandListeners.Keys)
        {
            player.PrintToConsole(item.GetType().Name);
            player.PrintToConsole(item.GetType().FullName);
            player.PrintToConsole(item.GetType().ToString());
            player.PrintToConsole(item.Method.Name?.ToLower());
            player.PrintToConsole("-");
            player.PrintToConsole(item.GetType().BaseType.Name);
            player.PrintToConsole(item.GetType().BaseType.FullName);
            player.PrintToConsole(item.GetType().BaseType.ToString());
            player.PrintToConsole("------");
        }
    }

    [ConsoleCommand("csut")]
    public void csut(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        SetModelNextServerFrame(player.PlayerPawn.Value!, "characters/models/ambrosian/zmtr/sut/sut.vmdl");
    }

    private CParticleSystem auratest = null;

    [ConsoleCommand("cAuraTest")]
    public void cAuraTest(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgString.GetArg(0);

        Server.PrintToChatAll(info.GetArg(1));
        var snd = target switch
        {
            //"4" => "particles/kolka/4_particle.vpcf",
            "1" => "particles/kolka/1_particle.vpcf",
            "2" => "particles/kolka/2_particle.vpcf",
            "3" => "particles/kolka/3_particle.vpcf",
            "5" => "particles/kolka/5_particle.vpcf",//rocket
            "6" => "particles/kolka/6_particle.vpcf",
            "7" => "particles/kolka/7_particle.vpcf",
            "8" => "particles/kolka/8_particle.vpcf",
            "9" => "particles/kolka/9_particle.vpcf",
            "10" => "particles/kolka/10_particle.vpcf",
            "11" => "particles/kolka/11_particle.vpcf",
            "12" => "particles/kolka/12_particle.vpcf",
            "13" => "particles/kolka/13_particle.vpcf",
            "14" => "particles/kolka/14_particle.vpcf",
            "15" => "particles/kolka/15_particle.vpcf",
            "16" => "particles/kolka/16_particle.vpcf",
            "17" => "particles/kolka/17_particle.vpcf",
            "18" => "particles/kolka/18_particle.vpcf",
        };
        if (auratest != null && auratest.IsValid)
        {
            auratest.Remove();
        }
        auratest = Utilities.CreateEntityByName<CParticleSystem>("info_particle_system");
        if (auratest != null && auratest.IsValid)
        {
            auratest.EffectName = snd;
            auratest.TintCP = 1;
            auratest.Teleport(player.PlayerPawn.Value.AbsOrigin, ANGLE_ZERO, VEC_ZERO);
            auratest.DispatchSpawn();
            auratest.AcceptInput("Start");
            RoundEndParticles.Add(auratest);
            CustomSetParent(auratest, player.PlayerPawn.Value);
        }
    }

    [ConsoleCommand("cParach")]
    public void cParach(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
            "1" => "models/zmtr/parasut.vmdl",
            "2" => "models/parachute/parachute_rainbow.vmdl",
            "3" => "models/parachute/parachute_bf2.vmdl",
            "4" => "models/parachute/parachute_bf2142.vmdl",
            "5" => "models/parachute/parachute_spongebob.vmdl",
            "6" => "models/ptrunners/parachute/umbrella_big2.vmdl",
            "7" => "models/zmtr/special.vmdl"
        };

        var entity = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic_override");
        if (entity != null && entity.IsValid)
        {
            entity.MoveType = MoveType_t.MOVETYPE_NOCLIP;
            entity.Collision.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_NONE;
            entity.Collision.CollisionAttribute.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_NONE;

            entity.Teleport(player.PlayerPawn.Value.AbsOrigin, ANGLE_ZERO, VEC_ZERO);

            entity.DispatchSpawn();

            entity.SetModel(snd);

            gParaModel[player.UserId] = entity;
        }
    }

    [ConsoleCommand("cwelcome")]
    public void cwelcome(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        WelcomeMsgSpam(player);
    }

    [ConsoleCommand("crgb")]
    public void crgb(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var t = AddTimer(0.1f, () =>
          {
              player.PrintToCenterHtml("<img src='https://zmtr.org/assets/rgb.jpg'></img>");
          }, Full);
        AddTimer(30f, () =>
        {
            t.Kill();
        }, SOM);
    }

    [ConsoleCommand("cspawnweapon")]
    public void cspawnweapon(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var asdf = new CAK47(player.Handle);
        Server.PrintToChatAll("a");
        if (asdf != null && asdf.IsValid)
        {
            Server.PrintToChatAll("b");

            asdf.Teleport(player.PlayerPawn.Value.AbsOrigin, ANGLE_ZERO, VEC_ZERO);
            Server.PrintToChatAll("c");

            asdf.DispatchSpawn();
            Server.PrintToChatAll("d");
        }
    }

    [ConsoleCommand("csoundtest")]
    public void csoundtest(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        //var sound = entity.EmitSound("Weapon_AK47.Single");
        //sound.SetParameter("volume", 100f);
        //sound.SetParameter("pitch", 2f);
    }

    [ConsoleCommand("cspscs")]
    public void cspscs(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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

            using (Process? process = Process.Start(psi))
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
            ConsMsg(e.Message);
        }
    }

    [ConsoleCommand("cmarkersifirla")]
    public void cmarkersifirla(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
            cam.Active = true;
            //cam.MoveType = MoveType_t.MOVETYPE_NOCLIP;
            //cam.TakesDamage = false;
            //cam.GravityScale = 0;
            //cam.SentToClients = 1;
            cam.IsOn = true;
            cam.AspectRatio = 1;
            cam.UseScreenAspectRatio = false;
            cam.FogEnable = false;
            cam.CanHLTVUse = true;
            //player.PlayerPawn.Value.ObserverServices.ObserverTarget. = 0;
            //player.PlayerPawn.Value.ObserverServices.ObserverMode = (int)ObserverMode_t.OBS_MODE_CHASE;
            //player.PlayerPawn.Value.ObserverServices.ObserverMode = 1;
            //Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_hObserverTarget");

            cam.FOV = 90;
            cam.ZNear = 4;
            cam.ZFar = 10_000;
            player.PlayerPawn.Value!.CameraServices!.ViewEntity.Raw = cam.EntityHandle.Raw;
            Utilities.SetStateChanged(player.PlayerPawn.Value, "CBasePlayerPawn", "m_pCameraServices");

            var abs = player.PlayerPawn.Value.AbsOrigin;

            cam.Teleport(new(abs.X, abs.Y, abs.Z + 40), null, player.PlayerPawn.Value.AbsVelocity);
            cam.AcceptInput("SetOnAndTurnOthersOff");
            cam.AcceptInput("Enable");
            cam.DispatchSpawn();

            //CustomSetParent(cam, player.PlayerPawn.Value);
        }
    }

    [ConsoleCommand("cthird2")]
    public void cthird2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var menu = new CenterHtmlMenu("123456780qwertyuio", this);
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
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
        if (!AdminManager.PlayerHasPermissions(x, Perm_Root))
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

    [ConsoleCommand("cinventoryService")]
    public void cinventoryService(CCSPlayerController? x, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(x, Perm_Root))
        {
            x.PrintToChat(NotEnoughPermission);
            return;
        }
        ;
        x.PrintToChat($"{x.InventoryServices.PersonaDataXpTrailLevel}");
        x.PrintToChat($"{x.InventoryServices.PersonaDataPublicCommendsLeader}");
        x.PrintToChat($"{x.InventoryServices.PersonaDataPublicCommendsTeacher}");
        x.PrintToChat($"{x.InventoryServices.PersonaDataPublicCommendsFriendly}");
        x.PrintToChat($"{x.InventoryServices.PersonaDataPublicLevel}");
        var i = 0;
        foreach (var item in x.InventoryServices.Rank)
        {
            x.PrintToChat($"{item}_{i}");
            i++;
        }
    }

    [ConsoleCommand("cviewdata")]
    public void cviewdata(CCSPlayerController? x, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(x, Perm_Root))
        {
            x.PrintToChat(NotEnoughPermission);
            return;
        }

        x.PrintToChat($"{x.PlayerPawn.Value.AbsOrigin}");
        x.PrintToChat($"{x.PlayerPawn.Value.AbsVelocity}");
        x.PrintToChat($"{x.PlayerPawn.Value.AbsRotation}");
        x.PrintToChat($"{x.AbsOrigin}");
        x.PrintToChat($"{x.AbsVelocity}");
        x.PrintToChat($"{x.AbsRotation}");
        x.PrintToChat($"{x.AngVelocity}");
        x.PrintToChat($"{x.BaseVelocity}");
        x.PrintToChat($"{x.Pawn.Value.AbsOrigin}");
        x.PrintToChat($"{x.Pawn.Value.AbsVelocity}");
        x.PrintToChat($"{x.Pawn.Value.AbsRotation}");
        x.PrintToChat($"{x.Pawn.Value.V_angle}");
        x.PrintToChat($"{x.PlayerPawn.Value.DeathEyeAngles}");
        x.PrintToChat($"{x.PlayerPawn.Value.EyeAngles}");
        x.Teleport(x.PlayerPawn.Value.AbsOrigin, null, null);
    }

    [ConsoleCommand("cbattlepassLogin")]
    public void cbattlepassLogin(CCSPlayerController? x, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(x, Perm_Root))
        {
            x.PrintToChat(NotEnoughPermission);
            return;
        }
        GetPlayerBattlePassData(x.SteamID);
    }

    [ConsoleCommand("cbattlepasspremiumLogin")]
    public void cbattlepassPremiumLogin(CCSPlayerController? x, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(x, Perm_Root))
        {
            x.PrintToChat(NotEnoughPermission);
            return;
        }
        GetPlayerBattlePassPremiumData(x.SteamID);
    }

    [ConsoleCommand("cbpjson")]
    public void cbpjson(CCSPlayerController? x, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(x, Perm_Root))
        {
            x.PrintToChat(NotEnoughPermission);
            return;
        }
        if (int.TryParse(info.ArgString.GetArg(0), out var d) == false)
        {
            d = 1;
        }

        var data = GetBattlePassLevelConfig(d);
        var s = JsonConvert.SerializeObject(data, Formatting.None);
        x.PrintToConsole(s);
    }

    [ConsoleCommand("cteamimage")]
    public void cteamimage(CCSPlayerController? x, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(x, Perm_Root))
        {
            x.PrintToChat(NotEnoughPermission);
            return;
        }
        var ent = Utilities.FindAllEntitiesByDesignerName<CCSTeam>("cs_team_manager");

        var ctScore = ent.Where(team => team.Teamname == "CT")
                         .FirstOrDefault();
        x.PrintToChat($"{ctScore.TeamFlagImage}");
        x.PrintToChat($"{ctScore.TeamLogoImage}");
        x.PrintToChat($"{ctScore.TeamMatchStat}");
        x.PrintToChat($"{ctScore.Teamname}");
        x.PrintToChat($"{ctScore.ClanTeamname}");
        x.PrintToChat($"{ctScore.InitialTeamNum}");
    }

    [ConsoleCommand("csutol")]
    public void csutol(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var position = player.PlayerPawn.Value.CBodyComponent?.SceneNode?.Origin;
        string model = player.PlayerPawn.Value.CBodyComponent?.SceneNode?.GetSkeletonInstance()?.ModelState.ModelName ?? string.Empty;

        var sutModel = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic_override");
        sutModel.Target = ""; // TODO : ??
        sutModel.SetModel(model);
        sutModel.Teleport(player.PlayerPawn.Value.AbsOrigin, ANGLE_ZERO, VEC_ZERO);

        sutModel.DispatchSpawn();
        var anim = _random.Next();
        var animStr = "";
        if (anim >= 0.5)
        {
            animStr = "deathpose_lowviolence";
        }
        else
        {
            animStr = "surrender";
        }
        sutModel.IdleAnim = animStr;
        sutModel.AcceptInput("SetAnimation");
        sutModel.AcceptInput("Enable");

        HidePlayer(player);
        SetMoveType(player, MoveType_t.MOVETYPE_NONE);
        RefreshPawn(player);
        AddTimer(6f, () =>
        {
            SetMoveType(player, MoveType_t.MOVETYPE_WALK);
            RefreshPawn(player);
            ShowPlayer(player);
        }, SOM);
        /*
        DispatchKeyValue(Surrender_Prop[client], "targetname", iTarget)
        */
    }

    [ConsoleCommand("cdeath")]
    public void cdeath(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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

    private static bool cDeathActive = false;

    [ConsoleCommand("cdeath2")]
    public void cdeath2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        cDeathActive = !cDeathActive;
    }

    /// <summary>
    /// EventName=player_death
    /// </summary>
    private static bool cplayerJumpActive = false;

    [ConsoleCommand("cplayerJump")]
    public void cplayerJump(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        cplayerJumpActive = !cplayerJumpActive;
    }

    [ConsoleCommand("cwtf")]
    public void cwtf(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        ForceEntInput("func_breakable", "Break", "DropEldenGidiyeah");
    }

    [ConsoleCommand("cbichaha")]
    public void cbichaha(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
        //p.PlayerPawn.Value!.CameraServices!.ViewEntity.Raw = cameraProp.EntityHandle.Raw;
        //Utilities.SetStateChanged(p.PlayerPawn.Value, "CBasePlayerPawn", "m_pCameraServices");

        //var weapon2 = new CBasePlayerWeapon(knife.);
        if (player?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
        {
            foreach (var weapon in player.PlayerPawn.Value.WeaponServices!.MyWeapons)
            {
                if (weapon.Value != null
                    && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                    && weapon.Value.DesignerName != "[null]")
                {
                    if (weapon.Value.DesignerName.Contains("knife")
                       || weapon.Value.DesignerName.Contains("bayonet"))
                    {
                    }
                }
            }
        }
        Server.NextFrame(() =>
        {
            knife.SetModel("models/coop/challenge_coin.vmdl");
        });
        Server.PrintToChatAll("1");
    }

    [ConsoleCommand("cbicdeneme2")]
    public void cbicdeneme2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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
        //CBaseEntity_EmitSoundParams(player, "sounds/player/burn_damage3.vsnd_c", p, v, d);
    }

    [ConsoleCommand("ctakim2")]
    public void takim2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        player!.ChangeTeam(CsTeam.Spectator);
    }

    [ConsoleCommand("ctakim3")]
    public void ctakim3(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        player!.ChangeTeam(CsTeam.Terrorist);
    }

    [ConsoleCommand("ts")]
    public void testses(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
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