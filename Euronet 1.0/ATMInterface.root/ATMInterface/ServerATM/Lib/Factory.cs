using System;
using Make;
using ConfigureQCash;
using Common;

namespace MakeFactory
{
    /// <summary>
    /// MakeFactory will choose from the processcode what type of Make object the invoker requires.
    /// </summary>
    public class Factory
    {

        #region Configuration objects

        private ConfigureQCash.CashConfig echoRequestCashConfig;
        private ConfigureQCash.CashConfig echoResponseCashConfig;


        private ConfigureQCash.CashConfig mini_statementRequestCashConfig;
        private ConfigureQCash.CashConfig mini_statementResponseCashConfig;

        private ConfigureQCash.CashConfig withdrawalRequestCashConfig;
        private ConfigureQCash.CashConfig withdrawalResponseCashConfig;

        private ConfigureQCash.CashConfig euroWithdrawalRequestCashConfig;
        private ConfigureQCash.CashConfig euroWithdrawalResponseCashConfig;

        private ConfigureQCash.CashConfig euroWithdrawalReversalRequestCashConfig;
        private ConfigureQCash.CashConfig euroWithdrawalReversalResponseCashConfig;

        private ConfigureQCash.CashConfig euroBalQryRequestCashConfig;
        private ConfigureQCash.CashConfig euroBalQryResponseCashConfig;

        private ConfigureQCash.CashConfig euroFundTransferRequestCashConfig;
        private ConfigureQCash.CashConfig euroFundTransferResponseCashConfig;

        private ConfigureQCash.CashConfig euroMiniStatementRequestCashConfig;
        private ConfigureQCash.CashConfig euroMiniStatementResponseCashConfig;

        private ConfigureQCash.CashConfig queryRequestCashConfig;
        private ConfigureQCash.CashConfig queryResponseCashConfig;

        private ConfigureQCash.CashConfig transferRequestCashConfig;
        private ConfigureQCash.CashConfig transferResponseCashConfig;

        #endregion

        #region file path for each of the configuration files.
        //file path for each of the configuration files.
        private const string ECHO_REQUEST_CASHCONFIG_FILEPATH = @"CashConfigEchoRequest.config";       //C:\ATMProject\ServerATM\

        private const string ECHO_RESPONSE_CASHCONFIG_FILEPATH = @"CashConfigEchoResponse.config";

        private const string MINI_STATEMENT_REQUEST_CASHCONFIG_FILEPATH = @"CashConfigMiniStatementRequest.config";

        private const string MINI_STATEMENT_RESPONSE_CASHCONFIG_FILEPATH = @"CashConfigMiniStatementResponse.config";

        private const string WITHDRAWAL_REQUEST_CASHCONFIG_FILEPATH = @"CashConfigWithdrawalRequest.config";

        private const string WITHDRAWAL_RESPONSE_CASHCONFIG_FILEPATH = @"CashConfigWithdrawalResponse.config";

        private const string EURO_WITHDRAWAL_REQUEST_CASHCONFIG_FILEPATH = @"EuroCashConfigWithdrawalRequest.config";

        private const string EURO_WITHDRAWAL_RESPONSE_CASHCONFIG_FILEPATH = @"EuroCashConfigWithdrawalResponse.config";

        private const string EURO_WITHDRAWAL_REQUEST_REVERSAL_CASHCONFIG_FILEPATH = @"EuroCashConfigWithdrawalReversalRequest.config";

        private const string EURO_WITHDRAWAL_RESPONSE_REVERSAL_CASHCONFIG_FILEPATH = @"EuroCashConfigWithdrawalReversalResponse.config";

        private const string EURO_BAL_QUERY_REQUEST_CASHCONFIG_FILEPATH = @"EuroCashConfigBalQryRequest.config";

        private const string EURO_BAL_QUERY_RESPONSE_CASHCONFIG_FILEPATH = @"EuroCashConfigBalQryResponse.config";

        private const string EURO_FUND_TRANSFER_REQUEST_CASHCONFIG_FILEPATH = @"EuroFundTransferRequest.config";

        private const string EURO_FUND_TRANSFER_RESPONSE_CASHCONFIG_FILEPATH = @"EuroFundTransferResponse.config";

        private const string EURO_MINI_STATEMENT_REQUEST_CASHCONFIG_FILEPATH = @"EuroMiniStatementRequest.config";

