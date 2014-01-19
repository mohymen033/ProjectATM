using System;
using System.Data ;
using System.Text ;
using ConfigureQCash;
using com.mislbd.sunflower.utility.dao;
using Common;

namespace Make
{
	/// <summary>	
	/// Command class for the Command Pattern.
	/// </summary>
	public abstract class Make
	{
		#region Elements(Fields) of MAKE Class

		protected string wholeData;
		public  string MTI;
		public  string BITMAP;
		private CashConfig cashConfigRequest;
		private CashConfig cashConfigResponse; 		
		public string requestBITMAP;
		public  string actualReqBITMAP;
		public string responseBITMAP;

		#endregion
		
		#region Data Elements(Fields) of ISO8583		
		
		public int acquirer_ID;
		public string p_0MessageType = null;
		public string p_1BitMapPrimary = null;
		public string p_2PrimaryAccountNumber = null;

		public string p_3ProcessingCode = null;
		public string p_3_1TransactionCode = null;
		public string p_3_2FromAccountType = null;
		public string p_3_3ToAccountType = null;

		public string p_4AmountTransaction = null;
		public string p_5AmountSettlement = null;
		public string p_6AmountCardholderBilling = null;
		public string p_7TransmissionDateTime = null;
		//		public string p_8AmountCardholderBillingFee;
		public string p_9ConversionRateSettlement =null;
		//		public string p_10ConversionRateCardholderBilling;
		public string p_11SystemsTraceAuditNumber = null;		
		public string p_12TimeLocalTransaction = null;//12
		public string p_13DateLocalTransaction = null;
		public string p_14DateExpiration;
		public string p_15DateSettlement = null;
		//		public string p_16DateConversion;
		public string p_17DateCapture = null;
		public string p_18MerchantCategoryCode = null;
		public string p_19AcquiringInstitutionCountryCode = null;
		public string p_22PointOfServiceEntryMode = null;
		public string p_23CardSequenceNumber = null;
		public string p_25PointOfServiceConditionCode = null;
		public string p_26MessageReasonCode = null;
		//		public string p_27AuthorizationIdentificationResponseLength;
		public string p_28AmountTransactionFee = null;
		//		public string p_29AmountSettlementFee;
		public string p_32AcquiringInstitutionIdentificationCode = null;
		public string p_33ForwardingInstitutionIdentificationCode = null;
		public string p_35TrackIIData = null;
		public string p_37RetrievalReferenceNumber = null;
		public string p_38AuthorizationIdentificationResponse = null;  //Dummy
		public string p_39Response="00";           //Dummy
		//		public string p_40ServiceRestrictionCode;
		public string p_41CardAcceptorTerminalIdentification = null;
		public string p_42CardAcceptorIdentificationCode = null;

		public string p_43CardAcceptorNameLocation = null;
		public string p_43_1TerminalOwner = null;
		public string p_43_2TerminalCity = null;
		public string p_43_3TerminalState = null;
		public string p_43_4TerminalCountry = null;
		public string p_43_5TerminalAddress = null;
		public string p_43_6TerminalBranch = null;
		public string p_43_7TerminalRegion = null;
		public string p_43_8TerminalClass = null;
		public string p_43_9TerminalDate = null;
		public string p_43_10TerminalPSName = null;
		public string p_43_11TerminalFinName = null;
		public string p_43_12TerminalRetailerName = null;
		public string p_43_13TerminalCounty = null;
		public string p_43_14TerminalZipCode = null;
		public string p_43_15TerminalTimeOffset = null;

		public string p_44ResultPINCVV = null;
		public string p_44_1ResultPIN = null;
		public string p_44_2ResultCVV = null;

		public string p_45TrackIData = null;

		//		public string p_47AdditionalDataNational;
		public string p_48ReferenceToOtherTransaction = null;
		public string p_48_1OtherTransactionsRRN = null;
		public string p_48_2OtherTransactionsPAN = null;

		public string p_49CurrencyTransactionCode = null;
		public string p_50CurrencyCodeSettlement = null;
		public string p_51CurrencyCodeCardholderBilling = null;
		public string p_52PersonalIdentificationNumber = null;
		public string p_54AmountAdjustment = null;
		public string p_55ICCSystemRelatedData = null;

		//		public string p_57AuthorizationLifeCycle;
		//		public string p_58NationalPointOfServiceConditionCode;

		public string p_61CardIssuerData = null;
		public string p_61_1IssuingInstitution = null;
		public string p_61_2IssuingPaymentSystem = null;

