using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using Make;
using Invoke;
using Common;


namespace ServerATM.Server
{
	/// <summary>
	/// Summary description for ISO8583_Session.
	/// </summary>
	public class ISO8583_Session
	{
		#region Region Private Member

		private Socket        m_pClientSocket = null;  // Referance to client Socket.
		private ISO8583_Server   m_pISO8583_Server = null;  // Referance to ISO8583 server.
		private string        m_SessionID = "";    // Holds session ID.
		private string        m_UserName= "";    // Holds loggedIn UserName.
		
		
		private DateTime      m_SessionStartTime = DateTime.Now;
		private _LogWriter    m_pLogWriter = null;
		private object        m_Tag= null;		

		/// <summary>
		/// Required designer variable.
		/// </summary>
		
		private System.ComponentModel.Container components = null;

		#endregion

		#region Region Constructor

        //public ISO8583_Session(Socket clientSocket,ISO8583_Server server,string sessionID,_LogWriter logWriter)
        //{
        //    m_pClientSocket    = clientSocket ;
        //    m_pISO8583_Server     = server ;
        //    m_SessionID        = sessionID ;
        //    m_pLogWriter       = logWriter ;
        //    m_SessionStartTime = DateTime.Now ;				
			
        //    InitializeComponent();
        //}

		public ISO8583_Session(Socket clientSocket,ISO8583_Server server,string sessionID)
		{
			m_pClientSocket    = clientSocket ;
			m_pISO8583_Server     = server ;
			m_SessionID        = sessionID ;			
			m_SessionStartTime = DateTime.Now ;					
			
			InitializeComponent();

		}

		#endregion
	
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

		#region function StartProcessing			

		public void StartProcessing()
		{			
			this.ProcessEuronetMessage();			
		}
		
