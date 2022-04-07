using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using DevExpress.XtraEditors;
using dcl.client.frame;
using dcl.common;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.result.CommonPatientInput
{
    public partial class FrmItemSelect : FrmCommon
    {
        /// <summary>
        /// 专业组别
        /// </summary>
        public string itm_ptype;


        public string itr_id { get; set; }

        ///// <summary>
        ///// 样本类别
        ///// </summary>
        //private string itm_sam_id;

        public string ReturnItemID { get; private set; }
        public string ReturnItemECD { get; private set; }


        public FrmItemSelect()
        {
            InitializeComponent();
        }
        List<EntityDicItmItem> listItem = new List<EntityDicItmItem>();
        private void FrmItemSelect_Load(object sender, EventArgs e)
        {
            listItem = CacheClient.GetCache<EntityDicItmItem>();
            ResetFilter();
            this.ActiveControl = this.textBox1;
            this.textBox1.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetFilter()
        {
            if (!string.IsNullOrEmpty(itr_id))
            {
                this.bsItem.Filter = string.Empty;

                List<EntityDicItrCombine> listItrComb = CacheClient.GetCache<EntityDicItrCombine>();

                List<EntityDicItrCombine> listInsComs = listItrComb.FindAll(i => i.ItrId == itr_id);

                if (listInsComs.Count == 0)
                    return;

                string[] comIds = listInsComs.Select(i => i.ComId).ToArray();

                List<EntityDicCombineDetail> listCombDetail = CacheClient.GetCache<EntityDicCombineDetail>();

                List<EntityDicCombineDetail> drComItms = (from x in listCombDetail where comIds.Contains(x.ComId) select x).ToList();

                if (drComItms.Count == 0)
                    return;

                string[] itmIds = drComItms.Select(i => i.ComItmId).ToArray();

                this.bsItem.DataSource = (from x in listItem where itmIds.Contains(x.ItmId) select x).ToList();

            }
            else if (!string.IsNullOrEmpty(this.itm_ptype))
            {
                this.bsItem.DataSource = listItem.FindAll(i => i.ItmPriId == this.itm_ptype);
            }
            else
            {
                this.bsItem.DataSource = new List<EntityDicItmItem>();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ResetFilter();
            string strFilter = textBox1.Text;
            if (strFilter == "")
                bsItem.DataSource = listItem;
            else
            {
                bsItem.DataSource = listItem.Where(w => w.ItmId.Contains(strFilter) ||
                                                        w.ItmName.Contains(strFilter) ||
                                                        w.ItmEcode.Contains(strFilter) ||
                                                        w.ItmPyCode.Contains(strFilter.ToUpper()) ||
                                                        w.ItmWbCode.Contains(strFilter.ToUpper())).ToList();
            }
        }

        private void gridControl2_DoubleClick(object sender, EventArgs e)
        {
            ChoosedAndClose();
        }

        private void ChoosedAndClose()
        {
            int[] selectedRow = this.gridView2.GetSelectedRows();
            if (selectedRow.Length > 0)
            {
                EntityDicItmItem dr = this.gridView2.GetRow(selectedRow[0]) as EntityDicItmItem;
                this.ReturnItemID = dr.ItmId;
                this.ReturnItemECD = dr.ItmEcode;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void gridControl2_EditorKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                ChoosedAndClose();
            }
        }

        /// <summary>
        /// 输入框有键盘事件时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13 || e.KeyValue == 40 || e.KeyValue == 38)//回车，向下，向上
            {
                this.gridView2.Focus();
            }
        }

        /// <summary>
        /// 在项目列表按下按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                ChoosedAndClose();
            }
        }
    }
}

