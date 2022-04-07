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
    public partial class FrmDict_An_Sstd : FrmCommon
    {
        public FrmDict_An_Sstd()
        {
            InitializeComponent();
        }
        FrmBacterialInputNew frmBaiNew = null;

        public FrmDict_An_Sstd(FrmBacterialInputNew frmBi)
        {
            InitializeComponent();
            frmBaiNew = frmBi;
        }
        public static object obkey = new object();
        List<EntityDicMicAntidetail> list = new List<EntityDicMicAntidetail>();
        private void FrmDict_An_Sstd_Load(object sender, EventArgs e)
        {
            this.dsBasicDictBindingSource.DataSource = CacheClient.GetCache<EntityDicMicAntitype>();
            this.dsMic.DataSource = CacheClient.GetCache<EntityDicMicAntidetail>().OrderBy(i => i.AntiSortNo).ToList();
            list = CacheClient.GetCache<EntityDicMicAntidetail>();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.dsBasicDictBindingSource.Current != null)
            {
                var dr = (EntityDicMicAntitype)dsBasicDictBindingSource.Current;

                FrmDict_Antibio1 fa = new FrmDict_Antibio1(dr.AtypeId);

                if (frmBaiNew != null)
                {
                    fa.clikcA += new FrmDict_Antibio1.ClikeHander(frmBaiNew.SetTable);
                    fa.Show(frmBaiNew);
                }
                this.Close();
            }


        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            if (dsMic.Current != null)
            {
                EntityDicMicAntidetail dr = (EntityDicMicAntidetail)dsMic.Current;
                dr.ObrRef = "KB";
                ClickEventArgs args = new ClickEventArgs();
                var list = new List<EntityDicMicAntidetail>();
                list.Add(dr);
                args.Antibio2 = list;
                frmBaiNew.SetTable(args);
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
                listMicDetail = listMicDetail.Where(w => w.AntCname != null && w.AntCname.Contains(stWhere) ||
                                                             w.AnsAntiCode != null && w.AnsAntiCode.Contains(stWhere)).ToList();

                this.dsMic.DataSource = listMicDetail.OrderBy(i => i.AntiSortNo).ToList();
            }
        }
    }
}
