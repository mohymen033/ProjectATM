using System;
using System.Data;
using System.Data.OracleClient;
using com.mislbd.sunflower.utility.dao;
using Make;
using Common;
using System.Text;

//using com.mislbd.sunflower.deposit.transaction.dao;

namespace ATMTransaction
{
    /// <summary>
    /// Summary description for TransationToCoreBank.
    /// </summary>
    public class TransationToCoreBank : DataAccessBase
    {
        private string GetGlobalTransactionNo(IDbTransaction tran)
        {
            string transactionNumber = "888888";

            using (OracleDataReader rdr = base.GetOracleDataReader(tran, Constants.SQL_GLOBAL_TRANSACTION_NUMBER))
            {
                rdr.Read();
               
                if (rdr.HasRows)
                {
                    transactionNumber = (rdr[0].ToString().PadLeft(6, '0'));
                }
            }

            return transactionNumber;
           
        }

        private void CBSOnlineStatus()
        {
            
            if (base.CheckRecordExists(Constants.SQL_CBSONLINE_STATUS))
            {
                Console.WriteLine("RESULT: CBS is ONLINE. Transaction will be persist in Abaril database.");
                base.activeConString = base.cbsConString;
                base.cbsConnected = true;
            }
            else
            {
                Console.WriteLine("RESULT: CBS OFFLINE, Transaction will be persist in ATM database.");
                activeConString = intConString;
                cbsConnected = false;
            }
        }

