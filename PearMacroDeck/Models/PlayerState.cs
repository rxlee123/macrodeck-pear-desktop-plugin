using Newtonsoft.Json;

namespace PearMacroDeck.Models
{
    /// <summary>
    /// Represents the current player state returned by GET /api/v1/state.
    /// </summary>
    public class SongInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("album")]
        public string Album { get; set; }

        [JsonProperty("isPaused")]
        public bool IsPaused { get; set; }

        [JsonProperty("songDuration")]
        public double SongDuration { get; set; }

        [JsonProperty("elapsedSeconds")]
        public double ElapsedSeconds { get; set; }
    }

    public class VolumeState
    {
        [JsonProperty("state")]
        public int Volume { get; set; }

        [JsonProperty("isMuted")]
        public bool IsMuted { get; set; }
    }

    public class ShuffleState
    {
        [JsonProperty("state")]
        public bool IsShuffled { get; set; }
    }

    public class RepeatModeState
    {
        [JsonProperty("mode")]
        public string Mode { get; set; } // NONE, ONE, ALL
    }

    public class AuthTokenResponse
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }
}

