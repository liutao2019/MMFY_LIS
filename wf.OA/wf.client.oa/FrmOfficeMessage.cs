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
using dcl.entity;

namespace dcl.client.oa
{
    public partial class FrmOfficeMessage : FrmCommon
    {
        public FrmOfficeMessage()
        {
            InitializeComponent();

            this.sysToolBar1.OnBtnAddClicked += new System.EventHandler(this.sysToolBar1_OnBtnAddClicked);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.sysToolBar1_OnBtnSaveClicked);
            this.sysToolBar1.OnBtnCancelClicked += new System.EventHandler(this.sysToolBar1_OnBtnCancelClicked);
            this.sysToolBar1.OnBtnRefreshClicked += new System.EventHandler(this.sysToolBar1_OnBtnRefreshClicked);
            this.sysToolBar1.BtnAnswerClick += new System.EventHandler(this.sysToolBar1_BtnAnswerClick);
        }

        DataSet dsSysMessage = new DataSet();

        //全局变量，操作数据的服务层
        ProxyNoticeManage proxyManage = new ProxyNoticeManage();

        private void FrmOfficeMessage_Load(object sender, EventArgs e)
        {
            //需要显示的按钮和顺序
            sysToolBar1.BtnSave.Caption = "发送";
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnAdd.Name, sysToolBar1.BtnSave.Name, sysToolBar1.BtnCancel.Name, sysToolBar1.BtnDelete.Name, sysToolBar1.BtnAnswer.Name, sysToolBar1.BtnRefresh.Name });
            sysToolBar1.BtnSave.Enabled = false;

            //默认一直不能编辑
            txtMessageFrom.Properties.ReadOnly = true;
            txtMessageFrom.Properties.ReadOnly = true;
            //初始化时不能编辑
            EnterEditingState(false);

            LoadData();
        }
        
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void LoadData()
        {
            EntitySysUser user = new EntitySysUser();
            user.UserId = UserInfo.userInfoId;

            EntityResponse result = new EntityResponse();

            result = proxyManage.Service.SearchMessUserRoleDepart(user);

            List<Object> listObj=result.GetResult() as List<Object>;
            
            if (listObj.Count > 0)
            {
                //消息列表
                List<EntitySysMessage> listMessage = new List<EntitySysMessage>();
                listMessage = listObj[0] as List<EntitySysMessage>;
                tvMessage.DataSource = listMessage;
                tvMessage.ExpandAll();

                //用户树   
                //   PowerUserInfo
                List<EntitySysUser> listUser = new List<EntitySysUser>();
                listUser = listObj[1] as List<EntitySysUser>;
                tvUserInfo.DataSource = listUser;
                // PowerRoleUser
                List<EntitySysRole> listRole = new List<EntitySysRole>();
                listRole = listObj[2] as List<EntitySysRole>;
                tvRoleUser.DataSource = listRole;
                // PowerUserDepart
                List<EntityUserDept> listDept = new List<EntityUserDept>();
                listDept = listObj[3] as List<EntityUserDept>;
                tvDepartUser.DataSource = listDept;
            }
            
        }

        /// <summary>
        /// 添加消息(新增按钮事件)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnAddClicked(object sender, EventArgs e)
        {
            EnterEditingState(true);

            txtMessageContent.Text = "";
            txtMessageTitle.Text = "";
            txtMessageTo.Text = "";
            txtMessageTo.Tag = null;
            txtMessageFrom.Text = UserInfo.userName;

            txtMessageTitle.Focus();
        }

        /// <summary>
        /// 控制所有输入框状态
        /// </summary>
        /// <param name="enable"></param>
        private void EnterEditingState(Boolean enable)
        {
            txtMessageContent.Properties.ReadOnly = !enable;
            txtMessageTitle.Properties.ReadOnly = !enable;
            txtMessageTo.Properties.ReadOnly = !enable;

            sysToolBar1.BtnAnswer.Enabled = !enable;
        }

        /// <summary>
        /// 新建通知(发送按钮事件)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            this.isActionSuccess = false;

            if (txtMessageTitle.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_MESSAGETITLE_NULL, OfficeMessage.BASE_TITLE);
                txtMessageTitle.Focus();
                return;
            }

            if (txtMessageContent.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_MESSAGECONTENT_NULL, OfficeMessage.BASE_TITLE);
                txtMessageContent.Focus();
                return;
            }

            if (txtMessageTo.Tag == null || ((ArrayList)txtMessageTo.Tag).Count == 0)
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_MESSAGETO_NULL, OfficeMessage.BASE_TITLE);
                return;
            }
            
            List<EntitySysMessage> listSysMessage = new List<EntitySysMessage>();

            //保存到发件箱
            if (ckeSaveFrom.Checked)
            {
                EntitySysMessage message = new EntitySysMessage();
                if (int.Parse(UserInfo.userInfoId) > 0)
                {
                    message.MessageType= "用户消息";
                }
                else
                {
                    message.MessageType = "系统消息";
                }
                message.MessageTitle = txtMessageTitle.Text.Trim();
                message.MessageContent = txtMessageContent.Text.Trim();
                message.MessageOwer=UserInfo.userInfoId;
                message.MessageOwerType = -1;


                message.MessageFrom = txtMessageFrom.Text.Trim();
                message.MessageTo= txtMessageTo.Text.Trim();
                message.MessageFromId = UserInfo.userInfoId;

                listSysMessage.Add(message);
            }


            //保存到收件箱
            ArrayList arrUser = (ArrayList)txtMessageTo.Tag;
            for (int i = 0; i < arrUser.Count; i++)
            {
                EntitySysMessage sysMessage = new EntitySysMessage();
          
                if (int.Parse(UserInfo.userInfoId) > 0)
                {
                    sysMessage.MessageType = "用户消息";
                }
                else
                {
                    sysMessage.MessageType = "系统消息";
                }

                sysMessage.MessageTitle= txtMessageTitle.Text.Trim();
                sysMessage.MessageContent = txtMessageContent.Text.Trim();
                sysMessage.MessageOwer = arrUser[i].ToString();
                sysMessage.MessageOwerType = -2;
                sysMessage.MessageFrom = txtMessageFrom.Text.Trim();
                sysMessage.MessageTo = txtMessageTo.Text.Trim();
                sysMessage.MessageFromId = UserInfo.userInfoId;

                listSysMessage.Add(sysMessage);
            }
            
            proxyManage.Service.SaveSysMessage(listSysMessage);

            LoadData();
        }

        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            List<EntitySysMessage> listDelete = new List<EntitySysMessage>();

            for (int i = 0; i < tvMessage.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvMessage.FindNodeByID(i);
                int messageId = int.Parse(tn.GetValue(colMessageId).ToString());

                if (tn.Checked == true && messageId > 0)
                {
                    EntitySysMessage masID = new EntitySysMessage();
                    masID.MessageId = messageId;
                    listDelete.Add(masID);
                }
            }

            if (listDelete.Count < 1)
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_SELECT_NULL, OfficeMessage.BASE_TITLE);
                return;
            }
            
            DialogResult dresult = MessageBox.Show(OfficeMessage.BASE_DELETE_CONFIRM, OfficeMessage.BASE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            switch (dresult)
            {
                case DialogResult.OK:
                    proxyManage.Service.DeleteSysMessage(listDelete);
                    break;
                case DialogResult.Cancel:
                    return;
            }

            //删除记录后重新加载界面
            LoadData();
        }

        /// <summary>
        /// 选择消息分组时同时选择所有记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvMessage_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode tn = e.Node;
            SetCheckedChildNodes(tn, tn.Checked);
        }

        /// <summary>
        /// 递归选择子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        private void SetCheckedChildNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, bool check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].Checked = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }

        private void tvUserInfo_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode tn = e.Node;
            SetCheckedChildNodes(tn, tn.Checked);
        }

        private void txtMessageTo_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            ArrayList arrayUserInfoId = new ArrayList();
            string toUser = "";

            for (int i = 0; i < tvUserInfo.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvUserInfo.FindNodeByID(i);
                int userInfoId = int.Parse(tn.GetValue(colUserInfoId).ToString());
                string userName = tn.GetValue(colUserInfoName).ToString();

                if (tn.Checked == true && userInfoId != -1 && arrayUserInfoId.Contains(userInfoId) == false)
                {
                    arrayUserInfoId.Add(userInfoId);
                    toUser += userName + ",";
                }
            }

            for (int i = 0; i < tvRoleUser.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvRoleUser.FindNodeByID(i);
                int userInfoId = int.Parse(tn.GetValue(colRoleUserInfoId).ToString());
                string userName = tn.GetValue(colRoleUserName).ToString();

                if (tn.Checked == true && userInfoId != -1 && arrayUserInfoId.Contains(userInfoId) == false)
                {
                    arrayUserInfoId.Add(userInfoId);
                    toUser += userName + ",";
                }
            }

            for (int i = 0; i < tvDepartUser.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvDepartUser.FindNodeByID(i);
                int userInfoId = int.Parse(tn.GetValue(colDepartUserInfoId).ToString());
                string userName = tn.GetValue(colDepartUserName).ToString();

                if (tn.Checked == true && userInfoId != -1 && arrayUserInfoId.Contains(userInfoId) == false)
                {
                    arrayUserInfoId.Add(userInfoId);
                    toUser += userName + ",";
                }
            }

            if (toUser != "")
            {
                toUser = toUser.Substring(0, toUser.Length - 1);
                txtMessageTo.Text = toUser;
                txtMessageTo.Tag = arrayUserInfoId;
            }
            else
            {
                txtMessageTo.Text = "";
                txtMessageTo.Tag = null;
            }


        }

        /// <summary>
        /// 放弃操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnCancelClicked(object sender, EventArgs e)
        {
            tvMessage_FocusedNodeChanged(null, null);
        }

        /// <summary>
        /// 选择消息列表时显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvMessage_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            EnterEditingState(false);
            sysToolBar1.EnableButton(false);
            txtMessageContent.Text = "";
            txtMessageTitle.Text = "";
            txtMessageTo.Text = "";
            txtMessageTo.Tag = null;
            txtMessageFrom.Text = "";

            DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvMessage.Selection[0];
            int messageId = int.Parse(tn.GetValue(colMessageId).ToString());
            if (messageId > 0)
            {
                List<EntitySysMessage> listSysMessage = new List<EntitySysMessage>();
                listSysMessage = proxyManage.Service.UpdateAndSearchSysMessage(messageId);
                
                if (listSysMessage.Count > 0)
                {
                    EntitySysMessage masResult = listSysMessage[0];

                    txtMessageContent.Text = masResult.MessageContent;
                    txtMessageTitle.Text = masResult.MessageTitle;
                    txtMessageTo.Text = masResult.MessageTo;
                    txtMessageTo.Tag = null;
                    txtMessageFrom.Text = masResult.MessageFrom;
                    tn.SetValue(colReadDate, "是");
                }
            }
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        ///  行颜色控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvMessage_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            if (e.Node.GetValue(colReadDate).ToString() != "是")
            {
                if (e.Node.GetValue(colMessageType).ToString() != "用户消息")
                {
                    //超级管理员发送的未读消息显示为红色
                    e.Appearance.ForeColor = Color.Red;
                }
                else
                {
                    //普通未读消息显示为黑色
                    e.Appearance.ForeColor = Color.Black;
                }
            }
            else
            {
                //已被阅读的消息显示为灰色
                e.Appearance.ForeColor = Color.Gray;
            }

        }

        private void tvRoleUser_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode tn = e.Node;
            SetCheckedChildNodes(tn, tn.Checked);
        }

        private void tvDepartUser_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode tn = e.Node;
            SetCheckedChildNodes(tn, tn.Checked);
        }

        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnAnswerClick(object sender, EventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvMessage.Selection[0];
            int messageId = int.Parse(tn.GetValue(colMessageId).ToString());
            if (messageId > 0)
            {
                List<EntitySysMessage> listResult = new List<EntitySysMessage>();
                listResult = proxyManage.Service.UpdateAndSearchSysMessage(messageId);
                
                if (listResult.Count > 0)
                {
                    sysToolBar1.BtnAdd.PerformClick();
                    
                    EntitySysMessage masResult = listResult[0];

                    txtMessageTitle.Text = "回复:" + masResult.MessageTitle;
                    txtMessageTo.Text = masResult.MessageFrom;
                    txtMessageContent.Text = "\r\n\r\n\r\n---------------------------\r\n" + masResult.MessageContent;

                    if (masResult.MessageFromId != null && masResult.MessageFromId.ToString() != "")
                    {
                        ArrayList arrayUserInfoId = new ArrayList();
                        arrayUserInfoId.Add(int.Parse(masResult.MessageFromId.ToString()));
                        txtMessageTo.Tag = arrayUserInfoId;
                    }
                    txtMessageContent.Focus();
                    txtMessageContent.Select(0, 0);
                }
            }
        }
    }
}
