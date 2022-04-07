using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.entity;
using dcl.client.wcf;
using System.Linq;

namespace dcl.client.report
{
    public partial class FrmSelectTemplate : FrmCommon
    {
        public FrmSelectTemplate()
        {
            InitializeComponent();
        }
        string type;

        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="e"></param>
        public delegate void ClikeHander(ClickEventArgs e);

        /// <summary>
        /// 事件
        /// </summary>
        public event ClikeHander clikcA;

        public FrmSelectTemplate(string type)
        {
            InitializeComponent();
            this.type = type;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            getTemplate();
        }

        /// <summary>
        /// 选中模板
        /// </summary>
        private void getTemplate()
        {
            EntityTpTemplate dr = this.gridView1.GetFocusedRow() as EntityTpTemplate;
            if (dr != null)
            {
                ClickEventArgs cea = new ClickEventArgs();
                cea.name = dr.StName.ToString();
                clikcA(cea);
            }
            this.Close();
        }
        List<EntityTpTemplate> dtTem = new List<EntityTpTemplate>();
        private void FrmSelectTemplate_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnConfirm.Name, sysToolBar1.BtnDelete.Name });
            EntityTpTemplate dtTemp = new EntityTpTemplate();
            dtTemp.StType = type;
            ProxyStatistical proxy = new ProxyStatistical();
            dtTem = proxy.Service.GetReportTemple(dtTemp);
            var dvTem = dtTem.GroupBy(w => w.StName).ToList();
            dtTem = new List<EntityTpTemplate>();
            foreach (var item in dvTem)
            {
                EntityTpTemplate TpTemp = new EntityTpTemplate();
                TpTemp.StName = item.Key;
                dtTem.Add(TpTemp);
            }
            this.bsTpTemplate.DataSource = dtTem;
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            if (lis.client.control.MessageDialog.Show("是否删除该模板？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                EntityTpTemplate dr = this.gridView1.GetFocusedRow() as EntityTpTemplate;
                if (dr != null)
                {
                    ProxyStatTemp proxy = new ProxyStatTemp();
                    bool result = proxy.Service.DeleteStatTemp(dr.StName, type);
                    if (result)
                    {
                        List<EntityTpTemplate> dtTem = this.bsTpTemplate.DataSource as List<EntityTpTemplate>;
                        dtTem.Remove(dr);
                        bsTpTemplate.ResetBindings(true);
                    }
                }
            }
        }

        /// <summary>
        /// 选中模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnConfirmClicked(object sender, EventArgs e)
        {
            getTemplate();
        }
    }

    public class ClickEventArgs : EventArgs
    {
        public string name
        {
            get;
            set;
        }
    }
}
