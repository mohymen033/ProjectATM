using System;
using System.Data;
using System.Data.OracleClient;
using com.mislbd.sunflower.utility.dao;
using Make ;
using Common;

namespace ATMTransaction
{
	/// <summary>
	/// Summary description for Txn.
	/// </summary>
	public class Txn
	{
		#region Region Elements(Fields) of doing TXN

		IDbTransaction tran=null;
		IDbConnection con1=null;
		string cbsConString= "Data Source = xe; User id = ababil; Password =a";
		string intConString= "Data Source = xe; User id = atmDB; Password =atmdb";
		string activeConString;
		bool cbsConnected;
		int atmTrId;
		int offlineException;
		//long globalTxnNo = 99910123;
		int acquirer;
		string CW_ProcessCode,BQ_ProcessCode,MS_ProcessCode,FT_ProcessCode;
		#endregion

		public Txn()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private long get_globalTxnNo(IDbTransaction tran )
		{
			string strSql ="select get_globaltxnno from dual";
			OracleConnection con = (OracleConnection)tran.Connection;
			OracleTransaction oraTxn = (OracleTransaction)tran;
			OracleCommand oCmd = new OracleCommand("",con,oraTxn);
			oCmd.CommandType = CommandType.Text;
			oCmd.CommandText = strSql;
				
			//reading data...
			OracleDataReader aReader = oCmd.ExecuteReader();	
			aReader.Read();
			if(aReader.HasRows)
			{
				long globalTxnNo= long.Parse(aReader[0].ToString());
				return globalTxnNo;

			}
			else 
			{
				return 888888;
			}


		}

