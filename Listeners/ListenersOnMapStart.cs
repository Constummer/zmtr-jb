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
        foreach (var item in RandomCT)
        {
            Server.PrecacheModel(item);
        }
        foreach (var item in RandomT)
        {
            Server.PrecacheModel(item);
        }
    }
}