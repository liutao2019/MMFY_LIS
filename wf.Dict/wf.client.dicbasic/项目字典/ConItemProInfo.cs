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
using lis.client.control;
using dcl.entity;
using System.Linq;
using dcl.client.control;
using System.Text.RegularExpressions;
using dcl.common;

namespace dcl.client.dicbasic
{
    public partial class ConItemProInfo : FrmCommon
    {
        public ConItemProInfo()
        {
            InitializeComponent();
            this.Shown += ConItemProInfo_Shown;
        }

        private void ConItemProInfo_Shown(object sender, EventArgs e)
        {
            txtItmRepEcd.EditValue = ItmRepCode;
        }

        bool isTrue = true;//标明金额是否显示到项目一级中
        bool isChanged = false;//是否执行改变时间标志


        ProxyCommonDic proxy = new ProxyCommonDic("svc.ConItemPro");
        private List<EntityDicItemSample> listsamp = new List<EntityDicItemSample>();
        private List<EntityDicItmRefdetail> listreft = new List<EntityDicItmRefdetail>();
        public EntityDicItmItem itm = new EntityDicItmItem();



        /// <summary>
        /// 当前选中的项目ID，用于刷新后重新选中该项目
        /// </summary>
        string currentItemID = null;

        /// <summary>
        /// 是否绑定用户可用专业组
        /// </summary>
        private bool IsBindingUserTypeSp { get; set; }
        public bool IsSam { get; internal set; }
        public bool IsRef { get; internal set; }

        string ItmRepCode { get; set; }


        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        { }




        ColumnFilterInfo cfiEcd = new ColumnFilterInfo();
        ColumnFilterInfo cfiName = new ColumnFilterInfo();
        ColumnFilterInfo cfiId = new ColumnFilterInfo();
        ColumnFilterInfo cfiRepEcd = new ColumnFilterInfo();

        bool isFiststLoad = true;
        /// <summary>
        /// 用户控件载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConItemPro_Load(object sender, EventArgs e)
        {
            if (!isFiststLoad) return;
            isFiststLoad = false;
            //[项目字典]是否关联用户可用专业组
            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("IsBindingUserTypeSp") == "是")
            {
                IsBindingUserTypeSp = true;
            }
            else
            {
                IsBindingUserTypeSp = false;//默认不绑定用户可用专业组
            }
            sysItem.SetToolButtonStyle(new string[] { sysItem.BtnSave.Name
                , sysItem.BtnAdd.Name,sysItem.BtnDelete.Name
            , sysItem.BtnCopy.Name,sysItem.BtnDeRef.Name,sysItem.BtnClose.Name});
            sysItem.BtnAdd.Caption = "新增标本";
            sysItem.BtnDelete.Caption = "删除标本";
            sysItem.BtnCopy.Caption = "新增参考值";
            sysItem.BtnCopy.LargeGlyph = sysItem.BtnAdd.LargeGlyph;
            sysItem.BtnDeRef.LargeGlyph = sysItem.BtnDelete.LargeGlyph;
            sysItem.BtnDeRef.Caption = "删除参考值";
            sysItem.OnBtnSaveClicked += SysItem_OnBtnSaveClicked;
            sysItem.OnBtnAddClicked += SysItem_OnBtnAddClicked;
            sysItem.OnBtnDeleteClicked += SysItem_OnBtnDeleteClicked;
            sysItem.BtnCopyClick += SysItem_BtnCopyClick;
            sysItem.BtnDeRefClick += SysItem_BtnDeRefClick;
            this.cboItmFlag.Properties.DataSource = CommonValue.GetItmFlag();
            this.cboItmSex.Properties.DataSource = getSex();
            this.cboItmDel.Properties.DataSource = CommonValue.GetDelFlag();
            this.cboItmDType.Properties.DataSource = CommonValue.GetSampleResultType();
            lookSex.DataSource = getSex();
            this.cboItmSexLimit.Properties.DataSource = getSex();
            if (UserInfo.GetSysConfigValue("ItemMoney") == "否")
                isTrue = false;

            colitm_meams.Visible = !isTrue;
            plItemInf2.Visible = isTrue;
            pnlTypeInf2.Visible = !isTrue;
            if (pnlTypeInf2.Visible == true)
            {
                pnlTypeInf2.Height = 28;
            }
            else
            {
                pnlTypeInf2.Height = 0;

            }
            colitm_pri.Visible = !isTrue;
            colitm_cost.Visible = !isTrue;

            plItemInf3.Visible = true;
            txtItmEcd.Focus();

