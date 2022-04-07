using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using DevExpress.XtraCharts;
using lis.client.control;
using dcl.entity;
using dcl.client.common;
using dcl.client.wcf;
using dcl.client.cache;

namespace dcl.client.qc
{
    public partial class FrmMensurate : FrmCommon
    {
        private List<EntityObrQcResult> dtQc = new List<EntityObrQcResult>();
        string userTypes = "";

        /// <summary>
        /// 上一次操作ID
        /// </summary>
        public string lastOperationID = string.Empty;

        /// <summary>
        /// 上一次操作人密码
        /// </summary>
        public string lastOperationPw = string.Empty;
        public FrmMensurate()
        {
            InitializeComponent();
            login();
            this.ShowSucessMessage = false;
        }

        private void login()
        {
            //DataSet dsMen = base.doView();
            this.bdqcvalue.DataSource = dtQc;
            this.lueLevel.DataSource = CacheClient.GetCache<EntityDicInstrument>();
            this.lueQcItem.DataSource = CacheClient.GetCache<EntityDicItmItem>();
            this.dePicker.EditValue = DateTime.Now;
            dtBegin.EditValue = DateTime.Now.AddDays(-DateTime.Now.Day + 1).Date;
            dtEnd.EditValue = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.Day).Date;
            dtSour = new List<EntityObrQcResult>();
        }

        public FrmMensurate(object deBegin, object deEnd, string[] str)
        {
            InitializeComponent();
            login();

            this.ShowSucessMessage = false;
        }

        public string ItmId { get; set; }

        public string MatSn { get; set; }

        public string ItrId { get; set; }

        public string TypeId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool isFirstShow = false;

        private List<EntityObrQcResult> dtSour { get; set; }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            sysToolBar1.Focus();
            if (this.lue_Apparatus.displayMember == null || (lue_Apparatus.displayMember != null && lue_Apparatus.displayMember == ""))
            {
                lis.client.control.MessageDialog.Show("请输入你要查询的仪器！", "提示");
                return;
            }

