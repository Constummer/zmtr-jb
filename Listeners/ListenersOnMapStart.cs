using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnMapStart()
    {
        RegisterListener<Listeners.OnMapStart>(name =>
        {
            TelliSeferActive = false;
            PatronuKoruActive = false;
            PatronuKoruTLider = null;
            PatronuKoruCTLider = null;
            PatronuKoruCTKoruma1 = null;
            PatronuKoruCTKoruma2 = null;
            PatronuKoruTKoruma1 = null;
            PatronuKoruTKoruma2 = null;
            MapStartTime = DateTime.UtcNow;
            Server.ExecuteCommand("mp_force_pick_time 3000");
            Server.ExecuteCommand("mp_autoteambalance 0");
            Server.ExecuteCommand("mp_equipment_reset_rounds 1");
            Server.ExecuteCommand("mp_t_default_secondary \"\" ");
            ClearAll();
            CreditModifier = 1;
            TPModifier = 1;
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