            sysItem.BtnCopy.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            sysItem.BtnDeRef.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            sysItem.BtnAdd.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            sysItem.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            if (IsSam)
            {
                xtraTabControl1.SelectedTabPageIndex = 1;
                splitContainerControl1.CollapsePanel = DevExpress.XtraEditors.SplitCollapsePanel.Panel2;
                this.splitContainerControl1.Collapsed = true;

                sysItem.BtnAdd.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                sysItem.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            if (IsRef)
            {
                xtraTabControl1.SelectedTabPageIndex = 1;
                splitContainerControl1.CollapsePanel = DevExpress.XtraEditors.SplitCollapsePanel.Panel1;
                this.splitContainerControl1.Collapsed = true;
                sysItem.BtnCopy.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                sysItem.BtnDeRef.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            //DoRefresh();

            //开启报告解读
           if( ConfigHelper.GetSysConfigValueWithoutLogin("Interpretation_Report") == "是")
            {
                paMiAdd.Visible = true;
                panel3.Visible = true;
                panel2.Visible = true;
                panelControl1.Visible = true;
            }
        }



        private void SysItem_OnBtnSaveClicked(object sender, EventArgs e)
        {
            bool b = true;
            if (!string.IsNullOrEmpty(txtDL1.Text) && txtDL1.Text.Trim() != txtDL1.Properties.NullText)
            {
                b = CheckDecisiveLeve(txtDL1.Text);
                if (!b)
                {
                    this.txtDL1.Focus();
                    return;
                }
                    
            }

            if (!string.IsNullOrEmpty(txtDL2.Text) && txtDL2.Text.Trim() != txtDL2.Properties.NullText)
            {
               b= CheckDecisiveLeve(txtDL2.Text);
                if (!b)
                {
                    this.txtDL2.Focus();
                    return;
                }
            }
               
            if (!string.IsNullOrEmpty(txtDL3.Text) && txtDL3.Text.Trim() != txtDL3.Properties.NullText)
            {
               b= CheckDecisiveLeve(txtDL3.Text);
                if (!b)
                {
                    this.txtDL3.Focus();
                    return;
                }
            }
               
            if (!string.IsNullOrEmpty(txtDL4.Text) && txtDL4.Text.Trim() != txtDL4.Properties.NullText)
            {
               b= CheckDecisiveLeve(txtDL4.Text);
                if (!b)
                {
                    this.txtDL4.Focus();
                    return;
                }
            }
               
            this.isActionSuccess = false;
            if (this.txtItmName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("项目名称不能为空", "提示");
                this.txtItmName.Focus();
                return;
            }

            if (this.txtItmEcd.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("项目代码不能为空", "提示");
                this.txtItmEcd.Focus();
                return;
            }

            if (this.txtItmRepEcd.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("显示代码不能为空", "提示");
                this.txtItmRepEcd.Focus();
                return;
            }

            if (this.cboPType.valueMember == "")
            {
                lis.client.control.MessageDialog.Show("专业组不能为空", "提示");
                this.cboPType.Focus();
                return;
            }



            //结束编辑状态
            this.bsItem.EndEdit();
            this.bsItemSam.EndEdit();
            this.bsItemMi.EndEdit();
            inLoadItemSam = true;
            inLoadItemSam = false;
            if (bsItem.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            //检查标本类别不能为空
            for (int i = 0; i < bsItemSam.Count; i++)
            {
                EntityDicItemSample dv = (EntityDicItemSample)bsItemSam[i];
                if (dv.ItmSamId == null || dv.ItmSamId == "")
                {
                    lis.client.control.MessageDialog.Show("标本类别不能为空", "提示");
                    bsItemSam.Position = i;

                    cboSamId.Focus();
                    return;
                }
            }

            //检查是否选择多个默认标本
            //DataTable btItemSam = (DataTable)bsItemSam.DataSource;
            //List<EntityDicItemSample> btItemSam = bsItemSam.DataSource as List<EntityDicItemSample>;
            //int itemSamCount = btItemSam.Where(w => w.ItmId.Contains("-1")).ToList().Count;
            //if (itemSamCount > 1)
            //{
            //    lis.client.control.MessageDialog.Show("默认标本类别只能存在一个！", "提示");
            //    return;
            //}
            EntityDicItemSample drsam = new EntityDicItemSample();
            //同步项目的是否停用删除状态
            for (int i2 = 0; i2 < bsItemSam.Count; i2++)
            {
                drsam = (EntityDicItemSample)bsItemSam[i2];
                if (this.cboItmDel.EditValue.ToString() == "1")
                {
                    drsam.ItmDelFlag = "1";
                }
                else
                {
                    drsam.ItmDelFlag = "0";
                }
            }
            EntityDicItmRefdetail drmi = new EntityDicItmRefdetail();
            List<EntityDicItmRefdetail> etDetail = listreft;
            List<EntityDicItmRefdetail> Detail = bsItemMi.Current as List<EntityDicItmRefdetail>;
            //检查参考值名称和类型不能为空,因为bsItemMi的Filter关系使用dtItemMi，Filter的使用可以做到一键保存,缺点是精确定位到下面提示对应行记录会存在困难,需要较多代码现
            for (int i = 0; i < listreft.Count; i++)
            {
                drmi = listreft[i];

                //同步项目的是否停用删除状态
                if (this.cboItmDel.EditValue.ToString() == "1")
                {
                    drmi.ItmDelFlag = "1";

                }
                else
                {
                    drmi.ItmDelFlag = "0";
                }

                if (drmi.ItmRefName.ToString() == null || drmi.ItmRefName.ToString() == "")
                {
                    lis.client.control.MessageDialog.Show("参考值名称不能为空", "提示");

                    cboItmFlag.Focus();
                    return;
                }

                //参考值类型为分期不需要输入年龄
                if ((drmi.ItmRefFlag.ToString() != null && drmi.ToString() == "0") && (drmi.ItmAgeLowerLimit.ToString() == null || drmi.ItmAgeLowerLimit.ToString().Trim() == ""))
                {
                    lis.client.control.MessageDialog.Show("年龄下限不能为空", "提示");

                    txtItmAgeL.Focus();
                    return;
                }

                //参考值类型为分期不需要输入年龄
                if ((drmi.ItmRefFlag.ToString() != null && drmi.ItmRefFlag.ToString() == "0") && (drmi.ItmAgeUpperLimit.ToString() == null || drmi.ItmAgeUpperLimit.ToString().Trim() == ""))
                {
                    lis.client.control.MessageDialog.Show("年龄上限不能为空", "提示");

                    txtItmAgeH.Focus();
                    return;
                }

                int age_l = 0;
                int age_h = 0;
                //判断年龄下限类型
                if (drmi.ItmAgeLowerLimit.ToString() != null && drmi.ItmAgeLowerLimit.ToString().Trim() != "")
                {
                    try
                    {
                        age_l = int.Parse(drmi.ItmAgeLowerLimit.ToString());
                    }
                    catch (Exception)
                    {
                        lis.client.control.MessageDialog.Show("年龄下限值输入错误！", "提示");

                        txtItmAgeL.Focus();
                        return;
                    }
                }
                //判断上限类型
                if (drmi.ItmAgeUpperLimit.ToString() != null && drmi.ItmAgeUpperLimit.ToString().Trim() != "")
                {
                    try
                    {
                        age_h = int.Parse(drmi.ItmAgeUpperLimit.ToString());
                    }
                    catch (Exception)
                    {
                        lis.client.control.MessageDialog.Show("年龄上限值输入错误！", "提示");

                        txtItmAgeH.Focus();
                        return;
                    }
                }

                //转换年龄数据,分期不需要输入年龄
                if (drmi.ItmRefFlag.ToString() != null && drmi.ItmRefFlag.ToString() == "0")
                {
                    string age_l_unit = drmi.ItmAgeLowerLimitUnit.ToString();
                    string age_h_unit = drmi.ItmAgeUpperLimitUnit.ToString();
                    drmi.ItmAgeLowerMinute = this.GetDateValue(age_l, age_l_unit);
                    drmi.ItmAgeUpperMinute = this.GetDateValue(age_h, age_h_unit);


                    if (int.Parse(drmi.ItmAgeUpperMinute.ToString()) < int.Parse(drmi.ItmAgeLowerMinute.ToString()))
                    {
                        lis.client.control.MessageDialog.Show("年龄上限不能小于年龄下限", "提示");

                        txtItmAgeH.Focus();
                        return;
                    }
                }

                //年龄为'',分期时，上下限默认为null
                if (drmi.ItmRefFlag.ToString() != null && drmi.ItmRefFlag.ToString() == "1")
                {
                    if (drmi.ItmAgeLowerLimit.ToString().Trim() == "")
                        drmi.ItmAgeLowerLimit = 0;
                    if (drmi.ItmAgeUpperLimit.ToString().Trim() == "")
                        drmi.ItmAgeUpperLimit = 100;
                }
            }
            //检查同一专业组下项目代码不能重复
            EntityDicItmItem drItem = (EntityDicItmItem)bsItem.Current;
            string itmId = drItem.ItmId;
            string itmEcd = drItem.ItmEcode.Trim().Replace("'", "''");
            string itmPtype = drItem.ItmPriId.ToString().Replace("'", "''");

            //List<EntityDicItmItem> dvItem = bsItem.DataSource as List<EntityDicItmItem>;
            ////dvItem.RowFilter = "itm_ecd = '" + itmEcd + "' and  itm_ptype='" + itmPtype + "'";
            //dvItem = dvItem.Where(w => w.ItmEcode.Contains(itmEcd) ||
            //                                              w.ItmPriId.Contains(itmPtype)).ToList();
            //if (dvItem.Count > 1)
            //{
            //    if (lis.client.control.MessageDialog.Show("该专业组下已经存在此项目代码,是否继续保存？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            //        return;
            //}

            //检查同一项目下标本类别不能重复
            List<EntityDicItemSample> etItemSam = bsItemSam.DataSource as List<EntityDicItemSample>;
            //dvItemSam.Sort = "itm_sam_id";
            string checkSamId = "";
            string checkItrId = "";
            for (int i = 0; i < etItemSam.Count; i++)
            {
                if (etItemSam[i].ItmItrId == null || etItemSam[i].ItmItrId.ToString().Trim() == string.Empty)
                    etItemSam[i].ItmItrId = "-1";
                string thisSamId = etItemSam[i].ItmSamId.ToString();
                string thisItrId = etItemSam[i].ItmItrId.ToString();
                if (thisSamId == checkSamId && thisItrId == checkItrId)
                {
                    lis.client.control.MessageDialog.Show("该项目下存在标本类别重复的数据:" + etItemSam[i].ItmSamName + "", "提示");
                    return;
                }
                checkSamId = thisSamId;
                checkItrId = thisItrId;
            }
            #region
            ////检查同一项目和标本下,不能存在性别相同且年龄上下限重叠的数据
            List<EntityDicItmRefdetail> list = bsItemMi.DataSource as List<EntityDicItmRefdetail>;
            for (int i = 0; i < list.Count; i++)
            {
                string ckitm_id = list[i].ItmId;
                string ckitm_sam_id = list[i].ItmSamId;
                string ckitm_itr_id = list[i].ItmItrId;
                string ckitm_sex = list[i].ItmSex;
                string ckitm_sam_rem = list[i].ItmSamRemark;

                //不检查分期参考值

                list = list.Where(w => w.ItmRefFlag == 0 && w.ItmId == ckitm_id && w.ItmSamId == ckitm_sam_id && w.ItmSex == ckitm_sex &&
                                       w.ItmSamRemark == ckitm_sam_rem && w.ItmItrId == ckitm_itr_id).OrderBy(w => w.ItmAgeLowerMinute).ThenBy(w => w.ItmAgeUpperMinute).ToList();
                int ckItm_age_ls = -1;
                int ckItm_age_hs = -1;

                for (int j = 0; j < list.Count; j++)
                {
                    try
                    {
                        int thisItm_age_ls = int.Parse(list[j].ItmAgeLowerMinute.ToString());//年龄下限50
                        int thisItm_age_hs = int.Parse(list[j].ItmAgeUpperMinute.ToString());//年龄上限100
                        if (ckItm_age_ls != -1 && ckItm_age_hs != -1)
                        {
                            if ((thisItm_age_ls < ckItm_age_hs) || (thisItm_age_ls == ckItm_age_hs && thisItm_age_hs == ckItm_age_hs))
                            {
                                lis.client.control.MessageDialog.Show("有标本存在性别相同,标本备注相同且年龄上下限重叠的数据", "提示");
                                return;
                            }
                        }

                        ckItm_age_ls = thisItm_age_ls;
                        ckItm_age_hs = thisItm_age_hs;
                    }
                    catch
                    {
                        //前面应已经确保传过来类型为"默认"数据一定有年龄,为确保不出错,如发现异常允许保存
                        break;
                    }
                }
            }
            #endregion
            drItem.ItmEcode = itmEcd;

            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("Item", drItem);
            d.Add("Sam", etItemSam);
            d.Add("Detail", etDetail);
            d.Add("IP", IPUtility.GetIP());
            this.isActionSuccess = true;
            try
            {
                if (string.IsNullOrEmpty(itmId))
                {
                    EntitySysOperationLog etOperation = CreateOperateInfo("新增");
                    d.Add("Operation", etOperation);
                    request.SetRequestValue(d);
                    EntityResponse ds = proxy.New(request);
                    if (this.isActionSuccess)
                    {
                    }
                }
                else
                {
                    EntitySysOperationLog etOperation = CreateOperateInfo("修改");
                    d.Add("Operation", etOperation);
                    request.SetRequestValue(d);
                    EntityResponse ds = proxy.Update(request);
                }

                if (this.isActionSuccess)
                {
                    this.DialogResult = DialogResult.OK;
                    itm = bsItem.Current as EntityDicItmItem;
                    lis.client.control.MessageDialog.Show("操作成功", "提示");
                }
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("有标本存在性别相同且年龄上下限重叠的数据", "提示");
                return;
            }
        }

        /// <summary>
        /// 判断水平一、二、三、四的输入格式
        /// </summary>
        private bool CheckDecisiveLeve(string str)
        {
            //只能是数字、小于号、大于号、等于号或者-
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace("-", "");
            str = str.Replace("=", "");
            if (!string.IsNullOrEmpty(str))
            {
                Regex regex = new Regex("^[0-9]*$");
                if (regex.IsMatch(str))
                    return true;
                else
                { 
                    MessageBox.Show("请按照提示输入正确的水平值");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 不知什么原因和系统缓存里的性别偶尔会发生冲突(缓存里sex存在但无数据),先用单独的
        /// </summary>
        /// <returns></returns>
        private DataTable getSex()
        {
            DataTable result = new DataTable("sex");
            result.Columns.Add("id");
            result.Columns.Add("value");
            result.Rows.Add(new Object[] { "0", "" });
            result.Rows.Add(new Object[] { "1", "男" });
            result.Rows.Add(new Object[] { "2", "女" });

            return result;
        }

        /// <summary>
        /// 设置列表控件自带按钮Enable属性
        /// </summary>
        private void SetEmbeddedNavigator(bool enable)
        {
            gvItemSam.OptionsBehavior.Editable = enable;
            gdItemSam.EmbeddedNavigator.Buttons.Append.Enabled = enable;
            gdItemSam.EmbeddedNavigator.Buttons.Remove.Enabled = enable;
            gvItemMi.OptionsBehavior.Editable = enable;
            gdItemMi.EmbeddedNavigator.Buttons.Append.Enabled = enable;
            gdItemMi.EmbeddedNavigator.Buttons.Remove.Enabled = enable;
            txtPY.Properties.ReadOnly = !enable;
            txtWB.Properties.ReadOnly = !enable;
        }

        DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo[] filter = new DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo[5];

        /// <summary>
        /// 读取项目列表
        /// </summary>
        public void LoadItem(EntityDicItmItem current)
        {
            ItmRepCode = current.ItmRepCode;
            bsItem.DataSource = current;
            itm = current;
            LoadItemSam();
            LoadItemMi();


            SetEmbeddedNavigator(true);


            if (bsItemSam.Count > 0)
            {
                panel1.Enabled = true;
            }
            else
            {
                panel1.Enabled = false;
            }

            if (bsItemMi.Count > 0)
            {
                pnlMi.Enabled = true;
                paMiAdd.Enabled = true;
            }
            else
            {
                pnlMi.Enabled = false;
                paMiAdd.Enabled = false;
            }

        }



        dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        /// <summary>
        /// 生成拼音码和五笔码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtItmName_TextChanged(object sender, EventArgs e)
        {
            itm.ItmPyCode = tookit.GetSpellCode(this.txtItmName.Text);
            itm.ItmWbCode = tookit.GetSpellCode(this.txtItmName.Text);
        }


        /// <summary>
        /// 时间转换函数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public int GetDateValue(int value, string unit)
        {

            int yearUnit = 12;
            int monthUnit = 30;
            int hourUnit = 24;
            int minuteUnit = 60;

            int num;

            switch (unit)
            {
                case "岁":
                    {
                        num = value * yearUnit * monthUnit * hourUnit * minuteUnit;
                        break;
                    }
                case "月":
                    {
                        num = value * monthUnit * hourUnit * minuteUnit;
                        break;
                    }
                case "天":
                    {
                        num = value * hourUnit * minuteUnit;
                        break;
                    }
                default:
                    {
                        //时
                        num = value * minuteUnit;
                        break;
                    }
            }

            return num;
        }


        /// <summary>
        /// 显示参考值信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvItemSam_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //inLoadItemSam = true;

            //if (bsItem.Position > -1 && bsItemSam.Position > -1)
            //{
            //    LoadItemMi();
            //    panel1.Enabled = true;
            //}
            //else
            //{
            //    panel1.Enabled = false;
            //}

            //inLoadItemSam = false;
        }

        bool inLoadItemSam = false;
        /// <summary>
        /// 读取标本信息
        /// </summary>
        /// <param name="itm_id"></param>
        private void LoadItemSam()
        {
            inLoadItemSam = true;

            if (bsItem.Position > -1)
            {
                EntityRequest request = new EntityRequest();
                EntityDicItmItem dr = (EntityDicItmItem)bsItem.Current;
                if (dr != null)
                {
                    request.SetRequestValue(dr);
                    EntityResponse ds = proxy.Other(request);
                    EntityResponse dsdetail = proxy.View(request);
                    if (isActionSuccess)
                    {
                        listsamp = ds.GetResult() as List<EntityDicItemSample>;
                        bsItemSam.DataSource = listsamp;
                        listreft = dsdetail.GetResult() as List<EntityDicItmRefdetail>;
                        foreach (EntityDicItmRefdetail refdetail in listreft)
                        {
                            List<EntityDicItemSample> list = listsamp.Where(w => w.ItmSamId == refdetail.ItmSamId && w.ItmItrId == refdetail.ItmItrId).ToList();
                            if (list.Count > 0)
                            {
                                refdetail.ItmSort = list[0].ItmSort;
                            }
                        }
                        bsItemMi.DataSource = listreft;
                    }
                }
                //result = base.doOther(ds);

                bsItemSam.Filter = "1=1";
            }
            else
            {
                bsItemSam.Filter = "1<>1";
            }

            //避免重复设置Filter造成界面刷新
            if (bsItemSam.Count == 0)
            {
                bsItemMi.Filter = "1<>1";
            }

            inLoadItemSam = false;
        }


        private void LoadItemMi()
        {
            inLoadItemSam = true;
            if (bsItem.Position > -1 && bsItemSam.Position > -1 && bsItemSam.Current != null)
            {
                EntityDicItemSample samp = (EntityDicItemSample)bsItemSam.Current;

                if (samp.ItmSamId != null && samp.ItmSamId != "")
                {
                    string itm_sam_id = samp.ItmSamId;
                    string itm_itr_id = samp.ItmItrId.Trim() == string.Empty ? "-1" : samp.ItmItrId.Trim();
                    int itm_sort = samp.ItmSort;
                    List<EntityDicItmRefdetail> list = listreft.FindAll(w => w.ItmSamId == itm_sam_id
                                                                        && w.ItmItrId == itm_itr_id && w.ItmSort == itm_sort);
                    bsItemMi.DataSource = new List<EntityDicItmRefdetail>();
                    bsItemMi.DataSource = list;
                }
                else
                {
                    bsItemMi.Filter = "1<>1";
                }
            }
            else
            {
                bsItemMi.Filter = "1<>1";
            }
            inLoadItemSam = false;
        }

        /// <summary>
        /// 点击标本列表自带工具条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdItemSam_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            //非编辑状态时不做任何操作
            if (cboSamId.Readonly)
            {
                e.Handled = true;
                return;
            }

            //新增标本
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Append)
            {
                e.Handled = true;

                if (bsItem.Position != -1)
                {
                    EntityDicItmItem drItem = (EntityDicItmItem)bsItem.Current;
                    string itm_meams = "";
                    if (bsItemSam.Current != null)//继承上一个试验方法
                    {
                        EntityDicItemSample drSam = (EntityDicItemSample)bsItemSam.Current;
                        itm_meams = drSam.ItmMethod;
                    }
                    if (true)
                    {
                        int rowCount = ((List<EntityDicItemSample>)bsItemSam.DataSource).Count;
                        EntityDicItemSample drDetail = (EntityDicItemSample)bsItemSam.AddNew();
                        drDetail.ItmId = drItem.ItmId;
                        if (itm_meams != "")
                            drDetail.ItmMethod = itm_meams;
                        drDetail.ItmValid = 0;
                        drDetail.ItmItrId = "-1";
                        drDetail.ItmAcceptFlag = 0;
                        drDetail.ItmMaxDigit = 0;
                        drDetail.ItmSort = ++rowCount;
                        bsItemSam.EndEdit();
                        bsItemSam.ResetCurrentItem();
                        this.cboSamId.Focus();
                        bsItemMi.DataSource = new List<EntityDicItmRefdetail>();
                    }

                    panel1.Enabled = true;
                }
            }

            //删除标本时需要删除对应的参考值
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
            {
                EntityDicItemSample drItemSam = (EntityDicItemSample)bsItemSam.Current;
                string delSam = drItemSam.ItmSamId.ToString();
                string delItrId = string.Empty;
                if (!string.IsNullOrEmpty(drItemSam.ItmItrId))
                {
                    delItrId = drItemSam.ItmItrId.ToString();
                }
                else
                {
                    delItrId = "-1";
                }
                List<EntityDicItmRefdetail> listDetail = ((List<EntityDicItmRefdetail>)bsItemMi.DataSource);
                for (int i = listDetail.Count - 1; i >= 0; i--)
                {
                    EntityDicItmRefdetail dr = listDetail[i];
                    if (dr.ItmSamId.ToString() == delSam
                        && dr.ItmItrId.ToString() == delItrId)
                    {
                        listreft.Remove(dr);
                        ((List<EntityDicItmRefdetail>)bsItemMi.DataSource).RemoveAt(i);
                    }
                }
                gvItemMi.RefreshData();
            }
        }

        /// <summary>
        /// 点击参考值自带工具条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdItemMi_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            //非编辑状态时不做任何操作
            if (txtItmRefStages.Readonly)
            {
                e.Handled = true;
                return;
            }

            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Append)
            {
                e.Handled = true;

                if (bsItemSam.Position != -1)
                {
                    EntityDicItemSample drItemSam = (EntityDicItemSample)bsItemSam.Current;
                    if (drItemSam.ItmSamId != null && drItemSam.ItmSamId.ToString() != "")
                    {
                        txtItmRefStages.valueMember = "";
                        txtSam_rem.valueMember = "";
                        txtItmRefStages.selectRow = null;
                        EntityDicItmRefdetail drDetail = new EntityDicItmRefdetail();
                        //EntityDicItmRefdetail drDetail=(EntityDicItmRefdetail)bsItemMi.AddNew();
                        drDetail.ItmId = drItemSam.ItmId;
                        drDetail.ItmSort = drItemSam.ItmSort;
                        int count = bsItemMi.Count;
                        drDetail.ItmSamId = drItemSam.ItmSamId;
                        drDetail.ItmItrId = string.IsNullOrEmpty(drItemSam.ItmItrId) ? "-1" : drItemSam.ItmItrId;
                        drDetail.ItmRefFlag = 0;
                        drDetail.ItmDelFlag = "0";
                        drDetail.ItmSex = "0";
                        drDetail.ItmAgeLowerLimitUnit = "岁";
                        drDetail.ItmAgeUpperLimitUnit = "岁";
                        drDetail.ItmSamRemark = "";
                        bsItemMi.Insert(count, drDetail);
                        listreft.Add(drDetail);
                        bsItemMi.EndEdit();
                        bsItemMi.ResetCurrentItem();
                        this.txtItmRefStages.Focus();
                        pnlMi.Enabled = true;
                        paMiAdd.Enabled = true;
                    }
                    else
                    {
                        lis.client.control.MessageDialog.Show("参考值所对应的标本类别不能为空", "信息");

                        cboSamId.Focus();
                    }
                }
            }
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
            {
                EntityDicItmRefdetail detail = (EntityDicItmRefdetail)bsItemMi.Current;
                listreft.Remove(detail);
            }

        }

