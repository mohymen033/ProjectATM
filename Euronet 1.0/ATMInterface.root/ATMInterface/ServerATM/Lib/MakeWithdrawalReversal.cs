using System;
using System.IO ;
using System.Text;
using System.Data ;
using ConfigureQCash;
//using TestAccessDb;
using com.mislbd.sunflower.utility.constants ;
using com.mislbd.sunflower.utility.dao;

namespace Make
{
	/// <summary>
	/// Summary description for MakeWithdrawal.
	/// </summary>
	public class MakeWithdrawalReversal:Make 
	{
		//this line is added by Asad
		
		//this two lines is modified, author 'Asad'
		private CashConfig cashConfigRequest;

		private CashConfig cashConfigResponse;
		
	
			
		#region Constructor

		public MakeWithdrawalReversal()
		{

		}

		public MakeWithdrawalReversal(CashConfig cashConfigRequest, CashConfig cashConfigResponse, string wholeData)
			:base(cashConfigRequest,cashConfigResponse,wholeData)
		{
			//
			// TODO: Add constructor logic here
			//
			this.cashConfigRequest = cashConfigRequest;
			this.cashConfigResponse = cashConfigResponse;
			this.wholeData = wholeData ;
			
	
		}
	
		#endregion

		#region Main Execution Sector

	
		
		public override bool MakeRequest()
		{
			PerformRequest();; 
			
			int[] additions=new int[1];
			//additions[0]=38;
			additions[0]=39;
			//additions[2]=54;
			
			ResponseBitmap(additions);
			BITMAP = responseBITMAP;
			
			return true;
		}

		//after updating data, response is to be made
		public override string MakeResponse()
		{
			string response = PerformResponse();
			return response ;
		}

		#endregion
	
	
	}

}
