namespace YTMDMacroDeck.UI
{
    partial class PlaylistActionConfigurator
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblCurrentPlaylist;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox cmbPlaylists;
        private System.Windows.Forms.Label lblInfo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblCurrentPlaylist = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cmbPlaylists = new System.Windows.Forms.ComboBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblHeader
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(8, 8);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(200, 20);
            this.lblHeader.Text = "🎵 Select a Playlist";

            // lblCurrentPlaylist
            this.lblCurrentPlaylist.AutoSize = true;
            this.lblCurrentPlaylist.ForeColor = System.Drawing.Color.LightGray;
            this.lblCurrentPlaylist.Location = new System.Drawing.Point(8, 35);
            this.lblCurrentPlaylist.Name = "lblCurrentPlaylist";
            this.lblCurrentPlaylist.Size = new System.Drawing.Size(150, 15);
            this.lblCurrentPlaylist.Text = "Current: (none)";

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(8, 60);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(340, 30);
            this.btnRefresh.Text = "🔄 Fetch Playlists from YTMD";
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // cmbPlaylists
            this.cmbPlaylists.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cmbPlaylists.Location = new System.Drawing.Point(8, 100);
            this.cmbPlaylists.Name = "cmbPlaylists";
            this.cmbPlaylists.Size = new System.Drawing.Size(340, 23);
            this.cmbPlaylists.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.cmbPlaylists.ForeColor = System.Drawing.Color.White;
            this.cmbPlaylists.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            // lblInfo
            this.lblInfo.AutoSize = true;
            this.lblInfo.ForeColor = System.Drawing.Color.Gray;
            this.lblInfo.Location = new System.Drawing.Point(8, 132);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(300, 15);
            this.lblInfo.Text = "Click \"Fetch Playlists\" to load your YouTube Music playlists";

            // PlaylistActionConfigurator
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.lblCurrentPlaylist);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.cmbPlaylists);
            this.Controls.Add(this.lblInfo);
            this.Name = "PlaylistActionConfigurator";
            this.Size = new System.Drawing.Size(360, 160);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
