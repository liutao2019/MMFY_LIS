using System;
using System.Collections.Generic;

using System.Text;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraGrid;

namespace dcl.client.common
{
    public class FormatHelper
    {

        public static void FormatRow(GridView gridview, string columnName, string value, Color color)
        {
            string ColorStyle = ConfigHelper.GetSysConfigValueWithoutLogin("BarcodeInfo_ColorStyle");
            GridFormatRule rule = new GridFormatRule();

            rule.Column = gridview.Columns[columnName];
            rule.ColumnApplyTo = gridview.Columns["PidName"];
            //rule.Name = "Format0";
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.Value1 = value;
            if (ColorStyle != "整行字体")
            {
                formatConditionRuleValue1.Appearance.BackColor = color;
            }
            else
            {
                formatConditionRuleValue1.Appearance.ForeColor = color;
            }
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            rule.Rule = formatConditionRuleValue1;
            gridview.FormatRules.Add(rule);
        }

       /// <summary>
       /// 给条码号一个底色
       /// </summary>
       /// <param name="gridview"></param>
       /// <param name="columnName">急查标志列</param>
       /// <param name="value"></param>
       /// <param name="color">颜色</param>
        public static void FormatRowBarcode(GridView gridview, string columnName, bool value, Color color)
        {
            GridFormatRule rule = new GridFormatRule();

            rule.Column = gridview.Columns[columnName];
            rule.ColumnApplyTo = gridview.Columns["SampBarCode"];
            //rule.Name = "Format0";
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.Value1 = value;
            formatConditionRuleValue1.Appearance.BackColor = color;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            rule.Rule = formatConditionRuleValue1;
            gridview.FormatRules.Add(rule);
        }
        public static void FormatRowIdentity(GridView gridview, string columnName, string value, Color color)
        {
            GridFormatRule rule = new GridFormatRule();

            rule.Column = gridview.Columns[columnName];
            rule.ColumnApplyTo = gridview.Columns["PidIdentityName"];
            //rule.Name = "Format0";
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.Value1 = value;
            formatConditionRuleValue1.Appearance.BackColor = color;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            rule.Rule = formatConditionRuleValue1;
            gridview.FormatRules.Add(rule);
        }
        public static void FormatRow(GridView gridview, string columnName, object value1,object value2, Color color)
        {
            StyleFormatCondition cn;
            cn = new StyleFormatCondition(FormatConditionEnum.Between, gridview.Columns[columnName], null, value1, value2);
            cn.ApplyToRow = true;
            cn.Appearance.ForeColor = color;
            if (cn.Column != null)
                gridview.FormatConditions.Add(cn);
        }


        public static void FormatRow(GridView gridview, string columnName, string[] value, Color color)
        {
            foreach (string one in value)
            {
                FormatRow(gridview, columnName, one, color);
            }
        }
    }
}