        private void SetProcessCode(int acquirer)
        {
            if (acquirer == 1)
            {
                CW_ProcessCode = "01";
                BQ_ProcessCode = "31";
                MS_ProcessCode = "90";
                FT_ProcessCode = "40";
            }
            else
            {
                CW_ProcessCode = "010";
                BQ_ProcessCode = "030";
                MS_ProcessCode = "071";
            }
        }
        //here use Make
        public void CompleteATMTransaction(Make.Make make)
        {
            offlineException = 0;

            try
            {
                CBSOnlineStatus();
            }
            catch (Exception ex)
            {
                //it's used for Bank code is not supported
                make.p_39Response = Constants.ERROR_BANK_NOT_SUPPORTED_BY_SWITCH;
                Console.WriteLine("ERROR: Database Connection - " + ex.ToString());
                new Utility().WriteErrorToLog(ex.ToString());
                return;
            }
            // from make we get acquirer_ID;
            acquirer = make.acquirer_ID;

            SetProcessCode(acquirer);
            #region try
            try
            {
                OracleConnection con = base.GetOracleConnection(activeConString);
                con.Open();
                _Transaction = con.BeginTransaction();

                string txnDefinitionCode = "";
                int requestType = -1; //For request type(Debit/Reversal)

                //Credit(1)/Debit(2)?
                //here use MAKE 
                if (make.MTI.Substring(1, 1).Equals("2"))//Financial Request
                {
                    txnDefinitionCode = "2";
                    //make use
                    if (make.p_3_1TransactionCode.Equals(CW_ProcessCode))
                    {
                        if (!CheckHistory(make, 1))
                        {
                            requestType = 1;
                        }
                        else
                        {
                            //make.p_39Response = "38"; //Duplicate request
                            make.p_39Response = Constants.ERROR_DUPLICATE_TRANSMISSION;
                        }
                    }
                }
                else if (make.MTI.Substring(1, 1).Equals("4"))//Reversal Request
                {
                    txnDefinitionCode = "1";

                    if (make.p_3_1TransactionCode.Equals(CW_ProcessCode))
                    {   //make use as parameter         
                        if (CheckHistory(make, 0))//previous cash-withdrawal data found in archive.
                        {
                            requestType = 0;
                        }
                        else
                        {
                            make.p_39Response = Constants.ERROR_FUNCTION_NOT_SUPPORTED;
                        }
                    }
                }

                string primaryAccountNo = make.s_102AccountIdentificationI.Trim();
                string pan = primaryAccountNo.PadLeft(12, '0');
                string dbAccountCode = primaryAccountNo.TrimStart(new char[] { '0' });
                int branchCode = Convert.ToInt32(dbAccountCode.Substring(0, Constants.BRANCH_CODE_LENGTH));

                if (make.p_3_1TransactionCode.Equals(CW_ProcessCode))//Cash withdraw
                {
                    txnDefinitionCode += "01"; //201 - Cashwithdraw; 101-Reversal

                    string initiatorModule = "DEPOSIT";
                    string instruments = "V-";
                    string narration = "ATM_Narration";

                    DateTime insDate = DateTime.ParseExact(Utility.CurrentYear + make.p_7TransmissionDateTime.Substring(0, 4), "yyyyMMdd", null);

                    if (requestType != -1) //Valid Transaction
                    {
                        decimal txnAmount = Convert.ToDecimal(make.p_4AmountTransaction.Substring(0, make.p_4AmountTransaction.Length - 2));

                        // during partial txn the the DE-95 is available..
                        if (null != make.s_95_1ActualTxnAmount)
                        {
                            decimal settlementAmount = Convert.ToDecimal(make.s_95_1ActualTxnAmount.Substring(0, make.s_95_1ActualTxnAmount.Length - 2));
                            txnAmount -= settlementAmount;
                        }

                        make.p_38AuthorizationIdentificationResponse = this.GetGlobalTransactionNo(_Transaction).ToString();

                        CompleteDepositTransaction(_Transaction, "False", txnDefinitionCode, dbAccountCode
                            , instruments, insDate, narration, txnAmount, Convert.ToInt64(make.p_38AuthorizationIdentificationResponse),
                            initiatorModule, "1", branchCode, branchCode);

                        if (offlineException != 1)
                        {
                            make.p_54AmountAdjustment = GetBalanceInString(_Transaction, pan);
                        }

                        if (offlineException == 1)
                        {
                            make.p_39Response = Constants.ERROR_INVALID_EXCEPTION;
                        }
                        else if (offlineException == 2)
                        {
                            //make.p_39Response = "16";  //Balance limit Exceeded 
                            make.p_39Response = Constants.ERROR_EXCEED_WITHDRAWAL_AMOUNT;
                        }

                        //if (txnDefinitionCode.StartsWith("1"))
                        //{
                        //    Console.Write("REVERSAL: Complete.Credited to ");
                        //}
                        //else if (txnDefinitionCode.StartsWith("2"))
                        //{
                        //    Console.Write("REVERSAL: Debited from ");
                        //}
                    }
                    else
                    {
                        make.p_54AmountAdjustment = GetBalanceInString(_Transaction, pan);
                    }
                }
                else if (make.p_3_1TransactionCode.Equals("020")) //Credit Refund
                {
                    txnDefinitionCode = "101";
                }
                else if (make.p_3_1TransactionCode.Equals(FT_ProcessCode))//Fund Transfer
                {
                    string initiatorModule = "DEPOSIT";
                    string instruments = "V-";
                    string narration = "ATM_Narration";
                    txnDefinitionCode = "201"; //Account Debit
                    decimal txnAmount = Convert.ToDecimal(make.p_4AmountTransaction.Substring(0, make.p_4AmountTransaction.Length - 2));
                    DateTime insDate = DateTime.ParseExact(Utility.CurrentYear + make.p_7TransmissionDateTime.Substring(0, 4), "yyyyMMdd", null);
                    string processionCode = make.p_3ProcessingCode;
                    string strProcCode = processionCode.Substring(2, 4);

                    bool is_valid_txn = true;
                    string txnCodeFrom = make.s_102AccountIdentificationI.Trim().Substring(3, 3);
                    string txnCodeTo = make.s_103AccountIdentificationII.Trim().Substring(3, 3);
                    string txnCodeEuronet = txnCodeFrom.Equals("102") ? "20" : "10";
                    txnCodeEuronet += txnCodeTo.Equals("102") ? "20" : "10";

                    // check the account type to. 
                    //For temporary it is approved
                    //if (!txnCodeEuronet.Equals(strProcCode))
                    //{
                    //    is_valid_txn = false;
                    //}

                    //check the account duplicacy
                    if (pan.Equals(make.s_103AccountIdentificationII))
                    {
                        is_valid_txn = false;
                    }

                    string creditPan = make.s_103AccountIdentificationII.Trim();

                    if (is_valid_txn)
                    {
                        make.p_38AuthorizationIdentificationResponse = this.GetGlobalTransactionNo(_Transaction).ToString();
                        // Debit transction 
                        CompleteDepositTransaction(_Transaction, "False", txnDefinitionCode, dbAccountCode
                            , instruments, insDate, narration, txnAmount, Convert.ToInt64(make.p_38AuthorizationIdentificationResponse), initiatorModule
                            , "1", branchCode, branchCode);

                        make.p_54AmountAdjustment = GetBalanceInString(_Transaction, pan);

                        txnDefinitionCode = "101"; //reversal code

                        creditPan = make.s_103AccountIdentificationII.Trim();
                        string dbCreditAccountCode = creditPan.TrimStart(new char[] { '0' });
                        // Credit transction 
                        CompleteDepositTransaction(_Transaction, "False", txnDefinitionCode, dbCreditAccountCode
                            , instruments, insDate, narration, txnAmount, Convert.ToInt64(make.p_38AuthorizationIdentificationResponse), initiatorModule
                            , "1", branchCode, branchCode);
                    }
                    else
                    {
                        make.p_39Response = Constants.ERROR_NOCHECKING_ACCOUNT; //Invalid Account
                        make.p_54AmountAdjustment = GetBalanceInString(_Transaction, pan);
                        make.p_38AuthorizationIdentificationResponse = this.GetGlobalTransactionNo(_Transaction).ToString(); ;
                    }

                    string last54AccInfor = "";
                    string final54AccInfo = "";

                    if (offlineException != 1)
                    {
                        last54AccInfor = make.p_54AmountAdjustment;
                    }

                    final54AccInfo = GetBalanceInString(_Transaction, creditPan);
                    make.p_54AmountAdjustment = last54AccInfor + final54AccInfo;



                    if (offlineException == 1)
                    {
                        make.p_39Response = Constants.ERROR_NOCHECKING_ACCOUNT; //Invalid Account
                    }
                    else if (offlineException == 2)
                    {
                        //make.p_39Response = "16"; //Balance limit Exceeded 
                        make.p_39Response = Constants.ERROR_EXCEED_WITHDRAWAL_AMOUNT;
                    }
                    else
                    {
                        make.p_39Response = Constants.SUCCESS_RESPONSE;
                    }

                    //if (txnDefinitionCode.StartsWith("1"))
                    //{
                    //    Console.Write("Reversal Complete.Credited to ");
                    //}
                    //else if (txnDefinitionCode.StartsWith("2"))
                    //{
                    //    Console.Write("Debited from ");
                    //}

                }
                else if (make.p_3_1TransactionCode.Equals(BQ_ProcessCode)) //Balance Query
                {
                    txnDefinitionCode = "301";

                    make.p_38AuthorizationIdentificationResponse = GetGlobalTransactionNo(_Transaction).ToString();

                    make.p_54AmountAdjustment = GetBalanceInString(_Transaction, pan);
                }
                else if (make.p_3_1TransactionCode.Equals(MS_ProcessCode)) //Mini Statement
                {
                    txnDefinitionCode = "701";
                    make.p_38AuthorizationIdentificationResponse = GetGlobalTransactionNo(_Transaction).ToString();
                    make.p_54AmountAdjustment = GetBalanceInString(_Transaction, pan);
                    int totalTxn = 0;
                    string statement = doMiniStatementQuery(_Transaction, dbAccountCode, out totalTxn);
                    string totalData = totalTxn < 10 ? "0" + totalTxn : totalTxn.ToString();
                    string curr120Data = make.s_120ExtendedTransactionData;
                    string currBalance = GetCurrentBalance(_Transaction, dbAccountCode).ToString().PadLeft(12, '0');
                    string dataLength = statement.Length.ToString().PadLeft(3, '0');
                    string final120Data = curr120Data + "002003ATM005002" + totalData + "006" + dataLength + statement + "007013" + currBalance + "+008013" + currBalance + "+030003050";
                    make.s_120ExtendedTransactionData = final120Data;
                    make.p_39Response = Constants.SUCCESS_RESPONSE;
                }

                _Transaction.Commit();
            }            
            catch (Exception ex)
            {
                _Transaction.Rollback();
                make.p_39Response = ex.Message.ToUpper().IndexOf("SUFFICIENT BALANCE NOT AVAILABLE") == -1 ? Constants.ERROR_SYSTEM_MULFUNCTION : Constants.ERROR_NOT_SUFFICIENT_FUND;
                Console.WriteLine("ERROR: - " + ex.ToString());
                new Utility().WriteErrorToLog(ex.ToString());
            }
            #endregion try
        }

