using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.wcf;
using DevExpress.XtraGrid.Columns;
using lis.client.control;
using DevExpress.XtraExport;
using dcl.client.common;
using dcl.entity;

namespace dcl.client.dicbasic
{
    public partial class frmCombineView : DevExpress.XtraEditors.XtraForm
    {
        public frmCombineView()
        {
            InitializeComponent();
            this.txtNameFilter.TextChanged += new EventHandler(txtFilter_TextChanged);
            this.txtCodeFilter.TextChanged += new EventHandler(txtCodeFilter_TextChanged);
            this.txtSplit_codeFilter.TextChanged += new EventHandler(txtSplit_codeFilter_TextChanged);
        }


        public frmCombineView(Control parentControl): this()
        {
        
        }

        #region 数据检索


        void txtCodeFilter_TextChanged(object sender, EventArgs e)
        {
            DataFilter();
        }

        void txtFilter_TextChanged(object sender, EventArgs e)
        {
            DataFilter();
        }

        void txtSplit_codeFilter_TextChanged(object sender, EventArgs e)
        {
            DataFilter();
        }

        private void txtCType_TextChanged(object sender, EventArgs e)
        {
            DataFilter();
        }

        void DataFilter()
        {
            if (this.bsCombine.DataSource != null)
            {
                List<EntitySampMergeRule> filterList = new List<EntitySampMergeRule>();
                filterList = listRule.FindAll(i => (i.ComHisName.Contains(this.txtNameFilter.Text) || i.ComName.Contains(this.txtNameFilter.Text))
               && i.ComHisFeeCode.Contains(this.txtCodeFilter.Text)
               && i.ComSplitCode.Contains(this.txtSplit_codeFilter.Text)
               && i.ProName.Contains(this.txtCType.Text));
                this.bsCombine.DataSource = filterList;
            }
        }

        #endregion



        List<EntitySampMergeRule> listRule = new List<EntitySampMergeRule>();
        private void frmCombineView_Load(object sender, EventArgs e)
        {

            ProxyCombineSplitBarCode proxyBarCode = new ProxyCombineSplitBarCode();
            listRule = proxyBarCode.Service.GetAllCombineSplitBarCode();
            this.bsCombine.DataSource = listRule;
        }


        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.bsCombine.DataSource != null
                && this.bsCombine.DataSource is List<EntitySampMergeRule>
                && (this.bsCombine.DataSource as List<EntitySampMergeRule>).Count > 0)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                ofd.FileName = "组合视图查看";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                        return;
                    }

                    try
                    {
                        this.gridViewSingle.ExportToXls(ofd.FileName.Trim());
                        MessageDialog.ShowAutoCloseDialog("导出成功！");
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                MessageDialog.Show("没有可导出的数据！");
            }
        }
    }
}
