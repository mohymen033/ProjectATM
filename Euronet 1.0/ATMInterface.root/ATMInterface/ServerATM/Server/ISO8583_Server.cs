using System;
using System.IO;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using MakeFactory;
using System.Configuration;
using Common;

namespace ServerATM.Server
{
	public class ISO8583_Server : System.ComponentModel.Component
	{		
		private System.ComponentModel.Container components = null;
		private TcpListener ISO8583_Listener  = null;		
		private Hashtable   m_SessionTable = null;
		
		public static  Factory makeFactory = new Factory() ; 
				
		private string m_IPAddress          = "ALL";  // Holds IP Address, which to listen incoming calls.
        private int    m_port               = int.MinValue;    // Holds port number, which to listen incoming calls.
		private int    m_MaxThreads         = 50;     // Holds maximum allowed Worker Threads (Users).
		private bool   m_enabled            = false;  // If true listens incoming calls.
		private bool   m_LogCmds            = true;   // If true, writes ISO8583 commands to log file.
		private int    m_SessionIdleTimeOut = 80000;  // Holds session idle timeout.
		private int    m_CommandIdleTimeOut = 60000;  // Holds command ilde timeout.		
			
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}		

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	
		#region Event declarations
		/// <summary>
		/// Occurs user session ends. This is place for clean up.
		/// </summary>
		public event EventHandler SessionEnd = null;

		/// <summary>
		/// Occurs user session resetted. Messages marked for deletion are unmarked.
		/// </summary>
		public event EventHandler SessionResetted = null;
		/// <summary>
		/// Occurs when POP3 session has finished and session log is available.
		/// </summary>
		public event LogEventHandler SessionLog = null;

		

		#endregion        				

		#region Constructors

		
		public ISO8583_Server()
		{					
			InitializeComponent();
            m_port = ConfigManager.PortNumber;
		}

		#endregion

		#region function Dispose

		/// <summary>
		/// Clean up any resources being used and STOPs ISO8583 server.
		/// </summary>
		public new void Dispose()
		{
			base.Dispose();

			Stop();				
		}

		#endregion
		
		

		/// <summary>
		/// Starts ISO8583SERVER Server.
		/// </summary>
		public void Start()
		{
			try
			{
				if(!m_enabled && !this.DesignMode)
				{
					m_SessionTable = new Hashtable();					
                    Run();
				}
			}
			catch(Exception x)
			{
                Console.WriteLine("ERROR: - Start failed - " + x.Message);
                new Utility().WriteErrorToLog(x.ToString());
			}
		}	
	
		/// <summary>
		/// Stops ISO8583 Server.
		/// </summary>
		public void Stop()
		{
			try
			{	
				if(ISO8583_Listener != null)
				{                   
					ISO8583_Listener.Stop();
				}
			}
			catch(Exception x)
			{
                Console.WriteLine("ERROR: Listener Stop failed - " + x.Message);
                new Utility().WriteErrorToLog(x.ToString());
			}	
		}
		

		/// <summary>
		/// Starts server message loop.
		/// </summary>
		private void Run()
		{		
			try
			{							
                ISO8583_Listener = m_IPAddress.ToLower().IndexOf("all") > -1 ? new TcpListener(IPAddress.Any, m_port) : new TcpListener(IPAddress.Parse(m_IPAddress), m_port);                  							                
				ISO8583_Listener.Start();                                

				while(true)
				{                 											
					Socket clientSocket = ISO8583_Listener.AcceptSocket();
					string sessionID = clientSocket.GetHashCode().ToString();					
                    //_LogWriter logWriter      = new _LogWriter(this.SessionLog);                        
                    //ISO8583_Session session      = new ISO8583_Session(clientSocket,this,sessionID,logWriter);
                    ISO8583_Session session = new ISO8583_Session(clientSocket, this, sessionID);
                    //// Add session to session list
                    //AddSession(sessionID, session, logWriter);                 
                    // Start proccessing
                    Action processing = new Action(session.StartProcessing);
                    processing.BeginInvoke(null, null);                    
                    Thread.Sleep(200);                     																								
				}
			}			        
			catch(Exception x)
			{
				Console.WriteLine("ERROR: " + x.ToString());
                new Utility().WriteErrorToLog(x.ToString());
			}
		}	

