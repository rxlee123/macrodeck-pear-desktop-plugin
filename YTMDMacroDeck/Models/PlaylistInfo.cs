using Newtonsoft.Json;

namespace YTMDMacroDeck.Models
{
    /// <summary>
    /// Represents a YouTube Music playlist returned by GET /api/v1/playlists.
    /// </summary>
    public class PlaylistInfo
    {
        [JsonProperty("playlistId")]
        public string PlaylistId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// First video ID in the playlist, used for the ytmd:// URI scheme.
        /// May be populated from the playlist details or left empty.
        /// </summary>
        [JsonProperty("firstVideoId")]
        public string FirstVideoId { get; set; }

        public override string ToString()
        {
            return $"{Title} ({Count} tracks)";
        }
    }

    /// <summary>
    /// Configuration saved per PlayPlaylistAction instance.
    /// </summary>
    public class PlaylistActionConfig
    {
        [JsonProperty("playlistId")]
        public string PlaylistId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("firstVideoId")]
        public string FirstVideoId { get; set; }
    }
}
