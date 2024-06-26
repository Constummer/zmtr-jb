using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using JailbreakExtras.Lib.Database.Models;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, bool> HideFoots = new();

    private void EventPlayerSpawn()
    {
        RegisterEventHandler<EventPlayerSpawn>(CTKitOnPlayerSpawn);

        RegisterEventHandler<EventPlayerSpawn>((@event, _) =>
        {
            try
            {
                if (@event == null)
                    return HookResult.Continue;
                var x = @event.Userid;
                if (ValidateCallerPlayer(x, false)
                    && x?.SteamID != null
                    && x!.SteamID != 0)
                {
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
                    SetStatusClanTag(@event.Userid);

                    var tempUserId = @event?.Userid?.UserId;
                    var tempSteamId = @event?.Userid?.SteamID;
                    if (tempSteamId.HasValue)
                    {
                        AddTimer(0.3f, () =>
                        {
                            GiveSkin(x, tempSteamId);
                        }, SOM);

                        CreateAuraParticle(tempSteamId.Value);
                    }

                    AddTimer(0.5f, () =>
                    {
                        if (ValidateCallerPlayer(x, false) == false) return;
                        if (x.PawnIsAlive && get_health(x) > 0)
                        {
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
        });
    }

    private static void GiveSkin(CCSPlayerController x, ulong? tempSteamId)
    {
        var data = GetPlayerMarketModel(tempSteamId);
        if (ValidateCallerPlayer(x, false) == false) return;
        if (data.Model != null && !data.ChooseRandom)
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
                            SetModelNextServerFrame(x, model.PathToModel);
                            return;
                        }
                    }
                    break;

                case CsTeam.CounterTerrorist:
                    if (string.IsNullOrWhiteSpace(data.Model.DefaultIdCT) == false)
                    {
                        if (int.TryParse(data.Model.DefaultIdCT, out var modelId)
                            && PlayerModels.TryGetValue(modelId, out model))
                        {
                            if (ValidateCallerPlayer(x, false))
                            {
                                SetModelNextServerFrame(x, model.PathToModel);
                                return;
                            }
                        }
                    }
                    break;
            }
        }
        GiveRandomSkin(x);
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
                SetModelNextServerFrame(x, randomModel);
                break;

            case CsTeam.CounterTerrorist:
                randomModel = GetRandomItem(_Config.Model.RandomCTModels);
                if (string.IsNullOrWhiteSpace(randomModel))
                {
                    return;
                }
                if (ValidateCallerPlayer(x, false) == false) return;
                SetModelNextServerFrame(x, randomModel);
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