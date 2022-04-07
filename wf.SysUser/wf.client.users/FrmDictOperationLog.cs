using System;
using System.Collections.Generic;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.users
{
    public partial class FrmDictOperationLog : FrmCommon
    {
        List<EntitySysOperationLog> listLog = new List<EntitySysOperationLog>();
        ProxySysOperationLog proxy = new ProxySysOperationLog();
        public FrmDictOperationLog()
        {
            InitializeComponent();
            this.sysToolBar1.OnBtnSearchClicked += new System.EventHandler(this.sysToolBar1_OnBtnSearchClicked);
            this.sysToolBar1.BtnResetClick += new System.EventHandler(this.sysToolBar1_BtnResetClick);
        }

        string userTypes = string.Empty;

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmOperationLog_Load(object sender, EventArgs e)
        {
            //初始化按钮
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnSearch.Name, sysToolBar1.BtnReset.Name });

            //重置搜索条件
            sysToolBar1_BtnResetClick(null, null);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            EntityLogQc qc = new EntityLogQc();
            qc.DateStart = timeFrom.EditValue.ToString();
            qc.DateEnd = timeTo.EditValue.ToString();
            qc.OperationUserId = selectDictSysUser1.valueMember;
            qc.OperatModule = cboModule.Text;

            listLog = proxy.Service.GetOperationLog(qc);
            bsOperatiopnLog.DataSource = listLog;
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnResetClick(object sender, EventArgs e)
        {
            //初始化时间段
            timeFrom.EditValue = DateTime.Now.AddDays(-3);
            timeTo.EditValue = DateTime.Now;
            selectDictSysUser1.valueMember = "";
            cboModule.Text = "";
        }

    }
}