        public void CloseConnection(bool success)
        {
            if (_Transaction.Connection != null)
            {
                if (success)
                {
                    _Transaction.Commit();
                }
                else
                {
                    _Transaction.Rollback();
                }
            }
        }

        public string GetBalanceInString(IDbTransaction tran, string pan)
        {
            string[] str = GetBalanceQuery(tran, pan);
            string lengthString = "";
            string currentAccountTypeCode = ConfigManager.CurrentAccountTypeCode;

            string accountType = str[2].ToString().Equals(currentAccountTypeCode) ? Constants.CURRENT_ACCOUNT_TYPE : Constants.SAVINGS_ACCOUNT_TYPE;

            // bal type 01 is for actual bal and 02 is for available bal
            string balTypeActualBal = accountType + "01050C";
            string balTypeAvailableBal = accountType + "02050C";
            string resultStringActualBal = str[0];
            string resultStringAvailableBal = str[1];

            resultStringActualBal = resultStringActualBal.PadLeft(12, '0');
            resultStringAvailableBal = resultStringAvailableBal.PadLeft(12, '0');

            return lengthString + balTypeAvailableBal + resultStringAvailableBal + balTypeActualBal + resultStringActualBal;
        }

        public string Padding(string str, int pad)
        {
            string temp = "";
            for (int i = 0; i < pad; i++)
                temp += "0";
            temp += str;
            return temp;
        }

