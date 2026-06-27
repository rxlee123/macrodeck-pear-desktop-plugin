using Newtonsoft.Json;

namespace YTMDMacroDeck.Models
{
    /// <summary>
    /// Plugin configuration persisted via Macro Deck's PluginConfiguration API.
    /// </summary>
    public class YTMDConfig
    {
        [JsonProperty("host")]
        public string Host { get; set; } = "127.0.0.1";

        [JsonProperty("port")]
        public int Port { get; set; } = 8989;

        [JsonProperty("token")]
        public string Token { get; set; } = "";

        [JsonProperty("appId")]
        public string AppId { get; set; } = "macrodeck-ytmd";

        /// <summary>
        /// Returns the base URL for the YTMD Companion Server.
        /// </summary>
        [JsonIgnore]
        public string BaseUrl => $"http://{Host}:{Port}";
    }
}
