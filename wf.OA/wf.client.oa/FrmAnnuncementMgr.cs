using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using dcl.client.wcf;
using dcl.client.frame;

using lis.client.control;
using dcl.entity;
using System.Linq;

namespace dcl.client.oa
{
    public partial class FrmAnnuncementMgr : FrmCommon
    {
        #region 全局变量

        private Dictionary<string, object> allUserBindingData;
        private List<EntitySysUser> userData;
        List<EntityOaAnnouncement> listAnn = new List<EntityOaAnnouncement>();
        #endregion

        #region 构造函数与窗体加载

        public FrmAnnuncementMgr()
        {
            InitializeComponent();
        }

        private void FrmAnnuncementMgr_Load(object sender, EventArgs e)
        {
            sysToolBar.BtnSave.Caption = "发布";
            sysToolBar.SetToolButtonStyle(new[]
                {
                    sysToolBar.BtnSearch.Name, sysToolBar.BtnAdd.Name, sysToolBar.BtnSave.Name, sysToolBar.BtnCancel.Name,
                    sysToolBar.BtnDelete.Name
                });
            sysToolBar.BtnSave.Enabled = false;

            DatePublishDateFrom.EditValue = DateTime.Now.AddMonths(-1).Date;
            DatePublishDateTo.EditValue = DateTime.Now;
            SetEditingState(false);

            LoadData();
        }

        /// <summary>
        /// 控制所有输入框状态
        /// </summary>
        /// <param name="enable"></param>
        private void SetEditingState(Boolean enable)
        {
            txtBindingSubject.Properties.ReadOnly = !enable;
            txtBingdingReceiverNames.Properties.ReadOnly = !enable;
            memoEditBody.Properties.ReadOnly = !enable;
        }

        /// <summary>
        /// 用户加载相关
        /// </summary>
        private void LoadUserData()
        {
            ProxyOfficeAnnouncement proxy = new ProxyOfficeAnnouncement();

            allUserBindingData = proxy.Service.GetAllUserBindingData().GetResult() as Dictionary<string, object>;

            userData = allUserBindingData["AllUser"] as List<EntitySysUser>;
            bsPub.DataSource = allUserBindingData["PowerUserInfo"];
            // tvRoleUser.DataSource = allUserBindingData["PowerRoleUser"];
            tvDepartUser.DataSource = allUserBindingData["PowerUserDepart"];
            bsRole.DataSource = allUserBindingData["PowerRoleUser"];


            bindingSourceUser.DataSource = null;
            //bindingSourceUser.DataSource = userData.Copy();
            bindingSourceUser.DataSource = userData;
        }

        #endregion

        #region 按钮事件

