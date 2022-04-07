using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.client.wcf;
using dcl.entity;
using System.Linq;

namespace dcl.client.msgclient
{
    public partial class FrmBscripe : FrmCommon
    {
        public FrmBscripe()
        {
            InitializeComponent();
        }
        FrmReadAffirm fba = null;
        
        List<EntityDicPubEvaluate> listBscripe = null;

        /// <summary>
        /// 是否通过标本过滤
        /// </summary>
        public bool IsSortSam = false;

        /// <summary>
        /// 样本ID(用于过滤)
        /// </summary>
        public string sam_id { get; set; }

        public string ftype = "";

        public FrmBscripe(FrmReadAffirm fb, string type)
        {
            InitializeComponent();
            fba = fb;
            ftype = type;
        }

        private void FrmBscripe_Load(object sender, EventArgs e)
        {
            this.TopMost = true;

            ProxyCommonDic proxyBscripe = new ProxyCommonDic("svc.ConBscripe");
            EntityResponse result = new EntityResponse();
            List<EntityDicPubEvaluate> listRv = new List<EntityDicPubEvaluate>();
            result = proxyBscripe.Service.Search(new EntityRequest());//获取危急值信息（描述评价信息）
            listRv = result.GetResult() as List<EntityDicPubEvaluate>;

            List<EntityDicPubEvaluate> listTemp = new List<EntityDicPubEvaluate>();
            if(listRv!=null&&listRv.Count>0)
            {
                listTemp = listRv.Where(w => w.EvaFlag == "16").ToList();
            }
            
            this.gridControl1.DataSource = listBscripe = listTemp;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            EntityDicPubEvaluate eyPubevaluate = this.gridView1.GetFocusedRow() as EntityDicPubEvaluate;
            if (eyPubevaluate != null)
            {
                string str = eyPubevaluate.EvaContent;

                if (fba != null)
                {
                    fba.getBscripe(str, ftype);
                }
            }
            this.Close();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            EntityDicPubEvaluate eyPubEvaluate = this.gridView1.GetFocusedRow() as EntityDicPubEvaluate;
            if (!string.IsNullOrEmpty(eyPubEvaluate.EvaContent))
            {
                this.memoEdit1.EditValue = eyPubEvaluate.EvaContent;
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

            if (listBscripe != null && listBscripe.Count > 0)
            {
                string sortText = txtSort.Text;
                sortText = dcl.common.SQLFormater.Format(sortText);

                try
                {
                    List<EntityDicPubEvaluate> listSearch = new List<EntityDicPubEvaluate>();
                    listSearch = listBscripe.Where(w => w.EvaId.Contains(sortText) ||
                                                      w.EvaContent.Contains(sortText.ToUpper()) ||
                                                      w.EvaCcode.Contains(sortText)).ToList();

                    this.gridControl1.DataSource = listSearch;
                }
                catch
                {
                }
            }
        }
    }
}
