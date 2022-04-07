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
    public partial class FrmStaffManage : FrmCommon
    {
        public FrmStaffManage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体载入时初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmStaffManage_Load(object sender, EventArgs e)
        {
            orderMain.OrderTypeCode = "15";
            orderMain.FocusedChanged += new OrderDetail.FocusedChangedEventHandler(orderMain_FocusedChanged);

            orderMain.LoadData();
            orderMain.SetCanEdit(UserInfo.HaveFunctionByCode("fun_office_StaffManage_Edit"), true);
            orderDetail.SetCanEdit(UserInfo.HaveFunctionByCode("fun_office_StaffManage_Edit"), true);

        }

        private void orderMain_FocusedChanged(object sender, EventArgs arg)
        {
            string orderCode = orderMain.GetFocusedOrderCode();

            if (orderCode != "")
            {
                orderDetail.S2 = "OrderDetail";
                orderDetail.S1 = orderCode;
                pnlDetail.Enabled = true;
                tabControl_SelectedPageChanged(null, null);
            }
            else
            {
                orderDetail.S2 = "OrderDetail";
                orderDetail.S1 = "-1";
                pnlDetail.Enabled = false;
            }
        }

        /// <summary>
        /// 显示明细项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            switch (tabControl.SelectedTabPage.Text)
            {
                case "学历简况":
                    {
                        orderDetail.OrderTypeCode = "16";
                        break;
                    }
                case "外语考级":
                    {
                        orderDetail.OrderTypeCode = "17";
                        break;
                    }
                case "微机培训":
                    {
                        orderDetail.OrderTypeCode = "18";
                        break;
                    }
                case "社会兼职":
                    {
                        orderDetail.OrderTypeCode = "19";
                        break;
                    }
                case "奖罚记录":
                    {
                        orderDetail.OrderTypeCode = "20";
                        break;
                    }
                case "工作经历":
                    {
                        orderDetail.OrderTypeCode = "21";
                        break;
                    }
                case "科研情况":
                    {
                        orderDetail.OrderTypeCode = "22";
                        break;
                    }
                case "论文情况":
                    {
                        orderDetail.OrderTypeCode = "23";
                        break;
                    }
                case "著作情况":
                    {
                        orderDetail.OrderTypeCode = "24";
                        break;
                    }
                case "交流论文":
                    {
                        orderDetail.OrderTypeCode = "25";
                        break;
                    }
                case "继续教育":
                    {
                        orderDetail.OrderTypeCode = "26";
                        break;
                    }
                case "工作轮换":
                    {
                        orderDetail.OrderTypeCode = "27";
                        break;
                    }
                case "学习性质":
                    {
                        orderDetail.OrderTypeCode = "28";
                        break;
                    }
                case "学习安排":
                    {
                        orderDetail.OrderTypeCode = "29";
                        break;
                    }
                case "课题开展":
                    {
                        orderDetail.OrderTypeCode = "30";
                        break;
                    }
                case "论文写作":
                    {
                        orderDetail.OrderTypeCode = "31";
                        break;
                    }
                case "差错事故":
                    {
                        orderDetail.OrderTypeCode = "32";
                        break;
                    }
                case "年终考核":
                    {
                        orderDetail.OrderTypeCode = "51";
                        break;
                    }
                case "教学情况":
                    {
                        orderDetail.OrderTypeCode = "52";
                        break;
                    }
                default:
                    {
                        orderDetail.OrderTypeCode = "16";
                        break;
                    }
            }

            orderDetail.LoadData();
        }
    }
}
