using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SuchByte.MacroDeck.Variables;
using PearMacroDeck.Models;

namespace PearMacroDeck.Services
{
    /// <summary>
    /// Background poller that periodically fetches the YTMD player state
    /// and updates Macro Deck variables.
    /// </summary>
    public class StatePoller : IDisposable
    {
        private readonly PearClient _client;
        private Timer _timer;
        private bool _disposed;
        private bool _isPolling;

        private const int PollIntervalMs = 2000; // 2 seconds
        private const string PluginName = "PearMacroDeck";

        // Track the last known repeat mode to enable cycling
        public int LastRepeatMode { get; private set; }
        public bool LastMuteState { get; private set; }

        public StatePoller(PearClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Start polling for player state updates.
        /// </summary>
        public void Start()
        {
            if (_timer != null) return;

            _timer = new Timer(OnTimerTick, null, 0, PollIntervalMs);
        }

        /// <summary>
        /// Stop polling.
        /// </summary>
        public void Stop()
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            _timer?.Dispose();
            _timer = null;
        }

        private async void OnTimerTick(object state)
        {
            if (_isPolling || !_client.IsAuthenticated) return;

            _isPolling = true;
            try
            {
                var songInfoTask = _client.GetSongInfoAsync();
                var volumeTask = _client.GetVolumeStateAsync();
                var repeatTask = _client.GetRepeatModeAsync();
                var shuffleTask = _client.GetShuffleStateAsync();

                await Task.WhenAll(songInfoTask, volumeTask, repeatTask, shuffleTask);

                UpdateVariables(songInfoTask.Result, volumeTask.Result, repeatTask.Result, shuffleTask.Result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[YTMD] Poll error: {ex.Message}");
            }
            finally
            {
                _isPolling = false;
            }
        }

        private void UpdateVariables(SongInfo songInfo, VolumeState volume, RepeatModeState repeatMode, ShuffleState shuffle)
        {
            try
            {
                if (songInfo != null)
                {
                    SetVariable("pear_title", songInfo.Title ?? "", VariableType.String);
                    SetVariable("pear_artist", songInfo.Artist ?? "", VariableType.String);
                    SetVariable("pear_album", songInfo.Album ?? "", VariableType.String);
                    SetVariable("pear_is_playing", (!songInfo.IsPaused).ToString(), VariableType.Bool);
                }

                if (volume != null)
                {
                    SetVariable("pear_volume", volume.Volume.ToString(), VariableType.Integer);
                    LastMuteState = volume.IsMuted;
                }

                if (shuffle != null)
                {
                    SetVariable("pear_is_shuffled", shuffle.IsShuffled.ToString(), VariableType.Bool);
                }

                if (repeatMode != null)
                {
                    SetVariable("pear_repeat_mode", repeatMode.Mode ?? "NONE", VariableType.String);

                    // Cache for action use
                    int repeatModeInt = 0;
                    if (repeatMode.Mode == "ALL") repeatModeInt = 1;
                    else if (repeatMode.Mode == "ONE") repeatModeInt = 2;
                    LastRepeatMode = repeatModeInt;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[YTMD] Variable update error: {ex.Message}");
            }
        }

        private void SetVariable(string name, string value, VariableType type)
        {
            try
            {
                VariableManager.SetValue(name, value, type, Main.Instance, false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[YTMD] SetVariable '{name}' error: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Stop();
                _disposed = true;
            }
        }
    }
}

