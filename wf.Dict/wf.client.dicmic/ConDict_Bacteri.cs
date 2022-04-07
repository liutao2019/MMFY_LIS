using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.client.dicmic;

using System.Data.SqlClient;
using DevExpress.XtraGrid.Columns;
using dcl.entity;
using System.Linq;
using lis.client.control;
using dcl.client.cache;

namespace dcl.client.dicmic
{
    public partial class ConDict_Bacteri : ConDicCommon, IBarActionExt
    {
        List<EntityDicMicBacteria> list = new List<EntityDicMicBacteria>();
        SpellAndWbCodeTookit spellcode = new SpellAndWbCodeTookit();

        #region IBarAction 成员

        public void Add()
        {
            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }
            EntityDicMicBacteria dr = bsBacteri.AddNew() as EntityDicMicBacteria;

            dr.BacId = string.Empty;
            dr.BacDelFlag = "0";

            this.txtEname.Focus();
            txtPY.Properties.ReadOnly = true;
            txtWB.Properties.ReadOnly = true;
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.bsBacteri.EndEdit();
            if (this.bsBacteri.Current == null)
            {
                isActionSuccess = false;
                return;
            }

            if (txtEname.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("英文代码不能为空！", "提示");
                txtEname.Focus();
                isActionSuccess = false;
                return;
            }

            if (txtCname.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("细菌名称不能为空！", "提示");
                isActionSuccess = false;
                txtCname.Focus();
                return;
            }

            if (selectDict_Btype1.displayMember == null || (selectDict_Btype1.displayMember != null && selectDict_Btype1.displayMember == ""))
            {
                lis.client.control.MessageDialog.Show("细菌类别不能为空！", "提示");
                selectDict_Btype1.Focus();
                isActionSuccess = false;
                return;
            }

            EntityDicMicBacteria dr = bsBacteri.Current as EntityDicMicBacteria;
            String itm_id = dr.BacId;

            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dr);
            EntityResponse result = new EntityResponse();
            if (itm_id == "")
            {
                result = base.New(request);
                gridView1.FocusedRowHandle = gridView1.DataRowCount - 1;
            }
            else
            {
                result = base.Update(request);

            }
            if (base.isActionSuccess)
            {
                if (itm_id == "")
                {
                    dr.BacId = result.GetResult<EntityDicMicBacteria>().BacId;
                }
                dr.BTypeCname = selectDict_Btype1.displayMember;
                MessageDialog.ShowAutoCloseDialog("保存成功");
                DoRefresh();
                
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
                //MessageDialog.Show(result.ErroMsg);
            }

        }

        public void Delete()
        {
            this.bsBacteri.EndEdit();
            if (bsBacteri.Current == null)
            {
                return;
            }

            EntityDicMicBacteria dr = bsBacteri.Current as EntityDicMicBacteria;
            String br_id = dr.BacId;

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
                list = result.GetResult() as List<EntityDicMicBacteria>;
                bsBacteri.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gridControl1", true);


            return dlist;
        }

        #endregion
        public ConDict_Bacteri()
        {
            InitializeComponent();
        }
        private void on_Load(object sender, EventArgs e)
        {
            InitComboBox();

            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);

            this.initData();

        }

        private void InitComboBox()
        {
            List<EntityDicMicTemplate> listTem = CacheClient.GetCache<EntityDicMicTemplate>();
            cbbDx.Properties.Items.AddRange(listTem.FindAll(i => i.TmpType == 21).Select(i => i.TmpContent).ToList());
            cbbXt.Properties.Items.AddRange(listTem.FindAll(i => i.TmpType == 22).Select(i => i.TmpContent).ToList());
            cbbBm.Properties.Items.AddRange(listTem.FindAll(i => i.TmpType == 23).Select(i => i.TmpContent).ToList());
            cbbYs.Properties.Items.AddRange(listTem.FindAll(i => i.TmpType == 24).Select(i => i.TmpContent).ToList());
            cbbBy.Properties.Items.AddRange(listTem.FindAll(i => i.TmpType == 25).Select(i => i.TmpContent).ToList());
            cbbRx.Properties.Items.AddRange(listTem.FindAll(i => i.TmpType == 26).Select(i => i.TmpContent).ToList());
            cbbTmd.Properties.Items.AddRange(listTem.FindAll(i => i.TmpType == 27).Select(i => i.TmpContent).ToList());
            cbbScfs.Properties.Items.AddRange(listTem.FindAll(i => i.TmpType == 28).Select(i => i.TmpContent).ToList());
            cbbXjjs.Properties.Items.AddRange(listTem.FindAll(i => i.TmpType == 29).Select(i => i.TmpContent).ToList());

        }

        private void initData()
        {
            this.DoRefresh();
        }

        #region 鼠标离开事件
        private void txtCname_Leave(object sender, EventArgs e)
        {
            EntityDicMicBacteria dr = bsBacteri.Current as EntityDicMicBacteria;
            dr.BacPyCode = spellcode.GetSpellCode(this.txtCname.Text);
            dr.BacWbCode = spellcode.GetWBCode(this.txtCname.Text);
            if (dr.BacPyCode.Length > 12)
            {
                dr.BacPyCode = dr.BacPyCode.Substring(1, 12);
            }
            if (dr.BacWbCode.Length > 12)
            {
                dr.BacWbCode = dr.BacWbCode.Substring(1, 12);
            }
        }
        #endregion

        #region IBarActionExt 成员

        public void Cancel()
        {

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

        
    }
}
