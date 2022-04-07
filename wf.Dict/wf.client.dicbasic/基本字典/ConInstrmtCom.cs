using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.entity;
using lis.client.control;
using System.Linq;
using System.Diagnostics;

namespace dcl.client.dicbasic
{
    public partial class ConInstrmtCom : ConDicCommon, IBarActionExt
    {
        /// <summary>
        /// 是否为新增事件
        /// </summary>
        private bool blIsNew = false;
        /// <summary>
        /// 是否绑定用户可用物理组
        /// </summary>
        private bool IsBindingUserTypes { get; set; }

        //全局变量:存放仪器数据
        private List<EntityDicInstrument> listInstrumt = new List<EntityDicInstrument>();
        private List<EntityDicItrCombine> listCombineNotInAll = new List<EntityDicItrCombine>();

        private List<EntityDicItrCombine> listCombineIn = new List<EntityDicItrCombine>();
        private List<EntityDicItrCombine> listCombineNotIn = new List<EntityDicItrCombine>();

        private List<EntityDicItrCombine> listAllCombineIn = new List<EntityDicItrCombine>();//包含组合的所有数据
        private List<EntityDicItrCombine> listAllCombineNotIn = new List<EntityDicItrCombine>(); //未包含组合的所有数据
        private List<EntityDicItrCombine> listAllItrCom = new List<EntityDicItrCombine>(); //存放仪器包含组合单表全部的数据

        #region IBarActionExt

        public void Add()
        {
            blIsNew = true;//标记为新增事件

            this.gridControl1.Enabled = false;
            return;
        }

        public void Save()
        {

            this.bsInstrmt.EndEdit();
            if (bsInstrmt.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();

            EntityDicInstrument dr = (EntityDicInstrument)bsInstrmt.Current;
            String type_id = dr.ItrId;

            EntityResponse result = new EntityResponse();

            List<EntityDicItrCombine> dtCombineIn = gcCombineIn.DataSource as List<EntityDicItrCombine>;
            Dictionary<string, object> d = new Dictionary<string, object>();
            EntitySysOperationLog etOperation = CreateOperateInfo("修改");
            d.Add("Operation", etOperation);
            d.Add("ItrComIn", dtCombineIn);
            if (dtCombineIn.Count <= 0)
            {
                EntityDicItrCombine combine = new EntityDicItrCombine();
                combine.ItrId = dr.ItrId;
                request.SetRequestValue(combine);
                result = base.Delete(request);
            }
            else
            {
                foreach (var info in dtCombineIn)
                {
                    info.ItrId = dr.ItrId;
                }

                request.SetRequestValue(d);

                result = base.New(request);
            }

            if (base.isActionSuccess)
            {
                if (type_id == "")
                {
                    dr.ItrId = result.GetResult<EntityDicItrCombine>().ItrId;
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");
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

            DoRefresh();//刷新数据(解决保存之后未包含组合为空的情况，实际上是不为空的)
            //this.bsInstrmt.ResetCurrentItem();//刷新当前行
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
        public void Delete()
        {
            return;
        }

        public void DoRefresh()
        {
            #region 新添加代码 查询所有 已包含组合、未包含组合 数据
            EntityRequest req = new EntityRequest();
            req.SetRequestValue("SearchAllData");

            EntityResponse result = base.Search(req);

            List<List<EntityDicItrCombine>> listAllCombine = new List<List<EntityDicItrCombine>>();
            listAllCombine = result.GetResult() as List<List<EntityDicItrCombine>>;
            listAllCombineIn = listAllCombine[0]; //包含组合(全部)
            listAllCombineNotIn = listAllCombine[1]; //未包含组合(全部)

            EntityResponse result2 = base.View(new EntityRequest());//查询仪器包含组合数据（为了获取com_id的值）
            listAllItrCom = result2.GetResult() as List<EntityDicItrCombine>;

            #endregion

            //查询仪器数据
            EntityResponse inCom = base.Other(new EntityRequest());
            List<EntityDicInstrument> listCom = new List<EntityDicInstrument>();

            listCom = inCom.GetResult() as List<EntityDicInstrument>;
            //给全局变量赋值
            this.bsInstrmt.DataSource = listInstrumt = FiltrateUserTypes(listCom);

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
                result = dtObjData.Where(w => UserInfo.sqlUserTypesFilter.Contains(w.ItrTypeName)).ToList();
                return result;
            }
            else
            {
                return dtObjData;//没绑定,不过滤
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gridControl1", true);
            dlist.Add(gcCombineIn.Name, true);
            dlist.Add(gcCombineNotIn.Name, true);
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);

            return dlist;
        }

        public void Cancel()
        {
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
                blIsNew = false;//取消新增事件
        }

        public void Close() { }

        public void Edit()
        {
            blIsNew = true;
            this.gridControl1.Enabled = false;
        }

        public void MoveNext() { }

        public void MovePrev() { }

        #endregion
        //五笔和拼音码工具类
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        public ConInstrmtCom()
        {
            InitializeComponent();
            this.Name = "ConInstrmtCom";
        }

        private void on_Load(object sender, EventArgs e)
        {
            //[项目字典]是否关联用户可用物理组
            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("IsBindingUserTypes") == "是")
            {
                IsBindingUserTypes = true;
            }
            else
            {
                IsBindingUserTypes = false;//默认不绑定用户可用物理组
            }

            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);

            this.initData();

        }
        private void initData()
        {
            this.DoRefresh();
        }

       

        /// <summary>
        /// 添加所有组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAllUser_Click(object sender, EventArgs e)
        {
            List<EntityDicItrCombine> combineNotIn = gcCombineNotIn.DataSource as List<EntityDicItrCombine>;
            if (combineNotIn.Count > 0)
            {
                List<EntityDicItrCombine> combineIn = gcCombineIn.DataSource as List<EntityDicItrCombine>;
                foreach (EntityDicItrCombine itr in combineNotIn)
                {
                    combineIn.Add(itr);
                }
                gcCombineNotIn.DataSource = new List<EntityDicItrCombine>();
                gridView2.RefreshData();
                gridView4.RefreshData();
            }
        }
        /// <summary>
        /// 添加单个组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            AddSigleCombine();
        }

