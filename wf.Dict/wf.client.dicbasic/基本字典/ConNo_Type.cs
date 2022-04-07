using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using DevExpress.XtraGrid.Columns;
using dcl.entity;
using lis.client.control;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.dicbasic
{
    public partial class ConNo_Type : ConDicCommon, IBarActionExt
    {
        /// <summary>
        /// 是否为新增事件
        /// </summary>
        bool blIsNew = false;
        List<EntityDicPubIdent> list = new List<EntityDicPubIdent>();

        #region IBarAction 成员

        public void Add()
        {
            blIsNew = true;//标记为新增事件

            this.btnName.Focus();
            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            EntityDicPubIdent dr = bsType.AddNew() as EntityDicPubIdent;
            dr.IdtId = string.Empty;
            dr.IdtDelFlag = LIS_Const.del_flag.OPEN;

            btnPYM.Properties.ReadOnly = true;
            btnWBM.Properties.ReadOnly = true;
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            if (this.btnName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("病人标识类型名称不能为空", "提示信息");
                return;
            }
            this.bsType.EndEdit();
            if (bsType.Current == null)
            {
                return;
            }

            EntityDicPubIdent dr = bsType.Current as EntityDicPubIdent;
            String type_id = dr.IdtId;

            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dr);

            EntityResponse result = new EntityResponse();

            if (type_id == "")
            {
                result = base.New(request);
            }
            else
            {
                result = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                if (type_id == "")
                {
                    dr.IdtId = result.GetResult<EntityDicPubIdent>().IdtId;
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
                throw new Exception(result.ErroMsg);
            }
            //新增事件时保存的处理方式
            if (blIsNew)
            {
                this.blIsNew = false;
                this.gridControl1.Enabled = true;
            }
            DoRefresh();
        }

        public void Delete()
        {
            this.bsType.EndEdit();
            if (bsType.Current == null)
            {
                return;
            }

            EntityDicPubIdent dr = bsType.Current as EntityDicPubIdent;
            String br_id = dr.IdtId;
            if (br_id == "106" || br_id == "107" || br_id == "110")
            {
                lis.client.control.MessageDialog.Show("此标识为基础项目不能删除!");
                return;
            }
            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dr);

            if (br_id == "")
            {
                return;
            }
            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dresult == DialogResult.OK)
            {
                base.Delete(request);
                if (base.isActionSuccess)
                {
                    MessageDialog.ShowAutoCloseDialog("删除成功");
                    DoRefresh();
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("删除失败");
                }
            }
            else
            {

            }
        }

        public void DoRefresh()
        {
            EntityResponse result = base.Search(new EntityRequest());
            if (isActionSuccess)
            {
                list = result.GetResult() as List<EntityDicPubIdent>;
                bsType.DataSource = list;
            }
        }
        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gridControl1", true);
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);

            return dlist;
        }

        #endregion
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        public ConNo_Type()
        {

            InitializeComponent();
            this.Name = "ConNo_Type";
        }

        private void on_Load(object sender, EventArgs e)
        {

            this.initData();
            setGridControl();

            //绑定"接口类型"下拉
            string[] types = Enum.GetNames(typeof(NetInterfaceType));
            foreach (string typeItem in types)
            {
                this.txtInterfaceType.Properties.Items.Add(typeItem);
            }
            this.txtInterfaceType.SelectedIndex = 0;


            //绑定"使用接口"下拉
            List<EntitySysItfInterface> listInterface = CacheClient.GetCache<EntitySysItfInterface>();

            EntitySysItfInterface itfInterface = new EntitySysItfInterface();
            itfInterface.ItfaceName = "(无)";
            listInterface.Insert(0, itfInterface);
            this.txtInterface.Properties.DataSource = listInterface;
        }

        private void setGridControl()
        {
            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].OptionsColumn.AllowEdit = false;
            }

            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);
        }
        private void initData()
        {
            this.DoRefresh();
        }


        private void btnName_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void btnName_Leave(object sender, EventArgs e)
        {
            if (bsType.Current != null)
            {
                EntityDicPubIdent noType = bsType.Current as EntityDicPubIdent;
                noType.IdtPyCode = tookit.GetSpellCode(this.btnName.Text);
                noType.IdtWbCode = tookit.GetWBCode(this.btnName.Text);
            }
        }

        #region IBarActionExt 成员

        public void Cancel()
        {
            //新增事件时保存的处理方式
            if (blIsNew)
            {
                this.blIsNew = false;
                this.gridControl1.Enabled = true;
            }
        }

        public void Close()
        {

        }

        public void Edit()
        {
            this.gridControl1.Enabled = false;
        }

        public void MoveNext()
        {

        }

        public void MovePrev()
        {

        }

        #endregion

        
        private void bsType_CurrentChanged(object sender, EventArgs e)
        {
            //if (bsType.Current != null)
            //{
            //    EntityDicPubIdent ident = bsType.Current as EntityDicPubIdent;
            //    if (ident.IdtPatinnoNotnull == 1)
            //    {
            //        chkPatInNo_NotAllowNull.Checked = true;
            //    }
            //    else
            //    {
            //        chkPatInNo_NotAllowNull.Checked = false;
            //    }
            //}
        }
    }
}
