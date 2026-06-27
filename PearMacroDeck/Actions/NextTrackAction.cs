using SuchByte.MacroDeck.ActionButton;
using SuchByte.MacroDeck.Plugins;

namespace PearMacroDeck.Actions
{
    public class NextTrackAction : PluginAction
    {
        public override string Name => "Next Track";
        public override string Description => "Skip to the next track on YouTube Music Desktop";
        public override bool CanConfigure => false;

        public override void Trigger(string clientId, ActionButton actionButton)
        {
            _ = Main.Client.NextAsync();
        }
    }
}