        public void CompleteDepositTransaction(IDbTransaction dbtr, string isContra, string trDefCode, string accCode, string instrumentNo, DateTime insIssueDate,
            string txnNarration, decimal txnAmount, long globalTxnno, string inititaModule, string strBatchNo, int initiatorBranch, int ownerBranch)
        {

            OracleConnection con = (OracleConnection)dbtr.Connection;
            OracleTransaction oraTxn = (OracleTransaction)dbtr;

            if (trDefCode.Equals("101") || trDefCode.Equals("201"))
            {
                if (cbsConnected == true)
                {
                    OracleCommand oCmd = new OracleCommand("Public_Transaction.ENTRY_COMMON_Transaction", con, oraTxn);
                    oCmd.CommandType = CommandType.StoredProcedure;

                    oCmd.Parameters.Add("PENTRYMODE", OracleType.Number).Value = 1;

                    OracleParameter parTracerNo = oCmd.Parameters.Add("PTRACERNO", OracleType.VarChar, 10);
                    parTracerNo.Direction = ParameterDirection.InputOutput;
                    parTracerNo.Value = OracleString.Null;

                    oCmd.Parameters.Add("POWNERBRID", OracleType.Number).Value = ownerBranch;

                    oCmd.Parameters.Add("PTRDEFCODE", OracleType.VarChar).Value = trDefCode;

                    oCmd.Parameters.Add("PACCCODE", OracleType.VarChar).Value = accCode;

                    oCmd.Parameters.Add("PINSTRUMENTNO", OracleType.VarChar).Value = instrumentNo;

                    oCmd.Parameters.Add("PINSTRUMENTISSUEDATE", OracleType.DateTime).Value = insIssueDate;

                    oCmd.Parameters.Add("PVALUEDATE", OracleType.DateTime).Value = OracleDateTime.Null;

                    oCmd.Parameters.Add("PNARRATION", OracleType.VarChar).Value = txnNarration;

                    oCmd.Parameters.Add("PTXNAMT", OracleType.Number).Value = txnAmount;

                    oCmd.Parameters.Add("PINITIATORMODULE", OracleType.VarChar).Value = inititaModule;

                    oCmd.Parameters.Add("PTRBATCHNO", OracleType.VarChar).Value = String.IsNullOrEmpty(strBatchNo) ? OracleString.Null : strBatchNo;

                    oCmd.Parameters.Add("PISORIGINATING", OracleType.VarChar).Value = OracleString.Null;

                    oCmd.Parameters.Add("PIBTABRANCH", OracleType.Number).Value = 0;

                    oCmd.Parameters.Add("PORIGINATINGDATE", OracleType.VarChar).Value = OracleString.Null;

                    oCmd.Parameters.Add("PIBTAADVICE", OracleType.VarChar).Value = OracleString.Null;

                    oCmd.Parameters.Add("PIBTATRCODE", OracleType.VarChar).Value = OracleString.Null;

                    oCmd.Parameters.Add("PSUNDRYDATE", OracleType.DateTime).Value = DateTime.Today;

                    oCmd.Parameters.Add("PSUNDRYNO", OracleType.VarChar).Value = OracleString.Null;

                    oCmd.Parameters.Add("PCURRCODE", OracleType.VarChar).Value = OracleString.Null;

                    oCmd.Parameters.Add("PSUNDRYID", OracleType.Number).Value = 0;

                    oCmd.Parameters.Add("PGLOBALTXNO", OracleType.Number).Value = globalTxnno;

                    oCmd.Parameters.Add("PTXNDEFID", OracleType.Number).Value = 0;

                    oCmd.Parameters.Add("PACCNAME", OracleType.VarChar).Value = OracleString.Null;

                    oCmd.Parameters.Add("PVERIFYUSER", OracleType.VarChar).Value = "ATM-SYSTEM";

                    oCmd.Parameters.Add("PISONLINE", OracleType.Number).Value = 0;

                    oCmd.Parameters.Add("POPERATIONID", OracleType.VarChar).Value = OracleString.Null;

                    oCmd.Parameters.Add("PMKAPPUSERID", OracleType.VarChar).Value = "ATM-SYSTEM";

                    oCmd.Parameters.Add("PMKOSUSERID", OracleType.VarChar).Value = OracleString.Null;

                    oCmd.Parameters.Add("PMKTERMINAL", OracleType.VarChar).Value = OracleString.Null;

                    int result = oCmd.ExecuteNonQuery();
                }
                else
                {
                    string query = "SELECT ATMACCID, ATMACCCURRENTBAL FROM ATM_ACCOUNT_INFO "
                        + " WHERE ATMACCCODE ='" + accCode + "' AND ATMACCBID = " + initiatorBranch;

                    OracleCommand oCmd = new OracleCommand(query, con, oraTxn);
                    OracleDataReader reader = oCmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows)
                    {
                        string accId = reader[0].ToString();
                        int balance = int.Parse(reader[1].ToString());

                        if (balance < txnAmount && trDefCode.Equals("201"))
                        {
                            Console.WriteLine("VALIDATION: Insufficient Balance");
                            offlineException = 2;
                        }
                        else
                        {
                            string update = "UPDATE ATM_ACCOUNT_INFO SET ATMACCCURRENTBAL = ATMACCCURRENTBAL";

                            if (trDefCode.Equals("201"))
                                update += "-";
                            else if (trDefCode.Equals("101"))
                                update += "+";

                            update += txnAmount.ToString()
                                + " WHERE ATMACCCODE='" + accCode + "'  AND ATMACCBID=" + initiatorBranch;

                            oCmd.CommandText = update;
                            oCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        Console.WriteLine("VALIDATION: Not a valid account");
                        offlineException = 1;
                    }
                }
            }
        }

