using System;

namespace ServerATM.Server
{
	public delegate void LogEventHandler(object sender,Log_EventArgs e);	

	public delegate void Message2ServerHandler(object data);
}
