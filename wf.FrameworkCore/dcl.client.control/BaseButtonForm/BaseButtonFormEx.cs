using dcl.client.frame;
using dcl.client.wcf;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace dcl.client.control
{
    public partial class BaseButtonFormEx : CustomForm
    {
        #region 事件类

        private class ToolStripAndEvent
        {
            public BarLargeButtonItem TargetButton { get; set; }
            public ItemClickEventHandler BeClickEvent { get; set; }
        }
        
        #endregion

        #region 成员定义

        Dictionary<int, ToolStripAndEvent> HotKeyDict = new Dictionary<int, ToolStripAndEvent>();

        //忽略父级按键处理控件集合
        public HashSet<Control> IgnoreKeyPressControls = new HashSet<Control>();

        public InputStatus _Status = InputStatus.View;

        // KeyPress后自动TAB
        public bool AutoTabKeyPress { get; set; }

        #endregion

        #region 构造函数

        public BaseButtonFormEx()
        {
            InitializeComponent();
            AutoTabKeyPress = true;

            this.KeyPress += BaseButtonForm_KeyPress;
            this.KeyDown += BaseButtonForm_KeyDown;
            this.barManager.ItemClick += Bitem_ItemClick;
        }
       
        #endregion

        #region 根据字符串找到对应的工具按钮

        public BarLargeButtonItem GetComandButton(string Caption)
        {
            foreach (LinkPersistInfo item in this.bar.LinksPersistInfo)
            {
                BarLargeButtonItem bitem = (BarLargeButtonItem)item.Item;
                if (Caption.Equals(bitem.Caption))
                    return bitem;
            }

            //foreach (ToolStripItem tsi in CommandtoolStrip.Items)
            //{
            //    if (tsi is ToolStripDropDownButton)
            //    {
            //        ToolStripDropDownButton tsddb = tsi as ToolStripDropDownButton;
            //        foreach (ToolStripItem ddi in tsddb.DropDownItems)
            //        {
            //            if (Caption.Equals(ddi.Text))
            //                return ddi;
            //        }
            //    }
            //    if (tsi is ToolStripSplitButton)
            //    {
            //        ToolStripSplitButton tsddb = tsi as ToolStripSplitButton;
            //        foreach (ToolStripMenuItem ddi in tsddb.DropDown.Items)
            //        {
            //            if (Caption.Equals(ddi.Text))
            //                return ddi;
            //        }
            //    }
            //    if (Caption.Equals(tsi.Text))
            //    {
            //        return tsi;
            //    }
            // }
            return null;
        }

        #endregion

        #region 对工具栏增加一个button按钮

        private void Bitem_ItemClick(object sender, ItemClickEventArgs e)
        {
            BarLargeButtonItem item = (BarLargeButtonItem)e.Item;
            var parentForm = this.FindForm();
            if (item != null
                && !item.Caption.Contains("关闭")
                && !item.Caption.Contains("查询")
                && parentForm != null
                && !string.IsNullOrEmpty(parentForm.Text))
            {
                if (UserInfo.GetSysConfigValue("SaveOperationLog") == "是")
                {
                    //保存操作日志
                    ProxyLogin log = new ProxyLogin();
                    log.Service.InsertSystemLog(item.Caption, parentForm.Text, UserInfo.loginID, UserInfo.ip, UserInfo.mac, item.Name);
                }
            }
        }

        /// <summary>
        /// 对工具栏增加一个按钮
        /// </summary>
        /// <param name="Caption">按钮标题</param>
        /// <param name="image">图案</param>
        /// <param name="ClickEvent">被按下的事件</param>
        /// <param name="Hotkey">快捷键</param>
        public BarLargeButtonItem AddComandButton(string Caption, Image image, ItemClickEventHandler ClickEvent, int Hotkey)
        {
            try
            {
                Bitmap imageBitmap = new Bitmap(image);
                BarLargeButtonItem barLargeItem = new BarLargeButtonItem(barManager, Caption);
                barLargeItem.LargeGlyph = imageBitmap;//也可以设置 barLargeItem.LargeImageIndex,但是效果不是很好，可以试试                
                barLargeItem.Hint = Caption;
                barLargeItem.Tag = Caption;
                barLargeItem.Name = Caption;
                barLargeItem.Id = barManager.Items.Count + 1;
                bar.LinksPersistInfo.Add(new LinkPersistInfo(barLargeItem, true));
                barManager.Items.Add(barLargeItem);
                barLargeItem.ItemClick += ClickEvent;
                ToolStripAndEvent toolStripAndEvent = new ToolStripAndEvent()
                {
                    BeClickEvent = ClickEvent,
                    TargetButton = barLargeItem,
                };

                if (!HotKeyDict.ContainsKey(Hotkey))
                    HotKeyDict.Add(Hotkey, toolStripAndEvent);
                return barLargeItem;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return null;
            }
        }

        public BarLargeButtonItem GenComandButton(string Caption, Image image, ItemClickEventHandler ClickEvent, int Hotkey, string tip = "")
        {
            try
            {
                Bitmap imageBitmap = new Bitmap(image);
                BarLargeButtonItem barLargeItem = new BarLargeButtonItem(barManager, Caption);
                barLargeItem.LargeGlyph = imageBitmap;               
                barLargeItem.Hint = Caption;
                barLargeItem.Tag = Caption;
                barLargeItem.Name = Caption;
                barLargeItem.Id = barManager.Items.Count + 1;
                barLargeItem.ItemClick += ClickEvent;
                ToolStripAndEvent toolStripAndEvent = new ToolStripAndEvent()
                {
                    BeClickEvent = ClickEvent,
                    TargetButton = barLargeItem,
                };

                if (!HotKeyDict.ContainsKey(Hotkey))
                    HotKeyDict.Add(Hotkey, toolStripAndEvent);
                return barLargeItem;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return null;
            }
        }

        public void GenDropDownButton(BarManager bm, BarLargeButtonItem parentItem, BarLargeButtonItem[] childs)
        {
            if (childs == null || childs.Count() <=0)
                return;

            PopupMenu popupMenu = new PopupMenu();
            popupMenu.Manager = bm;

            foreach (BarLargeButtonItem child in childs)
            {
                popupMenu.LinksPersistInfo.Add(new DevExpress.XtraBars.LinkPersistInfo(child));
            }
            parentItem.ButtonStyle = BarButtonStyle.DropDown;
            parentItem.DropDownControl = popupMenu;
        }

        private SuperToolTip GenSuperTip(string tip)
        {
            ToolTipTitleItem toolTipTitleItem1 = new ToolTipTitleItem();
            toolTipTitleItem1.Text = tip;

            SuperToolTip superToolTip = new SuperToolTip();
            superToolTip.Items.Add(toolTipTitleItem1);

            return superToolTip;
        }

        public BarManager GetBarManagerInstance()
        {
            return this.barManager;
        }

        #region 对工具栏增加一个透明色按钮

        /// <summary>
        /// 对工具栏增加一个透明色按钮
        /// </summary>
        /// <param name="Caption">按钮标题</param>
        /// <param name="image">图案</param>
        /// <param name="ClickEvent">被按下的事件</param>
        /// <param name="Hotkey">快捷键</param>
        public BarLargeButtonItem AddComandButtonNew(string Caption, Bitmap image, ItemClickEventHandler ClickEvent, int Hotkey)
        {
            try
            {
                ChangeColor(image);
                Bitmap imageBitmap = new Bitmap(image);
                BarLargeButtonItem barLargeItem = new BarLargeButtonItem(barManager, Caption);
                barLargeItem.LargeGlyph = imageBitmap;//也可以设置 barLargeItem.LargeImageIndex,但是效果不是很好，可以试试                
                barLargeItem.Hint = Caption;
                barLargeItem.Tag = Caption;
                barLargeItem.Name = Caption;
                barLargeItem.Id = barManager.Items.Count + 1;
                bar.LinksPersistInfo.Add(new LinkPersistInfo(barLargeItem, true));
                barManager.Items.Add(barLargeItem);
                barLargeItem.ItemClick += ClickEvent;
                ToolStripAndEvent toolStripAndEvent = new ToolStripAndEvent()
                {
                    BeClickEvent = ClickEvent,
                    TargetButton = barLargeItem,
                };

                if (!HotKeyDict.ContainsKey(Hotkey))
                    HotKeyDict.Add(Hotkey, toolStripAndEvent);
                return barLargeItem;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return null;
            }
        }

        /// <summary>
        /// 修改透明色
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        Bitmap ChangeColor(Bitmap bmp)
        {
            if (bmp == null)
                return null;

            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    var pixel = bmp.GetPixel(i, j);
                    if (pixel.A == 0)
                        continue;
                    var newColor = Color.FromArgb(pixel.A, 255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                    bmp.SetPixel(i, j, newColor);
                }
            return bmp;
        }

        #endregion

        #endregion

        #region 消息处理

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="KeyMessage"></param>
        public override void HotKeyProc(PawayHotKeyMessage KeyMessage)
        {
            try
            {
                int iKeyMessage = (int)KeyMessage;
                if (!HotKeyDict.Keys.Contains(iKeyMessage)) return;

                ToolStripAndEvent ClickEvent = HotKeyDict[iKeyMessage];
                if (ClickEvent != null && ClickEvent.TargetButton.Enabled)
                {
                    ClickEvent.BeClickEvent(ClickEvent.TargetButton, null);
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region 窗体KeyPress

        void BaseButtonForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                ////拥有 控件BarCodeSendControl 时，屏蔽此父类的keypress事件
                foreach (Control FousControl in IgnoreKeyPressControls)
                {
                    if (FousControl.Focused || FousControl.ContainsFocus)
                    {
                        return;
                    }
                }

                if (AutoTabKeyPress && e.KeyChar == (int)Keys.Return)
                {
                    e.Handled = true;
                    SendKeys.Send("{TAB}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region 窗体KeyDown

        void BaseButtonForm_KeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.KeyValue;
            ToolStripAndEvent toolStripAndEvent = GetKeyEventHandler((PawayHotKeyMessage)key);
            if (toolStripAndEvent != null)
                toolStripAndEvent.BeClickEvent(toolStripAndEvent.TargetButton, null);
        }

        #endregion

        #region 取得注册键字典中对应的事件代理

        /// <summary>
        /// 取得注册键字典中对应的事件代理
        /// </summary>
        /// <param name="KeyMessage">按键名bu</param>
        /// <returns>如果是空那么就是没有找到</returns>
        private ToolStripAndEvent GetKeyEventHandler(PawayHotKeyMessage KeyMessage)
        {
            int iKeyMessage = (int)KeyMessage;
            if (!HotKeyDict.Keys.Contains(iKeyMessage)) return null;
            return HotKeyDict[iKeyMessage] as ToolStripAndEvent;

        }

        #endregion

        List<string> ignoreControlList = new List<string>();

        public void IgnoreControl(string ignoreControlName)
        {
            if (!ignoreControlList.Contains(ignoreControlName))
                ignoreControlList.Add(ignoreControlName);
        }

        public virtual void SetControlRead(Control parentControl, bool Read)     //对panel进行遍历的函数
        {
            foreach (Control children in parentControl.Controls)
            {
                if (ignoreControlList.Contains(children.Name))
                    continue;

                if (children is DevExpress.XtraEditors.DateEdit)
                {
                    (children as DevExpress.XtraEditors.DateEdit).Enabled = !Read;
                    (children as DevExpress.XtraEditors.DateEdit).Properties.ReadOnly = Read;
                }

                if (children is DevExpress.XtraEditors.TextEdit)
                {
                    //(children as DevExpress.XtraEditors.TextEdit).Enabled = !Read;
                    (children as DevExpress.XtraEditors.TextEdit).Properties.ReadOnly = Read;
                }

                if (children is DevExpress.XtraEditors.LookUpEdit)
                {
                    (children as DevExpress.XtraEditors.LookUpEdit).Enabled = !Read;
                    (children as DevExpress.XtraEditors.LookUpEdit).Properties.ReadOnly = Read;

                }

                if (children is TextBox)
                {
                    (children as TextBox).ReadOnly = Read;
                 //   (children as TextBox).Enabled = !Read;
                } 

                if (children is DevExpress.XtraGrid.GridControl)
                {
                    DevExpress.XtraGrid.GridControl g = children as DevExpress.XtraGrid.GridControl;
                    g.UseDisabledStatePainter = false;
                    DevExpress.XtraGrid.Views.Grid.GridView gv = g.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
                    if (gv != null)
                    {
                        gv.OptionsBehavior.Editable = !Read;
                    }
                }

                if (children is UserControl)
                    (children as UserControl).Enabled = !Read;

                if (children.HasChildren)
                    SetControlRead(children, Read);
            }
        }

        public void ClearItems()
        {
            barManager.Items.Clear();
            this.bar.LinksPersistInfo.Clear();
        }


        private SplashScreenManager _loadForm;
        /// <summary>
        /// 等待窗体管理对象
        /// </summary>
        protected SplashScreenManager LoadForm
        {
            get
            {
                if (_loadForm == null)
                {
                    this._loadForm = new SplashScreenManager(this, typeof(PubWaitForm), true, true);
                    this._loadForm.ClosingDelay = 0;
                }
                return _loadForm;
            }
        }
        /// <summary>
        /// 显示等待窗体
        /// </summary>
        public void BeginLoading()
        {
            bool flag = !this.LoadForm.IsSplashFormVisible;
            if (flag)
            {
                this.LoadForm.ShowWaitForm();
                Thread.Sleep(300);
            }
        }
        /// <summary>
        /// 关闭等待窗体
        /// </summary>
        public void CloseLoading()
        {
            bool isSplashFormVisible = this.LoadForm.IsSplashFormVisible;
            if (isSplashFormVisible)
            {
                this.LoadForm.CloseWaitForm();
            }
        }

        #region DesignMode


        protected new bool DesignMode
        {
            get
            {
                bool returnFlag = false;
#if DEBUG
                if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                    returnFlag = true;
                else if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper().Equals("DEVENV"))
                    returnFlag = true;
#endif
                return returnFlag;
            }
        }


        #endregion
    }
}
