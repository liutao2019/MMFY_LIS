using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using dcl.client.frame;
using Lib.DataInterface;
using Lib.DataInterface.Implement;
using dcl.entity;
using dcl.client.wcf;
using dcl.client.cache;

namespace dcl.client.msgsend
{
    public partial class FrmAddDIYMsg : FrmCommon
    {
        /// <summary>
        /// 选中的病人信息
        /// </summary>
        private EntityDicObrMessageContent abDrData { get; set; }

        /// <summary>
        /// 是否选中行
        /// </summary>
        private bool IsCurrRow = false;

        /// <summary>
        /// his病人信息选择
        /// </summary>
        public EntityPidReportMain drhispatParam { get; set; }

        public FrmAddDIYMsg()
        {
            InitializeComponent();
        }

        public FrmAddDIYMsg(EntityDicObrMessageContent entityContent)
        {
            InitializeComponent();
            abDrData = entityContent;
            IsCurrRow = true;
        }

        private void FrmAddDIYMsg_Load(object sender, EventArgs e)
        {
            this.selectDicPubDocName.onAfterChange += SelectDicPubDocName_onAfterChange;

            try
            {
                datePatDate.EditValue = ServerDateTime.GetServerDateTime();
                //chkSendMsg.Visible = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("SendMsgTriggerType") == "内部确认危机值时";
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }

            if (IsCurrRow)
            {
                txtSelHisNo.Enabled = false;

                txtPatName.Text = abDrData.PidName;
                txtPatInNo.Text = abDrData.PidInNo;
                txtAge.Text = abDrData.PidAge;
                if (abDrData.PidSex == "1")
                {
                    cbSex.Text = "男";
                }
                else if (abDrData.PidSex == "2")
                {
                    cbSex.Text = "女";
                }
                else
                {
                    cbSex.Text = "未知";
                }
                txtBedNo.Text = abDrData.PidBedNo;
                if (!string.IsNullOrEmpty(abDrData.ObrUserId))
                {
                    selectDicPubDept1.SelectByID(abDrData.ObrUserId);
                }
                if (!string.IsNullOrEmpty(abDrData.PidSrcId))
                {
                    selectDicPubSource.SelectByID(abDrData.PidSrcId);
                }
                if (!string.IsNullOrEmpty(abDrData.ObrDoctorCode))
                {
                    selectDicPubDoctor.SelectByID(abDrData.ObrDoctorCode);
                }
                if (!string.IsNullOrEmpty(abDrData.PidTel))
                {
                    txtTel.Text = abDrData.PidTel;
                }

            }

            if (string.IsNullOrEmpty(selectDicPubDept.valueMember) && !string.IsNullOrEmpty(FrmDIYCritical.strLocaDepID))
            {
                selectDicPubDept.valueMember = FrmDIYCritical.strLocaDepID;
                selectDicPubDept.displayMember = FrmDIYCritical.strLocaDepName;
            }

        }


        private void FrmAddDIYMsg_Shown(object sender, EventArgs e)
        {

        }

        private void sbtExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sbtSave_Click(object sender, EventArgs e)
        {
            DataTable dtSave = new DataTable("dtSave");
            dtSave.Columns.Add("dep_code", typeof(string));
            dtSave.Columns.Add("dep_name", typeof(string));
            dtSave.Columns.Add("msg_sender_id", typeof(string));
            dtSave.Columns.Add("msg_sender_name", typeof(string));
            dtSave.Columns.Add("msg_ext3", typeof(string));
            dtSave.Columns.Add("pat_name", typeof(string));
            dtSave.Columns.Add("pat_sex", typeof(string));
            dtSave.Columns.Add("pat_age_str", typeof(string));
            dtSave.Columns.Add("pat_bed_no", typeof(string));
            dtSave.Columns.Add("pat_in_no", typeof(string));
            dtSave.Columns.Add("pat_ori_id", typeof(string));
            dtSave.Columns.Add("msg_doc_code", typeof(string));
            dtSave.Columns.Add("msg_doc_name", typeof(string));
            dtSave.Columns.Add("msg_send_depcode", typeof(string));
            dtSave.Columns.Add("msg_send_flag", typeof(string));
            dtSave.Columns.Add("msg_pat_tel", typeof(string));
            DataRow drSave = dtSave.NewRow();
            EntityDicObrMessageReceive entityReceive = new EntityDicObrMessageReceive();
            if (string.IsNullOrEmpty(txtPatName.Text.Trim()))
            {
                lis.client.control.MessageDialog.Show("请填写病人名称");
                txtPatName.Focus();
                return;
            }
            entityReceive.ObrMessageContent.PidName = txtPatName.Text.Trim();
            if (string.IsNullOrEmpty(txtPatInNo.Text.Trim()))
            {
                lis.client.control.MessageDialog.Show("请填写病人号");
                txtPatInNo.Focus();
                return;
            }
            entityReceive.ObrMessageContent.PidInNo = txtPatInNo.Text.Trim();
            if (string.IsNullOrEmpty(txtAge.Text.Trim()))
            {
                lis.client.control.MessageDialog.Show("请填写年龄");
                txtAge.Focus();
                return;
            }
            entityReceive.ObrMessageContent.PidAge = txtAge.Text.Trim();
            if (string.IsNullOrEmpty(cbSex.Text.Trim()))
            {
                lis.client.control.MessageDialog.Show("请填写性别");
                cbSex.Focus();
                return;
            }

            if (cbSex.Text.Trim() == "男")
            {
                entityReceive.ObrMessageContent.PidSex = "1";
            }
            else if (cbSex.Text.Trim() == "女")
            {
                entityReceive.ObrMessageContent.PidSex = "2";
            }

            if (string.IsNullOrEmpty(selectDicPubDept1.valueMember))
            {
                lis.client.control.MessageDialog.Show("请填写科室");
                selectDicPubDept1.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtBedNo.Text.Trim()))
            {
                entityReceive.ObrMessageContent.PidBedNo = txtBedNo.Text.Trim();
            }

