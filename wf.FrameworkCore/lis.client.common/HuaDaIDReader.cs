using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace dcl.client.common
{
    /// <summary>
    /// 华大读卡操作接口
    /// </summary>
    public class HuaDaIDReader
    {
        /// <summary>
        /// 打开电脑端口
        /// </summary>
        /// <param name="dev_Name">默认值：USB1</param>
        /// <returns>大于0为成功</returns>
        [DllImport("SSSE32.dll")]
        public static extern int ICC_Reader_Open(string dev_Name);

        /// <summary>
        /// 蜂鸣声
        /// </summary>
        /// <param name="ReaderHandle"></param>
        /// <param name="time">0x05</param>
        /// <returns></returns>
        [DllImport("SSSE32.dll")]
        public static extern int ICC_PosBeep(int ReaderHandle, byte time);

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="ReaderHandle"></param>
        /// <returns></returns>
        [DllImport("SSSE32.dll")]
        public static extern int ICC_Reader_Close(long ReaderHandle);

        /// <summary>
        /// Mifare card
        ///请求卡片
        /// </summary>
        /// <param name="ReaderHandle"></param>
        /// <returns></returns>
        [DllImport("SSSE32.dll")]
        public static extern int PICC_Reader_Request(int ReaderHandle);

        /// <summary>
        /// 防碰撞卡片
        /// </summary>
        /// <param name="ReaderHandle"></param>
        /// <param name="sb">输出参数（卡片 ID）</param>
        /// <returns></returns>
        [DllImport("SSSE32.dll")]
        public static extern int PICC_Reader_anticoll(int ReaderHandle, ref ulong sb);

        /// <summary>
        /// 下载认证密钥
        /// </summary>
        /// <param name="ReaderHandle"></param>
        /// <param name="Mode">默认值：96</param>
        /// <param name="SecNr">默认值：0</param>
        /// <param name="key">默认值：ACF9FF8DFE57</param>
        /// <returns></returns>
        [DllImport("SSSE32.dll", EntryPoint = "PICC_Reader_Authentication_PassHEX")]
        public static extern int PICC_Reader_Authentication_PassHEX(int ReaderHandle, int Mode, int SecNr, string key);

        /// <summary>
        /// 读卡
        /// </summary>
        /// <param name="ReaderHandle"></param>
        /// <param name="Addr">1-卡号;2-门诊号</param>
        /// <param name="DataHex">返回卡信息</param>
        /// <returns></returns>
        [DllImport("SSSE32.dll", EntryPoint = "PICC_Reader_ReadHEX")]
        public static extern int PICC_Reader_ReadHEX(int ReaderHandle, int Addr, StringBuilder DataHex);
    }
}
