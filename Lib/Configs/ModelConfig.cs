using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class ModelConfig
    {
        [JsonPropertyName("RandomTModels")]
        public List<string> RandomTModels { get; set; } = new(){
            "characters\\models\\tm_jumpsuit\\tm_jumpsuit_varianta.vmdl",
            "characters\\models\\tm_jumpsuit\\tm_jumpsuit_variantb.vmdl",
            "characters\\models\\tm_jumpsuit\\tm_jumpsuit_variantc.vmdl",
        };

        [JsonPropertyName("RandomCTModels")]
        public List<string> RandomCTModels { get; set; } = new()
        {
            "characters\\models\\ctm_gendarmerie\\ctm_gendarmerie_variantc.vmdl",
            "characters\\models\\ctm_swat\\ctm_swat_variante.vmdl",
            "characters\\models\\ctm_gendarmerie\\ctm_gendarmerie_variantd.vmdl",
        };

        [JsonPropertyName("CustomModelActive")]
        public bool CustomModelActive { get; set; } = true;

        [JsonPropertyName("SetModelActive")]
        public bool SetModelActive { get; set; } = true;

        [JsonPropertyName("SelectedModelIdT")]
        public int SelectedModelIdT { get; set; } = 18;

        [JsonPropertyName("SelectedModelIdCT")]
        public int SelectedModelIdCT { get; set; } = 1;
    }
}