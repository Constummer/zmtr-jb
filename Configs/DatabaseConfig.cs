using System.Text.Json.Serialization;

namespace JailbreakExtras
{
    public class DatabaseConfig
    {
        public DatabaseConfig()
        {
        }

        [JsonPropertyName("Username")]
        public String Username { get; set; } = "root";

        [JsonPropertyName("Password")]
        public String Password { get; set; } = "sikimdedeil";

        [JsonPropertyName("Server")]
        public String Server { get; set; } = "127.0.0.1";

        [JsonPropertyName("Port")]
        public String Port { get; set; } = "3306";

        [JsonPropertyName("Database")]
        public String Database { get; set; } = "cs2_extras";
    }
}