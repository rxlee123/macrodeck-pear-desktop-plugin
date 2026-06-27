using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PearMacroDeck.Models;

namespace PearMacroDeck.Services
{
    /// <summary>
    /// HTTP client wrapper for the YTMD Companion Server API v2.
    /// Handles authentication, player commands, and playlist retrieval.
    /// </summary>
    public class PearClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private PearConfig _config;
        private bool _disposed;

        public bool IsAuthenticated => !string.IsNullOrEmpty(_config?.Token);

        public PearClient()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
            _config = new PearConfig();
        }

        /// <summary>
        /// Updates the client configuration (host, port, token).
        /// </summary>
        public void Configure(PearConfig config)
        {
            _config = config ?? new PearConfig();
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            if (!string.IsNullOrEmpty(_config.Token))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config.Token}");
            }
        }

        #region Authentication

        /// <summary>
        /// Step 1: For Pear Desktop, we don't have a separate code request step.
        /// We just return a dummy code to progress to the next step in the UI.
        /// </summary>
        public async Task<string> RequestAuthCodeAsync()
        {
            return await Task.FromResult("waiting-for-approval");
        }

        /// <summary>
        /// Step 2: Exchange the dummy code for a token by calling /auth/{id}.
        /// This call may take up to 30 seconds as it waits for user approval in YTMD.
        /// </summary>
        public async Task<string> RequestTokenAsync(string code)
        {
            using (var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(35)))
            {
                var response = await PostJsonAsync($"{_config.BaseUrl}/auth/{_config.AppId}", new { }, cts.Token);
                var result = JsonConvert.DeserializeObject<AuthTokenResponse>(response);

                if (!string.IsNullOrEmpty(result?.AccessToken))
                {
                    _config.Token = result.AccessToken;
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config.Token}");
                }

                return result?.AccessToken;
            }
        }

        #endregion

        #region Player Commands

        public async Task PlayPauseAsync() => await PostJsonAsync($"{_config.BaseUrl}/api/v1/toggle-play", new { });
        public async Task NextAsync() => await PostJsonAsync($"{_config.BaseUrl}/api/v1/next", new { });
        public async Task PreviousAsync() => await PostJsonAsync($"{_config.BaseUrl}/api/v1/previous", new { });
        private int? _targetVolume;
        private DateTime _lastVolumeChange = DateTime.MinValue;

        public async Task VolumeUpAsync() => await ChangeVolume(10);
        public async Task VolumeDownAsync() => await ChangeVolume(-10);

        private async Task ChangeVolume(int delta)
        {
            int currentVolume;
            // Use locally cached target volume if we changed it recently,
            // to prevent bouncing back when hitting the button rapidly.
            if ((DateTime.Now - _lastVolumeChange).TotalSeconds < 2 && _targetVolume.HasValue)
            {
                currentVolume = _targetVolume.Value;
            }
            else
            {
                var volumeState = await GetVolumeStateAsync();
                if (volumeState == null) return;
                currentVolume = volumeState.Volume;
            }

            _targetVolume = Math.Max(0, Math.Min(100, currentVolume + delta));
            _lastVolumeChange = DateTime.Now;
            
            await SetVolumeAsync(_targetVolume.Value);
        }
        public async Task MuteAsync() => await PostJsonAsync($"{_config.BaseUrl}/api/v1/toggle-mute", new { });
        public async Task UnmuteAsync() => await PostJsonAsync($"{_config.BaseUrl}/api/v1/toggle-mute", new { });
        public async Task ShuffleAsync() => await PostJsonAsync($"{_config.BaseUrl}/api/v1/shuffle", new { });
        public async Task SetRepeatModeAsync(int mode) => await PostJsonAsync($"{_config.BaseUrl}/api/v1/switch-repeat", new { iteration = mode });
        public async Task SetVolumeAsync(int volume) => await PostJsonAsync($"{_config.BaseUrl}/api/v1/volume", new { volume });

        #endregion

        #region State

        public async Task<SongInfo> GetSongInfoAsync()
        {
            if (!IsAuthenticated) return null;
            try { return JsonConvert.DeserializeObject<SongInfo>(await GetJsonAsync($"{_config.BaseUrl}/api/v1/song-info")); }
            catch { return null; }
        }

        public async Task<VolumeState> GetVolumeStateAsync()
        {
            if (!IsAuthenticated) return null;
            try { return JsonConvert.DeserializeObject<VolumeState>(await GetJsonAsync($"{_config.BaseUrl}/api/v1/volume")); }
            catch { return null; }
        }

        public async Task<RepeatModeState> GetRepeatModeAsync()
        {
            if (!IsAuthenticated) return null;
            try { return JsonConvert.DeserializeObject<RepeatModeState>(await GetJsonAsync($"{_config.BaseUrl}/api/v1/repeat-mode")); }
            catch { return null; }
        }

        public async Task<ShuffleState> GetShuffleStateAsync()
        {
            if (!IsAuthenticated) return null;
            try { return JsonConvert.DeserializeObject<ShuffleState>(await GetJsonAsync($"{_config.BaseUrl}/api/v1/shuffle")); }
            catch { return null; }
        }

        #endregion

        #region Playlists

        /// <summary>
        /// Fetch the user's playlists from YTMD. Note: this may take up to 30s.
        /// </summary>
        public async Task<List<PlaylistInfo>> GetPlaylistsAsync()
        {
            if (!IsAuthenticated) return new List<PlaylistInfo>();

            try
            {
                using (var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(35)))
                {
                    var response = await GetJsonAsync($"{_config.BaseUrl}/api/v1/playlists", cts.Token);
                    return JsonConvert.DeserializeObject<List<PlaylistInfo>>(response)
                           ?? new List<PlaylistInfo>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[YTMD] GetPlaylists failed: {ex.Message}");
                return new List<PlaylistInfo>();
            }
        }

        public void PlayPlaylist(string playlistId, string firstVideoId = null)
        {
            if (!IsAuthenticated) return;

            try
            {
                var payload = new Dictionary<string, string>
                {
                    { "playlistId", playlistId }
                };

                if (!string.IsNullOrEmpty(firstVideoId))
                {
                    payload.Add("videoId", firstVideoId);
                }

                string json = JsonConvert.SerializeObject(payload);
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"{_config.BaseUrl}/api/v1/playPlaylist")
                {
                    Content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json")
                };
                _httpClient.SendAsync(request).Wait();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[YTMD] PlayPlaylist failed: {ex.Message}");
            }
        }

        #endregion

        #region HTTP Helpers

        private async Task<string> PostJsonAsync(string url, object payload,
            System.Threading.CancellationToken cancellationToken = default)
        {
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> GetJsonAsync(string url,
            System.Threading.CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        #endregion

        public void Dispose()
        {
            if (!_disposed)
            {
                _httpClient?.Dispose();
                _disposed = true;
            }
        }
    }
}

