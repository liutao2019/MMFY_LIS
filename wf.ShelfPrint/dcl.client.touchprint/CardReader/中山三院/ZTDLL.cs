using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace wf.ShelfPrint
{
    /// <summary>
    /// 中山三院农行机读卡
    /// </summary>
    public class ZTDLL
    {
        private const string ZT_DEVPath = "ZT_DEV.dll";
        private const string ZT_IS4215Path = "ZT_IS4215.dll";

        /// <summary>
        /// 读取所有卡型数据（IC卡等）
        /// </summary>
        [DllImport(ZT_DEVPath, EntryPoint = "ZT_DEV_ReadAll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int ZT_DEV_ReadAll(int nPort, bool bRunMagnetic, int MagneticRunTime, bool bRunM1, int SectorNo, int BlockNo, int KeyType, string Key, bool bRunConnectIC, bool bRunConnectlessIC, int iProtocol, ref int RetType, StringBuilder OutData);

        /// <summary>
        /// 解码 – 将BCD码转ASC码，并以字符串输出
        /// </summary>
        [DllImport(ZT_DEVPath, EntryPoint = "ZT_BcdToAsc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void ZT_BcdToAsc(string InData, StringBuilder OutData);

        #region 扫描仪接口

        /// <summary>
        /// 获取驱动版本号
        /// </summary>
        [DllImport(ZT_IS4215Path, EntryPoint = "ZT_SBC_OpenDevice", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int ZT_SBC_OpenDevice(string pstrCom);

        /// <summary>
        /// 设备复位
        /// </summary>
        [DllImport(ZT_IS4215Path, EntryPoint = "ZT_SBC_Reset", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int ZT_SBC_Reset();

        /// <summary>
        /// 开始扫描
        /// </summary>
        [DllImport(ZT_IS4215Path, EntryPoint = "ZT_SBC_BeginScan", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int ZT_SBC_BeginScan();

        /// <summary>
        /// 读取数据
        /// </summary>
        [DllImport(ZT_IS4215Path, EntryPoint = "ZT_SBC_Scan", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int ZT_SBC_Scan(StringBuilder OutData);

        /// <summary>
        /// 停止扫描
        /// </summary>
        [DllImport(ZT_IS4215Path, EntryPoint = "ZT_SBC_StopScan", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int ZT_SBC_StopScan();

        /// <summary>
        /// 关闭设备
        /// </summary>
        [DllImport(ZT_IS4215Path, EntryPoint = "ZT_SBC_CloseDevice", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int ZT_SBC_CloseDevice();

        #endregion
    }
}
