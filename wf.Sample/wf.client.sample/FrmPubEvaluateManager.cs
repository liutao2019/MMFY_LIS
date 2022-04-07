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

namespace dcl.client.sample
{
    public partial class FrmPubEvaluateManager : FrmCommon
    {
        public string result;

        public FrmPubEvaluateManager()
        {
            InitializeComponent();
        }

        string ftype = "";

        /// <summary>
        /// 类别 0-描述(报告评价) 1-处理意见 2-细菌备注 3-描述报告 4-菌落计数 5-质控失控原因
        /// 6-质控解决措施 7-危急类型 8-标本超时签收理由 9-标本超时拒绝理由 10-自编危急范文
        /// 11-细菌临时报告结论 12-危急值信息原因 13-危急值反馈信息 14-交接班字典信息 
        /// 15-危急值记录信息原因 16-临床危急值备注 17-处理结果 18-审核结果 19-院感监测对象
        /// 20-复查意见 21-质控结果分析与处理 22-骨髓象描述  23-保养仪器描述
        /// </summary>
        /// <param name="type"></param>
        public FrmPubEvaluateManager(string type,string result = "")
        {
            InitializeComponent();
            ftype = type;
            this.result = result;
            this.memoEdit1.EditValue = result;

            this.gridView1.DoubleClick += GridView1_DoubleClick;
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            EntityDicPubEvaluate dr = this.bindingSource1.Current as EntityDicPubEvaluate;
            if (dr == null || string.IsNullOrEmpty(dr.EvaContent))
                return;


            if (string.IsNullOrEmpty(memoEdit1.Text))
            {
                this.memoEdit1.EditValue = dr.EvaContent.ToString();
            }
            else
            {
                this.memoEdit1.EditValue += "＋" + dr.EvaContent.ToString();
            }
        }

        private void FrmBscripe_Load(object sender, EventArgs e)
        {
            List<EntityDicPubEvaluate> dtData = CacheClient.GetCache<EntityDicPubEvaluate>();
            dtData = dtData.Where(r => r.EvaFlag == ftype).ToList();
            this.bindingSource1.DataSource = dtData;
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            this.memoEdit1.Text = string.Empty;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.result = this.memoEdit1.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}
