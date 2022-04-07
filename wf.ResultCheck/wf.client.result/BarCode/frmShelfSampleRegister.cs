using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.wcf;
using lis.client.control;

using dcl.root.logon;
using System.IO;
using dcl.client.result.CommonPatientInput;
using dcl.common;
using dcl.client.common;
using DevExpress.XtraReports.UI;
using dcl.entity;
using System.Linq;

namespace dcl.client.result
{
    public partial class frmShelfSampleRegister : DevExpress.XtraEditors.XtraForm
    {
        DataTable dtPatientToImportSchema;
        public frmShelfSampleRegister()
        {
            InitializeComponent();

            DateTime today = DateTime.Now;
            this.txt_regdate_from.EditValue = today;
            this.txt_regdate_to.EditValue = today;
            this.txt_pat_date.EditValue = today;

            sysToolBar1.OnBtnSearchClicked += new EventHandler(sysToolBar1_OnBtnSearchClicked);
            sysToolBar1.OnBtnImportClicked += new EventHandler(sysToolBar1_OnBtnImportClicked);
            sysToolBar1.BtnClearClick += new EventHandler(sysToolBar1_BtnClearClick);
            sysToolBar1.OnBtnSaveClicked += new EventHandler(sysToolBar1_OnBtnSaveClicked);
            sysToolBar1.OnBtnPrintListClicked += new EventHandler(sysToolBar1_OnBtnPrintListClicked);

            dtPatientToImportSchema = new DataTable();
            dtPatientToImportSchema.Columns.Add("pat_sid");
            dtPatientToImportSchema.Columns.Add("pat_name");
            dtPatientToImportSchema.Columns.Add("st_no", typeof(int));
            dtPatientToImportSchema.Columns.Add("st_etagere");
            dtPatientToImportSchema.Columns.Add("com_names");
            dtPatientToImportSchema.Columns.Add("com_ids");
            dtPatientToImportSchema.Columns.Add("pat_itr_id");
            dtPatientToImportSchema.Columns.Add("pat_itr_name");
            dtPatientToImportSchema.Columns.Add("pat_date", typeof(DateTime));
            dtPatientToImportSchema.Columns.Add("jy_date", typeof(DateTime));
            dtPatientToImportSchema.Columns.Add("bc_ids");
            dtPatientToImportSchema.Columns.Add("bc_bar_no");
            dtPatientToImportSchema.Columns.Add("type_id");
            dtPatientToImportSchema.Columns.Add("com_count", typeof(int));
            dtPatientToImportSchema.Columns.Add("pat_host_order");
        }

        /// <summary>
        /// 打印清单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sysToolBar1_OnBtnPrintListClicked(object sender, EventArgs e)
        {
            if (this.gridViewLeft.RowCount > 0)
            {
                string resPath = PathManager.ReportPath + @"条码导入清单.repx";
                FileInfo fi = new FileInfo(resPath);

                if (!fi.Exists)
                {
                    MessageDialog.Show("找不到打印报表文件[条码导入清单]", "提示");
                    return;
                }

                try
                {
                    List<EntityPatientsToImport> listSource = this.bsImport.DataSource as List<EntityPatientsToImport>;

                    DataTable dtPrint = new DataTable();
                    dtPrint.Columns.Add("样本号");
                    dtPrint.Columns.Add("姓名");
                    dtPrint.Columns.Add("编号");
                    dtPrint.Columns.Add("标本位置");
                    dtPrint.Columns.Add("组合名称");
                    dtPrint.Columns.Add("仪器名称");
                    dtPrint.Columns.Add("检查日期");
                    dtPrint.Columns.Add("组合ID");
                    dtPrint.Columns.Add("检验时间");


                    foreach (EntityPatientsToImport entitySource in listSource)
                    {
                        DataRow drPrint = dtPrint.NewRow();
                        drPrint["样本号"] = entitySource.RepSid;
                        drPrint["姓名"] = entitySource.PidName;
                        drPrint["编号"] = entitySource.RegNumber;
                        drPrint["标本位置"] = entitySource.RegRackNo;
                        drPrint["组合名称"] = entitySource.ComNames;
                        drPrint["仪器名称"] = entitySource.RepItrName;
                        drPrint["检查日期"] = entitySource.RepInDate;
                        drPrint["组合ID"] = entitySource.ComIds;
                        drPrint["检验时间"] = entitySource.SampCheckDate;

                        dtPrint.Rows.Add(drPrint);
                    }

                    DevExpress.XtraReports.UI.XtraReport rep = new DevExpress.XtraReports.UI.XtraReport();
                    rep.LoadLayout(resPath);
                    rep.DataSource = dtPrint;

                    ReportPrintTool pt = new ReportPrintTool(rep);
                    pt.Print();
                }
                catch (Exception ex)
                {
                    MessageDialog.Show("打印出错！", "提示");
                    Logger.WriteException(this.GetType().Name, "sysToolBar1_OnBtnPrintListClicked", ex.ToString());
                }
            }
            else
            {
                MessageDialog.Show("没有可以打印的数据", "提示");
            }
        }

