using System;
using System.Diagnostics;
using Newtonsoft.Json;
using SuchByte.MacroDeck.ActionButton;
using SuchByte.MacroDeck.GUI;
using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Plugins;
using YTMDMacroDeck.Models;
using YTMDMacroDeck.UI;

namespace YTMDMacroDeck.Actions
{
    /// <summary>
    /// Configurable action that plays a specific YouTube Music playlist.
    /// Each button instance can be assigned to a different playlist.
    /// </summary>
    public class PlayPlaylistAction : PluginAction
    {
        public override string Name => "Play Playlist";
        public override string Description => "Play a specific YouTube Music playlist";
        public override bool CanConfigure => true;

        /// <summary>
        /// Returns the per-action configurator (shown when user right-clicks the button).
        /// </summary>
        public override ActionConfigControl GetActionConfigControl(ActionConfigurator actionConfigurator)
        {
            return new PlaylistActionConfigurator(this, actionConfigurator);
        }

        /// <summary>
        /// Triggered when the user presses this button.
        /// Reads the saved playlist config and starts playing it.
        /// </summary>
        public override void Trigger(string clientId, ActionButton actionButton)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Configuration))
                {
                    Debug.WriteLine("[YTMD] PlayPlaylist: No playlist configured for this button.");
                    return;
                }

                var config = JsonConvert.DeserializeObject<PlaylistActionConfig>(this.Configuration);
                if (config == null || string.IsNullOrEmpty(config.PlaylistId))
                {
                    Debug.WriteLine("[YTMD] PlayPlaylist: Invalid configuration.");
                    return;
                }

                Main.Client.PlayPlaylist(config.PlaylistId, config.FirstVideoId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[YTMD] PlayPlaylist trigger error: {ex.Message}");
            }
        }
    }
}
