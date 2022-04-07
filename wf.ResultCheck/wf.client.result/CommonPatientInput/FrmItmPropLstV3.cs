using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using dcl.root.logon;
using dcl.client.result.CommonPatientInput;
using dcl.client.result.PatControl;
using dcl.client.frame;
using dcl.common;

namespace dcl.client.result
{
    public partial class FrmItmPropLstV3 : FrmCommon
    {

        public FrmItmPropLstV3()
        {
            InitializeComponent();
        }

        private void FrmItmPropLstV3_Load(object sender, EventArgs e)
        {

        }

        private void FrmItmPropLstV3_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            this.Hide();
        }

        /// <summary>
        /// 根据项目ID获取项目特征
        /// </summary>
        /// <param name="itm_id"></param>
        public void SetItemID(string itm_id)
        {
            //DataRow rowItem = DictItem.Instance.GetItem(itm_id);
            //if (rowItem != null)
            //{
            //    this.Text = "当前项目：" + rowItem["itm_ecd"].ToString();
            //    DataTable dtProp = DictItem.Instance.GetItmProp(itm_id);
            //    this.bindingSource1.DataSource = dtProp;
            //}
            //else
            //{
            //    this.Text = "项目特征";
            //    this.bindingSource1.DataSource = null;
            //}

        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            DataRow dr = this.gridView1.GetFocusedDataRow();
            if (dr != null)
            {
                if (this.OnClickSelected != null)
                {
                    this.OnClickSelected(dr["itm_prop"].ToString());
                }
                this.Close();
            }
        }
        internal delegate void ClickSelectedEventHandler(string item_prop);

        /// <summary>
        /// 单击获取当前特征
        /// </summary>
        internal event ClickSelectedEventHandler OnClickSelected;
    }
}