        public void UpdateHistory(Make.Make make, int reqType)
        {
            int revPossbl = 0;
            long originalTrStan = Convert.ToInt64(make.p_11SystemsTraceAuditNumber); //Convert.ToInt64(make.p_90OriginalDataElement.Substring(4,6));

            if (make.p_39Response.Equals(Constants.SUCCESS_RESPONSE) && reqType == 1)
            {
                revPossbl = 1;
            }

            DateTime trnDateTime = System.DateTime.ParseExact(make.p_7TransmissionDateTime.Substring(4,10), "MMddHHmmss", null);
            int newAtmTrId = atmTrId + 1;
            string atmTrcode = "";
            string atmTrdefCode = "";

            if (reqType == 1)
            {
                atmTrcode = "200";
                atmTrdefCode = "201";
            }
            else
            {
                atmTrcode = "400";
                atmTrdefCode = "101";
            }

            string insert = "INSERT INTO ATM_TXN_ARCH VALUES"
                + " (" + newAtmTrId.ToString() + ",'"
                + make.p_11SystemsTraceAuditNumber + "','"
                + make.s_102AccountIdentificationI.Trim().PadLeft(12, '0') + "','"
                + make.p_4AmountTransaction.Substring(0, make.p_4AmountTransaction.Length - 2) + "','"
                + atmTrcode + "',"
                + atmTrdefCode + ",'"
                + make.p_41CardAcceptorTerminalIdentification + "',"
                + "TO_DATE('" + trnDateTime.Hour.ToString() + ":" + trnDateTime.Minute.ToString() + ":"
                + trnDateTime.Second.ToString() + " " + trnDateTime.Month.ToString() + "/"
                + trnDateTime.Day.ToString() + "/" + trnDateTime.Year.ToString() + "','hh24:mi:ss mm/dd/yyyy'),'"
                + "True','"
                + make.p_32AcquiringInstitutionIdentificationCode + "','"
                + make.p_38AuthorizationIdentificationResponse.ToString() + "','"
                + make.s_102AccountIdentificationI.Trim().PadLeft(12, '0') + "',"
                + revPossbl + ","
                + make.p_37RetrievalReferenceNumber.Trim() + ","
                + originalTrStan + ")";


            using (OracleConnection conHstry = new OracleConnection(intConString))
            {
                conHstry.Open();

                OracleTransaction tranHstry = conHstry.BeginTransaction();

                OracleCommand oCmd = new OracleCommand(insert, conHstry, tranHstry);

                oCmd.ExecuteNonQuery();

                if ((make.p_39Response.Equals(Constants.SUCCESS_RESPONSE) || make.p_39Response.Equals("32") || make.p_39Response.Equals("22")) && reqType == 0)
                {
                    long originalStan = 0;

                    if (null != make.p_90OriginalDataElement)
                    {
                        originalStan = Convert.ToInt64(make.p_90OriginalDataElement.Substring(4, 6));
                    }

                    string update = "UPDATE ATM_TXN_ARCH SET ATMREVERSED = 0 WHERE  ATMTRSTAN = '" + originalTrStan + "' AND ATMTRDEFID = 201";

                    oCmd.CommandText = update;
                    oCmd.ExecuteNonQuery();
                }

                if (cbsConnected == false && offlineException == 0)
                {
                    insert = "INSERT INTO ATM_TXN VALUES"
                        + " (" + newAtmTrId.ToString() + ",'"
                        + make.p_11SystemsTraceAuditNumber + "','"
                        + make.p_2PrimaryAccountNumber + "','"
                        + make.p_4AmountTransaction + "','"
                        + atmTrcode + "',"
                        + atmTrdefCode + ",'"
                        + make.p_41CardAcceptorTerminalIdentification + "',"
                        + "TO_DATE('" + trnDateTime.Hour.ToString() + ":" + trnDateTime.Minute.ToString() + ":"
                        + trnDateTime.Second.ToString() + " " + trnDateTime.Month.ToString() + "/"
                        + trnDateTime.Day.ToString() + "/" + trnDateTime.Year.ToString() + "','hh24:mi:ss mm/dd/yyyy'),'"
                        + "True','"
                        + make.p_32AcquiringInstitutionIdentificationCode + "','"
                        + "False','"
                        + make.p_38AuthorizationIdentificationResponse.ToString() + "','"
                        + make.s_102AccountIdentificationI + "')";

                    oCmd.CommandText = insert;

                    oCmd.ExecuteNonQuery();
                }

                tranHstry.Commit();
            }
        }


