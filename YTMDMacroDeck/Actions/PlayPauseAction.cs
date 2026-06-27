using SuchByte.MacroDeck.ActionButton;
using SuchByte.MacroDeck.Plugins;
using System.Diagnostics;

namespace YTMDMacroDeck.Actions
{
    public class PlayPauseAction : PluginAction
    {
        public override string Name => "Play / Pause";
        public override string Description => "Toggle play/pause on YouTube Music Desktop";
        public override bool CanConfigure => false;

        public override void Trigger(string clientId, ActionButton actionButton)
        {
            _ = Main.Client.PlayPauseAsync();
        }
    }
}