            tableBind();
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void tableBind()
        {
            bdqcvalue.EndEdit();

            EntityObrQcResultQC resultQc = new EntityObrQcResultQC();
            resultQc.StateTime = this.dtBegin.DateTime.Date;
            resultQc.EndTime = this.dtEnd.DateTime.Date.AddDays(1).AddMilliseconds(-1);

            //默认勾选行为选中行
            if (isFirstShow)
            {
                List<EntityDicQcMateria> listQcMatAll = this.gcLevel.DataSource as List<EntityDicQcMateria>;
                int rowNum = listQcMatAll.FindIndex(w => w.MatSn == MatSn);
                this.gvLevel.FocusedRowHandle = rowNum;
            }
            isFirstShow = false;

            EntityDicQcMateria drLevel = (EntityDicQcMateria)this.gvLevel.GetFocusedRow();

            if (drLevel != null)
                resultQc.QcParDetailId = drLevel.MatSn;

            if (!string.IsNullOrEmpty(lue_Items.valueMember))
                resultQc.ItemId = lue_Items.valueMember;

            if (!string.IsNullOrEmpty(lue_Apparatus.valueMember))
                resultQc.ItrId = this.lue_Apparatus.valueMember;

            ProxyObrQcResult proxy = new ProxyObrQcResult();

            dtQc = proxy.Service.QcResultQuery(resultQc);

            this.bdqcvalue.DataSource = dtQc;
            dtSour = EntityManager<EntityObrQcResult>.ListClone(dtQc);

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            sysToolBar1.Focus();
            string message = "";

            if (dePicker.EditValue == null)
            {
                lis.client.control.MessageDialog.Show("请输入日期！", "提示");
                return;
            }

            if (this.textEdit1.Text.ToString() == "")
                message = "测定值不能为空！";

            if (this.panel1.Visible == false)
                message = "请点击新增！";

            EntityDicQcMateria drLevel = (EntityDicQcMateria)this.gvLevel.GetFocusedRow();
            if (drLevel == null)
                message = "请选择水平！";

            if (this.lue_Items.displayMember != null && (lue_Items.displayMember != null && lue_Items.displayMember == ""))
                message = "请选择项目！";

            if (this.lue_Apparatus.displayMember != null && (lue_Apparatus.displayMember != null && lue_Apparatus.displayMember == ""))
                message = "请选择仪器！";

            if (message != "")
            {
                lis.client.control.MessageDialog.Show(message, "提示");
                return;
            }

            EntityObrQcResult qcValue = new EntityObrQcResult();// ((EntityObrQcResult)bdqcvalue.AddNew());


            DateTime dtNow = Convert.ToDateTime(this.dePicker.EditValue).Date.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
            qcValue.QresItrId = this.lue_Apparatus.valueMember.ToString();
            qcValue.QresItmId = this.lue_Items.valueMember.ToString();
            qcValue.QresDate = dtNow;
            qcValue.QresValue = this.textEdit1.Text.ToString();
            qcValue.QresLevel = drLevel.MatLevel;
            qcValue.QresMatDetId = drLevel.MatSn;
            qcValue.QresAuditFlag = 0;
            qcValue.QresDisplay = 0;
            qcValue.QresItmX = 0;
            qcValue.QresItmSd = 0;
            qcValue.DelFlag = "0";
            qcValue.QresType = "1";
            qcValue.QresAuditDate = Convert.ToDateTime("1800-01-02");
            ProxyObrQcResult proxyQc = new ProxyObrQcResult();
            bool success = proxyQc.Service.SaveQcResult(qcValue);
            if (success)
            {
                tableBind();
                lis.client.control.MessageDialog.ShowAutoCloseDialog("保存成功！");
            }

            if (rdbData.Checked)
            {
                this.dePicker.EditValue = dtNow.AddDays(1);
            }

            if (rdbItem.Checked)
            {
                try
                {
                    this.gvLevel.MoveNext();
                }
                catch (Exception)
                {
                    lis.client.control.MessageDialog.Show("水平索引已到最大项！", "提示");
                }
            }

            if (rdbItemEcd.Checked)
            {
                //lue_Items.MoveNext();
            }

            this.textEdit1.Text = "";
            this.textEdit1.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.panel1.Visible = true;
            this.textEdit1.Focus();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            this.sysToolBar1.Focus();
            this.bdqcvalue.EndEdit();
            List<EntityObrQcResult> drQcValues = ((List<EntityObrQcResult>)bdqcvalue.DataSource).FindAll(w => w.Checked);//.Select("qcm_select='1'");
            if (drQcValues.Count <= 0)
            {
                lis.client.control.MessageDialog.Show("请勾选要删除的数据", "提示");
                return;
            }

            string strMes = string.Empty;

            string strNotDelet = string.Empty;

            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 删除", "", "", "");

            DialogResult dig = frmCheck.ShowDialog();
            if (dig == DialogResult.OK)
            {
                List<string> listQresSn = new List<string>();

                foreach (EntityObrQcResult drQcValue in drQcValues)
                {
                    if (drQcValue.QresAuditFlag.ToString() == "1")
                    {
                        strMes += "日期：" + drQcValue.QresDate.ToString() + " 数据：" + drQcValue.QresValue.ToString() + " \r\n";
                        continue;
                    }
                    if (UserInfo.GetSysConfigValue("QCDataDelete") == "不允许" && drQcValue.QresType.ToString() == "0")
                    {
                        strNotDelet += "日期：" + drQcValue.QresDate.ToString() + " 数据：" + drQcValue.QresValue.ToString() + " \r\n";
                        continue;
                    }

                    listQresSn.Add(drQcValue.QresSn.ToString());
                }

                bool success = false;
                if (listQresSn.Count > 0)
                {
                    ProxyObrQcResult proxyQc = new ProxyObrQcResult();
                    success = proxyQc.Service.DeleteQcResult(listQresSn);
                }

                if (strMes != string.Empty)
                {
                    lis.client.control.MessageDialog.Show(strMes + "已经审核不允许删除！", "提示");
                    return;
                }
                if (strNotDelet != string.Empty)
                {
                    lis.client.control.MessageDialog.Show(strNotDelet + "仪器传入数据不允许删除！", "提示");
                    return;
                }
                if (success)
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("删除成功！");
                    tableBind();
                }
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmMensurate_Load(object sender, EventArgs e)
        {
            this.dtBegin.DateTime = StartTime;
            this.dtEnd.DateTime = EndTime;

            //需要显示的按钮和顺序
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnSearch", "BtnAdd", "BtnDelete", "BtnSave", sysToolBar1.BtnSinglePrint.Name, "BtnClose" });

            if (UserInfo.GetSysConfigValue("QCDataModify") == "允许")
            {
                if (UserInfo.HaveFunction("dcl.client.qc.FrmChart", "QCUndoAuti") || UserInfo.isAdmin)
                {
                    colqcm_meas.OptionsColumn.AllowEdit = true;
                    qcm_reson.OptionsColumn.AllowEdit = true;
                    qcm_fun.OptionsColumn.AllowEdit = true;
                }
            }

            if (TypeId != null && TypeId != string.Empty)
            {
                lueType.SelectByID(TypeId);
            }
            if (ItrId != null && ItrId != string.Empty)
            {
                lue_Apparatus.SelectByID(ItrId);
            }
            if (ItmId != null && ItmId != string.Empty)
            {
                lue_Items.SelectByID(ItmId);
            }

            if (ItmId != null && ItmId != string.Empty)
            {
                bindLevel();
                btnQuery_Click(null, null);
            }
            //从描述评价字典中加载失控原因和解决措施
            List<EntityDicPubEvaluate> listEvaluate = new List<EntityDicPubEvaluate>();
            listEvaluate = CacheClient.GetCache<EntityDicPubEvaluate>();
            this.repositoryItemLookUpEdit1.DataSource = listEvaluate.FindAll(w => w.EvaFlag == "5"); //失控原因
            this.repositoryItemLookUpEdit2.DataSource = listEvaluate.FindAll(w => w.EvaFlag == "6"); //解决措施
        }

        private void lue_Items_onBeforeFilter(ref string strFilter)
        {
            if (this.lue_Apparatus.valueMember == "" || this.lue_Apparatus.valueMember == null)
            {
                strFilter += "and 1<>1";
            }
            else
            {
                strFilter += "and qcr_itr_id='" + this.lue_Apparatus.valueMember + "'";
            }
        }

        private void lue_Items_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            bindLevel();
        }