		public void DoATMTarnsaction(Make.Make make)
		{
			//IDbTransaction tran = null ;	//By Sajib
			offlineException=0;			
			string query="SELECT * FROM ATM_SERVER_STATUS WHERE ATMSERVERNAME='CBS' AND ATMSERVERSTATUS=1";
			Console.WriteLine("Query for ATM"+ query ); 
			string conString =  intConString;
			try
			{
				if(checkRecord(conString,query))
				{					
					Console.WriteLine("CBS ONLINE");	
					activeConString=cbsConString;
					cbsConnected=true;
				}
				else 
				{
					Console.WriteLine("CBS OFFLINE,TXN WILL BE IN ATM DB");	
					activeConString=intConString;
					cbsConnected=false;					
				}
			}
			catch(Exception ex)
			{
				make.p_39Response="31";
				Console.WriteLine("Database Connection Error:"+ex.Message);
				return;
			}
			/**************************Now ready for TXN************************/
			acquirer=make.acquirer_ID;
			if(acquirer==1)
			{
				CW_ProcessCode="01";
				BQ_ProcessCode="31";
				MS_ProcessCode="90";
				FT_ProcessCode ="40";
			}
			else 
			{
				CW_ProcessCode="010";
				BQ_ProcessCode="030";
				MS_ProcessCode="071";
			}
			try
			{
				//IDbConnection con = null;
				OracleConnection con = null;
				//IDbConnection con1 = null;				
				conString = "";

				conString = activeConString;
				con = new OracleConnection(conString);		
				con1 = (IDbConnection)con;
				
				//con = new IDbConnection(); 
				//con = ConnectionPool.getInstance(conString).getConnection();
				con1.Open();
				tran = con1.BeginTransaction();

				//DepositTransaction depositTran = new DepositTransaction();

				//globalTxnNo = Sequencer.getSequencer().getGlobalTxnNo(dbtr);
				
				
				string txnDefinitionCode ="";
				int reqType=-1;                          ////For request type(Debit/Reversal)
				//added by Sajib
				//Credit(1)/Debit(2)?
				if(make.MTI.Substring(1,1).Equals("2"))
				{
					txnDefinitionCode +="2";
					if(make.p_3_1TransactionCode.Equals(CW_ProcessCode))
					{
						if(checkHistory(make,1)==false)
							reqType=1;
						else make.p_39Response="38";      ///Duplicate request
					}
				}
				else if(make.MTI.Substring(1,1).Equals("4"))
				{
					txnDefinitionCode +="1";
					if(make.p_3_1TransactionCode.Equals(CW_ProcessCode))
					{
						if(checkHistory(make,0)==true)
							reqType=0;			
						else make.p_39Response="40";      ///No original request found
					}
				}
				//Modified By Sajib
				//Balance field preparation
				
				//string pan=make.p_2PrimaryAccountNumber;// changes by sajjad99
				string primaryAcc= make.s_102AccountIdentificationI.Trim();
				string pan = primaryAcc.PadLeft(12,'0'); 
				if(make.p_3_1TransactionCode.Equals(CW_ProcessCode))
				{
					txnDefinitionCode += "01";
				
					
					//Single part
					string initiatorModule = "DEPOSIT";
					string instruments = "V-";
					string narration = "ATM_Narration";

					//DateTime insDate = System.DateTime.Today ;
					DateTime insDate = DateTime.ParseExact( Utility.CurrentYear + make.p_7TransmissionDateTime.Substring(0,4), "yyyyMMdd", null); 
				
					//doTransaction(IDbTransaction dbtr,int txnDefId,string accountCode,string contarAccCode,string instrumentNo,DateTime insTrIssueDate,
					//string txnNarration,decimal txnAmnt,string initiatorModule,long globalTxnNo)
			
					if(reqType!=-1) 
					{              //Valid Transaction

						
						decimal txnAmount = Convert.ToDecimal(make.p_4AmountTransaction.Substring(0,make.p_4AmountTransaction.Length -2));
						// check the partial adjustment if it is really a partial txn
						// during partial txn the the DE-95 is available..
						if(null != make.s_95_1ActualTxnAmount)
						{
							decimal settlementAmount = Convert.ToDecimal(make.s_95_1ActualTxnAmount.Substring(0,make.s_95_1ActualTxnAmount.Length -2));
							txnAmount = txnAmount - settlementAmount;
							
						}
						make.p_38AuthorizationIdentificationResponse = this.get_globalTxnNo(tran).ToString();
						doDepositTransaction(tran,"False", txnDefinitionCode,pan
							,instruments,insDate,narration,txnAmount,Convert.ToInt64(make.p_38AuthorizationIdentificationResponse),initiatorModule
							,"1",3,3); 

						//tran.Commit();
						//tran = con1.BeginTransaction();
						
						//make.p_39Response = "00001";

						if(offlineException!=1)
							//make.s_105BalanceData=BalanceString(tran,pan); by sajjad
							make.p_54AmountAdjustment =BalanceString(tran,pan);
						if(offlineException==1)
							make.p_39Response="12";        //Invalid Account
						else if(offlineException==2)
							make.p_39Response="16";        //Balance limit Exceeded
						//modified by Sajib
					
						//updateHistory(make,reqType);
					
						if(txnDefinitionCode.StartsWith("1"))
							Console.Write("Reversal Complete.Credited to ");
						else if(txnDefinitionCode.StartsWith("2"))
							Console.Write("Debited from ");
						Console.WriteLine("A/C No. "+make.s_102AccountIdentificationI);
					}
					else
					{
						make.p_54AmountAdjustment =BalanceString(tran,pan);

					}
					//tran.Commit();
					//con.Close();
				}
				else if(make.p_3_1TransactionCode.Equals("020"))
				{
					txnDefinitionCode = "101";
					//
					//To Do:
					//
					//make.s_105BalanceData+="0000000060000000000060001";
				}
					// following else code block is for Fund Transfer
				else if(make.p_3_1TransactionCode.Equals(FT_ProcessCode))
				{
					
					//Single part
					string initiatorModule = "DEPOSIT";
					string instruments = "V-";
					string narration = "ATM_Narration";
					txnDefinitionCode = "201";
					decimal txnAmount = Convert.ToDecimal(make.p_4AmountTransaction.Substring(0,make.p_4AmountTransaction.Length -2));
					DateTime insDate = DateTime.ParseExact(Utility.CurrentYear + make.p_7TransmissionDateTime.Substring(0,4), "yyyyMMdd", null); 
					string processionCode = make.p_3ProcessingCode;
					string strProcCode = processionCode.Substring(2,4);
					
					bool is_valid_txn = true;
					//check the account type from
					string txnCodeFrom = make.s_102AccountIdentificationI.Trim().Substring(3,3);
					string txnCodeTo = make.s_103AccountIdentificationII.Trim().Substring(3,3);
					string txnCodeEuronet="";
					if(txnCodeFrom.Equals("102"))
					{
						txnCodeEuronet = "20";
					}
					else
					{
						txnCodeEuronet = "10";
					}
					if(txnCodeTo.Equals("102"))
					{
						txnCodeEuronet += "20";
					}
					else
					{
						txnCodeEuronet += "10";
					}
					// check the account type to
					if(!txnCodeEuronet.Equals(strProcCode))
					{
						is_valid_txn  = false;
						
						//offlineException = 1;
					}
					//check the account duplicacy
					if(pan.Equals(make.s_103AccountIdentificationII))
					{
						is_valid_txn = false;
						
					}

					string  creditPan= "";
					creditPan = make.s_103AccountIdentificationII.Trim();
					if(is_valid_txn)
					{
						make.p_38AuthorizationIdentificationResponse = this.get_globalTxnNo(tran).ToString();
						// first do the dr transction 
						doDepositTransaction(tran,"False", txnDefinitionCode,pan
							,instruments,insDate,narration,txnAmount,Convert.ToInt64(make.p_38AuthorizationIdentificationResponse),initiatorModule
							,"1",3,3); 
					
						make.p_54AmountAdjustment =BalanceString(tran,pan);
					
						txnDefinitionCode = "101";
						//
						//To Do:Create a method for Fund Transfer
						//
						creditPan = make.s_103AccountIdentificationII.Trim();
						creditPan.PadLeft(12,'0');
						
					
						doDepositTransaction(tran,"False", txnDefinitionCode,creditPan
							,instruments,insDate,narration,txnAmount,Convert.ToInt64(make.p_38AuthorizationIdentificationResponse),initiatorModule
							,"1",3,3); 

						//tran.Commit();
						//tran = con1.BeginTransaction();
						
					}
					else
					{
						make.p_39Response="52";        //Invalid Account
						make.p_54AmountAdjustment  = BalanceString(tran,pan);
						make.p_38AuthorizationIdentificationResponse = this.get_globalTxnNo(tran).ToString();;
					}
					//make.p_39Response = "00001";
					string last54AccInfor ="";
					string final54AccInfo = "";
					if(offlineException!=1)
						//make.s_105BalanceData=BalanceString(tran,pan); by sajjad
						last54AccInfor = make.p_54AmountAdjustment;
					final54AccInfo =  BalanceString(tran,creditPan);
					make.p_54AmountAdjustment = last54AccInfor+final54AccInfo;
					if(offlineException==1)
						make.p_39Response="52";        //Invalid Account
					else if(offlineException==2)
						make.p_39Response="16";        //Balance limit Exceeded
					//modified by Sajib
					
					//updateHistory(make,reqType);
					
					if(txnDefinitionCode.StartsWith("1"))
						Console.Write("Reversal Complete.Credited to ");
					else if(txnDefinitionCode.StartsWith("2"))
						Console.Write("Debited from ");
					Console.WriteLine("A/C No. "+make.s_102AccountIdentificationI);

					//make.p_38AuthorizationIdentificationResponse = "383838";
					//make.p_39Response = "00001";	
					//make.s_105BalanceData=BalanceString(tran,pan);// by sajjad
					//make.p_54AmountAdjustment = BalanceString(tran,pan);
					Console.WriteLine("Balance Query Complete for A/C No. "					
						+make.s_102AccountIdentificationI);
				}
				else if(make.p_3_1TransactionCode.Equals(BQ_ProcessCode))
				{
					txnDefinitionCode = "301";
					//
					//To Do:Create a method for Balance  Inquiry
					//

					make.p_38AuthorizationIdentificationResponse = get_globalTxnNo(tran).ToString();
					//make.p_39Response = "00001";	
					//make.s_105BalanceData=BalanceString(tran,pan);// by sajjad
					make.p_54AmountAdjustment = BalanceString(tran,pan);
					Console.WriteLine("Balance Query Complete for A/C No. "					
						+make.s_102AccountIdentificationI);
				}
				else if(make.p_3_1TransactionCode.Equals(MS_ProcessCode))
				{
					txnDefinitionCode = "701";
					//
					//To Do:Create a method for Mini Statement Inquiry
					//

					make.p_38AuthorizationIdentificationResponse = get_globalTxnNo(tran).ToString();
					make.p_39Response = "00";
					make.p_54AmountAdjustment = BalanceString(tran,pan);
					//make.s_105BalanceData=BalanceString(tran,pan);
					//make.s_105BalanceData+="0000000060000000000060001";
					//make.s_120ExtendedTransactionData = "";
					int totalTxn = 0;
					string statement=doMiniStatementQuery(tran,pan,out totalTxn);// data fromstatement
					string totalData="";
					if (totalTxn < 10)
					{
						totalData ="0"+totalTxn;
						
					}
					else{totalData = totalTxn.ToString();}
					string curr120Data = make.s_120ExtendedTransactionData;
					string currBalance = getCurrentBalance(tran,pan).ToString().PadLeft(12,'0');
					string balSign = "+";
					string final120Data = curr120Data+"005002"+totalData+"006"+statement.Length+statement+"007013"+currBalance+balSign+"008013"+currBalance+balSign+"030003050";
					make.s_120ExtendedTransactionData = final120Data;
					//make.s_114Statement = padding(statement.Length.ToString(),3-statement.Length)+statement;
					Console.WriteLine("Mini Statement Query Complete for A/C No. "					
						+make.p_2PrimaryAccountNumber);
				}
					
			}

			catch(Exception ex)
			{
                tran.Rollback();
				make.p_39Response="31";
				Console.WriteLine("Database Connection Error:"+ex.Message);
				tran.Rollback();
			}
			
		}
		
