using System;
using ConfigureQCash ;
//using TestAccessDb;

namespace Make
{
	/// <summary>
	/// Summary description for MakeQuery.
	/// </summary>
	public class MakeQuery: Make
	{

		private CashConfig cashConfigRequest;

		private CashConfig cashConfigResponse;
		

		//TestDB testDb = new TestDB();

		//private string wholeData ;

		#region Constructor
		
		public MakeQuery(CashConfig cashConfigRequest, CashConfig cashConfigResponse, string wholeData):base(cashConfigRequest,cashConfigResponse,wholeData)
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
			PerformRequest();//(cashConfigRequest, wholeData); 

			//this portion is only for Test purpose
			//testDb.insertRequest(Testpurpose.getInsertQueryString());

			int[] additions=new int[3];
//			additions[0]=38;
//			additions[1]=39;
//			additions[2]=105; out by sajjad
			additions[0]=38; 
			additions[1]=39;
			additions[2]=54;
			ResponseBitmap(additions);
			BITMAP = responseBITMAP;//"F63866112EB0A0080000000004C000A0" ;//By SR
			return true;
		
		}

		public override string MakeResponse()
		{
			string response = PerformResponse();

			//this portion is only for Test purpose
			//testDb.insertRequest(Testpurpose.getInsertQueryString());

			return response;
		
		}

		#endregion
	}
}
