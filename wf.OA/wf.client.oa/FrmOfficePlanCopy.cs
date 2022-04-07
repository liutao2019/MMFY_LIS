using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using System.Collections;
using dcl.client.wcf;

namespace dcl.client.oa
{
    public partial class FrmOfficePlanCopy : FrmCommon
    {
        public string TypeID = "";

        public FrmOfficePlanCopy()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmOfficePlanCopy_Load(object sender, EventArgs e)
        {
            //显示工具条
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnCopy.Name, sysToolBar1.BtnClose.Name });

            firstLoad = false;

            SetDTo();
        }

        /// <summary>
        /// 自动设置目的时间段
        /// </summary>
        private void SetDTo()
        {
            if (firstLoad == false)
            {
                if (UserInfo.GetSysConfigValue("Office_dutyTemplateSelect") == "否")
                {
                    TimeSpan span = Convert.ToDateTime(sTo.Text) - Convert.ToDateTime(sFrom.Text);
                    dTo.Text = Convert.ToDateTime(dFrom.Text).Add(span).ToString("yyyy-MM-dd");
                }
                else
                {
                    DateTime dFromMonth = Convert.ToDateTime(dFrom.Text);
                    dTo.Text = dFromMonth.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                }
            }
        }
        bool firstLoad = true;

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnCopyClick(object sender, EventArgs e)
        {
            DateTime tsFrom = Convert.ToDateTime(sFrom.Text + " 00:00:00");
            DateTime tsTo = Convert.ToDateTime(sTo.Text + " 00:00:00");
            DateTime tdFrom = Convert.ToDateTime(dFrom.Text + " 00:00:00");
            DateTime tdTo = Convert.ToDateTime(dTo.Text + " 00:00:00");
            string stFrom = tsFrom.ToString("yyyy-MM-dd 00:00:00");
            string stTo = tsTo.ToString("yyyy-MM-dd 00:00:00");
            string timeFrom=tdFrom.ToString("yyyy-MM-dd 00:00:00");
            string timeTo = tdTo.ToString("yyyy-MM-dd 00:00:00");
            if ((tdFrom >= tsFrom && tdFrom <= tsTo) || (tdTo >= tsFrom && tdTo <= tsTo))
            {
                lis.client.control.MessageDialog.Show("目的时间段和源时间段不能重叠", OfficeMessage.BASE_TITLE);
                return;
            }

            DialogResult dresult = MessageBox.Show("本操作会把当前物理组从'" + sFrom.Text + "'到'" + sTo.Text + "'的排班计划复制到从'" + dFrom.Text + "'到'" + dTo.Text + "',是否确认?", OfficeMessage.BASE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            switch (dresult)
            {
                case DialogResult.OK:
                    {
                        try
                        {
                            ProxyOaShiftDictDetail proxy = new ProxyOaShiftDictDetail();
                            proxy.Service.CopyShiftPlan(stFrom, stTo, timeFrom, timeTo);
                        }
                        catch
                        {
                            return;
                        }

                        break;
                    }
                case DialogResult.Cancel:
                    return;
            }

            if (MessageBox.Show("复制成功,是否把当前操作视图切换到从'" + dFrom.Text + "'到'" + dTo.Text + "'?", OfficeMessage.BASE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
            }

            this.Close();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 自动设置目的时间段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sFrom_TextChanged(object sender, EventArgs e)
        {
            SetDTo();
        }
    }
}
