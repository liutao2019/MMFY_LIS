using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.result
{
    public partial class FrmDict_Antibio1 : FrmCommon
    {
        public FrmDict_Antibio1()
        {
            InitializeComponent();
        }
        public static object obkey = new object();

       // DataTable dtSsid = new DataTable();
        public delegate void ClikeHander(ClickEventArgs e);
        public event ClikeHander clikcA;
        List<EntityDicMicAntidetail> list = new List<EntityDicMicAntidetail>();
        //private DataTable dtAnti = CommonClient.CreateDT(new string[] { "bt_id", "anti_id", "anr_res", "anr_res1","anr_st_id","anr_ref",
        //"ss_hstd","ss_mstd","ss_lstd","ss_rzone","ss_izone","ss_szone","ss_type"}, "an_rlts");
        public FrmDict_Antibio1(string id)
        {
            InitializeComponent();
            string sid = id;
            if (sid != "")
            {
                DataTable dt = CommonClient.CreateDT(new string[] { "pam" }, "count");
                dt.Rows.Add(sid);
                 var bt = CacheClient.GetCache<EntityDicMicAntidetail>();

                 list = new List<EntityDicMicAntidetail>();

                foreach(var info in bt)
                {
                    if(info.AnsDefId== sid)
                    {
                        list.Add((EntityDicMicAntidetail)info.Clone());
                    }
                }

                this.dsBasicDictBindingSource.DataSource = list;
            }
        }

        private void FrmDict_Antibio_Load(object sender, EventArgs e)
        {
            //this.dsBasicDictBindingSource.DataSource = bt;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            List<EntityDicMicAntidetail> list = dsBasicDictBindingSource.DataSource as List<EntityDicMicAntidetail>;
            //lis.client.control.MessageDialog.Show("该功能暂未开放，敬请期待！谢谢！", "说明");
            if (list!=null&& list.Count > 0)
            {
                //foreach (DataRow dtRow in dtSsid.Rows)
                //{
                //    DataRow row = dtAnti.Rows.Add("", dtRow["ss_anti_id"].ToString(), "", "", dtRow["ss_st_id"], "",
                //        dtRow["ss_hstd"], dtRow["ss_mstd"], dtRow["ss_lstd"], dtRow["ss_rzone"], dtRow["ss_izone"], dtRow["ss_szone"], "");
                //    row["anr_ref"] = "KB";
                //}
                ClickEventArgs args = new ClickEventArgs();
                args.Antibio2 = list;
                clikcA(args);
            }
            this.Close();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dsBasicDictBindingSource.Current != null)
            {
                EntityDicMicAntidetail dr = (EntityDicMicAntidetail)dsBasicDictBindingSource.Current;
                //DataRow row = dtAnti.Rows.Add("", dr["ss_anti_id"].ToString(), "", "", dr["ss_st_id"], "",
                //        dr["ss_hstd"], dr["ss_mstd"], dr["ss_lstd"], dr["ss_rzone"], dr["ss_izone"], dr["ss_szone"], "");

                //2012-12-28 林 手工选择抗生素的默认为kb法
                dr.ObrRef = "KB";

                ClickEventArgs args = new ClickEventArgs();
                var list = new List<EntityDicMicAntidetail>();
                list.Add(dr);
                args.Antibio2 = list;
                clikcA(args);
            }
        }

        private void txtSort_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicMicAntidetail> listMicDetail = new List<EntityDicMicAntidetail>();
            listMicDetail = list;
            if (this.txtSort.Text == "")
            {
                dsBasicDictBindingSource.Filter = "";
            }
            else
            {
                string stWhere = txtSort.Text.Replace("'", "''");
                listMicDetail = listMicDetail.Where(w =>  w.AntCname != null && w.AntCname.Contains(stWhere) ||
                                                             w.AnsAntiCode!=null && w.AnsAntiCode.Contains(stWhere)).ToList();

                this.dsBasicDictBindingSource.DataSource = listMicDetail;
            }
 

        }


    }
}
