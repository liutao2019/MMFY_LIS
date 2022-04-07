using System;
using System.Collections.Generic;
using dcl.client.frame;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.msgsend
{
    public partial class FrmBscripe : FrmCommon
    {
        public FrmBscripe()
        {
            InitializeComponent();
        }
        FrmAddDIYMsg fad = null;
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

        public FrmBscripe(FrmAddDIYMsg fb, string type)
        {
            InitializeComponent();
            fad = fb;
            ftype = type;
        }

        private void FrmBscripe_Load(object sender, EventArgs e)
        {

            List<EntityDicPubEvaluate> dt_temp = CacheClient.GetCache<EntityDicPubEvaluate>().FindAll(r => r.EvaFlag == ftype);
            //是否要通过标本筛选
            if (IsSortSam && (!string.IsNullOrEmpty(sam_id)) && dt_temp != null && dt_temp.Count > 0)
            {
                List<EntityDicPubEvaluate> dtClone = dt_temp.FindAll(r => r.EvaSamId == sam_id);

                this.gridControl1.DataSource = dtBscripe = dtClone;
            }
            else
            {
                this.gridControl1.DataSource = dtBscripe = dt_temp;
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            EntityDicPubEvaluate dr = this.gridView1.GetFocusedRow() as EntityDicPubEvaluate;
            if (dr != null)
            {
                string str = dr.EvaContent.ToString();
                if (fad != null)
                {
                    fad.setMSGRemarks(str);
                }
            }
            this.Close();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            EntityDicPubEvaluate dr = this.gridView1.GetFocusedRow() as EntityDicPubEvaluate;
            if (!string.IsNullOrEmpty(dr.EvaContent))
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
                FrmBscripe_Load(null, null);
                return;
            }

            if (dtBscripe != null && dtBscripe.Count > 0)
            {
                string sortText = txtSort.Text;
                sortText = dcl.common.SQLFormater.Format(sortText);

                try
                {
                    List<EntityDicPubEvaluate> dt = dtBscripe.FindAll(r => r.EvaContent.Contains(sortText) || r.EvaCcode.Contains(sortText) || r.EvaId.Contains(sortText));
                    this.gridControl1.DataSource = dt;
                }
                catch
                {
                }
            }
        }
    }
}