		#region Session handling stuff

		#region function AddSession

		/// <summary>
		/// Adds session.
		/// </summary>
		/// <param name="sessionID">Session ID.</param>
		/// <param name="session">Session object.</param>
		/// <param name="logWriter">Log writer.</param>
		internal void AddSession(string sessionID,ISO8583_Session session,_LogWriter logWriter)
		{
			m_SessionTable.Add(sessionID,session);

			if(m_LogCmds)
			{
				logWriter.AddEntry("//----- Sys: 'Session:'" + sessionID + " added" + DateTime.Now);				
			}
		}
		

		#endregion
		

		/// <summary>
		/// Removes session.
		/// </summary>
		/// <param name="session">Session which to remove.</param>
		/// <param name="logWriter">Log writer.</param>
		internal void RemoveSession(ISO8583_Session session,_LogWriter logWriter)
		{
			lock(m_SessionTable)
			{
				if(!m_SessionTable.Contains(session.SessionID))
				{
					Console.WriteLine( "Session '" + session.SessionID + "' doesn't exist.");
					return;
				}
				m_SessionTable.Remove(session.SessionID);
			
				// Raise session end event
				OnSessionEnd(session);
			}

			if(m_LogCmds)
			{
				logWriter.AddEntry("//----- Sys: 'Session:'" + session.SessionID + " removed" + DateTime.Now);
			}
		}
				

		/// <summary>
		/// Checks if user is logged in.
		/// </summary>
		/// <param name="userName">User name.</param>
		/// <returns></returns>
		internal bool IsUserLoggedIn(string userName)
		{			
			lock(m_SessionTable)
			{
				foreach(ISO8583_Session sess in m_SessionTable.Values)
				{
					if(sess.UserName == userName)
					{
						return true;
					}
				}
			}
			
			return false;
		}		

		#endregion

		

		/// <summary>
		/// Raises SessionEnd event.
		/// </summary>
		/// <param name="session">Session which is ended.</param>
		internal void OnSessionEnd(object session)
		{
			if(this.SessionEnd != null)
			{
				this.SessionEnd(session,new EventArgs());
			}
		}		

		/// <summary>
		/// Raises SessionResetted event.
		/// </summary>
		/// <param name="session">Session which is resetted.</param>
		internal void OnSessionResetted(object session)
		{
			if(this.SessionResetted != null)
			{
				this.SessionResetted(session,new EventArgs());
			}
		}

				
		/// <summary>
		/// Gets or sets whick IP address to listen.
		/// </summary>
		[		
		Description("IP Address to Listen ISO8583 requests"),
		DefaultValue("ALL"),
		]
		public string IpAddress 
		{
			get{ return m_IPAddress; }

			set{ m_IPAddress = value; }
		}


		/// <summary>
		/// Gets or sets which port to listen.
		/// </summary>
		[		
		Description("Port to use for ISO8583"),
		DefaultValue(6002),
		]
		public int Port 
		{
			get{ return m_port;	}

			set{ m_port = value; }
		}


		/// <summary>
		/// Gets or sets maximum session threads.
		/// </summary>
        [
        Description("Maximum Allowed threads"),
        DefaultValue(50),
        ]
        public int NumberOfThreads
        {
            get { return m_MaxThreads; }

            set { m_MaxThreads = value; }
        }		
		
		/// <summary>
		/// Gets or sets if to log commands.
		/// </summary>
        public bool LogCommands
        {
            get { return m_LogCmds; }

            set { m_LogCmds = value; }
        }

		/// <summary>
		/// Session idle timeout.
		/// </summary>
        public int SessionIdleTimeOut
        {
            get { return m_SessionIdleTimeOut; }

            set { m_SessionIdleTimeOut = value; }
        }

        public int CommandIdleTimeOut
        {
            get { return m_CommandIdleTimeOut; }

            set { m_CommandIdleTimeOut = value; }
        }
	}
}
