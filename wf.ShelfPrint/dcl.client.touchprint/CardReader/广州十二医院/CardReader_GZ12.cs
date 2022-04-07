using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace wf.ShelfPrint
{
    public class CardReader_GZ12 : ICardReader
    {
        string DevPort = string.Empty;
        UInt32 Hdle = 0;

        public CardReader_GZ12()
        {
            DevPort = ConfigurationManager.AppSettings["DevPort"];
        }

        public bool Open()
        {
            Hdle = CRTdll.CommOpen(DevPort);
            if (Hdle != 0)
                return true;
            else
                return false;
        }



        public string Reader()
        {
            string strCardNumber = string.Empty;
            try
            {
                //string DevPort = ConfigurationManager.AppSettings["DevPort"];
                //打开串口
                //UInt32 Hdle = CRTdll.CommOpen(DevPort);
                if (Hdle != 0)
                {
                    byte[] _TrackData = new byte[500];
                    int _TrackDataLen = 0;
                    int result = -1;
                    //读卡
                    result = CRTdll.MC_ReadTrack(Hdle, 0x30, 0x37, ref _TrackDataLen, _TrackData);
                    if (result == 0)
                    {
                        int n;
                        int position1 = 0;
                        int position2 = 0;
                        int position3 = 0;
                        string Tra2Buf = "";
                        for (n = 0; n < _TrackDataLen; n++)
                        {
                            if (_TrackData[n] == 31)
                            {
                                position1 = n;
                                break;
                            }
                        }
                        for (n = position1 + 1; n < _TrackDataLen; n++)
                        {
                            if (_TrackData[n] == 31)
                            {
                                position2 = n;
                                break;
                            }
                        }
                        for (n = position2 + 1; n < _TrackDataLen; n++)
                        {
                            if (_TrackData[n] == 31)
                            {
                                position3 = n;
                                break;
                            }
                        }
                        if (_TrackData[position2 + 1] == 89)
                        {

                            for (n = position2 + 2; n < position3; n++)
                            {
                                Tra2Buf += (char)_TrackData[n];
                            }
                            strCardNumber = Tra2Buf;
                            if (!string.IsNullOrEmpty(Tra2Buf))
                            {
                                //退卡
                                int reset = CRTdll.CRT310_Reset(Hdle, 0x01);
                            }

                        }
                    }
                }
                //关闭串口
                //int close = CRTdll.CommClose(Hdle);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("广州12医院创智电动读卡器读取接口", ex);
            }

            return strCardNumber;
        }

        public bool Close()
        {
            if (CRTdll.CommClose(Hdle) != 0)
                return true;
            else
                return false;
        }
    }
}
