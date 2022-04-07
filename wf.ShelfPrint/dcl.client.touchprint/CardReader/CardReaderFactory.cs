using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace wf.ShelfPrint
{
    public static class CardReaderFactory
    {
        //private static string CardMode = ConfigurationManager.AppSettings["CardReaderMode"];

        private static ICardReader CardReader { get; set; }

        public static ICardReader DCLCardReader
        {
            get
            {
                if (CardReader == null)
                {
                    string CardMode = ConfigurationManager.AppSettings["CardReaderMode"];
                    switch (CardMode)
                    {
                        case "CRT"://创智电动 广州12医院
                            CardReader = new CardReader_GZ12();
                            break;
                        case "MT3"://中山三院
                            CardReader = new CardReader_ZSSY();
                            break;
                        default:
                            CardReader = null;
                            break;
                    }
                }

                return CardReader;
            }
        }


    }
}
