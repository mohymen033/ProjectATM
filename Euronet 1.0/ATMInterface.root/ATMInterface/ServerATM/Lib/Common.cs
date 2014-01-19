using System;
using System.Configuration;
using System.IO;
using System.Xml;

namespace Common
{
    public static class ConfigManager
    {
        private static string _CurrentAccountTypeCode;
        private static string _Key = string.Empty;
        private static string _OracleATMConnectionString = string.Empty;
        private static string _OracleAbabilConnectionString = string.Empty;
        private static AppSettingsReader _ConfigReader = new AppSettingsReader();
        private static int _PortNumber = int.MinValue;
        private static string _ErrorLogTypeValue = null;

        public static void InitializeConfigurationData()
        {
            lock (_Key)
            {
                _Key = "currentaccounttypecode";
            }          
            _CurrentAccountTypeCode = _ConfigReader.GetValue(_Key, typeof(string)) as string;
           
            _OracleATMConnectionString = (string)_ConfigReader.GetValue(Constants.ATM_DB_KEY, typeof(string));
           
            _OracleAbabilConnectionString = (string)_ConfigReader.GetValue(Constants.ABABIL_DB_KEY, typeof(string));

            //_PortNumber = Convert.ToInt32(_ConfigReader.GetValue(Constants.PORT_NUMBER, typeof(string)));
            _PortNumber = Convert.ToInt32("2020");//added by monon must need to change
            _ErrorLogTypeValue = _ConfigReader.GetValue(Constants.LOGTYPE_KEY, typeof(String)) as string;            
        }

        public static string CurrentAccountTypeCode
        {
            get
            {              
                return _CurrentAccountTypeCode;
            }
        }

        public static string OracleATMConnectionString
        {
            get
            {               
                return _OracleATMConnectionString;
            }
        }

        public static string OracleAbabilConnectionString
        {
            get
            {               
                return _OracleAbabilConnectionString;
            }
        }

        /// <summary>
        /// GetRead TCP Listener Port Number from Config File.
        /// </summary>
        /// <returns></returns>
        public static int PortNumber
        {
            get
            {                
                return _PortNumber;
            }
        }

        public static string ErrorLogFileXMLPath
        {
            get
            {
                return Environment.CurrentDirectory + "\\ErrorLog.xml";
            }
        }

        public static string ErrorLogFileTextPath
        {
            get
            {
                return Environment.CurrentDirectory + "\\ErrorLog.log";
            }
        }

        public static ErrorLogType ErrorLogType
        {
            get
            {                                
                ErrorLogType logType = String.Compare("xml", _ErrorLogTypeValue, true) == 0 ? ErrorLogType.Xml : ErrorLogType.Text; //0 match -1 not match                  
                return logType;
            }
        }

    }//end class

    public class Constants
    {
        public const string SQL_CBSONLINE_STATUS = "SELECT * FROM ATM_SERVER_STATUS WHERE ATMSERVERNAME='CBS' AND ATMSERVERSTATUS=1";
        public const string SQL_GLOBAL_TRANSACTION_NUMBER = "SELECT get_globaltxnno FROM dual";

        public const string CURRENT_ACCOUNT_TYPE = "20";
        public const string SAVINGS_ACCOUNT_TYPE = "10";

        public const string FINANCIAL_MTI1 = "0200";//Cash withdraw
        public const string FINANCIAL_MTI1_RESPONSE = "0210";
        public const string FINANCIAL_MTI2 = "0400";
        public const string FINANCIAL_MTI2_RESPONSE = "0410";
        
        public const string REVERSAL_MTI1 = "0420";//Reversal
        public const string REVERSAL_MTI2 = "0421";
        public const string REVERSAL_MTI_RESPONSE = "0430";

        public const string NETWORK_MESSAGE = "0800";//Login
        public const string NETWORK_MESSAGE_RESPONSE = "0810";

        public const string PORT_NUMBER = "2020";//portnumber change by monon
        public const string ATM_DB_KEY = "atmdb";
        public const string ABABIL_DB_KEY = "ababildb";
        public const int BRANCH_CODE_LENGTH = 4;

        public const string SUCCESS_RESPONSE = "00";

        public const string ERROR_FUNCTION_NOT_SUPPORTED = "40";
        public const string ERROR_SYSTEM_MULFUNCTION = "96";
        public const string ERROR_EXCEED_WITHDRAWAL_AMOUNT = "61";        
        public const string ERROR_BANK_NOT_SUPPORTED_BY_SWITCH = "31";
        public const string ERROR_INVALID_EXCEPTION = "12";
        public const string ERROR_NOT_SUFFICIENT_FUND = "51";
        public const string ERROR_DUPLICATE_TRANSMISSION = "94";
        public const string ERROR_NOCHECKING_ACCOUNT = "52";

        public const string LOGTYPE_KEY = "errorlogtype";
    }