        private const string EURO_MINI_STATEMENT_RESPONSE_CASHCONFIG_FILEPATH = @"EuroMiniStatementResponse.config";


        private const string QUERY_REQUEST_CASHCONFIG_FILEPATH = @"CashConfigQueryRequest.config";
        private const string QUERY_RESPONSE_CASHCONFIG_FILEPATH = @"CashConfigQueryResponse.config";

        private const string TRANSFER_REQUEST_CASHCONFIG_FILEPATH = @"CashConfigTransferRequest.config";
        private const string TRANSFER_RESPONSE_CASHCONFIG_FILEPATH = @"CashConfigTransferlResponse.config";



        #endregion

        #region Construnctor

        public Factory()
        {

            //Load all the config files first and at once.
            //this is for mini_statement Request
            echoRequestCashConfig = new CashConfig();

            echoRequestCashConfig = CashConfig.Load(ECHO_REQUEST_CASHCONFIG_FILEPATH);

            //this is for mini_statement Request
            echoResponseCashConfig = new CashConfig();

            echoResponseCashConfig = CashConfig.Load(ECHO_RESPONSE_CASHCONFIG_FILEPATH);

            //this is for mini_statement Request
            mini_statementRequestCashConfig = new CashConfig();

            mini_statementRequestCashConfig = CashConfig.Load(MINI_STATEMENT_REQUEST_CASHCONFIG_FILEPATH);

            //this is for mini_statement Response
            mini_statementResponseCashConfig = new CashConfig();

            mini_statementResponseCashConfig = CashConfig.Load(MINI_STATEMENT_RESPONSE_CASHCONFIG_FILEPATH);

            //this is for withdrawal Request
            withdrawalRequestCashConfig = new CashConfig();

            withdrawalRequestCashConfig = CashConfig.Load(WITHDRAWAL_REQUEST_CASHCONFIG_FILEPATH);

            //this is for withdrawal Response
            withdrawalResponseCashConfig = new CashConfig();

            withdrawalResponseCashConfig = CashConfig.Load(WITHDRAWAL_RESPONSE_CASHCONFIG_FILEPATH);

            euroWithdrawalRequestCashConfig = new CashConfig();

            euroWithdrawalRequestCashConfig = CashConfig.Load(EURO_WITHDRAWAL_REQUEST_CASHCONFIG_FILEPATH);

            //this is for withdrawal Response
            euroWithdrawalResponseCashConfig = new CashConfig();

            euroWithdrawalResponseCashConfig = CashConfig.Load(EURO_WITHDRAWAL_RESPONSE_CASHCONFIG_FILEPATH);

            euroWithdrawalReversalRequestCashConfig = new CashConfig();

            euroWithdrawalReversalRequestCashConfig = CashConfig.Load(EURO_WITHDRAWAL_REQUEST_REVERSAL_CASHCONFIG_FILEPATH);

            //this is for withdrawal Response
            euroWithdrawalReversalResponseCashConfig = new CashConfig();

            euroWithdrawalReversalResponseCashConfig = CashConfig.Load(EURO_WITHDRAWAL_RESPONSE_REVERSAL_CASHCONFIG_FILEPATH);

            // for euro balance inquiry
            euroBalQryRequestCashConfig = new CashConfig();

            euroBalQryRequestCashConfig = CashConfig.Load(EURO_BAL_QUERY_REQUEST_CASHCONFIG_FILEPATH);

            euroBalQryResponseCashConfig = new CashConfig();

            euroBalQryResponseCashConfig = CashConfig.Load(EURO_BAL_QUERY_RESPONSE_CASHCONFIG_FILEPATH);

            // for euro fund transfer
            euroFundTransferRequestCashConfig = new CashConfig();
            euroFundTransferRequestCashConfig = CashConfig.Load(EURO_FUND_TRANSFER_REQUEST_CASHCONFIG_FILEPATH);

            euroFundTransferResponseCashConfig = new CashConfig();
            euroFundTransferResponseCashConfig = CashConfig.Load(EURO_FUND_TRANSFER_RESPONSE_CASHCONFIG_FILEPATH);

            // for euro Mini Statement 
            euroMiniStatementRequestCashConfig = new CashConfig();
            euroMiniStatementRequestCashConfig = CashConfig.Load(EURO_MINI_STATEMENT_REQUEST_CASHCONFIG_FILEPATH);

            euroMiniStatementResponseCashConfig = new CashConfig();
            euroMiniStatementResponseCashConfig = CashConfig.Load(EURO_MINI_STATEMENT_RESPONSE_CASHCONFIG_FILEPATH);

            //this is for transfer Request
            transferRequestCashConfig = new CashConfig();

            transferRequestCashConfig = CashConfig.Load(TRANSFER_REQUEST_CASHCONFIG_FILEPATH);



            //this is for transfer Response
            transferResponseCashConfig = new CashConfig();

            transferResponseCashConfig = CashConfig.Load(TRANSFER_RESPONSE_CASHCONFIG_FILEPATH);


            //this is for query Request
            queryRequestCashConfig = new CashConfig();

            queryRequestCashConfig = CashConfig.Load(QUERY_REQUEST_CASHCONFIG_FILEPATH);

            //this is for query Response
            queryResponseCashConfig = new CashConfig();

            queryResponseCashConfig = CashConfig.Load(QUERY_RESPONSE_CASHCONFIG_FILEPATH);

        }