		public string p_62ExternalTransactionAttributes = null;
		public string p_63NewPin = null;
		public string p_64MAC = null;

		public string p_70NetworkManagementInformationCode = null;
		public string p_90OriginalDataElement = null;
		

		public string s_95ReplacementAmountData = null;
		public string s_95_1ActualTxnAmount= null;
		public string s_95_2ActualSettlementAmount= null;
		public string s_95_3ActualTxnFeeSign = null;
		public string s_95_4ActualTxnFeeAmount = null;
		public string s_95_5ActualSettlementFeeSign = null;
		public string s_95_6ActualSettlemtnFeeAmount = null;

		public string s_100ReceivingInstitutionIdentificationCode = null;
		public string s_102AccountIdentificationI = null;
		public string s_103AccountIdentificationII = null;
		public string s_104HostNetIdentification = null;

		public string s_105BalanceData = null;
		public string s_105_1LedgerBalance = null;
		public string s_105_2AvailableBalance = null;
		public string s_105_3BalanceCurrencyAccount = null;

		public string s_106MultiCurrencyData = null;
		public string s_106_1CurrencyAccountTo = null;
		public string s_106_2CurrencyOrigin = null;
		public string s_106_3AmountAccountTo = null;
		public string s_106_4AmountOrigin = null;
		public string s_106_5ExchangeRateAccount = null;
		public string s_106_6ExchangeRateAccountTo = null;

		public string s_107RRNSentToFinalInterchange = null;
		public string s_108RegionalListingDataStringMessage = null;

		public string s_109MultiAccountData = null;
		public string s_109_1Len = null;
		public string s_109_2FromOrTo = null;
		public string s_109_3AccountNumber = null;
		public string s_109_4AccountTitle = null;
		public string s_109_5AccountCurrency = null;

		public string s_110NumericMessage = null;
		public string s_111PersonalNumber = null;
		public string s_114Statement = null;
		public string s_120ExtendedTransactionData = null;
		public string s_121AdditionalPOSData = null;
		public string s_121_1POSTransactionCategory = null;
		public string s_121_2DraftCapture = null;
		public string s_121_3CVV2 = null;
		public string s_121_4ClerkID = null;
		public string s_121_5InvoiceNumber = null;
		public string s_121_6POSBatchAndShiftData = null;

		public string s_122IIIDSecureData = null;
		public string s_122_1CAVV = null;
		public string s_122_2AuthenticationResultsCode = null;
		public string s_122_3UnpredictableNumber = null;
		public string s_122_4MerchantTranId = null;

		public string s_123MiscTransactionAttribute = null;

		public string s_126PreAuthorizationParameters = null;
		public string s_126_1OriginalTransactionInvoiceNumber = null;
		public string s_126_2OriginalSeqNumber = null;
		public string s_126_3PreauthorizationHold = null;

		public string s_128SecondaryMAC = null;

		public string fieldValue = "";
				
		#endregion
		
		#region Region Construnctor
		
		public Make()
		{			
		}

		public Make(CashConfig cashConfigRequest, CashConfig cashConfigResponse, string wholeData)
		{			
			this.cashConfigRequest = cashConfigRequest;
			this.cashConfigResponse = cashConfigResponse;
			this.wholeData = wholeData ;
		}

		#endregion

		#region Abstract Functions that will be overriden

		public abstract bool MakeRequest();
		
		public abstract string MakeResponse();
		
		#endregion

		#region Utilities that every Make class uses.
		
		private string[] hexToBinary = {"0000","0001","0010","0011",
										   "0100","0101","0110","0111",
										   "1000","1001","1010","1011",
										   "1100","1101","1110","1111"
									   };
		protected static string DUMMY = "0000000000000000" ;

