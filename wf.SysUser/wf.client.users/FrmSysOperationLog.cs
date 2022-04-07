using dcl.client.frame;
using dcl.client.wcf;
using dcl.entity;
using lis.client.control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.users
{
    public partial class FrmSysOperationLog : FrmCommon
    {
        public FrmSysOperationLog()
        {
            InitializeComponent();
        }

        private void FrmSysOperationLog_Load(object sender, EventArgs e)
        {
            //初始化工具按钮：查询，关闭
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnSearch.Name, sysToolBar1.BtnClose.Name });

            //查询条件：操作时间默认当前时间到往前推一个月时间
            this.dateEdit_BeginDate.EditValue = DateTime.Now.AddMonths(-1).Date;
            this.dateEdit_EndDate.EditValue = DateTime.Now.Date;

            this.lookUpEditOperState.Properties.DataSource = this.oper_State();
            this.lookUpEditOperState.Properties.DisplayMember = "name";
            this.lookUpEditOperState.Properties.ValueMember = "id";
        }

        private DataTable oper_State()
        {
            DataTable result = new DataTable("oper_state");
            result.Columns.Add("id", typeof(int));
            result.Columns.Add("name");
            result.Rows.Add(new Object[] { 0, "操作失败" });
            result.Rows.Add(new Object[] { 1, "操作成功" });
            return result;
        }

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            try
            {
                EntitySysInterfaceLog sysOperLog = new EntitySysInterfaceLog();

                sysOperLog.OperaBeginDateTime = Convert.ToDateTime(this.dateEdit_BeginDate.EditValue);//操作开始时间
                sysOperLog.OperaEndDateTime = Convert.ToDateTime(this.dateEdit_EndDate.EditValue).AddDays(1).AddSeconds(-1); //操作结束时间

                if (!string.IsNullOrEmpty(this.textEdit_barID.Text))//条码号
                {
                    sysOperLog.SampBarId = this.textEdit_barID.Text;
                }

                if (!string.IsNullOrEmpty(this.textEdit_orderSN.Text)) //医嘱ID
                {
                    sysOperLog.OrderSn = this.textEdit_orderSN.Text;
                }

                if (!string.IsNullOrEmpty(this.textEdit_repID.Text)) //报告标识ID
                {
                    sysOperLog.RepId = this.textEdit_repID.Text;
                }

                if (!string.IsNullOrEmpty(this.textEdit_operationName.Text)) //操作名称
                {
                    sysOperLog.OperationName = this.textEdit_operationName.Text;
                }

                if (!string.IsNullOrEmpty(this.textEdit_OperUserCode.Text)) //操作人代码
                {
                    sysOperLog.OperationUserCode = this.textEdit_OperUserCode.Text;
                }

                if (!string.IsNullOrEmpty(this.textEdit_operUserName.Text)) //操作人名称
                {
                    sysOperLog.OperationUserName = this.textEdit_operUserName.Text;
                }

                if (this.lookUpEditOperState.EditValue != null && !string.IsNullOrEmpty(this.lookUpEditOperState.EditValue.ToString())) //操作状态
                {
                    sysOperLog.OperationSuccess = Convert.ToInt32(this.lookUpEditOperState.EditValue);
                }


                ProxySysInterfaceLog proxySysIntfLog = new ProxySysInterfaceLog();
                List<EntitySysInterfaceLog> listIntfLog = new List<EntitySysInterfaceLog>();
                listIntfLog = proxySysIntfLog.Service.GetSysInterfaceLogData(sysOperLog);

                this.operationDataSource.DataSource = listIntfLog;
            }
            catch (Exception ex)
            {
                MessageDialog.Show("查询出错" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);
            }
        }
    }
}
