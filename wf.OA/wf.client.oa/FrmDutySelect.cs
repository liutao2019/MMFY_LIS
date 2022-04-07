using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.client.wcf;

using dcl.entity;
using System.Linq;

namespace dcl.client.oa
{
    public partial class FrmDutySelect : FrmCommon
    {
        #region 全局变量
     
        /// <summary>
        /// 先前FocusHandler
        /// </summary>
        private int intPreHandle = 0;
        /// <summary>
        /// 当前FocusHandler
        /// </summary>
        private int intFocueHandle = 0;
        /// <summary>
        /// 班次表
        /// </summary>
        private List<EntityOaDicShift> listDuty = null;
        ///// <summary>
        ///// 物理组别
        ///// </summary>
        //DataTable tbPhyic = null;
        #endregion


        public FrmDutySelect()
        {
            InitializeComponent();
            InitData();

        }

        #region 初始化数据

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {

            ProxyOaShiftDict proxy = new ProxyOaShiftDict();
            listDuty = proxy.Service.GetDutyData();
           // proxy.Dispose();
            gcDutys.DataSource = listDuty;
        }

        private void FrmDutyDict_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { 
                sysToolBar1.BtnSelectTemplate.Name });
            sysToolBar1.BtnSelectTemplate.Caption = "选择";
        }

        #endregion


        #region 查询操作
        private void txtSearch_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            List<EntityOaDicShift> list = new List<EntityOaDicShift>();
            if (e.NewValue != null)
            {
                string str = e.NewValue.ToString().Trim();
                list = listDuty.Where(w => w.ShiftId.Contains(str) || w.ShiftName.Contains(str) || w.WbCode.Contains(str) || w.PyCode.Contains(str)
                                                 || w.TypeName != null && w.TypeName.Contains(str) || w.TypePy != null && w.TypePy.Contains(str) || w.TypeWb != null && w.TypeWb.Contains(str)).ToList();
                this.gcDutys.DataSource = list;
            }
        }
        #endregion

        private void gvdutys_DoubleClick(object sender, EventArgs e)
        {
            SelectDuct();
        }

        private void sysToolBar1_BtnSelectTemplateClick(object sender, EventArgs e)
        {
            SelectDuct();
        }

        public EntityOaDicShift SelectList { get; set; }
        void SelectDuct()
        {
            EntityOaDicShift list = (EntityOaDicShift)bsDutyDict.Current;
            if (list != null)
            {
                SelectList = list;
                this.DialogResult = DialogResult.Yes;
            }
        }
     

       











    }
}
