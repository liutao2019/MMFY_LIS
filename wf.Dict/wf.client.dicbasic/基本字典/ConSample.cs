using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using DevExpress.XtraGrid.Columns;
using dcl.entity;
using lis.client.control;

namespace dcl.client.dicbasic
{
    public partial class ConSample : ConDicCommon, IBarActionExt
    {
        /// <summary>
        /// 是否为新增事件
        /// </summary>
        private bool blIsNew = false;

        private List<EntityDicSample> list = new List<EntityDicSample>();

        #region IBarActionExt

        public void Add()
        {
            blIsNew = true;//标记为新增事件

            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            this.btnName.Focus();
            btnPYM.Properties.ReadOnly = true;
            btnWBM.Properties.ReadOnly = true;

            EntityDicSample dr = (EntityDicSample)bsItem_Prop.AddNew();
            dr.SamId = string.Empty;
            dr.SamDelFlag = LIS_Const.del_flag.OPEN;
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.bsItem_Prop.EndEdit();
            if (string.IsNullOrEmpty(btnName.Text))
            {
                MessageDialog.Show("请输入标本类别");
                return;
            }

            if (bsItem_Prop.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();

            EntityDicSample dr = (EntityDicSample)bsItem_Prop.Current;
            ChangeData(dr);
            String type_id = dr.SamId;

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
                DoRefresh();
                gridControl1.RefreshDataSource();
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

        private void ChangeData(EntityDicSample dr)
        {
            if (!string.IsNullOrEmpty(cbbTiYe.Text))
            {
                if (cbbTiYe.Text == "无菌体液")
                {
                    dr.SamBacFlag = "0";
                }
                else
                {
                    dr.SamBacFlag = "1";
                }
            }
            else
            {
                dr.SamBacFlag = "";
            }
            #region 转换自定义类别
            if (!string.IsNullOrEmpty(cbbCustomType.Text))
            {
                if (cbbCustomType.Text == "BLD")
                {
                    dr.SamCustomType = "0";
                }
                else if (cbbCustomType.Text == "MISC")
                {
                    dr.SamCustomType = "1";
                }
                else if (cbbCustomType.Text == "UR")
                {
                    dr.SamCustomType = "2";
                }
                else if (cbbCustomType.Text == "RES")
                {
                    dr.SamCustomType = "3";
                }
                else if (cbbCustomType.Text == "CSF")
                {
                    dr.SamCustomType = "4";
                }
                else if (cbbCustomType.Text == "SBF")
                {
                    dr.SamCustomType = "5";
                }
                else if (cbbCustomType.Text == "STL")
                {
                    dr.SamCustomType = "6";
                }
                else
                {
                    dr.SamCustomType = "7";
                }
            }
            else
            {
                dr.SamCustomType = "";
            }
            #endregion
        }

        public void Delete()
        {
            if (this.btnName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("标本类别名称不能为空", "提示信息");
                return;
            }

            this.bsItem_Prop.EndEdit();
            if (bsItem_Prop.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicSample dr = (EntityDicSample)bsItem_Prop.Current;
            String br_id = dr.SamId;

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
                list = ds.GetResult() as List<EntityDicSample>;
                bsItem_Prop.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridControl1", true);
            dlist.Add("simpleButton1", true);

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

        public ConSample()
        {
            InitializeComponent();
            this.Name = "ConSample";
            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);
            RegisterEvent();
        }

        private void RegisterEvent()
        {
            bsItem_Prop.CurrentChanged += BsItem_Prop_CurrentChanged;
        }

        private void BsItem_Prop_CurrentChanged(object sender, EventArgs e)
        {
            EntityDicSample currentItem = bsItem_Prop.Current as EntityDicSample;
            if (currentItem != null)
            {
                cbbCustomType.Text = currentItem.SamCustomTypeText;
                cbbTiYe.Text = currentItem.SamBacFlagText;
            }
        }

        private void on_Load(object sender, EventArgs e)
        {
            this.initData();
            InitComboBox();
            setGridControl();
        }

        private void InitComboBox()
        {
            List<string> listTiye = new List<string>();
            listTiye.Add("");
            listTiye.Add("无菌体液");
            listTiye.Add("有菌体液");
            cbbTiYe.Properties.Items.AddRange(listTiye);

            List<string> listCustomType = new List<string>();
            listCustomType.Add("");
            listCustomType.Add("BLD");
            listCustomType.Add("MISC");
            listCustomType.Add("UR");
            listCustomType.Add("RES");
            listCustomType.Add("CSF");
            listCustomType.Add("SBF");
            listCustomType.Add("STL");
            listCustomType.Add("SV");
            cbbCustomType.Properties.Items.AddRange(listCustomType);

        }

        private void setGridControl()
        {
            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].OptionsColumn.AllowEdit = false;
            }
        }
        private void initData()
        {
            this.DoRefresh();
        }

        private void btnName_Leave(object sender, EventArgs e)
        {
            if (bsItem_Prop.Current != null)
            {
                EntityDicSample dr = (EntityDicSample)bsItem_Prop.Current;
                dr.SamPyCode = tookit.GetSpellCode(this.btnName.Text);
                dr.SamWbCode = tookit.GetWBCode(this.btnName.Text);
            }
        }

        
    }
}
