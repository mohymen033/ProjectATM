using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using System.IO;


namespace ServerATM.Server
{
    public partial class ErrorLogUI : Form
    {       
        public ErrorLogUI()
        {
            InitializeComponent();

            //grdData.Dock = DockStyle.Fill;
            //rtxtMain.Dock = DockStyle.Fill;
        }

        private void ErrorLogUI_Load(object sender, EventArgs e)
        {
            try
            {
                if (ConfigManager.ErrorLogType == ErrorLogType.Xml)
                {
                    grdData.Visible = true;
                    grdData.Dock = DockStyle.Fill;
                    DataSet ds = new DataSet();
                    ds.ReadXml(ConfigManager.ErrorLogFileXMLPath);
                    grdData.DataSource = ds.Tables[0];

                    if (grdData.Columns[0] != null && grdData.Columns[1] != null)
                    {
                        grdData.Columns[0].Width = 135;
                        grdData.Columns[1].Width = 1130;
                    }
                }
                else if (ConfigManager.ErrorLogType == ErrorLogType.Text)
                {
                    rtxtMain.Visible = true;
                    rtxtMain.Clear();
                    rtxtMain.Dock = DockStyle.Fill;

                    FileInfo fi = new FileInfo(ConfigManager.ErrorLogFileTextPath);

                    if (fi.Exists)
                    {
                        StreamReader sr = fi.OpenText();
                        rtxtMain.AppendText(sr.ReadToEnd());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
            }
        }
    }//end class
}//end namespace
