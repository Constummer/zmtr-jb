using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using JailbreakExtras.Lib.Database.Models;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerSpawn()
    {
        RegisterEventHandler<EventPlayerSpawn>((@event, _) =>
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
                       if (ValidateCallerPlayer(x, false))
                       {
                           AyakGizle(x, true);
                       }
                   }, SOM);
                }
                if (x.SteamID != LatestWCommandUser)
                {
                    if (Unmuteds.Contains(x.SteamID) == false)
                    {
                        if (x.Connected == PlayerConnectedState.PlayerConnected)
                            AddTimer(2f, () =>
                            {
                                if (ValidateCallerPlayer(x, false))
                                {
                                    x.VoiceFlags |= VoiceFlags.Muted;
                                }
                            }, SOM);
                    }
                }
                var tempSteamId = @event?.Userid?.SteamID;
                var tempUserId = @event?.Userid?.UserId;
                AddTimer(3f, () =>
                {
                    _ClientQueue.Enqueue(new(tempSteamId ?? 0, tempUserId, "", QueueItemType.OnPlayerSpawn));
                });
                AddTimer(0.5f, () =>
                {
                    if (ValidateCallerPlayer(x, false) == false) return;
                    if (LatestWCommandUser == x.SteamID)
                    {
                        if (ValidateCallerPlayer(x, false) == false) return;
                        SetColour(x, Color.FromArgb(255, 0, 0, 255));
                    }
                    else
                    {
                        if (ValidateCallerPlayer(x, false) == false) return;
                        SetColour(x, DefaultColor);
                    }

                    var data = GetPlayerMarketModel(tempSteamId);
                    if (data.Model == null || data.ChooseRandom)
                    {
                        if (ValidateCallerPlayer(x, false) == false) return;
                        GiveRandomSkin(x);
                    }
                    else
                    {
                        if (ValidateCallerPlayer(x, false) == false) return;
                        PlayerModel? model;
                        switch (GetTeam(x!))
                        {
                            case CsTeam.Terrorist:
                                if (string.IsNullOrWhiteSpace(data.Model.DefaultIdT) == false)
                                {
                                    if (int.TryParse(data.Model.DefaultIdT, out var modelId)
                                        && PlayerModels.TryGetValue(modelId, out model))
                                    {
                                        if (ValidateCallerPlayer(x, false) == false) return;
                                        SetModelNextServerFrame(x!.PlayerPawn.Value!, model.PathToModel);
                                    }
                                }
                                else
                                {
                                    if (ValidateCallerPlayer(x, false) == false) return;
                                    GiveRandomSkin(x);
                                }
                                break;

                            case CsTeam.CounterTerrorist:
                                if (string.IsNullOrWhiteSpace(data.Model.DefaultIdCT) == false)
                                {
                                    if (int.TryParse(data.Model.DefaultIdCT, out var modelId)
                                        && PlayerModels.TryGetValue(modelId, out model))
                                    {
                                        if (ValidateCallerPlayer(x, false) == false) return;
                                        SetModelNextServerFrame(x!.PlayerPawn.Value!, model.PathToModel);
                                    }
                                }
                                else
                                {
                                    if (ValidateCallerPlayer(x, false) == false) return;
                                    GiveRandomSkin(x);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }, SOM);
            }
            return HookResult.Continue;
        });
    }

    private static void GiveRandomSkin(CCSPlayerController? x)
    {
        if (ValidateCallerPlayer(x, false) == false) return;
        string randomModel;
        switch (GetTeam(x!))
        {
            case CsTeam.Terrorist:
                randomModel = GetRandomItem(_Config.Model.RandomTModels);
                if (string.IsNullOrWhiteSpace(randomModel))
                {
                    return;
                }
                if (ValidateCallerPlayer(x, false) == false) return;
                SetModelNextServerFrame(x!.PlayerPawn.Value!, randomModel);
                break;

            case CsTeam.CounterTerrorist:
                randomModel = GetRandomItem(_Config.Model.RandomCTModels);
                if (string.IsNullOrWhiteSpace(randomModel))
                {
                    return;
                }
                if (ValidateCallerPlayer(x, false) == false) return;
                SetModelNextServerFrame(x!.PlayerPawn.Value!, randomModel);
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