using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class DontBlockOnGaggedConfig
    {
        [JsonPropertyName("DontBlockOnGaggedCommands")]
        public List<string> DontBlockOnGaggedCommands { get; set; } = new List<string>()
        {
            "kredi",
            "kredim",
            "market",
            "env",
            "envanter",
            "inv",
            "inventory",
            "fk",
            "freekill",
            "rm",
            "rmf",
            "css_1",
            "css_2",
            "css_3",
            "css_4",
            "css_5",
            "css_6",
            "css_7",
            "css_8",
            "css_9",
            "w",
            "uw",
            "dc",
            "discord",
            "grup",
            "magaza",
            "grub",
            "steamgrup",
            "steamgrub",
            "kurucu",
            "owner",
            "adminlik",
            "css_ip"
        };
    }
}