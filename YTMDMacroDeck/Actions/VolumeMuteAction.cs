using SuchByte.MacroDeck.ActionButton;
using SuchByte.MacroDeck.Plugins;

namespace YTMDMacroDeck.Actions
{
    public class VolumeMuteAction : PluginAction
    {
        public override string Name => "Mute / Unmute";
        public override string Description => "Toggle mute on YouTube Music Desktop";
        public override bool CanConfigure => false;

        public override void Trigger(string clientId, ActionButton actionButton)
        {
            // Toggle mute based on last known state
            if (Main.Poller.LastMuteState)
            {
                _ = Main.Client.UnmuteAsync();
            }
            else
            {
                _ = Main.Client.MuteAsync();
            }
        }
    }
}
