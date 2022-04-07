using System;
using System.Collections.Generic;

using System.Text;
using dcl.common;
using dcl.client.frame;

namespace lis.client.control
{
    public class TextAgeInput : DevExpress.XtraEditors.TextEdit
    {
        private const string YearChs = "岁";
        private const string MonthChs = "月";
        private const string DayChs = "天";
        private const string HourChs = "时";
        private const string MinuteChs = "分";

        private const string YearSpliter = "Y";
        private const string MonthSpliter = "M";
        private const string DaySpliter = "D";
        private const string HourSpliter = "H";
        private const string MinuteSpliter = "m";
        private const string MinuteSpliter2 = "I";

        private string EmptyText;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit fProperties;

        private string EditFormat = "YMDHI";

        public TextAgeInput()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                string temp_format = UserInfo.GetSysConfigValue("Lab_AgeInputFormat");
                if (!string.IsNullOrEmpty(temp_format))
                {
                    EditFormat = temp_format;
                }
            }

            this.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Default;
            this.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Regular;
            this.Properties.Mask.SaveLiteral = true;
            this.Properties.Mask.ShowPlaceHolders = true;
            this.Properties.Mask.IgnoreMaskBlank = false;

            if (EditFormat == "YMD")
            {
                EmptyText = string.Format("   {0}  {1}  {2}", YearChs, MonthChs, DayChs);
                this.Properties.Mask.EditMask = @"\d?\d?" + YearChs + @"\d?\d?" + MonthChs + @"\d?\d?" + DayChs;
            }
            else if (EditFormat == "Y")
            {
                EmptyText = string.Format("   {0}", YearChs);
                this.Properties.Mask.EditMask = @"\d?\d?" + YearChs;
            }
            else//YMDHI
            {
                EmptyText = string.Format("   {0}  {1}  {2}  {3}  {4}", YearChs, MonthChs, DayChs, HourChs, MinuteChs);
                this.Properties.Mask.EditMask = @"\d?\d?" + YearChs + @"\d?\d?" + MonthChs + @"\d?\d?" + DayChs + @"\d?\d?" + HourChs + @"\d?\d?" + MinuteChs;
            }

            this.Properties.NullText = EmptyText;
            //文本编辑器掩码

