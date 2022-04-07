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
    public partial class ConComReptime : ConDicCommon, IBarActionExt
    {
        public ConComReptime()
        {
            InitializeComponent();
            Init();
            this.Name = "ConComReptime";
        }
        void Init()
        {
            this.cbType.SelectedIndexChanged += new EventHandler(cbType_SelectedIndexChanged);
            this.txtDay.ValueChanged += new EventHandler(txtDay_ValueChanged);
            this.txtTime.ValueChanged += new EventHandler(txtTime_ValueChanged);
            this.txtWeek.TextChanged += new EventHandler(txtWeek_TextChanged);
            bsGetReportTime.CurrentChanged += new EventHandler(bsGetReportTime_CurrentChanged);
            //this.barControl1.ValidateDataBeforeSave += new lis.client.control.BarControl.ValidateDataEventHandeler(barControl1_ValidateDataBeforeSave);
            this.txtTime.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDay.Properties.Mask.EditMask = "d";
            this.txtDay.Properties.DisplayFormat.FormatString = "d";
            this.txtDay.Properties.EditFormat.FormatString = "d";
            this.txtDay.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;

            this.txtTime.Properties.EditFormat.FormatString = "f1";
            this.txtDay.Properties.Mask.EditMask = "f1";
            this.txtTime.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.Name = "DicGetReportTime";
            this.txtDay.EditValue = null;
            this.txtTime.EditValue = null;
            SetState();
        }


        /// <summary>
        /// 是否为新增事件
        /// </summary>
        private bool blIsNew = false;

        private List<EntityDicComReptime> list = new List<EntityDicComReptime>();

        #region IBarActionExt

        public void Add()
        {
            blIsNew = true;//标记为新增事件

            this.txtCode.Focus();

            EntityDicComReptime dr = (EntityDicComReptime)bsGetReportTime.AddNew();
            dr.RetId = string.Empty;
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.bsGetReportTime.EndEdit();
            if (bsGetReportTime.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();

            EntityDicComReptime dr = (EntityDicComReptime)bsGetReportTime.Current;
            String retID = dr.RetId;
            int type = this.GetCurrentType();
            if (type == 3)
            {
                if (string.IsNullOrEmpty(this.txtWeek.Text))
                {
                    dr.RetDay = null;
                }
                else
                {
                    dr.RetDay = this.txtWeek.Text.Trim();
                    int valueWeek = 0;
                    if (int.TryParse(dr.RetDay, out valueWeek) && valueWeek > 7)
                    {
                        MessageBox.Show("周数不能超过7天!");
                        bsGetReportTime.Remove(dr);
                        this.gridControl1.RefreshDataSource();
                        return;
                    }
                }
            }
            else
            {
                if (this.txtDay.EditValue == null)
                {
                    dr.RetDay = null;
                }
                else
                {
                    dr.RetDay = this.txtDay.Value.ToString();
                }
            }

            if (dr.RetName == null || dr.RetName == "")
            {
                dr.RetName = GReportTimeName();
            }

            dr.RetType = GetCurrentType();

            dr.StartTime = DateTime.Parse(dr.StartTime).ToString("HH:mm:ss");
            dr.EndTime = DateTime.Parse(dr.EndTime).ToString("HH:mm:ss");

            request.SetRequestValue(dr);

            EntityResponse result = new EntityResponse();
            if (retID == "")
            {
                result = base.New(request);
            }
            else
            {
                result = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                if (retID == "")
                {
                    dr.RetId = result.GetResult<EntityDicComReptime>().RetId;
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");
                DoRefresh();
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
            this.bsGetReportTime.EndEdit();
            if (bsGetReportTime.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicComReptime dr = (EntityDicComReptime)bsGetReportTime.Current;
            String br_id = dr.RetId;

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
                list = ds.GetResult() as List<EntityDicComReptime>;
                bsGetReportTime.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridControl1", true);

            return dlist;
        }

        public void Cancel()
        {
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
                blIsNew = false;//取消新增事件

            this.DoRefresh();
        }

        public void Close() { }

        public void Edit()
        {
            this.isUpdatting = false;
            this.gridControl1.Enabled = false;
        }

        public void MoveNext() { }

        public void MovePrev() { }

        #endregion

        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        private void on_Load(object sender, EventArgs e)
        {
            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);

            this.initData();
            setGridControl();
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

      

        void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtDay.EditValue = null;
            this.txtTime.EditValue = null;
            this.txtWeek.EditValue = null;
            SetState();
        }
        void txtTime_ValueChanged(object sender, EventArgs e)
        {
            GReportTimeName();
        }

        void txtDay_ValueChanged(object sender, EventArgs e)
        {
            GReportTimeName();
        }
        void txtWeek_TextChanged(object sender, EventArgs e)
        {
            GReportTimeName();
        }
        void bsGetReportTime_CurrentChanged(object sender, EventArgs e)
        {
            EntityDicComReptime current = this.bsGetReportTime.Current as EntityDicComReptime;
            if (current != null)
            {

                if (current.RetType != 0)
                {
                    this.cbType.SelectedIndex = current.RetType - 1;
                }
                else
                {
                    this.cbType.EditValue = null;
                }

                this.isUpdatting = true;
                this.txtCode.EditValue = current.RetCode;
                this.memoEdit1.EditValue = current.RetName;
                this.testartTime.EditValue = current.StartTime;
                this.teEndTime.EditValue = current.EndTime;
                int currentType = this.GetCurrentType();
                if (currentType == 3)
                {
                    this.txtWeek.EditValue = current.RetDay;
                }
                else
                {
                    this.txtDay.EditValue = current.RetDay;
                }
                this.txtTime.EditValue = current.RetTime;
            }

        }

        void SetState()
        {
            isUpdatting = true;

            int type = GetCurrentType();
            switch (type)
            {
                case 0:

                    this.labDay.Visible = false;
                    this.txtDay.Visible = false;
                    this.labDayDes.Visible = false;
                    this.txtTime.Visible = false;
                    this.labTime.Visible = false;
                    this.txtWeek.Visible = false;
                    break;
                case 1:
                    this.labDay.Visible = false;
                    this.txtDay.Enabled = false;

                    this.txtDay.Visible = false;

                    this.labDayDes.Visible = false;
                    this.txtTime.Enabled = true;
                    this.txtTime.Visible = true;
                    this.labTime.Visible = true;
                    this.labTime.Text = "小时后";
                    this.txtWeek.Visible = false;
                    break;
                case 2:
                    this.labDay.Visible = false;
                    this.txtDay.Enabled = true;
                    this.txtTime.Visible = true;
                    this.txtDay.Visible = true;
                    this.txtWeek.Visible = false;
                    this.labDayDes.Visible = false;
                    this.labDayDes.Text = "天";
                    this.txtTime.Enabled = true;
                    this.labTime.Visible = true;
                    this.txtDay.Properties.MaxValue = 100;
                    this.txtDay.Properties.MinValue = 0;
                    this.txtTime.Properties.MaxValue = 24;
                    this.txtTime.Properties.MinValue = 0;
                    this.labTime.Text = "点后";
                    break;
                case 3:
                    this.txtTime.Visible = true;
                    this.txtDay.Visible = false;
                    this.labDay.Visible = true;
                    this.txtWeek.Visible = true;
                    this.labDay.Text = "周";
                    this.txtDay.Enabled = true;

                    this.labDayDes.Visible = false;

                    this.txtTime.Enabled = true;
                    this.labTime.Visible = true;
                    this.txtTime.Properties.MaxValue = 24;
                    this.txtTime.Properties.MinValue = 0;
                    this.labTime.Text = "点后";
                    break;
                default:
                    break;
            }
            isUpdatting = false;
        }
        bool isUpdatting = false;

        string GReportTimeName()
        {
            if (isUpdatting)
            {
                return null;
            }
            string name = string.Empty;
            int type = GetCurrentType();
            switch (type)
            {
                case 0:
                    break;
                case 1:
                    if (this.txtTime.EditValue != null)
                    {
                        decimal timeValue = this.txtTime.Value;
                        int hour = (int)timeValue;
                        string mintext = string.Empty;
                        if (hour > 0)
                        {
                            name += hour.ToString() + "小时";
                        }
                        if (timeValue > hour)
                        {
                            name += ((int)((timeValue - hour) * 60)).ToString() + "分钟";
                        }

                    }

                    break;
                case 2:
                    if (this.txtDay.EditValue != null && this.txtDay.Value > 0)
                    {
                        name += this.txtDay.Value.ToString() + "天";
                    }
                    if (this.txtTime.EditValue != null && this.txtTime.Value > 0)
                    {

                        name += GetTimeValueText();
                        if (!name.Contains(":"))
                        {
                            name += "点";
                        }

                    }

                    break;
                case 3:
                    if (!string.IsNullOrEmpty(this.txtWeek.Text))
                    {
                        string weekText = ConvertToMutiWeekDayText(this.txtWeek.Text);
                        if (!string.IsNullOrEmpty(weekText))
                        {
                            name += "周" + weekText;
                        }

                    }
                    if (this.txtTime.EditValue != null && this.txtTime.Value > 0)
                    {
                        name += "的" + GetTimeValueText();
                        if (!name.Contains(":"))
                        {
                            name += "点";
                        }
                    }
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(name))
            {
                name += "后";
            }
            this.memoEdit1.EditValue = name;
            bsGetReportTime.EndEdit();
            return name;
        }

        int GetCurrentType()
        {
            return this.cbType.SelectedIndex + 1;
        }

        string ConvertToMutiWeekDayText(string text)
        {
            string result = string.Empty;
            string[] weekDayArray = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (weekDayArray.Length > 0)
            {
                foreach (string item in weekDayArray)
                {
                    int value = 0;
                    if (int.TryParse(item, out value) && value > 0 && value <= 7)
                    {
                        if (!string.IsNullOrEmpty(result))
                        {
                            result += ",";
                        }
                        result += ConvertToWeekText(value);
                    }
                }
            }

            return result;
        }

        string GetTimeValueText()
        {
            string result = string.Empty;
            if (this.txtTime.EditValue != null)
            {
                decimal timeValue = this.txtTime.Value;
                int hour = (int)timeValue;
                string mintext = string.Empty;
                if (hour > 0)
                {
                    result += hour.ToString();
                }
                if (timeValue > hour)
                {
                    result += ":" + ((int)((timeValue - hour) * 60)).ToString();
                }

            }
            return result;
        }

        string ConvertToWeekText(int value)
        {
            string result = string.Empty;
            switch (value)
            {
                case 1:
                    result = "一";
                    break;
                case 2:
                    result = "二";
                    break;
                case 3:
                    result = "三";
                    break;
                case 4:
                    result = "四";
                    break;
                case 5:
                    result = "五";
                    break;
                case 6:
                    result = "六";
                    break;
                case 7:
                    result = "日";
                    break;
                default:
                    break;
            }
            return result;
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