        /// <summary>
        /// 点击保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            if (this.bsImport.DataSource != null)
            {
                List<EntityPatientsToImport> listData = this.bsImport.DataSource as List<EntityPatientsToImport>;
                if (listData.Count > 0)
                {
                    List<EntityShelfSampToReportMain> listEntity = new List<EntityShelfSampToReportMain>();

                    foreach (EntityPatientsToImport import in listData)
                    {
                        string bar_code = import.SampBarCode;
                        EntityShelfSampToReportMain entity = new EntityShelfSampToReportMain();

                        entity.SampBarCode = import.SampBarCode;
                        entity.ItrId = import.RepItrId;


                        if (import.RepItrName != null)
                        {
                            entity.ItrName = import.RepItrName;
                        }

                        entity.RepInDate = import.RepInDate;
                        entity.SampCheckDate = import.SampCheckDate;
                        entity.RepSid = import.RepSid;
                        entity.ProId = import.ProId;

                        if (!Compare.IsEmpty(import.RepSerialNum))
                        {
                            entity.RepSerialNum = import.RepSerialNum;
                        }
                        else
                        {
                            entity.RepSerialNum = null;
                        }

                        entity.RepCheckUserId = dcl.client.frame.UserInfo.loginID;

                        foreach (string com_id in import.ComIds.Split(','))
                        {
                            entity.ComIds.Add(com_id);
                        }

                        foreach (string com_id in import.DetSn.ToString().Split(','))
                        {
                            entity.DetSns.Add(com_id);
                        }

                        listEntity.Add(entity);
                    }


                    ProxyShelfSampRegister proxy = new ProxyShelfSampRegister();

                    WaitingDailog dia = new WaitingDailog("正在保存，请稍后");
                    dia.Show();
                    Application.DoEvents();
                    EntityResponse response = new EntityResponse();
                    response =  proxy.Service.SavePatients(dcl.client.common.Util.ToCallerInfo(), listEntity);
                    List<EntityOperateResult> listResult = response.GetResult() as List<EntityOperateResult>;
                    string errorMessage = string.Empty;
                    foreach (EntityOperateResult result in listResult)
                    {
                        if (!result.Success)
                        {
                            if (result.Message[0].ErrorCode == EnumOperateErrorCode.SIDExist)
                            {
                                string itr_name = this.txt_pat_itr.displayMember;

                                errorMessage += string.Format("仪器：{0} 标本号：[{1}]已存在\r\n", itr_name, result.Data.Patient.RepSid);
                            }
                            else if (result.Message[0].ErrorCode == EnumOperateErrorCode.HaveReturned)
                            {
                                string itr_name = this.txt_pat_itr.displayMember;

                                errorMessage += string.Format("仪器：{0} 标本号：{1},条码号：[{2}] 已回退\r\n", itr_name, result.Data.Patient.RepSid, result.Data.Patient.RepBarCode);
                            }
                            else if (!result.HasExcaptionError)
                            {
                                errorMessage += string.Format("标本号：[{0}] 姓名：[{1}] 保存失败\r\n", result.Data.Patient.RepSid, result.Data.Patient.PidName);//OperationMessageHelper.GetErrorMessage(result.Message));
                            }
                            else
                            {
                                errorMessage += string.Format("标本号：[{0}] 姓名：[{1}] 保存失败\r\n", result.Data.Patient.RepSid, result.Data.Patient.PidName);
                            }
                        }
                        else
                        {
                        }
                    }


                    sysToolBar1_OnBtnSearchClicked(this.sysToolBar1, EventArgs.Empty);
                    dia.Close();

                    if (errorMessage != string.Empty)
                    {
                        MessageDialog.Show(errorMessage);
                    }
                }
            }
        }

