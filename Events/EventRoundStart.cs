using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static DateTime RoundStartTime = DateTime.UtcNow;

    private void EventRoundStart()
    {
        RegisterEventHandler((GameEventHandler<EventRoundStart>)((@event, _) =>
        {
            try
            {
                RoundStartTime = DateTime.UtcNow;
                Server.ExecuteCommand("mp_force_pick_time 3000");
                if (PatronuKoruActive)
                {
                    PatronuKoruRoundStart();
                }
                else if (TelliSeferActive)
                {
                    TelliogluVsSeferogluRoundStart();
                }
                else
                {
                    Server.ExecuteCommand("mp_autoteambalance 0");
                    RoundDefaultCommands();
                }
                PrepareRoundDefaults();
                Server.ExecuteCommand("mp_equipment_reset_rounds 1");
                Server.ExecuteCommand("mp_t_default_secondary \"\" ");
                Ruletv2RoundStart();
                try
                {
                    var ent = Utilities.FindAllEntitiesByDesignerName<CCSTeam>("cs_team_manager");
                    CTWin = ent.Where(team => team.Teamname == "CT")
                                     .Select(team => team.Score)
                                     .FirstOrDefault();

                    TWin = ent.Where(team => team.Teamname == "TERRORIST")
                                    .Select(team => team.Score)
                                    .FirstOrDefault();
                }
                catch (Exception e)
                {
                }

                ClearAll();
                CoinAfterNewCommander();
                AddTimer(1.0f, () =>
                {
                    CoinGo = true;
                }, SOM);
                HookDisabled = true;

                foreach (var item in RoundEndParticles)
                {
                    if (item != null && item.IsValid)
                    {
                        item.Remove();
                    }
                }
                RoundEndParticles = new();
                if (!PatronuKoruActive && !TelliSeferActive)
                {
                    AddTimer(5.0f, () =>
                    {
                        HookDisabled = false;
                    }, SOM);
                }

                for (int i = 0; i < Server.MaxPlayers; i++)
                {
                    var x = Utilities.GetPlayerFromSlot(i);

                    if (x == null)
                        continue;
                    if (!x.IsValid || x.UserId == -1)
                        continue;

                    var tempSteamId = x.SteamID;
                    var tempName = x.PlayerName;
                    var tempUserId = x.UserId;
                    if (ValidateCallerPlayer(x, false) == false) continue;

                    AddTimer(20f, () =>
                    {
                        if (tempSteamId != 0)
                        {
                            GivePlayerRewards(tempSteamId, tempName);
                        }
                    }, SOM);
                    AddTimer(0.5f, () =>
                    {
                        if (tempUserId != -1)
                        {
                            CreateParachute(tempUserId, tempSteamId);
                        }
                        if (tempSteamId != 0 && !PatronuKoruActive && !TelliSeferActive)
                        {
                            CreateAuraParticle(tempSteamId);
                        }
                    }, SOM);
                }
                return HookResult.Continue;
            }
            catch (Exception e)
            {
                ConsMsg(e.Message);
                return HookResult.Continue;
            }
        }));
    }
}