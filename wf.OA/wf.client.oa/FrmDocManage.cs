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
    public partial class FrmDocManage : FrmCommon
    {
        public FrmDocManage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmDocManage_Load(object sender, EventArgs e)
        {
            orderDetail.OrderTypeCode = "38";

            orderMain.OrderTypeCode = "40";
            orderMain.FocusedChanged += new OrderDetail.FocusedChangedEventHandler(orderMain_FocusedChanged);
            orderMain.LoadData();

            orderMain.SetCanEdit(UserInfo.HaveFunctionByCode("Fun_offiec_DocManage_Edit"), true);
            orderDetail.SetCanEdit(UserInfo.HaveFunctionByCode("Fun_offiec_DocManage_Edit"), true);
            
        }

        /// <summary>
        /// 显示分类下的文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void orderMain_FocusedChanged(object sender, EventArgs arg)
        {
            string orderCode = orderMain.GetFocusedOrderCode();

            if (orderCode != "")
            {
                orderDetail.S2 = "OrderDetail";
                orderDetail.S1 = orderCode;
                orderDetail.Enabled = true;
            }
            else
            {
                orderDetail.S2 = "OrderDetail";
                orderDetail.S1 = "-1";
                orderDetail.Enabled = false;
            }

            orderDetail.LoadData();
        }
    }
}
