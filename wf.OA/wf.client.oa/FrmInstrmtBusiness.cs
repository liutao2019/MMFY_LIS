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
    public partial class FrmInstrmtBusiness : FrmCommon
    {
        public FrmInstrmtBusiness()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保养录入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPrice_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "34";

            orderDetail.LoadData();

            orderDetail.SetCanEdit(UserInfo.HaveFunctionByCode("Fun_offiec_InstrmtBusiness_Edit"), true);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmInstrmtBusiness_Load(object sender, EventArgs e)
        {
            navBarGroup.SelectedLinkIndex = 0;
            BtnPrice_LinkClicked(null, null);
        }

        /// <summary>
        /// 仪器维修
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRequest_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "35";

            orderDetail.LoadData();
        }

        /// <summary>
        /// 保养字典
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnError_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            orderDetail.OrderTypeCode = "36";

            orderDetail.LoadData();
        }
    }
}
