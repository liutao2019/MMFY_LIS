using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraLayout.Utils;
using Microsoft.CSharp;
using dcl.client.frame;
using dcl.client.common;
using lis.client.control;
using dcl.entity;
using System.Linq;
using dcl.common;

namespace dcl.client.dicbasic
{
    public partial class ConEfficacy : ConDicCommon, IBarActionExt
    {
        #region 全局信息

        //全局变量  规则组
        List<EntityDicItmCheck> dtGroup = new List<EntityDicItmCheck>();
        //全局变量  校验规则
        List<EntityDicItmCheckDetail> dtEfficacyItem = new List<EntityDicItmCheckDetail>();

        //private long randCheckId = 10009;

        List<EntityDicItmCheck> dtGroupDeleted = new List<EntityDicItmCheck>();
        List<EntityDicItmCheckDetail> dtEfficacyItemDeleted = new List<EntityDicItmCheckDetail>();
        List<EntityDicItmItem> dictItemList = new List<EntityDicItmItem>();

        /// <summary>
        /// 是否绑定用户可用物理组
        /// </summary>
        private bool IsBindingUserTypes { get; set; }

        /// <summary>
        /// 是否为新增事件
        /// </summary>
        bool blIsNew = false;

        #endregion

        #region 构造与初始化加载

        public ConEfficacy()
        {
            InitializeComponent();
            Name = "ConEfficacy";
        }

        private void ConEfficacy_Load(object sender, EventArgs e)
        {
            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gvItr, _gridLocalizer);

            //[项目字典]是否关联用户可用物理组
            if (ConfigHelper.GetSysConfigValueWithoutLogin("IsBindingUserTypes") == "是")
            {
                IsBindingUserTypes = true;
            }
            else
            {
                IsBindingUserTypes = false; //默认不绑定用户可用物理组
            }
            InitData();
        }

        private void InitData()
        {
            DoRefresh();

            DataTable dtType = new DataTable();
            dtType.Columns.Add("type_id");
            dtType.Columns.Add("type_name");

            dtType.Rows.Add("1", "检验结果效验");
            dtType.Rows.Add("2", "病人资料效验");
            lookUpEditType.Properties.DataSource = dtType;


            DataTable dtType2 = new DataTable();
            dtType2.Columns.Add("type_id");
            dtType2.Columns.Add("type_name");

            dtType2.Rows.Add("1", "结果效验");
            dtType2.Rows.Add("2", "资料效验");

            repositoryItemLookUpEditEfType.DataSource = dtType2;

            bsPatients.DataSource = CommonValue.GetEfficacyPatients();

            //过滤掉已经停用的项目(下拉控件)
            selectDicItmItem1.SetFilter(selectDicItmItem1.getDataSource().FindAll(w => w.ItmDelFlag == "0").ToList());

            SetControlEditStatus(false);
        }

        private void SetControlEditStatus(bool enable)
        {
            gvGroup.OptionsBehavior.Editable = enable;
            gridControlGroup.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = enable;
            gridControlGroup.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = enable;
            gridControlEfficacy.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = enable;
            gridControlEfficacy.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = enable;
            gridControlItr.Enabled = !enable;
            btnCopy.Enabled = !enable;
        }

        /// <summary>
        /// 过滤用户可用物理组
        /// </summary>
        /// <param name="dtObjData"></param>
        /// <returns></returns>
        private List<EntityDicInstrument> FiltrateUserTypes(List<EntityDicInstrument> dtObjData)
        {
            List<EntityDicInstrument> result = new List<EntityDicInstrument>();

            if (dtObjData == null || dtObjData.Count <= 0)
                return dtObjData;//为空,不过滤

            if (IsBindingUserTypes && !UserInfo.isAdmin) //是否绑定用户可用物理组,并且非admin用户
            {
                result = dtObjData.Where(w => UserInfo.sqlUserTypesFilter.Contains(w.ItrLabId)).ToList();
                return result;
            }
            else
            {
                return dtObjData;//没绑定,不过滤
            }
        }

        #endregion

        #region 行变事件

