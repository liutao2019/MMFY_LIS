using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using DevExpress.XtraTreeList.Nodes;
using dcl.common;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.qc
{
    public partial class FrmReagentsCompare : FrmCommon
    {
        //全局变量，存放测定仪器过滤之后的值
        private List<EntityDicInstrument> listLueApparatus = new List<EntityDicInstrument>();

        public FrmReagentsCompare()
        {
            InitializeComponent();
            this.tlQcItem.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.tlQcItem_AfterCheckNode);
        }

        private void lueType_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            lue_Apparatus.displayMember = "";
            lue_Apparatus.valueMember = "";
            //测定仪器过滤
            if (lueType.valueMember != null && lueType.valueMember != "")
            {
                if (listLueApparatus.Count > 0)
                {
                    lue_Apparatus.SetFilter(listLueApparatus.FindAll(w => w.ItrLabId == lueType.valueMember));
                }
                else
                {
                    lue_Apparatus.SetFilter(new List<EntityDicInstrument>());
                }
            }
        }


        private void FrmReagentsCompare_Load(object sender, EventArgs e)
        {
            if (!UserInfo.isAdmin)
            {
                if (UserInfo.entityUserInfo.UserItrsQc.Count > 0)
                {
                    listLueApparatus = lue_Apparatus.getDataSource().FindAll(w => UserInfo.entityUserInfo.UserItrsQc.FindIndex(i => i.ItrId == w.ItrId) > -1);
                    lue_Apparatus.SetFilter(listLueApparatus);
                }
                else
                {
                    lue_Apparatus.SetFilter(new List<EntityDicInstrument>());
                }
            }
            else
            {
                listLueApparatus = lue_Apparatus.getDataSource();
            }

            dtBegin.EditValue = DateTime.Now.Date.AddMonths(-1);
            dtEnd.EditValue = DateTime.Now.Date;
            dtCompareDate.EditValue = DateTime.Now.Date;

            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.btnCalculation.Name });
        }

        private void lue_Apparatus_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            loadQcItem();
        }

        private void loadQcItem()
        {
            if (lue_Apparatus.valueMember != null && lue_Apparatus.valueMember != "" && dtBegin.EditValue != null && dtEnd.EditValue != null)
            {
                DataTable dt = CommonClient.CreateDT(new string[] { "itm_id", "sDate", "eDate", "showType" }, "Audit");
                string sDate = this.dtBegin.DateTime.Date.ToString();
                string eDate = this.dtEnd.DateTime.Date.AddDays(1).AddMilliseconds(-1).ToString();
                int showType = 0;
                dt.Rows.Add(lue_Apparatus.valueMember, sDate, eDate, showType);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                DataTable Items = new ProxyObrQcResult().Service.doNew(ds)?.Tables["qc_value"];

                this.tlQcItem.DataSource = Items;

                tlQcItem.ExpandAll();

            }
        }

        private void sysToolBar1_BtnCalculationClick(object sender, EventArgs e)
        {
            if (txtSID.Text == string.Empty || txtSID.Text == "例：1-5.7.9.10-15")
            {
                lis.client.control.MessageDialog.Show("请输入对比样本号！");
                return;
            }

            List<long> lisSid = SampleIDRangeUtil.ToList(txtSID.Text);

            if (lisSid.Count <= 0)
            {
                lis.client.control.MessageDialog.Show("请输入正确的对比样本号！");
                return;
            }

            string strSidWhere = string.Empty;

            for (int i = 0; i < lisSid.Count; i++)
            {
                strSidWhere += ",'" + lisSid[i] + "'";
            }
            strSidWhere = strSidWhere.Remove(0, 1);

            DataTable dtQCWhere = CommonClient.CreateDT(new string[] { "qcm_itm_ecd", "qcm_id", "sdate", "edate" }, "QCWhere");
            DataTable dtCompareData = CommonClient.CreateDT(new string[] { "sdate", "edate", "sid", "itr_id", "itm_id" }, "CompareData");

            for (int j = 0; j < tlQcItem.AllNodesCount; j++)
            {
                TreeListNode tn = tlQcItem.FindNodeByID(j);

                if (tn.Checked && tn["type_type"].ToString() == "1")//判断选中并且选中的为项目
                {
                    dtQCWhere.Rows.Add(tn["qcr_id"].ToString(), tn["type_id"].ToString().Split('&')[0], dtBegin.DateTime.Date, dtEnd.DateTime.Date.AddDays(1).AddMilliseconds(-1));
                    dtCompareData.Rows.Add(dtCompareDate.DateTime.Date, dtCompareDate.DateTime.Date.AddDays(1).AddMilliseconds(-1), strSidWhere, lue_Apparatus.valueMember, tn["qcr_id"].ToString());
                }
            }

            if (dtQCWhere.Rows.Count == 0)
            {
                lis.client.control.MessageDialog.Show("选择质控项目！");
                return;
            }

            ProxyObrQcResult proxy = new ProxyObrQcResult();
            DataTable dtQcCompareData = proxy.Service.QcReagentsCompare(dtQCWhere, dtCompareData);
            gcCompareData.DataSource = dtQcCompareData;
        }

        private void tlQcItem_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            if (e.Node.Checked)
            {
                if (e.Node["type_type"].ToString() != "1")
                    e.Node.Checked = false;
                else
                {
                    for (int j = 0; j < tlQcItem.AllNodesCount; j++)
                    {
                        TreeListNode tn = tlQcItem.FindNodeByID(j);
                        if (tn != e.Node)
                            tn.Checked = false;
                    }
                }
            }
        }
    }
}
