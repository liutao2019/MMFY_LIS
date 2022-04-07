using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.IO;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.client.common;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.qc
{
    public partial class FrmBscripeSelectV3 : FrmCommon
    {
        public FrmBscripeSelectV3()
        {
            InitializeComponent();
        }

        public delegate string GetCurInstrmtIDEventHandler();
        public event GetCurInstrmtIDEventHandler GetCurInstrmtID = null;

        bool isInit = false;

        string ftype = "";

        public FrmBscripeSelectV3(string type)
        {
            InitializeComponent();
            ftype = type;
        }

        private void FrmBscripe_Load(object sender, EventArgs e)
        {
            string instrmtID = string.Empty;
            if (GetCurInstrmtID != null)
            {
                instrmtID = GetCurInstrmtID();
            }

            List<EntityDicPubEvaluate> dtData = CacheClient.GetCache<EntityDicPubEvaluate>();
            dtData = dtData.Where(r => r.EvaFlag == ftype).ToList();
            List<EntityDicPubEvaluate> desData = new List<EntityDicPubEvaluate>();
            if (!string.IsNullOrEmpty(instrmtID))
            {
                foreach (EntityDicPubEvaluate item in dtData)
                {
                    if (!string.IsNullOrEmpty(item.EvaItrId))
                    {
                        string[] array = item.EvaItrId.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (array.Length > 0)
                        {
                            List<string> list = new List<string>(array);
                            if (list.Contains(instrmtID))
                            {
                                desData.Add(item);
                            }
                        }
                    }
                    else
                    {
                        desData.Add(item);
                    }
                }
            }
            else
            {
                desData = dtData;
            }

            this.bindingSource1.DataSource = desData;

            if (File.Exists(path))
            {
                StreamReader sR = File.OpenText(path);
                string strLine;
                while ((strLine = sR.ReadLine()) != null && !string.IsNullOrEmpty((strLine = sR.ReadLine())))
                {
                    this.memoEdit1.Text = strLine;
                    isInit = true;
                }
                sR.Close();
            }

        }



        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (!isInit)
            {
                EntityDicPubEvaluate dr = this.bindingSource1.Current as EntityDicPubEvaluate;
                if (!string.IsNullOrEmpty(dr.EvaContent))
                {
                    this.memoEdit1.EditValue = dr.EvaContent.ToString();
                }
            }
            else
            {
                isInit = false;
            }
        }

        string path = "C:\\Program Files\\hope\\lis\\" + "QcAnalyseResult.txt";

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            FileStream f = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(f);
            sw.WriteLine(this.memoEdit1.Text);
            sw.Flush();
            sw.Close();
            f.Close();
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.memoEdit1.Text = string.Empty;
        }
    }
}
