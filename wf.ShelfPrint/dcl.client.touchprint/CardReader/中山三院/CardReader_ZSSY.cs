using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace wf.ShelfPrint
{
    public class CardReader_ZSSY : ICardReader
    {
        ReadLoadClass readCardFactory = null;

        string strCardType = string.Empty;
        string strSectionAddr = string.Empty;
        string strBlockAddr = string.Empty;
        string strSecKey = string.Empty;

        public bool Open()
        {
            strCardType = ConfigurationManager.AppSettings["CardType"];
            strSectionAddr = ConfigurationManager.AppSettings["SectionAddr"];
            strBlockAddr = ConfigurationManager.AppSettings["BlockAddr"];
            strSecKey = ConfigurationManager.AppSettings["SecKey"];

            readCardFactory = new ReadLoadClass();
            readCardFactory.Init(0, 0);

            return true;
        }

        public string Reader()
        {
            string strCardNumber = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(strCardType) &&
                    !string.IsNullOrEmpty(strSectionAddr) &&
                    !string.IsNullOrEmpty(strBlockAddr) &&
                    !string.IsNullOrEmpty(strSecKey))
                {
                    int CardType = 2;
                    int.TryParse(strCardType, out CardType);

                    int SectionAddr = 1;
                    int.TryParse(strSectionAddr, out SectionAddr);

                    int BlockAddr = 0;
                    int.TryParse(strBlockAddr, out BlockAddr);

                    byte[] SecKey = Encoding.Default.GetBytes(strSecKey);

                    string data = string.Empty;
                    int result = -1;
                    for (int i = 0; i < 5; i++)
                    {
                        result = readCardFactory.ReadRFIDCard(CardType, SectionAddr, BlockAddr, SecKey, ref data);
                        if (result == 0)
                        {
                            if (data.Length > 20)
                            {
                                for (int j = 1; j <= 10; j++)
                                {
                                    int startIndex = j * 2 - 1;
                                    strCardNumber += data.Substring(startIndex, 1);
                                }
                            }

                            //strCardNumber = data;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("中山三院读卡器读取接口", ex);
            }

            return strCardNumber;
        }

        public bool Close()
        {
            readCardFactory.Close();
            return true;
        }
    }
}
