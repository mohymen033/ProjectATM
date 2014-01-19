// Fig. 22.2: Client.cs
// Set up a Client that will read information sent
// from a Server and display the information.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace ChatClient
{
   /// <summary>
   /// connects to a chat server
   /// </summary>
   public class Client : System.Windows.Forms.Form
   {
      private System.Windows.Forms.TextBox inputTextBox;
      private System.Windows.Forms.TextBox displayTextBox;

      private NetworkStream output;

      private BinaryWriter writer;	  
	  private BinaryReader reader;
	  
      private string message = "";
	  const string TESTDATA_PATH = @"E:\ISO8583\ServerATM\echo.txt" ;
      const string TESTDATA_SAVEPATH = @"E:\ISO8583\ServerATM\echosave.txt" ;

      private Thread readThread;
   	  
	  private System.Windows.Forms.Timer echoTimer = new System.Windows.Forms.Timer();

	  const string ECHODATA_PATH = @"E:\ISO8583\ServerATM\echo.txt" ;
      const string IPAddress = "192.168.1.16";//"monon";
	  private System.ComponentModel.IContainer components;

      // default constructor
      public Client()
      {
         //
         // Required for Windows Form Designer support
         //
         InitializeComponent();
		 echoTimer.Tick += new EventHandler(EchoMessageProcessor);
		 echoTimer.Interval = 5000;
		 echoTimer.Start();

		 readThread = new Thread( new ThreadStart( RunClient ) );
         readThread.Start();
      }
	   private void EchoMessageProcessor(object sender,EventArgs e)
	   {
		 Thread echoThread = new Thread(new ThreadStart( RunEcho ) );
         echoThread.Start();
	   }
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
		  this.inputTextBox = new System.Windows.Forms.TextBox();
		  this.displayTextBox = new System.Windows.Forms.TextBox();
		  this.SuspendLayout();
		  // 
		  // inputTextBox
		  // 
		  this.inputTextBox.Location = new System.Drawing.Point(8, 24);
		  this.inputTextBox.Multiline = true;
		  this.inputTextBox.Name = "inputTextBox";
		  this.inputTextBox.Size = new System.Drawing.Size(864, 72);
		  this.inputTextBox.TabIndex = 0;
		  this.inputTextBox.Text = "";
		  this.inputTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputTextBox_KeyDown);
		  // 
		  // displayTextBox
		  // 
		  this.displayTextBox.Location = new System.Drawing.Point(8, 120);
		  this.displayTextBox.Multiline = true;
		  this.displayTextBox.Name = "displayTextBox";
		  this.displayTextBox.ReadOnly = true;
		  this.displayTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		  this.displayTextBox.Size = new System.Drawing.Size(864, 304);
		  this.displayTextBox.TabIndex = 1;
		  this.displayTextBox.Text = "";
		  // 
		  // Client
		  // 
		  this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		  this.ClientSize = new System.Drawing.Size(896, 485);
		  this.Controls.Add(this.displayTextBox);
		  this.Controls.Add(this.inputTextBox);
		  this.Name = "Client";
		  this.Text = "Client";
		  this.Closing += new System.ComponentModel.CancelEventHandler(this.Client_Closing);
		  this.ResumeLayout(false);

	  }
		#endregion

      [STAThread]
      static void Main() 
      {	     
         Application.Run( new Client() );
      }

      protected void Client_Closing( 
         object sender, CancelEventArgs e )
      {         
         System.Environment.Exit( System.Environment.ExitCode );
      }
	  
      // sends text the user typed to server
      
	   protected void inputTextBox_KeyDown ( 
         object sender, KeyEventArgs e )
      {
         try
         {         
            if ( e.KeyCode == Keys.Enter )
            {
				if(inputTextBox.Text.Equals("TEST"))
				{
					char[] buffer = new char[1024];
					StreamReader testStreamReader = new StreamReader(TESTDATA_PATH);
					testStreamReader.ReadBlock(buffer,0,1024);
					writer.Write(buffer);
				}
				
               
				writer.Write(inputTextBox.Text.Trim());


               displayTextBox.Text += 
                  "\r\nCLIENT>>> " + inputTextBox.Text;
         
               inputTextBox.Clear();
            }
         }
         catch ( SocketException )
         {
            displayTextBox.Text += "\nError writing object";
         }

      } // end method inputTextBox_KeyDown

	  //echo to server to see if it is alive
	   public void RunEcho()
	   {
		TcpClient echoClient = null;
	    NetworkStream echoOutput = null;
		BinaryWriter echoWriter = null;
		BinaryReader echoReader = null;
		//for accessing echo value message and sending it to ATMSERver
		//StreamReader echoStreamReader = null ;
		   try
		   {	//1 . make connection

				echoClient = new TcpClient();
               
				echoClient.Connect(IPAddress,2020 );

			   // Step 2: get NetworkStream associated with TcpClient
			   echoOutput = echoClient.GetStream();

			   // create objects for writing and reading across stream
			   echoWriter = new BinaryWriter( echoOutput );
			   echoReader = new BinaryReader( echoOutput );

			   //echoStreamReader = new StreamReader( ECHODATA_PATH ) ;
			   //echoWriter.Write("Afrad Bhai Ki Obostha");	
			   //echoWriter.Write(echoStreamReader.ReadLine());					

			   //echoWriter.Write("A4M030000800822000000000000004000000000000000731031601003608301") ;
			   
			   displayTextBox.Text += echoReader.ReadString();			   
			   displayTextBox.Text += "\r\n" + echoReader.ReadString(); 

		   }
		   catch(Exception ex)
		   {
				Console.WriteLine(ex.Message);
		   }
		   finally{
			echoReader.Close();
			echoWriter.Close();
		    echoOutput.Close();
		    echoClient.Close();
		   }
	   }
      // connect to server and display server-generated text






      public void RunClient()
      {
         TcpClient client;

         // instantiate TcpClient for sending data to server
         try
         {

             MethodInvoker action = delegate
             { displayTextBox.Text += "Connected to server... \n"; };
             displayTextBox.BeginInvoke(action);


           // displayTextBox.Text += "Attempting connection\r\n";

            // Step 1: create TcpClient and connect to server
            client = new TcpClient();
            client.Connect( IPAddress, 2020 );

            // Step 2: get NetworkStream associated with TcpClient
            output = client.GetStream();

            // create objects for writing and reading across stream
            writer = new BinaryWriter( output );
            reader = new BinaryReader( output );

            MethodInvoker action1 = delegate
            { displayTextBox.Text += "\r\nGot I/O streams\r\n"; };
            displayTextBox.BeginInvoke(action1);

           

            inputTextBox.ReadOnly = false;
      
            // loop until server signals termination
            do
            {

               // Step 3: processing phase
               try
               {
                  // read message from server
                  message = reader.ReadString();//////////////////////

                  MethodInvoker action2 = delegate
                  { displayTextBox.Text += "\r\n" + message; };
                   
                  displayTextBox.BeginInvoke(action2);

			    

               }

                  // handle exception if error in reading server data
               catch ( Exception )
               {
                  System.Environment.Exit( 
                     System.Environment.ExitCode );
               }
            } while( message != "SERVER>>> TERMINATE" );

            MethodInvoker action3 = delegate
            { displayTextBox.Text += "\r\nClosing connection.\r\n"; };

            displayTextBox.BeginInvoke(action3);
          

            // Step 4: close connection
            writer.Close();
            reader.Close();
            output.Close();
            client.Close();
            Application.Exit();
         }

            // handle exception if error in establishing connection
         catch ( Exception error )
         {
            MessageBox.Show( error.ToString() );
         }

      }

   	private void timer1_Tick(object sender, System.EventArgs e)
	   {
	   
	   } // end method RunClient

   } // end class Client
}

/*
 **************************************************************************
 * (C) Copyright 2002 by Deitel & Associates, Inc. and Prentice Hall.     *
 * All Rights Reserved.                                                   *
 *                                                                        *
 * DISCLAIMER: The authors and publisher of this book have used their     *
 * best efforts in preparing the book. These efforts include the          *
 * development, research, and testing of the theories and programs        *
 * to determine their effectiveness. The authors and publisher make       *
 * no warranty of any kind, expressed or implied, with regard to these    *
 * programs or to the documentation contained in these books. The authors *
 * and publisher shall not be liable in any event for incidental or       *
 * consequential damages in connection with, or arising out of, the       *
 * furnishing, performance, or use of these programs.                     *
 **************************************************************************
*/
