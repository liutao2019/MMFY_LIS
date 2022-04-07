using System;
using System.Collections.Generic;
using System.Drawing;
using dcl.client.frame;
using DevExpress.XtraGrid.Views.Grid;
using lis.client.control;
using dcl.root.logon;
using dcl.client.common;
using dcl.client.wcf;
using dcl.entity;
using System.Linq;

namespace dcl.client.sample
{
    public partial class FrmBCMonitor : FrmCommon
    {
        //用于判断病人ID是否进行了统一设置
        string PatientIDNameConfirm;
        string valueOutTime = string.Empty;
        //新增条码来源*******************************************//
        private int[] barOrigin = new int[3];
        //********************************************************//
        public FrmBCMonitor()
        {
            InitializeComponent();
            bool showConfirmBar = UserInfo.HaveFunction(304);
            this.pnlSecondSend.Visible = showConfirmBar;

            //****************************************************************************************//
            //新增代码


            this.tsZY.IsOn = true;
            this.tsMZ.IsOn = true;
            this.tsTJ.IsOn = true;

            barOrigin[0] = 1;
            barOrigin[1] = 2;
            barOrigin[2] = 3;

            //注册三个事件，包括住院、门诊、体检三个复选框的勾选变化事件。
            this.tsZY.Toggled += new EventHandler(ImpatientCBox_CheckedChanged);
            this.tsMZ.Toggled += new EventHandler(OutPatientsCBox_CheckedChanged);
            this.tsTJ.Toggled += new EventHandler(TJ_CheckedChanged);

            //注册签收事件中的两个科室、物理组的单选按钮
            this.ckSignKS.CheckedChanged += new EventHandler(ckSignKS_CheckedChanged);
            this.ckSignType.CheckedChanged += new EventHandler(ckSignKS_CheckedChanged);

            //注册二次送检事件中的两个科室、物理组的单选按钮
            this.ckSecondSendKS.CheckedChanged += new EventHandler(ckSecondSendKS_CheckedChanged);
            this.ckSecondSendType.CheckedChanged += new EventHandler(ckSecondSendKS_CheckedChanged);

            //****************************************************************************************//

            //系统配置:条码监控几分钟为超时
            valueOutTime = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("BCMonitor_Timeout");

            if (valueOutTime == "20分钟")
            {
                label1.Text = "条码在同一状态滞留时间超过20分钟为超时";
            }

            PatientIDNameConfirm = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("PatientIDNameConfirm");
            //若病人ID配置不是为"不统一设置"，则修改为配置的名称
            if (!string.IsNullOrEmpty(PatientIDNameConfirm) && PatientIDNameConfirm != "不统一设置")
            {
                SetCaption(gvblood, PatientIDNameConfirm);
                SetCaption(gvReach, PatientIDNameConfirm);
                SetCaption(gvSecondSend, PatientIDNameConfirm);
                SetCaption(gvSend, PatientIDNameConfirm);
                SetCaption(gvSign, PatientIDNameConfirm);
                SetCaption(gvRegister, PatientIDNameConfirm);
            }

        }

        private void tmMonitor_Tick(object sender, EventArgs e)
        {
            if (!toggleSwitch1.IsOn)
            {
                tmMonitor.Stop();
            }
            getBCPatients();
        }

        private void FrmBCMonitor_Load(object sender, EventArgs e)
        {
            sysToolBar.SetToolButtonStyle(new[] { sysToolBar.BtnRefresh.Name
            , sysToolBar.BtnClose.Name});
        }

        private void SetCaption(GridView gv, string cap)
        {
            foreach (DevExpress.XtraGrid.Columns.GridColumn c in gv.Columns)
            {
                //if (c.FieldName == "bc_in_no" && c.Caption!="住院号")
                if (c.FieldName == "PidInNo" && c.Caption != "住院号")
                {
                    c.Caption = cap;
                }
            }
        }

        void btnRefresh_Click(object sender, EventArgs e)
        {
            //获取最新数据
            getBCPatients();
        }

        void ckSecondSendKS_CheckedChanged(object sender, EventArgs e)
        {
            if (ckSecondSendKS.Checked)
            {
                gridColumn107.GroupIndex = -1;
                gridColumn104.GroupIndex = 0;
                //gvSecondSend.GroupSummary[0].FieldName = "bc_d_name";
                gvSecondSend.GroupSummary[0].FieldName = "PidDeptName";
            }
            else
            {
                gridColumn104.GroupIndex = -1;
                gridColumn107.GroupIndex = 0;
                //gvSecondSend.GroupSummary[0].FieldName = "type_name";
                gvSecondSend.GroupSummary[0].FieldName = "ProName";
            }
        }

