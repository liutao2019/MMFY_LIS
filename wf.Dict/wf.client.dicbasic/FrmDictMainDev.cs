using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using dcl.client.frame;
using dcl.client.common;
using DevExpress.XtraNavBar;
using DevExpress.XtraBars;
using System.Reflection;
using dcl.client.cache;

namespace dcl.client.dicbasic
{
    public partial class FrmDictMainDev : FrmCommon
    {
        #region 菜单列表
        private List<EntityDictMenu> _Menus;

        public List<EntityDictMenu> Menus
        {
            get
            {
                if (_Menus == null)
                    _Menus = GetDictMenus();
                return _Menus;
            }
        }

        private List<EntityDictMenu> GetDictMenus()
        {
            List<EntityDictMenu> _Menus = new List<EntityDictMenu>();

            _Menus.Add(new EntityDictMenu { ID = "ROOT1", Name = "公用字典", ParentID = "123", PicName = imageCollection1.Images["1-公用字典.png"]  });
            _Menus.Add(new EntityDictMenu { ID = "barConHospital", Name = "机构", ParentID = "ROOT1",ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConDepart", Name = "科室", ParentID = "ROOT1", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConDoctor", Name = "医生", ParentID = "ROOT1", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConOrigin", Name = "病人类型", ParentID = "ROOT1", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConNo_Type", Name = "接口类型", ParentID = "ROOT1", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConsType", Name = "专业组", ParentID = "ROOT1", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConType", Name = "实验组", ParentID = "ROOT1", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConFeesType", Name = "费用类别", ParentID = "ROOT1", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConDiagnos", Name = "ICD诊断", ParentID = "ROOT1", ClickHandler = Item_Click });

            _Menus.Add(new EntityDictMenu { ID = "ROOT2", Name = "仪器字典", ParentID = "1233" , PicName = imageCollection1.Images["2-仪器字典.png"] });
            _Menus.Add(new EntityDictMenu { ID = "barConInstrmt", Name = "仪器资料", ParentID = "ROOT2", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConInstrmtCom", Name = "仪器组合", ParentID = "ROOT2", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "BarConMitm_No", Name = "仪器通道", ParentID = "ROOT2", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "BarConResAdjust", Name = "结果调整", ParentID = "ROOT2", ClickHandler = Item_Click });

            _Menus.Add(new EntityDictMenu { ID = "ROOT3", Name = "标本字典", ParentID = "12334", PicName = imageCollection1.Images["3-标本字典.png"]  });
            _Menus.Add(new EntityDictMenu { ID = "barConSample", Name = "标本资料", ParentID = "ROOT3", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConSamRemarks", Name = "标本备注", ParentID = "ROOT3", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConS_State", Name = "标本状态", ParentID = "ROOT3", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "BarConCheckb", Name = "检查目的", ParentID = "ROOT3", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConUGR_Type", Name = "镜检类型", ParentID = "ROOT3", ClickHandler = Item_Click });

            _Menus.Add(new EntityDictMenu { ID = "ROOT4", Name = "项目字典", ParentID = "123345" , PicName = imageCollection1.Images["4-项目字典.png"]  });
            _Menus.Add(new EntityDictMenu { ID = "barConItemPro", Name = "检验项目", ParentID = "ROOT4", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConItemCombineInfo", Name = "检验组合", ParentID = "ROOT4", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConItemCombineDetail", Name = "组合明细", ParentID = "ROOT4", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConItemCombineTAT", Name = "组合TAT", ParentID = "ROOT4", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConIcdCombine", Name = "诊断组合关联", ParentID = "ROOT4", ClickHandler = Item_Click });
            if (CacheClient.EnableLisFunc())
            {
                _Menus.Add(new EntityDictMenu { ID = "BarConClItem", Name = "计算项目", ParentID = "ROOT4", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConItem_Prop", Name = "项目特征", ParentID = "ROOT4", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barconItemUrgentValue", Name = "危急值", ParentID = "ROOT4", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConReferenceName", Name = "参考值名称", ParentID = "ROOT4", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConCombineTimerule", Name = "TAT时间", ParentID = "ROOT4", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConComReptime", Name = "取报告时间", ParentID = "ROOT4", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConEfficacy", Name = "项目效验", ParentID = "ROOT4", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConBscripe", Name = "描述评价", ParentID = "ROOT4", ClickHandler = Item_Click });
            }

            //TABLE.Rows.Add(new string[] { "ROOT5", "结果字典", "123346" });
            //TABLE.Rows.Add(new string[] { "barConBscripe", "描述评价", "ROOT5" });

            _Menus.Add(new EntityDictMenu { ID = "ROOT7", Name = "细菌字典", ParentID = "1233467", PicName = imageCollection1.Images["5-细菌字典.png"] });
            _Menus.Add(new EntityDictMenu { ID = "barConDict_BType", Name = "细菌分类", ParentID = "ROOT7", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConDict_An_Stype", Name = "药敏分类", ParentID = "ROOT7", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConDict_Bacteri", Name = "细菌", ParentID = "ROOT7", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConDict_Antibio", Name = "抗生素", ParentID = "ROOT7", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConDict_An_Sstd", Name = "药敏标准", ParentID = "ROOT7", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConNobact", Name = "无菌和涂片", ParentID = "ROOT7", ClickHandler = Item_Click });
            _Menus.Add(new EntityDictMenu { ID = "barConDefAntiType", Name = "多耐规则", ParentID = "ROOT7", ClickHandler = Item_Click });
            if (CacheClient.EnableMicFunc())
                _Menus.Add(new EntityDictMenu { ID = "barConDic_Antibio_Type", Name = "抗生素大类", ParentID = "ROOT7", ClickHandler = Item_Click });

            if (CacheClient.EnableLisFunc())
            {
                _Menus.Add(new EntityDictMenu { ID = "ROOT8", Name = "条码字典", ParentID = "12334674", PicName = imageCollection1.Images["6-条码字典.png"] });
                _Menus.Add(new EntityDictMenu { ID = "barConItemCombineBarcode", Name = "合并规则", ParentID = "ROOT8", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConBCCombineSplit", Name = "大小组合", ParentID = "ROOT8", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConBCCuvette", Name = "试管类型", ParentID = "ROOT8", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConSampReturn", Name = "回退信息", ParentID = "ROOT8", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "navConTubeRack", Name = "试管架", ParentID = "ROOT8", ClickHandler = Item_Click });

                //暂时删除而已 TODO
                _Menus.Add(new EntityDictMenu { ID = "ROOT9", Name = "酶标字典", ParentID = "12334679", PicName = imageCollection1.Images["10-院感字典.png"] });
                _Menus.Add(new EntityDictMenu { ID = "barConElisaItemHole", Name = "项目设置", ParentID = "ROOT9", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConElisaHoleMode", Name = "孔位序号", ParentID = "ROOT9", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConElisaHoleStatus", Name = "孔位状态", ParentID = "ROOT9", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConElisaJudgeControl", Name = "结果判断", ParentID = "ROOT9", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConElisaCalcControl", Name = "计算公式", ParentID = "ROOT9", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConElisaModControl", Name = "临床意义", ParentID = "ROOT9", ClickHandler = Item_Click });
                

                _Menus.Add(new EntityDictMenu { ID = "ROOT10", Name = "归档字典", ParentID = "123346791", PicName = imageCollection1.Images["7-归档字典.png"] });
                _Menus.Add(new EntityDictMenu { ID = "barConDictRack", Name = "架子设定", ParentID = "ROOT10", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConIceBox", Name = "冰箱设定", ParentID = "ROOT10", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConCups", Name = "柜子设定", ParentID = "ROOT10", ClickHandler = Item_Click });

                _Menus.Add(new EntityDictMenu { ID = "ROOT11", Name = "温控字典", ParentID = "123346791123", PicName = imageCollection1.Images["8-温控字典.png"] });
                _Menus.Add(new EntityDictMenu { ID = "barConDictHarvester", Name = "采集器", ParentID = "ROOT11", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConTemperature", Name = "温度范围", ParentID = "ROOT11", ClickHandler = Item_Click });

                _Menus.Add(new EntityDictMenu { ID = "ROOT12", Name = "试剂字典", ParentID = "1233467911234", PicName = imageCollection1.Images["8-温控字典.png"] });
                _Menus.Add(new EntityDictMenu { ID = "barConReaUnit", Name = "试剂包装单位", ParentID = "ROOT12", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConReaGroup", Name = "试剂组别", ParentID = "ROOT12", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConReaProduct", Name = "生产厂商", ParentID = "ROOT12", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConReaSupplier", Name = "供货商", ParentID = "ROOT12", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConReaStorePosition", Name = "储存位置", ParentID = "ROOT12", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConReaStoreCondition", Name = "储存条件", ParentID = "ROOT12", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConReaDept", Name = "认领部门", ParentID = "ROOT12", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConReaClaimant", Name = "认领人", ParentID = "ROOT12", ClickHandler = Item_Click });
                _Menus.Add(new EntityDictMenu { ID = "barConReaReturn", Name = "回退原因", ParentID = "ROOT12", ClickHandler = Item_Click });

            }
            return _Menus;
        }
        #endregion
        
        public FrmDictMainDev()
        {
            InitializeComponent();
            this.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 100;
            this.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 50;

            this.Shown += FrmDictMainDev_Shown;
            this.Load += FrmDictMainDev_Load;
            this.FormClosing += FrmDictMainDev_FormClosing;
            this.sysToolBar1.OnBtnAddClicked += new System.EventHandler(this.onNew);
            this.sysToolBar1.OnBtnModifyClicked += new System.EventHandler(this.btnModify_Click);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.onDel);
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.onSave);
            this.sysToolBar1.OnBtnCancelClicked += new System.EventHandler(this.simpleButton1_Click);
            this.sysToolBar1.OnBtnRefreshClicked += new System.EventHandler(this.onRefresh);
        }

       

        private IBarAction barAction = null;

        //增加控制列表的项目
        Dictionary<string, Boolean> controlsList = new Dictionary<string, Boolean>();
        public Boolean defaultEnableStatus = false;
        Control parentControl = null;

        public void EnableButton()
        {
            sysToolBar1.AutoEnableButtons = false;
            sysToolBar1.BtnSave.Visibility = BarItemVisibility.Never;
            sysToolBar1.BtnCancel.Visibility = BarItemVisibility.Never;
            this.sysToolBar1.EnableButton(false);
        }

        public void ModifyButton()
        {
            sysToolBar1.BtnAdd.Visibility = BarItemVisibility.Never;
            sysToolBar1.BtnDelete.Visibility = BarItemVisibility.Never;
        }


        //遍历所有控件，以控制所有控件显示是否可编辑状态
        private void EnableControls(Control parentControl)
        {
            foreach (Control c in parentControl.Controls)
            {
                if (c.Name == "cmbSort" || c.Name == "txtSort" || c.Name == "sysItem" 
                    || c.Name == "radioGroup_Item" || c.Name == "btnSynchronous" || c.Name == "radioGroup_split" 
                    || c.Name == "txtFilter" || c.Name == "dateFrom" || c.Name == "dateEditTo"
                    || c.Name == "btnModify")
                {
                    c.Enabled = true;
                    if (c is DevExpress.XtraEditors.TextEdit)
                    {
                        ((DevExpress.XtraEditors.TextEdit)c).Properties.ReadOnly = false;
                    }
                    continue;
                }

                if (c.Controls.Count > 0)
                {
                    if (c is DevExpress.XtraGrid.GridControl)
                    {
                        //HB ,解决enable时样式难看问题
                        DevExpress.XtraGrid.GridControl g = c as DevExpress.XtraGrid.GridControl;
                        if (g != null) g.UseDisabledStatePainter = false;

                        if (controlsList.ContainsKey(c.Name))
                        {
                            c.Enabled = controlsList[c.Name];
                        }
                        else
                        {
                            c.Enabled = defaultEnableStatus;
                        }
                    }
                    else
                    {
                        if (c is DevExpress.XtraEditors.TextEdit || c is lis.client.control.HopePopSelect)
                        {
                            if (c is DevExpress.XtraEditors.TextEdit)
                            {
                                ((DevExpress.XtraEditors.TextEdit)c).Properties.ReadOnly = !defaultEnableStatus;
                            }

                            if (c is lis.client.control.HopePopSelect)
                            {
                                ((lis.client.control.HopePopSelect)c).Readonly = !defaultEnableStatus;
                            }

                        }
                        else
                        {
                            EnableControls(c);
                        }
                    }
                }
                else
                {
                    if (controlsList.ContainsKey(c.Name))
                        c.Enabled = controlsList[c.Name];
                    else
                    {
                        if (c is DevExpress.XtraEditors.LabelControl)
                            c.Enabled = true;
                        else
                        {
                            if (c is DevExpress.XtraEditors.TextEdit || c is lis.client.control.HopePopSelect)
                            {
                                if (c is DevExpress.XtraEditors.TextEdit)
                                {
                                    ((DevExpress.XtraEditors.TextEdit)c).Properties.ReadOnly = !defaultEnableStatus;
                                }

                                if (c is lis.client.control.HopePopSelect)
                                {
                                    ((lis.client.control.HopePopSelect)c).Readonly = !defaultEnableStatus;
                                }

                            }
                            else
                            {
                                c.Enabled = defaultEnableStatus;
                            }
                        }
                    }
                }
            }

            if (AfterControlsEnableSetted != null)
            {
                AfterControlsEnableSetted(this, EventArgs.Empty);
            }
        }

        public delegate void EventHandler(object obj, EventArgs args);
        public event EventHandler AfterControlsEnableSetted;

        private void FrmDictMainDev_Shown(object sender, EventArgs e)
        {
            plTop.Enabled = false;
        }

        private void FrmDictMainDev_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnAdd", "BtnModify", "BtnDelete", "BtnSave", "BtnCancel", "BtnRefresh" });
            BuildNav();
        }

        private void FrmDictMainDev_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall && lis.client.control.MessageDialog.Show("您确定要关闭当前窗口吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        #region 按钮菜单点击事件
        private void onNew(object sender, EventArgs e)
        {
            this.toNew();
        }

        //控制所有编辑按钮状态
        private void EnterEditingState(Boolean enable)
        {
            defaultEnableStatus = enable;
        }

        private void toNew()
        {
            if (barAction != null)
            {
                defaultEnableStatus = true;
                EnableControls(parentControl);
                EnterEditingState(true);

                barAction.Add();
            }
        }
        private void onSave(object sender, EventArgs e)
        {
            this.isActionSuccess = false;

            this.toSave();
        }

        private void toSave()
        {
            sysToolBar1.Focus();

            if (barAction != null)
            {
                try
                {

                    barAction.Save();

                    if (parentControl != null && parentControl is ConDicCommon)
                    {
                        this.isActionSuccess = ((ConDicCommon)parentControl).isActionSuccess;
                        if (this.isActionSuccess == false)
                        {
                            return;
                        }
                    }

                }
                catch (Exception ex)
                {
                    //Logger.WriteException("MainForm", "初始化报表设计错误", ex.ToString());
                    lis.client.control.MessageDialog.Show("保存异常:" + ex.Message, "信息");
                    return;
                }
                EnterEditingState(false);
                EnableControls(parentControl);
            }
        }
        private void onDel(object sender, EventArgs e)
        {
            this.toDel();
        }

        private void toDel()
        {
            if (barAction != null) { barAction.Delete(); }
        }
        private void onRefresh(object sender, EventArgs e)
        {
            this.toRefresh();
        }

        private void toRefresh()
        {
            if (barAction != null) { barAction.DoRefresh(); }
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            toModify();
        }

        private void toModify()
        {
            EnterEditingState(true);
            EnableControls(parentControl);

            if (barAction is IBarActionExt)
            {
                IBarActionExt tmpAction = barAction as IBarActionExt;
                tmpAction.Edit();

                if (parentControl != null && parentControl.GetType().BaseType != null)
                {
                    if (parentControl.GetType().BaseType.Name == "ConCommon")
                        this.isActionSuccess = ((ConCommon)parentControl).isActionSuccess;
                    if (parentControl.GetType().BaseType.Name == "ConDicCommon")
                        this.isActionSuccess = ((ConDicCommon)parentControl).isActionSuccess;
                    if (this.isActionSuccess == false)
                    {
                        //((ConCommon)parentControl).isActionSuccess = true;
                        return;
                    }
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            EnterEditingState(false);
            EnableControls(parentControl);
            this.toRefresh();

            if (barAction is IBarActionExt)
            {
                IBarActionExt tmpAction = barAction as IBarActionExt;
                tmpAction.Cancel();
            }
        }
        #endregion

        #region 创建 nav菜单
        /// <summary>
        /// 创建 nav菜单
        /// </summary>
        private void BuildNav()
        {
            NavBarItemHelper helper = new NavBarItemHelper();
            helper.Control = nBCMenus;
            helper.CreatItem(Menus);

        }

       
        /// <summary>
        /// Nav菜单项点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_Click(object sender, NavBarLinkEventArgs e)
        {
            if (e.Link == null)
                return;
            if (parentControl != null)
            {
                sysToolBar1.BtnSave.Visibility = BarItemVisibility.Always;
                sysToolBar1.BtnCancel.Visibility = BarItemVisibility.Always;
                sysToolBar1.BtnAdd.Visibility = BarItemVisibility.Always;
                sysToolBar1.BtnDelete.Visibility = BarItemVisibility.Always;
                sysToolBar1.AutoEnableButtons = true;
                ((IBarAction)parentControl).DoRefresh();
                this.sysToolBar1.EnableButton(false);
            }

            string ItemName = e.Link.ItemName;

            if (ItemName == "ROOT1" || ItemName == "ROOT2" || ItemName == "ROOT3" || ItemName == "ROOT12"
                || ItemName == "ROOT4" || (ItemName == "ROOT8" && ItemName == "barConItemCombineBarcode" || ItemName == "ROOT10"))
            {
                plTop.Visible = true;
            }


            Control curConDict = null;
            String conName = ItemName.Substring(3);


            foreach (Control ctrl in this.gcChildFrm.Controls)
            {
                if (ctrl.Name == conName && conName != "ConType")
                {
                    curConDict = ctrl;
                    break;
                }
            }

            var S= from a in Menus where a.ID == ItemName select a;
            if (S.Count() == 0)
                throw new Exception(string.Format("未找到名为{0}的菜单项！",e.Link.Caption));

            EntityDictMenu menu = null;
            foreach(EntityDictMenu en in S)
            {
                menu = en;
                break;
            }

            

            if (curConDict == null)
            {
                //如果还没创建
                Assembly assembly = Assembly.GetExecutingAssembly();
                Type type;
                if (menu.ParentID == "ROOT7" 
                    || menu.ParentID == "ROOT13"
                    || menu.ParentID == "ROOT14"
                    || menu.ParentID == "ROOT15"
                    || menu.ParentID == "ROOT16"
                    || menu.ParentID == "ROOT17"
                    || menu.ParentID == "ROOT18")
                {

                    assembly = Assembly.Load("dcl.client.dicmic");
                    type = assembly.GetType("dcl.client.dicmic." + conName, false, true);

                    if (type == null)
                    {
                        assembly = Assembly.GetExecutingAssembly();
                        type = assembly.GetType("dcl.client.dicbasic." + conName, false, true);
                    }
                }


                else if (menu.ParentID == "ROOT8" && menu.ID != "barConItemCombineBarcode" || menu.ParentID == "ROOT10")
                {
                    assembly = Assembly.Load("dcl.client.dicbasic");
                    type = assembly.GetType("dcl.client.dicbasic." + conName, false, true);
                }
                else if (menu.ParentID == "ROOT9")
                {
                    assembly = Assembly.Load("dcl.client.dicbasic");
                    type = assembly.GetType("dcl.client.dicbasic." + conName, false, true);
                }
                else if (menu.ParentID == "ROOT12")
                {
                    assembly = Assembly.Load("wf.client.dicreagent");
                    type = assembly.GetType("wf.client.dicreagent." + conName, false, true);
                }
                else
                {
                    type = assembly.GetType("dcl.client.dicbasic." + conName, false, true);
                }
                if (type == null)
                {
                    lis.client.control.MessageDialog.Show(conName + "不存在，无法加载，请联系管理员重新配置");
                    return;
                }
                BindingFlags bflags = BindingFlags.DeclaredOnly | BindingFlags.Public
                                                            | BindingFlags.NonPublic | BindingFlags.Instance;
                Object obj = null;
                try
                {
                    obj = type.InvokeMember("UserControl", bflags | BindingFlags.CreateInstance, null, null, null);
                }
                catch (Exception ex)
                {
                    AppError.show(ex);
                }
                if (obj == null) return;
                if (!(obj is IBarAction))
                {
                    lis.client.control.MessageDialog.Show("子窗体未能实现IBarAction接口!");
                    return;
                }
                barAction = obj as IBarAction;
                curConDict = (Control)obj;

                this.gcChildFrm.Controls.Add(curConDict);
                curConDict.Dock = DockStyle.Fill;
            }
            else
            {
                if ((menu.ParentID == "ROOT8" || menu.ParentID == "ROOT9") && menu.ID != "barConItemCombineBarcode" || menu.ParentID == "ROOT10")
                {
                    // plTop.Visible = false;
                }
                barAction = curConDict as IBarAction;
                barAction.DoRefresh();
            }

            curConDict.BringToFront();
            gcChildFrm.Text = menu.Name;
            this.Text = menu.Name;

            //增加修改控制控件的状态
            //增加保存现有的控件
            parentControl = curConDict;

            controlsList = barAction.GetActiveCtrls();
            controlsList.Add("cmbSort", true);
            controlsList.Add("txtSort", true);
            //增加控制actlist的列表      
            defaultEnableStatus = false;
            EnableControls(curConDict);
            EnterEditingState(false);
            //不显示编辑按钮
            plTop.Enabled = true;
        }
        #endregion
    }
}