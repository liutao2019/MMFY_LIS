using dcl.client.cache;
using dcl.client.common;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.tools
{
    public partial class FrmTempDetail : FrmCommon
    {
        ProxyTempHandle proxy;
        public FrmTempDetail()
        {
            InitializeComponent();
            this.Load += FrmTempDetail_Load;
            proxy = new ProxyTempHandle();
        }

        private void FrmTempDetail_Load(object sender, EventArgs e)
        {
            //在这里添加你要加入的控件
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnSearch.Name, sysToolBar1.BtnRefresh.Name, sysToolBar1.BtnClose.Name });
            sysToolBar1.OnBtnSearchClicked += SysToolBar1_OnBtnSearchClicked;

            this.Initial();
        }

        private void SysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            DataFilter();
        }

        /// <summary>
        /// 数据过滤
        /// </summary>
        private void DataFilter()
        {
            startTime.Text = DateTime.Now.AddDays(-7).ToShortDateString();
            endTime.Text = DateTime.Now.Date.ToShortDateString();
            if (bsDictHar.Current == null)
                return;
            EntityDictHarvester current = bsDictHar.Current as EntityDictHarvester;
            List<EntityTemHarvester> list;
            if (current != null)
            {
                list = proxy.Service.GetHarvesterByDhId(current.DhId);
            }
            else
            {
                list = new List<EntityTemHarvester>();
            }

            bsTemHar.DataSource = list.Where(a => a.ThDate.Date >= startTime.DateTime && a.ThDate.Date <= endTime.DateTime).ToList();
        }
        private void Initial()
        {
            //从缓存中获取字典表中的值
            List<EntityDictHarvester> listHar = CacheClient.GetCache<EntityDictHarvester>();
            this.bsDictHar.DataSource = listHar;
            SetGridControl();

        }
        private void SetGridControl()
        {
            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].OptionsColumn.AllowEdit = false;
            }
            for (int i = 0; i < this.gridView2.Columns.Count; i++)
            {
                this.gridView2.Columns[i].OptionsColumn.AllowEdit = false;
            }
        }

        private void bsTemHar_CurrentChanged(object sender, EventArgs e)
        {
            if (bsDictHar.Current == null)
                return;

            DataFilter();

            //EntityDictHarvester current = bsDictHar.Current as EntityDictHarvester;
            //string dhId = current.DhId;
            //bsTemHar.DataSource = proxy.Service.GetHarvesterByDhId(dhId);
        }
    }
}