        /// <summary>
        /// 清除左侧列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sysToolBar1_BtnClearClick(object sender, EventArgs e)
        {
            if (lis.client.control.MessageDialog.Show("是否清除左侧列表？", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.bsImport.DataSource = null;
                ResetCombineEditor();
            }
        }

        /// <summary>
        /// 点击查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txt_receive_dept.valueMember))
            {
                ProxyShelfSampRegister proxy = new ProxyShelfSampRegister();

                int? shelfNoFrom = null;
                if (this.txt_shelfnumber_from.Text.Trim() != string.Empty)
                {
                    shelfNoFrom = Convert.ToInt32(this.txt_shelfnumber_from.Text);
                }

                int? shelfNoTo = null;
                if (this.txt_shelfnumber_to.Text.Trim() != string.Empty)
                {
                    shelfNoTo = Convert.ToInt32(this.txt_shelfnumber_to.Text);
                }

                int? seqFrom = null;
                if (this.txt_seq_from.Text.Trim() != string.Empty)
                {
                    seqFrom = Convert.ToInt32(this.txt_seq_from.Text);
                }

                int? seqTo = null;
                if (this.txt_seq_to.Text.Trim() != string.Empty)
                {
                    seqTo = Convert.ToInt32(this.txt_seq_to.Text);
                }
                List<EntitySampRegister> listSamp = new List<EntitySampRegister>();
                listSamp = proxy.Service.GetCuvetteShelfInfo(this.txt_receive_dept.valueMember,
                                                  (DateTime)this.txt_regdate_from.EditValue,
                                                  (DateTime)this.txt_regdate_to.EditValue,
                                                  shelfNoFrom,
                                                  shelfNoTo,
                                                  seqFrom,
                                                  seqTo);

                //***************************************************************************************
                //若已回退的则不显示在右侧的查询结果中
                List<EntitySampRegister> NoneReturnBarCode = new List<EntitySampRegister>();
                foreach (EntitySampRegister samp in listSamp)
                {
                    if (samp.SampStatusId != "9")
                    {
                        NoneReturnBarCode.Add(samp);
                    }
                }
                this.bsSampList.DataSource = NoneReturnBarCode;
                //***************************************************************************************

                RefreshRightLabel();

            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择[接收室]");
            }
        }

