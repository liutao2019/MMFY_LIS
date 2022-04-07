using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.common;

using dcl.client.frame;
using dcl.entity;
using lis.client.control;
using System.Linq;

namespace dcl.client.dicbasic
{
    [DesignTimeVisible(false)]
    public partial class ConTubeRack : ConDicCommon, IBarActionExt //ClientBaseControl
    {
        #region 旧代码
        //public override void InitParamters()
        //{
        //    subTable = BarcodeTable.CuvetteShelf.TableName;
        //    gcSub = gridControl1;
        //    gvSub = gridView3;
        //    primaryKeyOfSubTable = BarcodeTable.CuvetteShelf.ID;
        //    bsSub = bsCuvetteShelf;
        //    //barControl1.BarManager = this;
        //    BaseInfoContainer = groupBox1;

        //}

        //protected override void InitOtherData()
        //{
        //    BindingControl();
        //    //searchControl1.Initialize(gvSub, bsSub, dtSub, dtSub);
        //    //if (!string.IsNullOrEmpty(searchControl1.Text))
        //        //searchControl1.SearchAgain();
        //}

        //private void BindingControl()
        //{
        //    if (txtName.DataBindings == null || txtName.DataBindings.Count == 0)
        //        txtName.DataBindings.Add(new System.Windows.Forms.Binding("Text", bsCuvetteShelf, "cus_name", true));
        //    if (seXnum.DataBindings == null || seXnum.DataBindings.Count == 0)
        //        seXnum.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", bsCuvetteShelf, "cus_x_num", true));
        //    if (seYnum.DataBindings == null || seYnum.DataBindings.Count == 0)
        //        seYnum.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", bsCuvetteShelf, "cus_y_num", true));

        // }
        #endregion

        public ConTubeRack()
        {
            InitializeComponent();
            this.Name = "ConTubeRack";
        }

        /// <summary>
        /// 是否为新增事件
        /// </summary>
        private bool blIsNew = false;

        private List<EntityDicTubeRack> list = new List<EntityDicTubeRack>();

        #region IBarActionExt

        public void Add()
        {
            blIsNew = true;//标记为新增事件

            this.txtName.Focus();
            //this.seXnum.Focus();
            //this.seYnum.Focus();

            EntityDicTubeRack dr = (EntityDicTubeRack)bsCuvetteShelf.AddNew();
            dr.RackCode = string.Empty;
            //dr.SamDelFlag = LIS_Const.del_flag.OPEN;
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.bsCuvetteShelf.EndEdit();
            if (bsCuvetteShelf.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();

            EntityDicTubeRack dr = (EntityDicTubeRack)bsCuvetteShelf.Current;
            String type_id = dr.RackCode;

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
                    dr.RackCode = result.GetResult<EntityDicTubeRack>().RackCode;
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
            }
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                blIsNew = false;//取消新增事件
                this.gridControl1.Enabled = true;
            }
        }

        public void Delete()
        {
            if (this.txtName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("名称不能为空", "提示信息");
                return;
            }

            this.bsCuvetteShelf.EndEdit();
            if (bsCuvetteShelf.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicTubeRack dr = (EntityDicTubeRack)bsCuvetteShelf.Current;
            String br_id = dr.RackCode;

            request.SetRequestValue(dr);

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
        }

        public void DoRefresh()
        {
            EntityResponse ds = base.Search(new EntityRequest());
            if (isActionSuccess)
            {
                list = ds.GetResult() as List<EntityDicTubeRack>;
                bsCuvetteShelf.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridControl1", true);
            //dlist.Add("simpleButton1", true);

            return dlist;
        }

        public void Cancel()
        {
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
                blIsNew = false;//取消新增事件
        }

        public void Close() { }

        public void Edit()
        {
            this.gridControl1.Enabled = false;
        }

        public void MoveNext() { }

        public void MovePrev() { }

        #endregion

        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        private void on_Load(object sender, EventArgs e)
        {
            this.initData();
            setGridControl();
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
        
    }
}
