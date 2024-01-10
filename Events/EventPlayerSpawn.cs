using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using JailbreakExtras.Lib.Database.Models;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerSpawn()
    {
        RegisterEventHandler((GameEventHandler<EventPlayerSpawn>)((@event, _) =>
        {
            if (@event == null)
                return HookResult.Continue;
            var x = @event.Userid;
            if (ValidateCallerPlayer(x, false)
                && x?.SteamID != null
                && x!.SteamID != 0)
            {
                if (HideFoots.TryGetValue(x.SteamID, out var _) == false && Config.Additional.HideFootsOnConnect)
                {
                    AddTimer(2f, () =>
                   {
                       AyakGizle(x, true);
                   });
                }
                if (x.SteamID != LatestWCommandUser)
                {
                    if (Unmuteds.Contains(x.SteamID) == false)
                    {
                        if (x.Connected == PlayerConnectedState.PlayerConnected)
                            AddTimer(2f, () =>
                            {
                                x.VoiceFlags |= VoiceFlags.Muted;
                            });
                    }
                }
                AddTimer(0.5f, () =>
                {
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
                                if (string.IsNullOrWhiteSpace(data.Model.DefaultIdT) == false)
                                {
                                    if (int.TryParse(data.Model.DefaultIdT, out var modelId)
                                        && PlayerModels.TryGetValue(modelId, out model))
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
                                if (string.IsNullOrWhiteSpace(data.Model.DefaultIdCT) == false)
                                {
                                    if (int.TryParse(data.Model.DefaultIdCT, out var modelId)
                                        && PlayerModels.TryGetValue(modelId, out model))
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
                });
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
                randomModel = GetRandomItem(_Config.Model.RandomTModels);
                if (string.IsNullOrWhiteSpace(randomModel))
                {
                    return;
                }
                SetModelNextServerFrame(item!.PlayerPawn.Value!, randomModel);
                break;

            case CsTeam.CounterTerrorist:
                randomModel = GetRandomItem(_Config.Model.RandomCTModels);
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