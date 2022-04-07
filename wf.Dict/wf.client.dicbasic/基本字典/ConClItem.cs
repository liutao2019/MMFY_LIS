using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using System.Collections;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using DevExpress.XtraGrid.Columns;
using dcl.entity;
using lis.client.control;
using System.Linq;
using dcl.common;

namespace dcl.client.dicbasic
{
    public partial class ConClItem : ConDicCommon, IBarActionExt
    {
        #region IBarActionExt 成员

        public void Add()
        {
            blIsNew = true;//标记为新增事件
            #region 数据表的过滤信息保存到新建临时过滤信息里
            cfiId = colcal_id.FilterInfo;
            cfiEcd = colcal_itm_ecd.FilterInfo;
            cfiFmla = colcal_fmla.FilterInfo;
            cfiFlag = colcal_flag.FilterInfo;

            #endregion

            #region 重新清空数据表的过滤信息
            //重新清空数据表的过滤信息
            colcal_id.FilterInfo = new ColumnFilterInfo();
            colcal_itm_ecd.FilterInfo = new ColumnFilterInfo();
            colcal_fmla.FilterInfo = new ColumnFilterInfo();
            colcal_flag.FilterInfo = new ColumnFilterInfo();
            gridView2.SortInfo.View.ActiveFilter.Clear();
            #endregion
            EntityDicItmCalu list = (EntityDicItmCalu)bsBscript.Current;
            int strFlag = 1;
            if (bsBscript.Current != null)
            {
                strFlag = list.CalFlag;
            }
            EntityDicItmCalu dr = ((EntityDicItmCalu)bsBscript.AddNew());
            gridControl1.Enabled = false;
            dr.CalId = "";
            fDict_item.displayMember = "";
            fDict_item.valueMember = "";
            selectDict_Instrmt1.valueMember = "";
            fDict_item.Focus();
            dr.CalFlag = strFlag;
            bsBscript.EndEdit();
            bsBscript.ResetCurrentItem();
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.bsBscript.EndEdit();
            if (bsBscript.Current == null)
            {
                return;
            }
            string meth = this.memoEdit1.Text;

            if (this.lookUpEdit1.Text == "")
            {
                lis.client.control.MessageDialog.Show("请选择公式类别！");
                return;
            }
            string val = "";
            if (this.lookUpEdit1.Text == "计算" || this.lookUpEdit1.Text == "酶标")
            {

                if (fDict_item.valueMember != null && fDict_item.valueMember != "")
                    val = this.isMetch(meth, 1, true);
                else
                {
                    lis.client.control.MessageDialog.Show("请选择项目", "提示");
                    return;
                }
            }
            else
                val = this.isMetch(meth, 2, true);
            EntityDicItmCalu dr = ((EntityDicItmCalu)bsBscript.Current);
            String cal_id = dr.CalId.ToString();
            dr.CalVariable = val;

            dr.CalItrId = selectDict_Instrmt1.valueMember;
            if (chkSupportItrCal.Checked)
            {
                dr.CalSupportItrCal = "1";
            }
            else
            {
                dr.CalSupportItrCal = "0";
            }
            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dr);
            EntityResponse result = new EntityResponse();