        private void SysItem_BtnDeRefClick(object sender, EventArgs e)
        {
            if (txtItmRefStages.Readonly)
            {
                return;
            }

            EntityDicItmRefdetail drDetail = ((EntityDicItmRefdetail)bsItemMi.Current);

            ((List<EntityDicItmRefdetail>)bsItemMi.DataSource).Remove(drDetail);
        }

        private void SysItem_BtnCopyClick(object sender, EventArgs e)
        {
            if (txtItmRefStages.Readonly)
            {
                return;
            }

            if (bsItemSam.Position != -1)
            {
                EntityDicItemSample drItemSam = (EntityDicItemSample)bsItemSam.Current;
                if (drItemSam.ItmSamId != null && drItemSam.ItmSamId.ToString() != "")
                {
                    txtItmRefStages.valueMember = "";
                    txtItmRefStages.selectRow = null;
                    EntityDicItmRefdetail drDetail = (EntityDicItmRefdetail)bsItemMi.AddNew();
                    drDetail.ItmId = drItemSam.ItmId;
                    drDetail.ItmSamId = drItemSam.ItmSamId;
                    drDetail.ItmItrId = drItemSam.ItmItrId;
                    drDetail.ItmSort = drItemSam.ItmSort;
                    drDetail.ItmDelFlag = "0";
                    drDetail.ItmSex = "0";
                    drDetail.ItmAgeLowerLimitUnit = "岁";
                    drDetail.ItmAgeUpperLimitUnit = "岁";
                    drDetail.ItmSamRemark = "";
                    bsItemMi.EndEdit();
                    bsItemMi.ResetCurrentItem();
                    this.txtItmRefStages.Focus();

                    pnlMi.Enabled = true;
                    paMiAdd.Enabled = true;
                }
                else
                {
                    lis.client.control.MessageDialog.Show("参考值所对应的标本类别不能为空", "信息");

                    cboSamId.Focus();
                }
            }
        }

