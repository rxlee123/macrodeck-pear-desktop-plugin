using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SuchByte.MacroDeck.Plugins;
using YTMDMacroDeck.Actions;
using YTMDMacroDeck.Models;
using YTMDMacroDeck.Services;
using YTMDMacroDeck.UI;

namespace YTMDMacroDeck
{
    /// <summary>
    /// YTMD Controller plugin for Macro Deck 2.
    /// Controls YouTube Music Desktop App via its Companion Server API v2.
    /// </summary>
    public class Main : MacroDeckPlugin
    {
        /// <summary>
        /// Shared YTMD HTTP client instance.
        /// </summary>
        public static YTMDClient Client { get; private set; }

        /// <summary>
        /// Shared state poller instance.
        /// </summary>
        public static StatePoller Poller { get; private set; }

        public static Main Instance { get; private set; }


        public override bool CanConfigure => true;

        /// <summary>
        /// Called when the plugin is loaded by Macro Deck.
        /// </summary>
        public override void Enable()
        {
            Instance = this;
            // Initialize client and poller
            Client = new YTMDClient();
            Poller = new StatePoller(Client);

            // Load saved config and apply to client
            LoadConfig();

            // Register all available actions
            this.Actions = new List<PluginAction>
            {
                new PlayPauseAction(),
                new NextTrackAction(),
                new PreviousTrackAction(),
                new VolumeUpAction(),
                new VolumeDownAction(),
                new VolumeMuteAction(),
                new ShuffleAction(),
                new RepeatAction(),
                new PlayPlaylistAction(),
            };

            // Start polling if already authorized
            if (Client.IsAuthenticated)
            {
                Poller.Start();
            }
        }

        /// <summary>
        /// Opens the plugin-level configuration form (host, port, auth).
        /// </summary>
        public override void OpenConfigurator()
        {
            using (var form = new ConfiguratorForm(this))
            {
                form.ShowDialog();
            }
        }

        /// <summary>
        /// Load saved configuration and apply to the client.
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                var json = PluginConfiguration.GetValue(this, "config");
                if (!string.IsNullOrEmpty(json))
                {
                    var config = JsonConvert.DeserializeObject<YTMDConfig>(json);
                    Client.Configure(config);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[YTMD] LoadConfig error: {ex.Message}");
            }
        }
    }
}
