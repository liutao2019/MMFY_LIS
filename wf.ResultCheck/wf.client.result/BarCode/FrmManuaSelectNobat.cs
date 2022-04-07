using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.frame;
using lis.client.control;
using dcl.entity;

namespace dcl.client.result
{
    public partial class FrmManuaSelectNobat : FrmCommon
    {
        List<EntityDicMicSmear> AllNobat = new List<EntityDicMicSmear>();
        public List<string> NobatList { get; set; }

        public FrmManuaSelectNobat()
        {
            InitializeComponent();
            NobatList = new List<string>();
            Shown += FrmResultView_Shown;
        }
        void FrmResultView_Shown(object sender, EventArgs e)
        {
            bsCombine.DataSource = AllNobat;
        }
        public void LoadData(List<EntityDicMicSmear> intable)
        {
            AllNobat = EntityManager<EntityDicMicSmear>.ListClone(intable);
        }
        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmResultView_Load(object sender, EventArgs e)
        {
            barSave.SetToolButtonStyle(new string[] { barSave.BtnConfirm.Name,barSave.BtnClose.Name }, new string[] { "F4" });
            barSave.OnBtnConfirmClicked += new EventHandler(barSave_OnBtnConfirmClicked);
        }

        void barSave_OnBtnConfirmClicked(object sender, EventArgs e)
        {
            gridView1.CloseEditor();
            List<EntityDicMicSmear> table = bsCombine.DataSource as List<EntityDicMicSmear>;
            if (table != null && table.Count>0)
            {
                var rows = table.FindAll(a=>a.isselected==1);
                NobatList = new List<string>();
                if (rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        NobatList.Add(row.SmeName);
                    }
                    table.Clear();
                    DialogResult = DialogResult.Yes;
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("请选择结果！");
                }
            }
        }

        private void txtSort_KeyDown(object sender, KeyEventArgs e)
        {
            gridView1.CloseEditor();
            if (e.KeyCode == Keys.Enter)
            {
                string sort = txtSort.Text.Trim().ToUpper().Replace("'", "''");

                if (sort.Trim() == string.Empty)
                {
                    bsCombine.DataSource = AllNobat;
                }
                else
                {

                    var list = AllNobat.FindAll(a => a.isselected == 1 || a.SmeId.Contains(sort) || a.SmeName.Contains(sort));


                    bsCombine.DataSource = list;
                }

            }
            gdSysLog.RefreshDataSource();
        }

        private void barSave_OnCloseClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

      
    }
}
