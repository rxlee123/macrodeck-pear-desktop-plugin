using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using SuchByte.MacroDeck.Plugins;
using PearMacroDeck.Models;

namespace PearMacroDeck.UI
{
    /// <summary>
    /// Plugin-level configurator form for setting YTMD connection details and authorizing.
    /// </summary>
    public partial class ConfiguratorForm : Form
    {
        private readonly MacroDeckPlugin _plugin;
        private PearConfig _config;

        public ConfiguratorForm(MacroDeckPlugin plugin)
        {
            _plugin = plugin;
            InitializeComponent();
            LoadConfig();
        }

        private void LoadConfig()
        {
            try
            {
                var json = PluginConfiguration.GetValue(_plugin, "config");
                _config = !string.IsNullOrEmpty(json)
                    ? JsonConvert.DeserializeObject<PearConfig>(json)
                    : new PearConfig();
            }
            catch
            {
                _config = new PearConfig();
            }

            txtHost.Text = _config.Host;
            numPort.Value = _config.Port;
            UpdateStatus();
        }

        private void SaveConfig()
        {
            _config.Host = txtHost.Text.Trim();
            _config.Port = (int)numPort.Value;

            var json = JsonConvert.SerializeObject(_config);
            PluginConfiguration.SetValue(_plugin, "config", json);

            // Apply to client
            Main.Client.Configure(_config);
        }

        private void UpdateStatus()
        {
            if (!string.IsNullOrEmpty(_config.Token))
            {
                lblStatus.Text = "✅ Authorized";
                lblStatus.ForeColor = System.Drawing.Color.LimeGreen;
            }
            else
            {
                lblStatus.Text = "❌ Not authorized";
                lblStatus.ForeColor = System.Drawing.Color.Tomato;
            }
        }

        private async void btnAuthorize_Click(object sender, EventArgs e)
        {
            btnAuthorize.Enabled = false;
            lblStatus.Text = "⏳ Requesting code...";
            lblStatus.ForeColor = System.Drawing.Color.Gold;

            try
            {
                // Save current host/port first
                SaveConfig();

                // Step 1: Request auth code
                var code = await Main.Client.RequestAuthCodeAsync();
                if (string.IsNullOrEmpty(code))
                {
                    lblStatus.Text = "❌ Failed to get auth code. Is YTMD running?";
                    lblStatus.ForeColor = System.Drawing.Color.Tomato;
                    return;
                }

                lblStatus.Text = $"⏳ Code: {code} — Approve in YTMD app...";

                // Step 2: Exchange for token (waits for user approval)
                var token = await Main.Client.RequestTokenAsync(code);
                if (!string.IsNullOrEmpty(token))
                {
                    _config.Token = token;
                    SaveConfig();
                    UpdateStatus();

                    // Start polling now that we're authorized
                    Main.Poller.Start();
                }
                else
                {
                    lblStatus.Text = "❌ Authorization denied or timed out";
                    lblStatus.ForeColor = System.Drawing.Color.Tomato;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"❌ Error: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Tomato;
            }
            finally
            {
                btnAuthorize.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

