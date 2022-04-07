using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Lib.LogManager;
using dcl.entity;

namespace dcl.common
{
    public class Outlink : IDisposable
    {
        public const string OutLinkPath = "OutLink.dll";
        public const string OutLinkPath2 = @"C:\Program Files\hope\OutLink.dll";
        int huser32 = 0;
        MsgBox mymsg = null;

        [DllImport("Kernel32")]
        public static extern int GetProcAddress(int handle, String funcname);
        [DllImport("Kernel32")]
        public static extern int LoadLibrary(String funcname);
        [DllImport("Kernel32")]
        public static extern int FreeLibrary(int handle);

        public Outlink()
        {
            Load();
        }

        public void Load()
        {
            huser32 = LoadLibrary(OutLinkPath);
        }

        private static Delegate GetAddress(int dllModule, string functionname, Type t)
        {
            int addr = GetProcAddress(dllModule, functionname);
            if (addr == 0)
                return null;
            else
                return Marshal.GetDelegateForFunctionPointer(new IntPtr(addr), t);
        }

        public delegate string MsgBox(string msg);

        public string GetLisExe(string input)
        {
            return GetLisExe(input, false);
        }

        public string GetLisExe(string input, bool alone)
        {
            string result = "";
            IntPtr ptrIn = Marshal.StringToHGlobalAnsi(input);

            if (alone)
            {
                IntPtr ptrRet = HISDownloadBarcodeDllForZY(ptrIn);
                result = Marshal.PtrToStringAnsi(ptrRet);
            }
            else
            {
                IntPtr ptrRet = HISDownloadBarcodeDll(ptrIn);
                result = Marshal.PtrToStringAnsi(ptrRet);
            }
            return result;
        }

        public string GetOutlinkInfo(string functionName, string input)
        {
            string result = "";
            try
            {
                mymsg = (MsgBox)GetAddress(huser32, functionName, typeof(MsgBox));//1354137
                result = mymsg(input);
            }
            catch (Exception ex)
            {
                Logger.LogException("Outlink调用_" + functionName, ex);
            }
            finally
            {
            }

            return result;

        }



        /// <summary>
        /// 门诊病人信息
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        [DllImport(OutLinkPath, EntryPoint = "GetClinPat", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string GetClinPat(string input);

        /// <summary>
        /// 住院病人信息
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        [DllImport(OutLinkPath, EntryPoint = "GetWardPat", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string GetWardPat(string input);


        /// <summary>
        /// 出院病人信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DllImport(OutLinkPath, EntryPoint = "GetPatOutData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string GetPatOutData(string input);

        /// <summary>
        /// 校验用户账号合法性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DllImport(OutLinkPath, EntryPoint = "VerifyStaff", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string VerifyStaff(string input);


        /// <summary>
        /// 下载条码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DllImport(OutLinkPath, EntryPoint = "LisExec", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr HISDownloadBarcodeDll(IntPtr input);


        /// <summary>
        /// 检验执行确认临嘱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DllImport(OutLinkPath, EntryPoint = "LisCfmTmpOrder", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr LisCfmTmpOrder(IntPtr input);

        /// <summary>
        /// 取消费用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DllImport(OutLinkPath, EntryPoint = "cdPatPrvExeCfm", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr cdPatPrvExeCfm(IntPtr input);

        /// <summary>
        /// 下载条码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DllImport(OutLinkPath, EntryPoint = "LisExec", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr HISDownloadBarcodeDllForZY(IntPtr input);


        public string HISDownloadBarcode(string input, bool isAlone)
        {
            return GetLisExe(input, isAlone);
        }


        public string CancelLisMzOrder(string TWCId, string Rpid, string SeqId, string CfmStfId)
        {
            string strPar = string.Format("OpType=2;TWCId={0};Rpid={1};SeqId={2};stfid={3};", TWCId, Rpid, SeqId, CfmStfId);
            IntPtr ptrIn = Marshal.StringToHGlobalAnsi(strPar);

            string result = "";
            IntPtr ptrRet = cdPatPrvExeCfm(ptrIn);
            result = Marshal.PtrToStringAnsi(ptrRet);
            return result;
        }


        public static string GenerateAuditInfo(AuditInfo auditInfo)
        {
            return string.Format("StfNo=\"{0}\";Psw=\"{1}\";", auditInfo.UserId, auditInfo.Password);
        }

        public static string GenerateMZInput(string patID)
        {
            return string.Format("Mzno={0};", patID);
        }

        public static string GenerateZYInput(string patID)
        {
            return string.Format("PatNo={0};PatNm=;sDT=;eDT=;", patID);
        }

        public static string GenerateDownloadInfoString(EntityInterfaceExtParameter downloadInfo)
        {
            if (downloadInfo == null)
                return "";

            string downloadType = "0";
            string patNo = "";

            if (downloadInfo.DownloadType == InterfaceType.ZYDownload)
            {
                downloadType = "1";
                patNo = downloadInfo.PatientID;
            }
            else if (downloadInfo.DownloadType == InterfaceType.MZDownload)
            {
                patNo = downloadInfo.PatientID;
            }

            return string.Format("DataSrcType={0};PatNo={1};MzNm={2};DepNum={3};InvAccID={4};SDt=\"{5}\";EDt=\"{6}\";",
                downloadType, patNo, downloadInfo.PatientName, downloadInfo.DeptID, string.IsNullOrEmpty(downloadInfo.InvoiceID) ? "0" : downloadInfo.InvoiceID, downloadInfo.StartTime.ToString(CommonValue.OutlinkDateTimeLongFormat), downloadInfo.EndTime.ToString(CommonValue.OutlinkDateTimeLongFormat));
        }

        #region IDisposable 成员

        public void Dispose()
        {
            GC.Collect();

        }

        #endregion
    }
}