        /// <summary>
        /// 删除单个已包含组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelUser_Click(object sender, EventArgs e)
        {
            DelCombineIn();
        }
        /// <summary>
        /// 删除所有包含组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelAllUser_Click(object sender, EventArgs e)
        {
            List<EntityDicItrCombine> combineIn = gcCombineIn.DataSource as List<EntityDicItrCombine>;
            if (combineIn.Count > 0)
            {
                List<EntityDicItrCombine> combineNotIn = gcCombineNotIn.DataSource as List<EntityDicItrCombine>;
                foreach (EntityDicItrCombine itr in combineIn)
                {
                    combineNotIn.Add(itr);
                }
                gcCombineIn.DataSource = new List<EntityDicItrCombine>();
                gridView2.RefreshData();
                gridView4.RefreshData();
            }
        }

        private void bsInstrmt_CurrentChanged(object sender, EventArgs e)
        {
            if (bsInstrmt.Current != null)
            {
                EntityDicInstrument drIns = bsInstrmt.Current as EntityDicInstrument;

                if (drIns == null) return;
                String itr_id = drIns.ItrId;

                #region 已包含组合、未包含组合过滤数据之后赋值于控件

                //已包含组合根据仪器ID过滤
                listCombineIn = listAllCombineIn.Where(w => w.ItrId == itr_id).ToList();


                //根据仪器Id过滤仪器包含组合数据（获取CombineComId）
                List<EntityDicItrCombine> listItrComId = new List<EntityDicItrCombine>();
                listItrComId = listAllItrCom.Where(w => w.ItrId == itr_id && !string.IsNullOrEmpty(w.ComId)).ToList();
                //未包含组合过滤 not in 过滤
                listCombineNotInAll = listAllCombineNotIn.FindAll(w => listItrComId.FindIndex(i => i.ComId == w.CombineComId) < 0).ToList();

                gcCombineIn.DataSource = listCombineIn;
                this.gridView4.FocusedRowHandle = listCombineIn.Count - 1; //锁定到最后一行

                gcCombineNotIn.DataSource = listCombineNotIn = listCombineNotInAll;
                #endregion

                lueType.valueMember = drIns.ItrLabId;
                lueType.displayMember = drIns.ItrTypeName;

                lueType_ValueChanged(sender, null);
            }
        }

        //过滤事件（未包含组合）
        private void lueType_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (lueType.valueMember != null && lueType.valueMember != "")
            {
                if (listCombineNotInAll.Count > 0)
                {
                    List<EntityDicItrCombine> listItrComNotIn = listCombineNotInAll.Where(w => w.TypeId.Contains(lueType.valueMember)).ToList();

                    this.gcCombineNotIn.DataSource = listCombineNotIn = listItrComNotIn;
                }
                else
                {
                    return;
                }
            }
        }

        private void gcCombineNotIn_DoubleClick(object sender, EventArgs e)
        {
            if (blIsNew)
            {
                AddSigleCombine();
            }
        }
        private void gcCombineIn_DoubleClick(object sender, EventArgs e)
        {
            if (blIsNew)
            {
                DelCombineIn();
            }
        }

        private void AddSigleCombine()
        {
            List<EntityDicItrCombine> combineNotIn = gcCombineNotIn.DataSource as List<EntityDicItrCombine>;
            EntityDicItrCombine combineNotInRow = gridView2.GetFocusedRow() as EntityDicItrCombine;
            if (combineNotIn != null)
            {
                List<EntityDicItrCombine> combineIn = gcCombineIn.DataSource as List<EntityDicItrCombine>;
                combineIn.Add(combineNotInRow);
                combineNotIn.Remove(combineNotInRow);

                gridView2.RefreshData();
                gridView4.RefreshData();
            }
        }
        private void DelCombineIn()
        {
            List<EntityDicItrCombine> combineIn = gcCombineIn.DataSource as List<EntityDicItrCombine>;
            EntityDicItrCombine combineNotInRow = gridView4.GetFocusedRow() as EntityDicItrCombine;
            if (combineIn != null)
            {
                List<EntityDicItrCombine> combineNotIn = gcCombineNotIn.DataSource as List<EntityDicItrCombine>;
                combineNotIn.Add(combineNotInRow);
                combineIn.Remove(combineNotInRow);
                gridView2.RefreshData();
                gridView4.RefreshData();
            }
        }

        private void txtItemSort_TextChanged(object sender, EventArgs e)
        {
            string sort = txtItemSort.Text.Trim().ToUpper().Replace("'", "''");
            List<EntityDicItrCombine> listNotIn = new List<EntityDicItrCombine>();
            listNotIn = listCombineNotIn;
            if (sort.Trim() == string.Empty)
            {
                gcCombineNotIn.DataSource = listCombineNotIn;
            }
            else
            {
                listNotIn = listNotIn.Where(w => w.CombineComId.Contains(sort) ||
                                                (w.ComName != null && w.ComName.Contains(sort)) ||
                                                (w.ComCode != null && w.ComCode.Contains(sort)) ||
                                                 w.ComPyCode.Contains(sort) ||
                                                 w.ComWbCode.Contains(sort)
                                            ).ToList();
            }
            gcCombineNotIn.DataSource = listNotIn;
        }
    }
}

