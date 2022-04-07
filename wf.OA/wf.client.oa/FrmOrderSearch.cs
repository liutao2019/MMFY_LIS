using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using DevExpress.XtraEditors;
using dcl.client.frame;
using lis.client.control;
using System.Configuration;
using System.IO;
using System.Net;
using dcl.client.wcf;
using dcl.entity;
using System.Linq;
using dcl.client.control;

namespace dcl.client.oa
{
    public partial class FrmOrderSearch : FrmCommon
    {
        //单证类型
        List<EntityOaTable> listTable = new List<EntityOaTable>();
        List<EntityOaTableField> listField = new List<EntityOaTableField>();
        List<EntityOaTableDetail> tabDetail = new List<EntityOaTableDetail>();
        //DataTable dtOrderDetail = null;

        string OrderTypeCode = "";
        ProxyOaTableDetail proxyDetail = new ProxyOaTableDetail();
        ProxyOrderTable proxyTable = new ProxyOrderTable();

        public FrmOrderSearch()
        {
            InitializeComponent();

            this.sysToolBar1.OnBtnSearchClicked += new System.EventHandler(this.sysToolBar1_OnBtnSearchClicked);
            this.sysToolBar1.OnBtnExportClicked += new System.EventHandler(this.sysToolBar1_OnBtnExportClicked);
            this.sysToolBar1.BtnResetClick += new System.EventHandler(this.sysToolBar1_BtnResetClick);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmOrderSearch_Load(object sender, EventArgs e)
        {
            //初始化按钮
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnSearch.Name, sysToolBar1.BtnExport.Name, sysToolBar1.BtnReset.Name });

            //初始化单证类别下拉框
            listTable = proxyTable.Service.GetOrderTable(new EntityRequest()).GetResult() as List<EntityOaTable>;
            List<EntityOaTable> sourceTable = new List<EntityOaTable>();
            //不显示部分系统限定单证类型
            List<string> orderTypeCode = new List<string>() { "3", "4", "5", "6", "7", "8", "9", "11", "12", "13", "14", "15", "34", "35", "36", "38", "41" };
            sourceTable = (from x in listTable where orderTypeCode.Contains(x.TabCode) select x).OrderBy(i => i.TabName).ToList();

            foreach (EntityOaTable dr in sourceTable)
            {
                if (dr.TabName == "保养录入" || dr.TabName == "仪器维修" || dr.TabName == "保养字典")
                    continue;
                cboOrderType.Properties.Items.Add(dr.TabName.ToString());
            }

            DateTime time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            timeFrom.EditValue = time;
            timeTo.EditValue = time.AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 生成查询条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabDetail = null;
            tvOrderDetail.DataSource = null;

            if (cboOrderType.SelectedIndex != -1)
            {
                string orderTypeName = cboOrderType.Text;

                OrderTypeCode = listTable.Find(i => i.TabName == orderTypeName).TabCode;

                LoadEdit();
            }
            else
            {
                OrderTypeCode = "";
            }
        }

