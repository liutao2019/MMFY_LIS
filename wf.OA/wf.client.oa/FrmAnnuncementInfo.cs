using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using dcl.client.wcf;
using dcl.client.frame;
using dcl.entity;

namespace dcl.client.oa
{
    public partial class FrmAnnuncementInfo : FrmCommon
    {
        public FrmAnnuncementInfo()
        {
            InitializeComponent();
        }

        private void FrmAnnuncementInfo_Load(object sender, EventArgs e)
        {
            sysToolBar.BtnResultView.Caption = "公告管理";
            sysToolBar.BtnConfirm.Caption = "处理";
            sysToolBar.SetToolButtonStyle(new[] { sysToolBar.BtnConfirm.Name,sysToolBar.BtnResultView.Name });

            LoadData();
        }

        private int? annId;
        private void LoadData()
        {
            ProxyOfficeAnnouncement proxy = new ProxyOfficeAnnouncement();

            List<EntityOaAnnouncement> list = proxy.Service.GetLastUnReadAnnouncement(UserInfo.userInfoId);

            if (list.Count == 0)
            {
                Close();
            }
            var row = list[0];
            txtBindingSubject.Text = row.AnctTitle;
            memoEditBody.Text = row.AnctContent;
            txtBindingPublisherName.Text = row.AnctPublishUserName;
            txtBingdingReceiverNames.Text = row.AnctReciverName;
            dateBindingPublishDate.EditValue = row.AnctPublishDate;
            txtBindingType.EditValue = row.AnctType;
            annId = int.Parse(row.AnctId.ToString());

        }

        private void sysToolBar_OnResultViewClicked(object sender, EventArgs e)
        {
            FrmCommon frmCommon = Owner as FrmCommon;
            if (frmCommon != null)
            {
                MethodInfo mi = frmCommon.GetType().GetMethod("LoadForm");
                Close();
                mi.Invoke(frmCommon, new object[] { "科室公告管理", "dcl.client.oa.FrmAnnuncementMgr" });
            }
        }

        private void sysToolBar_OnCloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void sysToolBar_BtnDeRefClick(object sender, EventArgs e)
        {
            if (annId.HasValue)
            {
                ProxyOfficeAnnouncement proxy = new ProxyOfficeAnnouncement();

                proxy.Service.SetReadFlag(UserInfo.userInfoId, annId.Value);
                MessageBox.Show("处理成功");
                Close();

            }
        }
    }
}