		protected string MakeBitMapToBinary(string hexBitmap)
		{
		
			//Field bitmapField = cc.FindFieldByID(BITMAP_ID) ;
			
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

		protected int StoreFieldValue(string wholeData, Field field, int length, int index )
		{
			// take care of LLVAR formats
			// length of this field is not fixed
			// instead first two characters specify the number
			// LL stands for the length and next comes the
			// lengthed number of values
			string fieldId = field.Id;

			//this local variable is taken as member variable by Asad
			//string fieldValue = "";

			//member variable fieldValue is reset
			this.fieldValue = "";
			
			try
			{						 					
				if(field.Format.Equals("LLVAR"))
				{
					string temp = "" ;
					for(int j = index ; j < index + 2 ; j++ )
						temp += wholeData[j] ;
					length = Convert.ToInt16(temp);
					index += 2 ;
					for(int i = index ; i< index+length; i++ )
						fieldValue += wholeData[i] ;
					
					length += 2 ;					
				}

				else if(field.Format.Equals("LLLVAR"))
				{	
					string temp = "" ;
					for(int j = index ; j < index+3 ; j++ )
						temp += wholeData[j] ;
					length = Convert.ToInt16(temp);
					
					index +=3;
					
					for(int i = index ; i< index+length ; i++ )
						fieldValue += wholeData[i] ;
					length += 3 ;
				}
				else
				{

					for(int i = index ; i< index+length; i++ )
						fieldValue += wholeData[i] ;
					
				}

				Console.WriteLine("REQUEST: Field ID - "+ field.Id +"  Value - " + fieldValue);
				StoreValueInVariable( fieldId, fieldValue );
			}
			catch(Exception ex)
			{
				Console.WriteLine("ERROR: Message From Store Field Value - " + ex.ToString());
				this.p_39Response="00018";
			}
			return length;		
		}

		protected int StoreSubFieldValue(String subFieldToParse, Field field, int length )
		{
			// take care of LLVAR formats
			// length of this field is not fixed
			// instead first two characters specify the number
			// LL stands for the length and next comes the
			// lengthed number of values
									
			string fieldId = field.Id;
			//this line is added by Asad
			this.fieldValue = "";
		
			if(field.Format.Equals("LLVAR"))
			{
				string temp = "" ;
				for(int j = 0 ; j < 2 ; j++ )
					temp += subFieldToParse[j];
				length = Convert.ToInt16(temp);
			}

			if(field.Format.Equals("LLLVAR"))
			{
				string temp = "" ;
				for(int j = 0 ; j < 3 ; j++ )
					temp += subFieldToParse[j];
				length = Convert.ToInt16(temp);
			}

			for(int i = 0 ; i< length ; i++ )
			{
				//this line is modified by Asad
				//field.Value += subFieldToParse[i];
				fieldValue += subFieldToParse[i];
			}

			//this line is added by Asad
			StoreValueInVariable(fieldId,fieldValue);
			return length;
		
		}

		/// <summary>
		/// Get field value by id
		/// </summary>
		
		protected string getFieldValue(string id)
		{
			switch(id)
			{
				case "0" :
					return p_0MessageType;///By SR
				case "2" :
					return p_2PrimaryAccountNumber;

				case "3" :
					//return p_3ProcessingCode;
					return p_3_1TransactionCode + p_3_2FromAccountType + p_3_3ToAccountType ;
							
				case "4" :
					return p_4AmountTransaction;
				case "5" :
					return p_5AmountSettlement;
					
				case "6" :	
					return p_6AmountCardholderBilling;
					
				case "7" :
					return p_7TransmissionDateTime;
				case "9" :
					return p_9ConversionRateSettlement;
				case "11" :
					return p_11SystemsTraceAuditNumber;	
					
				case "12" :
					return p_12TimeLocalTransaction;
					
				case "13" :
					return p_13DateLocalTransaction ;
				case "14" :
					return p_14DateExpiration ;
				case "15" :
					return p_15DateSettlement ;
				case "17" :
					return p_17DateCapture ;
				case "18" :
					return p_18MerchantCategoryCode;
					
				case "19" :
					return p_19AcquiringInstitutionCountryCode;
					
				case "22" :
					return p_22PointOfServiceEntryMode;
					
				case "23" :
					return p_23CardSequenceNumber;
					
				case "25" :
					return p_25PointOfServiceConditionCode ;
					
				case "26" :
					return p_26MessageReasonCode ;
					
				case "28" :
					return p_28AmountTransactionFee ;
					
				case "32" :
					return p_32AcquiringInstitutionIdentificationCode ;
					
				case "33" :
					return p_33ForwardingInstitutionIdentificationCode ;
					
				case "35" :
					return p_35TrackIIData ;
					
				case "37" :
					return p_37RetrievalReferenceNumber ;
					
				case "38" :
				{
					return p_38AuthorizationIdentificationResponse;
				}

				case "39" :
				{
					return p_39Response;
				}
													
				case "41" :
					return p_41CardAcceptorTerminalIdentification ;
				case "42" :
					return p_42CardAcceptorIdentificationCode ;
					
				case "43" :
					//return p_43CardAcceptorNameLocation ;
					return getCardAcceptorNameLocation();
					
				case "44" :
					//return p_44ResultPINCVV ;
					return p_44_1ResultPIN + p_44_2ResultCVV;
					
				case "45" :	
					return p_45TrackIData ;
					
				case "48" :
					return p_48_1OtherTransactionsRRN + p_48_2OtherTransactionsPAN ;
					
				case "49" :
					return p_49CurrencyTransactionCode ;
				case "50" :
					return p_50CurrencyCodeSettlement ;
				case "51" :
					return p_51CurrencyCodeCardholderBilling ;
					
				case "52" :
					return p_52PersonalIdentificationNumber ;
					
				case "54" :
					return p_54AmountAdjustment ;
					
				case "55" :
					return p_55ICCSystemRelatedData ;
					
				case "61" :
					//return p_61CardIssuerData ;
					return p_61_1IssuingInstitution + p_61_2IssuingPaymentSystem;
					
				case "62" :
					return p_62ExternalTransactionAttributes ;
					
				case "63" :
					return p_63NewPin ;
					
				case "64" :
					return p_64MAC ;
					
				case "70" :
					return p_70NetworkManagementInformationCode ;
				case "90" :
					return p_90OriginalDataElement ;
					

				case "95" :
					//return s_95ReplacementAmountData ;
					return s_95_1ActualTxnAmount + s_95_2ActualSettlementAmount + s_95_3ActualTxnFeeSign+s_95_4ActualTxnFeeAmount+s_95_5ActualSettlementFeeSign+s_95_6ActualSettlemtnFeeAmount;
					
				case "100" :
					return s_100ReceivingInstitutionIdentificationCode ;
					
				case "102" :
					return s_102AccountIdentificationI ;
					
				case "103" :
					return s_103AccountIdentificationII ;
					
				case "104" :
					return s_104HostNetIdentification ;
					
				case "105" :
				{
					return s_105BalanceData;
				}
					
				case "106" :
					return getMultiCurrencyData() ;
					
				case "107" :
					return s_107RRNSentToFinalInterchange ;
					
				case "108" :
					return s_108RegionalListingDataStringMessage ;
					
				case "109" :
					//return s_109MultiAccountData ;
					return getMultiAccountData();
					
				case "110" :
					return s_110NumericMessage ;
					
				case "111" :
					return s_111PersonalNumber ;
					
				case "114" :
					return s_114Statement ;
				case "120" :
					return s_120ExtendedTransactionData ;
					
				case "121" :
					//return s_121AdditionalPOSData ;
					return getAdditionalPOSData();
					
				case "122" :
					return s_122_1CAVV + s_122_2AuthenticationResultsCode
						   + s_122_3UnpredictableNumber + s_122_4MerchantTranId ;
					
				case "123" :
					return s_123MiscTransactionAttribute  ;
					
				case "126" :
					return s_126_1OriginalTransactionInvoiceNumber + s_126_2OriginalSeqNumber
						   + s_126_3PreauthorizationHold ;
					
				case "128" :
					return s_128SecondaryMAC ;
					
				default:
					return null;
			}

		}

		
		#endregion
	
		#region public method
		
		public void PerformRequest()
		{
			try
			{			
				int index = 0;
				int lengthHeader = 1;
				int acquirer = 1;
				this.acquirer_ID = acquirer;                											
				bool flagBitmap = false ;
				bool flagStoreSubField = false ;
				int counterSubFieldHas = 0 ;
				string subFieldToParse = null ;
				string currentFieldId = null ;	

				MTI = wholeData.Substring(0,4);//added by monon must need to check
				
				foreach(Field field in cashConfigRequest.Fields)
				{
					if(field.Id != ""  )
					{
						int length = 0;
					
						if(field.Size != "" ) 
							length = Convert.ToUInt16(field.Size);										
						
						//check if it has gone past the boundary								
						if( field.Name != "dummy" )
						 {			
							if(flagBitmap)
							{		
								currentFieldId = field.Id ;
								
								if(field.Id.Length <=3 )
								{
									if(p_1BitMapPrimary[ Convert.ToInt16(field.Id) - 1 ] == '1' ) 
									{	
										length = this.StoreFieldValue(wholeData,field,length,index);
										index += length ;																																																																																		
										counterSubFieldHas = Convert.ToInt16(field.Sub) ;
									}                                   
								}
							}

							if( (!flagStoreSubField) && counterSubFieldHas > 0 )
							{							
								flagStoreSubField = true ;
								flagBitmap = false ;													
								subFieldToParse = fieldValue;	
							}
							
							if(field.Id.Equals("0"))
							{																
								this.StoreFieldValue( wholeData, field, 4, lengthHeader-1 );
								index += length + lengthHeader -1 ;								
							}
							
							if(field.Name.Equals("BIT MAP"))
							{
								//This length is process-code specific
								length = 16 ;
								//use the DUMMY value so that iteration can complete for all the 127 fields.
								//This won't be necessary if everything is done in a class specific way.																			
								p_1BitMapPrimary = MakeBitMapToBinary(wholeData.Substring(index,length));								
								//not always the case that the secondary BITMAP will be zero
								//if the first bit of Primary BITMAP is 1 , secondary BITMAP should exists
								if(p_1BitMapPrimary.Substring(0, 1).CompareTo("1") == 0)
								{
									//p_1BitMapPrimary.Remove(64, 64);
									string secondaryBitMap = MakeBitMapToBinary( wholeData.Substring(index + 16,length));
									p_1BitMapPrimary = p_1BitMapPrimary + secondaryBitMap;

									length = 32;
								}
								else
								{
									string dummyBitMap = MakeBitMapToBinary(DUMMY);
									p_1BitMapPrimary = p_1BitMapPrimary + dummyBitMap;
								}
								
								requestBITMAP=p_1BitMapPrimary;
								flagBitmap = true ;
								index += length ;
							}						
						}		
			
						if( flagStoreSubField && field.Id != currentFieldId )
						{			
							counterSubFieldHas-- ;
							length = this.StoreSubFieldValue( subFieldToParse, field, length ) ;
							
							subFieldToParse = subFieldToParse.Remove(0, length);
								
							if(counterSubFieldHas == 0)
							{
								flagBitmap = true ;
								flagStoreSubField = false ;
							}
						}			
					}
				}							
			}
			catch(Exception x)
			{
				Console.WriteLine( "ERROR: ParseFile - "+ x.ToString() );
				this.p_39Response="18";
			}				
		}
      
		public virtual void UpdateData(string ReponseBitMap, Make mk)
		{
			UpdateData upData = new UpdateData();
			upData.UpdateDataForResponse(mk.MakeBitMapToBinary(ReponseBitMap), mk);
		}

		public string PerformResponse()
		{					
			string binaryBITMAP = MakeBitMapToBinary(BITMAP);					
			StringBuilder responseBuilder = new StringBuilder() ;		
			string fieldValue = "";
            int counter = 0;
			try
			{
				foreach( Field field in cashConfigResponse.Fields  )
				{
                    counter++;
					if( field.Id == "0" )
					{						
						fieldValue = getFieldValue(field.Id);

                        if (fieldValue == Constants.FINANCIAL_MTI1)
                        {
                            responseBuilder.Append(Constants.FINANCIAL_MTI1_RESPONSE);
                        }
                        else if (fieldValue == "0201")
                        {
                            responseBuilder.Append(Constants.FINANCIAL_MTI1_RESPONSE);
                        }
                        else if (fieldValue == Constants.REVERSAL_MTI1 || fieldValue == Constants.REVERSAL_MTI2)
                        {
                            responseBuilder.Append(Constants.REVERSAL_MTI_RESPONSE);
                        }
                        else if (fieldValue == "0220")
                        {
                            responseBuilder.Append("0230");
                        }
                        else if (fieldValue == Constants.FINANCIAL_MTI2)
                        {
                            responseBuilder.Append(Constants.FINANCIAL_MTI2_RESPONSE);
                        }
                        else if (fieldValue == Constants.NETWORK_MESSAGE)
                        {
                            responseBuilder.Append(Constants.NETWORK_MESSAGE_RESPONSE);
                        }
					}
					else if ( field.Id == "1" )
					{
						responseBuilder.Append(BITMAP);						
					}					
					else
					{
						if( field.Sub != "-1" )
						{
							//129 is a dummy field ID so its bitmap wont have any value
							if( field.Id != "129" )
							{		
								if( binaryBITMAP [ Convert.ToInt16(field.Id) - 1 ] == '1' ) 
								{														
									int length ;
									string startPad = "" ;
									
									if(field.Format.Equals("LLVAR"))
									{									
										fieldValue = getFieldValue(field.Id);
										length = fieldValue.Length;                                        
                                        startPad = length.ToString().PadLeft(2, '0');
									}

									if(field.Format.Equals("LLLVAR"))
									{									
										fieldValue = getFieldValue(field.Id);                                       
                                        length = fieldValue.Length;
                                        startPad = length.ToString().PadLeft(3, '0');
                                        
									}							
									
									fieldValue = getFieldValue(field.Id);									
									Console.WriteLine("RESPONSE: Field Id: " + field.Id + " - Value: " + fieldValue);									
									responseBuilder.Append(startPad+fieldValue);
								}
							}
						}
					}
				}													
			}
			catch(Exception x)
			{
				Console.WriteLine("ERROR: DEAL with 210 :" + x.ToString());
                object obj = fieldValue;
				this.p_39Response="18";
            }
            Console.WriteLine("RESPONSE: BITMAP - " + binaryBITMAP);
            Console.WriteLine("RESPONSE: DATA - " + responseBuilder.ToString());

            return responseBuilder.ToString();
		}

		//This fn is done by Sajib
		//It creates response bitmap for given additional fields
		public void ResponseBitmap(int[] additions)
		{				
			int length=requestBITMAP.Length;
			char[] temp1=new char[length];
			temp1=requestBITMAP.ToCharArray();	
			int number=additions.Length;
			for(int i=0;i<number;i++)
			{				
				int field=additions[i];				
				temp1[field-1]='1';				
			}
			//
			string temp2=new string(temp1);
			responseBITMAP=binaryToHexa(temp2);

		}
		//This fn is done by Sajib
		//It converts binary number to hexa
		public string binaryToHexa(string binary)
		{
			int length=binary.Length;
			string hexa="";
			for(int i=0;i<=length-4;i+=4)
			{
				string segment=binary.Substring(i,4);
				int segValue=0;
				for(int j=0;j<4;j++)
				{
					if(j==0)segValue+=int.Parse(segment[j].ToString())*8;
					else if(j==1)segValue+=int.Parse(segment[j].ToString())*4;
					else if(j==2)segValue+=int.Parse(segment[j].ToString())*2;
					else segValue+=int.Parse(segment[j].ToString())*1;						
				}
				
				if(segValue<10)hexa+=segValue.ToString();
				else
				{
					if(segValue==10)hexa+="A";
					else if(segValue==11)hexa+="B";
					else if(segValue==12)hexa+="C";
					else if(segValue==13)hexa+="D";
					else if(segValue==14)hexa+="E";
					else if(segValue==15)hexa+="F";
				}
			}
			return hexa;
		}
		public bool checkBITMAP()
		{
			if(requestBITMAP.Equals("F638661128B0A00800000000044000A0")==false)
			{
				p_39Response="18";
				return false;
			}
			return true;
		}
		/*
		public string currentTime()
		{
			Console.WriteLine(System.DateTime.Now.ToString());
			return null;
		}*/
		#endregion

		#region Private methods

		void StoreValueInVariable(string id, string value)
		{
			switch(id)
			{
				case "0" :
					p_0MessageType = value;
					break;
				case "1" :
					p_1BitMapPrimary = value;
					break;
				case "2" :
					p_2PrimaryAccountNumber = value;
					break;

				case "3" :
					p_3ProcessingCode = value;
					break;
				case "3-1" :
					p_3_1TransactionCode = value;
					break;
				case "3-2" :
					p_3_2FromAccountType = value;
					break;
				case "3-3" :	
					p_3_3ToAccountType = value;
					break;
				case "4" :
					p_4AmountTransaction = value;
					break;
				case "5" :
					p_5AmountSettlement = value;
					break;
				case "6" :	
					p_6AmountCardholderBilling = value;
					break;
				case "7" :
					p_7TransmissionDateTime = value;
					break;
				case "9" :
					p_9ConversionRateSettlement = value;
					break;
				case "11" :
					p_11SystemsTraceAuditNumber = value;	
					break;
				case "12" :
					p_12TimeLocalTransaction = value;
					break;
				case "13" :
					p_13DateLocalTransaction = value;
					break;
				case "14" :
					p_14DateExpiration = value;
					break;
				case "15" :
					p_15DateSettlement = value;
					break;
				case "17" :
					p_17DateCapture = value;
					break;
				case "18" :
					p_18MerchantCategoryCode = value;
					break;
				case "19" :
					p_19AcquiringInstitutionCountryCode = value;
					break;
				case "22" :
					p_22PointOfServiceEntryMode = value;
					break;
				case "23" :
					p_23CardSequenceNumber = value;
					break;	
				case "25" :
					p_25PointOfServiceConditionCode = value;
					break;
				case "26" :
					p_26MessageReasonCode = value;
					break;	
				case "28" :
					p_28AmountTransactionFee = value;
					break;
				case "32" :
					p_32AcquiringInstitutionIdentificationCode = value;
					break;
				case "33" :
					p_33ForwardingInstitutionIdentificationCode = value;
					break;
				case "35" :
					p_35TrackIIData = value;
					break;
				case "37" :
					p_37RetrievalReferenceNumber = value;
					break;
				case "38" :
					p_38AuthorizationIdentificationResponse = value;
					break;
				case "39" :
					p_39Response = value;
					break;
				case "41" :
					p_41CardAcceptorTerminalIdentification = value;
					break;
				case "42" :
					p_42CardAcceptorIdentificationCode = value;
					break;
				case "43" :
					p_43CardAcceptorNameLocation = value;
					break;
				case "43-1" :
					p_43_1TerminalOwner = value;
					break;
				case "43-2" :
					p_43_2TerminalCity = value;
					break;
				case "43-3" :	
					p_43_3TerminalState = value;
					break;
				case "43-4" :	
					p_43_4TerminalCountry = value;
					break;
				case "43-5" :
					p_43_5TerminalAddress = value;
					break;
				case "43-6" :	
					p_43_6TerminalBranch = value;
					break;
				case "43-7" :
					p_43_7TerminalRegion = value;
					break;
				case "43-8" :
					p_43_8TerminalClass = value;
					break;
				case "43-9" :
					p_43_9TerminalDate = value;
					break;
				case "43-10" :
					p_43_10TerminalPSName = value;
					break;
				case "43-11" :
					p_43_11TerminalFinName = value;
					break;
				case "43-12" :
					p_43_12TerminalRetailerName = value;
					break;
				case "43-13" :	
					p_43_13TerminalCounty = value;
					break;
				case "43-14" :
					p_43_14TerminalZipCode = value;
					break;
				case "43-15" :
					p_43_15TerminalTimeOffset = value;
					break;
					
				case "44" :
					p_44ResultPINCVV = value;
					break;
				case "44-1" :
					p_44_1ResultPIN = value;
					break;
				case "44-2" :
					p_44_2ResultCVV = value;
					break;
				
				case "45" :	
					p_45TrackIData = value;
					break;

				case "48" :
					p_48ReferenceToOtherTransaction = value;
					break;
				case "48-1" :	
					p_48_1OtherTransactionsRRN = value;
					break;	
				case "48-2" :
					p_48_2OtherTransactionsPAN = value;
					break;
				case "49" :
					p_49CurrencyTransactionCode = value;
					break;
				case "50" :
					p_50CurrencyCodeSettlement = value;
					break;
				case "51" :
					p_51CurrencyCodeCardholderBilling = value;
					break;
				case "52" :
					p_52PersonalIdentificationNumber = value;
					break;
				case "54" :
					p_54AmountAdjustment = value;
					break;
				case "55" :
					p_55ICCSystemRelatedData = value;
					break;

				case "61" :
					p_61CardIssuerData = value;
					break;
				case "61-1" :
					p_61_1IssuingInstitution = value;
					break;
				case "61-2" :
					p_61_2IssuingPaymentSystem = value;
					break;

				case "62" :
					p_62ExternalTransactionAttributes = value;
					break;
				case "63" :
					p_63NewPin = value;
					break;
				case "64" :
					p_64MAC = value;
					break;
				case "70" :
					p_70NetworkManagementInformationCode = value;
					break;
				case "90" :
					p_90OriginalDataElement = value;
					break;
				case "95" :
					s_95ReplacementAmountData = value;
					break;
				case "95-1" :
					s_95_1ActualTxnAmount = value;
					break;
				case "95-2" :
					s_95_2ActualSettlementAmount = value;
					break;
				case "95-3" :
					s_95_3ActualTxnFeeSign = value;
					break;
				case "95-4" :
					s_95_4ActualTxnFeeAmount = value;
					break;
				case "95-5" :
					s_95_5ActualSettlementFeeSign = value;
					break;
				case "95-6" :
					s_95_6ActualSettlemtnFeeAmount = value;
					break;
				case "100" :
					s_100ReceivingInstitutionIdentificationCode = value;
					break;
				case "102" :
					s_102AccountIdentificationI = value;
					break;
				case "103" :
					s_103AccountIdentificationII = value;
					break;
				case "104" :
					s_104HostNetIdentification = value;
					break;

				case "105" :
					s_105BalanceData = value;
					break;
				case "105-1" :
					s_105_1LedgerBalance = value;
					break;
				case "105-2" :
					s_105_2AvailableBalance = value;
					break;	
				case "105-3" :	
					s_105_3BalanceCurrencyAccount = value;
					break;

				case "106" :
					s_106MultiCurrencyData = value;
					break;
				case "106-1" :
					s_106_1CurrencyAccountTo = value;
					break;
				case "106-2" :
					s_106_2CurrencyOrigin = value;
					break;
				case "106-3" :
					s_106_3AmountAccountTo = value;
					break;
				case "106-4" :
					s_106_4AmountOrigin = value;
					break;
				case "106-5" :
					s_106_5ExchangeRateAccount = value;
					break;
				case "106-6" :
					s_106_6ExchangeRateAccountTo = value;
					break;

				case "107" :
					s_107RRNSentToFinalInterchange = value;
					break;
				case "108" :
					s_108RegionalListingDataStringMessage = value;
					break;

				case "109" :
					s_109MultiAccountData = value;
					break;
				case "109-1" :
					s_109_1Len = value;
					break;
				case "109-2" :
					s_109_2FromOrTo = value;
					break;
				case "109-3" :
					s_109_3AccountNumber = value;
					break;
				case "109-4" :
					s_109_4AccountTitle = value;
					break;
				case "109-5" :
					s_109_5AccountCurrency = value;
					break;

				case "110" :
					s_110NumericMessage = value;
					break;
				case "111" :
					s_111PersonalNumber = value;
					break;
				case "114" :
					s_114Statement = value;
					break;

				case "121" :
					s_121AdditionalPOSData = value;
					break;
				case "121-1" :
					s_121_1POSTransactionCategory = value;
					break;
				case "121-2" :
					s_121_2DraftCapture = value;
					break;
				case "121-3" :
					s_121_3CVV2 = value;
					break;
				case "121-4" :
					s_121_4ClerkID = value;
					break;
				case "121-5" :
					s_121_5InvoiceNumber = value;
					break;
				case "121-6" :
					s_121_6POSBatchAndShiftData = value;
					break;
				case "120" :
					s_120ExtendedTransactionData = value;
					break;
				case "122" :
					s_122IIIDSecureData = value;
					break;
				case "122-1" :
					s_122_1CAVV = value;
					break;
				case "122-2" :
					s_122_2AuthenticationResultsCode = value;
					break;
				case "122-3" :
					s_122_3UnpredictableNumber = value;
					break;
				case "122-4" :
					s_122_4MerchantTranId = value;
					break;


				case "123" :
					s_123MiscTransactionAttribute = value;
					break;

				case "126" :
					s_126PreAuthorizationParameters = value;
					break;
				case "126-1" :
					s_126_1OriginalTransactionInvoiceNumber = value;
					break;
				case "126-2" :
					s_126_2OriginalSeqNumber = value;
					break;
				case "126-3" :
					s_126_3PreauthorizationHold = value;
					break;

				case "128" :
					s_128SecondaryMAC = value;
					break;
				default:
					break;
			}
		}

		string getCardAcceptorNameLocation()
		{
			string cANameLoc =    p_43_1TerminalOwner
								+ p_43_2TerminalCity 
								+ p_43_3TerminalState 
								+ p_43_4TerminalCountry 
								+ p_43_5TerminalAddress 
								+ p_43_6TerminalBranch 
								+ p_43_7TerminalRegion 
								+ p_43_8TerminalClass 
								+ p_43_9TerminalDate
								+ p_43_10TerminalPSName
								+ p_43_11TerminalFinName 
								+ p_43_12TerminalRetailerName
								+ p_43_13TerminalCounty 
								+ p_43_14TerminalZipCode 
								+ p_43_15TerminalTimeOffset ;
			return cANameLoc;

		}
		string getMultiCurrencyData() 
		{
			string currecyDate =  s_106_1CurrencyAccountTo 
								+ s_106_2CurrencyOrigin
				                + s_106_3AmountAccountTo 
								+ s_106_4AmountOrigin
								+ s_106_5ExchangeRateAccount
								+ s_106_6ExchangeRateAccountTo;
			return currecyDate ;
									
		}
		
		string getMultiAccountData()
		{
          string multiAccountData =   s_109_1Len 
									+ s_109_2FromOrTo 
									+ s_109_3AccountNumber
									+ s_109_4AccountTitle
									+ s_109_5AccountCurrency;

				return multiAccountData ;
		}

		string getAdditionalPOSData()
		{
			string additionalPOSData =    s_121_1POSTransactionCategory
										+ s_121_2DraftCapture
										+ s_121_3CVV2
										+ s_121_4ClerkID
										+ s_121_5InvoiceNumber
										+ s_121_6POSBatchAndShiftData;
			return additionalPOSData; 

		}

		#endregion


	}
}
