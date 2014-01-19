using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Common;

namespace ServerATM.Server
{
    public partial class ServerAgentUI : Form
    {
        private ISO8583_Server _Server;
        const string _YesMessage = "  Service status: Running";
        const string _NoMessage = "  Service status: Stop";

        public ServerAgentUI()
        {
            InitializeComponent();

            _Server = new ISO8583_Server();

            EnableDisableCommandButton(true);

            nicoMain.Icon = new Icon("world.ico");

            btnStart_Click(null, null);                                        
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            Action startListen = new Action(_Server.Start);

            ConfigManager.InitializeConfigurationData();

            startListen.BeginInvoke(null, null);

            tmrMain.Start();

            EnableDisableCommandButton(false);

            this.Cursor = Cursors.Default;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            _Server.Stop();
            tmrMain.Stop();
            nicoMain.Icon = new Icon("world.ico");
            EnableDisableCommandButton(true);

            this.Cursor = Cursors.Default;
        }       

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.Hide();           
        }

        private void EnableDisableCommandButton(bool enableStartButton)
        {
            btnStart.Enabled = enableStartButton;
            btnStop.Enabled = !btnStart.Enabled;            
            ShowServiceStatusMessage(!enableStartButton);           
        }

        private void ShowServiceStatusMessage(bool show)
        {            
            sbrMain.Items[0].TextAlign = ContentAlignment.MiddleCenter;
            sbrMain.Items[0].Text = show ? _YesMessage : _NoMessage;           
        }

        static bool flag = true;
        private void tmrMain_Tick(object sender, EventArgs e)
        {           
            sbrMain.Items[0].ForeColor = sbrMain.Items[0].ForeColor == Color.Black ? Color.Blue : Color.Black;

            if (flag)
            {
                nicoMain.Icon = new Icon("world.ico");
                flag = false;
            }
            else
            {
                nicoMain.Icon = new Icon("earth.ico");
                flag = true;
            }                
         }

        private void nicoMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
            tmrMain.Start();
        }

        private void ServerAgentUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            tmrMain.Stop();
        }        

        private void ServerAgentUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F)
            {                
                ErrorLogUI logForm = new ErrorLogUI();
                logForm.Show();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
                tmrMain.Stop();
            }
            else if (e.KeyCode == Keys.F10)
            {
                if (DialogResult.Yes == MessageBox.Show("Are you sure want to shutdown this service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {                    
                    System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
                    p.Kill();                    
                }
            }
        }    
    
    }//end class

}//end namespace