		private void ProcessEuronetMessage()
		{			
			try
			{							
				string theRequest = "asas";		
				
				do
				{					
					try
					{
                        DateTime t = DateTime.Now;
                        //MessageBox.Show("FDF" + t);

						string totalMessgae = "";
						string fisrtPortion ="";
						string messageToMakeTxn = "";
						string finalHexstring = "";
						string binMessge = "";
                        string theResponse = "";

                        #region offline messages
                       //string CashWithdraw= "0200b2302001080081804000000017dc02004100000000100000002009021717560975609820090126104215014408000012343030303030303032303039303231373137353630393930360144054d616d756e075368616d65656d5908000012350a112332255008445663320454504254053031353532053032353633075265666174687508536861726979616c20090126045552";
                       // string CashWithdraw = "0200FABA800188E0C000000000000400000018603590002018000169011000000000010000000000010000200905111230486100000058808013295705070909090906599996003313        ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  05005019      4027133000024";
                        //string CashWithdraw = "0200FABA800188E0C00000000000040000001860359000201800016901100000000001000000000001000005111230486100000058808013295705070909090906599996003313        ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  05005019      4027133000024";
					    //string cashProbWith = "0200FABA800188E0C00000000000040000001860359000201800016901100000000005000000000005000006041119486100000064150111194206040909090906599996004064        ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  05005019       003112002324";
						//string cashWith = "0200FABA800188E0C00000000000040000001860359000201800016901100000000005000000000005000006031719306100000063879717192506030909090906599996004046        ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  05005019       003112002324";
						//string CashWithreversalMessage  = "0421FABA04C12AE0C00000000040040000001860359000201800016901100000000001000000000001000005121829516100000060203013295705070909       065999963313        ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  05005002006020300507132957000005999960000000000019         3112002324";
						//string firstCash ="0200FABA800188E0C00000000000040000001860359000201800016931200000000005000000000005000005131717516100000060631517174905130909090906599996003414        ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  05005019         3102000064";
						
                        //string BalInq = "0200E23A800188E08000000000000400000018603590002018000169312000051417362960723815362705140909090906599996003432        ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  05019      4027133000024";
                        //string fundTransfer = "0421FABA04C12AE0C00000000040060000001862797400129990010040102000000001000000000001000010211551036100000009534515424510210909       06599996007452        91ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  050050020009534510211542450000059999600000000000190000004027134000015190000004027133000024";
						//string ft_tr = "0200FABA800188E0C00000000000060000001860359000201800016940102000000000500000000000500005201134006100000062939911335605200909090906599996003505        ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  05005019       00311200232419       003102000064";						
                        //string statement_Mini = "0200E23A800188E08000000000000400010018603590002018000169901000052012124862948312124505200909090906599996003513        ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  05019      402713300002400800100207";
						//string reversal =	"0420FABA04812EE0C00000000040040000001860359000201800016901100000000005000000000005000006041213346100000064175712125506040909     06599996004066        35178322ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  05005002006417570604121255000005999960000000000019       003112002324";
						//string partialReversal = "0421FABA04812EE0C00000000042040000001860359000201800016901100000000030000000000030000006041852456100000064297818474206040909     06599996004097        35180732ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  050050020064297806041847420000059999600000000000000000200000000000000000C00000000C0000000019       003112002324";
						 //string reversalTimeOut = "0420FABA04C12AE0C00000000040040000001860359000201800016901100000000005000000000005000006041950366100000064326819495906040909       06599996004101        91ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  05005002006432680604194959000005999960000000000019       003112002324";
						//string reversalDouble = "0421FABA04812EE0C00000000040040000001860359000201800016901100000000005000000000005000006051221556100000064665012200006050909     06599996004124        35184222ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  05005002006466500605122000000005999960000000000019       003112002324";
						//string problemReversal ="0421FABA04C12EE0C00000000040060000001860359000201800016940102000000007500000000007500006051933506100000064800118213106050909       06599996004161        35188291ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  05005002006480010605182131000005999960000000000019       00311200232419       003102000064";
						//string probreversal ="0420FABA04C12EE0C00000000040060000001860359000201800016940102000000007500000000007500006051927366100000064800118213106050909       06599996004161        35188291ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  05005002006480010605182131000005999960000000000019       00311200232419       003102000064";
                      //  theResponse = this.ParseData(CashWithdraw);

                        //string str = "0200FABA800188E0C00000000000060000001862797400129990010040102000000050000000000050000010231244196100000010118612441210230909090906599996007569        ABB0002 599996         DHAKA  BANGALDESH      DHAKA        BD  050050190000004027134000015190000004027133000024";

                        //string data = CashWithdraw;                      

                        //str = this.ParseData(str);
                        //return;
                        #endregion                        

                        //string test1 = "dghfhgsdhgfs232323";
                       // Byte[] testbyte = Encoding.ASCII.GetBytes(test1);
                       // string test = Encoding.ASCII.GetString(testbyte);
                       // MessageBox.Show("this is received data :" + test);
                       
                        NetworkStream socketStream = new NetworkStream(m_pClientSocket);
                        //string test2 = Encoding.ASCII.GetString(m_pClientSocket.Send();
                        int tr = m_pClientSocket.Available;
                       
                        if (tr.ToString() != "0")//data found
                        {
                            Console.WriteLine("MESSAGE: " + tr.ToString() + " received");
                        }
                        do 
                        {
                            string readline1 = Core.ReadLine(m_pClientSocket);
                        
                        }

                        while(tr.ToString() != "0");
                        MessageBox.Show("dfdf");
                        byte[]  socB = new byte[tr];

                        m_pClientSocket.Receive(socB);
                        string test = Encoding.ASCII.GetString(socB,4,298);

                        theResponse = this.ParseData(test);
                        Byte[] testbyte = Encoding.ASCII.GetBytes(theResponse);
                        tr = testbyte.Length;
                        //byte[] resultByte = this.GenerateResponse(theResponse);
                        //m_pClientSocket.Send(resultByte);
                       // Console.WriteLine("RESPONSE: result message sent-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
					
                       
                        int toCount = 0;
                        UInt16 intuns;
                        int yr;
                        long lo;
                        string currentString = string.Empty;
                        string finalmessageTomakeTxn = string.Empty;

                        if (tr > 0)
                        {
                            for (int i = 0; i < tr; i++)
                            {
                                intuns = (UInt16)socB[i];
                                yr = (int)socB[i];
                                lo = (long)socB[i];

                                // handle the fisrt posrtion of the transaction
                                if (i > 1 && i < 6)
                                 
                                {
                                    //int fisrtStr = intuns-48;
                                    int fisrtStr = intuns - 240;
                                    fisrtPortion += fisrtStr;
                                }

                                if (i == 6)
                                {
                                    toCount = 22;
                                    string firstByteOfPrimBitMap = socB[i].ToString("X");
                                    string binaryBitmap = this.MakeBitMapToBinary(firstByteOfPrimBitMap);
                                    string primaryFirstBit = binaryBitmap.Substring(0, 1);                                   				
                                }

                                // convert to hex
                                if (i > 5 && i < toCount)
                                {
                                    string hexString = intuns.ToString("X");
                                    if (hexString.Length > 1)
                                    {
                                        finalHexstring += hexString;
                                    }
                                    else 
                                    { 
                                        finalHexstring += "0" + hexString; 
                                    }
                                }

                                //get  the binary string from hex
                                // handle the bitmap portion of the transciton message
                                //follofwing is for last portion of the txn message
                                if (i > 21)
                                {
                                    //currentString = intuns - 240;
                                    currentString = getStringValFromEBCDICVal(socB[i]);
                                    messageToMakeTxn += currentString;
                                }

                                totalMessgae += " " + intuns.ToString();
                            }                            

                            binMessge = this.MakeBitMapToBinary(finalHexstring);
                           
                            Console.WriteLine("REQUEST: BITMAP - " + fisrtPortion + " is " + binMessge);

                            finalmessageTomakeTxn = fisrtPortion + finalHexstring + messageToMakeTxn;

                            Console.WriteLine("REQUEST: DATA - " + finalmessageTomakeTxn);                            
						}
					
						//string theResponse="";

						if(!finalmessageTomakeTxn.Equals(""))
						{
                            if (messageToMakeTxn != null && !fisrtPortion.Equals(Constants.NETWORK_MESSAGE))
                            {
                                theResponse = this.ParseData(finalmessageTomakeTxn);
                            }
						}

                        if (fisrtPortion.Equals(Constants.NETWORK_MESSAGE))
						{						
							byte[] finalBytetoSend = getEchoResponseMessage(socB,fisrtPortion,finalHexstring,messageToMakeTxn);
							m_pClientSocket.Send(finalBytetoSend);
							Console.WriteLine("RESPONSE: Network message 810 sent");
						}
                        else if (fisrtPortion.Equals(Constants.FINANCIAL_MTI1))
						{							
							byte[] resultByte = this.GenerateResponse(theResponse);							
							m_pClientSocket.Send(resultByte);
                            Console.WriteLine("RESPONSE: result message sent-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
						}
                        else if (fisrtPortion.Equals(Constants.REVERSAL_MTI1) || fisrtPortion.Equals(Constants.REVERSAL_MTI2))//response for withdrawl message
						{																			
							byte[] resultByte = this.GenerateResponse(theResponse);							
							m_pClientSocket.Send(resultByte);
                            Console.WriteLine("RESPONSE: Reversal result message sent...................................................................................................................................................................................");				                
						}                         
					}			
					catch(Exception x)
					{
                        //added by monon for only test we can send data to eldorado need to change 
                        Byte[] byte_data = new byte[3000];
                        int size = m_pClientSocket.Send(byte_data, byte_data.Length, 0);
                        new Utility().WriteErrorToLog(x.ToString());					
                        Console.WriteLine("ERROR:  " + x.ToString());
					}					
				} while ( theRequest != "CLIENT>>>TERMINATE"  && m_pClientSocket.Connected );
			}			
			catch(Exception x)
			{
                new Utility().WriteErrorToLog(x.ToString());	
                Console.WriteLine("ERROR:  " + x.ToString());		
			}
			finally
			{
				m_pISO8583_Server.RemoveSession(this,m_pLogWriter);

				if(m_pClientSocket.Connected)
				{
					m_pClientSocket.Close();
				}

                //// Write logs to log file, if needed
                //if (m_pISO8583_Server.LogCommands)
                //{
                //    m_pLogWriter.Flush();
                //}
			}
		}

		private byte[] GenerateResponse(string responseMessage)
		{
			// get MTI string 
			byte[] mtiByte = this.getBytesFromString(responseMessage.Substring(0,4));// added by monon need to change 
			//get decimal value of hex string 			
			byte[]hexInBytes = this.getDecimalFromHexString(responseMessage.Substring(4,32)); //get data in bytes
			byte[]dataBytes = this.getBytesFromString(responseMessage.Substring(36));
			byte[]resultByte = new byte[hexInBytes.Length + 6+ dataBytes.Length];
			//resultByte[0]= (byte)1;
			resultByte[1]= 0;
			resultByte[2]= mtiByte[0];
			resultByte[3]= mtiByte[1];
			resultByte[4]= mtiByte[2];
			resultByte[5]= mtiByte[3];
			//Creating the final Bytes
			int finalArrayLength = 5;
			int resultbytesIndex = 6;
			int hexIndex = 0;
			int dataindex = 0;

			for(int index = 6;index <= hexInBytes.Length+5; index++)
			{
				resultByte[index]= hexInBytes[hexIndex];
				resultbytesIndex++;
				finalArrayLength++;
				hexIndex++;
			}
			for(int index = 1;index <= dataBytes.Length; index++)
			{
				resultByte[resultbytesIndex]= dataBytes[index-1];
				resultbytesIndex++;
				finalArrayLength++;
				dataindex++;
			}
			char firstByte; 
			int dtlLenght ;

			if(finalArrayLength >255)
			{
				decimal divisor =0;
				divisor = finalArrayLength/256;
				string fisrtStr =divisor.ToString().Substring(0,1);
				divisor = Convert.ToDecimal(fisrtStr);
				firstByte = (char)divisor;
				dtlLenght = (finalArrayLength-1)%256;
			}
			else
			{
				firstByte = (char)0;
				dtlLenght = finalArrayLength-1;
			}

			char chrLenght = (char)dtlLenght;
			//resultByte[1]= (byte)(finalArrayLength-1);			
			//Encoding  ascii = Encoding.ASCII;
			//char[] asciiChars = new char[ascii.GetCharCount(resultByte, 0, 2)];
			//ascii.GetChars(resultByte, 0, 2, asciiChars,0);
			resultByte[0]=(byte)firstByte;
			resultByte[1]=(byte)chrLenght;			
			//Console.WriteLine("RESPONSE: Final Message sent to the port is "+resultByte[0]+resultByte[1]+responseMessage);
			return resultByte;
		}

		private string getStringValFromEBCDICVal(byte streamText)
		{
			System.Text.Encoding encoding37 = Encoding.GetEncoding(37);
			System.Text.Decoder  decod = encoding37.GetDecoder();
			byte[] streamBytes = new byte[1];
			streamBytes[0] = streamText;
			char[] testChsr = new char[500];
			decod.GetChars(streamBytes, 0, 1, testChsr,	0);
			string teststring = testChsr[0].ToString();
			return teststring;

		}

		private byte[] getDecimalFromHexString(string hexText)
		{
			byte[] decimalBytes = new byte[16];
			int bytesIndex = 0;
			for (int i = 0; i < hexText.Length; i++)
			{
				string hexVal = hexText.Substring(i, 2);
				int decAgain;
				decAgain = int.Parse(hexVal, System.Globalization.NumberStyles.HexNumber);
				i++;
				decimalBytes[bytesIndex] = (byte)decAgain;
				bytesIndex++;
			}
			return decimalBytes;

		}

		private byte[] getBytesFromString(string streamText)
		{
			System.Text.Encoding encoding37 = Encoding.GetEncoding(37);
			byte[] streamBytes = encoding37.GetBytes(streamText);
			return streamBytes;
		}


		private byte[] getEchoResponseMessage(byte[] streamByte,string firstPortion,string bitmapinHex,string lastPortion)
		{
			
			DateTime dt = DateTime.Now;
			byte[] messageByte = new byte[43]; 
			streamByte.CopyTo(messageByte,0);
			
			System.Text.ASCIIEncoding encoding =new System.Text.ASCIIEncoding();
			//string chsrt = encoding.GetBytes("41");
			char chr = (char)41;
					
			messageByte[1]= (byte)chr;
			messageByte[4]= (byte) 241; //
			messageByte[10]= 2; //
			string currMOnth = dt.Month.ToString();
			string currDate = dt.Day.ToString();
			string currhour = dt.Hour.ToString();
			string currMinute = dt.Minute.ToString();
			if(currMinute.Length == 1)
			{
				currMinute = currMinute.PadLeft(2,'0');
			}
			if(currhour.Length == 1)
			{
				currhour = currhour.PadLeft(2,'0');
			}
			string currSecond = dt.Second.ToString();
			
			int minfirst = Convert.ToInt32(currMinute.Substring(0,1));
			
			minfirst = 240+ minfirst;
			

			int minSecond = Convert.ToInt32(currMinute.Substring(1,1));
			minSecond = 240+ minSecond;

			int secondfirst = Convert.ToInt32(currSecond.Substring(0,1));
			secondfirst = 240+ secondfirst;
			int secondSecond = Convert.ToInt32(currSecond.Substring(1,1));
			secondSecond= 240+ secondSecond;

			//messageByte[22]= (byte)240; //m
			//messageByte[23]= (byte) Convert.ToDecimal(240+currMOnth) ; //m
//			messageByte[24]= (byte)Convert.ToDecimal(240+currMOnth) ; //d
//			messageByte[25]= (byte)240; //d
//			messageByte[26]= (byte)240;//h
//			messageByte[27]= (byte)245; //h
			//messageByte[28]= (byte)minfirst; //min
			//messageByte[29]= (byte)minSecond;//min
			//messageByte[30]= (byte)secondfirst;//ss
			//messageByte[31]= (byte)secondSecond; // ss
//			messageByte[32]= (byte)240; //
//			messageByte[33]= (byte)240; //
//			messageByte[34]= (byte)240; //
//			messageByte[35]= (byte)240; //
//			messageByte[36]= (byte)245; //
//			messageByte[37]= (byte)245; //
			messageByte[39]= (byte)240; //
			messageByte[40]= (byte)240; //
			messageByte[41]= (byte)240; //
			messageByte[42]= (byte)241; //


			//messageByte[70]= (byte)240;

			//this.getStringFromBtyeBuffer(messageByte);
			return messageByte;

		}

		private string getStringFromEBCDICBtyeBuffer(byte[] streamBuffer) 
		{
			//System.Text.ASCIIEncoding encoding =new System.Text.ASCIIEncoding();
			System.Text.Encoding encoding37 = Encoding.GetEncoding(37);
			// Read string from binary file with UTF8 encoding
			string stringMessage = encoding37.GetString(streamBuffer);
			//Console.WriteLine("Data Sent to the port is "+stringMessage);
			return stringMessage;
		}

		private string getStringFromBtyeBuffer(byte[] streamBuffer) 
		{
			System.Text.ASCIIEncoding encoding =new System.Text.ASCIIEncoding();
			// Read string from binary file with UTF8 encoding
			string stringMessage = encoding.GetString(streamBuffer);
			//Console.WriteLine("Data Sent to the port is "+stringMessage);
			return stringMessage;
		}
//		private byte[] getBytesFromString(string streamText) 
//		{
//			System.Text.ASCIIEncoding encoding =new System.Text.ASCIIEncoding();
//			// Read string from binary file with UTF8 encoding
//			byte[] streamBytes = encoding.GetBytes(streamText);
//			return streamBytes;
//		}
		
		private string ConvertStringToEbcDic(string messge)
		{  
			return messge;
		}

		private string[] hexToBinary = {"0000","0001","0010","0011",
										   "0100","0101","0110","0111",
										   "1000","1001","1010","1011",
										   "1100","1101","1110","1111"
									   };

		protected static string DUMMY = "0000000000000000" ;

		public string MakeBitMapToBinary(string hexBitmap)
		{					
			string binaryBitmap = "" ;
			
			for( int i = 0 ; i< hexBitmap.Length ; i++)
			{
		
				int hex = Convert.ToInt16(hexBitmap.ToUpper()[i]);
				if(hex > 64)
					binaryBitmap += hexToBinary[hex -55];
				else binaryBitmap += hexToBinary[hex - 48 ];
				
			}
			
			return binaryBitmap;
		
		}

				#endregion
		
		private string ParseData(string wholeData)
		{						
            Make.Make make = ISO8583_Server.makeFactory.GetMakeInstance(wholeData);
			
			Invoker invoke = new Invoker();

			invoke.SetCommand( make );

			string response = invoke.ExecuteCommand();

			return response ;
		}
				
		#region function SendData (3)
			
		private void SendData(string data)
		{
			Byte[] byte_data = System.Text.Encoding.ASCII.GetBytes(data.ToCharArray());
			int size = m_pClientSocket.Send(byte_data,byte_data.Length,0);

			if(m_pISO8583_Server.LogCommands)
			{
				string reply = System.Text.Encoding.ASCII.GetString(byte_data);
				reply = reply.Replace("\r\n","<CRLF>");
				//m_pLogWriter.AddEntry(reply,this.SessionID,this.IpStr,"S");
			}
		}

		private void SendData(byte[] data)
		{
			using(MemoryStream strm = new MemoryStream(data))
			{
				SendData(strm);
			}
		}

		private void SendData(MemoryStream strm)
		{
			//---- split message to blocks -------------------------------//
			long totalSent = 0;
			while(strm.Position < strm.Length)
			{
				int blockSize = 4024;
				byte[] dataBuf = new byte[blockSize];
				int nCount = strm.Read(dataBuf,0,blockSize);
				int countSended = m_pClientSocket.Send(dataBuf,nCount,SocketFlags.None);

				totalSent += countSended;

				if(countSended != nCount)
				{
					strm.Position = totalSent;
				}
			}
            ////-------------------------------------------------------------//

            //if(m_pISO8583_Server.LogCommands)
            //{
            //    m_pLogWriter.AddEntry("big binary " + strm.Length.ToString() + " bytes",this.SessionID,this.IpStr,"S");
            //}
		}

		#endregion				

		/// <summary>
		/// Reads line from socket.
		/// </summary>
		/// <returns></returns>
		private string ReadLine()
		{
			string line = Core.ReadLine(m_pClientSocket,500,m_pISO8583_Server.CommandIdleTimeOut);
				
            //if(m_pISO8583_Server.LogCommands)
            //{
            //    m_pLogWriter.AddEntry(line + "<CRLF>",this.SessionID,this.IpStr,"C");
            //}

			return line;
		}		

		#region Region Properties Implementation

		/// <summary>
		/// Gets session ID.
		/// </summary>
		public string SessionID
		{
			get{ return m_SessionID; }
		}

		/// <summary>
		/// Gets session start time.
		/// </summary>
		public DateTime SessionStartTime
		{
			get{ return m_SessionStartTime; }
		}

		/// <summary>
		/// Gets loggded in user name (session owner).
		/// </summary>
		public string UserName
		{
			get{ return m_UserName; }
		}

		/// <summary>
		/// Gets EndPoint which accepted conection.
		/// </summary>
		public EndPoint LocalEndPoint
		{
			get{ return m_pClientSocket.LocalEndPoint; }
		}

		/// <summary>
		/// Gets connected Host(client) EndPoint.
		/// </summary>
		public EndPoint RemoteEndPoint
		{
			get{ return m_pClientSocket.RemoteEndPoint; }
		}
		
		/// <summary>
		/// Gets or sets custom user data.
		/// </summary>
		public object Tag
		{
			get{ return m_Tag; }

			set{ m_Tag = value; }
		}


		/// <summary>
		/// Gets connected Host(client) Ip address.
		/// </summary>
		internal string IpStr
		{
			get{ return Core.ParseIP_from_EndPoint(this.RemoteEndPoint.ToString()); }
		}

		#endregion

		#region Region UTILITY nothing implemented in this region
		/// <summary>
		/// THE MESSAGE RECEIVED FROM CLIENT WILL BE PARSED ACCORDING TO THE 
		/// CONFIGURATION XML FILE GIVEN 
		/// </summary>
		#endregion

	}
}
