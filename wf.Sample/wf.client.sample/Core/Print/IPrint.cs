using System;
using System.Collections.Generic;
using dcl.common;
using dcl.root.logon;
using dcl.entity;


namespace dcl.client.sample
{
    public abstract class IPrintMachine
    {
        public PrintInfo PrintInfo { get; set; }
        public bool Print(string prtTemplate)
        {
            return Print("", prtTemplate);
        }

        public abstract bool Print(string machineName, string prtTemplate);
        public abstract bool Print(string machineName, string prtTemplate, string column, string allCombinesName);
        public abstract bool PrintByTJPace(string machineName, string prtTemplate);

        /// <summary>
        /// 附加参数值列表
        /// </summary>
        public Dictionary<string, string> ExtendParameters = new Dictionary<string, string>();

        public abstract bool PrintExReturnReport(List<List<string>> printexp ,string printMachineName, string reutrnTemplate, string barcodeDisplayNumber, string v);
    }

    public class PrintInfo
    {
        public IList<string> ID { get; set; }

        //public IList<EntitySampMain> lstRows { get; set; }

        public PrintInfo(IList<string> ids)
        {
            this.ID = ids;
        }

    }

    public class BCDownLoadInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public string NoID { get; set; }
        /// <summary>
        /// 体检号
        /// </summary>
        public string EmpID { get; set; }

        public BCDownLoadInfo(string id, string empID)
        {
            this.NoID = id;
            this.EmpID = empID;
        }
    }

    /// <summary>
    /// 打印条码接口
    /// </summary>
    public abstract class IPrint
    {
        /// <summary>
        /// 按钮的显示位置
        /// </summary>
        public virtual System.Windows.Forms.DockStyle ButtonDock { get { return System.Windows.Forms.DockStyle.Bottom; } }
        /// <summary>
        /// 是否需要打印
        /// </summary>
        public virtual bool NeedPrint { get { return true; } }
        /// <summary>
        /// 打印时的验证方式：HIS或LIS
        /// </summary>
        public virtual IAudit Audit { get { return new LisAudit(); } }
        /// <summary>
        /// 验证时的信息
        /// </summary>
        public SignInfo SignInfo { get; set; }
        /// <summary>
        /// 条码下载信息
        /// </summary>
        public BCDownLoadInfo BCDownLoadInfo { get; set; }

        /// <summary>
        /// 默认的开始结束时间
        /// </summary>
        /// <returns></returns>
        public virtual DateTimeRange GetDefaultAdviceTime()
        { return null; }
        /// <summary>
        /// 是否在打印时一起更新采集状态
        /// </summary>
        public abstract bool ShouldMergeCollect { get; }
        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init() { }
        /// <summary>
        /// 名称
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// 打印上下文
        /// </summary>
        public IPrintable Printablor { get; set; }

        /// <summary>
        /// 是否独立界面
        /// </summary>
        public bool IsAlone { get; set; }

        /// <summary>
        /// 下载类型
        /// </summary>
        public abstract LoadDataType LoadDataType { get; }


        public System.Data.DataSet DownloadHisOrder(EntityInterfaceExtParameter DownLoadInfo)
        {
            string input = Outlink.GenerateDownloadInfoString(DownLoadInfo);
            if (String.IsNullOrEmpty(input))
                return null;
            //获取HIS结果字符串
            Outlink outlink = new Outlink();
            string result = "";
            try
            {
                result = outlink.HISDownloadBarcode(input, false);
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("下载医嘱失败:" + ex.Message);
                Logger.WriteException("条码", "下载医嘱", ex.Message + "\r\n" + ex.StackTrace);
                return null;
            }
            finally
            {
                //上线初期记录
                Logger.WriteException("条码下载", "Outlink下载数据", string.Format("时间:{0}", DateTime.Now)
                    + "\r\n" + string.Format("输入:{0}", input) + "\r\n" + string.Format("输出:{0}", result) + "\r\n");
                outlink.Dispose();
            }

            ConvertHelper convertHelper = new ConvertHelper();

            //HIS项目转成DataTable
            System.Data.DataSet dsHISData = convertHelper.ConvertToDataSet(result, SplitType.MzInfo);
            return dsHISData;
        }

        /// <summary>
        /// 是否显示确认窗口
        /// </summary>
        /// <returns></returns>
        public virtual bool ShowSpecialComfirmWhenPrint()
        {
            return true;
        }

        public bool AutoPrintReturnBarcode = false;

    }

    /// <summary>
    /// 空打印类
    /// </summary>
    public class EmptyPrinter : IPrint
    {

        public override bool ShouldMergeCollect
        {
            get { return false; }
        }

        public override string Name
        {
            get { return ""; }
        }

        public override LoadDataType LoadDataType
        {
            get { return LoadDataType.Add; }
        }
    }

    /// <summary>
    /// 添加条码的方式
    /// </summary>
    public enum LoadDataType
    {
        Add,
        DownLoad
    }

    /// <summary>
    /// 条码打印工厂
    /// </summary>
    public class PrintFactory
    {
        internal static IPrint Create(PrintType printType)
        {
            switch (printType)
            {
                case PrintType.Inpatient:
                    return new Inpatient();
                case PrintType.Outpatient:
                    return new OutPaitent();
                case PrintType.Manual:
                    return new Manual();
                case PrintType.TJ:
                    return new TJPaitent();
                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// 条码打印的客户端(Context)接口
    /// </summary>
    public interface IPrintable
    {
        IPrint Printer { get; set; }
        PrintType PrintType { get; set; }
        //string GetInpatientSql();
        //string GetOutpatientSql();
        void OutpatientInit();
        void InpatientInit();
    }


}