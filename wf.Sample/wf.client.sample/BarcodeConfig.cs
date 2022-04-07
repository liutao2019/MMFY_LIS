using dcl.client.common;

namespace dcl.client.sample
{
    /// <summary>
    /// 条码客户端配置
    /// </summary>
    public class BarcodeConfig
    {
        internal static string GetPlace()
        {
            return ConfigHelper.GetSetting("PlaceName");
        }

        static BarcodeConfig()
        {
            BarCode_ReCheckCharge = ConfigHelper.GetSysConfigValueWithoutLogin("BarCode_ReCheckCharge") == "是";
        }

        /// <summary>
        /// 用户验证采用新模式
        /// </summary>
        public static bool? BarCode_ReCheckCharge = null;


    }
}