    public class Utility
    {
        string[] hexToBinary = {"0000","0001","0010","0011",
								   "0100","0101","0110","0111",
								   "1000","1001","1010","1011",
								   "1100","1101","1110","1111"
							   };
        public Utility()
        {
        }

        public string GetProcessingCode(string wholeData)
        {
            int acquirer = 1;
            string temp = wholeData.Substring(1);
            wholeData = temp;
            int nLengthHeader = 4;
            int nLengthMessageID = 0;
            int nLengthPrimaryBitMap = 16;
            int lengthSecBitMap = 16;
            int lengthPAN = 0;

            string hexBitMap;
            hexBitMap = wholeData.Substring(nLengthHeader - 1, nLengthPrimaryBitMap);
            string binaryBitMap = this.MakeBitMapToBinary(hexBitMap);

            //set length to zero if Secondary Bitmap does not exist
            if (binaryBitMap[0].ToString().CompareTo("0") == 0)
            {
                lengthSecBitMap = 0;
            }

            //get length of PAN if exists            
            int startIndexPAN = nLengthHeader + nLengthMessageID + nLengthPrimaryBitMap + lengthSecBitMap - 1;

            if (binaryBitMap[1].ToString().CompareTo("1") == 0)
            {
                lengthPAN = 2 + Convert.ToInt32(wholeData.Substring(startIndexPAN, 2));
            }

            //string lengthPAN = wholeData.Substring(20, 2);            
            int indexProcessingCode = startIndexPAN + lengthPAN;
            string processingCode = wholeData.Substring(indexProcessingCode, 5 + acquirer /*length processing code*/);

            return processingCode;
        }

        private string MakeBitMapToBinary(string hexBitmap)
        {
            string binaryBitmap = "";

            for (int i = 0; i < hexBitmap.Length; i++)
            {
                int hex = Convert.ToInt16(hexBitmap.ToUpper()[i]);

                if (hex > 64)
                {
                    binaryBitmap += hexToBinary[hex - 55];
                }
                else
                {
                    binaryBitmap += hexToBinary[hex - 48];
                }
            }

            return binaryBitmap;
        }

        public static string CurrentYear
        {
            get
            {
                return DateTime.Now.Year.ToString();
            }
        }

        public void WriteErrorToLog(string errorMessage)
        {
            try
            {
                IErrorLogger logger = FactoryErrorLogger();
                logger.WriteLog(DateTime.Now, errorMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
            }
        }

        private IErrorLogger FactoryErrorLogger()
        {
            IErrorLogger log = ConfigManager.ErrorLogType == ErrorLogType.Xml ? (IErrorLogger)new XmlErrorLogger() : (IErrorLogger)new TextErrorLogger();
            return log;           
        }

    }//end class

    public enum ErrorLogType : byte
    {
        Xml,
        Text
    };

    public interface IErrorLogger
    {
        void WriteLog(DateTime errorDateTime, string errorDescription);
    }

    public class TextErrorLogger : IErrorLogger
    {            
        void IErrorLogger.WriteLog(DateTime errorDateTime, string errorDescription)
        {
            FileInfo fi = new FileInfo(ConfigManager.ErrorLogFileXMLPath);
            using (StreamWriter sw = fi.AppendText())
            {
                sw.WriteLine(errorDateTime.ToString() + "    " + errorDescription);
                sw.WriteLine(Environment.NewLine);
                sw.Flush();
                sw.Close();
            }   
        }        
    }

    public class XmlErrorLogger : IErrorLogger
    {        
        public void WriteLog(DateTime errorDateTime, string errorDescription)
        {
            FileInfo fi = new FileInfo(ConfigManager.ErrorLogFileXMLPath);
            System.Xml.XmlDataDocument doc = new XmlDataDocument();

            if (!fi.Exists)
            {
                XmlNode rootElement = doc.CreateNode(XmlNodeType.Element, "errors", "");
                doc.AppendChild(rootElement);
                doc.Save(ConfigManager.ErrorLogFileXMLPath);
            }

            doc = new XmlDataDocument();
            doc.Load(ConfigManager.ErrorLogFileXMLPath);

            XmlNode ndError = doc.CreateNode(XmlNodeType.Element, "error", string.Empty);

            XmlNode ndDate = doc.CreateNode(XmlNodeType.Element, "datetime", string.Empty);
            ndDate.InnerText = errorDateTime.ToString();

            XmlNode ndDesc = doc.CreateNode(XmlNodeType.Element, "description", string.Empty);
            ndDesc.InnerText = errorDescription;

            ndError.AppendChild(ndDate);
            ndError.AppendChild(ndDesc);

            doc.DocumentElement.AppendChild(ndError);

            doc.Save(ConfigManager.ErrorLogFileXMLPath);   
        }       
    }
    
}//end namespace
