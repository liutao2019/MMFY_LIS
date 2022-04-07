using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.client.common;
using lis.client.control;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.result
{
    public partial class FrmPatientsExt : FrmCommon
    {
        public FrmPatientsExt()
        {
            InitializeComponent();

        }

        public string PatId { get; set; }

        private bool update = false;

        /// <summary>
        /// 是否微生物调用
        /// </summary>
        public bool IsBacterial = false;

        public bool selectMode = false;

        private void FrmPatientsExt_Load(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            EntityQcResultList resultList = new ProxyPatResult().Service.GetPatientCommonResult(PatId, false);
            EntityPidReportMain patients = resultList.patient;

            txtName.Text = patients.PidName.ToString();
            txtSex.Text = patients.PidSexExp;
            txtAge.Text = patients.PidAgeExp;
            txtBedNumber.Text = patients.PidBedNo;
            txtDep.Text = patients.PidDeptName;
            txtID.Text = patients.PidInNo;
            txtOriName.Text = patients.SrcName;
            txtTel.Text = patients.PidTel;
            dtDate.DateTime = DateTime.Now;

            //微生物
            if (IsBacterial)
            {
                ProxyObrMessageContent proxyMsg = new ProxyObrMessageContent();
                if (UserInfo.GetSysConfigValue("Lab_ReportCriticalValueInfo") == "是"
                    && (patients.RepStatus == 2 || patients.RepStatus == 4))
                {
                    label8.Visible = false;
                    gcItem.Visible = false;
                    groupBox1.Visible = true;
                    List<EntityDicObrMessageContent> dtPatMsg = proxyMsg.Service.GetBacPatientsMsg(PatId);
                    txtItem.Text = dtPatMsg[0].PidComName.ToString();
                    metResult.Text = dtPatMsg[0].ObrValueC.ToString();
                }
                else
                {
                    label8.Visible = false;
                    gcItem.Visible = false;
                }
            }
            else
            {
                #region 普通报告结果

                List<EntityObrResult> dvRes = resultList.listResulto;

                string resRefFlag = @"'6','16','24','32','40','48','56','256','384','512','640','768','896'";

                dvRes = dvRes.Where(i => resRefFlag.Contains(i.RefFlag)).ToList();

                foreach (EntityObrResult drRes in dvRes)
                {
                    drRes.ObrRemark = "超出危急值";
                }

                gcItem.DataSource = dvRes;

                #endregion
            }
            List<EntityDicPidReportMainExt> dtPatExt = new ProxyPidReportMainExt().Service.GetPatientExtDataByPatID(PatId);

            bool OverTime = false;

            if (dtPatExt != null && dtPatExt.Count > 0)
            {
                if (dtPatExt[0].MsgDate > Convert.ToDateTime("1900-01-01"))
                {
                    EntityDicPidReportMainExt drPatExt = dtPatExt[0];

                    txtContent.Text = drPatExt.MsgContent;
                    txtDocNum.Text = drPatExt.MsgDocNum;
                    txtDocName.Text = drPatExt.MsgDocName;
                    txtTel.Text = drPatExt.MsgDepTel;
                    dtDate.Text = drPatExt.MsgDate.ToString();
                    txtUserName.Text = drPatExt.MsgRegisterUserName;
                    OverTime = drPatExt.MsgDate.ToString().Trim() != string.Empty && dtDate.DateTime.AddMinutes(15) < DateTime.Now;
                }

                update = true;
            }

            if (selectMode || OverTime)
            {
                txtContent.Properties.ReadOnly = true;
                txtDocNum.Properties.ReadOnly = true;
                txtDocName.Properties.ReadOnly = true;
                txtTel.Properties.ReadOnly = true;
                btnConfirm.Enabled = false;
            }

            if (UserInfo.GetSysConfigValue("Lab_CriticalContentRelateDict") == "是")
            {
                List<EntityDicPubEvaluate> dtBscripe = CacheClient.GetCache<EntityDicPubEvaluate>();
                txtContent.Properties.Items.Clear();
                foreach (EntityDicPubEvaluate drBscripe in dtBscripe)
                {
                    if (drBscripe.EvaFlag == "15")
                        txtContent.Properties.Items.Add(drBscripe.EvaContent.ToString().Trim());
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (UserInfo.GetSysConfigValue("Lab_CriticalDocNameEnableNull") == "是"
                && txtDocName.Text.Trim() == string.Empty)
            {
                lis.client.control.MessageDialog.Show("临床记录者不能为空！");
                return;
            }
            if (txtContent.Text.Trim() == string.Empty && txtDocNum.Text.Trim() == string.Empty)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("临床工号不能为空！");
                return;
            }
            if (txtContent.Text.Trim() == string.Empty && txtDocName.Text.Trim() == string.Empty)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("临床姓名不能为空！");
                return;
            }
            if (txtContent.Text.Trim() == string.Empty && txtTel.Text.Trim() == string.Empty)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("临床电话不能为空");
                return;
            }

            FrmCheckPassword frmCheck = new FrmCheckPassword();
            DialogResult dig = frmCheck.ShowDialog();

            if (dig == DialogResult.OK)
            {
                EntityDicPidReportMainExt reportmMainExt = new EntityDicPidReportMainExt();

                reportmMainExt.RepId = PatId;
                reportmMainExt.MsgContent = txtContent.Text.Trim();
                reportmMainExt.MsgDocNum = txtDocNum.Text.Trim();
                reportmMainExt.MsgDocName = txtDocName.Text.Trim();
                reportmMainExt.MsgDepTel = txtTel.Text.Trim();
                reportmMainExt.MsgDate = ServerDateTime.GetServerDateTime();
                reportmMainExt.MsgRegisterLoginId = frmCheck.OperatorID;
                reportmMainExt.MsgRegisterUserName = frmCheck.OperatorName;

                ProxyPidReportMainExt proxy = new ProxyPidReportMainExt();

                bool strReturn = proxy.Service.UpdatePatientsExt(reportmMainExt,update);

                if (strReturn)
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("保存成功");
                    this.Close();
                }
                else
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("保存失败");
            }
        }

        private void txtDocNum_Leave(object sender, EventArgs e)
        {
            if (txtDocNum.Text.Trim() != string.Empty)
            {
                List<EntityDicDoctor> DoctorCache = CacheClient.GetCache<EntityDicDoctor>();

                DoctorCache = DoctorCache.Where(i => i.DoctorCode == txtDocNum.Text.Trim()).ToList();

                if (DoctorCache.Count > 0)
                {
                    txtDocName.Text = DoctorCache[0].DoctorName.ToString();
                    return;
                }
                List<EntitySysUser> UserCache = CacheClient.GetCache<EntitySysUser>();
                UserCache = UserCache.Where(i => i.UserLoginid == txtDocNum.Text.Trim()).ToList();

                if (UserCache.Count > 0)
                {
                    txtDocName.Text = UserCache[0].UserName.ToString();
                }
            }
        }
    }
}
