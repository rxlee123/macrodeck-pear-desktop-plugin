using SuchByte.MacroDeck.ActionButton;
using SuchByte.MacroDeck.Plugins;

namespace YTMDMacroDeck.Actions
{
    public class RepeatAction : PluginAction
    {
        public override string Name => "Repeat Mode";
        public override string Description => "Cycle repeat mode (None → All → One) on YouTube Music Desktop";
        public override bool CanConfigure => false;

        public override void Trigger(string clientId, ActionButton actionButton)
        {
            // Cycle: 0 (None) → 1 (All) → 2 (One) → 0 (None)
            int currentMode = Main.Poller.LastRepeatMode;
            int nextMode = (currentMode + 1) % 3;
            _ = Main.Client.SetRepeatModeAsync(nextMode);
        }
    }
}
