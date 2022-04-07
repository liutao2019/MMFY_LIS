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

namespace dcl.client.dicbasic
{
    public partial class ConSamRemarks : ConDicCommon, IBarActionExt
    {
        /// <summary>
        /// 是否为新增事件
        /// </summary>
        bool blIsNew = false;
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        private List<EntityDicSampRemark> list = new List<EntityDicSampRemark>();
        #region IBarAction 成员

        public void Add()
        {
            blIsNew = true;//标记为新增事件
            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }
            this.btnName.Focus();
            EntityDicSampRemark samRemark = bsItem_Prop.AddNew() as EntityDicSampRemark;
            samRemark.RemId = string.Empty;

            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.gridControl1.Enabled = true;
            this.bsItem_Prop.EndEdit();

            if (bsItem_Prop.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicSampRemark dr = bsItem_Prop.Current as EntityDicSampRemark;
            String type_id = dr.RemId;

            request.SetRequestValue(dr);

            

            if (dr.RemContent == null || dr.RemContent.ToString().Trim() == "")
            {
                lis.client.control.MessageDialog.Show("标本类别不能为空！", "提示");
                return;
            }

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
                    dr.RemId = result.GetResult<EntityDicSampRemark>().RemId;
                    gridView1.RefreshData();
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
                throw new Exception(result.ErroMsg);
            }
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                blIsNew = false;//取消新增事件this.gridControl1.Enabled = true;
                this.gridControl1.Enabled = true;
            }
        }

        public void Delete()
        {
            this.bsItem_Prop.EndEdit();
            if (bsItem_Prop.Current == null)
            {
                lis.client.control.MessageDialog.Show("请选择要删除的数据！", "提示");
                return;
            }

            EntityDicSampRemark samRemark = bsItem_Prop.Current as EntityDicSampRemark;
            String br_id = samRemark.RemId;

            EntityRequest request = new EntityRequest();
            request.SetRequestValue(samRemark);
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
                list = result.GetResult() as List<EntityDicSampRemark>;
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
        #endregion
        public ConSamRemarks()
        {

            InitializeComponent();
        }

        private void on_Load(object sender, EventArgs e)
        {
            this.initData();

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

        private void btnName_Leave(object sender, EventArgs e)
        {
            if (bsItem_Prop.Current != null)
            {
                EntityDicSampRemark samRemark = bsItem_Prop.Current as EntityDicSampRemark;
                samRemark.RemPyCode = tookit.GetSpellCode(this.btnName.Text);
                samRemark.RemWbCode = tookit.GetWBCode(this.btnName.Text);
            }
        }


        #region IBarActionExt 成员

        public void Cancel()
        {
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                blIsNew = false;//取消新增事件this.gridControl1.Enabled = true;
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

        
    }
}
