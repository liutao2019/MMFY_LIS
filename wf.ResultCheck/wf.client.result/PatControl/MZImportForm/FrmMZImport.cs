using dcl.client.common;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.result.PatControl
{
    public partial class FrmMZImport : FrmCommon
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime PatDate
        {
            get
            {
                return (DateTime)this.dtBegin.EditValue;
            }
            set
            {
                dtBegin.EditValue = value;
            }
        }

        public DateTime PatDateEnd
        {
            get
            {
                return (DateTime)this.dtEnd.EditValue;
            }
            set
            {
                dtEnd.EditValue = value;
            }
        }

        string ItrID = "";
        string ItrName = "";

        public FrmMZImport()
        {
            InitializeComponent();
            this.Load += FrmMZImport_Load;
        }

        public FrmMZImport(string _ItrID,string _ItrName):this()
        {
            ItrID = _ItrID;
            ItrName = _ItrName;
            txtInstrment.Text = ItrName;
            
        }

        private void FrmMZImport_Load(object sender, EventArgs e)
        {
            PatDate = DateTime.Now.Date;
            PatDateEnd = DateTime.Now.Date;
            gvMain.FocusedRowChanged += GvMain_FocusedRowChanged;
            txtPID.Focus();
        }

        private void GvMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (this.DesignMode)
                return;
            gcDetail.DataSource = null;
            EntityPidReportMain report = (EntityPidReportMain)gvMain.GetFocusedRow();
            if (report == null)
                return;
            gcDetail.DataSource = report.ListPidReportDetail;
        }

        private void txtPID_EditValueChanged(object sender, EventArgs e)
        {
            if (txtPID.Text != null && txtPID.Text.StartsWith("#"))
                txtPID.Text = txtPID.Text.Replace("#", "").Trim();
        }

        private void txtPID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != System.Windows.Forms.Keys.Enter)
            {
                return;
            }

            gcDetail.DataSource = null;
            gcMain.DataSource = null;

            string PatientID = txtPID.Text.Trim();
            if(string.IsNullOrEmpty(PatientID))
            {
                lis.client.control.MessageDialog.Show("请输入门诊病人ID！", "提示");
                return;
            }

            EntityInterfaceExtParameter Parameter = new EntityInterfaceExtParameter()
            {
                DownloadType = InterfaceType.MZDownload,
                StartTime = DateTime.Parse(PatDate.ToString("yyyy-MM-dd 00:00:00")),
                EndTime = DateTime.Parse(PatDateEnd.ToString("yyyy-MM-dd 23:59:59")),
                PatientID = PatientID,
                MzFiterSams = LocalSetting.Current.Setting.MzDefaultSam
            };

            //1.获取接口数据
            List<EntityPidReportMain> Reports = new ProxySampMain().Service.MZImportReport(Parameter);
            if(Reports.Count == 0)
            {
                lis.client.control.MessageDialog.Show("未找到病人信息，请检查门诊号及时间是否正确！", "提示");
                return;
            }

            //2.检查是否所有组合都可在此仪器录入
            string Tip;
            CanSaveToItr(Reports, out Tip);

            //3.对数据进行一些填充
            if(Reports.Count>0)
            {
                int RepSid = int.Parse(new ProxyPidReportMain().Service.GetItrSID_MaxPlusOne(DateTime.Now.Date, ItrID, true));
                int i = 0;
                foreach (EntityPidReportMain report in Reports)
                {
                    report.RepItrId = ItrID;
                    report.ItrName = ItrName;
                    report.RepSid = (RepSid + i).ToString();
                    report.RepId = ItrID + DateTime.Now.ToString("yyyyMMdd") + report.RepSid;
                    report.RepCheckUserId = UserInfo.loginID;
                    foreach (EntityPidReportDetail det in report.ListPidReportDetail)
                    {
                        det.RepId = report.RepId;
                    }
                    i += 1;
                }
            }

            if (!string.IsNullOrEmpty(Tip))
                lis.client.control.MessageDialog.Show(Tip,"提示");
            
            //3.赋值
            gcMain.DataSource = Reports;

        }

        /// <summary>
        /// 检查组合信息
        /// </summary>
        /// <param name="reports"></param>
        /// <param name="tip"></param>
        /// <returns></returns>
        private void CanSaveToItr(List<EntityPidReportMain> reports, out string tip)
        {
            tip = "";
            try
            {
                List<EntityDicItrCombine> cachedata = (new ProxyCacheData().Service.GetCacheData("EntityDicItrCombine")).GetResult() as List<EntityDicItrCombine>;
                for(int i=0;i<reports.Count;i++)
                {
                    EntityPidReportMain ri = reports[i];
                    string comname = "";
                    for (int y = 0; y < ri.ListPidReportDetail.Count; y++)
                    {
                        EntityPidReportDetail dy = ri.ListPidReportDetail[y];
                        var count = cachedata.FindAll(w => w.ComId == dy.ComId && w.ItrId == ItrID);
                        if(count.Count()==0)
                        {
                            tip += string.Format("该病人所开组合【{0}】不能在此仪器录入，已过滤！\n",dy.PatComName);
                            ri.ListPidReportDetail.Remove(dy);
                        }
                        else
                        {
                            comname += dy.PatComName + "+";
                        }
                    }
                    if(ri.ListPidReportDetail.Count==0)
                    {
                        reports.Remove(ri);
                    }
                    else
                    {
                        ri.PidComName = comname;
                    }
                }
            }
            catch(Exception ex)
            {
                Lib.LogManager.Logger.LogException("门诊导入根据仪器过滤组合信息失败",ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
          
            List<EntityPidReportMain> reports = gcMain.DataSource as List<EntityPidReportMain>;
            if (reports == null || reports.Count == 0)
            {
                lis.client.control.MessageDialog.Show("暂无数据可保存，请检查！");
                return;
            }
            try
            {
                string ErrorMsg;
                bool result = new ProxyPidReportMain().Service.InsertReports(reports,out ErrorMsg);
                if(!result)
                {
                    lis.client.control.MessageDialog.Show(ErrorMsg);
                    return;
                }
                lis.client.control.MessageDialog.Show("保存成功！");
                if (AutoClose.Checked)
                    this.DialogResult = DialogResult.OK;
            }
            catch(Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            gcDetail.DataSource = null;
            gcMain.DataSource = null;
            PatDate = DateTime.Now.Date;
            PatDateEnd = DateTime.Now.Date;
            txtPID.Text = "";
            txtPID.Focus();
        }

        

    }
}
