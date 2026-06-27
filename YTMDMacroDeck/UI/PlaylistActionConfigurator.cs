using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Newtonsoft.Json;
using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.GUI;
using SuchByte.MacroDeck.Plugins;
using YTMDMacroDeck.Models;

namespace YTMDMacroDeck.UI
{
    /// <summary>
    /// Per-action configurator for PlayPlaylistAction.
    /// Lets users pick a playlist from their YTMD library.
    /// </summary>
    public partial class PlaylistActionConfigurator : ActionConfigControl
    {
        private readonly PluginAction _action;
        private readonly ActionConfigurator _actionConfigurator;
        private List<PlaylistInfo> _playlists = new List<PlaylistInfo>();
        private PlaylistActionConfig _currentConfig;

        public PlaylistActionConfigurator(PluginAction action, ActionConfigurator actionConfigurator)
        {
            _action = action;
            _actionConfigurator = actionConfigurator;
            InitializeComponent();
            LoadExistingConfig();
        }

        private void LoadExistingConfig()
        {
            try
            {
                if (!string.IsNullOrEmpty(_action.Configuration))
                {
                    _currentConfig = JsonConvert.DeserializeObject<PlaylistActionConfig>(_action.Configuration);
                    if (_currentConfig != null)
                    {
                        lblCurrentPlaylist.Text = $"Current: {_currentConfig.Title}";
                    }
                }
            }
            catch
            {
                _currentConfig = null;
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!Main.Client.IsAuthenticated)
            {
                lblInfo.Text = "❌ Not authorized. Configure the plugin first.";
                lblInfo.ForeColor = System.Drawing.Color.Tomato;
                return;
            }

            btnRefresh.Enabled = false;
            lblInfo.Text = "⏳ Fetching playlists (may take up to 30s)...";
            lblInfo.ForeColor = System.Drawing.Color.Gold;

            try
            {
                _playlists = await Main.Client.GetPlaylistsAsync();

                cmbPlaylists.Items.Clear();
                foreach (var playlist in _playlists)
                {
                    cmbPlaylists.Items.Add(playlist);
                }

                if (_playlists.Count > 0)
                {
                    cmbPlaylists.SelectedIndex = 0;
                    lblInfo.Text = $"✅ Found {_playlists.Count} playlists";
                    lblInfo.ForeColor = System.Drawing.Color.LimeGreen;
                }
                else
                {
                    lblInfo.Text = "⚠️ No playlists found";
                    lblInfo.ForeColor = System.Drawing.Color.Gold;
                }
            }
            catch (Exception ex)
            {
                lblInfo.Text = $"❌ Error: {ex.Message}";
                lblInfo.ForeColor = System.Drawing.Color.Tomato;
            }
            finally
            {
                btnRefresh.Enabled = true;
            }
        }

        /// <summary>
        /// Called by Macro Deck when the user clicks "OK" in the action configurator.
        /// Must return true if the configuration is valid.
        /// </summary>
        public override bool OnActionSave()
        {
            if (cmbPlaylists.SelectedItem is PlaylistInfo selected)
            {
                var config = new PlaylistActionConfig
                {
                    PlaylistId = selected.PlaylistId,
                    Title = selected.Title,
                    Thumbnail = selected.Thumbnail,
                    FirstVideoId = selected.FirstVideoId
                };

                _action.Configuration = JsonConvert.SerializeObject(config);
                _action.ConfigurationSummary = $"▶ {selected.Title}";
                return true;
            }
            else if (!string.IsNullOrWhiteSpace(cmbPlaylists.Text))
            {
                string input = cmbPlaylists.Text.Trim();
                string playlistId = input;
                string firstVideoId = null;

                // Simple check if it's a URL
                if (input.Contains("list="))
                {
                    var parts = input.Split(new[] { "list=" }, StringSplitOptions.None);
                    if (parts.Length > 1)
                    {
                        playlistId = parts[1].Split('&')[0];
                    }
                    if (input.Contains("v="))
                    {
                        var vParts = input.Split(new[] { "v=" }, StringSplitOptions.None);
                        if (vParts.Length > 1)
                        {
                            firstVideoId = vParts[1].Split('&')[0];
                        }
                    }
                }

                var config = new PlaylistActionConfig
                {
                    PlaylistId = playlistId,
                    Title = "Manual Playlist",
                    FirstVideoId = firstVideoId
                };

                _action.Configuration = JsonConvert.SerializeObject(config);
                _action.ConfigurationSummary = $"▶ {playlistId}";
                return true;
            }

            // If no new selection but existing config, keep it
            if (_currentConfig != null)
            {
                return true;
            }

            System.Windows.Forms.MessageBox.Show("Please select a playlist.", "YTMD",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
    }
}
