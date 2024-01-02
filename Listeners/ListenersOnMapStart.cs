using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnMapStart()
    {
        RegisterListener<Listeners.OnMapStart>(name =>
        {
            ClearAll();
            HideFoots?.Clear();
            HookPlayers?.Clear();
            bUsingPara?.Clear();
            Unmuteds?.Clear();
            LoadAllModels();
        });
    }

    private void LoadAllModels()
    {
        if (PlayerModels != null)
        {
            foreach (var model in PlayerModels)
            {
                if (string.IsNullOrWhiteSpace(model.Value?.PathToModel) == false)
                {
                    Server.PrecacheModel(model.Value.PathToModel);
                }
            }
        }
        if (Config.Model.RandomTModels != null)
        {
            foreach (var item in Config.Model.RandomTModels)
            {
                Server.PrecacheModel(item);
            }
        }
        if (Config.Model.RandomCTModels != null)
        {
            foreach (var item in Config.Model.RandomCTModels)
            {
                Server.PrecacheModel(item);
            }
        }
    }
}