        /// <summary>
        /// 生成控件编辑框和列
        /// </summary>
        public void LoadEdit()
        {
            pnlDetail.Visible = false;
            pnlDetail.Controls.Clear();

            string tabCode = OrderTypeCode;
            listField = proxyDetail.Service.GetOrderTableField(tabCode);

            for (int i = 0; i < listField.Count; i++)
            {
                //DataRow drItem = dtOrderItem.Rows[i];
                EntityOaTableField tableField = listField[i];

                if (tableField.FieldListDisplay.ToString() != "1")
                {
                    continue;
                }

                Panel panel = new Panel();
                panel.Name = "pnl" + i.ToString(OrderDetail.ZeroFormat);
                panel.Dock = DockStyle.Top;
                panel.Height = 26;

                Panel panelA = new Panel();
                panelA.Name = "pna" + i.ToString(OrderDetail.ZeroFormat);
                panelA.Dock = DockStyle.Top;
                panelA.Height = 26;

                Panel panelB = new Panel();
                panelB.Name = "pnb" + i.ToString(OrderDetail.ZeroFormat);
                panelB.Dock = DockStyle.Top;
                panelB.Height = 0;

                if (tableField.FieldType.ToString() == "数字" || tableField.FieldType.ToString() == "日期" || tableField.FieldType.ToString() == "时间")
                {
                    //生成起止查询
                    CreatePanel(tableField, ref panelB, false);
                    panelB.Height = 26;
                    panel.Height = 52;
                }

                CreatePanel(tableField, ref panelA, true);


                panel.Controls.Add(panelA);
                panel.Controls.Add(panelB);
                panelA.SendToBack();
                pnlDetail.Controls.Add(panel);
                panel.BringToFront();

                Panel panelSpace = new Panel();
                panelSpace.Name = "pns" + i.ToString(OrderDetail.ZeroFormat);
                panelSpace.Dock = DockStyle.Top;
                panelSpace.Height = 5;
                pnlDetail.Controls.Add(panelSpace);
                panelSpace.BringToFront();
            }
            pnlDetail.Visible = true;

            //生成列数据
            tvOrderDetail.Columns.Clear();
            DevExpress.XtraTreeList.Columns.TreeListColumn colOrderCode = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            colOrderCode.Caption = "OrderCode";
            colOrderCode.FieldName = "DetId";
            colOrderCode.Name = "colOrderCode";
            colOrderCode.VisibleIndex = 0;
            colOrderCode.OptionsColumn.AllowEdit = false;
            tvOrderDetail.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] { colOrderCode });
            colOrderCode.Visible = false;

            foreach (EntityOaTableField thisDr in listField)
            {
                DevExpress.XtraTreeList.Columns.TreeListColumn col = new DevExpress.XtraTreeList.Columns.TreeListColumn();
                col.Caption = thisDr.FieldName.ToString();

                col.FieldName = "fld" + int.Parse(thisDr.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
                col.Name = "col" + int.Parse(thisDr.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
                col.VisibleIndex = int.Parse(thisDr.FieldIndex.ToString()) + 1;
                col.OptionsColumn.AllowEdit = false;
                //col.OptionsColumn.FixedWidth = true;
                //col.Width = 100;
                tvOrderDetail.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] { col });
                if (thisDr.FieldListDisplay.ToString() == "1")
                {
                    col.Visible = true;
                }
                else
                {
                    col.Visible = false;
                }
            }
        }

        /// <summary>
        /// 创建一个单证字段输入
        /// </summary>
        /// <param name="drItem"></param>
        /// <param name="panel"></param>
        private void CreatePanel(EntityOaTableField tableField, ref Panel panel, bool isFirst)
        {
            LabelControl lab = new LabelControl();
            lab.Name = "lab" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
            if (isFirst)
            {
                if (tableField.FieldType.ToString() == "数字" || tableField.FieldType.ToString() == "日期" || tableField.FieldType.ToString() == "时间")
                {
                    lab.Text = tableField.FieldName.ToString() + " 从";
                }
                else
                {
                    lab.Text = tableField.FieldName.ToString();
                }
            }
            else
            {
                lab.Text = "到";
            }
            lab.Dock = DockStyle.Right;
            panel.Controls.Add(lab);


            Panel spaceA = new Panel();
            spaceA.Name = "spa" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
            spaceA.Dock = DockStyle.Right;
            spaceA.Width = 5;
            panel.Controls.Add(spaceA);


            if (tableField.FieldType.ToString() == "数字")
            {
                SpinEdit txtEdit = new SpinEdit();
                txtEdit.Properties.MaxValue = 99999999;
                txtEdit.Properties.IsFloatValue = true;
                if (isFirst)
                {
                    txtEdit.Name = "txt" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
                    txtEdit.Text = "0";
                }
                else
                {
                    txtEdit.Name = "too" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
                    txtEdit.Text = "99999999";
                }
                txtEdit.EnterMoveNextControl = true;
                txtEdit.Width = 140;
                txtEdit.Dock = DockStyle.Right;
                panel.Controls.Add(txtEdit);
            }

            if (tableField.FieldType.ToString() == "字符串" || tableField.FieldType.ToString() == "文件" || tableField.FieldType.ToString() == "文本")
            {
                TextEdit txtEdit = new TextEdit();
                if (isFirst)
                {
                    txtEdit.Name = "txt" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
                }
                else
                {
                    txtEdit.Name = "too" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
                }
                txtEdit.EnterMoveNextControl = true;
                txtEdit.Width = 140;
                txtEdit.Dock = DockStyle.Right;
                panel.Controls.Add(txtEdit);
            }

            if (tableField.FieldType.ToString() == "日期")
            {
                DateEdit txtEdit = new DateEdit();
                txtEdit.Text = "";
                if (isFirst)
                {
                    txtEdit.Name = "txt" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
                }
                else
                {
                    txtEdit.Name = "too" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
                }
                txtEdit.EnterMoveNextControl = true;
                txtEdit.Width = 140;
                txtEdit.Dock = DockStyle.Right;
                panel.Controls.Add(txtEdit);
            }

            if (tableField.FieldType.ToString() == "时间")
            {
                TimeEdit txtEdit = new TimeEdit();
                if (isFirst)
                {
                    txtEdit.Name = "txt" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
                    txtEdit.Text = "00:00:00";
                }
                else
                {
                    txtEdit.Name = "too" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
                    txtEdit.Text = "23:59:59";
                }
                txtEdit.EnterMoveNextControl = true;
                txtEdit.Width = 140;
                txtEdit.Dock = DockStyle.Right;
                panel.Controls.Add(txtEdit);
            }

            if (tableField.FieldType.ToString() == "枚举")
            {
                if (tableField.FieldDictList.ToString() != "" && (tableField.FieldDictSql.ToString() != "" || tableField.FieldDictList.ToString().IndexOf(",") != -1))
                {
                    ComboBoxEdit txtEdit = new ComboBoxEdit();
                    if (isFirst)
                    {
                        txtEdit.Name = "txt" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
                    }
                    else
                    {
                        txtEdit.Name = "too" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
                    }
                    txtEdit.EnterMoveNextControl = true;
                    txtEdit.Width = 140;
                    txtEdit.Dock = DockStyle.Right;

                    string dictName = tableField.FieldDictList.ToString();

                    try
                    {
                        if (dictName.IndexOf(",") != -1)
                        {
                            //文本枚举
                            string[] items = dictName.Split(',');
                            txtEdit.Properties.Items.AddRange(items);
                        }
                        else
                        {
                            //数据字典
                            List<EntityDicItmItem> listItem = proxyDetail.Service.GetItemList();

                            foreach (EntityDicItmItem drDict in listItem)
                            {
                                txtEdit.Properties.Items.Add(drDict.ItmName);
                            }

                        }
                    }
                    catch
                    {
                        txtEdit.Text = "字典绑定出错";
                    }

                    panel.Controls.Add(txtEdit);
                }
                else
                {
                    //使用系统自定义控件
                    string dictName = tableField.FieldDictList.ToString();
                    UserControl txtEdit = new UserControl();

                    if (dictName == "SelectDict_Type")
                    {
                        txtEdit = new SelectDicLabProfession();
                    }

                    if (dictName == "SelectDict_Instrmt")
                    {
                        txtEdit = new SelectDicInstrument();
                    }

                    if (isFirst)
                    {
                        txtEdit.Name = "txt" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
                    }
                    else
                    {
                        txtEdit.Name = "too" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
                    }
                    txtEdit.Width = 140;
                    txtEdit.Dock = DockStyle.Right;
                    //txtEdit.EnterMoveNext = true;
                    //txtEdit.SaveSourceID = true;
                    //txtEdit.SelectOnly = true;

                    panel.Controls.Add(txtEdit);
                }
            }

            Panel spaceB = new Panel();
            spaceB.Name = "spb" + int.Parse(tableField.FieldCode.ToString()).ToString(OrderDetail.ZeroFormat);
            spaceB.Dock = DockStyle.Right;
            spaceB.Width = 10;
            panel.Controls.Add(spaceB);
        }

        /// <summary>
        /// 导出文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (tvOrderDetail.DataSource != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", OfficeMessage.BASE_TITLE);
                        return;
                    }

                    try
                    {
                        tvOrderDetail.ExportToXls(ofd.FileName);
                        lis.client.control.MessageDialog.Show("导出成功！", OfficeMessage.BASE_TITLE);
                    }
                    catch (Exception)
                    {
                    }
                }

            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            if (OrderTypeCode == "")
            {
                return;
            }

            EntityOaTableDetail detail = new EntityOaTableDetail();
            detail.TabCode = OrderTypeCode;
            detail.StartDate = Convert.ToDateTime(timeFrom.EditValue);
            detail.EndDate = Convert.ToDateTime(timeTo.EditValue);

            tabDetail = proxyDetail.Service.GetTabDetailByTabCode(detail);

            DataTable dtOrderDetail = new DataTable();
            dtOrderDetail.Columns.Add("DetId");
            dtOrderDetail.Columns.Add("DetDate");
            dtOrderDetail.Columns.Add("DetContent");

            foreach (var item in tabDetail)
            {
                DataRow dr = dtOrderDetail.NewRow();
                dr["DetId"] = item.DetId;
                dr["DetDate"] = item.DetDate;
                dr["DetContent"] = item.DetContent;
                dtOrderDetail.Rows.Add(dr);
            }

            foreach (DevExpress.XtraTreeList.Columns.TreeListColumn col in tvOrderDetail.Columns)
            {
                if (dtOrderDetail.Columns.Contains(col.FieldName) == false)
                {
                    dtOrderDetail.Columns.Add(col.FieldName);
                }
            }
            foreach (DataRow thisDr in dtOrderDetail.Rows)
            {
                string[] orderDetail = thisDr["DetContent"].ToString().Split(OrderDetail.split);

                for (int i = 1; i < orderDetail.Length - 1; i = i + 2)
                {
                    string tmpDetail = (int.Parse(orderDetail[i])).ToString(OrderDetail.ZeroFormat);
                    if (dtOrderDetail.Columns.Contains("fld" + tmpDetail))
                    {
                        if (("fld" + tmpDetail) == "fld00000224")
                        {
                            thisDr["fld" + tmpDetail] = Convert.ToDateTime(orderDetail[i + 1]).Date.ToShortDateString();
                        }
                        else
                        {
                            thisDr["fld" + tmpDetail] = orderDetail[i + 1];
                        }

                    }
                }
            }

            //dtOrderDetail = dtSource;
            //}

            string where = "1=1";
            GetValue(pnlDetail, ref where);

            DataView dv = new DataView(dtOrderDetail);
            dv.RowFilter = where;

            tvOrderDetail.DataSource = dv;
            tvOrderDetail.BestFitColumns(false);
        }

        /// <summary>
        /// 获得文本数据
        /// </summary>
        /// <param name="control"></param>
        /// <param name="enable"></param>
        private void GetValue(Control control, ref string result)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextEdit && ((TextEdit)c).Text != null && ((TextEdit)c).Text != "")
                {
                    if (c is SpinEdit || c is DateEdit || c is TimeEdit)
                    {
                        if (c.Name.Substring(0, 3) == "txt")
                        {
                            if (c is DateEdit)
                            {
                                result += " and fld" + c.Name.Substring(3) + " >= '" + Convert.ToDateTime(((TextEdit)c).Text.Trim()) + "'";
                            }
                            else
                            {
                                result += " and fld" + c.Name.Substring(3) + " >= '" + ((TextEdit)c).Text.Trim() + "'";
                            }
                        }
                        else
                        {
                            if (c is DateEdit)
                            {
                                result += " and fld" + c.Name.Substring(3) + " <= '" + Convert.ToDateTime(((TextEdit)c).Text.Trim().Replace("'", "''")) + "'";
                            }
                            else
                            {
                                result += " and fld" + c.Name.Substring(3) + " <= '" + ((TextEdit)c).Text.Trim().Replace("'", "''") + "'";
                            }
                        }
                    }
                    else
                    {
                        if (c is ComboBoxEdit)
                        {
                            result += " and fld" + c.Name.Substring(3) + " = '" + ((TextEdit)c).Text.Trim().Replace("'", "''") + "'";
                        }
                        else
                        {
                            result += " and fld" + c.Name.Substring(3) + " like '%" + ((TextEdit)c).Text.Trim().Replace("'", "''") + "%'";
                        }
                    }
                }

                if (c is UserControl)
                {
                    if (c is SelectDicLabProfession && ((SelectDicLabProfession)c).displayMember != null && ((SelectDicLabProfession)c).displayMember != "")
                        result += " and fld" + c.Name.Substring(3) + " = '" + ((SelectDicLabProfession)c).displayMember.Trim() + "'";
                    if (c is SelectDicInstrument && ((SelectDicInstrument)c).displayMember != null && ((SelectDicInstrument)c).displayMember != "")
                        result += " and fld" + c.Name.Substring(3) + " = '" + ((SelectDicInstrument)c).displayMember.Trim() + "'";
                }

                if (!(c is TextEdit || c is UserControl))
                {
                    //如果是需要的底层控件则不再继续往下
                    GetValue(c, ref result);
                }
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeFrom_TextChanged(object sender, EventArgs e)
        {
            tabDetail = null;
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnResetClick(object sender, EventArgs e)
        {
            if (OrderTypeCode != "")
            {
                LoadEdit();
            }
        }

    }
}
