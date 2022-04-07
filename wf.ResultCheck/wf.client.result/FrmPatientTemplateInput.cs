using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.common.extensions;
using dcl.client.common;
using dcl.client.wcf;
using dcl.common;
using lis.client.control;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.result
{
    public partial class FrmPatientTemplateInput : FrmCommon
    {

        List<entity.EntityPidReportMain> listPat = new List<entity.EntityPidReportMain>();
        FrmPatientList frmPatients;
        PatientTemplate currentTemplate;
        List<entity.EntityPidReportMain> listPatBasic = new List<entity.EntityPidReportMain>();
        List<entity.EntityPidReportMain> listPhysical = new List<entity.EntityPidReportMain>();
        ProxyPatTempInput proxy = new ProxyPatTempInput();
        public FrmPatientTemplateInput()
        {
            InitializeComponent();
            Initialize();
            base.ShowSucessMessage = false;
        }

        public FrmPatientList FrmPatients
        {
            get
            {
                if (frmPatients == null)
                    frmPatients = new FrmPatientList();

                return frmPatients;
            }
        }

        public void Initialize()
        {
            dateSource.Text = DateTime.Now.ToShortDateString();
            dateEditRecv.Text = dateCheck.Text = dateSend.Text = dateInput.Text = DateTime.Now.ToString();

        }

        #region 验证

        private bool CheckSourceInput(ref string message)
        {
            bool success = true;
            if (message == null) message = String.Empty;

            if (selectItrSource.valueMember == null)
            {
                message += "请输入源仪器号!\r\n";
                success = false;
            }

            return success;
        }

        /// <summary>
        /// 表单验证
        /// </summary>
        private bool CheckTxtContent()
        {
            bool success = true;
            String message = String.Empty;

            success = CheckSourceInput(ref message);

            if (!success || message.Length > 0)
            {
                ShowInputError(message);
            }
            return success;
        }

        #endregion

        private static void ShowInputError(String message)
        {
            MessageBox.Show("操作被取消,原因是:\r\n" + message, "提示!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <returns></returns>
        private List<entity.EntityPidReportMain> Search()
        {
            List<entity.EntityPidReportMain> listPatients = proxy.Service.GetPatientsDetail(dateSource.DateTime, dateSource.DateTime.AddDays(1).AddMilliseconds(-1), "", selectItrSource.valueMember, txtSID.Text);
            return listPatients;
        }

        private void SearchSource()
        {
            if (GetSource() == false)
                return;

            FrmPatients.BindingSource.DataSource = listPat;
            FrmPatients.ShowDialog();
        }

        private bool GetSource()
        {
            string message = "";
            if (!CheckSourceInput(ref message))
            {
                ShowInputError(message);
                return false;
            }
            listPat = Search();
            if (listPat.Count <= 0)
            {
                MessageBox.Show("没有数据符合当前查询条件! ", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 复制
        /// </summary>
        private void Copy()
        {
            string message = "";
            if (!CheckCopyCondition(out message))
            {
                lis.client.control.MessageDialog.Show(message);
                return;
            }

            if (listPatBasic.Count < 0)
            {
                lis.client.control.MessageDialog.Show("无源数据，请查询！");
                return;
            }

            if (lis.client.control.MessageDialog.Show("是否复制? ", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DoSave();
            }
        }

        private bool CheckCopyCondition(out string message)
        {
            message = "";
            if (selectItrSource.displayMember == selectItrTarget.displayMember)
            {
                message += "同仪器不能复制!\r\n";
                return false;
            }

            if (Extensions.IsEmpty(selectItrTarget.displayMember))
            {
                message += "请选择目标仪器\r\n";
                return false;
            }

            return true;
        }

        private void DoSave()
        {
            List<entity.EntityPidReportMain> listInput = new List<entity.EntityPidReportMain>();
            if (currentTemplate == PatientTemplate.Basic)
            {
                gvBasic.CloseEditor();
                listInput = bsBasic.DataSource as List<entity.EntityPidReportMain>;
            }
            else if (currentTemplate == PatientTemplate.Physical)
            {
                gvPhysical.CloseEditor();
                listInput = bsPhysical.DataSource as List<entity.EntityPidReportMain>;
            }
            if (listInput == null || listInput.Count < 1)
            {
                return;
            }

            //修改listInput的值
            foreach (entity.EntityPidReportMain patient in listInput)
            {
                //录入日期
                patient.RepItrId = selectItrTarget.valueMember;
                patient.PidSamId = selectDict_Sample_type_Target.valueMember;

                //如果启动送检时间设定，则用重新设定送检时间，否则用原病人资料的送检时间
                if (checkBox_sendDate.Checked)
                {
                    patient.SampSendDate = dateSend.DateTime;

                    if (dateCheck.DateTime.ToString("yyyy-MM-dd HH:mm") != dateSend.DateTime.ToString("yyyy-MM-dd HH:mm"))
                    {
                        if (MessageDialog.Show("检验时间和送检时间不一致，是否继续? ", "提示", MessageBoxButtons.YesNo) ==
                            DialogResult.No)
                            return;
                    }
                }

                if (checkBoxRecv.Checked)
                {
                    patient.SampApplyDate = dateEditRecv.DateTime;

                }
                else
                {
                    patient.SampApplyDate = DateTime.Now;
                }

                patient.RepReportDate = DateTime.Now;
                patient.RepAuditDate = DateTime.Now;
                patient.SampCollectionDate = DateTime.Now;
                patient.SampReceiveDate = DateTime.Now;
                patient.SampReachDate = DateTime.Now;

                patient.SampCheckDate = this.dateCheck.DateTime;
                patient.RepCheckUserId = selectDict_inspect_target.valueMember;
                patient.RepInDate = dateInput.DateTime;
                if (currentTemplate == PatientTemplate.Physical)
                {
                    string prevPatIdType = null;//ID类型
                    prevPatIdType = this.txtPatIdType.valueMember;
                    string ori_id = CacheClient.GetCache<EntityDicPubIdent>().Find(i => i.IdtId == this.txtPatIdType.valueMember).IdtSrcId;//病人来源ID

                    if (patient.PatAgeTxt != null)
                    {
                        patient.PidAgeExp = FormartAge(patient.PatAgeTxt.ToString());
                    }
                    if (!string.IsNullOrEmpty(patient.PidAgeExp))
                        patient.PidAge = AgeConverter.AgeValueTextToMinute(patient.PidAgeExp.ToString());
                    if (ori_id == null || ori_id == "109")
                        patient.PidExamNo = patient.PidInNo;//赋值体检值

                    if (string.IsNullOrEmpty(prevPatIdType) || string.IsNullOrEmpty(ori_id))
                    {
                        this.txtPatIdType.SelectByID("110");

                        //病人ID类型
                        patient.PidIdtId = "110";//体检号

                        //来源
                        patient.PidSrcId = "109";//来源为体检
                    }
                    else
                    {

                        //病人ID类型
                        patient.PidIdtId = prevPatIdType;//体检号

                        //来源
                        patient.PidSrcId = ori_id;//来源为体检
                    }

                    //性别
                    if (patient.PidSex == "男")
                    {
                        patient.PidSex = "1";
                    }
                    else if (patient.PidSex == "女")
                    {
                        patient.PidSex = "2";
                    }
                    else if (patient.PidSex == string.Empty)
                    {
                        patient.PidSex = "";
                    }

                    //仪器
                    patient.ItrEname = selectItrTarget.valueMember;
                    //送检者
                    patient.PidDoctorCode = selectDict_Doctor1.valueMember;
                    patient.PidDocName = selectDict_Doctor1.displayMember;
                    //诊断
                    patient.PidDiag = selectDict_diagnos1.displayMember;
                    //费用类型
                    patient.PidInsuId = this.selectDict_Fees_Type1.displayMember;
                    //单位
                    patient.PidUnit = this.txtDept.Text;
                    //送检科室
                    patient.PidDeptId = this.selectDict_Depart1.valueMember;
                    patient.PidDeptName = this.selectDict_Depart1.displayMember;
                    //检查类型
                    patient.RepCtype = this.selectDict_PatCheckType1.valueMember;
                    //标本状态
                    patient.PidRemark = this.selectDict_s_state1.displayMember;
                }
            }
            SaveToDB(listInput);
        }

        private string FormartAge(string age)
        {
            if (string.IsNullOrEmpty(age) || age.Trim() == string.Empty)
                return string.Empty;
            if (age.IndexOf("Y") >= 0)
                return age;
            if (age.IndexOf("年") >= 0)
                return age.Replace('年', 'Y').Replace('月', 'M').Replace('日', 'D').Replace('时', 'H');//新outlink修改了返回年龄格式    2010-6-18 by li
            return age + "Y0M0D0H0I";
        }



        private void SaveToDB(List<entity.EntityPidReportMain> dtData)
        {
            EntityQcResultList resultList = new EntityQcResultList();
            bool allOK = true;
            string message = "";
            foreach (entity.EntityPidReportMain row in dtData)
            {
                resultList.patient = row;
                //病人明细
                List<EntityPidReportDetail> tbCombine = GetCombine(row.RepId);
                if (tbCombine != null)
                {
                    resultList.patient.ListPidReportDetail = tbCombine;
                    row.PidComName = this.combineEditor1.Text;
                }
                else
                {
                    row.PidComName = string.Empty;
                }


                EntityOperateResult result = proxy.Service.InsertPatCommonResult(dcl.client.common.Util.ToCallerInfo(), resultList); //PatientCRUDClient.NewInstance.InsertPatCommonResult(dcl.client.common.Util.ToCallerInfo(), dsInput);
                allOK = allOK && result.Success;
                if (!result.Success && !result.HasExcaptionError)
                    message += OperationMessageHelper.GetErrorMessage(result.Message) + "\r\n";
            }

            if (allOK)
                lis.client.control.MessageDialog.Show("操作成功!");
            else
            {
                if (Extensions.IsEmpty(message))
                    lis.client.control.MessageDialog.Show("操作失败!");
                else
                    lis.client.control.MessageDialog.Show("操作失败!\r\n" + message);
            }
        }

        /// <summary>
        /// 获取组合
        /// </summary>
        /// <param name="patientID">病人ID</param>
        private List<EntityPidReportDetail> GetCombine(string patientID)
        {
            if (HasNewCombine())
            {
                List<EntityPidReportDetail> combines = GetNewCombine();
                return combines;
            }
            else
            {
                return proxy.Service.GetPidReportDetailByRepId("-1");
            }
        }

        private List<EntityPidReportDetail> GetNewCombine()
        {
            return combineEditor1.listRepDetail;
        }

        private bool HasNewCombine()
        {
            return combineEditor1.listRepDetail != null && combineEditor1.listRepDetail.Count > 0;
        }

        /// <summary>
        /// 复制
        /// </summary>
        private void HandleData()
        {
            //DataTable dtMerge = dtSource.Clone();
            //DataTable dtSourceTemp = dtSource.Copy();
            //DataTable dtAdd = dtSource.Clone();
            //List<EntityPatients> listSourcePat = listPat;
            //foreach (EntityPatients sourceRow in listSourcePat)
            //{
            //    string itrID = sourceRow["res_itr_id"].ToString(); //仪器                   
            //    string sampleID = sourceRow["res_sid"].ToString(); //样本                  
            //    string itemCode = sourceRow["res_itm_ecd"].ToString();  //项目代码
            //    string targetSampleID = "";// CalcTargetSampleID(sampleID);
            //    //如果找到此数据,则复制
            //    DataRow[] targetRows = dtTarget.Select(string.Format(" res_itr_id = '{0}' and res_sid = '{1}' and res_itm_ecd = '{2}' ", itrID, targetSampleID, itemCode));
            //    if (targetRows != null && targetRows.Length > 0)
            //    {
            //        //已审核则跳过
            //        if (HasAudited(targetRows[0]))
            //            continue;

            //        targetRows[0]["res_chr"] = sourceRow["res_chr"];
            //        //如果有数据
            //        if (targetRows[0]["res_chr"] != null && targetRows[0]["res_chr"].ToString() != "")
            //        {
            //            dtMerge.Rows.Add(targetRows[0].ItemArray);
            //        }
            //    }
            //    else //找不到对应数据,则新增
            //    {
            //        //更改样本号,仪器和日期
            //        sourceRow["res_sid"] = targetSampleID;
            //        sourceRow["res_itr_id"] = selectItrTarget.valueMember;
            //        dtAdd.Rows.Add(sourceRow.ItemArray);
            //    }
            //}
            //bool merge = false;
            //if (Extensions.IsNotEmpty(dtMerge))//有要合并的数据
            //{
            //    string message = GetMergeMessage(dtMerge);
            //    DialogResult result = lis.client.control.MessageDialog.Show(message + "是否覆盖?", "提示", MessageBoxButtons.YesNoCancel);
            //    if (result == DialogResult.Yes)
            //    {
            //        merge = true;
            //        base.doUpdate(dtMerge.Copy());
            //    }
            //}

            //DataSet dsData = new DataSet();
            //dsData.Tables.Add(dtAdd);
            //base.doNew(dsData);
            //if (isActionSuccess)
            //{
            //    if (merge)
            //        dtAdd.Merge(dtMerge);
            //    else
            //        dtAdd.Merge(dtTarget);
            //    dtTarget = dtAdd;
            //}
        }

        private static bool HasAudited(DataRow targetRows)
        {
            return targetRows["pat_flag"].ToString() == LIS_Const.PATIENT_FLAG.Audited
                                    || targetRows["pat_flag"].ToString() == LIS_Const.PATIENT_FLAG.Printed
                                     || targetRows["pat_flag"].ToString() == LIS_Const.PATIENT_FLAG.Reported;
        }


        /// <summary>
        /// 存在数据的样本信息提示
        /// </summary>
        private string GetMergeMessage(DataTable dtMerge)
        {
            string message = "";
            foreach (DataRow row in dtMerge.Rows)
            {
                message += string.Format(" 样本号为{0}, 检查项目为{1} 的样本结果已存在 \r\n", row["res_sid"], row["res_itm_ecd"]);
            }
            return message;
        }

        private void FrmMergeResult_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            InitButtons();
            ShowButtons();
            InitData();
        }

        private void InitData()
        {
            combineEditor1.listRepDetail = GetCombine("-1");
            bsSampleState.DataSource = CacheClient.GetCache<EntityDicSState>();

            //初始化时默认检验者为登录者
            this.selectDict_inspect_target.valueMember = UserInfo.loginID;
            this.selectDict_inspect_target.displayMember = UserInfo.userName;

        }

        private void InitButtons()
        {
            sysToolBar1.BtnAdd.Caption = "建立模板";
            sysToolBar1.BtnSearch.Caption = "查询数据";
            sysToolBar1.BtnExport.Caption = "导出模板";
            sysToolBar1.BtnImport.Caption = "导入数据";

            sysToolBar1.SetToolButtonStyle(
                new string[] {
                     sysToolBar1.BtnAdd.Name,
                     sysToolBar1.BtnCopy.Name,
                     sysToolBar1.BtnSave.Name,
                     sysToolBar1.BtnSearch.Name,
                     sysToolBar1.BtnClose.Name,
                     sysToolBar1.BtnExport.Name,
                     sysToolBar1.BtnImport.Name},
                    new string[] { "F3", "F4", "F5", "F6", "F7", "F8", "F9" }
                    );
        }

        private void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            SearchSource();
        }

        private void sysToolBar1_BtnCopyClick(object sender, EventArgs e)
        {
            Copy();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == pagePhysical)
                currentTemplate = PatientTemplate.Physical;
            else if (xtraTabControl1.SelectedTabPage == pageBasic)
                currentTemplate = PatientTemplate.Basic;
            else
                currentTemplate = PatientTemplate.Basic;

            ShowUI(currentTemplate);
        }

        bool hasShowedNecessaryForPhysicalUI = false;

        /// <summary>
        /// 根据特定页调整界面
        /// </summary> 
        private void ShowUI(PatientTemplate template)
        {
            switch (template)
            {
                case PatientTemplate.Basic:

                    layitemIDType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    //excel导入导出按钮
                    sysToolBar1.BtnExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    sysToolBar1.BtnImport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                    ShowButtons();
                    if (hasShowedNecessaryForPhysicalUI == true)
                    {
                        layitemItrTar.Text = "目标仪器";
                        layitemSamType.Text = "标本类型";
                        hasShowedNecessaryForPhysicalUI = false;
                    }

                    //显示测定日期与仪器
                    layitemDate.Visibility =  DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layitemLab.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    break;
                case PatientTemplate.Physical:
                    panelPhysical.Visible = true;
                    sysToolBar1.BtnCopy.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    sysToolBar1.BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    //excel导入导出按钮
                    sysToolBar1.BtnExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    sysToolBar1.BtnImport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                    groupTarget.Visible = true;

                    layitemIDType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                    if (hasShowedNecessaryForPhysicalUI == false)
                    {
                        layitemItrTar.Text = "*目标仪器";
                        layitemSamType.Text = "*标本类型";
                        hasShowedNecessaryForPhysicalUI = true;
                    }

                    //隐藏测定日期与仪器
                    layitemDate.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layitemLab.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    break;

                default:
                    ShowButtons();
                    break;
            }
        }

        private void ShowButtons()
        {
            groupTarget.Visible = true;
            panelPhysical.Visible = false;
            sysToolBar1.BtnCopy.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            sysToolBar1.BtnImport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            sysToolBar1.BtnExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            sysToolBar1.BtnSave.Visibility = sysToolBar1.BtnCopy.Visibility == DevExpress.XtraBars.BarItemVisibility.Never ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
        }


        private void sysToolBar1_OnBtnAddClicked(object sender, EventArgs e)
        {
            AddTemplate();
        }

        /// <summary>
        /// 新建模板  
        /// </summary>
        private void AddTemplate()
        {
            if (currentTemplate == PatientTemplate.Basic)
            {
                if (GetSource() == false)
                    return;

                bsBasic.DataSource = listPat;


                GridViewHelper.FocusCell(gvBasic, 0, "PidName");
            }
            else if (currentTemplate == PatientTemplate.Physical)
            {
                if (Extensions.IsEmpty(txtSID.Text))
                {
                    lis.client.control.MessageDialog.Show("请选择样本范围!");
                    txtSID.Focus();
                    return;
                }

                if (listPhysical == null)
                {
                    listPhysical = GetPatientTableStructure();
                }

                listPhysical = new List<entity.EntityPidReportMain>();

                List<long> sampleIDRange = SampleIDRangeUtil.ToList(txtSID.Text);
                if (sampleIDRange.Count <= 0)
                    return;

                foreach (int item in sampleIDRange)
                {
                    entity.EntityPidReportMain patient = new entity.EntityPidReportMain();
                    patient.RepSid = item.ToString();
                    patient.PidSex = "";
                    listPhysical.Add(patient);
                }

                bsPhysical.DataSource = listPhysical;
                GridViewHelper.FocusCell(gvPhysical, 0, "PidName");
            }
        }



        /// <summary>
        /// 获取Patients表的结构
        /// </summary>
        /// <returns></returns>
        private List<entity.EntityPidReportMain> GetPatientTableStructure()
        {
            return new List<entity.EntityPidReportMain>();
        }

        private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            if (listPhysical.Count < 1)
            {
                lis.client.control.MessageDialog.Show("无源数据，请查询！");
                return;
            }

            string message = "";
            if (!CheckSaveCondition(out message))
            {
                lis.client.control.MessageDialog.Show(message);
                return;
            }

            if (lis.client.control.MessageDialog.Show("是否保存? ", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DoSave();
            }
        }

        private bool CheckSaveCondition(out string message)
        {
            message = "";

            if (Extensions.IsEmpty(selectItrTarget.displayMember))
            {
                message += "请选择目标仪器\r\n";
                return false;
            }

            if (Extensions.IsEmpty(selectDict_Sample_type_Target.displayMember))
            {
                message += "请选择标本类型\r\n";
                return false;
            }

            if (currentTemplate == PatientTemplate.Physical)
            {
                if (Extensions.IsEmpty(txtPatIdType.displayMember))
                {
                    message += "请选择标ID类型\r\n";
                    return false;
                }
            }
            return true;
        }

        private void gvPhysical_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0 && e.Column.Name == "PidSex")
            {
                entity.EntityPidReportMain currentPat = gvPhysical.GetFocusedRow() as entity.EntityPidReportMain;
                //如果是1或2,则转化成男或女
                //if (e.Value.ToString().Trim() == "1")
                //    currentPat.pat_sex_name = "男";
                //else if (e.Value.ToString().Trim() == "2")
                //    currentPat.pat_sex_name = "女";


                //如果是男或女,值所在列为1或2;否则清空,值所在列改为0
                //if (currentPat.PidSexName.Trim() == "男")
                //    currentPat.PidSex = "1";
                //else if (currentPat.PidSexName.Trim() == "女")
                //    currentPat.PidSex = "2";
                //else if (string.IsNullOrEmpty(currentPat.PidSexName.Trim()))
                //{
                //    currentPat.PidSex = "0";
                //}
                //else
                //{
                //    currentPat.PidSex = "0";
                //}
            }
        }

        /// <summary>
        /// 导出Excel方法
        /// </summary>
        /// <param name="gcExcel"></param>
        private void setExcel(DevExpress.XtraGrid.GridControl gcExcel)
        {
            if (gcExcel.DataSource != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                        return;
                    }

                    try
                    {
                        gcExcel.ExportToXls(ofd.FileName.Trim());
                        //gcExcel.ExportToXls(ofd.FileName.Trim());       
                        lis.client.control.MessageDialog.Show("导出模板成功！", "提示");
                    }
                    catch (Exception ex)
                    {
                        dcl.root.logon.Logger.WriteException(this.GetType().Name, "导出模板", ex.ToString());
                        lis.client.control.MessageDialog.Show("导出模板失败！", "提示");
                    }
                }

            }
        }
        /// <summary>
        /// 导出模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == pagePhysical)
            {
                #region 生成GridControl和GridView

                //GridView
                DevExpress.XtraGrid.Views.Grid.GridView gvExport_Phy = new DevExpress.XtraGrid.Views.Grid.GridView();

                //GridControl
                DevExpress.XtraGrid.GridControl gcExport_Physical = new DevExpress.XtraGrid.GridControl();

                //GridColumn
                DevExpress.XtraGrid.Columns.GridColumn colExport1 = new DevExpress.XtraGrid.Columns.GridColumn();
                DevExpress.XtraGrid.Columns.GridColumn colExport2 = new DevExpress.XtraGrid.Columns.GridColumn();
                DevExpress.XtraGrid.Columns.GridColumn colExport3 = new DevExpress.XtraGrid.Columns.GridColumn();
                DevExpress.XtraGrid.Columns.GridColumn colExport4 = new DevExpress.XtraGrid.Columns.GridColumn();
                DevExpress.XtraGrid.Columns.GridColumn colExport5 = new DevExpress.XtraGrid.Columns.GridColumn();

                //gcExport_Physical
                gcExport_Physical.Name = "gcExport_Physical";
                gcExport_Physical.MainView = gvExport_Phy;
                gcExport_Physical.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
                gvExport_Phy});

                //gvExport_Phy
                gvExport_Phy.Name = "gvExport_Phy";
                gvExport_Phy.GridControl = gcExport_Physical;
                gvExport_Phy.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
                colExport1,colExport2,colExport3,colExport4,colExport5});


                // 
                // colExport1
                // 
                colExport1.Caption = "样本号";
                colExport1.FieldName = "RepSid";
                colExport1.Name = "colExport1";
                colExport1.Visible = true;
                colExport1.VisibleIndex = 0;
                colExport1.Width = 131;
                // 
                // colExport2
                // 
                colExport2.Caption = "姓名";
                colExport2.FieldName = "PidName";
                colExport2.Name = "colExport2";
                colExport2.Visible = true;
                colExport2.VisibleIndex = 1;
                colExport2.Width = 122;
                // 
                // colExport3
                // 
                colExport3.Caption = "性别";
                colExport3.FieldName = "pat_sex_name";
                colExport3.Name = "colExport3";
                colExport3.Visible = true;
                colExport3.VisibleIndex = 2;
                colExport3.Width = 86;
                // 
                // colExport4
                // 
                colExport4.Caption = "年龄";
                colExport4.FieldName = "PatAgeTxt";
                colExport4.Name = "colExport4";
                colExport4.Visible = true;
                colExport4.VisibleIndex = 3;
                colExport4.Width = 86;
                // 
                // colExport5
                // 
                colExport5.Caption = "病人ID";
                colExport5.FieldName = "PidInNo";
                colExport5.Name = "colExport5";
                colExport5.Visible = true;
                colExport5.VisibleIndex = 4;
                colExport5.Width = 101;

                #endregion

                DataTable dtExportPhy = new DataTable("templet");

                dtExportPhy.Columns.Add("RepSid", System.Type.GetType("System.Int32"));//样本号
                dtExportPhy.Columns.Add("PidName", System.Type.GetType("System.String"));//姓名
                dtExportPhy.Columns.Add("PidSex", System.Type.GetType("System.String"));//性别
                dtExportPhy.Columns.Add("PatAgeTxt", System.Type.GetType("System.String"));//年龄
                dtExportPhy.Columns.Add("PidInNo", System.Type.GetType("System.String"));//病人ID

                dtExportPhy.Rows.Add(new object[] { 1, "张三", "男", "18", "10086" });

                gcExport_Physical.DataSource = dtExportPhy;//

                setExcel(gcExport_Physical);//导出模板
            }
        }
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnImportClicked(object sender, EventArgs e)
        {
            OpenFileDialog opendialog = new OpenFileDialog();

            if (opendialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string name = opendialog.FileName;

            try
            {
                //导入Excel数据
                DataSet dsPat_Info = ExcelToDS(name, "dtPat_Info");

                //导入数据是否为空
                if (dsPat_Info.Tables["dtPat_Info"].Rows.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("导入的数据为空!", "提示");
                    return;
                }

                //检查是否存在模板指定的列名
                if (!dsPat_Info.Tables["dtPat_Info"].Columns.Contains("样本号"))
                {
                    lis.client.control.MessageDialog.Show("缺少[样本号]列,请使用模板!", "提示");
                    return;
                }
                else if (!dsPat_Info.Tables["dtPat_Info"].Columns.Contains("姓名"))
                {
                    lis.client.control.MessageDialog.Show("缺少[姓名]列,请使用模板!", "提示");
                    return;
                }
                else if (!dsPat_Info.Tables["dtPat_Info"].Columns.Contains("性别"))
                {
                    lis.client.control.MessageDialog.Show("缺少[性别]列,请使用模板!", "提示");
                    return;
                }
                else if (!dsPat_Info.Tables["dtPat_Info"].Columns.Contains("年龄"))
                {
                    lis.client.control.MessageDialog.Show("缺少[年龄]列,请使用模板!", "提示");
                    return;
                }
                else if (!dsPat_Info.Tables["dtPat_Info"].Columns.Contains("病人ID"))
                {
                    lis.client.control.MessageDialog.Show("缺少[病人ID]列,请使用模板!", "提示");
                    return;
                }

                //检查样本号
                int p_intvalue = 0;
                foreach (DataRow rowPat in dsPat_Info.Tables["dtPat_Info"].Rows)
                {
                    //是否存在非数值型的样本号
                    if (!int.TryParse(rowPat["样本号"].ToString(), out p_intvalue))
                    {
                        string strSamEx = "导入的数据有误!\r\n存在非数值型的[样本号]\r\n" + rowPat["样本号"].ToString();
                        lis.client.control.MessageDialog.Show(strSamEx, "提示");
                        return;
                    }
                    //是否存在相同的样本号
                    if (dsPat_Info.Tables["dtPat_Info"].Select("[样本号]=" + rowPat["样本号"].ToString()).Length > 1)
                    {
                        string strSamEx = "导入的数据有误!\r\n存在相同的[样本号]\r\n" + rowPat["样本号"].ToString();
                        lis.client.control.MessageDialog.Show(strSamEx, "提示");
                        return;
                    }
                }


                if (listPhysical == null)
                {
                    listPhysical = GetPatientTableStructure();
                }

                listPhysical = new List<entity.EntityPidReportMain>();

                //填充导入的数据
                foreach (DataRow rowPat in dsPat_Info.Tables["dtPat_Info"].Rows)
                {
                    //DataRow addRowPhy = dtPhysical.NewRow();
                    entity.EntityPidReportMain addRowPhy = new entity.EntityPidReportMain();
                    addRowPhy.RepSid = (rowPat["样本号"].ToString());
                    addRowPhy.PidName = rowPat["姓名"].ToString();

                    if (rowPat["性别"].ToString() == "1")
                    {
                        addRowPhy.PidSex = "1";
                    }
                    else if (rowPat["性别"].ToString() == "2")
                    {
                        addRowPhy.PidSex = "2";
                    }
                    else if (rowPat["性别"].ToString() == "男")
                    {
                        addRowPhy.PidSex = "1";
                    }
                    else if (rowPat["性别"].ToString() == "女")
                    {
                        addRowPhy.PidSex = "2";
                    }

                    addRowPhy.PatAgeTxt = rowPat["年龄"].ToString();
                    addRowPhy.PidInNo = rowPat["病人ID"].ToString();
                    listPhysical.Add(addRowPhy);
                }

                //绑定数据源
                bsPhysical.DataSource = listPhysical;

            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException(this.GetType().Name, "导入数据", ex.ToString());
                lis.client.control.MessageDialog.Show("导入失败,请使用模板！", "提示");
            }
        }

        /// <summary>
        /// 读取Excel文档
        /// </summary>
        /// <param name="Path">文件名称</param>
        /// <param name="p_tableName">查询数据表的名字</param>
        /// <returns>返回一个数据集</returns>
        public DataSet ExcelToDS(string Path, string p_tableName)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(strConn);
            conn.Open();
            DataTable table = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            string tableName = table.Rows[0]["Table_Name"].ToString();
            string strExcel = "";
            System.Data.OleDb.OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = "select * from [" + tableName + "]";
            myCommand = new System.Data.OleDb.OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, tableName);
            ds.Tables[tableName].TableName = p_tableName;
            return ds;

        }

        private void selectItrTarget_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            this.combineEditor1.ItrID = this.selectItrTarget.valueMember;
            this.combineEditor1.ItrName = this.selectItrTarget.displayMember;
        }
    }

    /// <summary>
    /// 模板
    /// </summary>
    public enum PatientTemplate
    {
        /// <summary> 基本资料模板 </summary>
        Basic,
        /// <summary> 体检模板 </summary>
        Physical,
        /// <summary> 标本模板 </summary>
        Sample
    }
}