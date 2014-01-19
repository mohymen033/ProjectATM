using System;

namespace Make
{
	/// <summary>
	/// Summary description for UpdateData.
	/// </summary>
	public class UpdateData
	{
		
		#region region Constructor
		public UpdateData()
		{
			//
			// TODO: Add constructor logic here
            //
		}

		#endregion 

		public void UpdateDataForResponse(string ResponseBitMapBinary, Make mk)
		{
			//Make mk = new Make();

			for(int id = 1; id <= 128; id++)
			{
				if(ResponseBitMapBinary.Substring(id-1, 1).Equals("1"))
				{
					string sid = Convert.ToString(id);
					switch(sid)
					{
						case "1" :
							//p_1BitMapSecondary = value;
							break;
						case "2" :
							//p_2PrimaryAccountNumber = value;
							break;

						case "3" :
							//p_3ProcessingCode = value;
							break;
						case "3-1" :
							//p_3_1TransactionCode = value;
							break;
						case "3-2" :
							//p_3_2FromAccountType = value;
							break;
						case "3-3" :	
							//p_3_3ToAccountType = value;
							break;

						case "4" :
							//p_4AmountTransaction = value;
							break;
						case "6" :	
							//p_6AmountCardholderBilling = value;
							break;
						case "7" :
							//mk.currentTime();
							//p_7TransmissionDateTime = value;
							break;
						case "11" :
							//p_11SystemsTraceAuditNumber = value;	
							break;
						case "12" :
							//p_12TimeLocalTransaction = value;
							break;
						case "13" :
							//p_13DateLocalTransaction = value;
							break;
						case "18" :
							//p_18MerchantCategoryCode = value;
							break;
						case "19" :
							//p_19AcquiringInstitutionCountryCode = value;
							break;
						case "22" :
							//p_22PointOfServiceEntryMode = value;
							break;
						case "23" :
							//p_23CardSequenceNumber = value;
							break;	
						case "25" :
							//p_25PointOfServiceConditionCode = value;
							break;
						case "26" :
							//p_26MessageReasonCode = value;
							break;	
						case "28" :
							//p_28AmountTransactionFee = value;
							break;
						case "32" :
							//p_32AcquiringInstitutionIdentificationCode = value;
							break;
						case "33" :
							//p_33ForwardingInstitutionIdentificationCode = value;
							break;
						case "35" :
							//p_35TrackIIData = value;
							break;
						case "37" :
							//p_37RetrievalReferenceNumber = value;
							break;
						case "38" :
							//mk.p_38AuthorizationIdentificationResponse = "383838";
							break;
						case "39" :
							//mk.p_39Response = "00001";
							break;
						case "41" :
							//p_41CardAcceptorTerminalIdentification = value;
							break;

						case "43" :
							//p_43CardAcceptorNameLocation = value;
							break;
						case "43-1" :
							//p_43_1TerminalOwner = value;
							break;
						case "43-2" :
							//p_43_2TerminalCity = value;
							break;
						case "43-3" :	
							//p_43_3TerminalState = value;
							break;
						case "43-4" :	
							//p_43_4TerminalCountry = value;
							break;
						case "43-5" :
							//p_43_5TerminalAddress = value;
							break;
						case "43-6" :	
							//p_43_6TerminalBranch = value;
							break;
						case "43-7" :
							//p_43_7TerminalRegion = value;
							break;
						case "43-8" :
							//p_43_8TerminalClass = value;
							break;
						case "43-9" :
							//p_43_9TerminalDate = value;
							break;
						case "43-10" :
							//p_43_10TerminalPSName = value;
							break;
						case "43-11" :
							//p_43_11TerminalFinName = value;
							break;
						case "43-12" :
							//p_43_12TerminalRetailerName = value;
							break;
						case "43-13" :	
							//p_43_13TerminalCounty = value;
							break;
						case "43-14" :
							//p_43_14TerminalZipCode = value;
							break;
						case "43-15" :
							//p_43_15TerminalTimeOffset = value;
							break;
						case "44" :
							//p_44ResultPINCVV = value;
							break;
						case "44-1" :
							//p_44_1ResultPIN = value;
							break;
						case "44-2" :
							//p_44_2ResultCVV = value;
							break;
						case "45" :	
							//p_45TrackIData = value;
							break;
						case "48" :
							//p_48ReferenceToOtherTransaction = value;
							break;
						case "48-1" :	
							//p_48_1OtherTransactionsRRN = value;
							break;	
						case "48-2" :
							//p_48_2OtherTransactionsPAN = value;
							break;
						case "49" :
							//p_49CurrencyTransactionCode = value;
							break;
						case "51" :
							//p_51CurrencyCodeCardholderBilling = value;
							break;
						case "52" :
							//p_52PersonalIdentificationNumber = value;
							break;
						case "54" :
							//p_54AmountAdjustment = value;
							break;
						case "55" :
							//p_55ICCSystemRelatedData = value;
							break;
						case "61" :
							//p_61CardIssuerData = value;
							break;
						case "61-1" :
							//p_61_1IssuingInstitution = value;
							break;
						case "61-2" :
							//p_61_2IssuingPaymentSystem = value;
							break;
						case "62" :
							//p_62ExternalTransactionAttributes = value;
							break;
						case "63" :
							//p_63NewPin = value;
							break;
						case "64" :
							//p_64MAC = value;
							break;
						case "70" :
							//p_70NetworkManagementInformationCode = value;
							break;
						case "95" :
							//s_95ReplacementAmountData = value;
							break;
						case "95-1" :
							//s_95_1ReplacementAmount = value;
							break;
						case "95-2" :
							//s_95_2ReplacementOriginalAmount = value;
							break;
						case "100" :
							//s_100ReceivingInstitutionIdentificationCode = value;
							break;
						case "102" :
							//s_102AccountIdentificationI = value;
							break;
						case "103" :
							//s_103AccountIdentificationII = value;
							break;
						case "104" :
							//s_104HostNetIdentification = value;
							break;
						case "105" :
							//mk.s_105BalanceData = "1051051051051051051051051";
							break;
						case "105-1" :
							//mk.s_105_1LedgerBalance = "105105105105";
							break;
						case "105-2" :
							//mk.s_105_2AvailableBalance = "105105105105";
							break;	
						case "105-3" :	
							//mk.s_105_3BalanceCurrencyAccount = "1";
							break;
						case "106" :
							//s_106MultiCurrencyData = value;
							break;
						case "106-1" :
							//s_106_1CurrencyAccountTo = value;
							break;
						case "106-2" :
							//s_106_2CurrencyOrigin = value;
							break;
						case "106-3" :
							//s_106_3AmountAccountTo = value;
							break;
						case "106-4" :
							//s_106_4AmountOrigin = value;
							break;
						case "106-5" :
							//s_106_5ExchangeRateAccount = value;
							break;
						case "106-6" :
							//s_106_6ExchangeRateAccountTo = value;
							break;

						case "107" :
							//s_107RRNSentToFinalInterchange = value;
							break;
						case "108" :
							//s_108RegionalListingDataStringMessage = value;
							break;
						case "109" :
							//s_109MultiAccountData = value;
							break;
						case "109-1" :
							//s_109_1Len = value;
							break;
						case "109-2" :
							//s_109_2FromOrTo = value;
							break;
						case "109-3" :
							//s_109_3AccountNumber = value;
							break;
						case "109-4" :
							//s_109_4AccountTitle = value;
							break;
						case "109-5" :
							//s_109_5AccountCurrency = value;
							break;

						case "110" :
							//s_110NumericMessage = value;
							break;
						case "111" :
							//s_111PersonalNumber = value;
							break;
						case "114" :
							//mk.s_114Statement = "1000113001000000000001+0113001000000000001+0113001000000000001+0113001000000000001+0113001000000000001+";
							break;
						case "121" :
							//s_121AdditionalPOSData = value;
							break;
						case "121-1" :
							//s_121_1POSTransactionCategory = value;
							break;
						case "121-2" :
							//s_121_2DraftCapture = value;
							break;
						case "121-3" :
							//s_121_3CVV2 = value;
							break;
						case "121-4" :
							//s_121_4ClerkID = value;
							break;
						case "121-5" :
							//s_121_5InvoiceNumber = value;
							break;
						case "121-6" :
							//s_121_6POSBatchAndShiftData = value;
							break;
						case "122" :
							//s_122IIIDSecureData = value;
							break;
						case "122-1" :
							//s_122_1CAVV = value;
							break;
						case "122-2" :
							//s_122_2AuthenticationResultsCode = value;
							break;
						case "122-3" :
							//s_122_3UnpredictableNumber = value;
							break;
						case "122-4" :
							//s_122_4MerchantTranId = value;
							break;
						case "123" :
							//s_123MiscTransactionAttribute = value;
							break;
						case "126" :
							//s_126PreAuthorizationParameters = value;
							break;
						case "126-1" :
							//s_126_1OriginalTransactionInvoiceNumber = value;
							break;
						case "126-2" :
							//s_126_2OriginalSeqNumber = value;
							break;
						case "126-3" :
							//s_126_3PreauthorizationHold = value;
							break;
						case "128" :
							//s_128SecondaryMAC = value;
							break;
						default:
							break;
					}

				}			
			}
		}



	}
}
