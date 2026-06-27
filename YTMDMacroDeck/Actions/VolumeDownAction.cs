using SuchByte.MacroDeck.ActionButton;
using SuchByte.MacroDeck.Plugins;

namespace YTMDMacroDeck.Actions
{
    public class VolumeDownAction : PluginAction
    {
        public override string Name => "Volume Down";
        public override string Description => "Decrease volume on YouTube Music Desktop";
        public override bool CanConfigure => false;

        public override void Trigger(string clientId, ActionButton actionButton)
        {
            _ = Main.Client.VolumeDownAsync();
        }
    }
}
