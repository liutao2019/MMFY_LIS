using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using System.Linq;
using dcl.entity;

using lis.client.control;

namespace dcl.client.qc
{
    public partial class FrmCriterion : ConCommon
    {
        #region 全局变量

        ProxyCommonDic proxy = new ProxyCommonDic("svc.FrmCriterion");

        #endregion

        public FrmCriterion()
        {
            InitializeComponent();
            login();
        }

        List<EntityDicQcRule> qc_rule = new List<EntityDicQcRule>();

        private void login()
        {
            EntityResponse ds = proxy.Search(new EntityRequest());
            isActionSuccess = ds.Scusess;
            if (isActionSuccess)
            {
                qc_rule = ds.GetResult() as List<EntityDicQcRule>;
                this.bdqcrule.DataSource = qc_rule;
            }
        }

        #region 关闭按钮
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            // this.Close();
        }
        #endregion

        #region 新增按钮
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //this.bdqcrule.AddNew();
            EntityDicQcRule dr = (EntityDicQcRule)this.bdqcrule.AddNew();
            dr.RulId = string.Empty;
            dr.DelFlag = LIS_Const.del_flag.OPEN;

            txtName.Focus();
            isEnabled(false);
            this.checkBox1.Checked = true;
        }
        #endregion

        #region 修改按钮
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            isEnabled(false);
            txtName.Focus();
        }
        #endregion

        #region 删除按钮
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.bdqcrule.EndEdit();
            if (this.bdqcrule.Current == null)
            {
                lis.client.control.MessageDialog.Show("请选择需删除的数据！", "提示");
                return;
            }
            //DataRow dr = ((DataRowView)bdqcrule.Current).Row;
            //string rule_id = dr["rule_id"].ToString();
            EntityDicQcRule dr = bdqcrule.Current as EntityDicQcRule;
            string rule_id = dr.RulId.ToString();

            //DataSet dtQc = new DataSet();
            //DataTable datb = qc_rule.Clone();
            //datb.TableName = "qc_rule";
            //datb.Rows.Add(dr.ItemArray);
            //dtQc.Tables.Add(datb);
            //DataSet result = new DataSet();

            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dr);

            if (rule_id == "")
            {
                //this.qc_rule.Rows.Remove(dr);
                this.qc_rule.Remove(dr);
            }
            else
            {
                DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                switch (dresult)
                {
                    case DialogResult.OK:
                        //base.doDel(dtQc); 
                        proxy.Delete(request);
                        break;
                    case DialogResult.Cancel:
                        return;

                }

            }
            if (base.isActionSuccess)
            {
                if (rule_id != "")
                {
                    //qc_rule.Rows.Remove(dr);
                    this.qc_rule.Remove(dr);
                }
            }

            login(); //刷新数据
        }
        #endregion

        #region 保存按钮
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            sysToolBar1.Focus();
            this.isActionSuccess = false;

            if (this.bdqcrule.Current == null)
            {
                lis.client.control.MessageDialog.Show("无需保存数据！", "提示");
                return;
            }
            int n = 0;
            int m = 0;
            double sd = 0;
            if (this.txtName.Text.ToString() == "")
            {
                lis.client.control.MessageDialog.Show("请输入质控标准名称！", "提示");
                txtName.Focus();
                return;
            }
            try
            {
                n = Convert.ToInt32(this.txtN.Text);
            }
            catch (Exception)
            {
                lis.client.control.MessageDialog.Show("N值为空或类型错误(请输入整数)！", "提示");
                txtN.Focus();
                return;
            }
            try
            {
                m = Convert.ToInt32(this.txtM.Text);
            }
            catch (Exception)
            {
                lis.client.control.MessageDialog.Show("M值为空或类型错误(请输入整数)！", "提示");
                txtM.Focus();
                return;
            }
            try
            {
                sd = Convert.ToDouble(this.txtSD.Text);
            }
            catch (Exception)
            {
                lis.client.control.MessageDialog.Show("SD值为空或类型错误(请输入数字)！", "提示");
                txtSD.Focus();
                return;
            }
            if (n <= 0 || m <= 0 || sd < 0)
            {
                lis.client.control.MessageDialog.Show("N、M、SD不能小于零(N、M不能等于零)！", "提示");
                return;
            }
            if (m > n)
            {
                lis.client.control.MessageDialog.Show("M不能大于N！", "提示");
                txtM.Focus();
                return;
            }

            if (cmbType.Text == null || cmbType.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("请选择类型！", "提示");
                return;
            }

            if (txtSeq.Value == null || txtSeq.Value.ToString().Trim() == "")
            {
                lis.client.control.MessageDialog.Show("请填入序号！", "提示");
                return;
            }

            EntityDicQcRule dr = bdqcrule.Current as EntityDicQcRule;
            dr.RulLevelType = checkBox1.Checked ? 1 : 0;
            dr.RulIsDesc = ckIsDesc.Checked ? 1 : 0;
            string rule_id = dr.RulId;

            EntityRequest request = new EntityRequest();

            request.SetRequestValue(dr);

            EntityResponse result = new EntityResponse();

            if (rule_id == "")
            {
                result = proxy.New(request);
                base.isActionSuccess = result.Scusess;
            }
            else
            {
                result = proxy.Update(request);
                base.isActionSuccess = result.Scusess;
            }
            if (base.isActionSuccess)
            {
                if (rule_id == "")
                {
                    dr.RulId = result.GetResult<EntityDicQcRule>().RulId;
                }
                MessageDialog.ShowAutoCloseDialog("操作成功!");
            }
            isEnabled(true);

            login();  //刷新数据
            EnabledButton();
        }
        #endregion
        public void EnabledButton()
        {
            sysToolBar1.BtnAdd.Enabled = true;
            sysToolBar1.BtnModify.Enabled = true;
            sysToolBar1.BtnDelete.Enabled = true;
            sysToolBar1.BtnSave.Enabled = false;
            sysToolBar1.BtnCancel.Enabled = false;
        }

        private void isEnabled(bool isture)
        {
            txtName.Properties.ReadOnly = isture;
            txtN.Properties.ReadOnly = isture;
            txtM.Properties.ReadOnly = isture;
            txtSD.Properties.ReadOnly = isture;
            txtSeq.Properties.ReadOnly = isture;
            memoEdit1.Properties.ReadOnly = isture;
            cmbType.Properties.ReadOnly = isture;
            checkBox1.Enabled = !isture;
            ckIsDesc.Enabled = !isture;
            ckMoreLevel.Enabled = !isture;
            gridControl1.Enabled = isture;
        }

        #region 取消按钮
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.bdqcrule.EndEdit();

            //DataTable dt = base.doSearch().Tables["qc_rule"];
            //this.bdqcrule.DataSource = qc_rule = dt;
            EntityResponse dt = proxy.Search(new EntityRequest());
            isActionSuccess = dt.Scusess;
            if (isActionSuccess)
            {
                qc_rule = dt.GetResult() as List<EntityDicQcRule>;
                this.bdqcrule.DataSource = qc_rule;
            }

            isEnabled(true);
        }
        #endregion

        private void FrmCriterion_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnAdd", "BtnModify", "BtnDelete", "BtnSave", "BtnCancel", "BtnClose" });
            isEnabled(true);
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (bdqcrule.Current != null)
            {
                EntityDicQcRule dr = bdqcrule.Current as EntityDicQcRule;
                ckIsDesc.Checked = dr.RulIsDesc.ToString() == "1" ? true : false;
            }
        }
    }
}
