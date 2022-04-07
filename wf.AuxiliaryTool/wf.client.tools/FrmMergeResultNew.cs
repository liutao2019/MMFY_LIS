using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.common.extensions;

using dcl.client.common;
using lis.client.control;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.tools
{
    public partial class FrmMergeResultNew : FrmCommon
    {
        List<EntityObrResult> dtTarget;
        List<EntityObrResult> dtSource;
        DataTable dtError = GenerateErrorTable();

        public FrmMergeResultNew()
        {
            InitializeComponent();
            Initialize();

            this.sysToolBar1.OnBtnModifyClicked += new System.EventHandler(this.sysToolBar1_OnBtnModifyClicked);
            this.sysToolBar1.OnBtnSearchClicked += new System.EventHandler(this.sysToolBar1_OnBtnSearchClicked);
            this.sysToolBar1.OnBtnSingleAuditClicked += new System.EventHandler(this.sysToolBar1_OnBtnSingleAuditClicked);
            this.sysToolBar1.BtnClearClick += new System.EventHandler(this.sysToolBar1_BtnClearClick);
            this.sysToolBar1.BtnCopyClick += new System.EventHandler(this.sysToolBar1_BtnCopyClick);

        }

        public void Initialize()
        {
            dateSource.Text = DateTime.Now.Date.ToShortDateString();
            dateTarget.Text = DateTime.Now.Date.ToShortDateString();
            this.sysToolBar1.BtnModify.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.sysToolBar1.BtnModify.Enabled = false;
        }

        private void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            try
            {
                SearchSource();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);
            }
        }

        private void sysToolBar1_BtnCopyClick(object sender, EventArgs e)
        {
            try
            {
                Copy();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);
            }
        }

        private void sysToolBar1_OnBtnModifyClicked(object sender, EventArgs e)
        {
            try
            {
                Merge();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);
            }
        }

        private void sysToolBar1_OnBtnSingleAuditClicked(object sender, EventArgs e)
        {
            try
            {
                SearchTarget();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);
            }
        }

        private void sysToolBar1_BtnClearClick(object sender, EventArgs e)
        {
            Clear();
        }

        private bool CheckSourceInput(ref string message)
        {
            bool success = true;

            if (message == null)
                message = String.Empty;

            if (txtStartNumSource.Text.Trim().Length <= 0)
            {
                message = "请输入源样本起始号!\r\n";
                return false;
            }

            if (txtEndNumSource.Text.Trim().Length <= 0)
            {
                message = "请输入源样本终止号!\r\n";
                return false;
            }

            if (selectItrSource.valueMember == null)
            {
                message = "请输入源仪器号!\r\n";
                return false;
            }

            Int64 start = 0;
            if (!Int64.TryParse(txtStartNumSource.Text, out start))
            {
                message = "源样本起始号请输入数字!\r\n";
                return false;
            }

            Int64 end = 0;
            if (!Int64.TryParse(txtEndNumSource.Text, out end))
            {
                message = "源样本终止号请输入数字!\r\n";
                return false;
            }

            if (txtStartNumSource.Text.Length > 0 && txtEndNumSource.Text.Length > 0)
            {
                if (Int64.Parse(txtStartNumSource.Text) > Int64.Parse(txtEndNumSource.Text))
                {
                    message = "源样本起始号不能大于源样本终止号!\r\n";
                    return false;
                }
            }



            return success;
        }

        /// <summary>
        /// 检查目标条件是否合格
        /// </summary>
        /// <param name="message">出错信息</param>
        /// <returns>True:成功,False:失败</returns>
        private bool CheckTargetInput(ref string message)
        {
            bool success = true;

            if (message == null)
                message = String.Empty;

            if (txtStartNumTarget.Text.Trim().Length <= 0)
            {
                message = "请输入目标样本起始号!\r\n";
                return false;
            }

            if (txtEndNumTarget.Text.Trim().Length <= 0)
            {
                message = "请输入目标样本终止号!\r\n";
                return false;
            }

            if (selectItrTarget.valueMember == null)
            {
                message = "请输入目标仪器号!\r\n";
                return false;
            }

            Int64 start = 0;
            if (!Int64.TryParse(txtStartNumTarget.Text, out start))
            {
                message = "目标起始号请输入数字!\r\n";
                return false;
            }

            Int64 end = 0;
            if (!Int64.TryParse(txtEndNumTarget.Text, out end))
            {
                message = "目标终止号请输入数字!\r\n";
                return false;
            }

            if (txtStartNumTarget.Text.Length > 0 && txtEndNumTarget.Text.Length > 0)
            {
                if (Int64.Parse(txtStartNumTarget.Text) > Int64.Parse(txtEndNumTarget.Text))
                {
                    message = "目标样本起始号不能大于目标样本终止号!\r\n";
                    return false;
                }
            }

            if (txtStartNumTarget.Text.Length > 0 && txtEndNumTarget.Text.Length > 0 && txtStartNumTarget.Text.Length > 0 && txtEndNumTarget.Text.Length > 0)
            {
                if ((Int64.Parse(txtEndNumTarget.Text) - Int64.Parse(txtStartNumTarget.Text)) != (Int64.Parse(txtEndNumTarget.Text) - Int64.Parse(txtStartNumTarget.Text)))
                {
                    message = "目标样本号数与目标样本号数不一致!\r\n";
                    return false;
                }
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

            bool ok1 = CheckSourceInput(ref message);
            bool ok2 = CheckTargetInput(ref message);
            success = ok1 && ok2;
            if (txtStartNumSource.Text.Length > 0 && txtEndNumSource.Text.Length > 0 && txtStartNumTarget.Text.Length > 0 && txtEndNumTarget.Text.Length > 0)
            {
                if ((Int64.Parse(txtEndNumSource.Text) - Int64.Parse(txtStartNumSource.Text)) != (Int64.Parse(txtEndNumTarget.Text) - Int64.Parse(txtStartNumTarget.Text)))
                {
                    message += "源样本与目标样本范围不一致!\r\n";
                    success = false;
                }
            }
            if (message.Length > 0)
            {
                ShowInputError(message);
            }
            return success;
        }

        private static void ShowInputError(String message)
        {
            MessageBox.Show("操作被取消,原因是:\r\n" + message, "提示!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <returns></returns>
        private List<EntityObrResult> DoSourceSearch()
        {
            List<EntityObrResult> listObrResult = new List<EntityObrResult>();

            //换成从 lis_result_originalex 表取
            //List<EntityObrResultOriginalEx> listObrResultOriginalEx = new List<EntityObrResultOriginalEx>();
            try
            {
                EntityMergeResultQC query = GetSourceQuery();

                ProxyMergeResult proxy = new ProxyMergeResult();
                //listObrResult = proxy.Service.GetMergeResult(query);
                listObrResult = proxy.Service.GetSourceResult(query);

                return listObrResult;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            return null;
        }

        private EntityMergeResultQC GetSourceQuery()
        {
            EntityMergeResultQC query = new EntityMergeResultQC();

            query.MatchMode = rgfiltertype.EditValue?.ToString();
            query.RepItrId = this.selectItrSource.valueMember == null ? ("") : this.selectItrSource.valueMember.ToString();
            query.ObrDate = this.dateSource.DateTime.Date;
            query.IdStart = Convert.ToInt64(txtStartNumSource.Text);
            query.IdEnd = Convert.ToInt64(txtEndNumSource.Text);
            return query;
        }

        private List<EntityObrResult> DoSearchTarget()
        {
            EntityMergeResultQC query = GetTargetQuery();

            ProxyMergeResult proxy = new ProxyMergeResult();

            return proxy.Service.GetMergeResult(query);

        }

        private EntityMergeResultQC GetTargetQuery()
        {
            EntityMergeResultQC query = new EntityMergeResultQC();
            query.MatchMode = rgfilterTypeT.EditValue?.ToString();
            query.RepItrId = selectItrTarget.valueMember == null ? ("") : selectItrTarget.valueMember.ToString();
            query.ObrDate = this.dateTarget.DateTime.Date;
            query.IdStart = Convert.ToInt64(txtStartNumTarget.Text);
            query.IdEnd = Convert.ToInt64(txtEndNumTarget.Text);
            return query;
        }

        /// <summary>
        /// 查询目标数据
        /// </summary>
        private void SearchTarget()
        {
            string message = "";
            if (!CheckTargetInput(ref message))
            {
                ShowInputError(message);
                return;
            }
            dtTarget = DoSearchTarget();
            if (dtTarget.Count <= 0)
            {
                MessageDialog.ShowAutoCloseDialog("未查询到数据! ");
                bsTarget.DataSource = dtTarget;
                return;
            }

            bsTarget.DataSource = dtTarget;
        }

        /// <summary>
        /// 查询源数据
        /// </summary>
        private void SearchSource()
        {
            string message = "";
            if (!CheckSourceInput(ref message))
            {
                ShowInputError(message);
                return;
            }
            dtSource = DoSourceSearch();
            if (dtSource.Count <= 0)
            {
                MessageDialog.ShowAutoCloseDialog("未查询到数据! ");
                bsSource.DataSource = dtSource;
                return;
            }

            bsSource.DataSource = dtSource;
        }

        bool IsEmpty(List<EntityObrResult> source)
        {
            if (source == null || source.Count == 0)
                return true;
            else
                return false;
        }

        bool IsEmpty(List<EntityObrResultOriginalEx> source)
        {
            if (source == null || source.Count == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        private void Merge()
        {
            ClearErrorTable();

            if (MessageDialog.Show("是否修改? ", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (!ckSid.Checked && !CheckTxtContent())
                    return;

                if (IsEmpty(dtSource))
                {
                    SearchSource();
                }

                //查询不到提示用户
                if (IsEmpty(dtSource))
                {
                    MessageDialog.Show("无源数据！");
                    return;
                }

                if (IsEmpty(dtTarget))
                {
                    dtTarget = EntityManager<EntityObrResult>.ListClone(dtSource);
                    // lis.client.control.MessageDialog.Show("无目标数据，请查询！");
                    // return;
                }

                HandleData();
                if (isActionSuccess)
                {
                    DataSet dsReturn = null;//base.doDel(dtSource.Copy());
                    dtSource.Clear();

                    DataTable tableRetMessage = dsReturn.Tables["retmessage"];
                    if (tableRetMessage.Rows.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (DataRow row in tableRetMessage.Rows)
                        {
                            sb.Append(row["msg"] + "\r\n");
                        }
                        MessageDialog.Show(sb.ToString());
                    }
                    else
                    {
                        MessageDialog.ShowAutoCloseDialog("操作成功!");
                    }
                }
                else
                    MessageDialog.Show("操作失败!");

                ShowError();
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        private void Copy()
        {
            ClearErrorTable();

            if (MessageDialog.Show("是否复制源数据到目标数据? ", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (!ckSid.Checked && !CheckTxtContent())
                    return;

                //没有源数据则查询
                if (IsEmpty(dtSource))
                {
                    SearchSource();
                }
                //查询不到提示用户
                if (IsEmpty(dtSource))
                {
                    MessageDialog.Show("无源数据！");
                    return;
                }

                ////查询不到提示用户
                if (IsEmpty(dtTarget))
                {
                    dtTarget = EntityManager<EntityObrResult>.ListClone(dtSource);
                }

                ProxyMergeResult proxy = new ProxyMergeResult();
                string strMsg = proxy.Service.MergeResult(GetSourceQuery(), GetTargetQuery(), false, false);

                if (strMsg != string.Empty)
                    MessageDialog.Show(strMsg);
                else
                    MessageDialog.ShowAutoCloseDialog("操作成功!");
                RefleshTarget();
            }
        }

        private void ShowError()
        {
            if (Extensions.IsNotEmpty(dtError))
            {
                FrmError frmError = new FrmError(dtError);
                frmError.ShowDialog();
            }
        }

        private void ClearErrorTable()
        {
            if (dtError != null)
                dtError.Clear();
        }

        bool IsNotEmpty(DataTable source)
        {
            return Extensions.IsNotEmpty(source);
        }

        private static DataTable GenerateErrorTable()
        {
            DataTable error = new DataTable("error");
            error.Columns.Add("instr");
            error.Columns.Add("date");
            error.Columns.Add("sid");
            error.Columns.Add("itm_ecd");
            error.Columns.Add("message");
            return error.Clone();
        }

        /// <summary>
        /// 合并与复制
        /// </summary>
        private DataSet HandleData()
        {
            return null;
            //ProxyResultMergeNew proxy = new ProxyResultMergeNew();
            //return proxy.Service.MergeData(GetSourceQuery(), GetTargetQuery());
        }

        /// <summary>
        /// 刷新目标表
        /// </summary>
        private void RefleshTarget()
        {
            dtTarget = DoSearchTarget();
            if (dtTarget != null && dtTarget.Count > 0)
                bsTarget.DataSource = dtTarget;
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
            //取消病人资料过滤
            //FormatGridView();
            InitButtons();
        }

        private void FormatGridView()
        {
            string[] auditedFlag = new string[]{
                LIS_Const.PATIENT_FLAG.Audited,
             LIS_Const.PATIENT_FLAG.Printed,
             LIS_Const.PATIENT_FLAG.Reported
             };
            FormatHelper.FormatRow(gvTarget, "pat_flag", auditedFlag, Color.Red);
            FormatHelper.FormatRow(gvSource, "pat_flag", auditedFlag, Color.Red);
        }

        /// <summary>
        /// 初始化按钮
        /// </summary>
        private void InitButtons()
        {
            sysToolBar1.BtnSearch.Caption = "源数据";
            sysToolBar1.BtnSingleAudit.Caption = "目标数据";
            sysToolBar1.SetToolButtonStyle(
                new string[] { sysToolBar1.BtnSearch.Name,sysToolBar1.BtnSingleAudit.Name,
                     sysToolBar1.BtnCopy.Name, sysToolBar1.BtnModify.Name,sysToolBar1.BtnClear.Name, sysToolBar1.BtnClose.Name },
                    new string[] { "F3", "F4", "F5", "F6", "F7", "F8" }
                    );
            sysToolBar1.BtnModify.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        /// <summary>
        /// 清空并复位
        /// </summary>
        private void Clear()
        {
            if (!IsEmpty(dtSource))
            {
                dtSource.Clear();
                gvSource.RefreshData();
            }
            if (!IsEmpty(dtTarget))
            {
                dtTarget.Clear();
                gvTarget.RefreshData();
            }

            txtEndNumTarget.Text = txtStartNumSource.Text = txtStartNumTarget.Text = txtEndNumSource.Text = "";

            dateTarget.DateTime = dateSource.DateTime = DateTime.Now.Date;
            selectItrSource.displayMember = selectItrSource.valueMember = "";
            selectItrTarget.displayMember = selectItrTarget.valueMember = "";
        }
       
        private void rgfiltertype_EditValueChanged(object sender, EventArgs e)
        {
            string str = rgfiltertype.EditValue?.ToString();
            if (str == "0")
            {
                layitemBegin.Text = "起始样本";
                layitemEnd.Text = "终止样本";
            }
            else
            {
                layitemBegin.Text = "起始序号";
                layitemEnd.Text = "终止序号";
            }
        }
        

        private void rgfilterTypeT_EditValueChanged(object sender, EventArgs e)
        {
            string str = rgfilterTypeT.EditValue?.ToString();
            if (str == "0")
            {
                layitemBeginT.Text = "起始样本";
                layitemEndT.Text = "终止样本";
            }
            else
            {
                layitemBeginT.Text = "起始序号";
                layitemEndT.Text = "终止序号";
            }
        }
    }
}