using System;
using ConfigureQCash ;
//using TestAccessDb;


namespace Make
{	
	public class MakeTransfer: Make
	{		
		private CashConfig cashConfigRequest;
		private CashConfig cashConfigResponse;		

		public MakeTransfer(CashConfig cashConfigRequest, CashConfig cashConfigResponse, string wholeData):base(cashConfigRequest,cashConfigResponse,wholeData)
		{
			this.cashConfigRequest = cashConfigRequest;
			this.cashConfigResponse = cashConfigResponse;
			this.wholeData = wholeData ;
        }	

		public override bool MakeRequest()
		{
			
			PerformRequest();//(cashConfigRequest, wholeData);
			int[] additions=new int[3];// For euro fund Transfer
			additions[0]=38;
			additions[1]=39;
			additions[2]=54;
			//additions[3]=103;
			
			ResponseBitmap(additions);
			BITMAP = responseBITMAP;

			return true;
		}

		public override string MakeResponse()
		{
			string response = PerformResponse();
			return response;		
		}		
	}
}
