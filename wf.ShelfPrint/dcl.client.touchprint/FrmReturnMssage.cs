using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace wf.ShelfPrint
{
    public partial class FrmReturnMssage : Form
    {
        List<EntityTouchPrintData> listMsgData = new List<EntityTouchPrintData>();

        int index = 0;

        public FrmReturnMssage(List<EntityTouchPrintData> listReturn)
        {
            InitializeComponent();
            listMsgData = listReturn;
            if (listReturn.Count > 1)
            {
                pbPrevious.Visible = true;
                pbNext.Visible = true;
            }
            ShowMsgInfo(listReturn[0]);
        }

        private void FrmMssage_Load(object sender, EventArgs e)
        {
            this.Opacity = 0.9;

            string strCount = string.Empty;

            switch (listMsgData.Count)
            {
                case 1:
                    strCount = "一";
                    break;
                case 2:
                    strCount = "二";
                    break;
                case 3:
                    strCount = "三";
                    break;
                case 4:
                    strCount = "四";
                    break;
                case 5:
                    strCount = "五";
                    break;
                case 6:
                    strCount = "六";
                    break;
                case 7:
                    strCount = "七";
                    break;
                case 8:
                    strCount = "八";
                    break;
                case 9:
                    strCount = "九";
                    break;
                default:
                    strCount = listMsgData.Count.ToString();
                    break;
            }

            lblMsg.Text = string.Format("，您有{0}条召回信息。", strCount);
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pbNext_Click(object sender, EventArgs e)
        {
            if (index == (listMsgData.Count - 1))
                index = 0;
            else
                index += 1;

            ShowMsgInfo(listMsgData[index]);
        }

        private void pbPrevious_Click(object sender, EventArgs e)
        {
            if (index == 0)
                index = listMsgData.Count - 1;
            else
                index -= 1;

            ShowMsgInfo(listMsgData[index]);
        }

        private void ShowMsgInfo(EntityTouchPrintData msgData)
        {
            lblName.Text = msgData.PidName;
            lblCom.Text = msgData.PidComName;
            lblTime.Text = string.Empty;

            if (msgData.SampCollectionDate != string.Empty)
            {
                string[] collectionDate = msgData.SampCollectionDate.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                lblTime.Text = collectionDate[0];
            }
        }
    }
}
