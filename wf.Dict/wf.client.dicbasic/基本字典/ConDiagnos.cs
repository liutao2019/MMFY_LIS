using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using DevExpress.XtraGrid.Columns;
using dcl.entity;
using lis.client.control;
using System.Linq;

namespace dcl.client.dicbasic
{
    public partial class ConDiagnos : ConDicCommon, IBarActionExt
    {


        /// <summary>
        /// 是否为新增事件
        /// </summary>
        bool blIsNew = false;

        List<EntityDicPubIcd> list = new List<EntityDicPubIcd>();

        #region IBarAction 成员

        public void Add()
        {
            this.blIsNew = true;

            foreach(GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }
            buttonEdit7.Focus();
            buttonEdit2.Properties.ReadOnly = true;
            buttonEdit5.Properties.ReadOnly = true;

            EntityDicPubIcd diagnos = bsDiagnos.AddNew() as EntityDicPubIcd;
            diagnos.IcdId = string.Empty;
            diagnos.IcdDelFlag = "0";

            this.gridControl1.Enabled = false;
            buttonEdit7.Focus();
            buttonEdit2.Properties.ReadOnly = true;
            buttonEdit5.Properties.ReadOnly = true;
        }

        public void Save()
        {
            this.bsDiagnos.EndEdit();
            if (bsDiagnos.Current == null)
            {
                return;
            }

            
            EntityDicPubIcd dr = bsDiagnos.Current as EntityDicPubIcd;
            String itm_id = dr.IcdId;

            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dr);

            EntityResponse result = new EntityResponse();
            if (itm_id == "")
            {
                result = base.New(request);
            }
            else
            {
                result = base.Update(request);

            }
            if (base.isActionSuccess)
            {
                if (itm_id == "")
                {
                    dr.IcdId = result.GetResult<EntityDicPubIcd>().IcdId;
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存成功");
                throw new Exception(result.ErroMsg);
            }
            //新增事件处理
            if (blIsNew)
            {
                this.blIsNew = false;
                this.gridControl1.Enabled = true;
            }
            bsDiagnos.ResetCurrentItem();
        }

        public void Delete()
        {
            this.bsDiagnos.EndEdit();
            if (bsDiagnos.Current == null)
            {
                return;
            }

            EntityDicPubIcd dr = bsDiagnos.Current as EntityDicPubIcd;
            String br_id = dr.IcdId;

            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dr);

            
            if (br_id == "")
            {
                return;
            }
            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if(dresult== DialogResult.OK)
            {
                base.Delete(request);
                if (base.isActionSuccess)
                {
                    MessageDialog.ShowAutoCloseDialog("删除成功");
                    DoRefresh();
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("删除成功");
                }
            }
            else
            {

            }
            
        }

        public void DoRefresh()
        {
            EntityResponse result = base.Search(new EntityRequest());
            if (isActionSuccess)
            {
                list = result.GetResult() as List<EntityDicPubIcd>;
                bsDiagnos.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridControl1", true);
            dlist.Add("simpleButton1", true);

            return dlist;
        }
        #endregion
        private DataTable dtHospital = new DataTable();
        private DataTable dtAll = new DataTable();
        public ConDiagnos()
        {
            InitializeComponent();
        }

        private void on_Load(object sender, EventArgs e)
        {
            this.initData();
        }

        private void initData()
        {
            this.DoRefresh();
        }
        SpellAndWbCodeTookit spellcode = new SpellAndWbCodeTookit();
        private void textEdit1_Leave(object sender, EventArgs e)
        {
            if (bsDiagnos.Current != null)
            {
                EntityDicPubIcd diagnos = bsDiagnos.Current as EntityDicPubIcd;
                diagnos.IcdPyCode = spellcode.GetSpellCode(this.textEdit1.Text);
                diagnos.IcdWbCode = spellcode.GetWBCode(this.textEdit1.Text);
            }
        }

        //private void txtSort_EditValueChanged(object sender, EventArgs e)
        //{
        //    if (this.cmbSort.Text != "")
        //    {
        //        if (this.txtSort.Text == "")
        //        {
        //            DoRefresh();
        //        }
        //        else
        //        {
        //            string sortText = txtSort.Text;
        //            sortText = dcl.common.SQLFormater.Format(sortText);
        //            if (this.cmbSort.Text == "编码")
        //            {
        //                DataTable dt = dtHospital.Clone();
        //                DataRow[] dr = dtAll.Select("diag_id like'%" + sortText + "%'");
        //                for (int i = 0; i < dr.Length; i++)
        //                {
        //                    dt.ImportRow(dr[i]);
        //                }
        //                bsDiagnos.DataSource = dtHospital = dt;
        //            }
        //            if (this.cmbSort.Text == "诊断名称")
        //            {
        //                DataTable dt = dtHospital.Clone();
        //                DataRow[] dr = dtAll.Select("diag_diag like'%" + sortText + "%'");
        //                for (int i = 0; i < dr.Length; i++)
        //                {
        //                    dt.ImportRow(dr[i]);
        //                }
        //                bsDiagnos.DataSource = dtHospital = dt;
        //            }
        //        }
        //    }
        //}

        #region IBarActionExt 成员

        public void Cancel()
        {
            //新增事件处理
            if (blIsNew)
            {
                this.blIsNew = false;
                this.gridControl1.Enabled = true;
            }
        }

        public void Close()
        {

        }

        public void Edit()
        {
            this.gridControl1.Enabled = false;
        }

        public void MoveNext()
        {

        }

        public void MovePrev()
        {

        }

        #endregion
        
    }
}
