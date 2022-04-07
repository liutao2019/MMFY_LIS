using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.client.wcf;

using dcl.entity;
using lis.client.control;
using System.Linq;

namespace dcl.client.dicbasic
{
    public partial class conItemUrgentValue : ConDicCommon, IBarActionExt
    {
        public conItemUrgentValue()
        {
            InitializeComponent();
        }

        private void conItemUrgentValue_Load(object sender, EventArgs e)
        {
            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);

            if (!DesignMode)
            {
                DoRefresh();
            }
        }

        #region IBarActionExt 成员

        public void Cancel()
        {
            this.bsList.CancelEdit();
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

        #region IBarAction 成员

        public void Add()
        {
            this.selectDict_Depart1.displayMember = null;
            this.selectDict_Depart1.valueMember = null;

            this.selectDict_Item1.displayMember = null;
            this.selectDict_Item1.valueMember = null;

            this.selectDict_Sample1.displayMember = null;
            this.selectDict_Sample1.valueMember = null;
            this.gridControl1.Enabled = false;

            this.bsList.AddNew();
        }

        public void Delete()
        {
            this.bsList.EndEdit();
            if (bsList.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicUtgentValue dr = (EntityDicUtgentValue)bsList.Current;

            request.SetRequestValue(dr);
            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            if (dresult == DialogResult.OK)
            {
                base.Delete(request);
                if (base.isActionSuccess)
                {
                    MessageDialog.ShowAutoCloseDialog("删除成功");
                    this.DoRefresh();
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("删除失败");
                }
            }
        }

        private List<EntityDicUtgentValue> list = new List<EntityDicUtgentValue>();
        private List<EntityDicPubIcd> Icdlist = new List<EntityDicPubIcd>();

        public void DoRefresh()
        {
            List<EntityDicItmItem> dtItem = new List<EntityDicItmItem>();
            dtItem = selectDict_Item1.dtSource as List<EntityDicItmItem>;
            dtItem = dtItem.Where(i => i.ItmDelFlag == "0").ToList();
            selectDict_Item1.dtSource = dtItem;
            EntityRequest IcdRequest = new EntityRequest();
            EntityDicPubIcd Icd = new EntityDicPubIcd();
            IcdRequest.SetRequestValue(Icd);
            EntityResponse result = base.View(IcdRequest);
            EntityRequest request = new EntityRequest();
            EntityDicUtgentValue dr = new EntityDicUtgentValue();
            request.SetRequestValue(dr);
            EntityResponse ds = base.Search(request);
            if (isActionSuccess)
            {
                Icdlist = result.GetResult() as List<EntityDicPubIcd>;
                bsDiagnos.DataSource = Icdlist;
                list = ds.GetResult() as List<EntityDicUtgentValue>;
                this.bsList.DataSource = list;
                if(bsList.Current != null)
                {
                    bsEditingItem.DataSource = this.bsList.Current as EntityDicUtgentValue;
                    if (txtIcdName.Text != null || txtIcdName.Text != "")
                    {
                        sb.Append(txtIcdName.Text);
                    }
                }
            }
        }

        /// <summary>
        /// 非编辑状态下可用控件
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, bool> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();

            dlist.Add("gridControl1", true);
            return dlist;
        }

        public void Save()
        {
            this.bsEditingItem.EndEdit();
            this.bsList.EndEdit();
            EntityRequest request = new EntityRequest();
            EntityDicUtgentValue list = (EntityDicUtgentValue)bsList.Current;
            request.SetRequestValue(list);
            String type_id = list.UgtKey.ToString();
            request.SetRequestValue(list);
            EntityResponse result = new EntityResponse();
            if (type_id == "-1")
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
            }
            else
            {
                lis.client.control.MessageDialog.Show("保存失败", "提示信息");
            }
        }

        #endregion

        private void bsList_CurrentChanged(object sender, EventArgs e)
        {
            EntityDicUtgentValue current = this.bsList.Current as EntityDicUtgentValue;
            if (current == null)
            {
                bsEditingItem.Clear();
            }
            else
            {
                bsEditingItem.DataSource = current;
                sb = new StringBuilder();
                sb.Append(current.UgtIcdName);
            }
        }

        StringBuilder sb = new StringBuilder();

        private void gridView1_Click(object sender, EventArgs e)
        {
            EntityDicPubIcd current = this.bsDiagnos.Current as EntityDicPubIcd;
            sb = new StringBuilder();
            sb.Append(txtIcdName.Text);

            if (sb.Length == 0)
            {
                sb.Append(current.IcdName);
            }
            else
            {
                if (!sb.ToString().Contains(current.IcdName))
                {
                    sb.Append(",");
                    sb.Append(current.IcdName);
                }
            }
            txtIcdName.Text = sb.ToString();
        }

        private void txtExtmax_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
