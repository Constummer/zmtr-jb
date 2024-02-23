using CounterStrikeSharp.API.Modules.Utils;
using System.Text.Json.Serialization;

namespace JailbreakExtras.Lib.Configs
{
    public class MarketConfig
    {
        [JsonPropertyName("MarketModeller")]
        public List<MarketModel> MarketModeller { get; set; } = new List<MarketModel>()
        {
            //NOTE, ID 0 DAN BUYUKSE UNIQUE OLMALI, YANI 2 ADET AYNI DEGER BULUNAMAZ. 2 ADET 1 OLAMAZ
            //NOTE, ID 0 DAN KUCUKSE ISE VARSAYILAN MODELLERDEN RANDOM MODEL VERIR'
            //NOTE, ENABLE DEGERI FALSE ISE MARKET MENUDE GOZUKMEZLER
            new (-2, CsTeam.CounterTerrorist ,"Default", 0, true,  null ),
            new (-1, CsTeam.Terrorist        ,"Default", 0, true,  null ),
            new (1,  CsTeam.CounterTerrorist ,"Beyaz Karakter", 7000, true,  "characters\\models\\kaesar\\mapper\\mapper.vmdl" ),
            new (2,  CsTeam.CounterTerrorist ,"Shrek", 7000, true,  "characters\\models\\stepanof\\stalker_shrek\\shrek.vmdl" ),
            new (4,  CsTeam.Terrorist        ,"Buzul Freddy", 8000, true,  "characters\\models\\nozb1\\freddy_icebear_player_model\\freddy_icebear_player_model.vmdl" ),
            new (5,  CsTeam.CounterTerrorist ,"Huggy", 10000, true,  "characters\\models\\nozb1\\huggy_male_player_model\\huggy_male_player_model.vmdl" ),
            new (6,  CsTeam.Terrorist        ,"Alevli Adam", 10000, true,  "characters\\models\\nozb1\\maalik_infernal_player_model\\maalik_infernal_player_model.vmdl" ),
            new (7,  CsTeam.Terrorist        ,"Pepe", 9000, true,  "characters\\models\\nozb1\\pepe_player_model\\pepe_player_model.vmdl" ),
            new (8,  CsTeam.Terrorist        ,"Noel Baba", 5000, true,  "characters\\models\\nozb1\\santa_player_model\\santa_player_model.vmdl" ),
            new (9,  CsTeam.Terrorist        ,"Yoru", 10000, true,  "characters\\models\\kolka\\yoru\\yoru.vmdl")
        };
    }

    public class MarketModel : ParticleModel
    {
        public MarketModel(int id, CsTeam teamNo, string text, int cost, bool enable, string pathToModel) : base(id, text, cost, enable, pathToModel)
        {
            Id = id;
            TeamNo = teamNo;
            Text = text;
            Cost = cost;
            Enable = enable;
            PathToModel = pathToModel;
        }

        [JsonPropertyName("TeamNo")]
        public CsTeam TeamNo { get; set; } = CsTeam.None;
    }
}