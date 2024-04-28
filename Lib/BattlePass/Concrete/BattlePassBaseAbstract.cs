using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using MySqlConnector;
using System.Text.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public partial class BattlePassBase
    {
        internal static void GiveReward(BattlePassBase data, CCSPlayerController player)
        {
            if (data.Other != null)
            {
                player.PrintToChat($"{Prefix} {CC.B}{data.Other}");
            }
            if (data.Credit != 0)
            {
                try
                {
                    var marketData = GetPlayerMarketModel(player.SteamID);
                    if (marketData.Model != null)
                    {
                        marketData.Model.Credit += data.Credit;
                        PlayerMarketModels[player.SteamID] = marketData.Model;
                        player.PrintToChat($"{Prefix} {CC.B}{data.Credit} {CC.W}KREDI KAZANDIN!");
                        player.PrintToChat($"{Prefix} {CC.W}Mevcut Kredin = {CC.B}{marketData.Model.Credit}{CC.R}");
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (data.TP != 0)
            {
                try
                {
                    if (PlayerLevels.TryGetValue(player.SteamID, out var level))
                    {
                        level.Xp += data.TP;
                        PlayerLevels[player.SteamID] = level;
                        player.PrintToChat($"{Prefix} {CC.B}{data.TP} {CC.W}TP KAZANDIN!");
                        player.PrintToChat($"{Prefix} {CC.W} Mevcut TP ={CC.B} {level.Xp}");
                    }
                    else
                    {
                        player.PrintToChat($"{Prefix} {CC.W}Seviyen yok, seviye alabilmek için  {CC.DR}!slotol {CC.W},{CC.DR} !seviyeol {CC.W}yazabilirsin!");
                        player.PrintToChat($"{Prefix} {CC.R}TP ÖDÜLÜNÜ ALABİLMEK İÇİN DİSCORD'DAN TİCKET AÇMALISIN !");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            try
            {
                using (var con = Connection())
                {
                    if (con != null)
                    {
                        var conf = GetBattlePassLevelConfig(data.Level + 1);
                        var cmd = new MySqlCommand(@$"INSERT INTO `PlayerBattlePass`
                                      (SteamId,Level,Config,Completed)
                                      VALUES (@SteamId,@Level,@Config,0);", con);

                        cmd.Parameters.AddWithValue("@SteamId", player.SteamID);
                        cmd.Parameters.AddWithValue("@Level", conf.Level);
                        cmd.Parameters.AddWithValue("@Config", JsonSerializer.Serialize(conf));
                        cmd.ExecuteNonQuery();
                        conf.SteamId = player.SteamID;
                        BattlePassDatas[conf.SteamId] = conf;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        internal static void RoundTWin()
        {
            GetPlayers(CsTeam.Terrorist)
                  .ToList()
                  .ForEach(x =>
                  {
                      if (BattlePassDatas.TryGetValue(x.SteamID, out var battlePassData))
                      {
                          battlePassData?.OnRoundTWinCommand();
                      }
                  });
        }

        internal static void RoundCTWin()
        {
            GetPlayers(CsTeam.Terrorist)
                  .ToList()
                  .ForEach(x =>
                  {
                      if (BattlePassDatas.TryGetValue(x.SteamID, out var battlePassData))
                      {
                          battlePassData?.OnRoundCTWinCommand();
                      }
                  });
        }

        internal static void EventPlayerDeath(EventPlayerDeath? @event)
        {
            if (BattlePassDatas.TryGetValue(@event.Attacker.SteamID, out var battlePassData))
            {
                var deadOne = @event.Userid;
                var attacker = @event.Attacker;
                if (GetTeam(deadOne) != GetTeam(attacker))
                {
                    if (@event.Noscope)
                    {
                        battlePassData.EventNoScopeKill();
                    }
                    switch (@event.Weapon)
                    {
                        case "p90":
                            battlePassData.EventP90Kill();
                            break;

                        case "ak47":
                            battlePassData.EventAK47Kill();
                            break;

                        case "awp":
                            battlePassData.EventAWPKill();
                            break;

                        case "mag7":
                            battlePassData.EventMAG7Kill();
                            break;

                        case "m4a1":
                            battlePassData.EventM4A4Kill();
                            break;

                        case "ssg08":
                            battlePassData.EventSSG08Kill();
                            break;

                        default:
                            break;
                    }
                    if (@event.Weapon.Contains("knife") || @event.Weapon.Contains("bayonet"))
                    {
                        battlePassData.EventKnifeKill();
                    }
                }

                if (GetTeam(deadOne) == CsTeam.CounterTerrorist &&
                GetTeam(attacker) == CsTeam.Terrorist)
                {
                    //t->ct
                    battlePassData.EventCTKilled();
                }
                else if (GetTeam(deadOne) == CsTeam.Terrorist &&
                    GetTeam(attacker) == CsTeam.CounterTerrorist)
                {
                    //ct->t
                    battlePassData.EventTKilled();
                }
                else if (GetTeam(deadOne) == CsTeam.CounterTerrorist &&
                    GetTeam(attacker) == CsTeam.Terrorist &&
                    deadOne.SteamID == LatestWCommandUser)
                {
                    //w
                    battlePassData.EventWKilled();
                }
            }
        }
    }
}