        /// <summary>
        /// 仪器行变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsInstrmt_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                if (bsInstrmt.Current != null)
                {
                    EntityDicInstrument drIns = bsInstrmt.Current as EntityDicInstrument;
                    if (drIns == null) return;
                    String itr_id = drIns.ItrId;

                    EntityRequest req = new EntityRequest();
                    req.SetRequestValue(itr_id);

                    EntityResponse checkList = base.Search(req);

                    List<Object> listAll = new List<Object>();
                    listAll = checkList.GetResult() as List<Object>;

                    //校验规则赋值
                    dtEfficacyItem = new List<EntityDicItmCheckDetail>();
                    List<EntityDicItmCheckDetail> info = new List<EntityDicItmCheckDetail>();
                    info = listAll[1] as List<EntityDicItmCheckDetail>; //给全局变量 校验组赋值
                    dtEfficacyItem.AddRange(info);
                    bsEfficacyItem.DataSource = dtEfficacyItem;

                    //规则组赋值
                    List<EntityDicItmCheck> itmCheck = listAll[0] as List<EntityDicItmCheck>;
                    dtGroup = new List<EntityDicItmCheck>();
                    dtGroup.AddRange(itmCheck);  // 给全局变量 规则组赋值
                    bsGroup.DataSource = itmCheck;

                }
            }
            catch (Exception exception)
            {
                MessageDialog.Show(exception.ToString(), "操作失败");
            }
        }

        /// <summary>
        ///  规则组行变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsGroup_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                if (bsGroup.Current != null)
                {
                    EntityDicItmCheck check = bsGroup.Current as EntityDicItmCheck;
                    if (check == null || check.CheckId == null)
                        return;
                    string groupID = check.CheckId;

                    bsEfficacyItem.DataSource = dtEfficacyItem.Where(w => w.CheckIdDetial.Contains(groupID)).ToList();

                }
            }
            catch (Exception exception)
            {
                MessageDialog.Show(exception.ToString(), "操作失败");
            }
        }

        #endregion

        #region EmbeddedNavigator按钮事件 规则组（+，-）

        private void gridControlGroup_EmbeddedNavigator_ButtonClick(object sender,
                                                                    NavigatorButtonClickEventArgs
                                                                        e)
        {
            try
            {
                if (bsInstrmt.Current == null)
                    return;

                if (e.Button.Tag.ToString() == "ADD")
                {
                    gvGroup.CloseEditor();
                    EntityDicInstrument dr = bsInstrmt.Current as EntityDicInstrument;

                    EntityDicItmCheck drDetail = new EntityDicItmCheck();
                    List<EntityDicItmCheck> listRow = bsGroup.DataSource as List<EntityDicItmCheck>;
                    int rowCount = listRow.Count;

                    drDetail.CheckItrId = dr.ItrId;
                    drDetail.CheckId = Guid.NewGuid().ToString();
                    // drDetail.CheckId = Convert.ToString(randCheckId);
                    //randCheckId++;
                    drDetail.SeqNo = rowCount;

                    bsGroup.Insert(rowCount, drDetail);
                    dtGroup.Add(drDetail);
                    bsGroup.EndEdit();
                    bsGroup.ResetCurrentItem();
                    gvGroup.FocusedRowHandle = dtGroup.Count - 1;
                }
                if (e.Button.Tag.ToString() == "DELETE" && bsGroup.Current != null)
                {
                    EntityDicItmCheck drItemGrp = bsGroup.Current as EntityDicItmCheck;
                    string groupID = drItemGrp.CheckId.ToString();

                    dtGroup.Remove(drItemGrp);//规则组全局变量移除选中的对象

                    bsGroup.RemoveCurrent();
                    bsGroup.CurrencyManager.Refresh();

                    //筛选出删除的规则组所包含的校验规则
                    List<EntityDicItmCheckDetail> dtEfficacyItemDel = dtEfficacyItem.Where(w => w.CheckIdDetial != groupID).ToList();
                    dtEfficacyItem = dtEfficacyItemDel;

                    List<EntityDicItmCheckDetail> detailList = bsEfficacyItem.DataSource as List<EntityDicItmCheckDetail>;
                    foreach (EntityDicItmCheckDetail detailInfo in detailList)
                    {
                        if (detailInfo.CheckIdDetial.ToString() == groupID)
                        {
                            if (groupID.Length <= 12)
                            {
                                dtEfficacyItemDeleted.Add(detailInfo);
                            }
                            bsEfficacyItem.Remove(detailInfo);
                        }
                    }
                    int count = 1;
                    foreach (EntityDicItmCheck group in dtGroup)
                    {
                        group.SeqNo = count;
                        count++;
                    }

                    bsGroup.CurrencyManager.Refresh();
                    gridControlGroup.RefreshDataSource();
                }

            }
            catch (Exception exception)
            {
                MessageDialog.Show(exception.ToString(), "操作失败");
            }
        }
        #endregion

        #region EmbeddedNavigator按钮事件 校验规则（+，-）
        private void gridControlEfficacy_EmbeddedNavigator_ButtonClick(object sender,
                                                                       NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (bsGroup.Current == null)
                    return;

                if (e.Button.Tag.ToString() == "ADD")
                {
                    gvEfficacy.CloseEditor();

                    EntityDicItmCheck dr = bsGroup.Current as EntityDicItmCheck;

                    EntityDicItmCheckDetail drDetail = new EntityDicItmCheckDetail();
                    List<EntityDicItmCheckDetail> table = bsEfficacyItem.DataSource as List<EntityDicItmCheckDetail>;

                    int rowCount = table.Count;
                    drDetail.CheckIdDetial = dr.CheckId;
                    drDetail.SeqNoDetail = rowCount;
                    drDetail.CheckTypeDetail = "1";
                    drDetail.IsNew = 1;

                    bsEfficacyItem.Insert(rowCount, drDetail);

                    dtEfficacyItem.Add(drDetail);

                    bsEfficacyItem.EndEdit();
                    bsEfficacyItem.ResetCurrentItem();

                    gvEfficacy.FocusedRowHandle = bsEfficacyItem.Count - 1;
                }
                if (e.Button.Tag.ToString() == "DELETE" && bsEfficacyItem.Current != null)
                {
                    EntityDicItmCheckDetail drItemEff = bsEfficacyItem.Current as EntityDicItmCheckDetail;

                    dtEfficacyItem.Remove(drItemEff);

                    bsEfficacyItem.RemoveCurrent();
                }
            }
            catch (Exception exception)
            {
                MessageDialog.Show(exception.ToString(), "操作失败");
            }
        }
        #endregion

        #region IBarActionExt 增删改查刷新 按钮操作

        public void Add()
        {
            try
            {
                SetControlEditStatus(true);
            }
            catch (Exception exception)
            {
                MessageDialog.Show(exception.ToString(), "操作失败");
            }
        }

        public void Save()
        {
            try
            {
                this.isActionSuccess = false;

                this.bsGroup.EndEdit();
                this.bsEfficacyItem.EndEdit();

                EntityDicInstrument instrumt = gvItr.GetFocusedRow() as EntityDicInstrument;

                List<EntityDicItmCheck> check = dtGroup;
                List<EntityDicItmCheckDetail> checkDetail = dtEfficacyItem;

                if (instrumt == null)
                    return;

                EntityDicItmCheck itmCheck = new EntityDicItmCheck();
                itmCheck.CheckItrId = instrumt.ItrId;

                int i = 0;
                foreach (EntityDicItmCheck chkInfo in check)
                {
                    if (chkInfo.CheckName == null || chkInfo.CheckName.ToString().Equals(""))
                    {
                        MessageDialog.Show("规则组列表规则名称不能为空", "提示");
                        bsGroup.Position = i;
                        return;
                    }
                    i++;
                }
                foreach (EntityDicItmCheckDetail chkDetailInfo in checkDetail)
                {
                    if (chkDetailInfo.CheckExpression == null || string.IsNullOrEmpty(chkDetailInfo.CheckExpression.ToString()))
                    {
                        MessageDialog.Show("效验公式不能为空", "提示");
                        return;
                    }
                    try
                    {
                        string val = IsMetch(chkDetailInfo.CheckExpression.ToString(), chkDetailInfo.CheckTypeDetail.ToString(), true);
                        chkDetailInfo.CheckVariable = val;
                    }
                    catch (Exception)
                    {
                        isActionSuccess = true;
                        return;
                    }
                }

                EntityRequest request = new EntityRequest();
                List<Object> updateList = new List<object>();

                updateList.Add(itmCheck);

                updateList.Add(check);
                updateList.Add(checkDetail);

                request.SetRequestValue(updateList);

                base.Update(request);

                isActionSuccess = true;

                DoRefresh();
                SetControlEditStatus(false);
            }
            catch (Exception exception)
            {
                MessageDialog.Show(exception.ToString(), "操作失败");
            }
        }

        public void Copy(string itr_id)
        {
            try
            {
                if (bsInstrmt.Current == null)
                    return;
                bsGroup.EndEdit();
                bsEfficacyItem.EndEdit();

                EntityDicItmCheck dr = this.gvGroup.GetFocusedRow() as EntityDicItmCheck;
                dr.CheckId = getString(15);
                dr.CheckItrId = itr_id;

                //获取需要被复制的规则组的行数
                EntityRequest req = new EntityRequest();
                req.SetRequestValue(itr_id);
                EntityResponse checkList = base.Search(req);
                List<Object> listAll = new List<Object>();
                listAll = checkList.GetResult() as List<Object>;
                List<EntityDicItmCheck> countNum = listAll[0] as List<EntityDicItmCheck>;

                dr.SeqNo = countNum.Count + 1;//将行数+1后赋值给序号值

                List<EntityDicItmCheck> dtGp = new List<EntityDicItmCheck>();
                List<EntityDicItmCheckDetail> dtEff = bsEfficacyItem.DataSource as List<EntityDicItmCheckDetail>;
                dtGp.Add(dr);

                int i = 1;
                foreach (var dtEFFDetail in dtEff)
                {
                    dtEFFDetail.CheckIdDetial = dr.CheckId;
                    dtEFFDetail.SeqNoDetail = i + 1;
                    dtEFFDetail.CheckTypeDetail = "1";
                    dtEFFDetail.IsNew = 1;
                    i++;
                }

                EntityRequest request = new EntityRequest();
                List<Object> updateList = new List<object>();

                EntityDicItmCheck itmCheck = new EntityDicItmCheck();

                updateList.Add(itmCheck);
                updateList.Add(dtGp);
                updateList.Add(dtEff);

                request.SetRequestValue(updateList);

                base.Update(request);

                MessageDialog.ShowAutoCloseDialog("复制成功!");

                DoRefresh();
            }
            catch (Exception exception)
            {
                MessageDialog.Show(exception.ToString(), "复制失败!");
            }
        }

        public void Delete()
        {
            try
            {
                if (bsInstrmt.Current == null)
                    return;
                bsGroup.EndEdit();
                bsEfficacyItem.EndEdit();

                DialogResult dresult = MessageBox.Show("确定要删除该仪器下所有效验规则吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                if (dresult != DialogResult.OK)
                    return;

                EntityDicInstrument instrmt = bsInstrmt.Current as EntityDicInstrument;
                if (instrmt == null) return;
                EntityRequest drInfo = new EntityRequest();
                string itrId = instrmt.ItrId;
                drInfo.SetRequestValue(itrId);

                base.Delete(drInfo);
                DoRefresh();
            }
            catch (Exception exception)
            {
                MessageDialog.Show(exception.ToString(), "删除失败");
            }
        }
        private List<EntityDicInstrument> list = new List<EntityDicInstrument>();
        public void DoRefresh()
        {
            try
            {
                isActionSuccess = true;
                dtEfficacyItemDeleted = null;
                dtGroupDeleted = null;

                EntityRequest request = new EntityRequest();

                EntityResponse result = new EntityResponse();
                result = base.View(request);

                List<Object> listAll = new List<Object>();

                listAll = result.GetResult() as List<Object>;
                if (listAll.Count > 0)
                {
                    List<EntityDicInstrument> instrList = listAll[0] as List<EntityDicInstrument>;
                    this.bsInstrmt.DataSource = list = FiltrateUserTypes(instrList);

                    List<EntityDicItmItem> filterItmItem = new List<EntityDicItmItem>();
                    filterItmItem = listAll[1] as List<EntityDicItmItem>;
                    filterItmItem = filterItmItem.Where(w => w.ItmDelFlag == "0").ToList();//过滤已停用的项目

                    dictItemList = filterItmItem; //给全局变量赋值

                    this.bsDictItem.DataSource = filterItmItem;
                }

            }
            catch (Exception exception)
            {
                MessageDialog.Show(exception.ToString(), "刷新失败");
            }

        }

        public Dictionary<string, bool> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gridControlItr", true);
            dlist.Add("gridControlEfficacy", true);
            dlist.Add("gridControlGroup", true);
            dlist.Add(splitContainerControl1.Panel1.Name, true);
            dlist.Add(btnCopy.Name, true);

            return dlist;
        }

        public void Cancel()
        {
            try
            {
                SetControlEditStatus(false);
                DoRefresh();
            }
            catch (Exception exception)
            {
                MessageDialog.Show(exception.ToString(), "操作失败");
            }
        }

        public void Edit()
        {
            try
            {
                SetControlEditStatus(true);
            }
            catch (Exception exception)
            {
                MessageDialog.Show(exception.ToString(), "操作失败");
            }
        }

        public void Close()
        {
            //throw new NotImplementedException();
        }

        public void MoveNext()
        {
            //throw new NotImplementedException();
        }

        public void MovePrev()
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region 效验按钮与公式验证

        private void gridControlItem_DoubleClick(object sender, EventArgs e)
        {
            string str = "[";
            EntityDicItmItem dr = bsDictItem.Current as EntityDicItmItem;
            str += dr.ItmEcode.ToString();
            str += "]";
            //this.memoEdit1.Text += str;
            string strFormula = memoEditFmla.Text.ToString();
            int Middle = memoEditFmla.SelectionStart;
            int leng = strFormula.Length - Middle;
            memoEditFmla.Text = strFormula.Substring(0, Middle) + str + strFormula.Substring(Middle, leng);
            memoEditFmla.Select(memoEditFmla.Text.Length, 0);
            memoEditFmla.Focus();
            gvitem.ClearColumnsFilter();
        }

        private void gridControlPatient_DoubleClick(object sender, EventArgs e)
        {
            string str = "[";
            DataRow dr = ((DataRowView)bsPatients.Current).Row;
            str += dr["column_name"].ToString();
            str += "]";
            //this.memoEdit1.Text += str;
            string strFormula = memoEditFmla.Text.ToString();
            int Middle = memoEditFmla.SelectionStart;
            int leng = strFormula.Length - Middle;
            memoEditFmla.Text = strFormula.Substring(0, Middle) + str + strFormula.Substring(Middle, leng);
            memoEditFmla.Select(memoEditFmla.Text.Length, 0);
            memoEditFmla.Focus();
            gvitem.ClearColumnsFilter();
        }

        private void btnInsertString_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEditFmla, (sender as SimpleButton).Text);
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            this.memoEditFmla.Text = "";
            memoEditFmla.Select(memoEditFmla.Text.Length, 0);
            memoEditFmla.Focus();
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEditFmla, "&&");
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEditFmla, "||");
        }

        private void btnSquare_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEditFmla, "Math.Pow(,2)");
        }

        private void btnPrescribing_Click(object sender, EventArgs e)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEditFmla, "Math.Sqrt()");
        }

        private void simpleButton24_Click(object sender, EventArgs e)
        {
            string val = "";
            string meth = memoEditFmla.Text;
            if (this.lookUpEditType.Text == "" || lookUpEditType.EditValue == null)
            {
                MessageDialog.Show("请选择公式类别！");
                return;
            }
            val = IsMetch(meth, lookUpEditType.EditValue.ToString(), false);

            if (val != "")
            {
                MessageDialog.Show("公式正确！", "恭喜");
            }
        }

        //验证效验公式类
        private string IsMetch(string meth, string eftype, bool istrue)
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

            if (count1 != count2 || count1 == 0)
            {
                MessageDialog.Show("变量请用[]括起来或[]个数不匹配", "提示");
                if (istrue)
                    throw new Exception();
                return "";
            }
            if (count3 != count4)
            {
                MessageDialog.Show("()个数不匹配，请检查", "提示");
                if (istrue)
                    throw new Exception();
                else
                    return "";
            }


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
                parameter.Add(meth.Substring(Convert.ToInt32(list[i]),
                                             Convert.ToInt32(list2[i]) - Convert.ToInt32(list[i]) + 1));
            }

            #endregion

            #region 检验是否存在变量值

            //这里取出的的数据没有用上，暂时蔽掉
            //DataTable dtItm = new FuncLibClient().getDictFromCache(new String[] { "Dict_Item" }).Tables[0];

            string variable = "";
            for (int i = 0; i < parameter.Count; i++)
            {
                string item = parameter[i].ToString().Substring(1, parameter[i].ToString().Length - 2);

                if (eftype == "2")
                {
                    DataTable pTable = (DataTable)bsPatients.DataSource;
                    DataRow[] rows = pTable.Select("column_name ='" + item + "' ");
                    if (pTable != null && rows.Length == 0)
                    {
                        MessageDialog.Show("不存在病人资料变量值" + item, "提示");
                        if (istrue)
                            throw new Exception();
                        return "";
                    }
                    item = rows[0]["column_id"].ToString();
                }
                else
                {
                    bool isFind = false;
                    if (dictItemList.Count > 0)  //判断全局变量是否有数据
                    {
                        foreach (var dictItm in dictItemList)
                        {
                            if (dictItm.ItmEcode.ToString() == item)
                            {
                                isFind = true;
                                break;
                            }
                        }
                    }

                    if (!isFind)
                    {
                        MessageDialog.Show("不存在变量值" + item, "提示");
                        if (istrue)
                            throw new Exception();
                        return "";
                    }
                }


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
            cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, EfficacyNum(parameter.Count, met));


            if (cr.Errors.HasErrors)
            {
                CompilerResults cr2;
                //再次检查一次字符串比较
                cr2 = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters,
                                                                EfficacyString(parameter.Count, met));
                if (cr2.Errors.HasErrors)
                {
                    MessageDialog.Show("公式错误！请书写正确的表达式！");

                    if (istrue)
                        throw new Exception();
                    return "";
                }
            }

            #endregion

            return variable;
        }

        //效验动态编译类
        private static string EfficacyNum(int count, string meth)
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

        //效验动态编译类
        private static string EfficacyString(int count, string meth)
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
                sb.Append("string ");
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

        private void lookUpEditType_EditValueChanged(object sender, EventArgs e)
        {
            if (lookUpEditType.EditValue != null && lookUpEditType.EditValue.ToString() == "2")
            {
                layoutControlItem2.Visibility = LayoutVisibility.Always;
                layoutControlItem19.Visibility = LayoutVisibility.Never;
            }
            else
            {
                layoutControlItem2.Visibility = LayoutVisibility.Never;
                layoutControlItem19.Visibility = LayoutVisibility.Always;
            }
        }

        #endregion

        /// <summary>
        /// 随机生成字符串
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private string getString(int count)
        {
            int number;
            string checkCode = String.Empty;     //存放随机码的字符串   

            System.Random random = new Random();

            for (int i = 0; i < count; i++) //产生4位校验码   
            {
                number = random.Next();
                number = number % 36;
                if (number < 10)
                {
                    number += 48;    //数字0-9编码在48-57   
                }
                else
                {
                    number += 55;    //字母A-Z编码在65-90   
                }

                checkCode += ((char)number).ToString();
            }
            return checkCode;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (bsGroup.Current == null || bsEfficacyItem.Current == null)
            {
                lis.client.control.MessageDialog.Show("请选择需要复制的规则组和校验规则！", "提示");
                return;
            }
            FrmPatInfoCopy frmCopy = new FrmPatInfoCopy();
            frmCopy.ShowDialog();
            if (frmCopy.isSeccess)
            {
                Copy(frmCopy.itrID);
            }
        }

        
    }
}