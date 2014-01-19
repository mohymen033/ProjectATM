namespace ServerATM.Server
{
    partial class ServerAgentUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerAgentUI));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpgService = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnHide = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.sbrMain = new System.Windows.Forms.StatusStrip();
            this.StatusMessage1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.nicoMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.tmrMain = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tpgService.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.sbrMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpgService);
            this.tabControl1.Location = new System.Drawing.Point(7, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(321, 182);
            this.tabControl1.TabIndex = 4;
            // 
            // tpgService
            // 
            this.tpgService.Controls.Add(this.pictureBox1);
            this.tpgService.Controls.Add(this.groupBox1);
            this.tpgService.Location = new System.Drawing.Point(4, 22);
            this.tpgService.Name = "tpgService";
            this.tpgService.Padding = new System.Windows.Forms.Padding(3);
            this.tpgService.Size = new System.Drawing.Size(313, 156);
            this.tpgService.TabIndex = 0;
            this.tpgService.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::ServerATM.Properties.Resources.Power;
            this.pictureBox1.Image = global::ServerATM.Properties.Resources.crystal4_4_3;
            this.pictureBox1.Location = new System.Drawing.Point(7, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(176, 143);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Controls.Add(this.btnHide);
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Location = new System.Drawing.Point(190, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(117, 147);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // btnStart
            // 
            this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStart.Location = new System.Drawing.Point(9, 14);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnHide
            // 
            this.btnHide.Image = ((System.Drawing.Image)(resources.GetObject("btnHide.Image")));
            this.btnHide.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHide.Location = new System.Drawing.Point(9, 111);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(100, 23);
            this.btnHide.TabIndex = 2;
            this.btnHide.Text = "Close";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // btnStop
            // 
            this.btnStop.Image = global::ServerATM.Properties.Resources.BULLSEYE;
            this.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStop.Location = new System.Drawing.Point(9, 63);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // sbrMain
            // 
            this.sbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusMessage1});
            this.sbrMain.Location = new System.Drawing.Point(0, 192);
            this.sbrMain.Name = "sbrMain";
            this.sbrMain.Size = new System.Drawing.Size(334, 22);
            this.sbrMain.TabIndex = 5;
            // 
            // StatusMessage1
            // 
            this.StatusMessage1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.StatusMessage1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.StatusMessage1.Name = "StatusMessage1";
            this.StatusMessage1.Size = new System.Drawing.Size(0, 17);
            this.StatusMessage1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nicoMain
            // 
            this.nicoMain.Text = "Ababil ATM Service Agent";
            this.nicoMain.Visible = true;
            this.nicoMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.nicoMain_MouseDoubleClick);
            // 
            // tmrMain
            // 
            this.tmrMain.Interval = 800;
            this.tmrMain.Tick += new System.EventHandler(this.tmrMain_Tick);
            // 
            // ServerAgentUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 214);
            this.Controls.Add(this.sbrMain);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServerAgentUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ababil ATM Server Agent ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerAgentUI_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ServerAgentUI_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tpgService.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.sbrMain.ResumeLayout(false);
            this.sbrMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpgService;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.StatusStrip sbrMain;
        private System.Windows.Forms.ToolStripStatusLabel StatusMessage1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NotifyIcon nicoMain;
        private System.Windows.Forms.Timer tmrMain;


    }
}