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

namespace dcl.client.notifyclient
{
    public partial class FrmBscripe : FrmCommon
    {
        public FrmBscripe()
        {
            InitializeComponent();
        }
        FrmReadAffirm fba = null;

        //DataTable dtBscripe = null;
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

            //DataTable dttype = CommonClient.CreateDT(new string[] { "id" }, "dict_bscripe");
            //dttype.Rows.Add(ftype);
            //DataSet dsWhere = new DataSet();
            //dsWhere.Tables.Add(dttype);
            //ProxyMessage proxy = new ProxyMessage();
            //DataSet dsRv = proxy.Service.GetUrgentHistoryMsg(dsWhere);//获取危急值信息

            ProxyCommonDic proxyBscripe = new ProxyCommonDic("svc.ConBscripe");
            EntityResponse result = new EntityResponse();
            List<EntityDicPubEvaluate> listRv = new List<EntityDicPubEvaluate>();
            result=proxyBscripe.Service.Search(new EntityRequest());//获取危急值信息（描述评价信息）
            listRv = result.GetResult() as List<EntityDicPubEvaluate>;

            //DataTable dt_temp = null;
            List<EntityDicPubEvaluate> listTemp = null;
            if (listRv != null && listRv.Count > 0)
            {
                listTemp = listRv;
                //过滤 临床危急值备注信息
                if (listTemp != null && listTemp.Count > 0)
                {
                    //dvtemp.RowFilter = "br_flag='16'";
                    listTemp = listTemp.Where(w => w.EvaFlag == "16").ToList();
                }
            }

            this.gridControl1.DataSource = listBscripe = listTemp;

        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            //DataRow dr = this.gridView1.GetFocusedDataRow();
            EntityDicPubEvaluate eyPubEvl = this.gridView1.GetFocusedRow() as EntityDicPubEvaluate;
            if (eyPubEvl != null)
            {
                //string str = dr["br_scripe"].ToString();
                string str = eyPubEvl.EvaContent;
                if (fba != null)
                {
                    fba.getBscripe(str, ftype);
                }
            }
            this.Close();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //DataRow dr = this.gridView1.GetFocusedDataRow();
            EntityDicPubEvaluate eyPubEvl = this.gridView1.GetFocusedRow() as EntityDicPubEvaluate;
            //if (dr["br_scripe"] != null && dr["br_scripe"] != DBNull.Value)
            if (!string.IsNullOrEmpty(eyPubEvl.EvaContent))
            {
                //this.memoEdit1.EditValue = dr["br_scripe"].ToString();
                this.memoEdit1.EditValue = eyPubEvl.EvaContent;
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
                    //DataRow[] dr = dtBscripe.Select("br_id like '" + sortText + "%' or br_scripe like '%" + sortText + "%' or br_incode like '" + sortText + "%'");
                    List<EntityDicPubEvaluate> listPubEvaSel = listBscripe.Where(w => (w.EvaId.Contains(sortText.ToUpper()) && w.EvaId.IndexOf(sortText) == 1)||
                                                                                       w.EvaContent.Contains(sortText.ToUpper())||
                                                                                      (w.EvaCcode.Contains(sortText.ToUpper())&&w.EvaCcode.IndexOf(sortText.ToUpper())==1)
                                                                                 ).ToList();
                    //for (int i = 0; i < dr.Length; i++)
                    //{
                    //    dt.ImportRow(dr[i]);
                    //}
                    this.gridControl1.DataSource = listPubEvaSel;
                }
                catch
                {
                }
            }
        }
    }
}
