using System;
using System.Data ;
using Make ;
using ATMTransaction;
using System.Timers;
using Common;

namespace Invoke
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Invoker
	{
		private Make.Make make;
        private TransationToCoreBank transaction;//added by Sajib
		bool transactionCompleted;
		bool timeOut=false;
		string response;

		public Invoker()
		{			
		}

		public void SetCommand(Make.Make make)
		{
			this.make = make;
			this.transaction = new TransationToCoreBank();
		}

		public string ExecuteCommand()
		{
            //if accurate msg then transaction is needed
			if(make.MakeRequest() && (make.p_39Response.Equals(Constants.SUCCESS_RESPONSE)|| make.p_39Response.Equals("22")) || make.p_39Response.Equals("32") || make.p_39Response.Equals("91"))				
			{			
				transactionCompleted = false;				
				Timer requestTimer = new Timer();
				 requestTimer.Elapsed += new ElapsedEventHandler(OnElapsed);
				requestTimer.Interval = 1500000;
				requestTimer.AutoReset = true;
				requestTimer.Start();			
				
				transaction.CompleteATMTransaction(make);	
				
				transactionCompleted = true;               
                requestTimer.Stop();                
                
               // transaction.CloseConnection(!timeOut); //true for Commit; false for Rollback                              
                
                UpdateHistory(make.p_3_1TransactionCode.Equals("01"));                
			}

			return response;
		}    
		
		public void OnElapsed(Object sender, ElapsedEventArgs e)
		{
			Timer reqTimer =sender as Timer;	
			reqTimer.Stop();		
			
			if(!transactionCompleted)
			{							
				timeOut=true;
				make.p_39Response="50"; //Response Code: time out				
				Console.WriteLine("ERROR: This process is delayed");                
			}
		}

		//Update response and history
		private void UpdateHistory(bool history)
		{
			make.UpdateData(make.BITMAP, make);
			response = make.MakeResponse();

			if(history==true && (make.p_39Response=="00" || make.p_39Response=="22" || make.p_39Response=="32"))
			{
				int reqType=-1;

                if (make.MTI.Equals(Constants.FINANCIAL_MTI1))
                {
                    reqType = 1;
                }
                else if (make.MTI.Equals(Constants.REVERSAL_MTI1) || make.MTI.Equals(Constants.REVERSAL_MTI2))
                {
                    reqType = 0;
                }

                try
                {
                    transaction.UpdateHistory(make, reqType);
                }
                catch (Exception ex)
                {                                        
                    new Utility().WriteErrorToLog(ex.ToString());
                }
			}			
		}
	}
}