		public bool closeConnection(bool success)
		{
			try
			{
				if(success==true)
					tran.Commit();
				else 
					tran.Rollback();
				con1.Close();
				return true;
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}
		}


		public void doDepositTransaction(IDbTransaction dbtr, string isContra,string trDefCode,string accCode,string instrumentNo,DateTime insIssueDate,
			string txnNarration,decimal txnAmount,long globalTxnno,string inititaModule,string strBatchNo,int initiatorBranch ,int ownerBranch)
			
		{
			try
			{
				OracleConnection con = (OracleConnection)dbtr.Connection;
				OracleTransaction oraTxn = (OracleTransaction)dbtr;
				if(trDefCode.Equals("101") || trDefCode.Equals("201"))
				{
					if(cbsConnected==true)
					{
						OracleCommand oCmd = new OracleCommand("Public_Transaction.ENTRY_COMMON_Transaction",con,oraTxn);
						oCmd.CommandType = CommandType.StoredProcedure;
				
						//By Sajib

						OracleParameter parEntryMode = oCmd.Parameters.Add("PENTRYMODE", OracleType.Number);
						parEntryMode.Value = 1;
						parEntryMode.Direction = ParameterDirection.Input;
				
						OracleParameter parTracerNo = oCmd.Parameters.Add("PTRACERNO", OracleType.VarChar, 10);
						parTracerNo.Value = OracleString.Null;
						parTracerNo.Direction = ParameterDirection.InputOutput;
					
						OracleParameter parOwnerBranchid= oCmd.Parameters.Add("POWNERBRID", OracleType.Number);
						parOwnerBranchid.Value = ownerBranch;
						parOwnerBranchid.Direction = ParameterDirection.Input;

						OracleParameter parDefID= oCmd.Parameters.Add("PTRDEFCODE", OracleType.VarChar);
						parDefID.Value = trDefCode;
						parDefID.Direction = ParameterDirection.Input;
			
						OracleParameter parAcccode= oCmd.Parameters.Add("PACCCODE", OracleType.VarChar);
						parAcccode.Value = accCode;
						parAcccode.Direction = ParameterDirection.Input;
						//
						OracleParameter parInstruNo= oCmd.Parameters.Add("PINSTRUMENTNO", OracleType.VarChar);
						parInstruNo.Value = instrumentNo;
						parInstruNo.Direction = ParameterDirection.Input;
						//
				
						OracleParameter parInstruDate= oCmd.Parameters.Add("PINSTRUMENTISSUEDATE", OracleType.DateTime);
						parInstruDate.Value = insIssueDate;
						parInstruDate.Direction = ParameterDirection.Input;
						//
				
						OracleParameter parValueDate= oCmd.Parameters.Add("PVALUEDATE", OracleType.DateTime);
						parValueDate.Value = OracleString.Null;
						parValueDate.Direction = ParameterDirection.Input;
			
						OracleParameter parNarration= oCmd.Parameters.Add("PNARRATION", OracleType.VarChar);
						parNarration.Value = txnNarration;
						parNarration.Direction = ParameterDirection.Input;
						//
						OracleParameter partxnAmounat = oCmd.Parameters.Add("PTXNAMT", OracleType.Number);
						partxnAmounat.Value = txnAmount;
						partxnAmounat.Direction = ParameterDirection.Input;
						//
						OracleParameter pariniitaorModule= oCmd.Parameters.Add("PINITIATORMODULE", OracleType.VarChar);
						pariniitaorModule.Value = inititaModule;
						pariniitaorModule.Direction = ParameterDirection.Input;
						//

						OracleParameter parBatchNo = oCmd.Parameters.Add("PTRBATCHNO", OracleType.VarChar);
						parBatchNo.Value = (strBatchNo.Equals(""))? OracleString.Null : strBatchNo;
						parBatchNo.Direction = ParameterDirection.Input;
						//

						OracleParameter parIsoRiginating = oCmd.Parameters.Add("PISORIGINATING", OracleType.VarChar);
						parIsoRiginating.Value = OracleString.Null;
						parIsoRiginating.Direction = ParameterDirection.Input;
						//

						OracleParameter parIbtaBranch = oCmd.Parameters.Add("PIBTABRANCH", OracleType.Number);
						parIbtaBranch.Value = 0;
						parIbtaBranch.Direction = ParameterDirection.Input;
						//
						OracleParameter parOriginatingDate = oCmd.Parameters.Add("PORIGINATINGDATE", OracleType.VarChar);
						parOriginatingDate.Value = OracleString.Null;
						parOriginatingDate.Direction = ParameterDirection.Input;
						//
						OracleParameter parIbtaAdvice = oCmd.Parameters.Add("PIBTAADVICE", OracleType.VarChar);
						parIbtaAdvice.Value = OracleString.Null;
						parIbtaAdvice.Direction = ParameterDirection.Input;
						//
						OracleParameter parIbtaTrCode = oCmd.Parameters.Add("PIBTATRCODE", OracleType.VarChar);
						parIbtaTrCode.Value = OracleString.Null;
						parIbtaTrCode.Direction = ParameterDirection.Input;
						//
						OracleParameter parSunDrydate= oCmd.Parameters.Add("PSUNDRYDATE", OracleType.DateTime);
						parSunDrydate.Value = DateTime.Today;
						parSunDrydate.Direction = ParameterDirection.Input;
						//
						OracleParameter parSunDryNo = oCmd.Parameters.Add("PSUNDRYNO", OracleType.VarChar);
						parSunDryNo.Value = OracleString.Null ;
						parSunDryNo.Direction = ParameterDirection.Input;
				
						OracleParameter parCurrCode = oCmd.Parameters.Add("PCURRCODE", OracleType.VarChar);
						parCurrCode.Value = OracleString.Null;
						parCurrCode.Direction = ParameterDirection.Input;
						//
						OracleParameter parSunDryId = oCmd.Parameters.Add("PSUNDRYID", OracleType.Number);
						parSunDryId.Value = 0;
						parSunDryId.Direction = ParameterDirection.Input;
						//
						OracleParameter parGlobalTxNo = oCmd.Parameters.Add("PGLOBALTXNO", OracleType.Number);
						parGlobalTxNo.Value = globalTxnno;
						parGlobalTxNo.Direction = ParameterDirection.Input;
						//
						OracleParameter parTxnDefId = oCmd.Parameters.Add("PTXNDEFID", OracleType.Number);
						parTxnDefId.Value = OracleString.Null;
						parTxnDefId.Direction = ParameterDirection.Input;
						//

						OracleParameter parAccName = oCmd.Parameters.Add("PACCNAME", OracleType.VarChar);
						parAccName.Value = OracleString.Null;
						parAccName.Direction = ParameterDirection.Input;
						//
						OracleParameter parVerifyUser = oCmd.Parameters.Add("PVERIFYUSER", OracleType.VarChar);
						parVerifyUser.Value = "ATM-SYSTEM";
						parVerifyUser.Direction = ParameterDirection.Input;
						//
						OracleParameter parIsoLine = oCmd.Parameters.Add("PISONLINE", OracleType.Number);
						parIsoLine.Value = 0;
						parIsoLine.Direction = ParameterDirection.Input;
						//

						OracleParameter parOperationId = oCmd.Parameters.Add("POPERATIONID", OracleType.VarChar);
						parOperationId.Value = OracleString.Null;
						parOperationId.Direction = ParameterDirection.Input;
						//

						OracleParameter parMkAppUserId = oCmd.Parameters.Add("PMKAPPUSERID", OracleType.VarChar);
						parMkAppUserId.Value = "ATM-SYSTEM";
						parMkAppUserId.Direction = ParameterDirection.Input;
						//

						OracleParameter parMkOsUserId = oCmd.Parameters.Add("PMKOSUSERID", OracleType.VarChar);
						parMkOsUserId.Value = OracleString.Null;
						parMkOsUserId.Direction = ParameterDirection.Input;
						//

						OracleParameter parMkTerminal= oCmd.Parameters.Add("PMKTERMINAL", OracleType.VarChar);
						parMkTerminal.Value = OracleString.Null;
						parMkTerminal.Direction = ParameterDirection.Input;
								
						oCmd.ExecuteNonQuery();
					}
					else 
					{
						string query="SELECT ATMACCID,ATMACCCURRENTBAL  FROM ATM_ACCOUNT_INFO "
							+" WHERE ATMACCCODE='"+ accCode+"'  AND ATMACCBID="+ initiatorBranch;
						OracleCommand oCmd = new OracleCommand("",con,oraTxn);
						oCmd.CommandType = CommandType.Text;
						oCmd.CommandText = query;
						OracleDataReader reader=oCmd.ExecuteReader();
						reader.Read();
						//tran.Commit();
						if(reader.HasRows)
						{
							string accId=reader[0].ToString();
							int balance=int.Parse(reader[1].ToString());
							if(balance<txnAmount && trDefCode.Equals("201"))
							{
								Console.WriteLine("Insufficient Balance");
								offlineException=2;	
							}
							else 
							{
								string update="UPDATE ATM_ACCOUNT_INFO SET ATMACCCURRENTBAL=ATMACCCURRENTBAL";
								if(trDefCode.Equals("201"))update+="-";
								else if(trDefCode.Equals("101"))update+="+";
								update += txnAmount.ToString()
									+" WHERE ATMACCCODE='"+accCode+"'  AND ATMACCBID="+initiatorBranch;
								oCmd.CommandText = update;
								oCmd.ExecuteNonQuery();
								//string insert="";
							}
						}
						else 
						{
							Console.WriteLine("Not a valid account");
							offlineException=1;	
						}


					}
				}
				
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				
			}
		

		}
		//Sajib:This method inserts Txn History
		public void updateHistory(Make.Make make,int reqType)
		{
			IDbTransaction tranHstry = null ;	//By Sajib 
			int revPossbl=0;
			long originalTrStan = Convert.ToInt64(make.p_11SystemsTraceAuditNumber); //Convert.ToInt64(make.p_90OriginalDataElement.Substring(4,6));
			if(make.p_39Response.Equals("00") && reqType==1)
				revPossbl=1;
			DateTime trnDateTime=System.DateTime.ParseExact(make.p_7TransmissionDateTime,"MMddHHmmss",null);
			int newAtmTrId=atmTrId+1;
			string atmTrcode="";
			string atmTrdefCode="";
			if(reqType==1)
			{
				atmTrcode="200";
				atmTrdefCode="201";
			}
			else
			{ 
				atmTrcode="400";
				atmTrdefCode="101";
			}

			string insert="INSERT INTO ATM_TXN_ARCH VALUES"
				+" ("+ newAtmTrId.ToString() +",'"
				+ make.p_11SystemsTraceAuditNumber +"','"
				+ make.s_102AccountIdentificationI.Trim().PadLeft(12,'0') +"','"				
				+ make.p_4AmountTransaction.Substring(0,make.p_4AmountTransaction.Length -2) +"','"
				+ atmTrcode+"',"
				+ atmTrdefCode+",'"
				+ make.p_41CardAcceptorTerminalIdentification+"',"
				+ "TO_DATE('"+ trnDateTime.Hour.ToString()+":"+trnDateTime.Minute.ToString()+ ":"
				+ trnDateTime.Second.ToString()+" "+trnDateTime.Month.ToString()+ "/"
				+ trnDateTime.Day.ToString()+"/"+trnDateTime.Year.ToString()+ "','hh24:mi:ss mm/dd/yyyy'),'"
				+ "True','"
				+ make.p_32AcquiringInstitutionIdentificationCode + "','"
				+ make.p_38AuthorizationIdentificationResponse.ToString()+"','"				
				+ make.s_102AccountIdentificationI.Trim().PadLeft(12,'0') +"',"
				+ revPossbl +","
				+ make.p_37RetrievalReferenceNumber.Trim()+","
				+ originalTrStan+")";
			System.Console.WriteLine("Insert Qry In ATM ARCH is:"+insert);
			try
			{				
				string conStringHstry =  intConString;
				OracleConnection conHstry = new OracleConnection(conStringHstry);		
				IDbConnection conHstry1 = (IDbConnection)conHstry;
								
				if(conHstry1.State==ConnectionState.Closed)conHstry1.Open();
				tranHstry = conHstry1.BeginTransaction();
				OracleTransaction oraTxn = (OracleTransaction)tranHstry;
				OracleCommand oCmd = new OracleCommand("",conHstry,oraTxn);
				oCmd.CommandType = CommandType.Text;
				oCmd.CommandText = insert;
				oCmd.ExecuteNonQuery();	
				if((make.p_39Response.Equals("00")|| make.p_39Response.Equals("32") || make.p_39Response.Equals("22") ) && reqType==0)
				{
						long originalStan;
					if(null != make.p_90OriginalDataElement) 
					{
						originalStan = Convert.ToInt64(make.p_90OriginalDataElement.Substring(4,6));
					}
					
					string update="UPDATE ATM_TXN_ARCH SET ATMREVERSED=0 WHERE  ATMTRSTAN='"
						+ originalTrStan +"' AND ATMTRDEFID=201";
					System.Console.WriteLine("Important update string for atm_txn_arch is "+ update);
					oCmd.CommandText=update;
					oCmd.ExecuteNonQuery();
				}
				if(cbsConnected==false && offlineException==0)
				{
					insert="INSERT INTO ATM_TXN VALUES"
						+" ("+ newAtmTrId.ToString() +",'"
						+ make.p_11SystemsTraceAuditNumber +"','"
						+ make.p_2PrimaryAccountNumber +"','"				
						+ make.p_4AmountTransaction +"','"
						+ atmTrcode+"',"
						+ atmTrdefCode+",'"
						+ make.p_41CardAcceptorTerminalIdentification+"',"
						+ "TO_DATE('"+ trnDateTime.Hour.ToString()+":"+trnDateTime.Minute.ToString()+ ":"
						+ trnDateTime.Second.ToString()+" "+trnDateTime.Month.ToString()+ "/"
						+ trnDateTime.Day.ToString()+"/"+trnDateTime.Year.ToString()+ "','hh24:mi:ss mm/dd/yyyy'),'"
						+ "True','"
						+ make.p_32AcquiringInstitutionIdentificationCode + "','"
						+ "False','"
						+ make.p_38AuthorizationIdentificationResponse.ToString()+"','"				
						+ make.s_102AccountIdentificationI +"')";
					System.Console.WriteLine("Insert Qry In ATM_TXN is:"+insert);
					oCmd.CommandText = insert;
					oCmd.ExecuteNonQuery();	

				}
				tranHstry.Commit();
				conHstry1.Close();

			}
			catch(Exception ex)
			{
				tranHstry.Rollback();
				Console.WriteLine("Record of this msg exists"+ex.Message);
				
			}

	
		}

