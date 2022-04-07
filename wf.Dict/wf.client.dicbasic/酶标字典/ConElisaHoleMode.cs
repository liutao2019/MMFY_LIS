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

namespace dcl.client.dicbasic
{
    [DesignTimeVisible(false)]
    public partial class ConElisaHoleMode : ConDicCommon, IBarActionExt
    {
        public ConElisaHoleMode()
        {
            InitializeComponent();
            DoRefresh();

            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);
        }

        public void Add()
        {
            EntityDicElisaSort dr = (EntityDicElisaSort)bsEiasaHoleMode.AddNew();
        }

        public void Cancel()
        {
            DoRefresh();
        }

        public void Close()
        {
        }

        public void Delete()
        {
            this.bsEiasaHoleMode.EndEdit();
            if (bsEiasaHoleMode.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicElisaSort dr = (EntityDicElisaSort)bsEiasaHoleMode.Current;
            request.SetRequestValue(dr);
            EntityResponse result = new EntityResponse();
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
        private List<EntityDicElisaSort> list = new List<EntityDicElisaSort>();
        public void DoRefresh()
        {
            EntityDicElisaSort dt = new EntityDicElisaSort();
            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dt);
            EntityResponse ds = base.Search(request);
            if (isActionSuccess)
            {
                list = ds.GetResult() as List<EntityDicElisaSort>;
                bsEiasaHoleMode.DataSource = list;
            }
        }

        public void Edit()
        {
            gcHoleMode.Enabled = true;
        }

        public Dictionary<string, bool> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gcHoleMode", true);
            dlist.Add("simpleButton1", true);

            return dlist;
        }

        public void MoveNext()
        {
            throw new NotImplementedException();
        }

        public void MovePrev()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            this.bsEiasaHoleMode.EndEdit();
            if (bsEiasaHoleMode.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicElisaSort dr = (EntityDicElisaSort)bsEiasaHoleMode.Current;
            if (string.IsNullOrEmpty(dr.SortName.ToString()))
            {
                lis.client.control.MessageDialog.Show("请填写孔位序号模式", "提示信息");
                return;
            }
            String sort_id = dr.SortId;
            dr.SortHoleSorting = sampleControl1.FormatHoleValues;
            request.SetRequestValue(dr);
            EntityResponse result = new EntityResponse();
            if (sort_id == "" || sort_id == null)
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


    }
}