            if (!string.IsNullOrEmpty(txtTel.Text.Trim()))
            {
                entityReceive.ObrMessageContent.PidTel = txtTel.Text.Trim();
            }
            entityReceive.ObrUserId = selectDicPubDept1.valueMember;
            entityReceive.ObrUserName = selectDicPubDept1.displayMember;
            if (string.IsNullOrEmpty(selectDicPubSource.valueMember))
            {
                lis.client.control.MessageDialog.Show("请填写来源");
                selectDicPubSource.Focus();
                return;
            }
            entityReceive.ObrMessageContent.PidSrcId = selectDicPubSource.valueMember;

            if (string.IsNullOrEmpty(selectDicPubDoctor.valueMember) && string.IsNullOrEmpty(selectDicPubDoctor.displayMember))
            {
                lis.client.control.MessageDialog.Show("请填写开单医生");
                selectDicPubDoctor.Focus();
                return;
            }
            entityReceive.ObrMessageContent.ObrDoctorCode = selectDicPubDoctor.valueMember;
            entityReceive.ObrMessageContent.ObrDoctorName = selectDicPubDoctor.displayMember;
            if (string.IsNullOrEmpty(selectDicPubDocName.displayMember))
            {
                lis.client.control.MessageDialog.Show("填写人名称不能为空");
                selectDicPubDocName.Focus();
                return;
            }
            entityReceive.ObrMessageContent.ObrSendUserName = selectDicPubDocName.displayMember;

            if (string.IsNullOrEmpty(txtsendID.Text))
            {
                lis.client.control.MessageDialog.Show("填写人工号不能为空");
                txtsendID.Focus();
                return;
            }
            entityReceive.ObrMessageContent.ObrSendUserId = txtsendID.Text.Trim();

            if (string.IsNullOrEmpty(selectDicPubDept.valueMember))
            {
                lis.client.control.MessageDialog.Show("填写人科室不能为空");
                selectDicPubDept.Focus();
                return;
            }
            entityReceive.ObrMessageContent.ObrSendDeptCode = selectDicPubDept.valueMember;

            if (string.IsNullOrEmpty(rtbMsgExt3.Text.Trim()))
            {
                lis.client.control.MessageDialog.Show("请填写危急信息");
                rtbMsgExt3.Focus();
                return;
            }
            entityReceive.ObrMessageContent.ObrValueC = rtbMsgExt3.Text.Trim();
            entityReceive.ObrMessageContent.ObrSendFlag = chkSendMsg.Checked ? "1" : "0";
            dtSave.Rows.Add(drSave);


            //添加自编危急值信息
            try
            {
                //int rv = new dcl.client.wcf.ProxyMessage().Service.AddDIYCriticalMsg(dtSave);
                //if (rv > 0)
                //{
                //    lis.client.control.MessageDialog.ShowAutoCloseDialog("新增成功");
                //    this.DialogResult = DialogResult.Yes;
                //    this.Close();
                //}
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                lis.client.control.MessageDialog.Show(ex.Message);
            }

        }

        private void SelectDicPubDocName_onAfterChange(EntityDicDoctor oldRow)
        {
            List<EntityDicDoctor> doc = CacheClient.GetCache<EntityDicDoctor>();
            if (doc != null && doc.Count > 0)
            {
                List<EntityDicDoctor> rows =
                 doc.FindAll(r => r.DoctorId == selectDicPubDocName.valueMember);

                if (rows.Count > 0)
                {
                    txtsendID.Text = rows[0].DoctorCode.ToString();
                }
            }
        }


