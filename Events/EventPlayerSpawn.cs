using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerSpawn()
    {
        RegisterEventHandler((GameEventHandler<EventPlayerSpawn>)((@event, _) =>
        {
            foreach (var item in GetPlayers())
            {
                if (ValidateCallerPlayer(item, false)
                    && item?.SteamID != null
                    && item!.SteamID != 0)
                {
                    var data = GetPlayerMarketModel(item?.SteamID);
                    if (data.Model == null || data.ChooseRandom)
                    {
                        GiveRandomSkin(item);
                    }
                    else
                    {
                        PlayerModel? model;
                        switch (GetTeam(item!))
                        {
                            case CsTeam.Terrorist:
                                if (data.Model.DefaultIdT.HasValue == true)
                                {
                                    if (PlayerModels.TryGetValue(data.Model.DefaultIdT.Value, out model))
                                    {
                                        SetModelNextServerFrame(item!.PlayerPawn.Value!, model.PathToModel);
                                    }
                                }
                                else
                                {
                                    GiveRandomSkin(item);
                                }
                                break;

                            case CsTeam.CounterTerrorist:
                                if (data.Model.DefaultIdCT.HasValue == true)
                                {
                                    if (PlayerModels.TryGetValue(data.Model.DefaultIdCT.Value, out model))
                                    {
                                        SetModelNextServerFrame(item!.PlayerPawn.Value!, model.PathToModel);
                                    }
                                }
                                else
                                {
                                    GiveRandomSkin(item);
                                }
                                break;

                            default:
                                break;
                        }
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