        private void SysItem_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            if (txtItmRefStages.Readonly)
            {

                return;
            }

            EntityDicItemSample drItemSam = (EntityDicItemSample)bsItemSam.Current;
            string delSam = drItemSam.ItmSamId.ToString();
            string delItrId = drItemSam.ItmItrId.ToString();
            string delSort = drItemSam.ItmSort.ToString();

            for (int i = bsItemMi.Count; i >= 0; i--)
            {
                EntityDicItmRefdetail dr = (EntityDicItmRefdetail)bsItemMi[i];

                if (dr.ItmSamId.ToString() == delSam
                    && dr.ItmItrId.ToString() == delItrId && dr.ItmSort.ToString() == delSort)
                {
                    bsItemMi.RemoveAt(i);
                }
            }
            bsItemSam.Remove(drItemSam);
        }

        private void SysItem_OnBtnAddClicked(object sender, EventArgs e)
        {
            if (txtItmRefStages.Readonly)
            {
                return;
            }

            if (bsItem.Position != -1)
            {
                EntityDicItmItem drItem = (EntityDicItmItem)bsItem.Current;

                string itm_meams = "";
                if (bsItemSam.Current != null)//继承上一个试验方法
                {
                    EntityDicItemSample drSam = (EntityDicItemSample)bsItemSam.Current;
                    itm_meams = drSam.ItmMethod.ToString();
                }
                if (true)
                {
                    EntityDicItemSample drDetail = (EntityDicItemSample)bsItemSam.AddNew();
                    drDetail.ItmId = drItem.ItmId;
                    if (itm_meams != "")
                        drDetail.ItmMethod = itm_meams;
                    drDetail.ItmValid = 0;
                    drDetail.ItmAcceptFlag = 0;
                    drDetail.ItmMaxDigit = 0;
                    bsItemSam.EndEdit();
                    drDetail.ItmItrId = "-1";
                    bsItemSam.ResetCurrentItem();
                    this.cboSamId.Focus();

                }

                panel1.Enabled = true;
            }
        }

