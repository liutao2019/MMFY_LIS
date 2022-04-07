using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using dcl.client.frame;

namespace lis.client.control
{
    public partial class FrmFilter : FrmCommon
    {
        public string filterString = "";

        public FrmFilter()
        {
            InitializeComponent();
        }

        public FrmFilter(object sourceControl,string filter)
        {
            InitializeComponent();

            filterControl.SourceControl = sourceControl;

            filterString = filter;

            filterControl.FilterString = filter;
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmFilter_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnConfirm", "BtnClose" });
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sysToolBar1_OnBtnConfirmClicked(object sender, EventArgs e)
        {
            txtF.Focus();

            this.DialogResult = DialogResult.OK;

            filterString = filterControl.FilterString;

            this.Close();
        }
    }
}
