using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.common;
using dcl.client.common;
using dcl.client.wcf;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.qc
{
    public partial class FrmQcCopyNew : FrmCommon
    {
        string itemId;
        string itemName;
        public FrmQcCopyNew()
        {
            InitializeComponent();
        }
        public FrmQcCopyNew(string itemId, string itemName)
        {
            InitializeComponent();
            this.itemId = itemId;
            this.itemName = itemName;
            lue_Ins.Focus();
        }

        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="e"></param>
        public delegate void ClikeHander(ClickEventArgs e);

        /// <summary>
        /// 事件
        /// </summary>
        public event ClikeHander clikcA;

        //全局变量，存放质控的所有数据
        private List<EntityDicQcMateria> listQcMater = new List<EntityDicQcMateria>();

        private void FrmQcCopy_Load(object sender, EventArgs e)
        {
            sysCopy.SetToolButtonStyle(new string[] { sysCopy.BtnCopy.Name });
            
            ProxyQcMateria proxyQcMater = new ProxyQcMateria();
            listQcMater = proxyQcMater.Service.SearchQcMateriaAll();

            gcLot.DataSource = listQcMater;

            txtSearch.Text = itemName;
            gvCopy_FocusedRowChanged(null, null);
        }

        /// <summary>
        /// 质控物检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_EditValueChanged(object sender, EventArgs e)
        {
            if (listQcMater.Count > 0 && txtSearch.EditValue.ToString() != "")
            {
                List<EntityDicQcMateria> listQcMatFilter = new List<EntityDicQcMateria>();
                string searchValue = txtSearch.EditValue.ToString();

                listQcMatFilter = listQcMater.Where(w => w.ItrEname.Contains(searchValue) ||
                                                       w.MatLevel.Contains(searchValue.ToUpper()) ||
                                                       w.MatBatchNo.Contains(searchValue.ToUpper())||
                                                       w.ProName.Contains(searchValue)
                                                       ).ToList();
                gcLot.DataSource = listQcMatFilter;
            }
            else
                gcLot.DataSource = listQcMater;
        }

        /// <summary>
        /// 复制质控物
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysCopy_BtnCopyClick(object sender, EventArgs e)
        {
            this.sysCopy.Focus();

            EntityDicQcMateria eyQcMatCopy = gvCopy.GetFocusedRow() as EntityDicQcMateria;
            if (lue_Ins.valueMember == null || (lue_Ins.valueMember != null && lue_Ins.valueMember == ""))
            {
                lis.client.control.MessageDialog.Show("请选择要复制到哪台仪器！", "提示");
                return;
            }
            if (eyQcMatCopy == null)
            {
                lis.client.control.MessageDialog.Show("请选择要复制的质控物！", "提示");
                return;
            }
            if (txtMark.EditValue == null || (txtMark.EditValue != null && txtMark.EditValue.ToString().Trim() == ""))
            {
                lis.client.control.MessageDialog.Show("质控物水平不能为空", "提示");
                return;
            }
            if (txtLot.EditValue == null || (txtLot.EditValue != null && txtLot.EditValue.ToString().Trim() == ""))
            {
                lis.client.control.MessageDialog.Show("批号不能为空！", "提示");
                return;
            }

            if (dtEdate.EditValue == null)
            {
                lis.client.control.MessageDialog.Show("有效日期不能为空！", "提示");
                return;
            }

            EntityDicQcMateria eyQcMateria = new EntityDicQcMateria();
            eyQcMateria.MatId = lue_Ins.valueMember;
            eyQcMateria.MatLevel = txtMark.EditValue.ToString();
            eyQcMateria.MatBatchNo = txtLot.EditValue.ToString();

            ProxyQcMateria proxyQcMat = new ProxyQcMateria();

            List<EntityDicQcMateria> listDetail = new List<EntityDicQcMateria>();

            listDetail = proxyQcMat.Service.SearchMatSnInQcMateria(eyQcMateria);//查询时间范围内存在的质控物、标识

            //判断该时间范围里是否存在同批号质控物
            if (listDetail.Count > 0)//查到该时间范围内有一个质控物，就不允许保存
            {
                lis.client.control.MessageDialog.Show("该仪器已存在此相同批号相同水平的控制物！", "提示");
                return;
            }
            
            //替换原数据  
            eyQcMatCopy.MatId = lue_Ins.valueMember;
            eyQcMatCopy.MatLevel = txtMark.EditValue.ToString();
            eyQcMatCopy.MatBatchNo = txtLot.EditValue.ToString();
            eyQcMatCopy.MatDateEnd = Convert.ToDateTime(dtEdate.EditValue);
            eyQcMatCopy.MatDateStart = ServerDateTime.GetServerDateTime();
            bool isOperSuccess = proxyQcMat.Service.CopyMethodQcMateria(eyQcMatCopy);

            if (isOperSuccess)
            {
                string matId = this.lue_Ins.valueMember;
                string matLevel = this.txtMark.Text;
                List<EntityDicQcMateria> lisrResult = new List<EntityDicQcMateria>();
                lisrResult = proxyQcMat.Service.SearchQcMateriaLeftRuleTimeAndInterface(matId, matLevel);

                FrmQcParameterRuleInstNew frmPRI = new FrmQcParameterRuleInstNew(lisrResult);
                frmPRI.ShowDialog();

                this.Close();
            }
        }

        private void FrmQcCopy_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClickEventArgs cea = new ClickEventArgs();
            cea.State = 1;
            clikcA(cea);
        }

        private void gvCopy_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            EntityDicQcMateria eyCopy = gvCopy.GetFocusedRow() as EntityDicQcMateria;
            if (eyCopy != null)
            {
                txtMark.EditValue = eyCopy.MatLevel;
                txtLot.EditValue = eyCopy.MatBatchNo;
                dtEdate.EditValue = eyCopy.MatDateEnd;
                lue_Ins.valueMember = eyCopy.MatId;
                lue_Ins.displayMember = eyCopy.ItrEname;
            }
        }

    }
}