        /// <summary>
        /// 自编危急值范围
        /// </summary>
        /// <param name="strRemarks"></param>
        public void setMSGRemarks(string strRemarks)
        {
            rtbMsgExt3.Text = strRemarks;
        }

        private void sbtRemarks_Click(object sender, EventArgs e)
        {
            FrmBscripe fb = new FrmBscripe(this, "10");
            fb.ShowDialog();
        }

        private void txtSelHisNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string selNo = txtSelHisNo.Text;

                if (selNo != null) { selNo = selNo.Trim(); }

                if (string.IsNullOrEmpty(selNo))
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("请填写住院号");
                    txtSelHisNo.Focus();
                    return;
                }

                //定义参数集合
                List<InterfaceDataBindingItem> list = new List<InterfaceDataBindingItem>();
                list.Add(new InterfaceDataBindingItem("selectNo", selNo));

                try
                {
                    //根据住院号获取病人基本信息接口cmdID
                    string cmd_id = System.Configuration.ConfigurationManager.AppSettings["GetZYPatData_cmdID"].ToString();

                    if (string.IsNullOrEmpty(cmd_id))
                    {
                        throw new Exception("没有在config中配置GetZYPatData_cmdID");
                    }

                    //DataTable dthispat = dcl.client.wcf.BarcodeClient.NewInstance.GetDtByDataInterfaceHelper(cmd_id, list.ToArray());

                    ProxyPidReportMain proxyPat = new ProxyPidReportMain();
                    EntityPatientQC patientQC = new EntityPatientQC();
                    EntitySortNo sortNo = new EntitySortNo();
                    sortNo.StartNo = Convert.ToInt32(selNo);
                    patientQC.ListSortRange.Add(sortNo);
                    List<EntityPidReportMain> patientsInfo = proxyPat.Service.PatientQuery(patientQC);

                    if (patientsInfo != null && patientsInfo.Count > 0)
                    {
                        drhispatParam = null;
                        EntityPidReportMain drhispat = null;

                        if (patientsInfo.Count == 1)
                        {
                            drhispat = patientsInfo[0];
                        }
                        else
                        {
                            FrmshowhispatView frmshispat = new FrmshowhispatView(this, patientsInfo);
                            frmshispat.ShowDialog();

                            if (drhispatParam != null)
                            {
                                drhispat = drhispatParam;
                            }
                            else
                            {
                                return;
                            }
                        }



                        #region 填充病人基本信息
                        txtPatName.Text = drhispat.PidName.ToString();
                        txtPatInNo.Text = drhispat.PidInNo.ToString();
                        txtAge.Text = drhispat.PidAgeExp.ToString();
                        if (drhispat.PidSex.ToString() == "1")
                        {
                            cbSex.Text = "男";
                        }
                        else if (drhispat.PidSex.ToString() == "2")
                        {
                            cbSex.Text = "女";
                        }
                        else
                        {
                            cbSex.Text = "未知";
                        }
                        txtBedNo.Text = drhispat.PidBedNo.ToString();
                        if (!string.IsNullOrEmpty(drhispat.PidDeptCode.ToString()))
                        {
                            selectDicPubDept1.SelectByID(drhispat.PidDeptCode.ToString());
                        }


                        if (!string.IsNullOrEmpty(drhispat.PidSrcId.ToString()))
                        {
                            selectDicPubSource.SelectByID(drhispat.PidSrcId.ToString());
                        }
                        else
                        {
                            selectDicPubSource.ClearSelect();
                        }

                        if (!string.IsNullOrEmpty(drhispat.PidDoctorCode.ToString()))
                        {
                            //医生代码转医生编码
                            string lis_doc_id = "";
                            List<EntityDicDoctor> temp_dtDoc = CacheClient.GetCache<EntityDicDoctor>();
                            if (temp_dtDoc != null && temp_dtDoc.Count > 0)
                            {
                                List<EntityDicDoctor> list_tempSel = temp_dtDoc.FindAll(r => r.DoctorCode == drhispat.PidDoctorCode.ToString());
                                if (list_tempSel != null && list_tempSel.Count > 0)
                                {
                                    lis_doc_id = list_tempSel[0].DoctorId.ToString();
                                }
                            }
                            if (!string.IsNullOrEmpty(lis_doc_id))
                            {
                                selectDicPubDoctor.SelectByID(lis_doc_id);
                            }
                        }
                        if (!string.IsNullOrEmpty(drhispat.PidTel.ToString()))
                        {
                            txtTel.Text = drhispat.PidTel.ToString();
                        }

                        #endregion
                    }
                    else
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("查不到相关信息");
                    }

                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    lis.client.control.MessageDialog.Show(ex.Message);
                }
            }
        }
    }
}
