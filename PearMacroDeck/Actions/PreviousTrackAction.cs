using SuchByte.MacroDeck.ActionButton;
using SuchByte.MacroDeck.Plugins;

namespace PearMacroDeck.Actions
{
    public class PreviousTrackAction : PluginAction
    {
        public override string Name => "Previous Track";
        public override string Description => "Go to the previous track on YouTube Music Desktop";
        public override bool CanConfigure => false;

        public override void Trigger(string clientId, ActionButton actionButton)
        {
            _ = Main.Client.PreviousAsync();
        }
    }
}

