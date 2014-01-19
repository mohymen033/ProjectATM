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
	/// Summary description for MakeEchoResponse.
	/// </summary>
	public class MakeEchoResponse:Make
	{
		private CashConfig cashConfigRequest;

		private CashConfig cashConfigResponse;

		public MakeEchoResponse()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public MakeEchoResponse(CashConfig cashConfigRequest, CashConfig cashConfigResponse, string wholeData)
			:base(cashConfigRequest,cashConfigResponse,wholeData)
	{
		//
		// TODO: Add constructor logic here
		//
		this.cashConfigRequest = cashConfigRequest;
		this.cashConfigResponse = cashConfigResponse;
		this.wholeData = wholeData ;
			
	
	}

		public override bool MakeRequest()
		{
			PerformRequest();
			int[] additions=new int[1];
			additions[0]=39;
			ResponseBitmap(additions);
			BITMAP = responseBITMAP;
			return false;

		}

        //public override void doTransactionToCoreBank(Make make)
        //{

        //}
		
		public override void UpdateData(string ReponseBitMap, Make mk)
		{
			mk.p_39Response="00001";
		}

		public override string MakeResponse()
		{
			string response = PerformResponse();
			return response ;
			//return "A4M030000810822000000200000004000000000000000073103160100360800301";

		}
		
	}
}