        #endregion

        #region Public functions

        public Make.Make GetMakeInstance(string wholeData)
        {
            Make.Make makeObject = null;
            int acquirer = 1;//Issuer
            string MTI = wholeData.Substring(0, 4);//change by monon need to must check

            if (MTI.Equals(Constants.NETWORK_MESSAGE))
            {
                makeObject = new Make.MakeEchoResponse(echoRequestCashConfig, echoResponseCashConfig, wholeData);
            }
            else if (MTI.Equals(Constants.FINANCIAL_MTI1) || MTI.Equals(Constants.FINANCIAL_MTI2) || MTI.Equals(Constants.REVERSAL_MTI1) || MTI.Equals(Constants.REVERSAL_MTI2)) //Financial/Reversal Msg
            {               
                string processingCode = new Utility().GetProcessingCode(wholeData);
                string txnCode = processingCode.Substring(0, 1 + acquirer);

                string CW_ProcessCode = ""; //CW-CashWithdraw; BQ-Balance Query; MS-MiniStatement; FT-FundTransfer
                string BQ_ProcessCode = ""; 
                string MS_ProcessCode = "";   
                string FT_ProcessCode = "";

                if (acquirer == 1)
                {
                    CW_ProcessCode = "41"; // cash withdraw and first cash 01
                    BQ_ProcessCode = "31"; // balance Inquiry
                    MS_ProcessCode = "90"; // mini statement
                    FT_ProcessCode = "40"; // fund transfer
                }
                else
                {
                    CW_ProcessCode = "010";
                    BQ_ProcessCode = "030";
                    MS_ProcessCode = "071";
                }

                if (txnCode == CW_ProcessCode) // 010 - 00,01,11,31
                {
                    if (acquirer == 1) // this is for euronet message parsing...sajjad
                    {
                        if (MTI.Equals(Constants.FINANCIAL_MTI1))
                        {
                            makeObject = new Make.MakeWithdrawal(euroWithdrawalRequestCashConfig, euroWithdrawalResponseCashConfig, wholeData);
                        }
                        else
                        {
                            makeObject = new Make.MakeWithdrawalReversal(euroWithdrawalReversalRequestCashConfig, euroWithdrawalReversalResponseCashConfig, wholeData);
                        }
                    }
                    else // this else was designed for ITCL message handling.....sajjad
                    {
                        makeObject = new Make.MakeWithdrawal(withdrawalRequestCashConfig, withdrawalResponseCashConfig, wholeData);
                    }
                }

                else if (txnCode.Equals(BQ_ProcessCode)) // 030 - 00,01,11,31    
                {
                    makeObject = new Make.MakeQuery(euroBalQryRequestCashConfig, euroBalQryResponseCashConfig, wholeData);
                }
                else if (txnCode.Equals(FT_ProcessCode)) /// 040 - 00,01,11,31  
                {
                    makeObject = new Make.MakeTransfer(euroFundTransferRequestCashConfig, euroFundTransferResponseCashConfig, wholeData);
                }
                else if (txnCode.Equals(MS_ProcessCode)) /// 040 - 00,01,11,31                
                {
                    makeObject = new Make.MakeMiniStatement(euroMiniStatementRequestCashConfig, euroMiniStatementResponseCashConfig, wholeData);
                }
                else
                {
                    makeObject = new Make.MakeQuery(queryRequestCashConfig, queryResponseCashConfig, wholeData);
                }
            }

            return makeObject;
        }

        #endregion       
        
    }
}