        /// <summary>
        /// load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmShelfSampleRegister_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] {
                sysToolBar1.BtnSearch.Name, 
                sysToolBar1.BtnImport.Name,
                sysToolBar1.BtnPrintList.Name,
                sysToolBar1.BtnSave.Name,
                sysToolBar1.BtnClear.Name,
                sysToolBar1.BtnClose.Name
            });
            sysToolBar1.btnCalculation.Caption = "统计";
            sysToolBar1.OnCloseClicked += SysToolBar1_OnCloseClicked;
            dePatDate.EditValue = DateTime.Now;
        }

        private void SysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 右边网格行式样
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewRight_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            DataRow dr = this.gridViewRight.GetDataRow(e.RowHandle);
            if (dr != null)
            {
                if (dr["bc_flag"] != DBNull.Value && !string.IsNullOrEmpty(dr["bc_flag"].ToString()))
                {
                    int flag = Convert.ToInt32(dr["bc_flag"]);
                    if (flag == 1)
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                    else
                    {
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                else
                {
                    e.Appearance.ForeColor = Color.Black;
                }
            }

        }

        /// <summary>
        /// 点击组合框按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCombineEdit_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Down)
            {
                if (this.bsSampList.DataSource != null)
                {
                    List<EntityDicCombine> listCombine = GetAvalibleCombine();

                    if (frmCombineViewer == null)
                    {
                        frmCombineViewer = new frmShelfSampleRegister_AvalibleCombine(this);
                    }

                    frmCombineViewer.LoadCombine(listCombine);

                    if (frmCombineViewer.Visible == false)
                    {
                        frmCombineViewer.Left = this.txtCombineEdit.PointToScreen(new Point(0, 0)).X;
                        frmCombineViewer.Top = this.txtCombineEdit.PointToScreen(new Point(0, this.txtCombineEdit.Height)).Y;
                        frmCombineViewer.Visible = true;
                    }
                }
            }
            else if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
            {
                ResetCombineEditor();
            }
        }

        /// <summary>
        /// 获取未导入数据的项目
        /// </summary>
        /// <returns></returns>
        public List<EntityDicCombine> GetAvalibleCombine()
        {
            List<EntityDicCombine> listCom = new List<EntityDicCombine>();
            List<EntitySampRegister> listShelfInfo = this.bsSampList.DataSource as List<EntitySampRegister>;


            Dictionary<string, string> currentSelectCombine = new Dictionary<string, string>();
            if (this.txtCombineEdit.Tag != null)
            {
                currentSelectCombine = this.txtCombineEdit.Tag as Dictionary<string, string>;
            }

            #region 如果所选仪器室双向仪器，需要判断组合在双向仪器上可做
            List<EntityDicItrCombine> listInsCom = new List<EntityDicItrCombine>();
            if (ckTwoWay.Checked)
            {
                ProxyShelfSampRegister proxy = new ProxyShelfSampRegister();
                listInsCom = proxy.Service.GetItrCombine(txt_pat_itr.valueMember,true);
            }
            #endregion
            if (listShelfInfo != null && listShelfInfo.Count > 0)
            {
                foreach (EntitySampRegister entityShelfInfo in listShelfInfo)
                {
                    if (!string.IsNullOrEmpty(entityShelfInfo.LisComId))
                    {
                        string com_id = entityShelfInfo.LisComId;
                        string com_name = entityShelfInfo.ComName;

                        if (!DictExistKey(currentSelectCombine, com_id) && entityShelfInfo.SampFlag.ToString() != "1")
                        {
                            if (ckTwoWay.Checked)
                            {
                                if (listInsCom != null && listInsCom.Where(w => w.ComId == com_id).Count() > 0)
                                {
                                    if (listCom.FindAll(w => w.ComId == com_id).Count == 0)
                                    {
                                        EntityDicCombine entityCom = new EntityDicCombine();
                                        entityCom.ComId = com_id;
                                        entityCom.ComName = com_name;
                                        listCom.Add(entityCom);
                                    }
                                }
                            }
                            else
                            {
                                if (listCom.FindAll(w => w.ComId == com_id).Count == 0)
                                {
                                    EntityDicCombine entityCom = new EntityDicCombine();
                                    entityCom.ComId = com_id;
                                    entityCom.ComName = com_name;
                                    listCom.Add(entityCom);
                                }
                            }
                        }
                    }
                }
            }
            return listCom;
        }

        private bool DictExistKey(Dictionary<string, string> dict, string key)
        {
            bool exist = false;

            foreach (string item in dict.Keys)
            {
                if (item == key)
                {
                    exist = true;
                    break;
                }
            }

            return exist;
        }

        frmShelfSampleRegister_AvalibleCombine frmCombineViewer = null;

        /// <summary>
        /// 清空组合选择框
        /// </summary>
        private void ResetCombineEditor()
        {
            this.txtCombineEdit.Tag = null;
            this.txtCombineEdit.Text = string.Empty;
        }

        /// <summary>
        /// 点击导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sysToolBar1_OnBtnImportClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_pat_dept.valueMember))
            {
                lis.client.control.MessageDialog.Show("请选择[组别]");
                this.ActiveControl = this.txt_pat_dept;
                this.txt_pat_dept.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.txt_pat_itr.valueMember))
            {
                lis.client.control.MessageDialog.Show("请选择[仪器]");
                this.ActiveControl = this.txt_pat_itr;
                this.txt_pat_itr.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.txt_start_sid.Text))
            {
                lis.client.control.MessageDialog.Show("请输入[开始样本号]");
                this.ActiveControl = this.txt_start_sid;
                this.txt_start_sid.Focus();
                return;
            }

            if (this.txtCombineEdit.Tag == null || (this.txtCombineEdit.Tag as Dictionary<string, string>).Count == 0)
            {
                lis.client.control.MessageDialog.Show("请选择[检验项目]");
                this.ActiveControl = this.txtCombineEdit;
                this.txt_start_sid.Focus();
                return;
            }

            DateTime jy_date = Convert.ToDateTime(dePatDate.EditValue);
            int importType = 0;
            if (ckSidContinuous.Checked)
                importType = 0;
            if (ckSidNoContinuous.Checked)
                importType = 1;
            if (ckTwoWay.Checked)
                importType = 2;
            List<EntityPatientsToImport> listImport = dtGetImportData(this.txt_pat_dept.valueMember,
                this.txt_pat_itr.valueMember,
                this.txt_pat_itr.displayMember,
                (DateTime)this.txt_pat_date.EditValue,
                jy_date,
                Convert.ToInt32(this.txt_start_sid.Text),
                importType
                );
            this.bsImport.DataSource = listImport;

        }

        /// <summary>
        /// 计算得出要导入数据
        /// </summary>
        /// <param name="jy_date"></param>
        /// <returns></returns>
        private List<EntityPatientsToImport> dtGetImportData(string type_id, string itr_id, string itr_name, DateTime pat_date, DateTime jy_date, int begin_sid, int importType)
        {
            List<EntityPatientsToImport> listImport = new List<EntityPatientsToImport>(); ;

            if (this.bsSampList.DataSource != null)
            {
                listImport = listImport.OrderBy(w => w.RegNumber).ToList();
                List<EntitySampRegister> listSource = bsSampList.DataSource as List<EntitySampRegister>;
                listSource = listSource.Where(w => w.SampFlag != 1 || w.SampFlag.ToString() == null).ToList();
                Dictionary<string, string> listComid = this.txtCombineEdit.Tag as Dictionary<string, string>;

                #region 生成非完全匹配导入数据
                int currSid = begin_sid;
                foreach (string com_id in listComid.Keys)//遍历组合框选中的项目
                {
                    //在右侧病人列表中搜索符合项目id的记录
                    List<EntitySampRegister> drsSource = listSource.Where(w => w.LisComId == com_id).ToList();

                    foreach (EntitySampRegister Source in drsSource)//遍历搜索出来的结果
                    {
                        string bc_bar_no = Source.RegBarCode;

                        //在导入数据中查找当前的条码号是否已存在
                        List<EntityPatientsToImport> drsImport = listImport.Where(w => w.SampBarCode == bc_bar_no).ToList();

                        //已存在则把当前条码号的病人的检验组合累加进去；不存在则插入一条记录
                        if (drsImport.Count > 0)
                        {
                            EntityPatientsToImport drImport = drsImport[0];
                            if (drImport.ComIds != null)
                            {
                                drImport.ComIds = drImport.ComIds.ToString() + "," + com_id;
                            }
                            else
                            {
                                drImport.ComIds = com_id;
                            }

                            if (drImport.ComNames != null)
                            {
                                drImport.ComNames = drImport.ComNames + "," + listComid[com_id];
                            }
                            else
                            {
                                drImport.ComNames = listComid[com_id];
                            }

                            if (drImport.DetSn.ToString() != null )
                            {
                                drImport.DetSn = drImport.DetSn + "," + Source.DetSn;
                            }
                            else
                            {
                                drImport.DetSn= Source.DetSn;
                            }

                            drImport.ComCount= drImport.ComCount + 1;

                        }
                        else
                        {
                            EntityPatientsToImport drImport =new EntityPatientsToImport();
                            drImport.ComIds = com_id;
                            drImport.ComNames= listComid[com_id];
                            drImport.PidName = Source.PidName;
                            drImport.RegNumber= Source.RegNumber;
                            drImport.RegRackNo= string.Format("{0}x{1}x{2}", Source.RegRackNo.ToString(), Source.RegXPlace.ToString(), Source.RegYPlace.ToString());
                            drImport.RepItrId= itr_id;
                            drImport.RepItrName = itr_name;
                            drImport.RepInDate = pat_date;
                            drImport.SampCheckDate = jy_date;
                            drImport.SampBarCode = bc_bar_no;
                            drImport.ProId = type_id;
                            drImport.DetSn = Source.DetSn;
                            drImport.ComCount = 1;

                            if (ckTwoWay.Checked)
                            {
                                drImport.RepSerialNum = Source.RegNumber.ToString();
                                drImport.RepSid = Source.RegBarCode;
                            }
                            else
                            {
                                drImport.RepSid= currSid.ToString();
                                drImport.RepSerialNum= null;
                            }

                            listImport.Add(drImport);
                            currSid = currSid + 1;
                        }
                    }
                }


                #endregion

                if (this.chkMatch.Checked)//完全匹配
                {
                    //遍历导入数据中的每一条记录
                    for (int i = listImport.Count - 1; i >= 0; i--)
                    {
                        //当前记录的检验组合
                        string[] coms = listImport[i].ComIds.Split(',');

                        //检查当前记录的检验组合是否与组合框的组合一致
                        int existedCom_id_Count = 0;
                        foreach (string needCom_id in listComid.Keys)
                        {
                            foreach (string existedCom_id in coms)
                            {
                                if (needCom_id == existedCom_id)
                                {
                                    existedCom_id_Count++;
                                    break;
                                }
                            }
                        }

                        //不一直则把当前记录去除
                        if (existedCom_id_Count < listComid.Keys.Count)
                        {
                            listImport.Remove(listImport[i]);
                        }
                    }
                }
                List<EntityPatientsToImport> listImport2 = listImport.OrderBy(w => w.RegNumber).ToList();

                if (importType == 0 && listImport2.Count > 0)//生成不连续样本号
                {
                    int number = Convert.ToInt32(listImport2[0].RegNumber);//第一行序号
                    string sid = listImport2[0].RepSid;//第一行样本号

                    int int_sid = Convert.ToInt32(sid);

                    //相差值
                    int mon = int_sid - number;

                    foreach (EntityPatientsToImport drImport in listImport2)
                    {
                        drImport.RepSid = (mon + Convert.ToInt32(drImport.RegNumber)).ToString();
                    }
                }
                else if (importType == 1)//生成连续样本号
                {

                    foreach (EntityPatientsToImport drImport in listImport2)
                    {
                        string str_begin_sid = begin_sid.ToString();
                        drImport.RepSid = str_begin_sid;

                        begin_sid++;
                    }
                }
                return listImport2;
            }
            return listImport;
        }

        private void txt_receive_dept_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
        }

        private void RefreshRightLabel()
        {
            int total = 0;
            int unreg = 0;
            int reg = 0;
            if (this.bsSampList.DataSource != null)
            {
                List<EntitySampRegister> dt = this.bsSampList.DataSource as List<EntitySampRegister>;

                total = dt.Count;
                object obj = dt.Where(w=>w.SampFlag==1).Count();
                reg = Convert.ToInt32(obj);
                unreg = total - reg;
            }

            this.lbl_right_totalrec.Text = "总数：" + total.ToString();
            this.lbl_right_reg.Text = "已录数：" + reg.ToString();
            this.lbl_right_unreg.Text = "未录数：" + unreg.ToString();
        }

        private void tmDate_Tick(object sender, EventArgs e)
        {
            dePatDate.EditValue = DateTime.Now;
        }

        private void lockDate_CheckedChanged(object sender, EventArgs e)
        {
            if (lockDate.Checked)
                tmDate.Stop();
            else
                tmDate.Start();
        }

        private void txt_pat_itr_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            EntityDicInstrument Itr = txt_pat_itr.selectRow;
            if (Itr != null && txt_pat_itr.valueMember != null && txt_pat_itr.valueMember.Trim() != ""
                && !string.IsNullOrEmpty(Itr.ItrHostType.ToString())  && Itr.ItrHostType.ToString() == "1")
            {
                ckTwoWay.Enabled = false;
                if (ckTwoWay.Checked)
                    ckSidNoContinuous.Checked = true;
            }
            else
                ckTwoWay.Enabled = true;
        }

        private void ckTwoWay_CheckedChanged(object sender, EventArgs e)
        {
            txt_start_sid.Properties.ReadOnly = ckTwoWay.Checked;
        }

        private void txt_pat_dept_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(txt_pat_dept.valueMember))
            {
                txt_pat_itr.SetFilter(txt_pat_itr.getDataSource().FindAll(w => w.ItrLabId == txt_pat_dept.valueMember));
            }
            else
            {
                txt_pat_itr.SetFilter(txt_pat_itr.getDataSource());
            }
        }
    }
}
