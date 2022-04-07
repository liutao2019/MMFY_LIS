using dcl.client.frame;
using dcl.client.wcf;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class FrmLisKnowage : FrmCommon
    {
        public FrmLisKnowage()
        {
            InitializeComponent();
            this.Load += FrmLisKnowage_Load;
        }
        private void FrmLisKnowage_Load(object sender, EventArgs e)
        {
            LoadAccord();
        }

        private void LoadAccord()
        {
            gcCombine.DataSource = CacheClient.GetCache<EntityDicCombine>();
            ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            gcCombine.ContextMenuStrip = ContextMenuStrip;
            ContextMenuStrip.Items.Add("刷新数据", null, Refresh_click);
        }

        private void Refresh_click(object sender, EventArgs e)
        {
            gcCombine.DataSource = CacheClient.GetCache<EntityDicCombine>();
        }

        private void gvCombine_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            EntityDicCombine com = gvCombine.GetFocusedRow() as EntityDicCombine;

            if (com == null)
            {
                gcItem.DataSource = new List<EntityDicItmItem>();
                gc.Text = string.Empty;
                return;
            }
            ProxyPatEnterNew proxyEnter = new ProxyPatEnterNew();
            var list = proxyEnter.Service.GetItmByComID(com.ComId);
            gc.Text = string.Format("{0} 共 {1} 项 -双击项目查看具体信息", com.ComName, list.Count);
            gcItem.DataSource = list;
        }

        private void gcItem_DoubleClick(object sender, EventArgs e)
        {
            EntityDicItmItem item = gvItem.GetFocusedRow() as EntityDicItmItem;
            EntityDicCombine com = gvCombine.GetFocusedRow() as EntityDicCombine;
            if (item != null && com != null)
            {
                //frmItemInfo frm = new frmItemInfo();
                //frm.LoadItem(item, com);
                //frm.ShowDialog();
                FrmItmInfo frm = new FrmItmInfo();
                frm.LoadItem(item, com);
                frm.ShowDialog();
            }
        }

        private void gvItem_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(gvCombine.FindFilterText))
            {
                gvCombine.ExpandAllGroups();
                return;
            }
            else
            {
                gvCombine.CollapseAllGroups();
                return;
            }
        }
    }
}
