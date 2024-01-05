using System.Text.Json.Serialization;

namespace JailbreakExtras.Lib.Configs
{
    public class DatabaseConfig
    {
        public DatabaseConfig()
        {
        }

        [JsonPropertyName("Username")]
        public string Username { get; set; } = "root";

        [JsonPropertyName("Password")]
        public string Password { get; set; } = "sikimdedeil";

        [JsonPropertyName("Server")]
        public string Server { get; set; } = "127.0.0.1";

        [JsonPropertyName("Port")]
        public string Port { get; set; } = "3306";

        [JsonPropertyName("Database")]
        public string Database { get; set; } = "cs2_extras";
    }
}