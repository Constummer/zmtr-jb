using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static readonly List<string> RandomT = new(){
        "characters\\models\\tm_jumpsuit\\tm_jumpsuit_varianta.vmdl",
        "characters\\models\\tm_jumpsuit\\tm_jumpsuit_variantb.vmdl",
        "characters\\models\\tm_jumpsuit\\tm_jumpsuit_variantc.vmdl",
    };

    private static readonly List<string> RandomCT = new()
    {
        "characters\\models\\ctm_gendarmerie\\ctm_gendarmerie_variantc.vmdl",
        "characters\\models\\ctm_swat\\ctm_swat_variante.vmdl",
        "characters\\models\\ctm_gendarmerie\\ctm_gendarmerie_variantd.vmdl",
    };

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
                        PlayerModel model;
                        switch (GetTeam(item))
                        {
                            case CsTeam.Terrorist:
                                if (data.Model.DefaultIdT.HasValue == true)
                                {
                                    if (PlayerModels.TryGetValue(data.Model.DefaultIdT.Value, out model))
                                    {
                                        SetModelNextServerFrame(item.PlayerPawn.Value!, model.PathToModel);
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
                                        SetModelNextServerFrame(item.PlayerPawn.Value!, model.PathToModel);
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
        switch (GetTeam(item))
        {
            case CsTeam.Terrorist:
                randomModel = GetRandomItem(RandomT);
                SetModelNextServerFrame(item.PlayerPawn.Value!, randomModel);
                break;

            case CsTeam.CounterTerrorist:
                randomModel = GetRandomItem(RandomCT);
                SetModelNextServerFrame(item.PlayerPawn.Value!, randomModel);
                break;

            default:
                break;
        }
    }

    private static string GetRandomItem(List<string> list)
    {
        int randomIndex = _random.Next(list.Count);

        return list[randomIndex];
    }
}