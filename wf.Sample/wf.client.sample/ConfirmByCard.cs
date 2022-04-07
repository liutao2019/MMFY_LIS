using System;
using System.Collections.Generic;
using System.Text;
using dcl.client.common;
using lis.client.control;


namespace dcl.client.sample
{
    /// <summary>
    /// 条码确认用户认证时读取卡数据
    /// </summary>
    public class ConfirmByCard
    {

        static ConfirmByCard()
        {

            CardDrive = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_ConfirmByCardDrive");
            Enable = !string.IsNullOrEmpty(CardDrive);
        }
        public ConfirmByCard()
        {
            FillPwdUseCardID = true;
        }
        /// <summary>
        /// 是否用卡ID当做密码
        /// </summary>
        public bool FillPwdUseCardID { get; set; }
        /// <summary>
        /// 卡驱动
        /// </summary>
        public static string CardDrive { get; private set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public static bool Enable { get; private set; }
        /// <summary>
        /// 卡数据
        /// </summary>
        public string CardData { get; private set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMsg { get; private set; }

        public bool ReadCard()
        {
            bool result = false;

            if (Enable)
            {

                try
                {
                    CardReader.ICardReader reader = CardReader.CardReaderFactory.CreateCardReader(CardDrive);
                    
                    result = reader.ReadCardData();
                    CardData = reader.CardData;
                    ErrMsg = reader.Msg;
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;

                }
                if (string.IsNullOrEmpty(CardData))
                {
                    MessageDialog.ShowAutoCloseDialog("无法读取卡数据！" + Environment.NewLine + ErrMsg,2m);

                }


            }

            return result;
        }

        public bool AutoReadCard()
        {
            bool result = false;

            if (Enable)
            {

                try
                {
                    CardReader.ICardReader reader = CardReader.CardReaderFactory.CreateCardReader(CardDrive);

                    result = reader.ReadCardData();
                    CardData = reader.CardData;
                    ErrMsg = reader.Msg;
                }
                catch (Exception ex)
                {
                    return result;
                   //Lib.LogManager.Logger.LogException(ex);
                }
                //if (string.IsNullOrEmpty(CardData))
                //{
                //    MessageDialog.ShowAutoCloseDialog("无法读取卡数据！" + Environment.NewLine + ErrMsg, 2m);

                //}


            }

            return result;
        }
    }
}
