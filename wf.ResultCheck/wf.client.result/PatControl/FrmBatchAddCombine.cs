using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.sample;
using dcl.common.extensions;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.result.PatControl
{
    public partial class FrmBatchAddCombine : FrmCommon
    {
        //DataTable combine = new DataTable();
        List<EntityDicCombine> listComb = new List<EntityDicCombine>();
        FrmCombineManager f;
        public List<string> list = null;

        public FrmBatchAddCombine()
        {
            InitializeComponent();
        }

        private void FrmBatchAddCombine_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnConfirm.Name });
            //combine.Columns.Add("com_id");
            //combine.Columns.Add("com_name");
        }

        private void sysToolBar1_OnBtnConfirmClicked(object sender, EventArgs e)
        {
            int pat_sid_begin = 0;
            int pat_sid_end = 0;
            if (string.IsNullOrEmpty(txtStartNumgoal.Text))
            {
                lis.client.control.MessageDialog.Show("请输入开始样本号", "提示");
                return;
            }
            if (string.IsNullOrEmpty(txtEndNumgoal.Text))
            {
                lis.client.control.MessageDialog.Show("请输入结束样本号", "提示");
                return;
            }
            if (!int.TryParse(txtStartNumgoal.Text, out pat_sid_begin))
            {
                lis.client.control.MessageDialog.Show("请输入正确开始样本号", "提示");
                return;
            }
            if (!int.TryParse(txtEndNumgoal.Text, out pat_sid_end))
            {
                lis.client.control.MessageDialog.Show("请输入正确结束样本号", "提示");
                return;
            }
            if (pat_sid_begin > pat_sid_end)
            {
                lis.client.control.MessageDialog.Show("开始样本号不能大于结束样本号", "提示");
                return;
            }
            if (string.IsNullOrEmpty(txtCombineEdit.Text))
            {
                lis.client.control.MessageDialog.Show("请选择需要新增的组合", "提示");
                return;
            }
            List<EntityPatientQC> patientsQcList = new List<EntityPatientQC>();
            if (f != null && f.dtCombine != null && f.dtCombine.Count > 0)
            {
                foreach (EntityDicCombine drCombine in f.dtCombine)
                {
                    EntityPatientQC patientsQc = new EntityPatientQC();

                    patientsQc.ListItrId.Add(list[0]);
                    patientsQc.DateStart = Convert.ToDateTime(list[1]).Date;
                    patientsQc.DateEnd = Convert.ToDateTime(list[1]).AddDays(1).Date;
                    EntitySid sid = new EntitySid();
                    sid.StartSid = Convert.ToInt32(txtStartNumgoal.Text);
                    sid.EndSid = Convert.ToInt32(txtEndNumgoal.Text);
                    patientsQc.ListSidRange.Add(sid);
                    patientsQc.ComName = drCombine.ComName.ToString();
                    patientsQc.ComId = drCombine.ComId.ToString();

                    patientsQcList.Add(patientsQc);
                }
            }
            ProxyReportDetail proxy = new ProxyReportDetail();
            bool result = proxy.Service.BatchAddCombine(patientsQcList);
            if (result)
            {
                lis.client.control.MessageDialog.Show("批量新增组合成功", "提示");
                this.Close();
            }
            else
                lis.client.control.MessageDialog.Show("批量新增组合失败:\r\n" + "未找到样本号", "提示");
        }

        private void txtCombineEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
            {
                //根据当前物理组合仪器专业组过滤

                //当前物理组
                string ctype = list[2];
                //if (this.txtType.valueMember != null)
                //{
                //    ctype = this.txtType.valueMember;
                //}

                f = new FrmCombineManager(listComb, ctype, "");
                f.RefreshCombineTextDemanded += new FrmCombineManager.ResreshCombineTextDemandedEventhandler(f_RefreshCombineTextDemanded);
                f.Location = this.txtCombineEdit.Location;
                //f.Left = this.txtCombineEdit.PointToScreen(new Point(0, 0)).X; // +this.txtCombineEdit.Width; //- f.Width;
                //f.Top = this.txtCombineEdit.PointToScreen(new Point(0, this.txtCombineEdit.Height)).Y;
                f.Left = this.txtCombineEdit.PointToScreen(new Point(0, 0)).X + this.txtCombineEdit.Width - f.Width;
                f.Top = this.txtCombineEdit.PointToScreen(new Point(0, this.txtCombineEdit.Height)).Y;


                f.ShowDialog();

            }
        }

        void f_RefreshCombineTextDemanded(object sender, EventArgs args)
        {
            if (f.dtCombine == null || f.dtCombine.Count == 0)
            {
                txtCombineEdit.Text = "";
                return;
            }

            txtCombineEdit.Text = "";

            foreach (EntityDicCombine row in f.dtCombine)
            {
                txtCombineEdit.Text += "+" + row.ComName.ToString();
            }

            txtCombineEdit.Text = txtCombineEdit.Text.TrimStart('+');
        }
    }
}