            this.Properties.Mask.PlaceHolder = ' ';
            this.Properties.Mask.UseMaskAsDisplayFormat = true;

        }

        public int? AgeYear
        {
            get
            {
                if (this.EditValue != null && this.EditValue.ToString().Trim() != string.Empty)
                {
                    string value = this.EditValue.ToString();
                    string strYear = value.Split(new string[] { YearChs }, StringSplitOptions.None)[0];

                    if (strYear != null && strYear.Trim(null) != string.Empty)
                    {
                        return Convert.ToInt32(strYear);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public int? AgeMonth
        {
            get
            {
                if (this.EditValue != null && this.EditValue.ToString().Trim() != string.Empty)
                {
                    if (this.EditFormat == "Y")
                    {
                        return null;
                    }

                    string value = this.EditValue.ToString();
                    string strMonth = value.Split(new string[] { MonthChs }, StringSplitOptions.None)[0];
                    strMonth = strMonth.Split(new string[] { YearChs }, StringSplitOptions.None)[1];
                    if (strMonth != null && strMonth.Trim(null) != string.Empty)
                    {
                        return Convert.ToInt32(strMonth);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public int? AgeDay
        {
            get
            {
                if (this.EditValue != null && this.EditValue.ToString().Trim() != string.Empty)
                {
                    if (this.EditFormat == "Y")
                    {
                        return null;
                    }

                    string value = this.EditValue.ToString();
                    string strDay = value.Split(new string[] { DayChs }, StringSplitOptions.None)[0];
                    strDay = strDay.Split(new string[] { MonthChs }, StringSplitOptions.None)[1];
                    if (strDay != null && strDay.Trim(null) != string.Empty)
                    {
                        return Convert.ToInt32(strDay);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public int? AgeHour
        {
            get
            {
                if (this.EditValue != null && this.EditValue.ToString().Trim() != string.Empty)
                {
                    if (this.EditFormat == "YMD" || this.EditFormat == "Y")
                    {
                        return null;
                    }

                    string value = this.EditValue.ToString();
                    string strHour = value.Split(new string[] { HourChs }, StringSplitOptions.None)[0];
                    strHour = strHour.Split(new string[] { DayChs }, StringSplitOptions.None)[1];
                    if (strHour != null && strHour.Trim(null) != string.Empty)
                    {
                        return Convert.ToInt32(strHour);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public int? AgeMinute
        {
            get
            {
                if (this.EditValue != null && this.EditValue.ToString().Trim() != string.Empty)
                {
                    if (this.EditFormat == "YMD" || this.EditFormat == "Y")
                    {
                        return null;
                    }

                    string value = this.EditValue.ToString();
                    string strMinute = value.Split(new string[] { MinuteChs }, StringSplitOptions.None)[0];
                    strMinute = strMinute.Split(new string[] { HourChs }, StringSplitOptions.None)[1];
                    if (strMinute != null && strMinute.Trim(null) != string.Empty)
                    {
                        return Convert.ToInt32(strMinute);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 年龄存储值
        /// </summary>
        public string AgeValueText
        {
            get
            {
                if (this.EditValue != null
                    && this.EditValue != DBNull.Value
                    && this.EditValue.ToString().Replace(" ", "") == YearChs + MonthChs + DayChs + HourChs + MinuteChs)
                {
                    return string.Empty;
                }
                else
                {
                    string v = null;

                    if (AgeYear == null && AgeMonth == null && AgeDay == null && AgeHour == null && AgeMinute == null)
                    {
                    }
                    else
                    {

                        v = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                            AgeYear == null ? 0 : AgeYear, YearSpliter,
                                        AgeMonth == null ? 0 : AgeMonth, MonthSpliter,
                                        AgeDay == null ? 0 : AgeDay, DaySpliter,
                                        AgeHour == null ? 0 : AgeHour, HourSpliter,
                                        AgeMinute == null ? 0 : AgeMinute, MinuteSpliter2);
                    }
                    return v;
                }
            }
            set
            {
                if (
                    value != null
                    && value.Contains(YearSpliter)
                    && value.Contains(MonthSpliter)
                    && value.Contains(DaySpliter)
                    && value.Contains(HourSpliter)
                    && (value.Contains(MinuteSpliter) || value.Contains(MinuteSpliter2))
                    )
                {
                    string txtValue = value.Replace(YearSpliter, YearChs)
                                           .Replace(MonthSpliter, MonthChs)
                                           .Replace(DaySpliter, DayChs)
                                           .Replace(HourSpliter, HourChs)
                                           .Replace(MinuteSpliter, MinuteChs)
                                           .Replace(MinuteSpliter2, MinuteChs);

                    if (txtValue == "0岁0月0天0时0分")
                    {
                        this.EditValue = "0岁  月  天  时  分";
                    }
                    else
                    {
                        this.EditValue = txtValue;


                        string Y = "   ";
                        if (AgeYear != 0) Y = AgeYear.ToString();

                        string M = "  ";
                        if (AgeMonth != 0) M = AgeMonth.ToString();

                        string D = "  ";
                        if (AgeDay != 0) D = AgeDay.ToString();

                        string H = "  ";
                        if (AgeHour != 0) H = AgeHour.ToString();

                        string m = "  ";
                        if (AgeMinute != 0) m = AgeMinute.ToString();

                        this.EditValue = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                                    Y, YearChs,
                                    M, MonthChs,
                                    D, DayChs,
                                    H, HourChs,
                                    m, MinuteChs);
                    }
                }
                else
                {
                    this.EditValue = this.EmptyText;
                }
            }
        }

        /// <summary>
        /// 获取以分钟为单位的年龄
        /// </summary>
        public int AgeToMinute
        {
            get
            {
                if (this.AgeYear == null
                    && this.AgeMonth == null
                    && this.AgeDay == null
                    && this.AgeHour == null
                    && this.AgeMinute == null
                    )
                {
                    return -1;
                }
                else
                {

                    int m = AgeConverter.ToMinute(
                        this.AgeYear == null ? 0 : this.AgeYear.Value,
                        this.AgeMonth == null ? 0 : this.AgeMonth.Value,
                        this.AgeDay == null ? 0 : this.AgeDay.Value,
                        this.AgeHour == null ? 0 : this.AgeHour.Value,
                        this.AgeMinute == null ? 0 : this.AgeMinute.Value
                        );

                    if (m < 0)
                    {
                        m = -1;
                    }
                    return m;
                }
            }
        }

        private void InitializeComponent()
        {
            this.fProperties = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.fProperties)).BeginInit();
            this.SuspendLayout();
            // 
            // fProperties
            // 
            this.fProperties.Name = "fProperties";
            // 
            // TextAgeInput
            // 
            this.Size = new System.Drawing.Size(100, 21);
            this.TextChanged += new System.EventHandler(this.TextAgeInput_TextChanged);
            ((System.ComponentModel.ISupportInitialize)(this.fProperties)).EndInit();
            this.ResumeLayout(false);

        }


        private void TextAgeInput_TextChanged(object sender, EventArgs e)
        {
            string strAgeMonth = AgeMonth.ToString();


            string strAgeDay = AgeDay.ToString();

            string strAgeHour = AgeHour.ToString();

            string strAgeMinute = AgeMinute.ToString();


            if (!string.IsNullOrEmpty(strAgeMonth))
            {
                if (Convert.ToInt16(strAgeMonth) > 12)
                {
                    //月份大于12时是否提示
                    if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("IsAgeMonthOverNotice") == "否")
                    {
                    }
                    else
                    {
                        MessageDialog.Show("输入的月份不能大于12！", "输入有误");
                    }
                    this.Select(this.Text.IndexOf(YearChs) + 1, 3);
                }
            }
            if (!string.IsNullOrEmpty(strAgeDay))
            {
                if (Convert.ToInt16(strAgeDay) > 31)
                {
                    if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("IsAgeDayOverNotice") == "否")
                    {
                    }
                    else
                    {
                        MessageDialog.Show("输入的天数不能大于31！", "输入有误");
                    }
                    this.Select(this.Text.IndexOf(MonthChs) + 1, 3);
                }
            }
            if (!string.IsNullOrEmpty(strAgeHour))
            {
                if (Convert.ToInt16(strAgeHour) > 24)
                {
                    MessageDialog.Show("输入的小时不能大于24！", "输入有误");
                    this.Select(this.Text.IndexOf(DayChs) + 1, 3);
                }
            }
            if (!string.IsNullOrEmpty(strAgeMinute))
            {
                if (Convert.ToInt16(strAgeMinute) > 60)
                {
                    MessageDialog.Show("输入的分钟不能大于60！", "输入有误");
                    this.Select(this.Text.IndexOf(HourChs) + 1, 3);
                }
            }

            //MessageDialog.Show(strAgeYear);
            //MessageDialog.Show(strAgeMonth);
            //MessageDialog.Show(strAgeDay);
            //MessageDialog.Show(strAgeHour);
            //MessageDialog.Show(strAgeMinute);
        }


    }
}
