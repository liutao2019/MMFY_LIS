using System;
using System.Data;
using Lib.LogManager;
using dcl.common;
using dcl.entity;

namespace dcl.svr.interfaces
{
    public class CardDataConvertInterface
    {
        public EntityOperationResult CardDataConvert(string cardData, PrintType printType)
        {
            EntityOperationResult result = new EntityOperationResult();
            string cardOutput = string.Empty;

            string interfaceKey = string.Empty;
            if (printType == PrintType.Inpatient)
            {
                interfaceKey = "住院卡号转换";
            }
            else if (printType == PrintType.TJ)
            {
                interfaceKey = "体检卡号转换";
            }
            else if (printType == PrintType.Outpatient)
            {
                interfaceKey = "门诊卡号转换";
            }
            if (!string.IsNullOrEmpty(interfaceKey))
            {
                Lib.DAC.SqlHelper helper = new Lib.DAC.SqlHelper();
                DataTable interfaceData = helper.GetTable(string.Format(@"select * from  dbo.dict_interfaces where in_name='{0}' ", interfaceKey));
                foreach (DataRow item in interfaceData.Rows)
                {
                    if (!item["in_db_address"].ToString().Contains(".") && item["in_db_name"].ToString().Length >= 12)
                    {
                        item["in_db_address"] = EncryptClass.Decrypt(item["in_db_address"].ToString());
                        item["in_db_name"] = EncryptClass.Decrypt(item["in_db_name"].ToString());
                        item["in_db_username"] = EncryptClass.Decrypt(item["in_db_username"].ToString());
                        item["in_db_password"] = EncryptClass.Decrypt(item["in_db_password"].ToString());
                    }
                }
                if (interfaceData.Rows.Count == 0)
                {
                    result.AddCustomMessage("CardDataConvert", "卡号转换", string.Format("没有设置名称为：{0} 的接口！", interfaceKey), EnumOperationErrorLevel.Error);
                }
                else
                {
                    try
                    {
                        result.OperationResultData = CardCovert(cardData, interfaceData);

                    }
                    catch (Exception ex)
                    {
                        string msg = cardData + " 转换失败！\r\n" + ex.Message;
                        Logger.LogInfo("CardDataConvertInterface：" + interfaceKey, msg);
                        result.AddCustomMessage("CardDataConvert", "卡号转换", msg, EnumOperationErrorLevel.Error);
                    }
                }
            }
            return result;
        }

        private string CardCovert(string cardData, DataTable interfaceData)
        {
            string result = string.Empty;
            DataRow newRow = interfaceData.Rows[0];
            HospitalInterface interfaces = new HospitalInterface(
               newRow[BarcodeTable.Interfaces.DBAddress].ToString(),
             newRow[BarcodeTable.Interfaces.DBName].ToString(),
             newRow[BarcodeTable.Interfaces.DBUsername].ToString(),
             newRow[BarcodeTable.Interfaces.DBPassword].ToString(),
             newRow[BarcodeTable.Interfaces.DBConnnectType].ToString(),
             newRow[BarcodeTable.Interfaces.InterfaceName].ToString());

            DataSet dataset = interfaces.Connecter.ExeInterface(cardData);
            if (dataset != null && dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
            {
                result = dataset.Tables[0].Rows[0][0].ToString();
            }
            return result;
        }
    }
}
