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
using dcl.client.cache;

namespace dcl.client.result
{
    public partial class FrmRecheckConfirm : FrmBase
    {
        public FrmRecheckConfirm()
        {
            InitializeComponent();

            //复查备注取报告评价模板内容

        }
        public string RemarkMsg { get { return this.comboBox1.Text; } }
        private string itrPtype;
        public void Init(string patID, string itrtype)
        {
            this.itrWarningMsg1.LoadItrWarningMsg(patID, true);
            this.comboBox1.Focus();
            itrPtype = itrtype;
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(RemarkMsg))
            {
                lis.client.control.MessageDialog.Show("请输入复查意见！");
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void FrmRecheckConfirm_Shown(object sender, EventArgs e)
        {
            List<EntityDicPubEvaluate> listEva = CacheClient.GetCache<EntityDicPubEvaluate>();
            foreach (EntityDicPubEvaluate drBscripe in listEva)
            {
                if (string.IsNullOrEmpty(itrPtype))
                {
                    if (drBscripe.EvaFlag == "20")
                        comboBox1.Items.Add(drBscripe.EvaContent.Trim());
                }
                else
                {
                    //针对物理组进行过滤
                    if (drBscripe.EvaFlag == "20")
                    {
                        if (!string.IsNullOrEmpty(drBscripe.EvaItrId))
                        {
                            if (drBscripe.EvaItrId.Contains(itrPtype))
                                comboBox1.Items.Add(drBscripe.EvaContent.Trim());
                        }
                        else {
                            comboBox1.Items.Add(drBscripe.EvaContent.Trim());
                        }
                    }
                        
                }

            }
            this.comboBox1.SelectedIndex = -1;
        }



    }
}