            //EntityDicItmItem Item = ((EntityDicItmItem)bsBscript.Current);
            //EntityRequest ItmRequest = new EntityRequest();
            //ItmRequest.SetRequestValue(Item);
            //EntityResponse ItmResult = new EntityResponse();
            if (cal_id == "")
            {
                result = base.New(request);
            }
            else
            {
                result = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                lis.client.control.MessageDialog.Show("保存成功", "提示信息");
                DoRefresh();
            }
            else
            {
                lis.client.control.MessageDialog.Show("保存失败", "提示信息");
            }

            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                colcal_flag.FilterInfo = cfiFlag;
                colcal_fmla.FilterInfo = cfiFmla;
                colcal_id.FilterInfo = cfiId;
                colcal_itm_ecd.FilterInfo = cfiEcd;
                blIsNew = false;//取消新增事件
                this.gridControl1.Enabled = true;
            }
        }

        public void Delete()
        {
            this.bsBscript.EndEdit();
            if (bsBscript.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicItmCalu dr = (EntityDicItmCalu)bsBscript.Current;

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

        public void DoRefresh()
        {
            this.gridControl1.Enabled = true;
            getTable();
            EntityRequest request = new EntityRequest();
            EntityDicItmItem dr = new EntityDicItmItem();
            request.SetRequestValue(dr);
            EntityResponse ds = base.View(request);
            if (isActionSuccess)
            {
                Itemlist = ds.GetResult() as List<EntityDicItmItem>;
                this.bsDictItem.DataSource = Itemlist.Where(i => i.ItmDelFlag == "0").ToList();
            }

        }

        /// <summary>
        /// 放弃
        /// </summary>
        public void Cancel()
        {
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                colcal_flag.FilterInfo = cfiFlag;
                colcal_fmla.FilterInfo = cfiFmla;
                colcal_id.FilterInfo = cfiId;
                colcal_itm_ecd.FilterInfo = cfiEcd;
                blIsNew = false;
                this.gridControl1.Enabled = true;

            }




        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gridControl1", true);
            dlist.Add("simpleButton1", true);
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            return dlist;
        }
        #endregion

        #region 保存数据表临时过滤信息
        /// <summary>
        /// 过滤编码信息
        /// </summary>
        ColumnFilterInfo cfiId = new ColumnFilterInfo();
        /// <summary>
        /// 过滤项目信息
        /// </summary>
        ColumnFilterInfo cfiEcd = new ColumnFilterInfo();
        /// <summary>
        /// 过滤计算项目信息
        /// </summary>
        ColumnFilterInfo cfiFmla = new ColumnFilterInfo();
        /// <summary>
        /// 过滤类型项目信息
        /// </summary>
        ColumnFilterInfo cfiFlag = new ColumnFilterInfo();
        /// <summary>
        /// 是否为新增事件
        /// </summary>
        bool blIsNew = false;
        #endregion

        public ConClItem()
        {
            InitializeComponent();
        }

        private void on_Load(object sender, EventArgs e)
        {
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);

            this.initData();
            this.bsItemType.DataSource = CommonValue.GetClItemType();
            List<EntityDicItmItem> ItemList = fDict_item.dtSource as List<EntityDicItmItem>;
            ItemList = ItemList.Where(i => i.ItmDelFlag == "0").ToList();
            fDict_item.dtSource = ItemList;
            this.repositoryItemLookUpEdit2.DataSource = fDict_item.dtSource;
            lookUpEdit2.Properties.DataSource = CommonValue.GetSpecClFormula(); 
        }

        private void initData()
        {
            this.DoRefresh();
        }
        private List<EntityDicItmCalu> list = new List<EntityDicItmCalu>();
        private List<EntityDicItmItem> Itemlist = new List<EntityDicItmItem>();
        private void getTable()
        {
            EntityRequest request = new EntityRequest();
            EntityDicItmCalu dr = new EntityDicItmCalu();
            request.SetRequestValue(dr);
            EntityResponse ds = base.Search(request);
            if (isActionSuccess)
            {
                list = ds.GetResult() as List<EntityDicItmCalu>;
                this.bsBscript.DataSource = list;
            }
        }

        #region 响应按钮菜单点击事件
        //private void toSave()
        //{
        //    this.bsBscript.EndEdit();
        //    if (bsBscript.Current == null)
        //    {
        //        return;
        //    }
        //    string meth = this.memoEdit1.Text;

        //    if (this.lookUpEdit1.Text == "")
        //    {
        //        lis.client.control.MessageDialog.Show("请选择公式类别！");
        //        return;
        //    }
        //    string val = "";
        //    if (this.lookUpEdit1.Text == "计算" || this.lookUpEdit1.Text == "酶标")
        //    {

        //        if (fDict_item.valueMember != null && fDict_item.valueMember != "")
        //            val = this.isMetch(meth, 1, true);
        //        else
        //        {
        //            lis.client.control.MessageDialog.Show("请选择项目", "提示");
        //            return;
        //        }
        //    }
        //    else
        //        val = this.isMetch(meth, 2, true);
        //    DataSet ds = new DataSet();
        //    DataRow dr = ((DataRowView)bsBscript.Current).Row;
        //    if (dr["cal_itm_ecd"] == DBNull.Value || dr["cal_itm_ecd"] == null)
        //    {
        //        dr["cal_itm_ecd"] = "";
        //    }
        //    String cal_id = dr["cal_id"].ToString();
        //    dr["cal_variable"] = val;
        //    if (dr.Table.Columns.Contains("cal_itr_id"))
        //    {
        //        dr["cal_itr_id"] = selectDict_Instrmt1.valueMember;
        //    }

        //    #region  2013-12-30  仪器sql计算功能
        //    //为保证在没有更新sql脚本时程序不会出错，需要判断此字段是否存在
        //    if (dr.Table.Columns.Contains("cal_supportItrCal"))
        //    {
        //        dr["cal_supportItrCal"] = chkSupportItrCal.Checked;
        //    }
        //    #endregion

        //    DataTable dtUpdate = this.dtItem.Clone();
        //    dtUpdate.Rows.Add(dtItem.Rows[bsBscript.Position].ItemArray);
        //    ds.Tables.Add(dtUpdate);
        //    DataSet result = new DataSet();
        //    if (cal_id == "")
        //    {
        //        result = base.doNew(ds);
        //    }
        //    else
        //    {
        //        result = base.doUpdate(ds);
        //    }
        //    if (base.isActionSuccess)
        //    {
        //        if (cal_id == "")
        //        {
        //            dtItem.Rows[bsBscript.Position]["cal_id"] =
        //            result.Tables["dict_cl_item"].Rows[0][0];
        //            this.dtItem.AcceptChanges();
        //        }
        //    }
        //    //只有新增事件放弃时才对过滤信息重新过滤
        //    if (blIsNew)
        //    {
        //        colcal_flag.FilterInfo = cfiFlag;
        //        colcal_fmla.FilterInfo = cfiFmla;
        //        colcal_id.FilterInfo = cfiId;
        //        colcal_itm_ecd.FilterInfo = cfiEcd;
        //        blIsNew = false;//取消新增事件
        //        this.gridControl1.Enabled = true;
        //    }

        //}
        //private void toDel()
        //{
        //    this.bsBscript.EndEdit();
        //    if (bsBscript.Current == null)
        //    {
        //        return;
        //    }

        //    DataSet ds = new DataSet();
        //    DataRow dr = ((DataRowView)bsBscript.Current).Row;
        //    String cal_id = dr["cal_id"].ToString();
        //    DataTable dtUpdate = this.dtItem.Clone();
        //    dtUpdate.Rows.Add(dtItem.Rows[bsBscript.Position].ItemArray);
        //    ds.Tables.Add(dtUpdate);
        //    DataSet result = new DataSet();
        //    if (cal_id == "")
        //    {
        //        this.dtItem.Rows.Remove(dr);
        //    }
        //    else
        //    {
        //        DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //        switch (dresult)
        //        {
        //            case DialogResult.OK:
        //                base.doDel(ds);
        //                break;
        //            case DialogResult.Cancel:
        //                return;

        //        }

        //    }
        //    if (base.isActionSuccess)
        //    {
        //        if (cal_id != "")
        //        {
        //            dtItem.Rows.Remove(dr);
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception();
        //    }
        //}
        #endregion

        //验证计算公式类
        private string isMetch(string meth, int num, bool istrue)
        {
            #region 验证公式
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;
            int count5 = 0;
            string strMeth = meth;
            strMeth = strMeth.Replace("[", "");
            count1 = meth.Length - strMeth.Length;

            strMeth = meth;
            strMeth = strMeth.Replace("]", "");
            count2 = meth.Length - strMeth.Length;

            strMeth = meth;
            strMeth = strMeth.Replace("(", "");
            count3 = meth.Length - strMeth.Length;

            strMeth = meth;
            strMeth = strMeth.Replace(")", "");
            count4 = meth.Length - strMeth.Length;

            strMeth = meth;
            strMeth = strMeth.Replace("=", "");
            count5 = meth.Length - strMeth.Length;
            //for (int i = 0; i < meth.Length; i++)
            //{

            //    if (meth[i].ToString() == "[")
            //        count1++;
            //    if (meth[i].ToString() == "]")
            //        count2++;
            //    if (meth[i].ToString() == "(")
            //        count3++;
            //    if (meth[i].ToString() == ")")
            //        count4++;
            //}
            if (count1 != count2 || count1 == 0)
            {
                lis.client.control.MessageDialog.Show("变量请用[]括起来或[]个数不匹配", "提示");
                if (istrue)
                    throw new Exception();
                else
                    return "";
            }
            if (count3 != count4)
            {
                lis.client.control.MessageDialog.Show("()个数不匹配，请检查", "提示");
                if (istrue)
                    throw new Exception();
                else
                    return "";
            }
            //if (count5 > 0 && lookUpEdit1.EditValue.ToString() == "1")
            //{
            //    lis.client.control.MessageDialog.Show("表达式错误，计算项目请勿输入=", "提示");
            //    if (istrue)
            //        throw new Exception();
            //    else
            //        return "";
            //}

            #endregion

            if (meth.Contains("[标本]") || meth.Contains("[标本备注]"))
            {

                meth = meth.Replace("[标本]", "\"\"");
                meth = meth.Replace("[标本备注]", "\"\"");

            }
            #region  储存变量位置
            int length = 0;
            ArrayList list = new ArrayList();
            for (int i = 0; i < count1; i++)
            {
                int index = meth.IndexOf("[", (length));
                length = index + 1;
                if (index >= 0 && length > 0)
                {
                    list.Add(index.ToString());
                }
                else break;
            }

            int length2 = 0;
            ArrayList list2 = new ArrayList();

            for (int i = 0; i < count1; i++)
            {
                int index = meth.IndexOf("]", (length2));
                length2 = index + 1;
                if (index >= 0 && length2 > 0)
                {
                    list2.Add(index.ToString());
                }
                else break;
            }
            #endregion

            #region 存储变量值
            ArrayList parameter = new ArrayList();
            for (int i = 0; i < list.Count; i++)
            {
                int a = Convert.ToInt32(list[i]);
                int b = Convert.ToInt32(list2[i]);
                parameter.Add(meth.Substring(Convert.ToInt32(list[i]), Convert.ToInt32(list2[i]) - Convert.ToInt32(list[i]) + 1));
            }
            #endregion

            #region 检验是否存在变量值
            List<EntityDicItmItem> dtItm = this.bsDictItem.DataSource as List<EntityDicItmItem>;
            List<EntityDicItmCalu> dtCl = (List<EntityDicItmCalu>)this.bsBscript.DataSource;
            string variable = "";
            for (int i = 0; i < parameter.Count; i++)
            {
                string item = parameter[i].ToString().Substring(1, parameter[i].ToString().Length - 2);
                //if (dtItm.Select("itm_ecd ='" + item + "' and itm_cal_flag <> 1").Length == 0)
                    bool isFind = false;
                List<EntityDicItmItem> ItmList = dtItm.Where(w => w.ItmCaluFlag != 1 && w.ItmEcode == item).ToList();
                //foreach (var itm in dtItm)
                //{
                //    if (itm.ItmEcode == item &&
                //        (itm.ItmCaluFlag != 1))
                //    {
                //        isFind = true;
                //        break;
                //    }
                //}
                if(ItmList.Count > 0)
                {
                    isFind = true;
                }
                if (!isFind)
                {
                    lis.client.control.MessageDialog.Show("不存在变量值" + item, "提示");
                    if (istrue)
                        throw new Exception();
                    else
                        return "";

                }
                //if (num == 1)
                //{
                //    if (dtCl.Select("cal_itm_ecd='" + item + "'").Length > 0)
                //    {
                //        lis.client.control.MessageDialog.Show("请不要嵌套计算公式！");
                //        if (istrue)
                //            throw new Exception();
                //        else
                //            return "";
                //    }
                //}

                variable += item;
                if (i != parameter.Count - 1)
                    variable += ",";
            }
            #endregion

            string met = meth;
            for (int i = 0; i < parameter.Count; i++)
            {
                met = met.Replace(parameter[i].ToString(), "a" + i.ToString());
            }

            #region 调用动态编译类

            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

            ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

            CompilerParameters objCompilerParameters = new CompilerParameters();
            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;
            CompilerResults cr;
            if (num == 1)
            {
                cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, GenerateCode(parameter.Count, met));
            }
            else
            {
                cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, Efficacy(parameter.Count, met));
            }
            if (cr.Errors.HasErrors)
            {
                lis.client.control.MessageDialog.Show("公式错误！请书写正确的表达式！");
                if (istrue)
                    throw new Exception();
                else
                    return "";

            }
            #endregion
            return variable;
        }

        //计算公式动态编译类
        static string GenerateCode(int count, string meth)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;");
            sb.Append(Environment.NewLine);
            sb.Append("namespace DynamicCodeGenerate");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("    public class HelloWorld");
            sb.Append(Environment.NewLine);
            sb.Append("    {");
            sb.Append(Environment.NewLine);
            sb.Append("        public double OutPut(");
            for (int i = 0; i < count; i++)
            {
                sb.Append("float ");
                sb.Append("a" + i.ToString());
                if (i != count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(")");
            sb.Append(Environment.NewLine);
            sb.Append("        {");
            sb.Append(Environment.NewLine);
            if (meth.Contains("return"))
            {
                sb.Append(meth);
            }
            else
            {
                sb.Append("            double med= ");
                sb.Append(meth);
                sb.Append("; return med;");
            }
            sb.Append(Environment.NewLine);
            sb.Append("        }");
            sb.Append(Environment.NewLine);
            sb.Append("    }");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            string code = sb.ToString();
            return code;
        }

        //效验动态编译类
        static string Efficacy(int count, string meth)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;");
            sb.Append(Environment.NewLine);
            sb.Append("namespace DynamicCodeGenerate");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("    public class HelloWorld");
            sb.Append(Environment.NewLine);
            sb.Append("    {");
            sb.Append(Environment.NewLine);
            sb.Append("        public object OutPut(");
            for (int i = 0; i < count; i++)
            {
                sb.Append("float ");
                sb.Append("a" + i.ToString());
                if (i != count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(")");
            sb.Append(Environment.NewLine);
            sb.Append("        {");
            sb.Append(Environment.NewLine);
            sb.Append("             if(");
            sb.Append(meth);
            sb.Append(")");
            sb.Append(Environment.NewLine);
            sb.Append("     return true;");
            sb.Append(Environment.NewLine);
            sb.Append("   else");
            sb.Append(Environment.NewLine);
            sb.Append("     return false;");
            sb.Append(Environment.NewLine);
            sb.Append("        }");
            sb.Append(Environment.NewLine);
            sb.Append("    }");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            string code = sb.ToString();
            return code;
        }

        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (this.lookUpEdit1.EditValue.ToString() == "2")
            {
                this.fDict_item.valueMember = null;
                this.fDict_item.displayMember = null;
                this.fDict_item.Enabled = false;
            }
            else
            {
                this.fDict_item.Enabled = true;
            }
        }

        #region 计算按钮事件
        private void gridControl2_DoubleClick(object sender, EventArgs e)
        {
            string str = "[";
            EntityDicItmItem dr = ((EntityDicItmItem)bsDictItem.Current);
            str += dr.ItmEcode.ToString();
            str += "]";
            //this.memoEdit1.Text += str;
            string strFormula = memoEdit1.Text.ToString();
            int Middle = memoEdit1.SelectionStart;
            int leng = strFormula.Length - Middle;
            memoEdit1.Text = strFormula.Substring(0, Middle) + str + strFormula.Substring(Middle, leng);
            memoEdit1.Select(memoEdit1.Text.Length, 0);
            memoEdit1.Focus();
            gridView2.ClearColumnsFilter();
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, "+");
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, "-");
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, "*");
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, "/");
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, "[");
        }

        private void simpleButton22_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, "]");
        }

        private void simpleButton23_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, "(");
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, ")");
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            this.memoEdit1.Text = "";
            memoEdit1.Select(memoEdit1.Text.Length, 0);
            memoEdit1.Focus();
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, "||");
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, "&&");
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, "!=");
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, "<");
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, ">");
        }

        private void simpleButton24_Click(object sender, EventArgs e)
        {
            string val = "";
            string meth = this.memoEdit1.Text;
            if (this.lookUpEdit1.Text == "")
            {
                lis.client.control.MessageDialog.Show("请选择公式类别！");
                return;
            }
            if (this.lookUpEdit1.Text == "计算" || this.lookUpEdit1.Text == "酶标")
                val = this.isMetch(meth, 1, false);
            else
                val = this.isMetch(meth, 2, false);

            if (val != "")
            {
                lis.client.control.MessageDialog.Show("公式正确！", "恭喜");
            }
        }

        private void btnPrescribing_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, "Math.Sqrt()");
        }

        private void btnSquare_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, "Math.Pow(,2)");
        }
        #endregion
        
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (this.bsBscript.Current != null)
            {
                EntityDicItmCalu rowView = (EntityDicItmCalu)bsBscript.Current;
                lyitemItrCal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                bool supportItrCal;
                if (rowView.CalSupportItrCal == null
                    || rowView.CalSupportItrCal == "")
                {
                    supportItrCal = false;
                }
                else
                {
                    supportItrCal = Convert.ToBoolean(rowView.CalSupportItrCal);
                }

                chkSupportItrCal.Checked = supportItrCal;
                gridView2.SortInfo.View.ActiveFilter.Clear();
            }
        }








        #region IBarActionExt 成员


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