		//Sajib:This method checks Txn History for Reversal
		public bool checkHistory(Make.Make make,int reqType)
		{
			IDbTransaction tran = null ;
			bool toReturn;
			long oroginalTrStan = 0;
			if(null != make.p_90OriginalDataElement)
			{	
				oroginalTrStan = Convert.ToInt64(make.p_90OriginalDataElement.Substring(4,6));
			}
			//string query="SELECT * FROM ATM_TXN_ARCH WHERE ATMTRCODE='"+ make.p_38AuthorizationIdentificationResponse+"'"; 
			//string query="SELECT * FROM ATM_TXN_ARCH WHERE ATMTRCODE='588080'"; 
			string query="SELECT * FROM ATM_TXN_ARCH WHERE ATMTRSTAN="+oroginalTrStan; 
			
			string accountCode = "";	 
			accountCode = make.s_102AccountIdentificationI.Trim().PadLeft(12,'0'); 
			decimal txnAmount = Convert.ToDecimal(make.p_4AmountTransaction.Substring(0,make.p_4AmountTransaction.Length -2));
			
			if(reqType==0)
				query+=" AND ATMTRACCID='"+ accountCode +"'"
					//+"' AND ATMTRAMOUNT="+ make.p_4AmountTransaction.Substring(0,make.p_4AmountTransaction.Length -2)
					+" AND ATMTRDEFID=201 AND ATMREVERSED=1"
					+" and ATMRETREFNO ="+make.p_37RetrievalReferenceNumber ;
			
			try
			{				
				string conString =  intConString;
				OracleConnection con = new OracleConnection(conString);		
				IDbConnection con1 = (IDbConnection)con;
								
				if(con1.State==ConnectionState.Closed)con1.Open();
				tran = con.BeginTransaction();
				OracleTransaction oraTxn = (OracleTransaction)tran;
				OracleCommand oCmd = new OracleCommand("",con,oraTxn);
				oCmd.CommandType = CommandType.Text;
				oCmd.CommandText = query;
				OracleDataReader reader=oCmd.ExecuteReader();
				reader.Read();
				if(reader.HasRows)
				{					
					Console.WriteLine("Request record found ");	
					//tran.Commit();
					toReturn= true;
				}
				else 
				{
					//make.p_39Response="40";
					Console.WriteLine("Request record not found ");	
					toReturn=false;
					
				}
				query="SELECT MAX(ATMTRID) FROM ATM_TXN_ARCH";
				oCmd.CommandText = query;
				reader=oCmd.ExecuteReader();
				reader.Read();
				if(reader.HasRows)
				{
					atmTrId=int.Parse(reader[0].ToString());
				}
				else atmTrId=0;
				tran.Commit();
				return toReturn;

			}
			catch(Exception ex)
			{
				tran.Rollback();
				//make.p_39Response="39";
				Console.WriteLine("Problem Occurred :"+ex.Message);	
				return false;
			}			
		}
		