        private void sysToolBar_OnBtnAddClicked(object sender, EventArgs e)
        {
            LoadUserData();
            SetEditingState(true);

            txtBindingSubject.Text = "";
            memoEditBody.Text = "";
            txtBindingPublisherName.Text = UserInfo.userName;
            txtBingdingReceiverNames.Text = "";
            dateBindingPublishDate.EditValue = DateTime.Now;

            txtBindingSubject.Focus();
        }
        public void EnableButton(bool enable)
        {
            sysToolBar.BtnSave.Enabled = enable;
            sysToolBar.BtnAdd.Enabled = !enable;
            sysToolBar.BtnDelete.Enabled = !enable;
            sysToolBar.BtnCancel.Enabled = enable;
        }
        private void sysToolBar_OnBtnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBindingSubject.Text))
                {
                    MessageDialog.Show("标题不能为空");
                    txtBindingSubject.Focus();
                    return;
                }
                List<string> reveiverList = ((List<string>)txtBingdingReceiverNames.Tag);
                if (txtBingdingReceiverNames.Tag == null || reveiverList.Count == 0)
                {
                    MessageDialog.Show("接收人不能为空");
                    return;
                }

                if (string.IsNullOrEmpty(memoEditBody.Text))
                {
                    MessageDialog.Show("公告内容不能为空");
                    memoEditBody.Focus();
                    return;
                }
                EntityOaAnnouncement entity = new EntityOaAnnouncement();

                entity.AnctPublishDate = DateTime.Now;
                entity.AnctPublishUserId = UserInfo.userInfoId;
                entity.AnctPublishUserName = UserInfo.userName;
                entity.AnctTitle = txtBindingSubject.Text;
                entity.AnctReciverName = txtBingdingReceiverNames.Text;
                entity.AnctContent = memoEditBody.Text;
                entity.AnctType = int.Parse(UserInfo.userInfoId) > 0 ? "用户消息" : "系统消息";

                ProxyOfficeAnnouncement proxy = new ProxyOfficeAnnouncement();

                int intrect = proxy.Service.SaveAnnouncementData(entity, reveiverList);
                if (intrect > 0)
                {
                    SetEditingState(false);
                    EnableButton(false);
                    List<EntityOaAnnouncement> returnList = proxy.Service.GetSingleAnnouncementData(UserInfo.userInfoId, intrect);
                    List<EntityOaAnnouncement> list = new List<EntityOaAnnouncement>();
                    foreach (EntityOaAnnouncement ann in returnList)
                    {
                        list = bindingSourceAnn.DataSource as List<EntityOaAnnouncement>;
                        list.Add(ann);
                    }
                    bindingSourceAnn.DataSource = list;
                    gridControlAnn.RefreshDataSource();
                    MessageDialog.Show("发送成功！");
                }
                if (radioGroup1.EditValue.ToString() == "1")
                {
                    radioGroup1.EditValue = "2";
                }
                else
                {
                    RadioFilter();
                }

            }
            catch (Exception ex)
            {
                MessageDialog.Show("保存出错：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);
            }
           


        }

        private void sysToolBar_OnBtnSearchClicked(object sender, EventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageDialog.Show("查询出错：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                     MessageBoxDefaultButton.Button1);
            }
        }

        private void LoadData()
        {
            DateTime dFrom = Convert.ToDateTime(DatePublishDateFrom.EditValue);
            DateTime dTo = Convert.ToDateTime(DatePublishDateTo.EditValue);
            string subject = string.Empty;
            string publisherName = string.Empty;


            if (!string.IsNullOrEmpty(txtSubject.Text))
            {
                subject = txtSubject.Text;
            }


            if (!string.IsNullOrEmpty(txtPublisherName.Text))
            {
                publisherName = txtPublisherName.Text;
            }


            ProxyOfficeAnnouncement proxy = new ProxyOfficeAnnouncement();

            listAnn = proxy.Service.GetAnnouncementData(
                UserInfo.userInfoId, subject, publisherName, dFrom, dTo);


            RadioFilter();
        }

        private void sysToolBar_OnCloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void sysToolBar_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            try
            {
                bindingSourceAnn.EndEdit();
                gridViewAnn.CloseEditor();
                List<EntityOaAnnouncement> list = bindingSourceAnn.DataSource as List<EntityOaAnnouncement>;
                if (list == null || list.Count == 0) return;
                //DataRow[] rows = table.Select(string.Format("isselected = {0} ", 1));
                list = list.Where(w => w.IsSelected.ToString().Contains("1")).ToList();
                if (list.Count == 0)
                {
                    MessageDialog.Show("请勾选要删除的公告", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                       MessageBoxDefaultButton.Button1);
                    return;
                }

                DialogResult result = MessageDialog.Show("确定要删除所勾选公告？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No)
                {
                    return;
                }
                ProxyOfficeAnnouncement proxy = new ProxyOfficeAnnouncement();

                proxy.Service.DeleteAnnouncement(list);


                LoadData();
            }
            catch (Exception ex)
            {
                MessageDialog.Show("删除出错：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);
            }
        }

     

        private void sysToolBar_OnBtnCancelClicked(object sender, EventArgs e)
        {
            gridViewAnn_FocusedRowChanged(null, null);
        }

        #endregion

        #region 选择接收人

        private void txtBingdingReceiverNames_Closed(object sender, ClosedEventArgs e)
        {
            List<string> userInfoIdList = new List<string>();
            string receiveNames =string.Empty;

            for (int i = 0; i < tvUserInfo.AllNodesCount; i++)
            {
                TreeListNode tn = tvUserInfo.FindNodeByID(i);
                string userInfoId = tn.GetValue(colUserInfoId).ToString();
                string userName = tn.GetValue(colUserInfoName).ToString();

                if (tn.Checked && userInfoId != "-1" && userInfoIdList.Contains(userInfoId) == false)
                {
                    userInfoIdList.Add(userInfoId);
                    receiveNames += userName + ",";
                }
            }

            for (int i = 0; i < tvRoleUser.AllNodesCount; i++)
            {
                TreeListNode tn = tvRoleUser.FindNodeByID(i);
                string userInfoId = tn.GetValue(colRoleUserInfoId).ToString();
                string userName = tn.GetValue(colRoleUserName).ToString();

                if (tn.Checked && userInfoId != "-1" && userInfoIdList.Contains(userInfoId) == false)
                {
                    userInfoIdList.Add(userInfoId);
                    receiveNames += userName + ",";
                }
            }

            for (int i = 0; i < tvDepartUser.AllNodesCount; i++)
            {
                TreeListNode tn = tvDepartUser.FindNodeByID(i);
                string userInfoId = tn.GetValue(colDepartUserInfoId).ToString();
                string userName = tn.GetValue(colDepartUserName).ToString();

                if (tn.Checked && userInfoId != "-1" && userInfoIdList.Contains(userInfoId) == false)
                {
                    userInfoIdList.Add(userInfoId);
                    receiveNames += userName + ",";
                }
            }
            bindingSourceUser.EndEdit();
            gridViewUser.CloseEditor();

            List<EntitySysUser> detailTb = bindingSourceUser.DataSource as List<EntitySysUser>;
            if (detailTb != null)
            {
                //detailTb.AcceptChanges();

                List<EntitySysUser> rows = detailTb.Where(w => w.Checked.ToString().Contains("True")).ToList(); 
                foreach (EntitySysUser row in rows)
                {
                    if (row.UserId!=null&&row.UserId.ToString()!="-1"&&!userInfoIdList.Contains(row.UserId.ToString()))
                    {
                        userInfoIdList.Add(row.UserId);
                        receiveNames += row.UserName + ",";
                    }
                }

            }

            if (receiveNames != "")
            {
                receiveNames = receiveNames.Substring(0, receiveNames.Length - 1);
                txtBingdingReceiverNames.Text = receiveNames;
                txtBingdingReceiverNames.Tag = userInfoIdList;
            }
            else
            {
                txtBingdingReceiverNames.Text = "";
                txtBingdingReceiverNames.Tag = null;
            }
        }

        #endregion

        #region 公告列表行变事件

        private void gridViewAnn_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                SetEditingState(false);
                sysToolBar.EnableButton(false);
                txtBindingSubject.Text = "";
                memoEditBody.Text = "";
                txtBindingPublisherName.Text = "";
                txtBingdingReceiverNames.Tag = null;
                txtBingdingReceiverNames.Text = "";
                dateBindingPublishDate.EditValue = null;

                EntityOaAnnouncement ann = (EntityOaAnnouncement)bindingSourceAnn.Current;
                if (ann != null)
                {
                    txtBindingSubject.Text = ann.AnctTitle;
                    memoEditBody.Text = ann.AnctContent;
                    txtBindingPublisherName.Text = ann.AnctPublishUserName;
                    txtBingdingReceiverNames.Text = ann.AnctReciverName;
                    dateBindingPublishDate.EditValue = ann.AnctPublishDate;

                    if (ann.AnnGoroup == "已收取" && ann.ReadFlag == "未读")
                    {
                        ann.ReadFlag = "已读";
                        gridViewAnn.SetRowCellValue(gridViewAnn.FocusedRowHandle, colReadFlag, "已读");
                        ProxyOfficeAnnouncement proxy = new ProxyOfficeAnnouncement();
                        proxy.Service.SetReadFlag(UserInfo.userInfoId, int.Parse(ann.AnctId.ToString()));

                    }

                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show("出错：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);
            }
        }

        #endregion

        #region radioGroup选择事件

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RadioFilter();
            }
            catch (Exception ex)
            {
                MessageDialog.Show("出错：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);
            }
        }

        private void RadioFilter()
        {
            if (listAnn == null) return;
            if (radioGroup1.EditValue != null && radioGroup1.EditValue.ToString() != "0")
            {
                gridViewAnn.OptionsView.ShowGroupPanel = false;
                gridViewAnn.ClearGrouping();
                colannGoroup.Visible = false;
                string searchStr = radioGroup1.EditValue.ToString() == "1"
                                       ? "已收取"
                                       : "已发送";
                colReadFlag.Visible = radioGroup1.EditValue.ToString() == "1";
                //dv.RowFilter = searchStr;
                List<EntityOaAnnouncement>list = listAnn.Where(w => w.AnnGoroup==searchStr).ToList();
                bindingSourceAnn.DataSource = list;
            }
            else
            {
                gridViewAnn.BeginSort();
                colannGoroup.Visible = true;
                colReadFlag.Visible = true;
                gridViewAnn.OptionsView.ShowGroupPanel = true;
                colannGoroup.GroupIndex = 0;
                gridViewAnn.EndSort();
                gridViewAnn.ExpandAllGroups();
                bindingSourceAnn.DataSource = listAnn;
                //dv.RowFilter = "";
            }
        }
        #endregion

        #region 用户检索事件

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (userData == null) return;
            if (!string.IsNullOrEmpty(txtSearch.EditValue.ToString()))
            {
                string searchStr = txtSearch.EditValue.ToString();
                bindingSourceUser.DataSource = userData.Where(w => w.Checked.ToString().Contains("true") ||
                                                                                          w.UserName != null && w.UserName.Contains(searchStr) ||
                                                                                          w.LoginId != null && w.LoginId.Contains(searchStr) ||
                                                                                          w.UserType != null && w.UserType.Contains(searchStr) ||
                                                                                          w.WbCode != null && w.WbCode.Contains(searchStr) ||
                                                                                          w.PyCode != null && w.PyCode.Contains(searchStr)).ToList();
            }
            else {
                bindingSourceUser.DataSource = userData;
            }
        }

        #endregion

        #region 用户树CheckNode事件

        private void tvUserInfo_AfterCheckNode(object sender, NodeEventArgs e)
        {
            TreeListNode tn = e.Node;
            SetCheckedChildNodes(tn, tn.Checked);
        }

        private void tvRoleUser_AfterCheckNode(object sender, NodeEventArgs e)
        {
            TreeListNode tn = e.Node;
            SetCheckedChildNodes(tn, tn.Checked);
        }

        private void tvDepartUser_AfterCheckNode(object sender, NodeEventArgs e)
        {
            TreeListNode tn = e.Node;
            SetCheckedChildNodes(tn, tn.Checked);
        }

        /// <summary>
        /// 递归选择子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        private void SetCheckedChildNodes(TreeListNode node, bool check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].Checked = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }

        #endregion

    }
}