        public bool CheckHistory(Make.Make make, int reqType)
        {
            bool toReturn = false;
            long oroginalTrStan = 0;

            if (!String.IsNullOrEmpty(make.p_90OriginalDataElement))
            {
                oroginalTrStan = Convert.ToInt64(make.p_90OriginalDataElement.Substring(4, 6));
            }

            string query = "SELECT * FROM ATM_TXN_ARCH WHERE ATMTRSTAN=" + oroginalTrStan;

            string accountCode = make.s_102AccountIdentificationI.Trim();

            decimal txnAmount = Convert.ToDecimal(make.p_4AmountTransaction.Substring(0, make.p_4AmountTransaction.Length - 2));

            if (reqType == 0)
            {
                query += " AND ATMTRACCID='" + accountCode + "'"
                    + " AND ATMTRDEFID=201 AND ATMREVERSED=1"
                    + " and ATMRETREFNO =" + make.p_37RetrievalReferenceNumber;
            }

            try
            {
                OracleConnection con = new OracleConnection(intConString);
                con.Open();
                OracleCommand oCmd = new OracleCommand(query, con);
                OracleDataReader reader = oCmd.ExecuteReader();
                reader.Read();

                toReturn = reader.HasRows ? true : false;

                query = "SELECT MAX(ATMTRID) FROM ATM_TXN_ARCH";

                oCmd.CommandText = query;
                reader = oCmd.ExecuteReader();
                reader.Read();

                atmTrId = reader.HasRows ? int.Parse(reader[0].ToString()) : 0;
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Problem Occurred in CheckHistory - " + ex.ToString());
                new Utility().WriteErrorToLog(ex.ToString());
                throw ex;
            }
            return toReturn;
        }