        private void bindLevel()
        {
            if (this.lue_Items.valueMember != null && this.lue_Items.valueMember.ToString() != "" && this.lue_Apparatus.valueMember != null && lue_Apparatus.valueMember.Trim().ToString() != "")
            {
                lblItem.Text = lue_Items.displayMember;

                ProxyQcMateria proxyMateria = new ProxyQcMateria();

                List<EntityDicQcMateria> listMateria = proxyMateria.Service.GetMatSnByItem(this.lue_Apparatus.valueMember,dtBegin.DateTime.Date, dtEnd.DateTime.Date, lue_Items.valueMember);

                gcLevel.DataSource = listMateria;
            }
            else
            {
                gcLevel.DataSource = null;
                lblItem.Text = "";
                lblLev.Text = "";
                lblRNo.Text = "";
            }
        }

        private void sysToolBar1_OnBtnSinglePrintClicked(object sender, EventArgs e)
        {
            if (gridControl1 != null)
            {
                gridControl1.Print();
            }
        }

        private void lue_Apparatus_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            this.gcLevel.DataSource = null;
            this.lue_Items.displayMember = null;
            this.lue_Items.valueMember = null;

            if (!string.IsNullOrEmpty(lue_Apparatus.valueMember))
            {
                ProxyQcMateriaDetail proxyDetail = new ProxyQcMateriaDetail();

                List<EntityDicQcMateriaDetail> listDetail = proxyDetail.Service.GetQcMateriaDetailItmId(lue_Apparatus.valueMember);

                List<EntityDicItmItem> listItem = lue_Items.getDataSource();

                listItem = listItem.FindAll(w => listDetail.FindIndex(f => f.MatItmId == w.ItmId) > -1);

                lue_Items.SetFilter(listItem);
            }
            else
            {
                lue_Items.SetFilter(new List<EntityDicItmItem>());
            }

        }

        private void lueType_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            lue_Apparatus.displayMember = "";
            lue_Apparatus.valueMember = "";

            if (!string.IsNullOrEmpty(lueType.valueMember))
            {
                List<EntityDicInstrument> listIns = lue_Apparatus.getDataSource();
                listIns = listIns.FindAll(w => w.ItrLabId == lueType.valueMember);

                if (!UserInfo.isAdmin)
                {
                    listIns = listIns.FindAll(w => UserInfo.entityUserInfo.UserItrsQc.FindIndex(f => f.ItrId == w.ItrId) > -1);
                }

                lue_Apparatus.SetFilter(listIns);
            }
            else
                lue_Apparatus.SetFilter(new List<EntityDicInstrument>());
        }

        private void gvLevel_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            EntityDicQcMateria drLeve = (EntityDicQcMateria)gvLevel.GetFocusedRow();
            if (drLeve != null)
            {
                lblLev.Text = drLeve.MatLevel;
                lblRNo.Text = drLeve.MatBatchNo;
            }

            this.btnQuery_Click(null, null);
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.Name == "colqcm_meas" || e.Column.Name == "qcm_reson" || e.Column.Name == "qcm_fun")
            {

                FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 修改", "", "", "");

                DialogResult dig = DialogResult.Cancel;

                if (lastOperationID != string.Empty && lastOperationPw != string.Empty)
                {
                    bool flag = frmCheck.Valid(lastOperationID, lastOperationPw);
                    if (!flag)
                        dig = frmCheck.ShowDialog();
                    else
                        dig = DialogResult.OK;
                }
                else
                {
                    //frmCheck.txtLoginid.Text = LastOperationID;
                    frmCheck.ActiveControl = frmCheck.txtLoginid;
                    dig = frmCheck.ShowDialog();
                }

                if (dig == DialogResult.OK)
                {
                    this.bdqcvalue.EndEdit();
                    EntityObrQcResult drUpdate = ((EntityObrQcResult)bdqcvalue.Current);

                    ProxyObrQcResult proxyQc = new ProxyObrQcResult();

                    proxyQc.Service.UpdateQcResult(drUpdate);

                    lastOperationID = frmCheck.txtLoginid.Text == string.Empty ? lastOperationID : frmCheck.txtLoginid.Text;
                    lastOperationPw = frmCheck.txtPassword.Text == string.Empty ? lastOperationPw : frmCheck.txtPassword.Text;
                }
                else
                    dtQc = dtSour;
            }
        }

    }
}
