using SuchByte.MacroDeck.ActionButton;
using SuchByte.MacroDeck.Plugins;

namespace YTMDMacroDeck.Actions
{
    public class ShuffleAction : PluginAction
    {
        public override string Name => "Shuffle";
        public override string Description => "Toggle shuffle mode on YouTube Music Desktop";
        public override bool CanConfigure => false;

        public override void Trigger(string clientId, ActionButton actionButton)
        {
            _ = Main.Client.ShuffleAsync();
        }
    }
}
