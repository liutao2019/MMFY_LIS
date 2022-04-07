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

namespace dcl.client.oa
{
    public partial class FrmOfficeBusiness : FrmCommon
    {
        public FrmOfficeBusiness()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.FrmOfficeBusiness_Load);
        }

        /// <summary>
        /// 窗体载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmOfficeBusiness_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;

            navBarGroup.SelectedLinkIndex = 0;
            BtnRequest_LinkClicked(null, null);
        }

        /// <summary>
        /// 投诉记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRequest_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "3";

            orderDetail.LoadData();

            orderDetail.SetCanEdit(UserInfo.HaveFunctionByCode("fun_office_OfficeBusiness_Edit"), true);
        }

        /// <summary>
        /// 差错记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnError_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "4";

            orderDetail.LoadData();
        }

        /// <summary>
        /// 科研情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDiscover_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "5";

            orderDetail.LoadData();
        }

        /// <summary>
        /// 论文发表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnArticle_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "6";

            orderDetail.LoadData();
        }

        /// <summary>
        /// 科室大事
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLog_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "7";

            orderDetail.LoadData();
        }

        /// <summary>
        /// 实习生资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStudent_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "9";

            orderDetail.LoadData();
        }

        /// <summary>
        /// 项目成本核算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCharge_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "11";

            orderDetail.LoadData();
        }

        /// <summary>
        /// 新项目登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewProject_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "12";

            orderDetail.LoadData();
        }

        /// <summary>
        /// 方法学变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChange_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "13";

            orderDetail.LoadData();
        }

        /// <summary>
        /// 授课安排
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "14";

            orderDetail.LoadData();
        }

        /// <summary>
        /// 授课情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBarItem6_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "8";

            orderDetail.LoadData();
        }

        private void barConTempSet_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "53";

            orderDetail.LoadData();
        }

    }
}