        //private string GetBranchCode(string accountCode)
        //{
        //    string sql = "SELECT AccBID FROM Account WHERE AccCODE='" + accountCode + "'";

        //    string branchCode = base.GetFirstRowFirstColumnValue(ConfigManager.GetOracleAbabilConnectionString(), sql).ToString();

        //    return branchCode;
        //}		

        private decimal GetCurrentBalance(IDbTransaction dbtr, string accCode)
        {
            string query;

            if (cbsConnected == true && offlineException != 1)
            {
                query = "SELECT  round(ACCOUNT.ACCCURRENTBALANCE)||'00' from account WHERE "
                    + " ACCCODE = '" + accCode + "'";
            }
            else
            {
                query = "SELECT round(ATMACCCURRENTBAL)||'00' FROM ATM_ACCOUNT_INFO "
                    + " WHERE ATMACCCODE='" + accCode + "'";
            }

            decimal currentbalance = Convert.ToDecimal(base.GetFirstRowFirstColumnValue(dbtr, query));

            return currentbalance;
        }

        private string[] GetBalanceQuery(IDbTransaction dbtr, string accCode)
        {
            string[] ret = new string[3];
            string query = string.Empty;
            string strAccCode = accCode.TrimStart(new char[] { '0' });

            if (cbsConnected == true && offlineException != 1)
            {
                query = "SELECT  round(ACCOUNT.ACCCURRENTBALANCE), round(ACCOUNT.ACCLIENAMOUNT), "
                    + "round(ACCOUNT.ACCBLOCKAMOUNT), round(ACCOUNTTYPE.ATMINBALANCE),accatid FROM ACCOUNT, ACCOUNTTYPE WHERE "
                    + "ACCOUNTTYPE.ATID = (SELECT ACCATID FROM ACCOUNT WHERE ACCOUNT.ACCCODE ='" + strAccCode + "') "
                    + "AND ACCOUNT.ACCCODE = '" + strAccCode + "'";
            }
            else
            {
                query = "SELECT round(ATMACCCURRENTBAL),round(ATMACCMINBAL) FROM ATM_ACCOUNT_INFO "
                       + " WHERE ATMACCCODE='" + strAccCode + "'";
            }

            try
            {
                OracleConnection con = (OracleConnection)dbtr.Connection;

                using (OracleDataReader aReader = base.GetOracleDataReader(dbtr, query))
                {
                    aReader.Read();

                    if (aReader.HasRows)
                    {
                        double currentbalance;
                        double availableBalance;
                        string accountType = "";

                        if (cbsConnected == true)
                        {
                            currentbalance = double.Parse(aReader[0].ToString());
                            double acclienamount = double.Parse(aReader[1].ToString());
                            double accblockamount = double.Parse("0" + aReader[2].ToString());
                            double atminbalance = double.Parse("0" + aReader[3].ToString());
                            accountType = aReader[4].ToString();
                            availableBalance = currentbalance - (acclienamount + accblockamount + atminbalance);
                        }
                        else
                        {
                            availableBalance = double.Parse(aReader[0].ToString());
                            currentbalance = availableBalance + double.Parse(aReader[1].ToString());
                        }

                        decimal finalAvBalance = Convert.ToDecimal(availableBalance.ToString().PadRight(availableBalance.ToString().Length + 2, '0'));
                        availableBalance = double.Parse(finalAvBalance.ToString());

                        decimal finalCurrBalance = Convert.ToDecimal(currentbalance.ToString().PadRight(currentbalance.ToString().Length + 2, '0'));
                        currentbalance = double.Parse(finalCurrBalance.ToString());

                        ret[0] = Padding(currentbalance.ToString(), 12 - currentbalance.ToString().Length);

                        ret[1] = Padding(availableBalance.ToString(), 12 - availableBalance.ToString().Length);
                        ret[2] = accountType;
                    }
                    else
                    {
                        ret = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: occured in GetBalanceQuery :" + ex.ToString());
                new Utility().WriteErrorToLog(ex.ToString());
                throw ex;
            }

            return ret;
        }

        private string doMiniStatementQuery(IDbTransaction dbtr, string accCode, out int totalDataCount)
        {
            totalDataCount = 0;
            string query;

            if (cbsConnected == true)
            {
                query = "SELECT * FROM ("
                    + " SELECT ROWNUM,TO_CHAR(TR.TRDATE,'YYMMDD') AS TXNDATE, RPAD(txndef.TRDDESCRIPTION,20,' ') AS DESCRIPITON, LPAD(TR.TRAMOUNT,12,'0') AS TXNAMOUNT, CASE WHEN SUBSTR(TRTRDID,1,1)='1' THEN '+' ELSE '-' END AS Sign, LPAD(TRGLOBALTXNNO,7,'0') AS REFF "
                    + " FROM TRANSACTIONRECORD TR,ACCOUNT,transactiondefinition txndef  WHERE ACCID = TRREFACCID AND trtrdid = trdid and "
                    + " ACCCODE = '" + accCode + "' "
                    + " ORDER BY trid DESC) "
                    + " WHERE ROWNUM <=10 ";
            }
            else
            {
                query = "SELECT ATMTRDATE,ATMTRAMOUNT,ATMTXNCODE FROM ATM_ACC_STATEMENT "
                       + " WHERE ATMACCID = "
                       + "(SELECT ATMACCID FROM ATM_ACCOUNT_INFO WHERE ATMACCCODE='" + accCode + "')";
            }

            StringBuilder ret = new StringBuilder();

            using (OracleDataReader aReader = base.GetOracleDataReader(dbtr, query))
            {
                while (aReader.Read())
                {
                    string txnDate = aReader[1].ToString();
                    string txnDesc = aReader[2].ToString();
                    string txnAmount = aReader[3].ToString();
                    string txnSign = aReader[4].ToString();
                    string txnReff = aReader[5].ToString();
                    ret.Append(txnDate + txnDesc + txnAmount + txnSign + txnReff);
                    totalDataCount++;
                }
            }

            return ret.ToString();
        }

    }
}



