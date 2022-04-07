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

namespace dcl.client.tools
{
    public partial class FrmLisDataReUpLoad : FrmCommon
    {
        #region 
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

        public DateTime PatDate2
        {
            get
            {
                return (DateTime)this.dateStart2.EditValue;
            }
            set
            {
                dateStart2.EditValue = value;
            }
        }

        public DateTime PatDateEnd2
        {
            get
            {
                return (DateTime)this.dateend2.EditValue;
            }
            set
            {
                dateend2.EditValue = value;
            }
        }

        private String TbLog
        {
            get
            {
                return tbLog.Text;
            }
            set
            {
                tbLog.Text = TbLog + value + "\r\n";
            }
        }

        private String TbReuploadLog
        {
            get
            {
                return tbReuploadLog.Text;
            }
            set
            {
                tbReuploadLog.Text = TbReuploadLog + value + "\r\n";
            }
        }

        /// <summary>
        /// 报告重传定时器
        /// </summary>
        private System.Timers.Timer timer1 = new System.Timers.Timer();


        /// <summary>
        /// 费用补收定时器
        /// </summary>
        private System.Timers.Timer timer2 = new System.Timers.Timer();

        internal GridCheckMarksSelection Selection { get; set; }
        #endregion

        public FrmLisDataReUpLoad()
        {
            InitializeComponent();
            this.Load += FrmLisDataReUpLoad_Load;
        }

        private void FrmLisDataReUpLoad_Load(object sender, EventArgs e)
        {
            txtBarcode.Focus();

            PatDate = DateTime.Now.Date;
            PatDateEnd = DateTime.Now.Date;

            PatDate2 = DateTime.Now.Date;
            PatDateEnd2 = DateTime.Now.Date;

            gvChargeInfo.ExpandAllGroups();
            Selection = new GridCheckMarksSelection(gvChargeInfo);
            Selection.CheckMarkColumn.Width = 20;
            Selection.CheckMarkColumn.VisibleIndex = 0;

            tbLog.Text = "补收日志：\r\n";
            tbReuploadLog.Text = "重传日志：\r\n";
        }

        #region Page1 中间表接口重传
        /// <summary>
        /// 数据检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            gcLisData.DataSource = null;

            if (PatDate.Date > PatDateEnd.Date)
            {
                lis.client.control.MessageDialog.Show("检索起始日期不能大于结束日期！");
                return;
            }
            if ((PatDateEnd.Date - PatDate.Date).Days > 10)
            {
                lis.client.control.MessageDialog.Show("检索日期最长不能超过10天！");
                return;
            }

            string strBarcode = txtBarcode.Text.Trim();
            string strRepId = txtRepid.Text.Trim();
            string ItrID = selectDict_Instrmt1.valueMember;

            EntityPatientQC patientQc = new EntityPatientQC();
            patientQc.RepBarCode = strBarcode;
            patientQc.RepId = strRepId;
            patientQc.ListItrId = new List<string> { ItrID };
            patientQc.DateStart = DateTime.Parse(PatDate.ToString("yyyy-MM-dd 00:00:00"));
            patientQc.DateEnd = DateTime.Parse(PatDateEnd.ToString("yyyy-MM-dd 23:59:59"));

            try
            {
                ProxyPidReportMain proxy = new ProxyPidReportMain();
                List<EntityPidReportMain> listPatient = proxy.Service.GetFaultUpLoadReport(patientQc);
                if (listPatient.Count == 0)
                    lis.client.control.MessageDialog.Show("暂无数据！");
                gcLisData.DataSource = listPatient;
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show(ex.Message);
            }
        }


        #region 重传
        private void btnUpload_Click(object sender, EventArgs e)
        {
            List<EntityPidReportMain> listPatient = gcLisData.DataSource as List<EntityPidReportMain>;
            if (listPatient == null || listPatient.Count == 0)
            {
                lis.client.control.MessageDialog.Show("暂无可以重传的数据！");
                return;
            }
            try
            {
                List<string> Pids = new List<string>();
                foreach (EntityPidReportMain report in listPatient)
                {
                    if (report.RepStatus == 0)
                    {
                        continue;
                    }
                    Pids.Add(report.RepId);
                }
                if (Pids.Count == 0)
                    return;



                ProxyDCLInterfacesTool Po = new ProxyDCLInterfacesTool();
                EntityResponse value = Po.Service.ReUploadDCLReport(Pids);
                if (value.Scusess)
                {
                    lis.client.control.MessageDialog.Show("上传成功！");
                }
                else
                {
                    lis.client.control.MessageDialog.Show(string.Format("上传失败，消息：{0} \r\n", value.ErroMsg));
                }

            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show(ex.Message);
            }
        }


