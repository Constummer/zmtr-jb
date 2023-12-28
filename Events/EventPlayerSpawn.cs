using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerSpawn()
    {
        RegisterEventHandler((GameEventHandler<EventPlayerSpawn>)((@event, _) =>
        {
            var x = @event.Userid;
            if (ValidateCallerPlayer(x, false)
            && x?.SteamID != null
                && x!.SteamID != 0)
            {
                AddTimer(2f, () =>
                {
                    Server.NextFrame(() =>
                    {
                        @event.Userid.VoiceFlags |= VoiceFlags.Muted;
                    });
                });
                if (HideFoots.TryGetValue(@event.Userid.SteamID, out var _) == false && Config.HideFootsOnConnect)
                {
                    AddTimer(2f, () =>
                    {
                        Server.NextFrame(() =>
                        {
                            AyakGizle(@event.Userid, true);
                        });
                    });
                }
                var data = GetPlayerMarketModel(x?.SteamID);
                if (data.Model == null || data.ChooseRandom)
                {
                    GiveRandomSkin(x);
                }
                else
                {
                    PlayerModel? model;
                    switch (GetTeam(x!))
                    {
                        case CsTeam.Terrorist:
                            if (data.Model.DefaultIdT.HasValue == true)
                            {
                                if (PlayerModels.TryGetValue(data.Model.DefaultIdT.Value, out model))
                                {
                                    SetModelNextServerFrame(x!.PlayerPawn.Value!, model.PathToModel);
                                }
                            }
                            else
                            {
                                GiveRandomSkin(x);
                            }
                            break;

                        case CsTeam.CounterTerrorist:
                            if (data.Model.DefaultIdCT.HasValue == true)
                            {
                                if (PlayerModels.TryGetValue(data.Model.DefaultIdCT.Value, out model))
                                {
                                    SetModelNextServerFrame(x!.PlayerPawn.Value!, model.PathToModel);
                                }
                            }
                            else
                            {
                                GiveRandomSkin(x);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            return HookResult.Continue;
        }));
    }

    private static void GiveRandomSkin(CCSPlayerController? item)
    {
        string randomModel;
        switch (GetTeam(item!))
        {
            case CsTeam.Terrorist:
                randomModel = GetRandomItem(_Config.RandomTModels);
                if (string.IsNullOrWhiteSpace(randomModel))
                {
                    return;
                }
                SetModelNextServerFrame(item!.PlayerPawn.Value!, randomModel);
                break;

            case CsTeam.CounterTerrorist:
                randomModel = GetRandomItem(_Config.RandomCTModels);
                if (string.IsNullOrWhiteSpace(randomModel))
                {
                    return;
                }
                SetModelNextServerFrame(item!.PlayerPawn.Value!, randomModel);
                break;

            default:
                break;
        }
    }

    private static string GetRandomItem(List<string> list)
    {
        if (list.Count == 0)
            return null;
        int randomIndex = _random.Next(list.Count);

        return list[randomIndex];
    }
}