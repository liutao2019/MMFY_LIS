using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using dcl.entity;
using lis.client.control;
using System.Linq;

namespace dcl.client.dicbasic
{
    public partial class ConItem_Prop : ConDicCommon, IBarActionExt
    {
        #region IBarAction 成员

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        { }

        /// <summary>
        ///下一个
        /// </summary>
        public void MoveNext()
        { }

        /// <summary>
        /// 上一个
        /// </summary>
        public void MovePrev()
        { }

        public void Add()
        {
            if (bsItem.Current == null) return;
            EntityDicItmItem itm = bsItem.Current as EntityDicItmItem;

            EntityDefItmProperty dr = (EntityDefItmProperty)bsItem_Prop.AddNew();
            dr.PtyId = "";
            dr.PtyItmId = itm.ItmId;
            dr.PtyItmEname = itm.ItmEcode;
            canChange = true;
            setGridControl2(true);
        }

        /// <summary>
        /// 放弃
        /// </summary>
        public void Cancel()
        {
            //主页面放弃事件中调用了刷新的方法
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public void Edit()
        {
            setGridControl2(true);
        }

        public void Save()
        {
            this.bsItem_Prop.EndEdit();
            if (bsItem_Prop.Current == null)
            {
                return;
            }
            isActionSuccess = false;
            EntityRequest request = new EntityRequest();
            List<EntityDefItmProperty> props = bsItem_Prop.DataSource as List<EntityDefItmProperty>;
            var listNew = new List<EntityDefItmProperty>();
            var listUpdate = new List<EntityDefItmProperty>();
            foreach(EntityDefItmProperty item in props)
            {
                if (item.PtyItmProperty.ToString() == "")
                {
                    lis.client.control.MessageDialog.Show("项目特征不能为空", "信息");
                    return;
                }
                if (item.PtyId == "")
                    listNew.Add(item);
                else
                    listUpdate.Add(item);
            }

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("new", listNew);
            dict.Add("update", listUpdate);

            
            request.SetRequestValue(dict);

            EntityResponse result = new EntityResponse();

            result = base.Other(request);
            if (base.isActionSuccess)
            {
                MessageDialog.ShowAutoCloseDialog("保存成功");
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
                throw new Exception(result.ErroMsg);
            }
            if (base.isActionSuccess)
            {
                getTable(); 
                canChange = true;
                setGridControl2(false);
            }
            else
            {
                throw new Exception();
            }
            DoRefresh();
        }

        public void Delete()
        {
            this.bsItem_Prop.EndEdit();
            if (bsItem_Prop.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDefItmProperty prop = bsItem_Prop.Current as EntityDefItmProperty;
            request.SetRequestValue(prop);

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
            setGridControl2(false);
        }

        public void DoRefresh()
        {
            EntityResponse result = base.View(new EntityRequest());
            if (isActionSuccess)
            {
                itmList = (result.GetResult() as List<EntityDicItmItem>).FindAll(i => i.ItmDelFlag == "0");
                this.bsItem.DataSource = itmList;
                getTable();//增加右边项目特征明细刷新
            }
            setGridControl2(false);
        }
        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridControl1", true);
            dlist.Add("gridControl2", true);
            return dlist;
        }
        #endregion

        List<EntityDicItmItem> itmList = new List<EntityDicItmItem>();
        List<EntityDefItmProperty> propList = new List<EntityDefItmProperty>();
        private String _itm_id = "";
        private String _itm_ecd = "";
        private bool canChange = true;
        private SpellAndWbCodeTookit tookit = new SpellAndWbCodeTookit();
        public ConItem_Prop()
        {

            InitializeComponent();
        }

        private void on_Load(object sender, EventArgs e)
        {
            this.DoRefresh();
            setGridControl();
            setGridControl2(false);

            if (UserInfo.GetSysConfigValue("seq_visible") == "是")
            {
                colitm_seq1.Visible = true;
                colitm_seq1.VisibleIndex = 0;
                colitm_id.Visible = false;
            }
            //lis.client.control.MessageDialog.Show(this.gridView1.curr);
        }

        private void setGridControl()
        {
            gridControl2.UseEmbeddedNavigator = false;
            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].OptionsColumn.AllowEdit = false;
            }
        }
        private void setGridControl2(bool b)
        {
            gridControl2.UseEmbeddedNavigator = b;
            for (int i = 0; i < this.gridView2.Columns.Count; i++)
            {
                this.gridView2.Columns[i].OptionsColumn.AllowEdit = b;
            }
            gridControl1.Enabled = !b;
        }
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            getTable();
        }
        private void getTable()
        {
            if (this.bsItem.Position > -1 && canChange)
            {
                EntityDicItmItem item = bsItem.Current as EntityDicItmItem;
                _itm_ecd = item.ItmEcode.ToString();
                _itm_id = item.ItmId.ToString();
                this.initDetail(_itm_id);
            }
        }
        private void initDetail(String itm_id)
        {
            if (bsItem.Position > -1 && canChange)
            {
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(itm_id);
                EntityResponse result = base.Search(request);
                if (isActionSuccess)
                {
                    propList = result.GetResult() as List<EntityDefItmProperty>;
                    this.bsItem_Prop.DataSource = null;
                    this.bsItem_Prop.DataSource = propList;
                }
            }
            else
            {
                this.bsItem_Prop.DataSource = propList;
            }
            canChange = true;
        }
        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "PtyItmProperty")
            {
                string itm_prop = e.Value.ToString();

                this.gridView2.SetFocusedRowCellValue(gridView2.Columns["PtyPyCode"], tookit.GetSpellCode(itm_prop).ToString());
                this.gridView2.SetFocusedRowCellValue(gridView2.Columns["PtyCCode"], tookit.GetSpellCode(itm_prop).ToString());
                this.gridView2.SetFocusedRowCellValue(gridView2.Columns["PtyWbCode"], tookit.GetWBCode(itm_prop).ToString());
            }
        }

        private void gridControl2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                Add();
            }
        }

        private void gridView2_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            view.FocusedColumn = view.Columns[2];
            view.ShowEditor();
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            EntityDicItmItem dr = this.gridView1.GetRow(e.RowHandle) as EntityDicItmItem;
            if (dr != null && dr.propCount != null && dr.propCount.ToString() != "")
            {
                if (dr.propCount.ToString() == "无")
                {
                    e.Appearance.ForeColor = Color.Blue;
                }
            }
        }

        
        private void gridControl2_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (e.Button.Tag != null && e.Button.Tag.ToString() == "Add")
                {
                    Add();
                }
                if (e.Button.Tag != null && e.Button.Tag.ToString() == "Delete")
                {
                    Delete();
                }
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show(ex.Message, "信息");
            }
        }

    }

}
