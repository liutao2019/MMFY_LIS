using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.result.PatControl;
using dcl.client.common;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.result
{
    public partial class FrmBscripe : FrmCommon
    {
        public FrmBscripe()
        {
            InitializeComponent();
        }
        FrmBacterialInputNew fbaNew = null;
        List<EntityDicPubEvaluate> dtBscripe = null;

        /// <summary>
        /// 是否通过标本过滤
        /// </summary>
        public bool IsSortSam = false;

        /// <summary>
        /// 样本ID(用于过滤)
        /// </summary>
        public string sam_id { get; set; }

        public string ftype = "";

        public FrmBscripe(FrmBacterialInputNew fb, string type)
        {
            InitializeComponent();
            fbaNew = fb;
            ftype = type;
        }

        private void FrmBscripe_Load(object sender, EventArgs e)
        {
            List<EntityDicPubEvaluate> listEvaluate = CacheClient.GetCache<EntityDicPubEvaluate>().
                FindAll(i => i.EvaFlag == ftype).OrderBy(i => i.EvaSortNo).ToList();


            //是否要通过标本筛选
            if (IsSortSam && (!string.IsNullOrEmpty(sam_id)) && listEvaluate != null && listEvaluate.Count > 0)
            {

                var dtClone = listEvaluate.FindAll(a => string.IsNullOrEmpty(a.EvaSamId) || a.EvaSamId == sam_id);


                this.gridControl1.DataSource = dtBscripe = dtClone;
            }
            else
            {
                this.gridControl1.DataSource = dtBscripe = listEvaluate;
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            EntityDicPubEvaluate dr = this.gridView1.GetFocusedRow() as EntityDicPubEvaluate;
            if (dr != null)
            {
                string str = dr.EvaContent;

                if (fbaNew != null)
                {
                    fbaNew.getBscripe(str, ftype);
                }
            }
            this.Close();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            EntityDicPubEvaluate dr = this.gridView1.GetFocusedRow() as EntityDicPubEvaluate;
            if (dr != null)
            {
                this.memoEdit1.EditValue = dr.EvaContent;
            }
        }

        /// <summary>
        /// 数据检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSort_EditValueChanged(object sender, EventArgs e)
        {
            if (this.txtSort.Text == "" || this.txtSort.Text == null)
            {
                gridControl1.DataSource = dtBscripe;
                return;
            }

            if (dtBscripe != null && dtBscripe.Count > 0)
            {
                string sortText = txtSort.Text;
                sortText = dcl.common.SQLFormater.Format(sortText);

                try
                {
                    this.gridControl1.DataSource = dtBscripe.FindAll(a => a.EvaId.Contains(sortText) || a.EvaContent.Contains(sortText));
                }
                catch
                {
                }
            }
        }
    }
}