		/*
		//Sajib:This method inserts Txn History
		public void updateHistory(Make.Make make,int reqType)
		{
			IDbTransaction tranHstry = null ;	//By Sajib 
			int revPossbl=0;
			if(make.p_39Response.Equals("00000") && reqType==1)
				revPossbl=1;
			DateTime trnDateTime=System.DateTime.ParseExact(make.p_7TransmissionDateTime,"MMddHHmmss",null);
			string insert="INSERT INTO ATMTXNHISTORY VALUES"
				+" ("+ reqType +",'"
				+ make.p_11SystemsTraceAuditNumber +"','"
				+ make.p_2PrimaryAccountNumber +"','"
				+ make.p_3ProcessingCode +"',"
				+ int.Parse(make.p_4AmountTransaction) +","
				+ "TO_DATE('"+ trnDateTime.Hour.ToString()+":"+trnDateTime.Minute.ToString()+ ":"
				+ trnDateTime.Second.ToString()+" "+trnDateTime.Month.ToString()+ "/"
				+ trnDateTime.Day.ToString()+"/"+trnDateTime.Year.ToString()+ "','hh24:mi:ss mm/dd/yyyy'),'"
				+ make.p_23CardSequenceNumber + "','"
				+ make.p_32AcquiringInstitutionIdentificationCode + "','"
				+ make.p_41CardAcceptorTerminalIdentification +"','"
				+ make.s_102AccountIdentificationI +"','"
				+ make.p_39Response+"',"+ revPossbl +")";
			try
			{				
				string conStringHstry =  intConString;
				OracleConnection conHstry = new OracleConnection(conStringHstry);		
				IDbConnection conHstry1 = (IDbConnection)conHstry;
								
				if(conHstry1.State==ConnectionState.Closed)conHstry1.Open();
				tranHstry = conHstry1.BeginTransaction();
				OracleTransaction oraTxn = (OracleTransaction)tranHstry;
				OracleCommand oCmd = new OracleCommand("",conHstry,oraTxn);
				oCmd.CommandType = CommandType.Text;
				oCmd.CommandText = insert;
				oCmd.ExecuteNonQuery();	
				if(make.p_39Response.Equals("00000") && reqType==0)
				{
					string update="UPDATE ATMTXNHISTORY SET RVRSLPOSSBL=0 WHERE STAN='"
						+ make.p_11SystemsTraceAuditNumber +"' AND REQTYPE=1";
					oCmd.CommandText=update;
					oCmd.ExecuteNonQuery();
				}
				tranHstry.Commit();
				conHstry1.Close();

			}
			catch(Exception ex)
			{
				tranHstry.Rollback();
				Console.WriteLine("Record of this msg exists"+ex.Message);
				
			}

	
		}

		//Sajib:This method checks Txn History for Reversal
		public bool checkHistory(Make.Make make,int reqType)
		{
			IDbTransaction tran = null ;
			string query="SELECT * FROM ATMTXNHISTORY WHERE STAN='"+ make.p_11SystemsTraceAuditNumber+"'"; 
				 
			if(reqType==0)
				query+=" AND PAN='"+ make.p_2PrimaryAccountNumber 
					  +"' AND TXNAMOUNT="+ int.Parse(make.p_4AmountTransaction)
					  +" AND REQTYPE=1 AND RSPNSECODE='00000' AND RVRSLPOSSBL=1";
			
			try
			{				
				string conString =  intConString;
				OracleConnection con = new OracleConnection(conString);		
				IDbConnection con1 = (IDbConnection)con;
								
				if(con1.State==ConnectionState.Closed)con1.Open();
				tran = con.BeginTransaction();
				OracleTransaction oraTxn = (OracleTransaction)tran;
				OracleCommand oCmd = new OracleCommand("",con,oraTxn);
				oCmd.CommandType = CommandType.Text;
				oCmd.CommandText = query;
				OracleDataReader reader=oCmd.ExecuteReader();
				reader.Read();
				if(reader.HasRows)
				{					
					Console.WriteLine("Request record found ");	
					tran.Commit();
					return true;
				}
				else 
				{
					//make.p_39Response="40";
					Console.WriteLine("Request record not found ");				
				}
				tran.Commit();
				return false;

			}
			catch(Exception ex)
			{
				tran.Rollback();
				//make.p_39Response="39";
				Console.WriteLine("Problem Occurred :"+ex.Message);	
				return false;
			}			
		}*/
		// this method is written by sajjad
		private decimal getCurrentBalance(IDbTransaction dbtr,string accCode)
		{
			decimal currentbalance;
			string query;
			//			string padding = "";
			if(cbsConnected==true && offlineException!=1)
			{

				query = "SELECT  round(ACCOUNT.ACCCURRENTBALANCE)||'00' from account WHERE "
					+ " ACCCODE = '" + accCode + "'";
			}
			else 
				query = "SELECT round(ATMACCCURRENTBAL)||'00' FROM ATM_ACCOUNT_INFO "
					+" WHERE ATMACCCODE='"+accCode+"'";
			try
			{
				//this method is done by Sajib
				//code for connection....
				OracleConnection con = (OracleConnection)dbtr.Connection;
				OracleTransaction oraTxn = (OracleTransaction)dbtr;
				OracleCommand oCmd = new OracleCommand("",con,oraTxn);
				oCmd.CommandType = CommandType.Text;
				oCmd.CommandText = query;
				
				//reading data...
				OracleDataReader aReader = oCmd.ExecuteReader();
				aReader.Read();
				if(aReader.HasRows)
				{
					
					
					
					if(cbsConnected==true)
					{
						currentbalance = Convert.ToDecimal(aReader[0].ToString());
						
						
					}
					else
					{
						currentbalance= Convert.ToDecimal(aReader[0].ToString());
						
					}
					
					return currentbalance;
				}
				else return 0;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return 0;
			}
		}
		
		
		//this method is written by Sajib
		private string[] doBalanceQuery(IDbTransaction dbtr,string accCode)
		{
			string []ret=new string[3];
			string query;
			//			string padding = "";
			if(cbsConnected==true && offlineException!=1)
			{

				query = "SELECT  round(ACCOUNT.ACCCURRENTBALANCE), round(ACCOUNT.ACCLIENAMOUNT), "
					+ "round(ACCOUNT.ACCBLOCKAMOUNT), round(ACCOUNTTYPE.ATMINBALANCE),accatid FROM ACCOUNT, ACCOUNTTYPE WHERE "
					+ "ACCOUNTTYPE.ATID = (SELECT ACCATID FROM ACCOUNT WHERE ACCOUNT.ACCCODE ='"+accCode+"') "
					+ "AND ACCOUNT.ACCCODE = '" + accCode + "'";
			}
			else 
				query = "SELECT round(ATMACCCURRENTBAL),round(ATMACCMINBAL) FROM ATM_ACCOUNT_INFO "
					+" WHERE ATMACCCODE='"+accCode+"'";
			try
			{
				//this method is done by Sajib
				//code for connection....
				OracleConnection con = (OracleConnection)dbtr.Connection;
				OracleTransaction oraTxn = (OracleTransaction)dbtr;
				OracleCommand oCmd = new OracleCommand("",con,oraTxn);
				oCmd.CommandType = CommandType.Text;
				oCmd.CommandText = query;
				
				//reading data...
				OracleDataReader aReader = oCmd.ExecuteReader();
				aReader.Read();
				if(aReader.HasRows)
				{
					double currentbalance,availableBalance;
					string accountType ="";
					
					if(cbsConnected==true)
					{
						currentbalance = double.Parse(aReader[0].ToString());
						double acclienamount = double.Parse(aReader[1].ToString());
						double accblockamount = double.Parse(aReader[2].ToString());
						double atminbalance = double.Parse(aReader[3].ToString());
						accountType = aReader[4].ToString();
						availableBalance = currentbalance - (acclienamount + accblockamount + atminbalance);
						
					}
					else
					{
						availableBalance=double.Parse(aReader[0].ToString());
						currentbalance=availableBalance+double.Parse(aReader[1].ToString());
					}
					decimal finalAvBalance = Convert.ToDecimal(availableBalance.ToString().PadRight(availableBalance.ToString().Length + 2 ,'0'));
					availableBalance = double.Parse(finalAvBalance.ToString());

					decimal finalCurrBalance = Convert.ToDecimal(currentbalance.ToString().PadRight(currentbalance.ToString().Length + 2 ,'0'));
					currentbalance = double.Parse(finalCurrBalance.ToString());
					////Padding for currentBalance
					ret[0] = padding(currentbalance.ToString(),12-currentbalance.ToString().Length);
					
					////Padding for availBalance
					ret[1]= padding(availableBalance.ToString(),12-availableBalance.ToString().Length);
					ret[2]= accountType;
					
					return ret;
				}
				else return null;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}
		
		//For query in DB if any record exists 
		bool checkRecord(string connString,string query)
		{
			OracleConnection con = new OracleConnection(connString);		
			IDbConnection con1 = (IDbConnection)con;
								
			con1.Open();//if(con1.State==ConnectionState.Closed)
			tran = con.BeginTransaction();
			OracleTransaction oraTxn = (OracleTransaction)tran;
			OracleCommand oCmd = new OracleCommand("",con,oraTxn);
			oCmd.CommandType = CommandType.Text;
			oCmd.CommandText = query;
			OracleDataReader reader=oCmd.ExecuteReader();
			reader.Read();
			tran.Commit();
			if(reader.HasRows)
				return true;
			else return false;
		}
		private string doMiniStatementQuery(IDbTransaction dbtr,string accCode,out int totalDataCount)
		{
			totalDataCount=0;
			string query;
			if(cbsConnected==true)
				//				query= "SELECT * FROM "
				//					+" ("
				//					+" select ROWNUM,TO_CHAR(TR.TRDATE,'YYMMDD') AS TXNDATE,RPAD(TR.TRNARRATION,20,' ') AS DESCRIPITON ,LPAD(TR.TRAMOUNT,12,'0') AS TXNAMOUNT,CASE WHEN SUBSTR(TRTRDID,1,1)='1' THEN '+' ELSE '-' END AS Sign,LPAD(TRGLOBALTXNNO,7,'0') AS REFF "
				//					+" FROM TRANSACTIONRECORD TR,ACCOUNT WHERE ACCID = TRREFACCID AND "
				//					+" ACCCODE = '"+accCode +"' "
				//					+" ORDER BY TRDATE,trid DESC "
				//					+" ) WHERE ROWNUM <=10 ";
				query= "SELECT * FROM "
					+" ("
					+" select ROWNUM,TO_CHAR(TR.TRDATE,'YYMMDD') AS TXNDATE,RPAD(txndef.TRDDESCRIPTION,20,' ') AS DESCRIPITON ,LPAD(TR.TRAMOUNT,12,'0') AS TXNAMOUNT,CASE WHEN SUBSTR(TRTRDID,1,1)='1' THEN '+' ELSE '-' END AS Sign,LPAD(TRGLOBALTXNNO,7,'0') AS REFF "
					+" FROM TRANSACTIONRECORD TR,ACCOUNT,transactiondefinition txndef  WHERE ACCID = TRREFACCID AND trtrdid = trdid and "
					+" ACCCODE = '"+accCode +"' "
					+" ORDER BY trid DESC "
					+" ) WHERE ROWNUM <=10 ";


			else query="SELECT ATMTRDATE,ATMTRAMOUNT,ATMTXNCODE FROM ATM_ACC_STATEMENT "
					 +" WHERE ATMACCID = "
					 +"(SELECT ATMACCID FROM ATM_ACCOUNT_INFO WHERE ATMACCCODE='"+accCode+"')";
			try
			{
				//this method is done by Sajib
				//code for connection....
				OracleConnection con = (OracleConnection)dbtr.Connection;
				OracleTransaction oraTxn = (OracleTransaction)dbtr;
				OracleCommand oCmd = new OracleCommand("",con,oraTxn);
				oCmd.CommandType = CommandType.Text;
				oCmd.CommandText = query;
				
				//reading data...
				OracleDataReader aReader = oCmd.ExecuteReader();				
				

				string ret="";
				
				while(aReader.Read())
				{
					//string finalStrToreturn ="";
					string txnDate = aReader[1].ToString();
					string txnDesc = aReader[2].ToString();
					string txnAmount = aReader[3].ToString();
					string txnSign = aReader[4].ToString();
					string txnReff = aReader[5].ToString();
					ret +=txnDate+txnDesc+txnAmount+txnSign+txnReff ;
					totalDataCount++;
					//					string []trDate=splitDate(aReader[0].ToString());
					//					string month=trDate[1];
					//					month=padding(month,2-month.Length);
					//					string date=trDate[0];
					//					date=padding(date,2-date.Length);
					//					string amount=aReader[1].ToString();
					//					amount=padding(amount,12-amount.Length);
					//					string opCode=aReader[2].ToString();
					//					//padding(aReader[2].ToString(),3-aReader[2].ToString().Length);
					//					ret+=month+date+opCode+amount;
					//					if(opCode.StartsWith("1"))
					//						ret+="+";
					//					else ret+="-"; 					
				}
				return ret;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}
		//this method is done by RAKIB

		public string BalanceString(IDbTransaction tran,string pan)
		{
			string []str=new string[3];
			str=doBalanceQuery(tran,pan);
			string lengthString = "";
			string accountType = "" ;
			if(str[2].ToString().Equals("102"))
			{
				accountType = "20";
			}
			else
			{
				accountType = "10";
			}
			// bal type 01 is for actual bal and 02 is for available bal
			string balTypeActualBal = accountType+"01050C";
			string balTypeAvailableBal = accountType+"02050C"; 
			string resultStringActualBal = str[0];//str[0]+str[1]+"1"; comm out by sajjad
			string resultStringAvailableBal = str[1];
			
			resultStringActualBal.PadLeft(12,' ');
			resultStringAvailableBal.PadLeft(12,' ');
			return lengthString+balTypeAvailableBal +resultStringAvailableBal+balTypeActualBal+resultStringActualBal;

		}
	  
		public string padding(string str,int pad)
		{
			string temp="";
			for(int i=0;i<pad;i++)
				temp += "0";
			temp += str;
			return temp;
		}
		//this method is done by Sajib
		public string[] splitDate(string date)
		{
			string []temp=new string[3];
			char []separator={'/',' '};
			temp=date.Split(separator,3);
			return temp;
		}
	}
}