        /// <summary>
        /// 自动重传勾选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbAutoReupload_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoReupload.Checked)
            {
                timer1.Interval = string.IsNullOrEmpty(tbReuploadInterTime.Text) ? 3600000 : Convert.ToInt32(tbReuploadInterTime.Text) * 60000;
                timer1.Elapsed += Timer1_Elapsed; ;
                timer1.Start();
                TbReuploadLog = "自动重传开始:" + DateTime.Now;
                this.tbReuploadInterTime.Text = (timer1.Interval / 60000).ToString();
                this.tbReuploadInterTime.Enabled = false;
                Timer1_Elapsed(null, null);
            }
            else
            {
                timer1.Elapsed -= Timer1_Elapsed;
                timer1.Stop();
                TbReuploadLog = "自动重传结束:" + DateTime.Now + "\r\n";
                this.tbReuploadInterTime.Enabled = true;
            }
        }

        /// <summary>
        /// 报告重传自动触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            EntityPatientQC patientQc = new EntityPatientQC();
            string logText = "正在重传：" + DateTime.Now + "\r\n";

            //patientQc.RepBarCode = strBarcode;
            //patientQc.RepId = strRepId;
            //patientQc.ListItrId = new List<string> { ItrID };
            patientQc.DateStart = DateTime.Now.Date;
            patientQc.DateEnd = DateTime.Now.Date.AddDays(1);

            List<EntityPidReportMain> listPatient = new List<EntityPidReportMain>();
            try
            {
                ProxyPidReportMain proxy = new ProxyPidReportMain();
                listPatient = proxy.Service.GetFaultUpLoadReport(patientQc);

                if (listPatient == null || listPatient.Count == 0)
                {
                    logText += "当天无需重传报告";
                    return;
                }

                List<string> Pids = new List<string>();
                foreach (EntityPidReportMain report in listPatient)
                {
                    if (report.RepStatus == 0)
                    {
                        continue;
                    }
                    Pids.Add(report.RepId);
                }
                if (Pids.Count == 0)
                {
                    logText += "当天无需重传报告";
                    return;
                }

                ProxyDCLInterfacesTool Po = new ProxyDCLInterfacesTool();
                EntityResponse value = Po.Service.ReUploadDCLReport(Pids);
                if (value.Scusess)
                {
                    logText += "此次共重传" + Pids.Count + "份报告：报告单号为：\r\n" + string.Join(",", Pids.ToArray());
                }
                else
                {
                    logText += "上传失败：" + value.ErroMsg;
                }
            }
            catch (Exception ex)
            {
                logText += "上传出错：" + ex.Message;
            }
            finally
            {
                TbReuploadLog = logText + "\r\n下一次执行：" + DateTime.Now.AddMilliseconds(timer1.Interval);
            }
        }

        #endregion

        #endregion

        #region Page2 HIS收费补收
        private void btnChargeSearch_Click(object sender, EventArgs e)
        {
            gcChargeInfo.DataSource = null;

            if (PatDate2.Date > PatDateEnd2.Date)
            {
                lis.client.control.MessageDialog.Show("检索起始日期不能大于结束日期！");
                return;
            }
            if ((PatDateEnd2.Date - PatDate2.Date).Days > 10)
            {
                lis.client.control.MessageDialog.Show("检索日期最长不能超过10天！");
                return;
            }

            string strBarcode = txtBarcode2.Text.Trim();
            string DeptID = lueDepart.valueMember;

            EntitySampQC sampQuery = new EntitySampQC();
            sampQuery.ListSampBarId = new List<string> { strBarcode };
            sampQuery.StartDate = PatDate2.ToString("yyyy-MM-dd 00:00:00");
            sampQuery.EndDate = PatDateEnd2.ToString("yyyy-MM-dd 23:59:59");
            sampQuery.PidDeptCode = DeptID;
            try
            {
                ProxySampMain proxy = new ProxySampMain();
                List<EntitySampMain> listPatient = proxy.Service.GetFaultChargeSamp(sampQuery);
                if (listPatient.Count == 0)
                    lis.client.control.MessageDialog.Show("暂无数据！");
                gcChargeInfo.DataSource = listPatient;
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show(ex.Message);
            }

        }

        private void btnReCharge_Click(object sender, EventArgs e)
        {
            List<EntitySampMain> listPatient = gcChargeInfo.DataSource as List<EntitySampMain>;
            if (listPatient == null || listPatient.Count == 0)
            {
                lis.client.control.MessageDialog.Show("暂无可以重新收费的数据！");
                return;
            }
            listPatient = Selection.GetAllSelectSamp();
            if (listPatient.Count == 0)
            {
                lis.client.control.MessageDialog.Show("请先选择需要收费的条码！");
                return;
            }
            try
            {
                ProxySampDetail det = new ProxySampDetail();
                List<EntitySampMain> Pids = new List<EntitySampMain>();
                foreach (EntitySampMain report in listPatient)
                {
                    if (int.Parse(report.SampStatusId) < 5 || int.Parse(report.SampStatusId) == 9)
                    {
                        continue;
                    }
                    report.ListSampDetail = det.Service.GetSampDetail(report.SampBarId);
                    Pids.Add(report);
                }
                if (Pids.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("所选条码不满足收费条件（条码未签收或已回退），请检查！");
                    return;
                }

                ProxyDCLInterfacesTool Po = new ProxyDCLInterfacesTool();
                EntityResponse value = Po.Service.ReChargeBarcode(Pids);
                if (value.Scusess)
                {
                    lis.client.control.MessageDialog.Show("操作完成！");
                    TbLog = "此次共补收" + Pids.Count + "条记录：\r\n" + string.Join(",", Pids.Select(o => o.SampBarCode).ToArray());
                }
                else
                {
                    lis.client.control.MessageDialog.Show(string.Format("收费失败，消息：{0} \r\n", value.ErroMsg));
                    TbLog = "补收失败：" + value.ErroMsg;
                }
            }
            catch (Exception ex)
            {
                TbLog = "补收出错：" + ex;
            }
        }


        /// <summary>
        /// 勾选自动补收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckAutoRun_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAutoRun.Checked)
            {
                timer2.Interval = string.IsNullOrEmpty(interMin.Text) ? 3600000 : Convert.ToInt32(interMin.Text) * 60000;
                timer2.Elapsed += T_Elapsed;
                timer2.Start();
                TbLog = "自动补收开始:" + DateTime.Now;
                this.interMin.Text = (timer2.Interval / 60000).ToString();
                this.interMin.Enabled = false;
                T_Elapsed(null, null);
            }
            else
            {
                timer2.Elapsed -= T_Elapsed;
                timer2.Stop();
                TbLog = "自动补收结束:" + DateTime.Now + "\r\n";
                this.interMin.Enabled = true;
            }
        }

        /// <summary>
        /// 自动补收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            EntitySampQC sampQuery = new EntitySampQC();
            string logText = "正在补收：" + DateTime.Now + "\r\n";

            sampQuery.StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            sampQuery.EndDate = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");

            try
            {
                ProxySampMain proxy = new ProxySampMain();
                List<EntitySampMain> listPatient = proxy.Service.GetFaultChargeSamp(sampQuery);
                if (listPatient.Count == 0)
                {
                    logText += "无需补收记录";
                    return;
                }

                ProxySampDetail det = new ProxySampDetail();
                List<EntitySampMain> Pids = new List<EntitySampMain>();
                foreach (EntitySampMain report in listPatient)
                {
                    if (int.Parse(report.SampStatusId) < 5 || int.Parse(report.SampStatusId) == 9)
                    {
                        continue;
                    }
                    report.ListSampDetail = det.Service.GetSampDetail(report.SampBarId);
                    Pids.Add(report);
                }
                if (Pids.Count == 0)
                {
                    logText += "无需补收记录";
                    return;
                }

                ProxyDCLInterfacesTool Po = new ProxyDCLInterfacesTool();
                EntityResponse value = Po.Service.ReChargeBarcode(Pids);
                if (value.Scusess)
                {
                    logText += "此次共补收" + Pids.Count + "条记录，标本号为：\r\n" + string.Join(",", Pids.Select(o => o.SampBarCode).ToArray());
                }
                else
                {
                    logText += string.Format("收费失败，消息：{0} \r\n", value.ErroMsg);
                }
            }
            catch (Exception ex)
            {
                logText += "补收出错" + ex.Message;
            }
            finally
            {

                TbLog = logText + "\r\n下一次执行：" + DateTime.Now.AddMilliseconds(timer2.Interval);
            }

        }

        #endregion


    }
}