        void ckSignType_CheckedChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void ckSignKS_CheckedChanged(object sender, EventArgs e)
        {
            if (ckSignKS.Checked)
            {
                //gridColumn126.GroupIndex = -1;
                //gridColumn129.GroupIndex = 0;
                //gvSign.GroupSummary[0].FieldName = "bc_d_name";

                gridColumn129.GroupIndex = -1;
                gridColumn126.GroupIndex = 0;
                //gvSign.GroupSummary[0].FieldName = "bc_d_name";
                gvSign.GroupSummary[0].FieldName = "PidDeptName";
            }
            else
            {
                //gridColumn129.GroupIndex = -1;
                //gridColumn126.GroupIndex = 0;
                //gvSign.GroupSummary[0].FieldName = "type_name";

                gridColumn126.GroupIndex = -1;
                gridColumn129.GroupIndex = 0;
                //gvSign.GroupSummary[0].FieldName = "type_name";
                gvSign.GroupSummary[0].FieldName = "ProName";
            }
        }

        //获取不同的住院、体检、门诊的各个状态下的条码
        private void getBCPatients()
        {
            string str = string.Empty;
            foreach (int item in barOrigin)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str += ",";
                }
                str += item.ToString();
            }

            //DataSet ds = base.doSearch();
            ProxySampProcessMonitor proxyProMoitor = new ProxySampProcessMonitor();
            List<EntitySampProcessMonitor> listProMoitor = proxyProMoitor.Service.GetBCPatients();