        /// <summary>
        /// 选择样本时同时更新参考值的样本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void cboSamId_onAfterSelected(object sender, EventArgs e)
        {
            if (inLoadItemSam == false && cboSamId.valueMember != null)
            {
                if (cboSamId.valueMember != "")
                {
                    string samId = cboSamId.valueMember;

                    //未改变bsItemMi的Filter情况下先修改样本类别
                    for (int i = bsItemMi.Count - 1; i >= 0; i--)
                    {
                        ((EntityDicItmRefdetail)bsItemMi[i]).ItmSamId = samId;
                    }
                }
                else
                {
                    for (int i = bsItemMi.Count - 1; i >= 0; i--)
                    {
                        bsItemMi.RemoveAt(i);
                    }
                }

                bsItemSam.EndEdit();
                bsItemMi.EndEdit();
                LoadItemMi();
            }
        }

        /// <summary>
        /// 控制录入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsItemSam_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (bsItemSam.Count > 0)
            {
                panel1.Enabled = true;
            }
            else
            {
                panel1.Enabled = false;
            }
        }

        /// <summary>
        /// 控制录入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsItemMi_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (bsItemMi.Count > 0)
            {
                pnlMi.Enabled = true;
                paMiAdd.Enabled = true;
            }
            else
            {
                pnlMi.Enabled = false;
                paMiAdd.Enabled = false;
            }
        }

        private void bsItem_CurrentChanged(object sender, EventArgs e)
        {
            this.isActionSuccess = true;
            if (inLoadItemSam == false)
            {
                inLoadItemSam = true;

                if (bsItem.Position > -1)
                {
                    LoadItemSam();
                    LoadItemMi();
                }

                inLoadItemSam = false;
            }

            if (logItemChanged)
            {
                if (this.bsItem.Current != null)
                {
                    currentItemID = (this.bsItem.Current as EntityDicItmItem).ItmId;
                }
                else
                {
                    currentItemID = null;
                }
            }
        }

        bool logItemChanged = true;


        private void bsItemSam_CurrentChanged(object sender, EventArgs e)
        {
            if (inLoadItemSam == false)
            {
                inLoadItemSam = true;

                if (bsItem.Position > -1 && bsItemSam.Position > -1)
                {
                    LoadItemMi();
                    panel1.Enabled = true;
                }
                else
                {
                    panel1.Enabled = false;
                }

                inLoadItemSam = false;
            }
        }
        
        /// <summary>
        /// 输入项目代码的时候，显示代码自动跟着改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtItmEcd_EditValueChanged(object sender, EventArgs e)
        {
            itm.ItmRepCode = getShowItemName();
            txtItmRepEcd.EditValue = itm.ItmRepCode;
        }



        private string getShowItemName()
        {
            string showName = "";

            //if (!isTrue)
            //{
            if (txtItmName.EditValue != null && txtItmName.EditValue.ToString().Trim() != "")
            {
                showName += txtItmName.EditValue.ToString();
            }
            if (txtItmEcd.EditValue != null && txtItmEcd.EditValue.ToString().Trim() != "")
            {
                string itmRep = "(" + txtItmEcd.EditValue.ToString().Trim() + ")";
                showName += itmRep;
            }
            // }
            //else
            //{
            //    if (txtItmEcd.EditValue != null && txtItmEcd.EditValue.ToString().Trim() != "")
            //        showName = txtItmEcd.EditValue.ToString().Trim();
            //}

            return showName;
        }






        private void txtSamRem_TextChanged(object sender, EventArgs e)
        {
            //if (txtSamRem.EditValue != null)
            //    txtSam_rem.displayMember = txtSamRem.EditValue.ToString();
        }

        private void txtRefStages_TextChanged(object sender, EventArgs e)
        {
            if (txtRefStages.EditValue != null)
            {
                this.txtItmRefStages.displayMember = txtRefStages.EditValue.ToString();
                txtItmRefStages.ValueChanged -= new DclPopSelect<dcl.entity.EntityDicItmReftype>.ValueChangedEventHandler(txtItmRefStages_ValueChanged);
                this.txtItmRefStages.valueMember = txtRefStages.EditValue.ToString();
                txtItmRefStages.ValueChanged += new DclPopSelect<dcl.entity.EntityDicItmReftype>.ValueChangedEventHandler(txtItmRefStages_ValueChanged);
            }
        }



        private void txtSam_rem_DisplayTextChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            EntityDicItmRefdetail refd = (EntityDicItmRefdetail)bsItemMi.Current;
            if (!string.IsNullOrEmpty(txtSam_rem.popupContainerEdit1.Text.ToString()))
            {
                refd.ItmSamRemark = txtSam_rem.popupContainerEdit1.Text;
            }
            else
            {
                txtSam_rem.displayMember = "";
            }
            bsItemMi.EndEdit();
        }

        private void itemValue_EditValueChanged(object sender, EventArgs e)
        {
            if (plItemInf2.Visible && isChanged)
            {
                this.bsItemSam.EndEdit();
                for (int i = 0; i < bsItemSam.Count; i++)
                {
                    EntityDicItemSample dv = (EntityDicItemSample)bsItemSam[i];
                    if (txtItemPri.EditValue != null && txtItemPri.EditValue.ToString().Trim() != "")
                        dv.ItmPrice = Convert.ToDecimal(txtItemPri.EditValue);
                    else
                        dv.ItmPrice = 0.00M;
                    if (txtItmCost.EditValue != null && txtItmCost.EditValue.ToString().Trim() != "")
                        dv.ItmCost = Convert.ToDecimal(txtItmCost.EditValue);
                    else
                        dv.ItmCost = 0.00M;
                    dv.ItmMethod = txtItmMeams.EditValue.ToString();
                }
            }
        }








        private void txtPatInstructment_onAfterSelected(object sender, EventArgs e)
        {
            if (inLoadItemSam == false && txtPatInstructment.valueMember != null)
            {
                if (txtPatInstructment.valueMember != "")
                {
                    string itrId = txtPatInstructment.valueMember;

                    //未改变bsItemMi的Filter情况下先修改所属仪器
                    for (int i = bsItemMi.Count - 1; i >= 0; i--)
                    {
                        ((EntityDicItmRefdetail)bsItemMi[i]).ItmItrId = itrId;
                    }
                }
                else
                {
                    for (int i = bsItemMi.Count - 1; i >= 0; i--)
                    {
                        ((EntityDicItmRefdetail)bsItemMi[i]).ItmItrId = "-1";
                    }
                }

                bsItemSam.EndEdit();
                bsItemMi.EndEdit();
                LoadItemMi();
            }
        }

        /// <summary>
        /// 生成操作用户信息记录
        /// </summary>
        /// <returns></returns>
        private EntitySysOperationLog CreateOperateInfo(string type)
        {
            EntitySysOperationLog log = new EntitySysOperationLog();
            log.OperatUserId = UserInfo.loginID;
            log.OperatUserName = UserInfo.userName;
            log.OperatAction = type;
            return log;
        }
        /// <summary>
        /// 参考值名称改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void txtItmRefStages_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (txtItmRefStages.selectRow != null)
            {
                EntityDicItmReftype dr = txtItmRefStages.selectRow;
                txtItmAgeH.EditValue = dr.RefAgeHigh;
                txtItmAgeL.EditValue = dr.RefAgeLower;
                if (dr.RefAgeLowerUnit != null && dr.RefAgeLowerUnit.ToString().Trim() != "")
                    cboAgeLUnit.EditValue = dr.RefAgeLowerUnit;
                if (dr.RefAgeHighUnit != null && dr.RefAgeHighUnit.ToString().Trim() != "")
                    cboAgeHUnit.EditValue = dr.RefAgeHighUnit;
                // dr.RefName = this.txtItmRefStages.popupContainerEdit1.Text;
                EntityDicItmRefdetail detail = bsItemMi.Current as EntityDicItmRefdetail;
                if (detail != null)
                {
                    detail.ItmRefName = this.txtItmRefStages.popupContainerEdit1.Text;
                }
                //  txtRefStages.EditValue = this.txtItmRefStages.popupContainerEdit1.Text;
                bsItemMi.EndEdit();
            }
        }


    }
}
