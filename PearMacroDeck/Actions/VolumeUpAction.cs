using SuchByte.MacroDeck.ActionButton;
using SuchByte.MacroDeck.Plugins;

namespace PearMacroDeck.Actions
{
    public class VolumeUpAction : PluginAction
    {
        public override string Name => "Volume Up";
        public override string Description => "Increase volume on YouTube Music Desktop";
        public override bool CanConfigure => false;

        public override void Trigger(string clientId, ActionButton actionButton)
        {
            _ = Main.Client.VolumeUpAsync();
        }
    }
}