            if (listProMoitor.Count > 0)
            {
                List<EntitySampProcessMonitor> listBcPatients = listProMoitor;
                //DataTable cache = GetBcPatientsFiltrateType(this.GetBcPatientsBarMonitor(dtBcPatients, str));
                List<EntitySampProcessMonitor> cache = GetBcPatientsFiltrateType(this.GetBcPatientsBarMonitor(listBcPatients, str));

                //将数据利用rowFilter过滤器进行根据字段属性过滤到采集框中
                List<EntitySampProcessMonitor> listBlood = new List<EntitySampProcessMonitor>();
                //dvBlood.RowFilter = string.Format("bc_status = '{0}'", EnumBarcodeOperationCode.SampleCollect);
                listBlood = cache.Where(w => w.SampStatusId == EnumBarcodeOperationCodeNew.SampleCollect.ToString()).ToList();
                bdBlood.DataSource = listBlood;
                pnlBlood.Text = string.Format("采集：{0} 条", listBlood.Count.ToString());

                //将数据利用rowFilter过滤器进行根据字段属性过滤到送达框中
                //DataView dvReach = new DataView(cache.Copy());
                List<EntitySampProcessMonitor> listReach = new List<EntitySampProcessMonitor>();
                //dvReach.RowFilter = string.Format("bc_status='{0}'", EnumBarcodeOperationCode.SampleReach);
                listReach = cache.Where(w => w.SampStatusId == EnumBarcodeOperationCodeNew.SampleReach.ToString()).ToList();
                bdReach.DataSource = listReach;
                pnlReach.Text = string.Format("送达：{0}条", listReach.Count.ToString());

                //将数据利用rowFilter过滤器进行根据字段属性过滤到收取框中
                //DataView dvSend = new DataView(cache.Copy());
                List<EntitySampProcessMonitor> listSend = new List<EntitySampProcessMonitor>();
                //dvSend.RowFilter = string.Format("bc_status='{0}'", EnumBarcodeOperationCode.SampleSend);
                listSend = cache.Where(w => w.SampStatusId == EnumBarcodeOperationCodeNew.SampleSend.ToString()).ToList();
                bdSend.DataSource = listSend;
                pnlSend.Text = string.Format("收取：{0}条", listSend.Count.ToString());

                //将数据利用rowFilter过滤器进行根据字段属性过滤到二次送检框中
                //DataView dvSecondSend = new DataView(cache.Copy());
                List<EntitySampProcessMonitor> listSecondSend = new List<EntitySampProcessMonitor>();
                //dvSecondSend.RowFilter = string.Format("bc_status='{0}'", EnumBarcodeOperationCode.SampleSecondSend);
                listSecondSend = cache.Where(w => w.SampStatusId == EnumBarcodeOperationCodeNew.SampleSecondSend.ToString()).ToList();
                bdSecondSend.DataSource = listSecondSend;
                pnlSecondSend.Text = string.Format("二次送检：{0}条", listSecondSend.Count.ToString());

                //将数据利用rowFilter过滤器进行根据字段属性过滤到签收框中
                //DataView dvReceive = new DataView(cache.Copy());
                List<EntitySampProcessMonitor> listReceive = new List<EntitySampProcessMonitor>();
                //dvReceive.RowFilter = string.Format("bc_status='{0}'", EnumBarcodeOperationCode.SampleReceive);
                listReceive = cache.Where(w => w.SampStatusId == EnumBarcodeOperationCodeNew.SampleReceive.ToString()).ToList();
                bdSign.DataSource = listReceive;
                groupControl1.Text = string.Format("签收：{0}条", listReceive.Count.ToString());


                //将数据利用rowFilter过滤器进行根据字段属性过滤到检验中框中
                //DataView dvRegister = new DataView(cache.Copy());
                List<EntitySampProcessMonitor> listRegister = new List<EntitySampProcessMonitor>();
                //资料登记 与 取消审核、一审 都被认为检验中
                //dvRegister.RowFilter = string.Format("bc_status='{0}' or bc_status='{1}' or bc_status='{2}'", EnumBarcodeOperationCode.SampleRegister, EnumBarcodeOperationCode.UndoAudit, EnumBarcodeOperationCode.Audit);
                listRegister = cache.Where(w => w.SampStatusId == EnumBarcodeOperationCodeNew.SampleRegister.ToString() || w.SampStatusId == EnumBarcodeOperationCodeNew.UndoAudit.ToString() || w.SampStatusId == EnumBarcodeOperationCodeNew.Audit.ToString()).ToList();
                bdRegister.DataSource = checkTAT(listRegister);
                pnlSampleRegister.Text = string.Format("检验中：{0}条", listRegister.Count.ToString());
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("无条码数据可监控", 2);
                return;
            }


        }

        private List<EntitySampProcessMonitor> checkTAT(List<EntitySampProcessMonitor> dvObj)
        {
            if (dvObj == null || dvObj.Count <= 0)
            {
                return dvObj;
            }
            else
            {
                List<EntitySampProcessMonitor> listTempTAT = dvObj;
                if (listTempTAT != null && listTempTAT.Count > 0)
                {
                    //dcl.client.wcf.ProxyMessage proxy = new dcl.client.wcf.ProxyMessage();
                    //DataTable t_dtComTATMsg = proxy.Service.GetComTATMessage("");
                    ProxyCombineTATMsg proxyComTATMsg = new ProxyCombineTATMsg();
                    List<EntityDicMsgTAT> listComTATMsg = proxyComTATMsg.Service.GetComTATMessage();

                    if (listComTATMsg != null && listComTATMsg.Count > 0)
                    {
                        //if (!dtTempTAT.Columns.Contains("over_tat"))
                        //{
                        //    //添加TAT标记，-1：没超TAT；0：超出TATW;1：超出TAT
                        //    dtTempTAT.Columns.Add("over_tat", Type.GetType("System.String"));
                        //}

                        foreach (var infoTempTAT in listTempTAT)
                        {
                            if (!string.IsNullOrEmpty(infoTempTAT.SampBarCode))
                            {
                                //DataRow[] drTempSel = t_dtComTATMsg.Select(string.Format("bc_bar_code='{0}'", drTempTAT["bc_bar_code"].ToString()));
                                List<EntityDicMsgTAT> listTatTempSel = listComTATMsg.Where(w => w.SampBarCode == infoTempTAT.SampBarCode).ToList();
                                if (listTatTempSel.Count > 0)
                                {
                                    bool isoverTAT = false;
                                    for (int i = 0; i < listTatTempSel.Count; i++)
                                    {
                                        if (listTatTempSel[i].OverTat.ToString() == "1")
                                        {
                                            isoverTAT = true;
                                            break;
                                        }
                                    }

                                    if (isoverTAT)
                                    {
                                        infoTempTAT.OverTat = "1";
                                       // infoTempTAT.BcIsTimeout = 1;
                                    }
                                    else
                                    {
                                        infoTempTAT.OverTat = "0";
                                       // infoTempTAT.BcIsTimeout = 0;
                                    }
                                }
                                else
                                {
                                    infoTempTAT.OverTat = "-1";
                                    //infoTempTAT.BcIsTimeout = 0;
                                }
                            }
                            else
                            {
                                infoTempTAT.OverTat = "-1";
                               // infoTempTAT.BcIsTimeout = 0;
                            }
                        }

                        return listTempTAT;
                    }
                    //else
                    //{
                    //    foreach (var infoTempTAT in dvObj)
                    //    {
                    //        infoTempTAT.BcIsTimeout =0;
                    //    }
                    //}
                }
            }
            return dvObj;
        }

        public List<EntitySampProcessMonitor> GetBcPatientsBarMonitor(List<EntitySampProcessMonitor> Cache, string bcOrigin)
        {
            List<EntitySampProcessMonitor> result = new List<EntitySampProcessMonitor>();

            string[] BarOrigin = bcOrigin.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                int[] barOriginStr = new int[3];
                for (int i = 0; i < BarOrigin.Length; i++)
                {
                    switch (BarOrigin[i])
                    {
                        case "1":
                            barOriginStr[i] = 108;
                            break;
                        case "2":
                            barOriginStr[i] = 107;
                            break;
                        case "3":
                            barOriginStr[i] = 109;
                            break;
                        default:
                            barOriginStr[i] = 111;
                            break;
                    }
                }

                //将所查询到的结果进行过滤筛选
                result = Cache.Where(w => w.PidSrcId == barOriginStr[0].ToString() || w.PidSrcId == barOriginStr[1].ToString() || w.PidSrcId == barOriginStr[2].ToString()).ToList();

            }
            catch (Exception ex)
            {
                Logger.WriteException("FrmBCMonitor", "ThreadRefresh.GetBcPatientsBarMonitor", ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 按物理组过滤条码信息
        /// </summary>
        /// <param name="Cache"></param>
        /// <returns></returns>
        private List<EntitySampProcessMonitor> GetBcPatientsFiltrateType(List<EntitySampProcessMonitor> Cache)
        {
            List<EntitySampProcessMonitor> result = new List<EntitySampProcessMonitor>();
            try
            {
                if (!ctypeCBox.Checked)//不按物理组过滤条码信息
                {
                    result = Cache;
                }
                else//按物理组过滤条码信息
                {
                    if (string.IsNullOrEmpty(lueType.displayMember))//如果为空不过滤
                    {
                        result = Cache;
                    }
                    else
                    {
                        //将所查询到的结果通过物理组过滤
                        //sqlWhere = string.Format("bc_ctype='{0}' ", lueType.valueMember);
                        result = Cache.Where(w => w.SampType == lueType.valueMember).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException("FrmBCMonitor", "ThreadRefresh.GetBcPatientsFiltrateType", ex.Message);
            }
            return result;
        }
        private void setGridColor(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //DataRow dr = ((GridView)sender).GetDataRow(e.RowHandle);
            EntitySampProcessMonitor eyProMonitor = ((GridView)sender).GetRow(e.RowHandle) as EntitySampProcessMonitor;
            if (
                eyProMonitor != null
                && eyProMonitor.BcTimeDifference != null
                && eyProMonitor.BcTimeDifference.ToString().Trim() != "")
            {
                string[] time = valueOutTime.Split('分');
                int diffTime = eyProMonitor.BcTimeDifference.Value;
                if (time.Count() > 0 && !string.IsNullOrEmpty(time[0]))
                {
                    if (diffTime > Convert.ToInt32(time[0]))
                        e.Appearance.ForeColor = Color.Red;
                }
            }
        }

        private void ckBlood_CheckedChanged(object sender, EventArgs e)
        {
            if (ckBloodKS.Checked)
            {
                gridColumn55.GroupIndex = -1;
                gridColumn17.GroupIndex = 0;
                //gvblood.GroupSummary[0].FieldName = "bc_d_name";
                gvblood.GroupSummary[0].FieldName = "PidDeptName";

                //gridColumn79.GroupIndex = -1;
                //gridColumn76.GroupIndex = 0;
                //gvblood.GroupSummary[0].FieldName = "bc_d_name";
            }
            else
            {
                gridColumn17.GroupIndex = -1;
                gridColumn55.GroupIndex = 0;
                //gvblood.GroupSummary[0].FieldName = "type_name";
                gvblood.GroupSummary[0].FieldName = "ProName";

                //gridColumn76.GroupIndex = -1;
                //gridColumn79.GroupIndex = 0;
                //gvblood.GroupSummary[0].FieldName = "type_name";
            }
            //gvblood.ExpandAllGroups();
        }

        private void ckSend_CheckedChanged(object sender, EventArgs e)
        {
            if (ckSendKS.Checked)
            {
                gridColumn56.GroupIndex = -1;
                gridColumn49.GroupIndex = 0;
                //gvSend.GroupSummary[0].FieldName = "bc_d_name";
                gvSend.GroupSummary[0].FieldName = "PidDeptName";
            }
            else
            {
                gridColumn49.GroupIndex = -1;
                gridColumn56.GroupIndex = 0;
                //gvSend.GroupSummary[0].FieldName = "type_name";
                gvSend.GroupSummary[0].FieldName = "ProName";
            }
            //gvSend.ExpandAllGroups();
        }

        private void ckReach_CheckedChanged(object sender, EventArgs e)
        {
            if (ckReachKS.Checked)
            {
                gridColumn57.GroupIndex = -1;
                gridColumn29.GroupIndex = 0;
                //gvReach.GroupSummary[0].FieldName = "bc_d_name";
                gvReach.GroupSummary[0].FieldName = "PidDeptName";
            }
            else
            {
                gridColumn29.GroupIndex = -1;
                gridColumn57.GroupIndex = 0;
                //gvReach.GroupSummary[0].FieldName = "type_name";
                gvReach.GroupSummary[0].FieldName = "ProName";
            }
            //gvReach.ExpandAllGroups();
        }

        //private void ckMonitor_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (!toggleSwitch1.IsOn)
        //    {
        //        tmMonitor.Stop();
        //        tmCheck.Start();
        //        this.btnRefresh.Enabled = true;
        //    }
        //    else
        //    {
        //        tmMonitor.Start();
        //        tmCheck.Stop();
        //        this.btnRefresh.Enabled = false;
        //    }
        //}

        private void tmCheck_Tick(object sender, EventArgs e)
        {
            toggleSwitch1.IsOn = false;
        }

        //根据住院复选框的勾选变化进行筛选
        private void ImpatientCBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.tsZY.IsOn == false)
            {
                barOrigin[0] = 4;
            }
            else
            {
                barOrigin[0] = 1;
            }
            getBCPatients();

        }

        //根据门诊复选框的勾选变化进行筛选
        private void OutPatientsCBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.tsMZ.IsOn == false)
            {
                barOrigin[1] = 4;
            }
            else
            {
                barOrigin[1] = 2;
            }
            getBCPatients();
        }

        //根据体检复选框的勾选变化进行筛选
        private void TJ_CheckedChanged(object sender, EventArgs e)
        {
            if (this.tsTJ.IsOn == false)
            {
                barOrigin[2] = 4;
            }
            else
            {
                barOrigin[2] = 3;
            }
            getBCPatients();
        }
        /// <summary>
        /// 启动根据物理组筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctypeCBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ctypeCBox.Checked)
            {
                this.lueType.Enabled = true;
            }
            else
            {
                this.lueType.Enabled = false;
            }
            getBCPatients();
        }

        private void ckRegisterType_CheckedChanged(object sender, EventArgs e)
        {
            if (!ckRegisterType.Checked)
            {
                gridColumn80.GroupIndex = -1;
                gridColumn77.GroupIndex = 0;
                //gvRegister.GroupSummary[0].FieldName = "bc_d_name";
                gvRegister.GroupSummary[0].FieldName = "PidDeptName";
            }
            else
            {
                gridColumn77.GroupIndex = -1;
                gridColumn80.GroupIndex = 0;
                //gvRegister.GroupSummary[0].FieldName = "type_name";
                gvRegister.GroupSummary[0].FieldName = "ProName";
            }
        }

        private void gvRegister_RowStyle(object sender, RowStyleEventArgs e)
        {
            //DataRow dr = this.gvRegister.GetDataRow(e.RowHandle);
            EntitySampProcessMonitor syProMonitor = this.gvRegister.GetRow(e.RowHandle) as EntitySampProcessMonitor;
            if (syProMonitor != null)
            {
                if (syProMonitor.BcIsTimeout == 1)//超出TAT为红色
                {
                    e.Appearance.BackColor = Color.Red;
                }
                //else if (syProMonitor.OverTat == "0")//只超出TATW为黄色
                //{
                //    e.Appearance.BackColor = Color.Yellow;
                //}
                else
                {
                    e.Appearance.BackColor = Color.Transparent;
                }
            }
        }

        private void sysToolBar_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            btnRefresh_Click(null, null);
        }

        private void sysToolBar_OnCloseClicked(object sender, EventArgs e)
        {
            tmMonitor.Stop();
            tmCheck.Stop();
            Close();
        }
        
        //监控按钮状态改变事件
        private void toggleSwitch1_Toggled(object sender, EventArgs e)
        {
            if (toggleSwitch1.IsOn)
            {
                tmMonitor.Start();
            }
            else {
                tmMonitor.Stop();
            }
        }
    }
}