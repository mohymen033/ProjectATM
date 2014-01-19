using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using System.Diagnostics;

namespace ServerATM.Server
{
    class EntryPoint
    {
        /// <summary>
        /// MAIN Entry Point
        /// </summary>
        [STAThread]
        static void Main()
        {     
            int count = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length;

            //No another application run.
            if (count == 1)
            {
                ConfigManager.InitializeConfigurationData();
                ServerAgentUI agent = new ServerAgentUI();
                Application.Run(agent);
            }
            else
            {
                MessageBox.Show("The Service application is already running.","Application running",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }
    }//end class
}//end namespace
